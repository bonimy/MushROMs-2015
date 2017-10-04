#if !DEBUG
#define USE_SOURCE
#endif

using System;
using System.Runtime.InteropServices;

namespace MushROMs.LunarCompress
{
    /// <summary>
    /// Provides a strong class-library (originally built in C) for SNES-related functionally in programming.
    /// </summary>
    public static partial class LC
    {
        internal const string DLLPath = "Libraries\\" +
#if x86
            "x86\\" +
#elif x64
            "x64\\" +
#endif
 "Lunar Compress.dll";

        #region Properties
        /// <summary>
        /// The current version of the DLL as an integer.
        /// For example, version 1.30 of the DLL would return "130" (decimal).
        /// </summary>
        public static int Version
        {
            get { return SafeNativeMethods.LunarVersion(); }
        }
        #endregion

#if USE_SOURCE
        private static int LunarVersion()
        {
            return 171;
        }
        
        private static int LunarSNEStoPC(int pointer, ROMType romType, int header)
        {
            if (header != 0)
                header = 0x200;
            switch (romType)
            {
                case ROMType.LoROM:
                case ROMType.LoROM2:
                    return (((pointer & 0x7F0000) >> 1) | (pointer & 0x7FFF)) + header;
                case ROMType.HiROM:
                case ROMType.HiROM2:
                    return (pointer & 0x3FFFFF) + header;
                case ROMType.ExHiROM:
                    if (pointer >= 0xC00000)
                        return (pointer & 0x3FFFFF) + header;
                    return (0x400000 | (pointer & 0x3FFFFF)) + header;
                case ROMType.ExLoROM:
                    if (pointer >= 0x800000)
                        return (((pointer & 0x7F0000) >> 1) | (pointer & 0x7FFF)) + header;
                    return (0x400000 | (((pointer & 0x7F0000) >> 1) | (pointer & 0x7FFF))) + header;
                default:
                    return 0;
            }
        }
        
        private static int LunarPCtoSNES(int pointer, ROMType romType, int header)
        {
            if (header != 0)
                header = 0x200;
            switch (romType)
            {
                case ROMType.LoROM:
                    if (pointer >= 0x380000)
                        return (0x800000 | (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF))) + header;
                    return (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF)) + header;
                case ROMType.HiROM:
                    return (pointer | 0xC00000) + header;
                case ROMType.ExHiROM:
                    if (pointer >= 0x7E0000)
                        return (pointer & ~0x400000) + header;
                    if (pointer >= 0x4000000)
                        return pointer + header;
                    return (0xC00000 | pointer) + header;
                case ROMType.ExLoROM:
                    if (pointer >= 0x400000)
                        return (0x80000 | (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF))) + header;
                    return (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF)) + header;
                case ROMType.LoROM2:
                    return (0x800000 | (((pointer << 1) & 0x7F0000) | 0x8000 | (pointer & 0x7FFF))) + header;
                case ROMType.HiROM2:
                    if (pointer >= 0x300000)
                        return (0xC00000 | pointer) + header;
                    return (0x400000 | pointer) + header;
                default:
                    return 0;
            }
        }

        private static uint LunarSNEStoPCRGB(ushort snesColor)
        {
            return unchecked((uint)(
                ((snesColor << (8 * 3 - 5 * 1)) & 0xF80000) |
                ((snesColor << (8 * 2 - 5 * 2)) & 0x00F800) |
                ((snesColor >> (5 * 3 - 8 * 1)) & 0x0000F8)));
        }

        private static ushort LunarPCtoSNESRGB(uint pcColor)
        {
            return (ushort)(
                ((pcColor & 0xF80000) >> (8 * 3 - 5 * 1)) |
                ((pcColor & 0x00F800) >> (8 * 2 - 5 * 2)) |
                ((pcColor & 0x0000F8) << (5 * 3 - 8 * 1)));
        }
#endif
    }
}