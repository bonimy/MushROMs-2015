using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs
{
    partial class Editor
    {
        private bool linear;
        private int mapL;
        private int mapW;
        private int mapH;
        private int mapR;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool MapIsLinear
        {
            get { return this.linear; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MapLength
        {
            get { return this.mapL; }
            set { SetMapLength(value); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MapWidth
        {
            get { return this.mapW; }
            set { SetMapSize(value, this.MapHeight); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MapHeight
        {
            get { return this.mapH; }
            set { SetMapSize(this.MapWidth, value); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size MapSize
        {
            get { return new Size(this.MapWidth, this.MapHeight); }
            set { SetMapSize(value.Width, value.Height); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int MapRemainder
        {
            get { return this.mapR; }
        }

        [Category("Property Changed")]
        [Description("Occurs when the map size or length changes.")]
        public event EventHandler MapReset;

        protected virtual void SetMapSize(int mapW, int mapH)
        {
            // Make sure the map parameters are in a valid range
            if (mapW <= 0)
                throw new ArgumentOutOfRangeException("mapW");
            if (mapH <= 0)
                throw new ArgumentOutOfRangeException("mapH");

            // Avoid redundant setting.
            if (!this.linear && this.mapW == mapW && this.mapH == mapH)
                return;

            // The map data is rectangular
            this.linear = false;

            // Set the map parameters
            this.mapW = mapW;
            this.mapH = mapH;
            this.mapL = mapW * mapH;
            this.mapR = 0;

            // The map has been reset
            OnMapReset(EventArgs.Empty);
        }

        protected virtual void SetMapLength(int mapL)
        {
            // Make sure the map length is valid.
            if (mapL <= 0)
                throw new ArgumentOutOfRangeException("mapL");

            // Avoid redundant setting.
            if (this.linear && this.mapL == mapL)
                return;

            // The map data is linear.
            this.linear = true;

            // Set the map parameters (the map width is now linked to the view width)
            this.mapW = this.viewW;
            this.mapH = mapL / this.viewW;
            this.mapL = mapL;
            this.mapR = mapL % this.viewW;

            // The map has been reset
            OnMapReset(EventArgs.Empty);
        }

        protected virtual void OnMapReset(EventArgs e)
        {
            // Reset the selection.
            this.selection = new Selection_(new IndexPoint(this.Zero));

            // Reset the selected tiles.
            this.selectedTiles = new bool[this.MapLength];
            SetSelectedTiles();

            // Reset zero address and active index
            this.Zero.Address = 0;
            this.Active.Index = 0;

            if (MapReset != null)
                MapReset(this, e);

            OnSizeChanged(e);
        }
    }
}