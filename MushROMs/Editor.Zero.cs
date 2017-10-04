using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs
{
    partial class Editor
    {
        private const bool FallbackCanScrollSelection = true;

        private const ScrollEnd FallbackHScrollEnd = ScrollEnd.Last;
        private const ScrollEnd FallbackVScrollEnd = ScrollEnd.Full;

        private ZeroPoint_ zero;

        private bool scroll;

        private ScrollEnd hScrollEnd;
        private ScrollEnd vScrollEnd;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ZeroPoint_ Zero
        {
            get { return this.zero; }
        }

        [Category("Scroll")]
        [DefaultValue(FallbackCanScrollSelection)]
        [Description("Determines whether the editor can automatically scroll itself while selecting.")]
        public bool CanScrollSelection
        {
            get { return this.scroll; }
            set { this.scroll = value; }
        }

        [Category("Scroll")]
        [DefaultValue(FallbackHScrollEnd)]
        [Description("The horizontal scroll end of the editor.")]
        public ScrollEnd HScrollEnd
        {
            get { return this.hScrollEnd; }
            set
            {
                if (Helper.IsEnumValid((int)value, (int)ScrollEnd.Full, (int)ScrollEnd.None))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ScrollEnd));
                this.hScrollEnd = value;
                OnHScrollEndChanged(EventArgs.Empty);
            }
        }

        [Category("Scroll")]
        [DefaultValue(FallbackVScrollEnd)]
        [Description("The vertical scroll end of the editor.")]
        public ScrollEnd VScrollEnd
        {
            get { return this.vScrollEnd; }
            set
            {
                if (Helper.IsEnumValid((int)value, (int)ScrollEnd.Full, (int)ScrollEnd.None))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(ScrollEnd));
                this.vScrollEnd = value;
                OnVScrollEndChanged(EventArgs.Empty);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int HScrollLimit
        {
            get
            {
                switch (this.HScrollEnd)
                {
                    case ScrollEnd.Full:
                        return 0;
                    case ScrollEnd.Last:
                        return this.ViewWidth - 1;
                    case ScrollEnd.None:
                    default:
                        return this.ViewWidth;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int VScrollLimit
        {
            get
            {
                switch (this.VScrollEnd)
                {
                    case ScrollEnd.Full:
                        return 0;
                    case ScrollEnd.Last:
                        return this.ViewHeight - 1;
                    case ScrollEnd.None:
                    default:
                        return this.ViewHeight;
                }
            }
        }

        [Category("Scroll")]
        [Description("Occurs when the zero-point changes.")]
        public event EventHandler ZeroAddressChanged
        {
            add { this.Zero.AddressChanged += value; }
            remove { this.Zero.AddressChanged -= value; }
        }

        [Category("Scroll")]
        [Description("Occurs when the horizontal scroll end changes.")]
        public event EventHandler HScrollEndChanged;

        [Category("Scroll")]
        [Description("Occurs when the vertical scroll end changes.")]
        public event EventHandler VScrollEndChanged;

        public virtual void SetActiveTile(Point tile)
        {
            // Get the IndexPoint of the tile.
            IndexPoint ip = new IndexPoint(this.Zero);
            ip.RelativePoint = tile;

            // The scroll amount of the zero point if the selection goes out of view range.
            Size delta = Size.Empty;

            // Determine whether the active tile is outside of the horizontal boundary.
            if (ip.RelativeX >= this.ViewWidth)
            {
                // Set the active tile to the right edge of the view region.
                ip.RelativeX = this.ViewWidth - 1;

                // Set the scroll offset right one tile if permissable.
                if (this.Zero.X + 1 < this.MapWidth)
                    delta.Width = 1;
            }
            else if (ip.RelativeX < 0)  // Repeat the check for the left edge.
            {
                ip.RelativeX = 0;
                if (this.Zero.X - 1 >= 0)
                    delta.Width = -1;
            }

            // Now check for the vertical boundaries.
            if (ip.RelativeY >= this.ViewHeight)
            {
                ip.RelativeY = this.ViewHeight - 1;
                if (this.Zero.Y + 1 < this.MapHeight)
                    delta.Height = 1;
            }
            else if (ip.RelativeY < 0)
            {
                ip.RelativeY = 0;
                if (this.Zero.Y - 1 >= 0)
                    delta.Height = -1;
            }

            // Do scrolling logic if selecting and allowed to scroll.
            if (this.Selecting && this.CanScrollSelection)
            {
                // Adjust horizontal scrolling given the editor boundaries.
                if (Math.Abs(ip.RelativeX - this.Selection.First.RelativeX + delta.Width) >= (this.MapIsLinear ? this.ViewWidth : this.MapWidth))
                    delta.Width = 0;

                // Adjust vertical scrolling.
                if (Math.Abs(ip.AbsoluteY + delta.Height) >= this.MapHeight)
                    delta.Height = 0;

                // Scroll the editor.
                if (delta != Size.Empty && this.CanScrollSelection)
                    Scroll(delta);
            }

            // Set the active point.
            this.Active.RelativePoint = ip.RelativePoint;
        }

        public virtual void Scroll(Size amount)
        {
            if (!this.CanScrollSelection)
                return;

            // Do nothing if no scroll.
            if (amount == Size.Empty)
                return;

            // Set the new zero point.
            this.Zero.Point += amount;
        }

        private void Zero_AddressChanged(object sender, EventArgs e)
        {
            SetZeroPointBoundary();

            // Changing the zero index means the visible boundary path has changed.
            OnSelectedTilesChanged(EventArgs.Empty);
        }

        protected virtual void SetZeroPointBoundary()
        {
            Point p = this.Zero.Point;

            // Set the left and right boundaries.
            if (p.X > this.HScrollLimit + this.MapWidth - this.ViewWidth)
                p.X = this.HScrollLimit + this.MapWidth - this.ViewWidth;
            if (p.X < 0)
                p.X = 0;

            // Set the top and bottom boundaries.
            if (p.Y > this.VScrollLimit + this.MapHeight - this.ViewHeight)
                p.Y = this.VScrollLimit + this.MapHeight - this.ViewHeight;
            if (p.Y < 0)
                p.Y = 0;

            if (p != this.Zero.Point)
            {
                this.Zero.Point = p;
                return;
            }
        }

        protected virtual void OnHScrollEndChanged(EventArgs e)
        {
            if (HScrollEndChanged != null)
                HScrollEndChanged(this, e);
        }

        protected virtual void OnVScrollEndChanged(EventArgs e)
        {
            if (VScrollEndChanged != null)
                VScrollEndChanged(this, e);
        }
    }
}
