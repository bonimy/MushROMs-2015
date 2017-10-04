using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MushROMs.Controls
{
    partial class EditorControl
    {
        private GraphicsPath boundary;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GraphicsPath SelectionBoundary
        {
            get { return this.boundary; }
        }

        protected virtual void SetBoundary()
        {
            if (this.boundary == null)
                return;

            // Reset the original boundary
            this.boundary.Reset();

            // Make sure there are actually map tiles.
            if (this.Editor == null || this.Editor.MapLength == 0)
                return;

            // Dereference the current editor.
            Editor _editor = this.Editor;

            // Dereference common variables
            int viewW = _editor.ViewSize.Width;
            int viewH = _editor.ViewSize.Height;
            int cellW = _editor.CellSize.Width;
            int cellH = _editor.CellSize.Height;
            int mapW = _editor.MapSize.Width;
            int mapH = _editor.MapSize.Height;
            int mapL = _editor.MapLength;
            int zeroI = _editor.Zero.Index;
            int zeroX = _editor.Zero.X;
            bool linear = _editor.MapIsLinear;

            // vertical index of cell
            int h = Math.Min(viewH, mapH);

            // Account for linear remainders
            if (_editor.MapIsLinear && viewH > mapH && _editor.MapRemainder != 0)
                h++;

            // y-coordinate of cell
            int y = h * cellH;

            unsafe
            {
                fixed (bool* tiles = _editor.SelectedTiles)
                {
                    // Loop vertically across all cell rows
                    while (--h >= 0)
                    {
                        // Update y-coordinate of cell
                        y -= cellH;

                        // Initialize the subloop variables
                        int w = Math.Min(viewW, mapW);  // horizontal index of cell
                        int x = w * cellW;              // x-coordinate of cell
                        int i = zeroI + (h * mapW) + w;  // Actual index of the cell in the selected cells pointer

                        // boundary condition across single rows
                        int j = w;

                        // Loop horizontally across specific row
                        while (--w >= 0)
                        {
                            // Update x-coordinate of cell
                            x -= cellW;
                            i--;
                            j--;

                            // Check that this cell is selected
                            if (i >= 0 && i < mapL && tiles[i])
                            {
                                // Check which adjacent cells are also selected
                                bool top = i - mapW >= 0 && tiles[i - mapW];
                                bool left = i - 1 >= 0 && tiles[i - 1];
                                bool bottom = i + mapW < mapL && tiles[i + mapW];
                                bool right = i + 1 < mapL && tiles[i + 1];

                                if (linear)
                                {
                                    left &= j - 1 >= 0;
                                    right &= j + 1 < mapW;
                                }
                                else
                                {
                                    left &= j + zeroX - 1 >= 0;
                                    right &= j + zeroX + 1 < mapW;
                                }

                                // Boundary points aorund this cell
                                Point topLeft = new Point(x, y);
                                Point topRight = new Point(x + cellW - 1, y);
                                Point bottomLeft = new Point(x, y + cellH - 1);
                                Point bottomRight = new Point(x + cellW - 1, y + cellH - 1);

                                // Draw boundaries when appropriate
                                if (!left)
                                {
                                    boundary.StartFigure();
                                    boundary.AddLine(bottomLeft, topLeft);
                                }
                                if (!top)
                                {
                                    boundary.StartFigure();
                                    boundary.AddLine(topLeft, topRight);
                                }
                                if (!right)
                                {
                                    boundary.StartFigure();
                                    boundary.AddLine(topRight, bottomRight);
                                }
                                if (!bottom)
                                {
                                    boundary.StartFigure();
                                    boundary.AddLine(bottomRight, bottomLeft);
                                }
                            }
                        }
                    }
                }
            }

            // Redraw the control.
            this.Invalidate();
        }
    }
}