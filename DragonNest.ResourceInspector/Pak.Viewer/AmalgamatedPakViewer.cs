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
        Dictionary<String, FileHeader> files;
        public AmalgamatedPakViewer()
        {
            InitializeComponent();
        }

        public AmalgamatedPakViewer LoadPaks(IEnumerable<Stream> streams, AmalgationMode Mode)
        {
            files = new Dictionary<string, FileHeader>();

            var paks = new List<PakFile>();
            Parallel.ForEach(streams, (s) => paks.Add(new PakFile(s)));
            foreach (PakFile p in paks.OrderBy(p => p.Name))
                foreach(FileHeader f in p.Files)
                    if(files.ContainsKey(f.Path))
                        files[f.Path] = f;
                    else
                        files.Add(f.Path, f);

            RefreshPakTree();
            return this;
        }

        void RefreshPakTree()
        {
            //To stop graphical inconsistency
            PakTree.SuspendLayout();
            PakTree.Nodes.Clear();

            foreach (var file in files.Values)
            {
                var pathComponents = file.Path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var Nodes = PakTree.Nodes;

                for (int count = 0; count < pathComponents.Length; count++)
                {
                    if (!Nodes.ContainsKey(pathComponents[count]))
                    {
                        TreeNode tn = new TreeNode(pathComponents[count]);
                        tn.Name = pathComponents[count];
                        tn.ImageIndex = tn.SelectedImageIndex = ((count == pathComponents.Length - 1) ? 2 : 0);
                        Nodes.Add(tn);
                    }
                    var next = Nodes.Find(pathComponents[count], false).First();
                    Nodes = next.Nodes;
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
            if (obj.Collapsed)
            {
                splitContainer1.SplitterDistance = obj.Size.Width;
                splitContainer1.IsSplitterFixed = true;
            }
            else
            {
                splitContainer1.IsSplitterFixed = false;
                splitContainer1.SplitterDistance = obj.Size.Width;
            }
        }

        private void PakViewer_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Width = toolStrip1.Size.Width - 4;
            toolStrip1.SizeChanged += (s, a) => toolStripTextBox1.Width = toolStrip1.Size.Width - 4;
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (sender == listView1)
                if (listView1.SelectedItems.Count == 1)
                {
                    var Nodes = PakTree.Nodes;
                    var path = toolStripTextBox1.Text + @"\" + listView1.SelectedItems[0].Text;
                    var pathComponents = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var v in pathComponents)
                        Nodes = Nodes.Find(v, false).First().Nodes;

                    
                    if (Nodes.Count == 0)
                        ExternOpen(files.Values.First(p => p.Path == path));
                    else
                    {
                        Nodes[0].TreeView.SelectedNode = Nodes[0];
                        PakTree_AfterSelect(PakTree, new TreeViewEventArgs(Nodes[0].Parent));
                    }
                }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {

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
            if (e.Node.Nodes.Count == 0)
                return;

            listView1.Items.Clear();
            foreach (TreeNode node in e.Node.Nodes)
            {
                ListViewItem item = new ListViewItem(node.Name);
                listView1.Items.Add(item);
            }

            var path = String.Empty;
            var Node = e.Node;
            for (int i = 0; i <= e.Node.Level; i++, path = path.Insert(0, @"\"))
            {
                path = path.Insert(0, Node.Text);
                Node = Node.Parent;
            }
            toolStripTextBox1.Text = path;
        }
        void ExternOpen(FileHeader header)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDataLocation = appData + @"\" + header;
            using (var fs = new FileStream(appDataLocation, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            using (var hs = header.GetStream())
            {
                hs.CopyTo(fs);
                Process.Start(appDataLocation);
            }
        }
    }
}
