using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MushROMs.Controls;
using MushROMs.Controls.SNES.PaletteEditor;
using MushROMs.LunarCompress;

namespace MushROMs.GenericEditor
{
    public partial class Form1 : EditorForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void SetSizingRectangle(Rectangle rect)
        {
            int left = rect.Left;
            int top = rect.Top;
            int right = rect.Right;
            int bottom = rect.Bottom;

            if ((this.SizingDirection & SizingDirections.Right) != SizingDirections.None)
                right &= ~0x1F;
            if ((this.SizingDirection & SizingDirections.Left) != SizingDirections.None)
                left &= ~0x1F;
            if ((this.SizingDirection & SizingDirections.Bottom) != SizingDirections.None)
                bottom &= ~0x1F;
            if ((this.SizingDirection & SizingDirections.Top) != SizingDirections.None)
                top &= ~0x1F;
            base.SetSizingRectangle(Rectangle.FromLTRB(left, top, right, bottom));
        }
    }
}
