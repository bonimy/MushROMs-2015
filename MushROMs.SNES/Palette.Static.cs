using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    partial class Palette
    {
        private const string ExtensionTPL = ".tpl";
        private const string ExtensionMW3 = ".mw3";
        private const string ExtensionPAL = ".pal";
        private const string FallbackNewFileExtension = ExtensionTPL;

        private static readonly byte[] TPLHeader = {
            (byte)'T',
            (byte)'P',
            (byte)'L',
            (byte)2 };

        public static PaletteFileFormat GetFileFormat(string path)
        {
            // Check for empty strings first.
            if (path == string.Empty)
                return PaletteFileFormat.None;

            // Get the extension
            string ext = Path.GetExtension(path).ToLower();

            // Check all possible palette file extensions.
            if (ext == ExtensionTPL)
                return PaletteFileFormat.TPL;
            if (ext == ExtensionMW3)
                return PaletteFileFormat.MW3;
            if (ext == ExtensionPAL)
                return PaletteFileFormat.PAL;
            if (ROM.IsROMExt(ext))
                return PaletteFileFormat.SNES;
            if (S9X.IsS9XExt(ext))
                return PaletteFileFormat.S9X;
            if (ZST.IsZSTFile(ext))
                return PaletteFileFormat.ZST;

            // Everything else will be read as a raw binary file.
            return PaletteFileFormat.BIN;
        }

        public static string GetExtension(PaletteFileFormat format)
        {
            switch (format)
            {
                case PaletteFileFormat.TPL:
                    return ExtensionTPL;
                case PaletteFileFormat.MW3:
                    return ExtensionMW3;
                case PaletteFileFormat.PAL:
                    return ExtensionPAL;
                case PaletteFileFormat.BIN:
                    return ROM.ExtensionBIN;
                case PaletteFileFormat.SNES:
                    return ROM.ExtensionSMC;
                case PaletteFileFormat.ZST:
                    return ZST.ExtensionZST;
                case PaletteFileFormat.S9X:
                    return S9X.Extension000;
                case PaletteFileFormat.None:
                    return FallbackNewFileExtension;
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorPaletteFileFormatUnknown);
            }
        }

        public static bool IsValidSize(PaletteFileFormat format, int size)
        {
            switch (format)
            {
                case PaletteFileFormat.TPL:
                    return size >= TPLHeader.Length && (size - TPLHeader.Length) % SNESColorSize == 0;
                case PaletteFileFormat.PAL:
                    return size > 0 && size % PALColorSize == 0;
                case PaletteFileFormat.MW3:
                    return size >= SNESColorSize && (size - SNESColorSize) % SNESColorSize == 0;
                case PaletteFileFormat.BIN:
                case PaletteFileFormat.None:
                    return size > 0 && size % SNESColorSize == 0;
                case PaletteFileFormat.SNES:
                    return size > 0 && (size & ~ROM.HeaderSize) % ROM.LoBankSize == 0;
                case PaletteFileFormat.ZST:
                    return size >= ZST.PaletteOffset + (SNESPaletteSize * SNESColorSize);
                case PaletteFileFormat.S9X:
                    return size >= S9X.S9XSize151 || size >= S9X.S9XSize153;
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorPaletteFileFormatUnknown);
            }
        }

        public static int GetNumColors(PaletteFileFormat format, int size)
        {
            switch (format)
            {
                case PaletteFileFormat.TPL:
                    return (size - TPLHeader.Length) / SNESColorSize;
                case PaletteFileFormat.PAL:
                    return size / PALColorSize;
                case PaletteFileFormat.MW3:
                    return (size / SNESColorSize) - 1;
                case PaletteFileFormat.BIN:
                case PaletteFileFormat.None:
                    return size / SNESColorSize;
                case PaletteFileFormat.SNES:
                    return (size & ~ROM.HeaderSize) / SNESColorSize;
                case PaletteFileFormat.S9X:
                case PaletteFileFormat.ZST:
                    return SNESPaletteSize;
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorPaletteFileFormatUnknown);
            }
        }

        public static int GetFormatSize(PaletteFileFormat format, int numColors)
        {
            switch (format)
            {
                case PaletteFileFormat.TPL:
                    return numColors * SNESColorSize + TPLHeader.Length;
                case PaletteFileFormat.PAL:
                    return numColors * PALColorSize;
                case PaletteFileFormat.MW3:
                    return (numColors + 1) * SNESColorSize;
                case PaletteFileFormat.BIN:
                case PaletteFileFormat.SNES:
                case PaletteFileFormat.None:
                    return numColors * SNESColorSize;
                case PaletteFileFormat.ZST:
                case PaletteFileFormat.S9X:
                    throw new InvalidEnumArgumentException(Resources.ErrorSaveStateSize);
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorPaletteFileFormatUnknown);
            }
        }

        public static string CreateFilter(PaletteFileFormat format)
        {
            List<string> names = new List<string>();
            List<string[]> extensions = new List<string[]>();
            List<string> ext = new List<string>();

            names.Add(Resources.FilterOptionsAll);

            ext.AddRange(new string[] {
                ExtensionTPL,
                ExtensionPAL,
                ExtensionMW3,
                ROM.ExtensionBIN });

            if (format == PaletteFileFormat.SNES || format == PaletteFileFormat.None)
                ext.AddRange(ROM.CreateFilter());

            if (format == PaletteFileFormat.S9X || format == PaletteFileFormat.None)
                ext.AddRange(S9X.CreateFilter());

            if (format == PaletteFileFormat.ZST || format == PaletteFileFormat.None)
                ext.AddRange(ZST.CreateFilter());

            extensions.Add(ext.ToArray());

            if (format == PaletteFileFormat.SNES || format == PaletteFileFormat.None)
            {
                names.Add(Resources.FilterOptionsSNS);
                extensions.Add(ROM.CreateFilter());
            }

            names.Add(Resources.FilterOptionsTPL);
            extensions.Add(new string[] { Palette.ExtensionTPL });

            names.Add(Resources.FilterOptionsPAL);
            extensions.Add(new string[] { Palette.ExtensionPAL });

            names.Add(Resources.FilterOptionsMW3);
            extensions.Add(new string[] { Palette.ExtensionMW3 });

            if (format == PaletteFileFormat.S9X || format == PaletteFileFormat.None)
            {
                names.Add(Resources.FilterOptionsS9X);
                extensions.Add(S9X.CreateFilter());
            }

            if (format == PaletteFileFormat.ZST || format == PaletteFileFormat.None)
            {
                names.Add(Resources.FilterOptionsZST);
                extensions.Add(ZST.CreateFilter());
            }

            names.Add(Resources.FilterOptionsBIN);
            extensions.Add(new string[] { ROM.ExtensionBIN });

            return IOHelper.CreateFilter(names, extensions);
        }
    }
}