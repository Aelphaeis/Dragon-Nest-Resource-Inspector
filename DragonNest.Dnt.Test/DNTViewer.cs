using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Guifreaks.NavigationBar;
namespace DragonNest.ResourceInspection.dnt.Test
{
    public partial class DNTViewer : DockContent
    {
        DragonNestDataTable dnt;

        public DNTViewer()
        {
            InitializeComponent();
        }

        public void LoadDNT(Stream stream)
        {
            dataGridView1.DataSource = null;
            var node = treeView1.Nodes[0];

            if (stream is FileStream) { 
                Text = ((FileStream)stream).Name.Split('\\').Last();
                toolStripStatusLabel1.Text = ((FileStream)stream).Name;
            }

            dnt = new DragonNestDataTable(stream);
            dataGridView1.DataSource = dnt;

            node.Nodes.Clear();
            foreach (DataColumn column in dnt.Columns)
                node.Nodes.Add(new TreeNode(column.ColumnName));
            treeView1.ExpandAll();

        }

        private void treeView1ColumnMenu_Opening(object sender, CancelEventArgs e)
        {
            var node = treeView1.SelectedNode;
            if (node.Level != 1) return;
            showToolStripMenuItem.Checked =  dataGridView1.Columns[node.Text].Visible;
            freezeToolStripMenuItem1.Checked = dataGridView1.Columns[node.Text].Frozen;
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[treeView1.SelectedNode.Text].Visible = !showToolStripMenuItem.Checked;
        }

        private void freezeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[treeView1.SelectedNode.Text].Frozen = !freezeToolStripMenuItem1.Checked;
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            switch (e.Button)
            {
                case MouseButtons.Right:
                    switch (e.Node.Level)
                    {
                        case 1:
                            contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
                            break;
                    }
                    break;
            }
        }

        
        private void naviBar1_Resize(object sender, EventArgs e)
        {
            var obj = ((NaviBar)sender);
            if (obj.Collapsed) { 
                splitContainer1.SplitterDistance = obj.Size.Width;
                splitContainer1.IsSplitterFixed = true;
            }
            else{
                splitContainer1.IsSplitterFixed = false;
                splitContainer1.SplitterDistance = obj.Size.Width;
            }
        }

        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {
            naviBar1.Width = splitContainer1.SplitterDistance;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            naviBar1.Width = splitContainer1.SplitterDistance;
        }
    }
}
