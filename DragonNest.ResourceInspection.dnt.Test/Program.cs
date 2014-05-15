using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
namespace DragonNest.ResourceInspection.dnt.Test
{
    class Program
    {
        [STAThread]
        static void Main(string [] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SingleInstance.SingleApplication.Run(new Main(args));
            //SingleInstance.SingleApplication.Run(new Parent());
        }

    }


}
