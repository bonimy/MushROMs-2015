using System;

namespace MushROMs
{
    public class PixelEventArgs : EventArgs
    {
        private readonly int width;
        private readonly int height;
        private readonly int stride;
        private readonly IntPtr scan0;

        public IntPtr Scan0
        {
            get { return this.scan0; }
        }

        public int Width
        {
            get { return this.width; }
        }

        public int Height
        {
            get { return this.height; }
        }

        public int Stride
        {
            get { return this.stride; }
        }

        public PixelEventArgs(IntPtr scan0, int width, int height)
        {
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException("scan0");
            if (width < 0)
                throw new ArgumentOutOfRangeException("width");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height");

            long stride = width * sizeof(uint);
            if (stride > Int32.MaxValue)
                throw new OverflowException();

            this.stride = width * sizeof(uint);
            this.scan0 = scan0;
            this.width = width;
            this.height = height;
        }

        public PixelEventArgs(IntPtr scan0, int width, int height, int stride)
        {
            if (scan0 == IntPtr.Zero)
                throw new ArgumentNullException("scan0");
            if (width < 0)
                throw new ArgumentOutOfRangeException("width");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height");
            if (stride < 0)
                throw new ArgumentOutOfRangeException("stride");

            this.scan0 = scan0;
            this.width = width;
            this.height = height;
            this.stride = stride;
        }
    }
}