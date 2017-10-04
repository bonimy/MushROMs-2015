using System;
using System.ComponentModel;

namespace MushROMs
{
    partial class Editor
    {
        protected byte[] data;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public byte[] Data
        {
            get { return this.data; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int DataSize
        {
            get { return this.Data.Length; }
        }

        [Category("Data")]
        [Description("Occurs when data of the editor is reset.")]
        public event EventHandler DataReset;

        protected virtual void ResetData(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            this.data = new byte[size];
            OnDataReset(EventArgs.Empty);
        }

        public virtual int GetAddressFromIndex(int index, object args)
        {
            return index;
        }

        public virtual int GetIndexFromAddress(int address, object args)
        {
            return address;
        }

        protected virtual void OnDataReset(EventArgs e)
        {
            if (this.FileDataType == FileDataType.ProgramCreated)
                CreateUntitledFileName();

            if (DataReset != null)
                DataReset(this, e);

            Redraw();
        }
    }
}