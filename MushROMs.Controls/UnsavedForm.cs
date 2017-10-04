using System.Windows.Forms;

namespace MushROMs.Controls
{
    internal sealed partial class UnsavedForm : Form
    {
        public string Message
        {
            get { return this.lblSaveChanges.Text; }
            set { this.lblSaveChanges.Text = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsavedForm"/> class.
        /// </summary>
        public UnsavedForm()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            this.lbxFiles.Items.Clear();
        }

        public string[] GetAllFiles()
        {
            string[] files = new string[this.lbxFiles.Items.Count];
            for (int i = 0; i < files.Length; i++)
                files[i] = this.lbxFiles.Items[i].ToString();
            return files;
        }

        public void PopulateListBox(string[] files)
        {
            this.lbxFiles.Items.Clear();
            for (int i = 0; i < files.Length; i++)
                this.lbxFiles.Items.Add(files[i]);
        }
    }
}