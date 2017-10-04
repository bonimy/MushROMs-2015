using System.Collections.Generic;

namespace MushROMs.SNES
{
    public class ROM
    {
        internal const string ExtensionSMC = ".smc";
        internal const string ExtensionSFC = ".sfc";
        internal const string ExtensionSWC = ".swc";
        internal const string ExtensionFIG = ".fig";
        internal const string ExtensionBIN = ".bin";

        public const int HeaderSize = 0x200;

        public const int LoBankSize = 0x8000;
        public const int HiBankSize = 0x10000;

        public enum HeaderTypes
        {
            NoHeader = 0,
            Header = HeaderSize
        }

        internal static bool IsROMExt(string ext)
        {
            return ext == ROM.ExtensionSMC || ext == ROM.ExtensionSFC || ext == ExtensionSWC || ext == ExtensionFIG;
        }

        internal static string[] CreateFilter()
        {
            List<string> list = new List<string>();
            list.AddRange(new string[] {
                ROM.ExtensionSMC,
                ROM.ExtensionSFC,
                ROM.ExtensionSWC,
                ROM.ExtensionFIG });
            return list.ToArray();
        }
    }
}