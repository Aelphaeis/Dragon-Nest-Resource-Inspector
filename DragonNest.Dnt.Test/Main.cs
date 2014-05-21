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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO.Pipes;
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
            foreach (var v in args)
                using (FileStream fs = new FileStream(v, FileMode.Open))
                    OpenWindowFromStream(fs);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "DNT | *.dnt";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenWindowFromStream(ofd.OpenFile());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void OpenWindowFromStream(Stream stream) 
        {
            DNTViewer viewer = new DNTViewer();
            viewer.LoadDNT(stream);
            viewer.Show(dockPanel1, DockState.Document);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showLinqToolStripMenuItem.Checked = DNTViewer.ShowLinq;
        }

        private void showLinqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DNTViewer.ShowLinq = !showLinqToolStripMenuItem.Checked;
        }
    }
}
