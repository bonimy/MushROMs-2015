using System;
using System.Drawing;

namespace MushROMs
{
    public struct Selection
    {
        private int startAddress;
        private int containerWidth;

        private Size size;

        public int StartAddress
        {
            get { return this.startAddress; }
        }

        public int ContainerWidth
        {
            get { return this.containerWidth; }
        }

        public Size Size
        {
            get { return this.size; }
        }

        public int Width
        {
            get { return this.Size.Width; }
        }

        public int Height
        {
            get { return this.Size.Height; }
        }

        public int TotalTiles
        {
            get { return this.Width * this.Height; }
        }
    }

    public sealed class Selection_
    {
        private IndexPoint first;
        private IndexPoint last;

        private IndexPoint min;
        private IndexPoint max;

        private int containerWidth;

        public IndexPoint First
        {
            get { return this.first; }
        }
        public IndexPoint Last
        {
            get { return this.last; }
        }

        public IndexPoint Min
        {
            get { return this.min; }
        }
        public IndexPoint Max
        {
            get { return this.max; }
        }

        public int Width
        {
            get { return this.Max.RelativeX - this.Min.RelativeX + 1; }
        }
        public int Height
        {
            get { return this.Max.RelativeY - this.Min.RelativeY + 1; }
        }
        public Size Size
        {
            get { return new Size(this.Width, this.Height); }
        }

        public int TotalTiles
        {
            get { return Width * Height; }
        }

        public int ContainerWidth
        {
            get { return this.containerWidth; }
        }

        public int StartAddress
        {
            get { return this.min.Address; }
        }

        public Selection_(IndexPoint first)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            this.first = first.Copy();
            this.last = first.Copy();
            this.min = first.Copy();
            this.max = first.Copy();
            this.containerWidth = first.Zero.Editor.MapWidth;
        }

        public Selection_(IndexPoint first, IndexPoint last)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (last == null)
                throw new ArgumentNullException("last");

            this.first = first.Copy();
            this.last = new IndexPoint(first.Zero);
            this.min = new IndexPoint(first.Zero);
            this.max = new IndexPoint(first.Zero);
            this.containerWidth = first.Zero.Editor.MapWidth;
            Update(last);
        }

        public Selection_(IndexPoint first, Size size)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            this.first = first.Copy();
            this.last = new IndexPoint(first.Zero);
            this.min = new IndexPoint(first.Zero);
            this.max = new IndexPoint(first.Zero);
            this.containerWidth = first.Zero.Editor.MapWidth;
            Update(size);
        }

        public Selection_ Copy()
        {
            return new Selection_(this.First, this.Last);
        }

        public void Update(IndexPoint first, IndexPoint last)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (last == null)
                throw new ArgumentNullException("last");

            this.First.RelativePoint = first.RelativePoint;
            Update(last);
        }

        public void Update(IndexPoint first, Size size)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            this.First.RelativePoint = first.RelativePoint;
            Update(size);
        }

        public void Update(IndexPoint last)
        {
            if (last == null)
                throw new ArgumentNullException("last");

            this.Last.RelativePoint = last.RelativePoint;
            Update();
        }

        public void Update(Size size)
        {
            this.Last.RelativePoint = this.First.RelativePoint + size - new Size(1, 1);
            Update();
        }

        private void Update()
        {
            this.Min.RelativeX = Math.Min(this.First.RelativeX, this.Last.RelativeX);
            this.Min.RelativeY = Math.Min(this.First.RelativeY, this.Last.RelativeY);
            this.Max.RelativeX = Math.Max(this.First.RelativeX, this.Last.RelativeX);
            this.Max.RelativeY = Math.Max(this.First.RelativeY, this.Last.RelativeY);
        }
    }
}