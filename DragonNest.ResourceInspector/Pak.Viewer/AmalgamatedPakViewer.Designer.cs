namespace DragonNest.ResourceInspector.Pak.Viewer
{
    partial class AmalgamatedPakViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmalgamatedPakViewer));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openInInspectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SearchButton = new System.Windows.Forms.ToolStripButton();
            this.SearchBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.LimitBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.RelevanceBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.naviBar1 = new Guifreaks.NavigationBar.NaviBar(this.components);
            this.naviBand1 = new Guifreaks.NavigationBar.NaviBand(this.components);
            this.PakTree = new System.Windows.Forms.TreeView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.ExportButton = new System.Windows.Forms.ToolStripButton();
            this.ExportAllButton = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.naviBar1)).BeginInit();
            this.naviBar1.SuspendLayout();
            this.naviBand1.ClientArea.SuspendLayout();
            this.naviBand1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInInspectorToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(169, 26);
            // 
            // openInInspectorToolStripMenuItem
            // 
            this.openInInspectorToolStripMenuItem.Name = "openInInspectorToolStripMenuItem";
            this.openInInspectorToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.openInInspectorToolStripMenuItem.Text = "Open In Inspector";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Icontexto-Elite-Folders-Close-Folder.ico");
            this.imageList1.Images.SetKeyName(1, "Icontexto-Elite-Folders-Open-Folder.ico");
            this.imageList1.Images.SetKeyName(2, "defult-text.ico");
            // 
            // SearchButton
            // 
            this.SearchButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SearchButton.Image = ((System.Drawing.Image)(resources.GetObject("SearchButton.Image")));
            this.SearchButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(23, 22);
            this.SearchButton.Text = "Search";
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchBox
            // 
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SearchBox.Size = new System.Drawing.Size(250, 25);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBox1.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LimitBox,
            this.toolStripLabel2,
            this.RelevanceBox,
            this.toolStripLabel1,
            this.SearchBox,
            this.SearchButton,
            this.toolStripTextBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(995, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // LimitBox
            // 
            this.LimitBox.Name = "LimitBox";
            this.LimitBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LimitBox.Size = new System.Drawing.Size(45, 25);
            this.LimitBox.Validating += new System.ComponentModel.CancelEventHandler(this.toolStripTextBox2_Validating);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(34, 22);
            this.toolStripLabel2.Text = "Limit";
            // 
            // RelevanceBox
            // 
            this.RelevanceBox.Name = "RelevanceBox";
            this.RelevanceBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RelevanceBox.Size = new System.Drawing.Size(45, 25);
            this.RelevanceBox.Validating += new System.ComponentModel.CancelEventHandler(this.toolStripTextBox3_Validating);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabel1.Text = "Relevance";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.naviBar1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(995, 480);
            this.splitContainer1.SplitterDistance = 163;
            this.splitContainer1.TabIndex = 10;
            // 
            // naviBar1
            // 
            this.naviBar1.ActiveBand = this.naviBand1;
            this.naviBar1.Controls.Add(this.naviBand1);
            this.naviBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.naviBar1.Location = new System.Drawing.Point(0, 0);
            this.naviBar1.Name = "naviBar1";
            this.naviBar1.Size = new System.Drawing.Size(163, 480);
            this.naviBar1.TabIndex = 0;
            this.naviBar1.Text = "Pak Explorer";
            this.naviBar1.Resize += new System.EventHandler(this.naviBar1_Resize);
            // 
            // naviBand1
            // 
            // 
            // naviBand1.ClientArea
            // 
            this.naviBand1.ClientArea.Controls.Add(this.PakTree);
            this.naviBand1.ClientArea.Controls.Add(this.toolStrip2);
            this.naviBand1.ClientArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.naviBand1.ClientArea.Location = new System.Drawing.Point(0, 0);
            this.naviBand1.ClientArea.Name = "ClientArea";
            this.naviBand1.ClientArea.Size = new System.Drawing.Size(161, 413);
            this.naviBand1.ClientArea.TabIndex = 0;
            this.naviBand1.Location = new System.Drawing.Point(1, 27);
            this.naviBand1.Name = "naviBand1";
            this.naviBand1.Size = new System.Drawing.Size(161, 413);
            this.naviBand1.TabIndex = 3;
            this.naviBand1.Text = "Pak Explorer";
            // 
            // PakTree
            // 
            this.PakTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PakTree.ImageIndex = 0;
            this.PakTree.ImageList = this.imageList1;
            this.PakTree.Location = new System.Drawing.Point(0, 25);
            this.PakTree.Name = "PakTree";
            this.PakTree.SelectedImageIndex = 0;
            this.PakTree.ShowLines = false;
            this.PakTree.ShowPlusMinus = false;
            this.PakTree.ShowRootLines = false;
            this.PakTree.Size = new System.Drawing.Size(161, 388);
            this.PakTree.TabIndex = 1;
            this.PakTree.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.PakTree_BeforeCollapse);
            this.PakTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.PakTree_BeforeExpand);
            this.PakTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.PakTree_AfterSelect);
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExportButton,
            this.ExportAllButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(161, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // ExportButton
            // 
            this.ExportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExportButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportButton.Image")));
            this.ExportButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(23, 22);
            this.ExportButton.Text = "Export Selected";
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ExportAllButton
            // 
            this.ExportAllButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ExportAllButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportAllButton.Image")));
            this.ExportAllButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExportAllButton.Name = "ExportAllButton";
            this.ExportAllButton.Size = new System.Drawing.Size(23, 22);
            this.ExportAllButton.Text = "Extract All";
            this.ExportAllButton.Click += new System.EventHandler(this.ExportAllButton_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(828, 458);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 422;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Original Size";
            this.columnHeader2.Width = 112;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Compressed Size";
            this.columnHeader3.Width = 121;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 458);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(828, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // AmalgamatedPakViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 505);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AmalgamatedPakViewer";
            this.Text = "PakViewer";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.naviBar1)).EndInit();
            this.naviBar1.ResumeLayout(false);
            this.naviBand1.ClientArea.ResumeLayout(false);
            this.naviBand1.ClientArea.PerformLayout();
            this.naviBand1.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openInInspectorToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripButton SearchButton;
        private System.Windows.Forms.ToolStripTextBox SearchBox;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox LimitBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox RelevanceBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Guifreaks.NavigationBar.NaviBar naviBar1;
        private Guifreaks.NavigationBar.NaviBand naviBand1;
        private System.Windows.Forms.TreeView PakTree;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton ExportButton;
        private System.Windows.Forms.ToolStripButton ExportAllButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}