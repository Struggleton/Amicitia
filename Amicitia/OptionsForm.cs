namespace Amicitia
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    public partial class OptionsForm : Form
    {
        public OptionsForm(MainForm form)
        {
            InitializeComponent();
            PastForm = form;
            foreach (string name in Properties.Settings.Default.RecFls)
            {
                richTextBox1.Text += Path.GetFileName(name) + "\n";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RemRecOpnFls = checkBox1.Checked;
        }

        private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            richTextBox1.Text = "";
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.RecFls.Count > 0)
            {
                PastForm.recentFilesToolStripMenuItem.DropDownItems.Clear();
                Properties.Settings.Default.RecFls.Clear();
                richTextBox1.Text = "";
            }
        }

        private MainForm _PastForm;
        private MainForm PastForm
        {
            get { return _PastForm; }
            set { _PastForm = value; }
        }
    }
}