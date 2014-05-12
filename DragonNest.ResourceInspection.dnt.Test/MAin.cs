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

namespace DragonNest.ResourceInspection.dnt.Test
{
    public partial class Main : Form
    {


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
        }
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
                    viewer.MdiParent = this;
                    viewer.LoadDNT(fs);
                    viewer.Show();
                }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "DNT | *.dnt";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DNTViewer viewer = new DNTViewer();
                viewer.MdiParent = this;
                viewer.LoadDNT(ofd.OpenFile());
                viewer.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
