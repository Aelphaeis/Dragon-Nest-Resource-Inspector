using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DragonNest.ResourceInspector.Pak;
using System.Diagnostics;

using Guifreaks.NavigationBar;
using WeifenLuo.WinFormsUI.Docking;
namespace DragonNest.ResourceInspector.Pak.Viewer
{
    public partial class AmalgamatedPakViewer : DockContent
    {
        public const double DefaultRelevance = 50D;
        public const double DefaultLimit = 50D;

        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                if (status == value)
                    return;
                status = value;
                if (StatusChangedEvent != null)
                    StatusChangedEvent(this, EventArgs.Empty);
            }
        }


        public event EventHandler StatusChangedEvent;
        Dictionary<String, IHeader> files;
        ListViewColumnSorter lvwColumnSorter;
        int status;

        public AmalgamatedPakViewer()
        {
            InitializeComponent();
            SearchBox.Width = 500;
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
        }

        //warning amalgation mode is not used at all
        public AmalgamatedPakViewer LoadPaks(IEnumerable<Stream> streams, AmalgationMode Mode)
        {
            //If there are no streams then this is a pointless window
            if (streams.Count() == 0)
                return this;

            var paks = new List<PakFile>();

            //Load all Pak's from streams
            Parallel.ForEach(streams, (s) => paks.Add(new PakFile().LoadPak(s)));

            //Determine the name of all Paks
            foreach (var stream in streams)
                if (stream is FileStream)
                    toolStripStatusLabel1.Text += ((FileStream)stream).Name + ";";


            //order pak files and iterate through them
            if (paks.Count() > 1)
                files = PakFile.Merge(paks);
            else
                files = paks.First().Files;

            RefreshPakTree();
            return this;
        }

        void RefreshPakTree()
        {
            //To stop graphical inconsistency
            PakTree.SuspendLayout();
            PakTree.Nodes.Clear();

            var tree = CreateTree(files);
            for (int i = 0; i < tree.Nodes.Count; i++)
                PakTree.Nodes.Add(tree.Nodes[i]);

            PakTree.ResumeLayout();
        }

        TreeNode CreateTree(Dictionary<String, IHeader> files)
        {
            TreeNode n = new TreeNode();
            foreach (var file in files.Values)
            {
                if (file is FolderHeader)
                {
                    TreeNode tn = CreateTree((file as FolderHeader).Files);
                    tn.Name = file.Path;
                    tn.Text = file.Name;
                    tn.ImageIndex = tn.SelectedImageIndex = 0;
                    n.Nodes.Add(tn);
                }
            }
            return n;
        }

        public void DisplayInListView(FolderHeader folderHeader)
        {
            listView1.Items.Clear();
            
            ListView.ListViewItemCollection items = new ListView.ListViewItemCollection(listView1);
            foreach (IHeader header in folderHeader.Files.Values)
            {
                if (header is FolderHeader)
                {
                    ListViewItem item = new ListViewItem(header.Name, 0) { Name = header.Path };
                    items.Add(item);
                }
                else
                {
                    var fileHeader = header as FileHeader;
                    ListViewItem item = new ListViewItem(header.Name, 2) { Name = header.Path };
                    item.SubItems.Add(fileHeader.OriginalSize.ToString());
                    item.SubItems.Add(fileHeader.CompressedSize.ToString());
                    items.Add(item);

                }
            }

            toolStripTextBox1.Text = folderHeader.Path;
        }

        public AmalgamatedPakViewer LoadPaks(IEnumerable<Stream> streams)
        {
            return LoadPaks(streams, AmalgationMode.Ordinal);
        }

        private void naviBar1_Resize(object sender, EventArgs e)
        {
            var obj = ((NaviBar)sender);
            splitContainer1.IsSplitterFixed = obj.Collapsed;
            splitContainer1.SplitterDistance = obj.Size.Width;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (sender == listView1)
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    String nextSelected = listView1.SelectedItems[0].Name;
                    try { 
                        var header = FolderHeader.Find(files, nextSelected);
                        DisplayInListView(header as FolderHeader);
                        }
                    catch{
                       ;
                        ExternOpen(FolderHeader.Find(files, nextSelected) as FileHeader);
                    }
                }
            }
        }


        private void PakTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var path = e.Node.Name;
            FolderHeader.Find(files, path);
        }

        private void PakTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count != 0)
            {
                e.Node.ImageIndex = 1;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void PakTree_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count != 0)
            {
                e.Node.ImageIndex = 0;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void PakTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var headerPath = e.Node.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);

            DisplayInListView(FolderHeader.Find(files, e.Node.Name) as FolderHeader);
        }

        void ExternOpen(FileHeader header)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Directory.CreateDirectory(appData + @"\Dragon Nest Resource Inspector\");
            var appDataLocation = appData + @"\Dragon Nest Resource Inspector\" + header;
            using (var fs = new FileStream(appDataLocation, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            using (var hs = header.GetStream())
            {
                hs.CopyTo(fs);
                Process.Start(appDataLocation);
            }
        }
        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (PakTree.SelectedNode == null)
                return;

            using (var fbd = new FolderBrowserDialog())
                if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    FolderHeader.Find(files, PakTree.SelectedNode.Name).CopyToFileSystem(fbd.SelectedPath);
        }


        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
                if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    new FolderHeader(){Name = @"\", Files = files}.CopyToFileSystem(fbd.SelectedPath);
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }

    }
}
