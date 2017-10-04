using System;
using System.Runtime.InteropServices;

namespace MushROMs.LunarCompress
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        [DllImport(LC.DLLPath, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern bool LunarOpenFile(string fileName, FileMode fileMode);
        [DllImport(LC.DLLPath, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr LunarOpenRAMFile(string data, int fileMode, int size);
        [DllImport(LC.DLLPath)]
        internal static extern IntPtr LunarOpenRAMFile(IntPtr data, int fileMode, int size);
        [DllImport(LC.DLLPath, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern bool LunarSaveRAMFile(string fileName);
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarCloseFile();
        [DllImport(LC.DLLPath)]
        internal static extern int LunarGetFileSize();
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarReadFile(IntPtr destination, int size, int address, int seek);
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarWriteFile(IntPtr source, int size, int address, int seek);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarDecompress(IntPtr destination, int addressToStart, int maxDataSize, CompressionFormat format1, int format2, out int lastROMPosition);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarDecompress(IntPtr destination, int addressToStart, int maxDataSize, CompressionFormat format1, int format2, IntPtr lastROMPosition);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarRecompress(IntPtr source, IntPtr destination, int dataSize, int maxDataSize, CompressionFormat format, int format2);
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarEraseArea(int address, int size);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarExpandROM(int mBits);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarVerifyFreeSpace(int addressStart, int addressEnd, int size, BankType bankType);
        [DllImport(LC.DLLPath, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern bool LunarIPSCreate(IntPtr hwnd, string ipsFileName, string romFileName, string rom2FileName, IPSOptions flags);
        [DllImport(LC.DLLPath, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern bool LunarIPSApply(IntPtr hwnd, string ipsFileName, string romFileName, IPSOptions flags);
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarCreatePixelMap(IntPtr source, IntPtr destination, int numTiles, GraphicsFormat gfxType);
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarCreateBppMap(IntPtr source, IntPtr destination, int numTiles, GraphicsFormat gfxType);
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarRender8x8(IntPtr theMapBits, int theWidth, int theHeight, int displayAtX, int displayAtY, IntPtr pixelMap, IntPtr pcPalette, int map8Tile, Render8x8Flags extra);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarWriteRatArea(IntPtr theData, int size, int preferredAddress, int minRange, int maxRange, RATFunctionFlags flags);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarEraseRatArea(int address, int size, int flags);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarGetRatAreaSize(int address, int flags);
    }
}