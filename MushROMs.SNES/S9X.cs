using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using MushROMs.SNES.Properties;

namespace MushROMs.SNES
{
    internal static class S9X
    {
        internal const string Extension000 = ".000";
        internal const string Extension00x = ".00";

        internal const string S9XHeader151 = "#!snes9x";
        internal const string S9XHeader153 = "#!s9xsnp";
        internal const int S9XV153 = 153;
        internal const int S9XV151 = 151;
        internal const int S9XSize153 = 0x11D073;
        internal const int S9XSize151 = 0x11C670;
        internal const int PaletteOffset153 = 0xB9;
        internal const int PaletteOffset151 = 0xBE;
        internal const int PathSizeAddress = 0x12;
        internal const int PathDigits = 6;

        internal static bool IsS9XExt(string path)
        {
            path = Path.GetExtension(path).ToLower();
            return path.Length == 4 && path.StartsWith(Extension00x) && path[3] >= '0' && path[3] <= '9';
        }

        internal static string[] CreateFilter()
        {
            List<string> list = new List<string>();
            for (int i = 0; i <= 9; i++)
                list.Add(Extension00x + i.ToString());
            return list.ToArray();
        }

        internal static Version GetVersion(IntPtr data)
        {
            unsafe
            {
                // Compare the data with the different SNES9x save state headers.
                if (new string((sbyte*)data, 0, S9X.S9XHeader151.Length) == S9X.S9XHeader151)
                    return Version.V151;
                else if (new string((sbyte*)data, 0, S9X.S9XHeader153.Length) == S9X.S9XHeader153)
                    return Version.V153;
            }

            // Return none if no headers matched.
            return Version.None;
        }

        internal static int GetPathLength(IntPtr data)
        {
            int length = 0;
            unsafe
            {
                if (!int.TryParse(new string((sbyte*)data, S9X.PathSizeAddress, S9X.PathDigits), out length))
                    throw new ArgumentException(Resources.ErrorS9XFormat);
            }
            return length;
        }

        internal static int GetSize(Version version)
        {
            switch (version)
            {
                case Version.V151:
                    return S9XSize151;
                case Version.V153:
                    return S9XSize153;
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorS9XVersion);
            }
        }

        internal static int GetPaletteOffset(Version version)
        {
            switch (version)
            {
                case Version.V151:
                    return PaletteOffset151;
                case Version.V153:
                    return PaletteOffset153;
                default:
                    throw new InvalidEnumArgumentException(Resources.ErrorS9XVersion);
            }
        }

        internal enum Version
        {
            None,
            V151,
            V153
        }
    }
}
