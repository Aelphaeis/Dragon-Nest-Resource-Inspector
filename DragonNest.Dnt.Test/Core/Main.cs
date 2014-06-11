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
    using Timer = System.Timers.Timer;
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
                            else if (argTrim.EndsWith(".pak"))
                                channel.OpenPak(argument);
                        }
                        channel.Activate();
                        //This prevents the system from trying to access this object in a disposed state.
                        Visible = false;
                        Close();
                        //Invoke(new Action(() => Close()));
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
                    else if (argTrim.EndsWith(".pak"))
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

        public void OpenPak(string path)
        {
            OpenPak(new FileStream(path, FileMode.Open));
        }

        public void OpenPak(Stream stream)
        {
            PakOpenerWorker.RunWorkerAsync(stream);
        }

        public bool IsOnline()
        {
            return true;
        }

        #endregion

        #region Prviate Methods
        DntViewer GetDntWindowFromStream(FileStream stream)
        {
            DntViewer viewer = new DntViewer();
            viewer.LoadDntStream(stream);
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
            if (@this != null)
            {
                @this.Close();
            }
        }
        private async void dntToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "DNT | *.dnt" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                (await Task<DntViewer>.Run(() => { return GetDntWindowFromStream((FileStream)ofd.OpenFile()); })).Show(dockPanel1, DockState.Document);
        }

        private void pakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "PAK | *.pak" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    PakOpenerWorker.RunWorkerAsync(ofd.OpenFile());
        }
        private void PakOpenerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Maybe someday someone else will call this background worker.
            var stream = e.Argument as Stream;
            var worker = sender as BackgroundWorker;
            bool IsFileStream = (stream is FileStream) ? true : false;

            ToolStripProgressBar bar = new ToolStripProgressBar() { Style = ProgressBarStyle.Continuous, Maximum = 100, Value = 0 };
            ToolStripLabel label = new ToolStripLabel("Loading : " + ((IsFileStream) ? ((FileStream)stream).Name : "File"));
            ToolStripItem[] items = { label, bar };
            PakViewer viewer = new PakViewer();

            viewer.StatusChanged += (s, a) => Invoke(new Action(() => bar.Value = viewer.Status));
            Invoke(new Action(() => statusStrip1.Items.AddRange(items)));

            viewer.LoadPakStream(stream);
            Invoke(new Action(() =>
            {
                viewer.Show(dockPanel1,DockState.Document);
                statusStrip1.Items.Remove(bar);
                statusStrip1.Items.Remove(label);
                stream.Dispose();
            }));
        }
        #endregion
        #endregion

        
    }
}
