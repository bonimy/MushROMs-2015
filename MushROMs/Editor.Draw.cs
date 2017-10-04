using System;
using System.ComponentModel;
using System.Timers;

namespace MushROMs
{
    partial class Editor
    {
        private const int FallbackRedrawSleepTime = 5;

        private Timer timer;

        [Category("Draw")]
        [Description("Occurs when the pixels are first initialized.")]
        public event EventHandler<PixelEventArgs> PixelsInitialized;

        [Category("Draw")]
        [Description("Occurs when drawing pixels are ready to be written to.")]
        public event EventHandler<PixelEventArgs> WritePixels;

        [Category("Draw")]
        [Description("Occurs when pixels are ready to be drawn to bitmap objects.")]
        public event EventHandler<PixelEventArgs> Draw;

        public void Redraw()
        {
            // Prevents the method from being called multiple times.
            if (!this.timer.Enabled)
                this.timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            uint[] pixels = InitializePixels();
            unsafe
            {
                fixed (uint* scan0 = pixels)
                {
                    PixelEventArgs pe = new PixelEventArgs((IntPtr)scan0, this.VisibleWidth, this.VisibleHeight);
                    OnPixelsInitialized(pe);
                    OnWritePixels(pe);
                    OnDraw(pe);
                }
            }
            this.timer.Stop();
        }

        protected virtual uint[] InitializePixels()
        {
            return new uint[this.VisibleHeight * this.VisibleWidth];
        }

        protected virtual void OnPixelsInitialized(PixelEventArgs e)
        {
            if (PixelsInitialized != null)
                PixelsInitialized(this, e);
        }

        protected virtual void OnWritePixels(PixelEventArgs e)
        {
            if (WritePixels != null)
                WritePixels(this, e);
        }

        protected virtual void OnDraw(PixelEventArgs e)
        {
            if (Draw != null)
                Draw(this, e);
        }
    }
}