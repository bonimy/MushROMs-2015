using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class EditorControl
    {
        private Bitmap image;
        private Collection<Pen> boundaryPens;
        private Collection<Pen> indexPens;

        public Collection<Pen> BoundaryPens
        {
            get { return this.boundaryPens; }
        }

        public Collection<Pen> ActiveIndexPenCollection
        {
            get { return this.indexPens; }
        }

        protected virtual Bitmap CreateEditorBitmap(PixelEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            return new Bitmap(e.Width, e.Height, e.Stride, PixelFormat.Format32bppRgb, e.Scan0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.DesignMode)
                DrawEditorData(e);

            base.OnPaint(e);
        }

        protected virtual void DrawEditorData(PaintEventArgs e)
        {
            DrawEditorBitmap(e);
            DrawSelectionBoundary(e);
            DrawActiveBoundary(e);
        }

        protected virtual void DrawEditorBitmap(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (this.image != null)
                e.Graphics.DrawImageUnscaled(this.image, Point.Empty);
        }

        protected virtual void DrawSelectionBoundary(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            for (int i = this.BoundaryPens.Count; --i >= 0; )
                e.Graphics.DrawPath(this.BoundaryPens[i], this.SelectionBoundary);
        }

        protected virtual void DrawActiveBoundary(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (this.Editor != null && this.Editor.Active.Index >= 0 && this.Editor.Active.Index < this.Editor.MapLength)
            {
                Rectangle r = new Rectangle(this.Editor.Active.RelativeX * this.Editor.CellSize.Width + 2,
                                            this.Editor.Active.RelativeY * this.Editor.CellSize.Height + 2,
                                            this.Editor.CellSize.Width - 5,
                                            this.Editor.CellSize.Height - 5);

                for (int i = this.ActiveIndexPenCollection.Count; --i >= 0; )
                    e.Graphics.DrawRectangle(this.ActiveIndexPenCollection[i], r);
            }
        }

        private void ActiveEditor_Draw(object sender, PixelEventArgs e)
        {
            if (this.DesignMode)
                return;

            this.image = CreateEditorBitmap(e);
            Invalidate();
        }
    }
}