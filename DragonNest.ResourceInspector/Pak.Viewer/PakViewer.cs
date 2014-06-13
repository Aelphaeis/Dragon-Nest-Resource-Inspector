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
    public partial class PakViewer : DockContent
    {
        public event EventHandler StatusChanged; 
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (StatusChanged != null)
                    StatusChanged(this, EventArgs.Empty); 
            }
        }
        int status;


        Stream pakStream;
        PakFile pakFile;
        public PakViewer()
        {
            InitializeComponent();
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

        string GetPath(TreeNode n, bool slash = false)
        {
            if (n == null)
                return String.Empty;
            else
                return GetPath(n.Parent, true) +  @"\" + n.Name;
        }
     
        public void LoadPakStream(Stream stream) 
        {
            Status = 0;
            if (pakStream != null)  
                pakStream.Close();

            Status = 5;

            if (stream is FileStream) { 
                Text = ((FileStream)stream).Name.Split('\\').Last();
                toolStripStatusLabel1.Text = ((FileStream)stream).Name;
            }
            pakFile = new PakFile(pakStream = stream);
            RefreshPakTree();
        }
        void RefreshPakTree()
        {
            //To stop graphical inconsistency
            PakTree.SuspendLayout();
            PakTree.Nodes.Clear();

            for (int i = 0; i < pakFile.Header.FileCount; i++)
            {
                var file = pakFile.Files[i];

                Status = Convert.ToInt32(Decimal.Divide(i, pakFile.Header.FileCount) * 95 + 5);

                var pathComponents = file.Path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                var Nodes = PakTree.Nodes;

                for (int count = 0; count < pathComponents.Length; count++)
                {
                    if (!Nodes.ContainsKey(pathComponents[count]))
                    {
                        TreeNode tn = new TreeNode(pathComponents[count]);
                        tn.Name = pathComponents[count];
                        tn.ImageIndex = tn.SelectedImageIndex= ((count == pathComponents.Length - 1) ? 2 : 0);
                        Nodes.Add(tn);
                    }
                    var next = Nodes.Find(pathComponents[count], false).First();
                    Nodes = next.Nodes;
                }
            }
            //To update the Graphics
            PakTree.ResumeLayout();
        }


        #region EventHandlers
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
                    
                    foreach(var v in pathComponents)
                        Nodes = Nodes.Find(v, false).First().Nodes;

                    if (Nodes.Count == 0)
                        ExternOpen(pakFile.Files.First(p => p.Path == path));
                    else
                    {
                        Nodes[0].TreeView.SelectedNode = Nodes[0];
                        PakTree_AfterSelect(PakTree, new TreeViewEventArgs(Nodes[0].Parent));
                    }
                } 
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                { 
                    if (listView1.SelectedItems.Count == 0)
                        return;
                    var Nodes = PakTree.Nodes;
                    var path = toolStripTextBox1.Text + @"\" + listView1.SelectedItems[0].Text;
                    var pathComponents = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var v in pathComponents)
                        Nodes = Nodes.Find(v, false).First().Nodes;

                    if (listView1.SelectedItems[0].Text.EndsWith(".dnt"))
                        contextMenuStrip1.Show(e.X, e.Y);

                    break;
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
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
            if(e.Node.Nodes.Count != 0)
            { 
                e.Node.ImageIndex = 0;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
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
            for (int i = 0; i <= e.Node.Level; i++, path = path.Insert(0, @"\"))
            {
                path = path.Insert(0, Node.Text);
                Node = Node.Parent;
            }
            toolStripTextBox1.Text = path;
        }

        private void PakTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                var path = GetPath(e.Node);
                ExternOpen(pakFile.Files.First(p => p.Path == path));
            }
        }
        #endregion
    }
}
