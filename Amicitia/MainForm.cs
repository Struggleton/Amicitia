using System;
using System.Windows.Forms;
using System.IO;
using Amicitia.ResourceWrappers;
using AtlusLibSharp.Utilities;
using AtlusLibSharp.Graphics;
using OpenTK;
using System.Runtime.InteropServices;
using Amicitia.ModelViewer;
using AtlusLibSharp.Graphics.RenderWare;
using AtlusLibSharp.Audio;
using System.Drawing;
using System.Reflection;

namespace Amicitia
{
    public partial class MainForm : Form
    {
        private static MainForm _instance;
        private ModelViewer.ModelViewer viewer;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        internal static MainForm Instance
        {
            get { return _instance; }
            set
            {
                if (_instance != null) { throw new Exception("Instance already exists!"); }
                _instance = value;
            }
        }

        internal TreeView MainTreeView
        {
            get { return mainTreeView; }
        }

        internal PropertyGrid MainPropertyGrid
        {
            get { return mainPropertyGrid; }
        }

        internal PictureBox MainPictureBox
        {
            get { return mainPictureBox; }
        }

        public MainForm()
        {
            InitializeComponent();
            InitializeMainForm();

            #if DEBUG
                AllocConsole();
            #endif
        }

        private void AddToList(string text)
        {
            if (Properties.Settings.Default.RecFls.IndexOf(text) == -1)
                Properties.Settings.Default.RecFls.Add(text);
        }

        private void AddItemDropDown(string text)
        {
            recentFilesToolStripMenuItem.DropDownItems.Add(Path.GetFileName(text));
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (filePaths.Length > 0) HandleFileOpenFromPath(filePaths[0]);
        }

        private void MainTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            mainTreeView.LabelEdit = false;
        }

        private void MainTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) HandleTreeViewCtrlShortcuts(e.KeyData);
        }

        private void MainTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            mainPictureBox.Visible = false;
            viewer.DeleteScene();
            glControl1.Visible = false;
            ResourceWrapper res = mainTreeView.SelectedNode as ResourceWrapper;

            mainPropertyGrid.SelectedObject = res;
            if (res == null) return;

            if(res.IsModelResource == true)
            {
                try { viewer.LoadScene((res as ResourceWrapper).WrappedObject as RMDScene); }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
                glControl1.Visible = true;
            }

            if (res.IsImageResource == true)
            {
                mainPictureBox.Visible = true;
                mainPictureBox.Image = ((ITextureFile)res.WrappedObject).GetBitmap();
            }
        }

        private void MainTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            mainTreeView.SelectedNode = e.Node;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDlg = new OpenFileDialog())
            {
                openFileDlg.Filter = SupportedFileHandler.FileFilter;
                if (openFileDlg.ShowDialog() != DialogResult.OK) return;
                HandleFileOpenFromPath(openFileDlg.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTreeView.Nodes.Count > 0) ((ResourceWrapper)mainTreeView.TopNode).Export(sender, e);
        }

        internal void UpdateReferences()
        {
            MainTreeView_AfterSelect(this, new TreeViewEventArgs(mainTreeView.SelectedNode));
        }

        private void InitializeMainForm()
        {
            DateTime time = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location);
            Text = string.Format("Amicitia [{0}/{1}/{2}] [DEBUG]", time.Month, time.Day, time.Year);

            Instance = this;
            mainTreeView.AfterSelect += MainTreeView_AfterSelect;
            mainTreeView.KeyDown += MainTreeView_KeyDown;
            mainTreeView.AfterLabelEdit += MainTreeView_AfterLabelEdit;
            mainTreeView.NodeMouseClick += MainTreeView_NodeMouseClick;
            mainPictureBox.Visible = false;
            mainPropertyGrid.PropertySort = PropertySort.NoSort;

            foreach (string name in Properties.Settings.Default.RecFls)
            {
                AddItemDropDown(name);
            }
        }

        private void HandleTreeViewCtrlShortcuts(Keys keys)
        {
            ResourceWrapper res = (ResourceWrapper)mainTreeView.SelectedNode;

            if (keys.HasFlagUnchecked(Keys.Up))
            {
                if (res.CanMove) res.MoveUp(this, EventArgs.Empty);
            }

            else if (keys.HasFlagUnchecked(Keys.Down))
            {
                if (res.CanMove) res.MoveDown(this, EventArgs.Empty);
            }

            else if (keys.HasFlagUnchecked(Keys.Delete))
            {
                if (res.CanDelete) res.Delete(this, EventArgs.Empty);
            }

            else if (keys.HasFlagUnchecked(Keys.R))
            {
                if (res.CanReplace) res.Replace(this, EventArgs.Empty);
            }

            else if (keys.HasFlagUnchecked(Keys.E))
            {
                if (res.CanRename) res.Export(this, EventArgs.Empty);
            }
        }

        private void HandleFileOpenFromPath(string filePath)
        {
            if (viewer.IsSceneReady == true) viewer.DeleteScene();
            int supportedFileIndex = SupportedFileHandler.GetSupportedFileIndex(filePath);
            if (supportedFileIndex == -1) return;
            if (mainPictureBox.Visible == true) mainPictureBox.Visible = false;
            if (mainTreeView.Nodes.Count > 0)
                mainTreeView.Nodes.Clear();
            if (Properties.Settings.Default.RemRecOpnFls)
            {
                AddToList(filePath);
                AddItemDropDown(Path.GetFileName(filePath));
            }

            TreeNode treeNode = null;

            #if !DEBUG
            try
            {
            #endif

            treeNode = 
                ResourceFactory.GetResource(Path.GetFileName(filePath), File.OpenRead(filePath), supportedFileIndex);
            
            #if !DEBUG
            }
            catch (InvalidDataException exception)
            {
                MessageBox.Show("Data was not in expected format, can't open file.\n Stacktrace:\n" + exception.StackTrace, "Open file error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            #endif

            mainTreeView.BeginUpdate();
            mainTreeView.Nodes.Add(treeNode);
            mainTreeView.SelectedNode = mainTreeView.TopNode;
            mainTreeView.EndUpdate();
        }


        private void glControl1_Load(object sender, EventArgs e)
        {
            glControl1.Visible = false;
            viewer = new ModelViewer.ModelViewer(glControl1);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
            viewer.DisposeViewer();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new OptionsForm(this).ShowDialog(this);
        }

        private void aDXToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //ADXFile adx = new ADXFile(File.OpenRead(@"BOKO.ADX"));
        }

        private void audioViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AudioViewer.AudioPlayer(new System.Media.SoundPlayer()).Show();
        }
    }
}
