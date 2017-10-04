using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;

namespace MushROMs
{
    public static class GZip
    {
        public static readonly ReadOnlyCollection<byte> MagicNumber = new ReadOnlyCollection<byte>(new byte[] { 0x1F, 0x8B });

        public static bool IsCompressed(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (data.Length < MagicNumber.Count)
                return false;

            for (int i = MagicNumber.Count; --i >= 0; )
                if (data[i] != MagicNumber[i])
                    return false;

            return true;
        }

        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (!IsCompressed(data))
                return (byte[])data.Clone();

            const int size = 0x10000;
            byte[] buffer = new byte[size];
            int count = 0;

            // using statement doesn't properly dispose these streams.
            {
                MemoryStream memory = new MemoryStream();
                GZipStream gzip = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);

                try
                {
                    while ((count = gzip.Read(buffer, 0, size)) > 0)
                        memory.Write(buffer, 0, count);
                    return memory.ToArray();
                }
                finally
                {
                    memory.Dispose();
                    gzip.Dispose();
                }
            }
        }

        public static byte[] Compress(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            {
                MemoryStream memory = new MemoryStream();
                GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true);

                try
                {
                    gzip.Write(data, 0, data.Length);
                    return memory.ToArray();
                }
                finally
                {
                    gzip.Dispose();
                    memory.Dispose();
                }
            }
        }
    }
}