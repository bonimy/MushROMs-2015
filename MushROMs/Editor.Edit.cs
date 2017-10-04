using System;
using System.ComponentModel;

namespace MushROMs
{
    partial class Editor
    {
        private _IEditorData lastEdit;
        private History<_IEditorData> history;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual _IEditorData LastEdit
        {
            get { return this.lastEdit; }
            protected set { this.lastEdit = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public History<_IEditorData> History
        {
            get { return this.history; }
        }

        [Category("Property Changed")]
        [Description("Occurs when the data of the editor is modified.")]
        public event EventHandler DataModified;

        public virtual void Undo()
        {
            this.History.InvokeUndo();
        }

        private void History_Undo(object sender, EventArgs e)
        {
            ModifyData(this.History.CurrentUndoData, false, false);
        }

        public virtual void Redo()
        {
            this.History.InvokeRedo();
        }

        private void History_Redo(object sender, EventArgs e)
        {
            ModifyData(this.History.CurrentRedoData, false, false);
        }

        public virtual _IEditorData CreateCopy()
        {
            return CreateCopy(this.Selection);
        }

        public virtual _IEditorData CreateCopy(Selection_ selection)
        {
            throw new NotImplementedException();
        }

        public virtual void Paste(_IEditorData data)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteSelection()
        {
            throw new NotImplementedException();
        }

        protected virtual void WriteCopyData(_IEditorData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            WriteCopyData(data, data.Selection.StartAddress);
        }

        protected virtual void WriteCopyData(_IEditorData data, int startAddress)
        {
            throw new NotImplementedException();
        }

        public virtual void ModifyData(_IEditorData data, bool history, bool silent)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            ModifyData(data, data.Selection.StartAddress, history, silent);
        }

        public virtual void ModifyData(_IEditorData data, int startAddress, bool history, bool silent)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            // Add the history data if prompted to do so.
            if (history)
            {
                this.History.AddUndoData(CreateCopy(data.Selection));
                this.History.AddRedoData(data);
            }

            // Write the copy data.
            WriteCopyData(data, startAddress);

            // Ignore invoking data modified event if this is a silent modification.
            if (silent)
                return;

            // Set last edit and invoke data modified event.
            this.lastEdit = data;
            OnDataModified(EventArgs.Empty);
        }

        protected virtual void OnDataModified(EventArgs e)
        {
            if (DataModified != null)
                DataModified(this, e);

            Redraw();
        }
    }
}