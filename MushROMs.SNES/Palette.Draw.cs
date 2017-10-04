using System;
using MushROMs.LunarCompress;

namespace MushROMs.SNES
{
    partial class Palette
    {
        protected override void OnWritePixels(PixelEventArgs e)
        {
            if (this.TotalVisibleTiles > 0)
            {
                if (e == null)
                    throw new ArgumentNullException("e");

                unsafe
                {
                    fixed (byte* _data = &this.Data[this.Zero.Address])
                    {
                        DrawPalette(
                            e.Scan0,
                            (ushort*)_data,
                            this.TotalVisibleTiles,
                            this.ViewWidth,
                            this.ViewHeight,
                            this.CellWidth,
                            this.CellHeight,
                            this.VisibleWidth);
                    }
                }
            }

            base.OnWritePixels(e);
        }

        public static unsafe void DrawPalette(IntPtr scan0, ushort* colors, int numColors, int viewW, int viewH, int cellW, int cellH, int width)
        {
            /* SPECIALIZED FUNCTION: Taking speed over generality, we specialize
             * this function for 1x1 tile sizes. Use zoom to change the cell size.
             * */

            // The Bitmap pixel data
            uint* dest = (uint*)scan0;

            // The current color to draw
            uint color = LC.SNEStoPCRGB(*colors);

            // Special offseters for fast-speed drawing.
            int row = width * cellH - width;    // Offset to next row
            int cell = width * cellH - cellW;    // Offset to next cell

            // Draw the Palette data
            for (int y = viewH; --y >= 0; dest += row, colors += viewW)
                for (int x = 0; x < viewW && --numColors >= 0; ++x, dest -= cell, color = LC.SNEStoPCRGB(colors[x]))
                    for (int i = cellH; --i >= 0; dest += width)
                        for (int j = cellW; --j >= 0; )
                            dest[j] = color;
        }
    }
}