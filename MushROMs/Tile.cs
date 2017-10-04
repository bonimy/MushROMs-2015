using System;
using System.Drawing;

namespace MushROMs
{
    public struct Tile
    {
        private Size tileSize;
        private Size zoomSize;

        public Size TileSize
        {
            get { return this.tileSize; }
        }
        public int TileWidth
        {
            get { return this.TileSize.Width; }
        }
        public int TileHeight
        {
            get { return this.TileSize.Height; }
        }

        public Size ZoomSize
        {
            get { return this.zoomSize; }
        }
        public int ZoomWidth
        {
            get { return this.ZoomSize.Width; }
        }
        public int ZoomHeight
        {
            get { return this.ZoomSize.Height; }
        }

        public Size CellSize
        {
            get { return new Size(this.CellWidth, this.CellHeight); }
        }
        public int CellWidth
        {
            get { return this.TileWidth * this.ZoomWidth; }
        }
        public int CellHeight
        {
            get { return this.TileHeight * this.ZoomHeight; }
        }

        public Tile(Size tileSize, Size zoomSize)
        {
            if (tileSize.Width <= 0 || tileSize.Height <= 0)
                throw new ArgumentOutOfRangeException("tileSize");
            if (zoomSize.Width <= 0 || zoomSize.Height <= 0)
                throw new ArgumentOutOfRangeException("zoomSize");

            this.tileSize = tileSize;
            this.zoomSize = zoomSize;
        }
    }
}