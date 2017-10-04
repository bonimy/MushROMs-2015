using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs
{
    partial class Editor
    {
        private const int FallbackTileWidth = 8;
        private const int FallbackTileHeight = FallbackTileWidth;

        private const int FallbackZoomWidth = 1;
        private const int FallbackZoomHeight = FallbackZoomWidth;

        private int tileW;
        private int tileH;

        private int zoomW;
        private int zoomH;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TileWidth
        {
            get { return this.tileW; }
            set { SetTileSize(value, this.tileH); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TileHeight
        {
            get { return this.tileH; }
            set { SetTileSize(this.tileW, value); }
        }
        [Category("Appearance")]
        [Description("The vertical and horizontal size of a single tile.")]
        public Size TileSize
        {
            get { return new Size(this.TileWidth, this.TileHeight); }
            set { SetTileSize(value.Width, value.Height); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ZoomWidth
        {
            get { return this.zoomW; }
            set { SetZoomSize(value, this.zoomH); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ZoomHeight
        {
            get { return this.zoomH; }
            set { SetZoomSize(this.zoomW, value); }
        }
        [Category("Appearance")]
        [Description("The vertical and horizontal zoom factor.")]
        public Size ZoomSize
        {
            get { return new Size(this.ZoomWidth, this.ZoomHeight); }
            set { SetZoomSize(value.Width, value.Height); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellWidth
        {
            get { return this.TileWidth * this.ZoomWidth; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellHeight
        {
            get { return this.TileHeight * this.ZoomHeight; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size CellSize
        {
            get { return new Size(this.CellWidth, this.CellHeight); }
        }

        [Category("Appearance")]
        [Description("Occurs when the tile size of the editor changes.")]
        public event EventHandler TileSizeChanged;

        [Category("Appearance")]
        [Description("Occurs when the zoom size of the editor changes.")]
        public event EventHandler ZoomSizeChanged;

        /// </summary>
        [Category("Appearance")]
        [Description("Occurs when the cell size of the editor changes.")]
        public event EventHandler CellSizeChanged;

        protected virtual void SetTileSize(int tileW, int tileH)
        {
            // Make sure the tile size is valid.
            if (tileW <= 0)
                throw new ArgumentOutOfRangeException("tileW");
            if (tileH <= 0)
                throw new ArgumentOutOfRangeException("tileH");

            // Avoid redundant setting.
            if (this.tileW == tileW && this.tileH == tileH)
                return;

            // Set the tile size.
            this.tileW = tileW;
            this.tileH = tileH;

            OnTileSizeChanged(EventArgs.Empty);
        }

        protected virtual void OnTileSizeChanged(EventArgs e)
        {
            if (TileSizeChanged != null)
                TileSizeChanged(this, e);

            OnCellSizeChanged(EventArgs.Empty);
        }

        protected virtual void SetZoomSize(int zoomW, int zoomH)
        {
            // Make sure the zoom size is valid.
            if (zoomW <= 0)
                throw new ArgumentOutOfRangeException("zoomW");
            if (zoomH <= 0)
                throw new ArgumentOutOfRangeException("zoomH");

            // Avoid redundant setting.
            if (this.zoomW == zoomW && this.zoomH == zoomH)
                return;

            //Set the zoom size.
            this.zoomW = zoomW;
            this.zoomH = zoomH;

            OnZoomSizeChanged(EventArgs.Empty);
        }

        protected virtual void OnZoomSizeChanged(EventArgs e)
        {
            if (ZoomSizeChanged != null)
                ZoomSizeChanged(this, e);

            OnCellSizeChanged(EventArgs.Empty);
        }

        protected virtual void OnCellSizeChanged(EventArgs e)
        {
            if (CellSizeChanged != null)
                CellSizeChanged(this, e);

            OnVisibleSizeChanged(EventArgs.Empty);
        }
    }
}