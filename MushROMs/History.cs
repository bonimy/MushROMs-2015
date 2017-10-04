using System;
using System.Collections.Generic;

namespace MushROMs
{
    public class History<T>
    {
        private int saveIndex;
        private int historyIndex;
        private bool forceUnsaved;

        private List<T> undo;
        private List<T> redo;

        public int SaveIndex
        {
            get { return this.saveIndex; }
        }
        public bool ForceUnsaved
        {
            get { return this.forceUnsaved; }
            set { this.forceUnsaved = value; }
        }
        public bool Unsaved
        {
            get { return this.historyIndex != this.saveIndex || this.forceUnsaved; }
        }

        public bool CanUndo
        {
            get { return this.historyIndex > 0; }
        }
        public bool CanRedo
        {
            get { return this.historyIndex < this.undo.Count; }
        }

        public T CurrentUndoData
        {
            get { return this.undo[this.historyIndex]; }
        }
        public T CurrentRedoData
        {
            get { return this.redo[this.historyIndex - 1]; }
        }

        public event EventHandler UndoDataAdded;
        public event EventHandler RedoDataAdded;

        public event EventHandler Undo;
        public event EventHandler Redo;

        public History()
        {
            Reset();
        }

        public History(bool forceUnsaved)
        {
            Reset(forceUnsaved);
        }

        public void Reset()
        {
            Reset(false);
        }

        public void Reset(bool forceUnsaved)
        {
            this.saveIndex =
            this.historyIndex = 0;

            this.undo = new List<T>();
            this.redo = new List<T>();

            this.forceUnsaved = forceUnsaved;
        }

        public void SetSaveIndex()
        {
            this.saveIndex = this.historyIndex;
            this.forceUnsaved = false;
        }

        public void AddUndoData(T data)
        {
            if (this.historyIndex < this.undo.Count)
            {
                this.undo.RemoveRange(this.historyIndex, this.undo.Count - this.historyIndex);
                this.redo.RemoveRange(this.historyIndex, this.redo.Count - this.historyIndex);
            }

            this.undo.Add(data);

            OnUndoDataAdded(EventArgs.Empty);
        }

        protected virtual void OnUndoDataAdded(EventArgs e)
        {
            if (UndoDataAdded != null)
                UndoDataAdded(this, e);
        }

        public void AddRedoData(T data)
        {
            this.redo.Add(data);
            this.historyIndex++;

            OnRedoDataAdded(EventArgs.Empty);
        }

        protected virtual void OnRedoDataAdded(EventArgs e)
        {
            if (RedoDataAdded != null)
                RedoDataAdded(this, e);
        }

        public void InvokeUndo()
        {
            if (this.historyIndex == 0)
                return;

            this.historyIndex--;
            OnUndo(EventArgs.Empty);
        }

        protected virtual void OnUndo(EventArgs e)
        {
            if (Undo != null)
                Undo(this, e);
        }

        public void InvokeRedo()
        {
            if (this.historyIndex == this.redo.Count)
                return;

            this.historyIndex++;
            OnRedo(EventArgs.Empty);
        }

        protected virtual void OnRedo(EventArgs e)
        {
            if (Redo != null)
                Redo(this, e);
        }
    }
}