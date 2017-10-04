using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public abstract class FormDialog : IComponent
    {
        private bool disposed = false;

        protected abstract Form Form { get; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site
        {
            get { return this.Form.Site; }
            set { this.Form.Site = value; }
        }

        public string Title
        {
            get { return this.Form.Text; }
            set { this.Form.Text = value; }
        }

        public bool ShowHelp
        {
            get { return this.Form.HelpButton; }
            set { this.Form.HelpButton = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            get { return this.Form.Tag; }
            set { this.Form.Tag = value; }
        }

        public event HelpEventHandler HelpRequested
        {
            add { this.Form.HelpRequested += value; }
            remove { this.Form.HelpRequested -= value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public event EventHandler Disposed
        {
            add { this.Form.Disposed += value; }
            remove { this.Form.Disposed -= value; }
        }

        ~FormDialog()
        {
            Dispose(false);
        }

        public abstract void Reset();

        public DialogResult ShowDialog()
        {
            return this.Form.ShowDialog();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return this.Form.ShowDialog(owner);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
                this.Form.Dispose();

            this.disposed = true;
        }
    }
}
