using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MushROMs.Controls
{
    public partial class EditorControl : DesignerControl, IEditorControl
    {
        private Editor editor;

        private EditorHScrollBar hScrollBar;
        private EditorVScrollBar vScrollBar;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Editor Editor
        {
            get { return this.editor; }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("The horizontal scroll bar that handles the scrolling for this editor control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public EditorHScrollBar EditorHScrollBar
        {
            get { return this.hScrollBar; }
            set
            {
                if (this.EditorHScrollBar == value)
                    return;

                this.hScrollBar = value;

                if (this.EditorHScrollBar != null)
                    this.EditorHScrollBar.EditorControl = this;
            }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("The vertical scroll bar that handles the scrolling for this editor control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public EditorVScrollBar EditorVScrollBar
        {
            get { return this.vScrollBar; }
            set
            {
                if (this.EditorVScrollBar == value)
                    return;

                this.vScrollBar = value;

                if (this.EditorVScrollBar != null)
                    this.EditorVScrollBar.EditorControl = this;
            }
        }

        [Localizable(true)]
        [Browsable(true)]   // This accessor is redone so it can be browsable in the designer.
        [Description("The size of the client area of the form.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size ClientSize
        {
            get { return base.ClientSize; }
        }

        [Localizable(true)]
        [Browsable(true)]
        [Description("The size of the control in pixels.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size Size
        {
            get { return base.Size; }
        }

        public EditorControl() : this(new Editor())
        {
        }

        protected EditorControl(Editor editor)
        {
            if (editor == null)
                throw new ArgumentNullException("editor");

            // Set the editor for the control.
            this.editor = editor;

            this.Editor.SelectedTilesChanged += Editor_SelectedTilesChanged;
            this.Editor.VisibleSizeChanged += Editor_VisibleSizeChanged;
            this.Editor.ActiveIndexChanged += Editor_ActiveIndexChanged;
            this.Editor.Draw += ActiveEditor_Draw;

            // Draw the editor data.
            SetControlSize();
            Invalidate();

            // Initialize selection mouse buttons.
            this.selectRegionMouseButton = FallbackSelectMouseButton;

            // Initialize selection boundary.
            this.boundary = new GraphicsPath();
            this.boundaryPens = new Collection<Pen>();
            this.indexPens = new Collection<Pen>();
        }

        protected virtual void SetControlSize()
        {
            if (this.Editor != null)
                SetClientSizeCore(this.Editor.VisibleWidth, this.Editor.VisibleHeight);
        }

        public virtual Point GetTileFromPixel(Point pixel)
        {
            if (this.Editor == null)
                return Point.Empty;

            // Each tile coordinate is separated exactly by the cell size.
            Point p = new Point(pixel.X / this.Editor.CellSize.Width, pixel.Y / this.Editor.CellSize.Height);

            // We round from the left boundary for negative coordinates.
            if (pixel.X < 0)
                p.X--;
            if (pixel.Y < 0)
                p.Y--;

            return p;
        }

        protected override void OnResize(EventArgs e)
        {
            this.Editor.Redraw();
            base.OnResize(e);
        }

        private void Editor_SelectedTilesChanged(object sender, EventArgs e)
        {
            SetBoundary();
        }

        private void Editor_ActiveIndexChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Editor_VisibleSizeChanged(object sender, EventArgs e)
        {
            SetBoundary();
            SetControlSize();
        }
    }
}