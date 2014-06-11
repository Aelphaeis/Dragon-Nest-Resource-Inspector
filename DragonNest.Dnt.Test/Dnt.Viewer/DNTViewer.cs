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

namespace DragonNest.ResourceInspection.Dnt.Viewer
{
    public partial class DntViewer : DockContent
    {

        public static event EventHandler HideLinqEvent;
        public static event EventHandler ShowLinqEvent;
        public static bool ShowLinq
        {
            get
            {
                return showLinq;
            }
            set
            {
                if (!value)
                {
                    if (ShowLinqEvent != null)
                        ShowLinqEvent(null, EventArgs.Empty);
                }
                else
                {
                    if (HideLinqEvent != null)
                        HideLinqEvent(null, EventArgs.Empty);
                }
                showLinq = value;
            }
        }
        static bool showLinq = false;
        
        DataTable Table;

        public DntViewer()
        {
            InitializeComponent();

            ShowLinqEvent += (s, e) => splitContainer2.Panel1Collapsed = true;
            HideLinqEvent += (s, e) => splitContainer2.Panel1Collapsed = false;

            splitContainer2.Panel1Collapsed = (showLinq)? false:true;
        }

        public void LoadDntStream(Stream stream)
        {
            dataGridView1.DataSource = null;

            if (stream is FileStream)
            {
                Text = ((FileStream)stream).Name.Split('\\').Last();
                toolStripStatusLabel1.Text = ((FileStream)stream).Name;
            }

            Table = new DragonNestDataTable(stream);
            textBox1.Text = "from r in Rows select r;";
            LoadDataSource();
            SetTree(Table);
        }

        void LoadDataSource()
        {
            dataGridView1.DataSource = Table;
        }

        public void SetCommmand(String command)
        {
            textBox1.Text = command;
        }

        public void SetTree(DataTable tab)
        {
            var node = treeView1.Nodes[0];

            node.Nodes.Clear();
            foreach (DataColumn column in tab.Columns)
                node.Nodes.Add(new TreeNode(column.ColumnName + " - - - " + column.DataType.Name) { Name = column.ColumnName });
            treeView1.ExpandAll();

        }

        private void treeView1ColumnMenu_Opening(object sender, CancelEventArgs e)
        {
            var node = treeView1.SelectedNode;
            if (node.Level != 1) return;
            showToolStripMenuItem.Checked = dataGridView1.Columns[node.Name].Visible;
            freezeToolStripMenuItem1.Checked = dataGridView1.Columns[node.Name].Frozen;
        }
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[treeView1.SelectedNode.Name].Visible = !showToolStripMenuItem.Checked;
        }

        private void freezeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns[treeView1.SelectedNode.Name].Frozen = !freezeToolStripMenuItem1.Checked;
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

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            naviBar1.Width = splitContainer1.SplitterDistance;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                var clone = Table.Clone();
                clone.Clear();

                var Rows = Table.Rows.Cast<DataRow>();
                var rows = LinqSandbox.Execute(textBox1.Text, Rows);

                foreach (var v in rows)
                    clone.ImportRow(v);
                DntViewer form = new DntViewer();
                form.Table = clone;

                form.LoadDataSource();
                form.SetTree(form.Table);
                form.SetCommmand(textBox1.Text);
                form.toolStripStatusLabel1.Text = String.Empty;
                form.Show(DockPanel, DockState.Document);
            }
            catch (AggregateException x)
            {
                var msg = String.Empty;
                foreach (var ex in x.InnerExceptions)
                    msg += ex.Message + Environment.NewLine;
                MessageBox.Show(msg);
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}
