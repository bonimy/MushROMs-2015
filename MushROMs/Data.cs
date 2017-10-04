using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MushROMs
{
    public class Data : IDisposable
    {
        private MemoryStream _memory;

        public int Length
        {
            get { return (int)_memory.Length; }
        }

        public Data()
            : this(new MemoryStream())
        {
        }

        public Data(MemoryStream memory)
        {
            if (memory == null)
                throw new ArgumentNullException("memory");

            this._memory = memory;
        }

        ~Data()
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
            if (disposing)
                _memory.Dispose();
        }

        public void ResetData(int size)
        {
            _memory.SetLength(size);
        }

        public int ReadByte()
        {
            return _memory.ReadByte();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _memory.Read(buffer, offset, count);
        }

        public void WriteByte(byte value)
        {
            _memory.WriteByte(value);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _memory.Write(buffer, offset, count);
        }

        public int Seek(int offset, SeekOrigin loc)
        {
            return (int)_memory.Seek(offset, loc);
        }
    }
}
