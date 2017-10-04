using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs.SNES
{
    public partial class Palette : Editor
    {
        public const int SNESPaletteWidth = 0x10;
        public const int SNESPaletteRows = 0x10;
        public const int SNESPaletteSize = SNESPaletteRows * SNESPaletteWidth;

        private const int SNESColorSize = sizeof(ushort);
        private const int PALColorSize = 3;

        private ushort backColor;

        public ushort BackColor
        {
            get { return this.backColor; }
            set { this.backColor = value; OnBackColorChanged(EventArgs.Empty); }
        }

        public override int TotalVisibleTiles
        {
            get { return Math.Min((this.DataSize - this.Zero.Address) / SNESColorSize, this.TotalViewTiles); }
        }

        public event EventHandler BackColorChanged;

        public Palette()
        {
            this.Zero.IndexArgs = new object[] { (int)0 };

            this.ViewSize = new Size(SNESPaletteWidth, SNESPaletteRows);
            this.TileSize = new Size(1, 1);
            this.ZoomSize = new Size(0x10, 0x10);

            this.FileDataType = FileDataType.ProgramCreated;
        }

        public Palette(IContainer container)
            : this()
        {
            if (container == null)
                return;

            container.Add(this);
        }

        public override int GetAddressFromIndex(int index, object args)
        {
            return (index * SNESColorSize) + ((int)args % SNESColorSize);
        }

        public override int GetIndexFromAddress(int address, object args)
        {
            return address / SNESColorSize;
        }

        protected virtual void OnBackColorChanged(EventArgs e)
        {
            if (BackColorChanged != null)
                BackColorChanged(this, e);
        }
    }
}