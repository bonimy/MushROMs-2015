using System.Drawing;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public class EditorHScrollBar : EditorScrollBar
    {
        protected override sealed int Direction
        {
            get { return EditorScrollBar.DIR_HORZ; }
        }

        protected override Size DefaultSize
        {
            get { return new Size(80, SystemInformation.HorizontalScrollBarHeight); }
        }
    }
}