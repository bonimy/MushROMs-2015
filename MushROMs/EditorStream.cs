using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MushROMs
{
    public class EditorStream : Stream
    {
        private Stream _stream;

        public override bool CanRead
        {
            get { return this._stream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this._stream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return this._stream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return this._stream.CanWrite; }
        }

        public override long Length
        {
            get { return this._stream.Length; }
        }

        public override long Position
        {
            get { return this._stream.Position; }
            set { this._stream.Position = value; }
        }

        public override int ReadTimeout
        {
            get { return this._stream.ReadTimeout; }
            set { this._stream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return this._stream.WriteTimeout; }
            set { this._stream.WriteTimeout = value; }
        }

        public EditorStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            this._stream = stream;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this._stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this._stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            this._stream.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                this._stream.Dispose();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return this._stream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            base.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            this._stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._stream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return this._stream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._stream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this._stream.WriteByte(value);
        }
    }
}
