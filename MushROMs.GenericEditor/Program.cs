using System;
using System.IO;
using System.Windows.Forms;
using MushROMs.LunarCompress;

namespace MushROMs.GenericEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string vocal = @"Z:\Libraries\Games\Dance\In The Groove 2\Themes\Simply Love\Vocalize\";
            Silent(vocal);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void Silent(string path)
        {
            string silent = @"Z:\Libraries\Games\Dance\In The Groove 2\Themes\Simply Love\Sounds\_silent.ogg";
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
                Silent(dir);

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext == ".ogg")
                {
                    File.Delete(file);
                    File.Copy(silent, file);
                }
            }
        }
    }
}
