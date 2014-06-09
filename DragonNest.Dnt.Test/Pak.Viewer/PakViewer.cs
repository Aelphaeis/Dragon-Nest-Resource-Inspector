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
using DragonNest.ResourceInspection.Pak;
using DragonNest.ResourceInspector.Pak;

using Guifreaks.NavigationBar;
namespace DragonNest.ResourceInspection.Pak.Viewer
{
    public partial class PakViewer : Form
    {
        Stream pakStream;
        PakFile pakFile;
        public PakViewer()
        {
            InitializeComponent();
            toolStripTextBox1.Dock = DockStyle.Fill;
            
        }

        private void PakViewer_Load(object sender, EventArgs e)
        {
            toolStripTextBox1.Width = toolStrip1.Size.Width - 4;
            toolStrip1.SizeChanged += (s, a) => toolStripTextBox1.Width = toolStrip1.Size.Width - 4;
        }
        public void LoadPakStream(Stream stream)
        {
            if (pakStream != null)  pakStream.Close();
            pakFile = new PakFile(pakStream = stream);

            RefreshPakTree();
        }

        void RefreshPakTree()
        {
            //To stop graphical inconsistency
            PakTree.SuspendLayout();
            PakTree.Nodes.Clear();
            foreach(var file in pakFile.Files)
            {
                var pathComponents = file.FileName.Split(new char [] {'\\'},StringSplitOptions.RemoveEmptyEntries);
                var Nodes = PakTree.Nodes;
                foreach(var v in pathComponents)
                {
                    if (!Nodes.ContainsKey(v))
                        Nodes.Add(new TreeNode(v) { Name = v });
                    var next = Nodes.Find(v, false).First();
                    Nodes = next.Nodes;
                }
            }
            //To update the Graphics
            PakTree.ResumeLayout();
        }

        void RefreshDetailView()
        {

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
        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            naviBar1.Width = splitContainer1.SplitterDistance;
        }

        private void PakViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pakStream != null)
                pakStream.Close();
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
            for (int i = 0; i <= e.Node.Level; i++, path = path.Insert(0,@"\"))
            {
                path = path.Insert(0, Node.Text);
                Node = Node.Parent;
            }
            toolStripTextBox1.Text = path;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (sender == listView1)
                if (listView1.SelectedItems.Count == 1)
                {
                    var Nodes = PakTree.Nodes;
                    var path = toolStripTextBox1.Text + @"\" + listView1.SelectedItems[0].Text;
                    var pathComponents = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach(var v in pathComponents)
                        Nodes = Nodes.Find(v, false).First().Nodes;
                    
                    if (Nodes.Count == 0)
                        return;

                    Nodes[0].TreeView.SelectedNode = Nodes[0];
                    PakTree_AfterSelect(PakTree, new TreeViewEventArgs(Nodes[0].Parent));
                } 
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case MouseButtons.Right:
                    if(listView1.SelectedItems.Count == 0)
                        return;
                    //MessageBox.Show(listView1.SelectedItems[0].Text);
                    break;
            }
        }

        //void RefreshPakTree()
        //{
        //    //To stop graphical inconsistency
        //    PakTree.SuspendLayout();
        //    PakTree.Nodes.Clear();
        //    foreach (var file in pakFile.Files)
        //    {
        //        var pathComponents = file.FileName.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
        //        var Nodes = PakTree.Nodes;
        //        foreach (var v in pathComponents)
        //        {
        //            if (!Nodes.ContainsKey(v))
        //                Nodes.Add(new TreeNode(v) { Name = v });
        //            var next = Nodes.Find(v, false).First();
        //            Nodes = next.Nodes;
        //        }
        //    }
        //    //To update the Graphics
        //    PakTree.ResumeLayout();
        //}
    }
}
