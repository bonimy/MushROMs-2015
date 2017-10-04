using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public class EditorVScrollBar : EditorScrollBar
    {
        protected override sealed int Direction
        {
            get { return EditorScrollBar.DIR_VERT; }
        }

        protected override Size DefaultSize
        {
            get { return new Size(SystemInformation.VerticalScrollBarWidth, 80); }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override RightToLeft RightToLeft
        {
            get { return RightToLeft.No; }
            set { }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler RightToLeftChanged
        {
            add { base.RightToLeftChanged += value; }
            remove { base.RightToLeftChanged -= value; }
        }
    }
}