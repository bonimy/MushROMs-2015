using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs
{
    partial class Editor
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get { return this.MapWidth * this.CellWidth; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get { return this.MapHeight * this.CellHeight; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size Size
        {
            get { return new Size(this.Width, this.Height); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VisibleWidth
        {
            get { return this.ViewWidth * this.CellWidth; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int VisibleHeight
        {
            get { return this.ViewHeight * this.CellHeight; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size VisibleSize
        {
            get { return new Size(this.VisibleWidth, this.VisibleHeight); }
        }

        [Category("Property Changed")]
        [Description("Occurs when the size of the editor changes.")]
        public event EventHandler SizeChanged;

        [Category("Property Changed")]
        [Description("Occurs when the size of the visible region changes.")]
        public event EventHandler VisibleSizeChanged;

        protected virtual void OnSizeChanged(EventArgs e)
        {
            if (SizeChanged != null)
                SizeChanged(this, e);
        }

        protected virtual void OnVisibleSizeChanged(EventArgs e)
        {
            if (VisibleSizeChanged != null)
                VisibleSizeChanged(this, e);

            Redraw();
        }
    }
}