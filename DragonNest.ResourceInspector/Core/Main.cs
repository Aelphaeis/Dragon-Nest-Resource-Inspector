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
using DragonNest.ResourceInspector.Dnt.Viewer;
using DragonNest.ResourceInspector.Pak.Viewer;
using System.ServiceModel;
//using DragonNest.ResourceInspector.Core.Explorer;
namespace DragonNest.ResourceInspector.Core
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

            //Do event handlers here to reduce code clutter later. 
            exitToolStripMenuItem.Click += (s, e) => Close();
            dragonNestTableToolStripMenuItem.Click += (s, e) => OpenDnt();
            singleFileToolStripMenuItem.Click += (s, e) => OpenPakSingle();
            amalgamationToolStripMenuItem.Click += (s, e) => OpenPakAmalgation();
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
                    else if (argTrim.EndsWith(".pak"))
                        OpenPak(argument);
                }
            }
        }
        #region Pubilc methods
        public void OpenDnt()
        {
            var ofd = new OpenFileDialog() { Filter = "DNT | *.dnt" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenDnt(ofd.OpenFile());
        }

        public void OpenPakSingle()
        {
            var ofd = new OpenFileDialog() { Filter = "PAK | *.pak" };
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenPak(ofd.OpenFile());
        }

        public void OpenPakAmalgation()
        {
            var ofd = new OpenFileDialog() { Filter = "PAK | *.pak" };
            ofd.Multiselect = true;

            AmalgamatedPakViewer apv = new AmalgamatedPakViewer();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                apv.LoadPaks(ofd.FileNames.Select(p => File.Open(p, FileMode.Open, FileAccess.Read,FileShare.ReadWrite))).Show(dockPanel1, DockState.Document);
        }
        #endregion
        #region Service Implementations
        public void OpenDnt(string path)
        {
            OpenDnt(new FileStream(path, FileMode.Open));
        }

        public void OpenPak(string path)
        {
            OpenPak(new FileStream(path, FileMode.Open));
        }
        public bool IsOnline()
        {
            return true;
        }

        #endregion

        #region Prviate Methods
        void OpenPak(Stream stream)
        {
            var PakOpenWorker = new BackgroundWorker();
            PakOpenWorker.DoWork += PakOpenerWorker_DoWork;
            PakOpenWorker.RunWorkerAsync(stream);
        }

        void OpenDnt(Stream stream)
        {
            var DntOpenWorker = new BackgroundWorker();
            DntOpenWorker.DoWork += DntOpenerWorker_DoWork;
            DntOpenWorker.RunWorkerAsync(stream);
        }

        #region Event Handlers
        private void showLinqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DntViewer.ShowLinq = !showLinqToolStripMenuItem.Checked;
            showLinqToolStripMenuItem.Checked = DntViewer.ShowLinq;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (@this != null)
                @this.Close();
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
                viewer.Show(dockPanel1, DockState.Document);
                statusStrip1.Items.Remove(bar);
                statusStrip1.Items.Remove(label);
                stream.Dispose();
            }));
        }

        private void DntOpenerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var stream = e.Argument as Stream;
            var worker = sender as BackgroundWorker;
            bool IsFileStream = (stream is FileStream) ? true : false;

            ToolStripProgressBar bar = new ToolStripProgressBar() { Style = ProgressBarStyle.Continuous, Maximum = 100, Value = 0 };
            ToolStripLabel label = new ToolStripLabel("Loading : " + ((IsFileStream) ? ((FileStream)stream).Name : "File"));
            ToolStripItem[] items = { label, bar };

            DntViewer viewer = new DntViewer();
            viewer.StatusChanged += (s, a) => Invoke(new Action(() => bar.Value = viewer.Status));
            Invoke(new Action(() => statusStrip1.Items.AddRange(items)));
            viewer.LoadDntStream(stream);
            Invoke(new Action(() =>
            {
                viewer.Show(dockPanel1, DockState.Document);
                statusStrip1.Items.Remove(bar);
                statusStrip1.Items.Remove(label);
                stream.Dispose();
            }));
        }
        #endregion

        private void FileBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        #endregion

    }
}
