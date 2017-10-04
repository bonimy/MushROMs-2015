using System;
using System.ComponentModel;
using System.Drawing;
using MushROMs.Controls;
using MushROMs.LunarCompress;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    partial class Palette
    {
        public override _IEditorData CreateCopy(Selection_ selection)
        {
            // Initialize the copy data.
            EditorData copy = new EditorData(selection);

            // Derefernce values
            int address = selection.StartAddress;
            int index0 = GetIndexFromAddress(address, null);
            unsafe
            {
                fixed (byte* _data = &this.Data[address % SNESColorSize])
                {
                    ushort* colors = (ushort*)_data;

                    int height = selection.Height;
                    int width = selection.Width;
                    int container = selection.ContainerWidth;
                    int length = this.MapLength;

                    // Write the color data.
                    for (int h = height; --h >= 0; )
                    {
                        int index = index0 + (h * container) + width - 1;
                        for (int w = width; --w >= 0; --index)
                            if (index >= 0 && index < length)
                                copy.Colors[h, w] = colors[index];
                    }
                }
            }

            return copy;
        }

        public override void Paste(_IEditorData data)
        {
            // Properly cast the copy data.
            EditorData copy = (EditorData)data;

            // Initialize the pasted data to the selection.
            EditorData paste = new EditorData(new Selection_(this.Selection.First, copy.Selection.Size));

            // Copy the source data.
            unsafe
            {
                fixed (ushort* src = copy.Colors)
                fixed (ushort* dest = paste.Colors)
                    for (int i = copy.Colors.Length; --i >= 0; )
                        dest[i] = src[i];
            }

            // Write the data.
            ModifyData(paste, true, false);
        }

        public override void DeleteSelection()
        {
            // Create a copy of the current selection.
            EditorData data = (EditorData)CreateCopy();

            unsafe
            {
                // Set all the colors to black within the selection.
                fixed (ushort* src = data.Colors)
                    for (int i = data.Colors.Length; --i >= 0; )
                        src[i] = 0;
            }

            // Modify the palette to show the selection has been blacked out.
            ModifyData(data, true, false);
        }

        public void EditPaletteColor(int address, ushort color)
        {
            // Get the index point of the address.
            IndexPoint ip = new IndexPoint(this.Zero);
            ip.Index = GetIndexFromAddress(address, null);

            // Don't go out of the memory range.
            if (ip.Index < 0 || ip.Index >= this.MapLength)
                throw new IndexOutOfRangeException(Resources.ErrorAddressOutOfRange);

            // Create the copy data for the new color.
            EditorData data = new EditorData(new Selection_(ip));
            data.Colors[0, 0] = color;

            // Modify the palette to show the new color.
            ModifyData(data, true, false);
        }

        public void Invert()
        {
            // Create a copy of the current selection.
            EditorData data = (EditorData)CreateCopy();

            unsafe
            {
                // Invert all the colors within the selection.
                fixed (ushort* src = data.Colors)
                    for (int i = data.Colors.Length; --i >= 0; )
                        src[i] ^= 0x7FFF;
            }
            // Modify the palette to show the selection with the inverted colors.
            ModifyData(data, true, false);
        }

        public void Colorize(_IEditorData data, bool colorize, int hue, int sat, int lum, int eff, bool preview)
        {
            // Properly cast the source data.
            EditorData source = (EditorData)data;

            // Create a copy of the current selection.
            EditorData copy = new EditorData(data.Selection);

            // Make sure the selections have the same size.
            if (data.Selection.TotalTiles != copy.Selection.TotalTiles)
                throw new ArgumentException(Resources.ErrorSelectionsMismatch);

            unsafe
            {
                // Colorize or modify all the colors.
                fixed (ushort* src = source.Colors)
                fixed (ushort* dest = copy.Colors)
                {
                    for (int i = source.Colors.Length; --i >= 0; )
                    {
                        ExpandedColor x = LC.SNESToSystemColor(src[i]);
                        if (colorize)
                            dest[i] = LC.SystemToSNESColor((Color)x.Colorize(hue, sat, lum, eff));
                        else
                            dest[i] = LC.SystemToSNESColor((Color)x.Modify(hue, sat, lum));
                    }
                }
            }

            // Modify the palette to show the selection has been colorized.
            ModifyData(copy, !preview, false);
        }

        public void Grayscale(_IEditorData data, int red, int green, int blue, bool preview)
        {
            // Properly cast the source data.
            EditorData source = (EditorData)data;

            // Create a copy of the current selection.
            EditorData copy = new EditorData(data.Selection);

            // Make sure the selections have the same size.
            if (data.Selection.TotalTiles != copy.Selection.TotalTiles)
                throw new ArgumentException(Resources.ErrorSelectionsMismatch);

            unsafe
            {
                // Grayscale the colors.
                fixed (ushort* src = source.Colors)
                fixed (ushort* dest = copy.Colors)
                {
                    for (int i = source.Colors.Length; --i >= 0; )
                    {
                        ExpandedColor x = LC.SNESToSystemColor(src[i]);
                        dest[i] = LC.SystemToSNESColor((Color)x.Grayscale(red, green, blue));
                    }
                }
            }

            // Modify the palette to show the selection has been grayscaled.
            ModifyData(copy, !preview, false);
        }

        public void Blend(_IEditorData data, ExpandedColor color, BlendMode mode, bool preview)
        {
            // Properly cast the source data.
            EditorData source = (EditorData)data;

            // Create a copy of the current selection.
            EditorData copy = new EditorData(data.Selection);

            // Make sure the selections have the same size.
            if (data.Selection.TotalTiles != copy.Selection.TotalTiles)
                throw new ArgumentException(Resources.ErrorSelectionsMismatch);

            unsafe
            {
                // Blend the colors.
                fixed (ushort* src = source.Colors)
                fixed (ushort* dest = copy.Colors)
                {
                    for (int i = source.Colors.Length; --i >= 0; )
                    {
                        ExpandedColor xColor = LC.SNESToSystemColor(src[i]);
                        dest[i] = LC.SystemToSNESColor((Color)xColor.Blend(color, mode));
                    }
                }
            }

            // Modify the palette to show the selection has been blended.
            ModifyData(copy, !preview, false);
        }

        public void Gradient(GradientStyle style)
        {
            // Create a copy of the current selection.
            EditorData copy = (EditorData)CreateCopy();

            // The last index of either the row or column.
            int last;

            // Create the gradient.
            switch (style)
            {
                case GradientStyle.Horizontal:

                    // The last index of the row.
                    last = copy.Selection.Width - 1;

                    // Create the gradient across evere row.
                    for (int h = copy.Selection.Height; --h >= 0; )
                    {
                        // Get the first color of the row.
                        Color c1 = LC.SNESToSystemColor(copy.Colors[h, 0]);

                        // Get the last color of the row.
                        Color c2 = LC.SNESToSystemColor(copy.Colors[h, last]);

                        // Create a linear gradient across all the colors inbetween.
                        for (int w = last; --w >= 1; )
                            copy.Colors[h, w] = LC.PCtoSNESRGB(
                                (uint)((((c1.R * (last - w)) + (c2.R * w)) / last) << 0x10 |
                                       (((c1.G * (last - w)) + (c2.G * w)) / last) << 8 |
                                       (((c1.B * (last - w)) + (c2.B * w)) / last)));
                    }
                    break;
                case GradientStyle.Vertical:

                    // The last index of the column.
                    last = copy.Selection.Height - 1;

                    // Create the gradient across every column.
                    for (int w = copy.Selection.Width; --w >= 0; )
                    {
                        // Get the first color of the column.
                        Color c1 = LC.SNESToSystemColor(copy.Colors[0, w]);

                        // Get the last color of the column.
                        Color c2 = LC.SNESToSystemColor(copy.Colors[last, w]);

                        // Create a linear gradient across all the colors inbetween.
                        for (int h = last; --h >= 1; )
                            copy.Colors[h, w] = LC.PCtoSNESRGB(
                                (uint)((((c1.R * (last - h)) + (c2.R * h)) / last) << 0x10 |
                                       (((c1.G * (last - h)) + (c2.G * h)) / last) << 8 |
                                       (((c1.B * (last - h)) + (c2.B * h)) / last)));
                    }
                    break;
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorGradientStyle);
            }

            // Modify the palette to show the selection has had the gradient applied.
            ModifyData(copy, true, false);
        }

        protected override void WriteCopyData(_IEditorData data, int startAddress)
        {
            // Properly cast the edit data.
            EditorData copy = (EditorData)data;

            unsafe
            {
                // Derefernce values
                fixed (byte* _data = &this.Data[startAddress % SNESColorSize])
                {
                    ushort* colors = (ushort*)_data;
                    int index0 = GetIndexFromAddress(startAddress, null);

                    int height = data.Selection.Height;
                    int width = data.Selection.Width;
                    int container = data.Selection.ContainerWidth;
                    int length = this.MapLength;

                    // Write the color data.
                    for (int h = height; --h >= 0; )
                    {
                        int index = GetIndexFromAddress(startAddress, null) + (h * container) + width - 1;
                        for (int w = width; --w >= 0; --index)
                            if (index >= 0 && index < length)
                                colors[index] = copy.Colors[h, w];
                    }
                }
            }
        }

        protected class EditorData : _IEditorData
        {
            private ushort[,] colors;
            private Selection_ selection;

            public ushort[,] Colors
            {
                get { return this.colors; }
            }

            public Selection_ Selection
            {
                get { return this.selection; }
            }

            protected internal EditorData(Selection_ selection)
            {
                this.colors = new ushort[selection.Height, selection.Width];
                this.selection = selection.Copy();
            }
        }
    }
}
