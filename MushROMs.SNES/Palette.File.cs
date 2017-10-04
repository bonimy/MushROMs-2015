using System;
using System.ComponentModel;
using System.IO;
using MushROMs.LunarCompress;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    partial class Palette
    {
        public event EventHandler FileFormatChanged;

        private static int UntitledNumber;

        private PaletteFileFormat fileFormat;
        private bool compressed;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PaletteFileFormat FileFormat
        {
            get { return this.fileFormat; }
            protected set { this.fileFormat = value; OnFileFormatChanged(EventArgs.Empty); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Compressed
        {
            get { return this.compressed; }
        }

        protected override string CreateUntitledFileName()
        {
            return "Untitled" + (++UntitledNumber).ToString() + FallbackNewFileExtension;
        }

        public void Initialize(int numColors)
        {
            // File is being programmatically created.
            this.FileDataType = FileDataType.ProgramCreated;
            this.Untitled = CreateUntitledFileName();

            // Set the number of colors (the map length).
            this.MapLength = numColors;
            ResetData(this.MapLength * SNESColorSize);

            unsafe
            {
                // Give all the colors a default value.
                fixed (byte* _data = this.Data)
                {
                    ushort* colors = (ushort*)_data;
                    for (int i = numColors; --i >= 0; )
                        colors[i] = 0x5555;
                }
            }

            // This has no predetermined file format.
            this.FileFormat = PaletteFileFormat.None;

            OnDataModified(EventArgs.Empty);
        }

        public void Initialize(_IEditorData data)
        {
            // File is being programmatically created.
            this.FileDataType = FileDataType.ProgramCreated;
            this.Untitled = CreateUntitledFileName();

            // Properly cast the color data.
            EditorData copy = (EditorData)data;

            // Set the number of colors (the map length).
            this.MapLength = copy.Colors.Length;
            ResetData(this.MapLength * SNESColorSize);

            unsafe
            {
                // Copy all the colors to the new palette.
                fixed (byte* _data = this.data)
                {
                    ushort* colors = (ushort*)_data;
                    fixed (ushort* src = copy.Colors)
                        for (int i = this.MapLength; --i >= 0; )
                            colors[i] = src[i];
                }
            }

            // This has no predetermined file format.
            this.FileFormat = PaletteFileFormat.None;
        }

        public void Initialize(byte[] data, PaletteFileFormat format)
        {
            // File is being programmatically created.
            this.FileDataType = FileDataType.ProgramCreated;
            this.Untitled = CreateUntitledFileName();

            // Set the file data to the source data.
            this.FileData = data;

            // Determine whethere the data was originally compressed.
            if (this.compressed = format == PaletteFileFormat.S9X && GZip.IsCompressed(this.FileData))
                this.FileData = GZip.Decompress(this.FileData);

            unsafe
            {
                // Load the palette data.
                fixed (byte* ptr = this.FileData)
                    LoadPaletteData(ptr, this.FileData.Length, format);
            }
        }

        protected override void OnFileOpened(EventArgs e)
        {
            // File has an associated path.
            this.FileDataType = FileDataType.ProgramCreated;

            // Get the file fromat from the path.
            PaletteFileFormat format = Palette.GetFileFormat(this.FilePath);

            // Determine whethere the data was originally compressed.
            if (this.compressed = format == PaletteFileFormat.S9X && GZip.IsCompressed(this.FileData))
                this.FileData = GZip.Decompress(this.FileData);

            unsafe
            {
                // Load the palette data.
                fixed (byte* ptr = this.FileData)
                    LoadPaletteData(ptr, this.FileData.Length, format);
            }

            base.OnFileOpened(e);
        }

        private unsafe void LoadPaletteData(byte* data, int size, PaletteFileFormat format)
        {
            // Make sure the data actually exists.
            if ((IntPtr)data == IntPtr.Zero)
                throw new ArgumentNullException("data");

            // Make sure the size of the data is valid for the given format.
            if (!IsValidSize(format, size))
                throw new ArgumentException(Resources.ErrorInvalidFileDataSize);

            // Set the number of colors (the map length).
            this.MapLength = GetNumColors(format, size);

            // Properly cast the data pointers.
            fixed (byte* _data = this.Data)
            {
                ushort* src = (ushort*)data;
                ushort* dest = (ushort*)_data;

                // Load palette data depending on the format.
                switch (format)
                {
                    case PaletteFileFormat.TPL:    // Tile Layer Pro palettes

                        //Check to make sure the header is correct.
                        for (int i = TPLHeader.Length; --i >= 0; )
                            if (data[i] != TPLHeader[i])
                                throw new ArgumentException(Resources.ErrorTPLFormat);

                        // Offset the source pointer past the header.
                        src = (ushort*)(data += TPLHeader.Length);
                        break;

                    case PaletteFileFormat.PAL:    // 24-bit RGB color data

                        // Set every color
                        for (int i = this.MapLength, x = this.MapLength * PALColorSize; --i >= 0; )
                        {
                            uint color = (uint)data[--x];
                            color |= (uint)data[--x] << 8;
                            color |= (uint)data[--x] << 0x10;
                            dest[i] = LC.PCtoSNESRGB(color);
                        }
                        goto end;

                    case PaletteFileFormat.MW3:    // Lunar Magic palette data

                        // Set the back color.
                        this.backColor = src[this.MapLength];
                        break;

                    case PaletteFileFormat.BIN:    // Raw binary data

                        // Do nothing. Treat as direct translation to palette data.
                        break;

                    case PaletteFileFormat.SNES:   // SNES ROM files

                        // Ignore header data if it exists.
                        src = (ushort*)(data += size & ROM.LoBankSize);
                        break;

                    case PaletteFileFormat.ZST:    // ZSNES save state data

                        // Offset the source pointer to the ZSNES palette offset.
                        src = (ushort*)(data += ZST.PaletteOffset);
                        break;

                    case PaletteFileFormat.S9X:    // SNES9x save state

                        // Get the version of the SNES9x save state.
                        S9X.Version version = S9X.GetVersion((IntPtr)data);

                        // Make sure we actually got a valid version.
                        if (version == S9X.Version.None)
                            throw new ArgumentException(Resources.ErrorS9XVersion);

                        // Get the save state's internal path's string length.
                        int pathLen = S9X.GetPathLength((IntPtr)data);

                        // Make sure the SNES9x save state is valid.
                        if (size != S9X.GetSize(version) + pathLen)
                            throw new ArgumentException(Resources.ErrorS9XFormat);

                        // Update the data to the palette offset.
                        data += S9X.GetPaletteOffset(version) + pathLen;

                        // Get the color data.
                        for (int i = this.MapLength, x = this.Data.Length; --i >= 0; )
                            dest[i] = (ushort)(data[--x] | (data[--x] << 8));
                        goto end;

                    default:
                        throw new InvalidEnumArgumentException("format", (int)format, typeof(PaletteFileFormat));
                }

                // Copy all the color data for given file formats.
                for (int i = this.MapLength; --i >= 0; )
                    dest[i] = src[i];
            }

        end:
            // Set the file format.
            this.FileFormat = format;
        }

        protected override void OnFileSaving(CancelEventArgs e)
        {
            // File now has an associated path.
            this.FileDataType = FileDataType.FromFile;

            // Get the new file format.
            PaletteFileFormat format = GetFileFormat(this.FilePath);

            // The new file data.
            byte[] data;

            // Determine how the file data will be initialized.
            switch (format)
            {
                case PaletteFileFormat.S9X:    // Save states
                case PaletteFileFormat.ZST:

                    if (File.Exists(this.FilePath))         // Get the file data from the new path
                        data = File.ReadAllBytes(this.FilePath);
                    else if (this.fileFormat == format)     // Use the source file data.
                        data = this.FileData;
                    else
                        throw new ArgumentException(Resources.ErrorSaveStateFile);
                    break;

                default:    // Create the new file data for any other format.
                    data = new byte[GetFormatSize(format, this.MapLength)];
                    break;
            }

            // Decompress SNES9x save state data.
            if (format == PaletteFileFormat.S9X && GZip.IsCompressed(data))
                data = GZip.Decompress(data);

            unsafe
            {
                // Save the palette data.
                fixed (byte* ptr = data)
                    SavePaletteData(ptr, data.Length, format);
            }

            // Compress SNES9x save state data.
            if (format == PaletteFileFormat.S9X && this.compressed)
                data = GZip.Compress(data);

            // Set the file data.
            this.FileData = data;

            base.OnFileSaving(e);
        }

        private unsafe void SavePaletteData(byte* data, int size, PaletteFileFormat format)
        {
            // Make sure the data actually exists.
            if ((IntPtr)data == IntPtr.Zero)
                throw new ArgumentNullException("data");

            // Make sure the size of the data is valid for the given format.
            if (!IsValidSize(format, size))
                throw new ArgumentException(Resources.ErrorInvalidFileDataSize);

            // Properly cast the data pointers.
            fixed (byte* _data = this.Data)
            {
                ushort* src = (ushort*)_data;
                ushort* dest = (ushort*)data;

                // Save paltte data depending on the format.
                switch (format)
                {
                    case PaletteFileFormat.TPL:    // Tile Layer Pro palettes

                        //Add the file header.
                        for (int i = TPLHeader.Length; --i >= 0; )
                            data[i] = TPLHeader[i];

                        // Offset the destination pointer past the header.
                        dest = (ushort*)(data += TPLHeader.Length);
                        break;

                    case PaletteFileFormat.PAL:    // 24-bit RGB color data

                        // Set every color.
                        for (int i = this.MapLength, x = this.MapLength * PALColorSize; --i >= 0; )
                        {
                            uint color = LC.SNEStoPCRGB(src[i]);
                            data[--x] = (byte)color;
                            data[--x] = (byte)(color >> 8);
                            data[--x] = (byte)(color >> 0x10);
                        }
                        goto end;

                    case PaletteFileFormat.MW3:    // Lunar Magic palette data

                        // Set the back color.
                        dest[this.MapLength] = this.backColor;
                        break;

                    case PaletteFileFormat.SNES:   // SNES ROM files
                        // Ignore header data if it exists.
                        dest = (ushort*)(data += size % ROM.HeaderSize);
                        break;

                    case PaletteFileFormat.BIN:    // Raw binary data.

                        // Do nothing. Treat as direct translation to palette data.
                        break;

                    case PaletteFileFormat.ZST:    //ZSNES save state data

                        // Offset the destination pointer to the ZSNES palette offset.
                        dest = (ushort*)(data += ZST.PaletteOffset);
                        break;

                    case PaletteFileFormat.S9X:    // SNES9x save state

                        // Get the version of the SNES9x save state.
                        S9X.Version version = S9X.GetVersion((IntPtr)data);

                        // Make sure we actually got a valid version.
                        if (version == S9X.Version.None)
                            throw new ArgumentException(Resources.ErrorS9XVersion);

                        // Get the save state's internal path's string length.
                        int pathLen = S9X.GetPathLength((IntPtr)data);

                        // Make sure the SNES9x save state is valid.
                        if (size != S9X.GetSize(version) + pathLen)
                            throw new ArgumentException(Resources.ErrorS9XFormat);

                        // Update the data to the palette offset.
                        data += S9X.GetPaletteOffset(version) + pathLen;

                        // Set every color
                        for (int i = this.MapLength, x = this.Data.Length; --i >= 0; )
                        {
                            ushort color = src[i];
                            data[--x] = (byte)color;
                            data[--x] = (byte)(color >> 8);
                        }
                        goto end;

                    default:
                        throw new InvalidEnumArgumentException(Resources.ErrorPaletteFileFormatUnknown);
                }

                // Copy all the color data for the given file formats.
                for (int i = this.MapLength; --i >= 0; )
                    dest[i] = src[i];
            }

        end:
            // Set the file format.
            this.FileFormat = format;
        }

        protected virtual void OnFileFormatChanged(EventArgs e)
        {
            if (FileFormatChanged != null)
                FileFormatChanged(this, e);
        }
    }
}