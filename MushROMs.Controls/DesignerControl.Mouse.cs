using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    partial class DesignerControl
    {
        private const bool FallbackProcessMouseOnChange = true;
        private const bool FallbackProcessMouseInRange = true;

        public static readonly Point MouseOutOfRange = new Point(-1, -1);
        public const int MouseWheelThreshold = 120;

        private Point currentMousePoint;
        private Point previousMousePoint;

        private MouseButtons currentMouseButtons;
        private MouseButtons previousMouseButtons;
        private MouseButtons activeMouseButtons;

        private bool processMouseOnChange;
        private bool processMouseInRange;

        [Browsable(true)]
        [Category("Editor")]
        [Description("Determines whether mouse handling will occur only while the mouse is moving.")]
        [DefaultValue(FallbackProcessMouseOnChange)]
        public bool ProcessMouseOnChange
        {
            get { return this.processMouseOnChange; }
            set { this.processMouseOnChange = value; }
        }

        [Browsable(true)]
        [Category("Editor")]
        [Description("Determines whether mouse handling will occur only while the mouse is in the client area.")]
        [DefaultValue(FallbackProcessMouseInRange)]
        public bool ProcessMouseInRange
        {
            get { return this.processMouseInRange; }
            set { this.processMouseInRange = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point CurrentMousePoint
        {
            get { return this.currentMousePoint; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Point PreviousMousePoint
        {
            get { return this.previousMousePoint; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MouseButtons CurrentMouseButtons
        {
            get { return this.currentMouseButtons; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MouseButtons PreviousMouseButtons
        {
            get { return this.previousMouseButtons; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MouseButtons ActiveMouseButtons
        {
            get { return this.activeMouseButtons; }
        }

        [Browsable(true)]
        [Category("Mouse")]
        [Description("Occurs when the mouse wheel moves while the control has focus.")]
        public new event MouseEventHandler MouseWheel
        {
            add { base.MouseWheel += value; }
            remove { base.MouseWheel -= value; }
        }

        [Category("Mouse")]
        [Description("Occurs when the mouse moves while in the defined valid range.")]
        public event MouseEventHandler InRangeMouseMove;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Ignore mouse processing unless moving if told to do so.
            if (this.ProcessMouseOnChange && this.currentMousePoint == e.Location)
                return;

            // Ignore mouse processing unless in range if told to do so.
            if (this.ProcessMouseInRange && !this.ClientRectangle.Contains(e.Location))
                return;

            // Update current and previous mouse locations.
            this.previousMousePoint = this.currentMousePoint;
            this.currentMousePoint = e.Location;

            OnInRangeMouseMove(e);

            // Continue standard mouse processing.
            base.OnMouseMove(e);
        }

        protected virtual void OnInRangeMouseMove(MouseEventArgs e)
        {
            if (InRangeMouseMove != null)
                InRangeMouseMove(this, e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            // Update previous mouse location
            this.previousMousePoint = this.currentMousePoint;

            // Set current mouse location to be out of range.
            this.currentMousePoint = MouseOutOfRange;

            // Continue standard mouse processing.
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Update current, previous, and active mouse buttons.
            this.previousMouseButtons = this.currentMouseButtons;
            this.currentMouseButtons = e.Button;
            this.activeMouseButtons = this.currentMouseButtons & ~this.previousMouseButtons;

            // Continue standard mouse processing.
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            // Update the current and previous mouse buttons.
            this.previousMouseButtons = this.currentMouseButtons;
            this.currentMouseButtons &= ~e.Button;
            this.activeMouseButtons = MouseButtons.None;

            // Continue standard mouse processing.
            base.OnMouseUp(e);
        }
    }
}
