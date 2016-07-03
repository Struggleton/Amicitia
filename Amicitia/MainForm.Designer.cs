﻿namespace Amicitia
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aDXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTreeView = new System.Windows.Forms.TreeView();
            this.mainPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.glControl1 = new OpenTK.GLControl();
            this.mainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.testingToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(779, 24);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.recentFilesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // recentFilesToolStripMenuItem
            // 
            this.recentFilesToolStripMenuItem.Name = "recentFilesToolStripMenuItem";
            this.recentFilesToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.recentFilesToolStripMenuItem.Text = "Recent Files";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // testingToolStripMenuItem
            // 
            this.testingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aDXToolStripMenuItem,
            this.audioViewerToolStripMenuItem});
            this.testingToolStripMenuItem.Name = "testingToolStripMenuItem";
            this.testingToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.testingToolStripMenuItem.Text = "Testing";
            // 
            // aDXToolStripMenuItem
            // 
            this.aDXToolStripMenuItem.Name = "aDXToolStripMenuItem";
            this.aDXToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aDXToolStripMenuItem.Text = "ADX";
            this.aDXToolStripMenuItem.Click += new System.EventHandler(this.aDXToolStripMenuItem_Click_1);
            // 
            // audioViewerToolStripMenuItem
            // 
            this.audioViewerToolStripMenuItem.Name = "audioViewerToolStripMenuItem";
            this.audioViewerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.audioViewerToolStripMenuItem.Text = "AudioViewer";
            this.audioViewerToolStripMenuItem.Click += new System.EventHandler(this.audioViewerToolStripMenuItem_Click);
            // 
            // mainTreeView
            // 
            this.mainTreeView.AllowDrop = true;
            this.mainTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainTreeView.Location = new System.Drawing.Point(12, 24);
            this.mainTreeView.MaximumSize = new System.Drawing.Size(400, 900);
            this.mainTreeView.Name = "mainTreeView";
            this.mainTreeView.Size = new System.Drawing.Size(300, 551);
            this.mainTreeView.TabIndex = 1;
            this.mainTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.mainTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            // 
            // mainPropertyGrid
            // 
            this.mainPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPropertyGrid.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.mainPropertyGrid.HelpVisible = false;
            this.mainPropertyGrid.Location = new System.Drawing.Point(319, 0);
            this.mainPropertyGrid.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.mainPropertyGrid.Name = "mainPropertyGrid";
            this.mainPropertyGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.mainPropertyGrid.Size = new System.Drawing.Size(448, 277);
            this.mainPropertyGrid.TabIndex = 2;
            this.mainPropertyGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.mainPropertyGrid.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPictureBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPictureBox.Enabled = false;
            this.mainPictureBox.Location = new System.Drawing.Point(318, 283);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(449, 292);
            this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mainPictureBox.TabIndex = 3;
            this.mainPictureBox.TabStop = false;
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(318, 283);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(448, 292);
            this.glControl1.TabIndex = 4;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 587);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.mainPictureBox);
            this.Controls.Add(this.mainPropertyGrid);
            this.Controls.Add(this.mainTreeView);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MinimumSize = new System.Drawing.Size(662, 569);
            this.Name = "MainForm";
            this.Text = "Amicitia 16/2/2016";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.TreeView mainTreeView;
        private System.Windows.Forms.PropertyGrid mainPropertyGrid;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aDXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioViewerToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem recentFilesToolStripMenuItem;
    }
}

