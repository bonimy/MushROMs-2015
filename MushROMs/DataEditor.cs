using System;
using System.IO;

namespace MushROMs
{
    public abstract class DataEditor
    {
        private Data _data;
        private ISelection _currentSelection;
        private History<DataSelection> _history;

        public DataEditor()
            : this(new Data())
        {
        }

        public DataEditor(Data data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this._data = data;

            this._history = new History<DataSelection>();
        }

        public Data Copy(ISelection selection)
        {
            using (Data data = selection.GetData(this._data))
            {
                data.Seek(0, SeekOrigin.Begin);
                byte[] copy = new byte[data.Length];
                data.Read(copy, 0, data.Length);
                return new Data(new MemoryStream(copy));
            }
        }

        public void Paste(Data data, ISelection selection)
        {
        }

        public void DeleteSelection()
        {
            DeleteSelection(this._currentSelection);
        }

        public void DeleteSelection(ISelection selection)
        {
            using (Data data = selection.GetData(this._data))
            {
                data.Seek(0, SeekOrigin.Begin);
                data.Write(new byte[data.Length], 0, data.Length);
                selection.SetData(data, this._data);
            }
        }
    }
}
