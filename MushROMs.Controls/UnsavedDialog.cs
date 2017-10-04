using System.ComponentModel;
using System.Windows.Forms;

namespace MushROMs.Controls
{
    public class UnsavedDialog : FormDialog
    {
        private UnsavedForm form = new UnsavedForm();

        protected override Form Form
        {
            get { return this.form; }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [Description("The message displayed on the dialog.")]
        public string Message
        {
            get { return this.form.Message; }
            set { this.form.Message = value; }
        }

        public override void Reset()
        {
            this.form.Reset();
        }

        public string[] GetAllFiles()
        {
            return this.form.GetAllFiles();
        }

        public void PopulateListBox(string[] files)
        {
            this.form.PopulateListBox(files);
        }
    }
}