using System.Runtime.InteropServices;

namespace MushROMs.LunarCompress
{
    internal static class SafeNativeMethods
    {
        [DllImport(LC.DLLPath)]
        internal static extern int LunarVersion();
        [DllImport(LC.DLLPath)]
        internal static extern bool LunarSetFreeBytes(int value);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarSNEStoPC(int pointer, ROMType romType, int header);
        [DllImport(LC.DLLPath)]
        internal static extern int LunarPCtoSNES(int pointer, ROMType romType, int header);
        [DllImport(LC.DLLPath)]
        internal static extern uint LunarSNEStoPCRGB(ushort snesColor);
        [DllImport(LC.DLLPath)]
        internal static extern ushort LunarPCtoSNESRGB(uint pcColor);
    }
}
