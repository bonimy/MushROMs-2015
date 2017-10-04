using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    [DefaultEvent("ColorValueChanged")]
    [DefaultProperty("SelectedColor")]
    [Description("Provides a control for representing and manipulating a color value.")]
    public class ColorValueControl : DesignerControl
    {
        private const int FallbackClientWidth = 0x10;
        private const int FallbackClientHeight = FallbackClientWidth;
        private static readonly Color FallbackColor = Color.Empty;

        private SolidBrush selectedColor = new SolidBrush(FallbackColor);

        [Category("Editor")]
        [DefaultValue("Black")]
        [Description("The selected color for the control.")]
        public Color SelectedColor
        {
            get { return this.selectedColor.Color; }
            set { this.selectedColor.Color = value; OnColorValueChanged(EventArgs.Empty); }
        }

        [Category("Editor")]
        [Description("Occurs when the selected color value of the control changes.")]
        public event EventHandler ColorValueChanged;

        public ColorValueControl()
        {
            // Set default size of the control.
            this.ClientSize = new Size(FallbackClientWidth, FallbackClientHeight);

            // Set the default selected color.
            this.SelectedColor = FallbackColor;
        }

        protected override void OnClick(EventArgs e)
        {
            SelectedColorClick(e);

            base.OnClick(e);
        }

        protected virtual void SelectedColorClick(EventArgs e)
        {
            // Show a color dialog to edit the color.
            using (ColorDialog dlg = new ColorDialog())
            {
                dlg.FullOpen = true;
                dlg.Color = this.SelectedColor;
                if (dlg.ShowDialog() == DialogResult.OK)
                    this.SelectedColor = dlg.Color;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            SetKeyClick(e);

            base.OnKeyDown(e);
        }

        protected virtual void SetKeyClick(KeyEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (e.KeyCode == Keys.Space)
                OnClick(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            base.OnLostFocus(e);
        }

        protected virtual void OnColorValueChanged(EventArgs e)
        {
            Invalidate();
            if (ColorValueChanged != null)
                ColorValueChanged(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintColorValue(e);

            base.OnPaint(e);
        }

        protected virtual void PaintColorValue(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Fill the color rectangle.
            e.Graphics.FillRectangle(this.selectedColor, this.ClientRectangle);

            // Make sure control is focused.
            if (!this.Focused)
                return;

            // These two pens make a black and white dotted line.
            using (Pen p1 = new Pen(Color.Black, 1),
                       p2 = new Pen(Color.White, 1))
            {
                p1.DashStyle = DashStyle.Dot;
                p2.DashStyle = DashStyle.Dot;
                p2.DashOffset = 1;

                // Draw a black and white dotted rectangle around the control when it is focused.
                Rectangle r = new Rectangle(0, 0, this.ClientWidth - 1, this.ClientHeight - 1);
                e.Graphics.DrawRectangle(p1, r);
                e.Graphics.DrawRectangle(p2, r);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this.selectedColor.Dispose();

            base.Dispose(disposing);
        }
    }
}