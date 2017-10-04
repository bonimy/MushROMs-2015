using System;
using System.Drawing;

namespace MushROMs
{
    public struct Map
    {
        private bool isLinear;
        private int length;
        private int width;

        public bool IsLinear
        {
            get { return this.isLinear; }
        }

        public int Length
        {
            get { return this.length; }
        }
        public int Width
        {
            get { return this.width; }
        }
        public int Height
        {
            get { return this.Length / this.Width; }
        }
        public Size Size
        {
            get { return new Size(this.Width, this.Height); }
        }
        public int Remainder
        {
            get { return this.Length % this.Width; }
        }

        private Map(int length, int width)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length");
            if (width <= 0)
                throw new ArgumentOutOfRangeException("width");

            this.isLinear = true;
            this.length = length;
            this.width = width;
        }
        private Map(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentOutOfRangeException("size");

            this.isLinear = false;
            this.length = size.Width * size.Height;
            this.width = size.Width;
        }

        public static Map MapFromLength(int length, int width)
        {
            return new Map(length, width);
        }
        public static Map MapFromSize(Size size)
        {
            return new Map(size);
        }

        public Point GetPointFromIndex(int index)
        {
            return new Point(GetXFromIndex(index), GetYFromIndex(index));
        }
        public int GetXFromIndex(int index)
        {
            return index % this.Width;
        }
        public int GetYFromIndex(int index)
        {
            return index / this.Width;
        }

        public int GetIndexFromPoint(Point point)
        {
            return GetIndexFromPoint(point.X, point.Y);
        }
        public int GetIndexFromPoint(int x, int y)
        {
            return (y * this.Width) + x;
        }
    }
}