using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    [DefaultEvent("Paint")]
    [DefaultProperty("ClientSize")]
    [Description("Provides a control with predefined settings for rendering images.")]
    public partial class DesignerControl : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual bool AllowPaintBackground
        {
            get { return this.DesignMode; }
        }

        [Browsable(true)]   // This accessor is redone so it can be browsable in the designer.
        [Category("Editor")]
        [DefaultValue(true)]
        [Description("A value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new bool DoubleBuffered
        {
            get { return base.DoubleBuffered; }
            set { base.DoubleBuffered = value; }
        }

        [Browsable(true)]   // This accessor is redone so it can be browsable in the designer.
        [Description("The size of the client area of the form.")]
        public new Size ClientSize
        {
            get { return base.ClientSize; }
            set { base.ClientSize = value; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ClientWidth
        {
            get { return this.ClientSize.Width; }
            set { this.ClientSize = new Size(value, this.ClientHeight); }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ClientHeight
        {
            get { return this.ClientSize.Height; }
            set { this.ClientSize = new Size(this.ClientWidth, value); }
        }

        public new BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; OnBorderStyleChanged(EventArgs.Empty); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size BorderSize
        {
            get
            {
                switch (this.BorderStyle)
                {
                    case BorderStyle.FixedSingle:
                        return SystemInformation.BorderSize;
                    case BorderStyle.Fixed3D:
                        return SystemInformation.Border3DSize;
                    case BorderStyle.None:
                    default:
                        return Size.Empty;
                }
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderWidth
        {
            get { return this.BorderSize.Width; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int BorderHeight
        {
            get { return this.BorderSize.Height; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size FullBorderSize
        {
            get { return this.BorderSize + this.BorderSize; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FullBorderWidth
        {
            get { return this.FullBorderSize.Width; }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FullBorderHeight
        {
            get { return this.FullBorderSize.Height; }
        }

        [Browsable(true)]
        [Category("Property Changed")]
        [Description("Event raised when the value of the BorderStyle property is changed on Control.")]
        public event EventHandler BorderStyleChanged;

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Occurs before the Paint event is raised.")]
        public event PaintEventHandler BeginPaint;

        [Browsable(true)]
        [Category("Appearance")]
        [Description("Occurs after the Paint event has been raised.")]
        public event PaintEventHandler EndPaint;

        public DesignerControl()
        {
            // Set control properties
            this.DoubleBuffered = this.AllowPaintBackground;    // Removes flickering.
            this.BackColor = SystemColors.ControlDark;          // A distinguishing default color.
            this.BorderStyle = BorderStyle.FixedSingle;         // Basic border style.
            this.Margin = Padding.Empty;                        // Most draw controls are placed with no margins desired.
            this.Padding = Padding.Empty;                       // There should also be no padding inside the control.
            this.ResizeRedraw = true;                           // Control will always be redrawn when resizing.

            // Set custom paint styles
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            // Initialize mouse processing options
            this.ProcessMouseOnChange = FallbackProcessMouseOnChange;
            this.ProcessMouseInRange = FallbackProcessMouseInRange;

            // Initialize fallback override input keys
            this.OverrideInputKeys = new Collection<Keys>();
            foreach (Keys key in FallbackOverrideInputKeys)
                this.OverrideInputKeys.Add(key);
        }

        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            if (BorderStyleChanged != null)
                BorderStyleChanged(this, e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (this.AllowPaintBackground)
                base.OnPaintBackground(e);
        }

        protected virtual void OnBeginPaint(PaintEventArgs e)
        {
            if (BeginPaint != null)
                BeginPaint(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            OnBeginPaint(e);
            base.OnPaint(e);
            OnEndPaint(e);
        }

        protected virtual void OnEndPaint(PaintEventArgs e)
        {
            if (EndPaint != null)
                EndPaint(this, e);
        }
    }
}