using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Timers;
using Timer = System.Timers.Timer;
using System.Windows.Forms;
using MushROMs.Controls.SNES.Properties;
using MushROMs.LunarCompress;
using MushROMs.SNES;

namespace MushROMs.Controls.SNES.PaletteEditor
{
    public partial class PaletteControl : EditorControl
    {
        private Timer timer;
        private int dashOffset = 0;

        public new Palette Editor
        {
            get { return (Palette)base.Editor; }
        }

        public PaletteControl() : base(new Palette())
        {
            this.Editor.PixelsInitialized += ActiveEditor_PixelsInitialized;

            this.timer = new Timer(200);
            this.timer.Elapsed += Timer_Elapsed;
            this.timer.Start();
        }

        protected override void DrawEditorData(PaintEventArgs e)
        {
            float DashLength1 = Settings.Default.PaletteDashLength1;
            float DashLength2 = Settings.Default.PaletteDashLength2;
            Pen p1 = new Pen(Settings.Default.PaletteDashColor1, 1);
            p1.DashStyle = DashStyle.Custom;
            p1.DashPattern = new float[] { DashLength1, DashLength2 };
            p1.DashOffset = this.dashOffset;

            Pen p2 = new Pen(Settings.Default.PaletteDashColor2, 1);
            p2.DashStyle = DashStyle.Custom;
            p2.DashPattern = new float[] { DashLength1, DashLength2 };
            p2.DashOffset = this.dashOffset + DashLength1;

            this.BoundaryPens.Clear();
            this.BoundaryPens.Add(p1);
            this.BoundaryPens.Add(p2);

            base.DrawEditorData(e);
        }

        private void ActiveEditor_PixelsInitialized(object sender, PixelEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (this.Editor == null || this.Editor.TotalVisibleTiles < this.Editor.TotalViewTiles)
            {
                int width = e.Width;
                int height = e.Height;

                int bgSize = Settings.Default.PaletteBGSize;

                uint bgColor1 = LC.SystemToPCColor(Settings.Default.PaletteBGColor1);
                uint bgColor2 = LC.SystemToPCColor(Settings.Default.PaletteBGColor2);

                unsafe
                {
                    uint* dest = (uint*)e.Scan0;

                    for (int i = width * height; --i >= 0; )
                        dest[i] = bgColor1;

                    if (bgColor1 != bgColor2)
                    {
                        int bgSize2 = bgSize << 1;
                        for (int y = height; (y -= bgSize) >= 0; )
                        {
                            int offset = y & bgSize;
                            int index = ((y + 1) * width) + offset;
                            for (int x = width; (x -= bgSize2) >= 0; )
                            {
                                index -= bgSize2;
                                for (int h = bgSize; --h >= 0; )
                                {
                                    int index2 = index + (h * width) + bgSize;
                                    for (int w = bgSize; --w >= 0; )
                                        dest[--index2] = bgColor2;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.dashOffset--;
            this.Invalidate();
        }
    }
}