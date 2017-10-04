using System;
using System.Drawing;

namespace MushROMs
{
    public struct ZeroPoint
    {
        private int index;
    }

    public sealed class ZeroPoint_
    {
        private readonly Editor editor;

        private int address;

        private object indexArgs;
        private object addressArgs;

        internal Editor Editor
        {
            get { return this.editor; }
        }

        public int X
        {
            get { return GetXCoordinate(this.Index); }
            set { this.Index = GetIndex(value, this.Y); }
        }
        public int Y
        {
            get { return GetYCoordinate(this.Index); }
            set { this.Index = GetIndex(this.X, value); }
        }
        public Point Point
        {
            get { return GetCoordinates(this.Index); }
            set { this.Index = GetIndex(value.X, value.Y); }
        }

        public int Index
        {
            get { return this.editor.GetIndexFromAddress(this.Address, this.AddressArgs); }
            set { this.Address = this.editor.GetAddressFromIndex(value, this.IndexArgs); }
        }

        public int Address
        {
            get { return this.address; }
            set { this.address = value; OnAddressChanged(EventArgs.Empty); }
        }

        public object IndexArgs
        {
            get { return this.indexArgs; }
            set { this.indexArgs = value; }
        }
        public object AddressArgs
        {
            get { return this.addressArgs; }
            set { this.addressArgs = value; }
        }

        public event EventHandler AddressChanged;

        public ZeroPoint_(Editor editor)
        {
            if (editor == null)
                throw new ArgumentNullException("editor");

            this.editor = editor;
        }

        public Point GetCoordinates(int index)
        {
            return new Point(GetXCoordinate(index), GetYCoordinate(index));
        }

        public int GetXCoordinate(int index)
        {
            return index % this.editor.MapWidth;
        }

        public int GetYCoordinate(int index)
        {
            return index / this.editor.MapWidth;
        }

        public int GetIndex(Point zeroPoint)
        {
            return GetIndex(zeroPoint.X, zeroPoint.Y);
        }

        public int GetIndex(int zeroX, int zeroY)
        {
            return (zeroY * this.editor.MapWidth) + zeroX;
        }

        private void OnAddressChanged(EventArgs e)
        {
            if (AddressChanged != null)
                AddressChanged(this, e);
        }
    }
}