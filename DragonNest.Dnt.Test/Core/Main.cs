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
using DragonNest.ResourceInspection.Dnt.Viewer;
using DragonNest.ResourceInspection.Pak.Viewer;
using System.ServiceModel;

namespace DragonNest.ResourceInspection.Core
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class Main : Form, DNRIService
    {
        const string PipeServiceName = "DNRIS";
        const string PipeName = "net.pipe://localhost";
        const string PipeService = PipeName + @"/" + PipeServiceName; 

        ServiceHost @this;

        public Main()
        {
            InitializeComponent();
        }

        public Main(String [] args) : this()
        {
            try
            {
                using (ChannelFactory<DNRIService> serviceFactory = new ChannelFactory<DNRIService>(new NetNamedPipeBinding(), new EndpointAddress(PipeService)))
                {
                    var channel = serviceFactory.CreateChannel();
                    if(channel.IsOnline())
                    {
                        foreach (var argument in args)
                        {
                            var argTrim = argument.Trim() ;
                            if (argTrim.EndsWith(".dnt"))
                                channel.OpenDnt(argument);
                            else if (argTrim.EndsWith(".dnt"))
                                channel.OpenPak(argument);
                        }
                        channel.Activate();
                        Close();
                    }
                }
            }
            catch
            {
                @this = new ServiceHost(this, new Uri(PipeName));
                @this.AddServiceEndpoint(typeof(DNRIService), new NetNamedPipeBinding(), PipeService);
                @this.BeginOpen((IAsyncResult ar) => @this.EndOpen(ar), null);

                foreach (var argument in args)
                {
                    var argTrim = argument.Trim();
                    if (argTrim.EndsWith(".dnt"))
                        OpenDnt(argument);
                    else if (argTrim.EndsWith(".dnt"))
                        OpenPak(argument);
                }
            }

        }

        #region Service Implementations
        public async void OpenDnt(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
                (await Task<DntViewer>.Run(() => { return GetDntWindowFromStream(fs); })).Show(dockPanel1, DockState.Document);
        }

        public async void OpenPak(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
                (await Task<DntViewer>.Run(() => { return GetPakWindowFromStream(fs); })).Show(dockPanel1, DockState.Document);
        }

        public bool IsOnline()
        {
            return true;
        }

        #endregion

        #region Prviate Methods
        DntViewer GetDntWindowFromStream(Stream stream)
        {

            DntViewer viewer = new DntViewer();
            viewer.LoadDNT(stream);
            return viewer;
        }

        PakViewer GetPakWindowFromStream(Stream stream)
        {
            PakViewer viewer = new PakViewer();
            viewer.LoadPakStream(stream);
            return viewer;
        }

        #region Event Handlers
        private void showLinqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DntViewer.ShowLinq = !showLinqToolStripMenuItem.Checked;
            showLinqToolStripMenuItem.Checked = DntViewer.ShowLinq;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {

            Task.Run(() => @this.BeginClose((IAsyncResult ar) => @this.EndClose(ar), null));
        }
        private async void dntToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "DNT | *.dnt" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                (await Task<DntViewer>.Run(() => { return GetDntWindowFromStream(ofd.OpenFile()); })).Show(dockPanel1, DockState.Document);
        }
        private async void pakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "PAK | *.pak" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                (await Task<PakViewer>.Run(() => { return GetPakWindowFromStream(ofd.OpenFile()); })).Show(dockPanel1, DockState.Document);
            }
        }
        #endregion
        #endregion
    }
}
