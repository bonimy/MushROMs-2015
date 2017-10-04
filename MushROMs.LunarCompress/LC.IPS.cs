using System;
using System.Windows.Forms;

namespace MushROMs.LunarCompress
{
    partial class LC
    {
        #region Methods
        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate()
        {
            return UnsafeNativeMethods.LunarIPSCreate(Form.ActiveForm.Handle, null, null, null, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(IWin32Window owner)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSCreate(owner.Handle, null, null, null, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(string romPath)
        {
            return UnsafeNativeMethods.LunarIPSCreate(Form.ActiveForm.Handle, null, romPath, null, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(IWin32Window owner, string romPath)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSCreate(owner.Handle, null, romPath, null, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath1">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="romPath2">
        /// Path of the modified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(string romPath1, string romPath2)
        {
            return UnsafeNativeMethods.LunarIPSCreate(Form.ActiveForm.Handle, null, romPath1, romPath2, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath1">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="romPath2">
        /// Path of the modified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(IWin32Window owner, string romPath1, string romPath2)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSCreate(owner.Handle, null, romPath1, romPath2, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath1">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="romPath2">
        /// Path of the modified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS patch to create. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(string romPath1, string romPath2, string ipsPath)
        {
            return UnsafeNativeMethods.LunarIPSCreate(Form.ActiveForm.Handle, ipsPath, romPath1, romPath2, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath1">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="romPath2">
        /// Path of the modified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS patch to create. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(IWin32Window owner, string romPath1, string romPath2, string ipsPath)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSCreate(owner.Handle, ipsPath, romPath1, romPath2, 0);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath1">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="romPath2">
        /// Path of the modified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS patch to create. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="flags">
        /// Optional flags. This can be any combination of <see cref="IPSOptions.None"/>,
        /// <see cref="IPSOptions.Quiet"/>, or <see cref="IPSOptions.ForceFileSaveAs"/>.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(string romPath1, string romPath2, string ipsPath, IPSOptions flags)
        {
            return UnsafeNativeMethods.LunarIPSCreate(Form.ActiveForm.Handle, ipsPath, romPath1, romPath2, flags);
        }

        /// <summary>
        /// Creates an IPS patch file by measuring differences between two other files.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath1">
        /// Path of the unmodified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="romPath2">
        /// Path of the modified ROM to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS patch to create. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="flags">
        /// Optional flags. This can be any combination of <see cref="IPSOptions.None"/>,
        /// <see cref="IPSOptions.Quiet"/>, or <see cref="IPSOptions.ForceFileSaveAs"/>.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSCreate(IWin32Window owner, string romPath1, string romPath2, string ipsPath, IPSOptions flags)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSCreate(owner.Handle, ipsPath, romPath1, romPath2, flags);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply()
        {
            return UnsafeNativeMethods.LunarIPSApply(Form.ActiveForm.Handle, null, null, 0);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(IWin32Window owner)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSApply(owner.Handle, null, null, 0);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath">
        /// Path of the ROM file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(string romPath)
        {
            return UnsafeNativeMethods.LunarIPSApply(Form.ActiveForm.Handle, null, romPath, 0);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath">
        /// Path of the ROM file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(IWin32Window owner, string romPath)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSApply(owner.Handle, null, romPath, 0);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath">
        /// Path of the ROM file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(string romPath, string ipsPath)
        {
            return UnsafeNativeMethods.LunarIPSApply(Form.ActiveForm.Handle, ipsPath, romPath, 0);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath">
        /// Path of the ROM file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(IWin32Window owner, string romPath, string ipsPath)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSApply(owner.Handle, ipsPath, romPath, 0);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="romPath">
        /// Path of the ROM file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="flags">
        /// Optional flags. Can be any combination of <see cref="IPSOptions.None"/>, 
        /// <see cref="IPSOptions.Log"/>, <see cref="IPSOptions.Quiet"/>, or <see cref="IPSOptions.ExtraWarnings"/>.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(string romPath, string ipsPath, IPSOptions flags)
        {
            return UnsafeNativeMethods.LunarIPSApply(Form.ActiveForm.Handle, ipsPath, romPath, flags);
        }

        /// <summary>
        /// Apply an IPS patch file to another file.
        /// This is based off the code in the LunarIPS (LIPS) utility on FuSoYa's site.
        /// </summary>
        /// <param name="owner">
        /// The window that will own the modal dialog box. This is used only to prevent
        /// user input to the window. You can set this to null if you want.
        /// </param>
        /// <param name="romPath">
        /// Path of the ROM file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="ipsPath">
        /// Path of the IPS file to use. Set this to null and Lunar Compress
        /// will prompt the user for the file name.
        /// </param>
        /// <param name="flags">
        /// Optional flags. Can be any combination of <see cref="IPSOptions.None"/>, 
        /// <see cref="IPSOptions.Log"/>, <see cref="IPSOptions.Quiet"/>, or <see cref="IPSOptions.ExtraWarnings"/>.
        /// </param>
        /// <returns>
        /// True on success, false on fail.
        /// </returns>
        /// <remarks>
        /// Lunar Compress will prompt the user for the files not specified.
        /// </remarks>
        public static bool IPSApply(IWin32Window owner, string romPath, string ipsPath, IPSOptions flags)
        {
            if (owner == null)
                return false;

            return UnsafeNativeMethods.LunarIPSApply(owner != null ? owner.Handle : IntPtr.Zero, ipsPath, romPath, flags);
        }
        #endregion
    }
}
