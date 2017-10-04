using System;
using System.ComponentModel;
using System.IO;
using MushROMs.Properties;

namespace MushROMs
{
    partial class Editor
    {
        private const string UnsavedNotification = "*";

        private FileDataType fileDataType;
        private string untitled;
        private string fp;
        private byte[] fData;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FileDataType FileDataType
        {
            get { return this.fileDataType; }
            protected set
            {
                if (!Helper.IsEnumValid((int)value, (int)FileDataType.NotAFile, (int)FileDataType.FromFile))
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(FileDataType));
                this.fileDataType = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected string Untitled
        {
            get { return this.untitled; }
            set { this.untitled = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string FilePath
        {
            get { return this.fp; }
            protected set { this.fp = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual byte[] FileData
        {
            get { return this.fData; }
            protected set { this.fData = value; }
        }

        [Category("File")]
        [Description("Occurs before file data is opened in the editor.")]
        public event CancelEventHandler FileOpening;

        [Category("File")]
        [Description("Occurs when file data is opened in the editor.")]
        public event EventHandler FileOpened;

        [Category("File")]
        [Description("Occurs before editor data is saved to a file.")]
        public event CancelEventHandler FileSaving;

        [Category("File")]
        [Description("Occurs when editor data is saved to a file.")]
        public event EventHandler FileSaved;

        protected virtual string CreateUntitledFileName()
        {
            return "Untitled.bin";
        }

        public virtual void Open(string path)
        {
            string oldpath = this.FilePath;
            this.FilePath = path;
            CancelEventArgs e = new CancelEventArgs();
            OnFileOpening(e);
            if (e.Cancel)
            {
                this.FilePath = oldpath;
                return;
            }

            this.FileData = File.ReadAllBytes(this.FilePath);
            this.FileDataType = FileDataType.FromFile;

            OnFileOpened(EventArgs.Empty);
        }

        protected virtual void OnFileOpening(CancelEventArgs e)
        {
            if (FileOpening != null)
                FileOpening(this, e);
        }

        protected virtual void OnFileOpened(EventArgs e)
        {
            if (FileOpened != null)
                FileOpened(this, e);
        }

        public virtual void Save()
        {
            Save(this.FilePath);
        }

        public virtual void Save(string path)
        {
            if (this.FileDataType == FileDataType.NotAFile)
                throw new InvalidOperationException(Resources.ErrorNoFileData);

            string oldpath = this.FilePath;
            this.FilePath = path;
            CancelEventArgs e = new CancelEventArgs();
            OnFileSaving(e);
            if (e.Cancel)
            {
                this.FilePath = oldpath;
                return;
            }

            File.WriteAllBytes(this.FilePath, this.FileData);
            OnFileSaved(EventArgs.Empty);
        }

        protected virtual void OnFileSaving(CancelEventArgs e)
        {
            if (FileSaving != null)
                FileSaving(this, e);
        }

        protected virtual void OnFileSaved(EventArgs e)
        {
            this.History.SetSaveIndex();

            if (FileSaved != null)
                FileSaved(this, e);
        }
    }
}