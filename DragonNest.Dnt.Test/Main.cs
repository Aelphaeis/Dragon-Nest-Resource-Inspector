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
using WeifenLuo.WinFormsUI.Docking;

namespace DragonNest.ResourceInspection.dnt.Test
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public Main(String [] args) : this()
        {
            foreach(var v in args)
                using(FileStream fs = new FileStream(v,FileMode.Open))
                {
                    DNTViewer viewer = new DNTViewer();
                    viewer.LoadDNT(fs); 
                    viewer.Show(dockPanel1, DockState.Document);
                }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "DNT | *.dnt";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DNTViewer viewer = new DNTViewer();
                viewer.LoadDNT(ofd.OpenFile());
                viewer.Show(dockPanel1, DockState.Document);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void AddTab(DockContent content){
            content.Show(dockPanel1, DockState.Document);
        }
    }
}
