using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Threading;
using DragonNest.ResourceInspector.Core;

namespace DragonNest.ResourceInspector
{
    class Program
    {
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Main(args);
            if(!form.IsDisposed)
                Application.Run(form);
        }
    }


}
