using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs
{
    partial class Editor
    {
        private const int FallbackViewWidth = 0x10;
        private const int FallbackViewHeight = 8;

        private int viewW;
        private int viewH;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ViewWidth
        {
            get { return this.viewW; }
            set { SetViewSize(value, this.viewH); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ViewHeight
        {
            get { return this.viewH; }
            set { SetViewSize(this.viewW, value); }
        }
        [Category("Appearance")]
        [Description("The number of columns and rows of the view region.")]
        public Size ViewSize
        {
            get { return new Size(this.ViewWidth, this.ViewHeight); }
            set { SetViewSize(value.Width, value.Height); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TotalViewTiles
        {
            get { return this.ViewHeight * this.ViewWidth; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int TotalVisibleTiles
        {
            get { return Math.Min(this.MapLength - this.Zero.Index, this.TotalViewTiles); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int VisibleTileWidth
        {
            get { return Math.Min(this.ViewWidth, this.MapWidth); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int VisibleTileHeight
        {
            get { return Math.Min(this.ViewHeight, this.MapHeight); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size VisibleTileRegion
        {
            get { return new Size(this.VisibleTileWidth, this.VisibleTileHeight); }
        }

        [Category("Property Changed")]
        [Description("Occurs when the size of the editor changes.")]
        public event EventHandler ViewSizeChanged;

        protected virtual void SetViewSize(int viewW, int viewH)
        {
            // Make sure the view size is valid.
            if (viewW <= 0)
                throw new ArgumentOutOfRangeException("viewW");
            if (viewH <= 0)
                throw new ArgumentOutOfRangeException("viewH");

            // Avoid redundant setting.
            if (this.viewW == viewW && this.viewH == viewH)
                return;

            // Set the view size.
            this.viewW = viewW;
            this.viewH = viewH;

            OnViewSizeChanged(EventArgs.Empty);
        }

        protected virtual void OnViewSizeChanged(EventArgs e)
        {
            // Change the map dimensions for linear displays
            if (this.linear)
            {
                // Set the rectangular map dimensions to fit the view width.
                this.mapW = this.viewW;
                this.mapH = this.mapL / this.viewW;
                this.mapR = this.mapL % this.viewW;

                OnSelectedTilesChanged(EventArgs.Empty);
            }

            // Keep the zero point in the view boundary.
            if (this.mapW > 0)
                SetZeroPointBoundary();

            // Invoke the event.
            if (ViewSizeChanged != null)
                ViewSizeChanged(this, e);

            // The editor size has changed.
            OnVisibleSizeChanged(EventArgs.Empty);
        }
    }
}