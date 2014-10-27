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
                if(status == value) 
                    return; 
                status = value;
                if (StatusChangedEvent != null)
                    StatusChangedEvent(this, EventArgs.Empty); 
            } 
        }


        public event EventHandler StatusChangedEvent;
        Dictionary<String, FileHeader> files;
        ListViewColumnSorter lvwColumnSorter;
        int status;

        public AmalgamatedPakViewer()
        {
            InitializeComponent();
            SearchBox.Width = 500;
            RelevanceBox.Text = DefaultRelevance.ToString();
            LimitBox.Text = DefaultLimit.ToString();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
        }

        //warning amalgation mode is not used at all
        public AmalgamatedPakViewer LoadPaks(IEnumerable<Stream> streams, AmalgationMode Mode)
        {
            int count = 0;
            double total = 0;
            var paks = new List<PakFile>();
            files = new Dictionary<string, FileHeader>();
            Parallel.ForEach(streams, (s) => paks.Add(new PakFile(s)));

            foreach (var v in streams)
                if (v is FileStream)
                    toolStripStatusLabel1.Text += ((FileStream)v).Name + ";";

            status = 5;
            paks.ForEach(p => total += p.Files.Count);

            foreach (PakFile p in paks.OrderBy(p => p.Name))
                for (int i = 0; i < p.Files.Count; i++, Status = 5 + Convert.ToInt32(++count/total * 45))
                    if (files.ContainsKey(p.Files[i].Path))
                        files[p.Files[i].Path] = p.Files[i];
                    else
                        files.Add(p.Files[i].Path, p.Files[i]);

            RefreshPakTree();
            return this;
        }

        void RefreshPakTree()
        {
            //To stop graphical inconsistency
            PakTree.SuspendLayout();

            int progress = 0;
            PakTree.Nodes.Clear();
            double total = files.Count;
            foreach (var file in files.Values)
            {
                var pathComponents = file.Path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var Nodes = PakTree.Nodes;
                Status = 50 + Convert.ToInt32(++ progress/ total * 50);
                for (int count = 0; count < pathComponents.Length; count++) 
                {
                    if (!Nodes.ContainsKey(pathComponents[count]))
                    {
                        TreeNode tn = new TreeNode(pathComponents[count]) { Name = pathComponents[count] };
                        tn.ImageIndex = tn.SelectedImageIndex = ((count == pathComponents.Length - 1) ? 2 : 0);
                        Nodes.Add(tn);
                    }
                    Nodes = Nodes.Find(pathComponents[count], false).First().Nodes;
                }
            }
            //To update the Graphics
            PakTree.ResumeLayout();
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
                    var Nodes = PakTree.Nodes;
                    var path = String.IsNullOrEmpty(toolStripTextBox1.Text)? String.Empty : toolStripTextBox1.Text + @"\" ;
                    path += listView1.SelectedItems[0].Text;
                    var pathComponents = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var v in pathComponents)
                        Nodes = Nodes.Find(v, false).First().Nodes;

                    if (Nodes.Count == 0)
                        ExternOpen(files.Values.First(p => p.Path == path));
                    else
                        PakTree_AfterSelect(PakTree, new TreeViewEventArgs((Nodes[0].TreeView.SelectedNode = Nodes[0]).Parent));
                }
            }
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

        List<BackgroundWorker> pakTreeWorkers = new List<BackgroundWorker>();
        private void PakTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if nothing is selected then no contents to show.
            if (e.Node.Nodes.Count == 0)
                return;

            //put the path of the new node in the text box
            var Node = e.Node;
            var path = String.Empty;
            for (int i = 0; i <= e.Node.Level; i++, path = path.Insert(0, @"\"), Node = Node.Parent)
                path = path.Insert(0, Node.Text);

            //If they are selected the same folder no need to reload and 
            //if you did you'd have problems with getting duplicates of files
            if(String.Equals(path, toolStripTextBox1.Text))
                return;

            //no need to continue loading if someone changed folder.
            pakTreeWorkers.ForEach(p => p.CancelAsync());
            pakTreeWorkers.Clear();

            //set the new path
            toolStripTextBox1.Text = path;
   
            //anything currently being shown is now out of sync with what is selected
            listView1.Items.Clear();

            //go through what is in the tree add children nodes to list view 
            foreach (TreeNode node in e.Node.Nodes){

                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.WorkerSupportsCancellation = true;

                //keep track of background workers 
                pakTreeWorkers.Add(backgroundWorker);

                backgroundWorker.DoWork += (s, args) =>
                    {
                        var lvi = new ListViewItem(node.Name, node.ImageIndex);
                        //if the node is not a folder then it is a file
                        if (node.Nodes.Count == 0)
                        {
                            //get and display the original size and compressed size of file
                            lvi.SubItems.Add(files[@"\" + node.FullPath].OriginalSize.ToString());
                            lvi.SubItems.Add(files[@"\" + node.FullPath].CompressedSize.ToString());
                        }
                        
                        // so if someone changes the folder then you're not screwed.
                        if (String.Equals(path, toolStripTextBox1.Text))
                            listView1.Invoke(new MethodInvoker(() => listView1.Items.Add(lvi)));
                    };

                backgroundWorker.RunWorkerAsync();
            }
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
                    Export(PakTree.SelectedNode, fbd.SelectedPath);

        }

        private void Export(TreeNode node, String Location)
        {
            if (node.GetNodeCount(false) == 0)
                using (var fs = new FileStream(Location, FileMode.CreateNew))
                using (var stream = files[@"\" + node.FullPath].GetStream())
                {
                    stream.CopyTo(fs);
                    stream.Close();
                    stream.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
            else
                Directory.CreateDirectory(Location += @"\" + node.Name);

            foreach (TreeNode n in node.Nodes)
                Export(n, Location + @"\" + n.Name);
        }

        private void ExportAllButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
                if (fbd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    Parallel.ForEach(PakTree.Nodes.Cast<TreeNode>(), p => Export(p, fbd.SelectedPath));
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            listView1.Items.Clear();
            toolStripTextBox1.Clear();
            double relevance = Double.Parse(RelevanceBox.Text) / 100;
            int limit  = Int32.Parse(LimitBox.Text);
            var results = files.Select(p => new
            {
                Value = p,
                Revelance = Math.Max(Levenshtein.Percentage(p.Key, SearchBox.Text),
                    Levenshtein.Percentage(p.Value.Name, SearchBox.Text))
            }).OrderByDescending(p => p.Revelance).Where(p => p.Revelance > relevance).Select(p => p.Value.Value);


            foreach (var result in results.Where((p, i) => i < limit))
            {
                var lvi = new ListViewItem(result.Path, 2);
                lvi.SubItems.Add(result.OriginalSize.ToString());
                lvi.SubItems.Add(result.CompressedSize.ToString());
                listView1.Items.Add(lvi);
            }

            ResumeLayout();
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

        private void toolStripTextBox3_Validating(object sender, CancelEventArgs e)
        {
            Double d = new double();
            if (!Double.TryParse(RelevanceBox.Text, out d) || d > 100 || d < 0)
            {
                MessageBox.Show("Relevance must be a valid number bettwen 0 and 100");
                RelevanceBox.Text = DefaultRelevance.ToString();
            }
        }

        private void toolStripTextBox2_Validating(object sender, CancelEventArgs e)
        {
            int i = new int();
            if (!Int32.TryParse(LimitBox.Text, out i) || i < 0)
            {
                MessageBox.Show("Limit must be a valid Integer greater than 0");           
                LimitBox.Text = DefaultLimit.ToString();
            }
        }

    }
}
