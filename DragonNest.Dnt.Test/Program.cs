using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Threading;
namespace DragonNest.ResourceInspection.dnt.Test
{
    class Program
    {
        const string pipeName = "dntlsnrpipe";
        static Main app;
        static PipeStream Pipe;
        
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //We can determine if this is the first instance of one of not
            if (Pipe is NamedPipeServerStream) //If it is the first instance we should open
                using(Pipe)
                    Application.Run(app = new Main(args));
                
            else //If this is not the first instance we should send information to the first instance about what this instance was suppose to do
                foreach(var arg in args) 
                    Send(arg);

            if (Pipe is NamedPipeServerStream)
                using(var p = new NamedPipeClientStream(pipeName))
                    p.Connect();
        }

        
        static Program()
        {
            try {
                var p = new NamedPipeServerStream(pipeName);
                new Thread(listen).Start();
                Pipe = p;
            }
            catch {
                var p = new NamedPipeClientStream(pipeName);
                p.Connect();
                Pipe = p;
            }
        }

        static void listen()
        {
            try
            {
                var p = (NamedPipeServerStream)Pipe;
                string file = String.Empty;
                while (true)
                {
                    p.WaitForConnection();
                    using (StreamReader reader = new StreamReader(p, Encoding.Default, true, 1024, true))
                        while (!reader.EndOfStream)
                            if (File.Exists(file = reader.ReadLine()))
                                using (var fs = new FileStream(file, FileMode.Open))
                                    app.Invoke(new Action(() => app.OpenWindowFromStream(fs)));
                    p.Disconnect();
                }
            }
            catch (ObjectDisposedException) { }
            catch (IOException) { }
        }


        static void Send(String file)
        {
            if(Pipe.IsConnected)
                using (StreamWriter writer = new StreamWriter(Pipe,Encoding.Default,1024,true))
                    writer.WriteLine(file);
            Pipe.Close();
            Pipe.Dispose();
        }
    }


}
