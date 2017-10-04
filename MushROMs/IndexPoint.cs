using System;
using System.Drawing;

namespace MushROMs
{
    public sealed class IndexPoint
    {
        private readonly ZeroPoint_ zero;

        private int address;
        private int absoluteX;
        private int absoluteY;

        internal ZeroPoint_ Zero
        {
            get { return this.zero; }
        }

        public int AbsoluteX
        {
            get { return this.absoluteX; }
            set { SetIndexPoint(value, this.AbsoluteY); }
        }
        public int AbsoluteY
        {
            get { return this.absoluteY; }
            set { SetIndexPoint(this.AbsoluteX, value); }
        }
        public Point AbsolutePoint
        {
            get { return new Point(this.AbsoluteX, this.AbsoluteY); }
            set { SetIndexPoint(value.X, value.Y); }
        }

        public int RelativeX
        {
            get { return this.AbsoluteX - this.Zero.X; }
            set { SetIndexPoint(value + this.Zero.X, this.AbsoluteY); }
        }
        public int RelativeY
        {
            get { return this.AbsoluteY - this.Zero.Y; }
            set { SetIndexPoint(this.AbsoluteX, value + this.Zero.Y); }
        }
        public Point RelativePoint
        {
            get { return new Point(this.RelativeX, this.RelativeY); }
            set { SetIndexPoint(value.X + this.Zero.X, value.Y + this.Zero.Y); }
        }

        public int Index
        {
            get { return this.Zero.GetIndex(this.AbsoluteX, this.AbsoluteY); }
            set { SetIndexPoint(this.Zero.GetXCoordinate(value), this.Zero.GetYCoordinate(value)); }
        }

        public int Address
        {
            get { return this.address; }
        }

        public event EventHandler IndexChanged;

        public IndexPoint(ZeroPoint_ zero)
        {
            if (zero == null)
                throw new ArgumentNullException("zero");

            this.zero = zero;
        }

        public IndexPoint Copy()
        {
            IndexPoint ip = new IndexPoint(this.zero);
            ip.address = this.address;
            ip.absoluteX = this.absoluteX;
            ip.absoluteY = this.absoluteY;
            return ip;
        }

        private void SetIndexPoint(int absoluteX, int absoluteY)
        {
            this.absoluteX = absoluteX;
            this.absoluteY = absoluteY;
            OnIndexChanged(EventArgs.Empty);
        }

        private void OnIndexChanged(EventArgs e)
        {
            this.address = this.Zero.Editor.GetAddressFromIndex(this.Index, this.zero.IndexArgs);
            if (IndexChanged != null)
                IndexChanged(this, e);
        }
    }
}