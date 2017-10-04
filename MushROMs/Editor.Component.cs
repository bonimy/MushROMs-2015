using System;
using System.ComponentModel;

namespace MushROMs
{
    partial class Editor : IComponent
    {
        private bool disposed = false;
        private ISite site;
        private IContainer components;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site
        {
            get { return this.site; }
            set { this.site = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public event EventHandler Disposed;

        ~Editor()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                lock (this)
                {
                    if (this.site != null && this.site.Container != null)
                        this.site.Container.Remove(this);

                    this.components.Dispose();

                    if (Disposed != null)
                        Disposed(this, EventArgs.Empty);
                }
            }

            disposed = true;
        }
    }
}