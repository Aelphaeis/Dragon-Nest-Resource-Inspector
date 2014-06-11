using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DragonNest.ResourceInspector.Pak;
using Ionic.Zlib;

namespace DragonNest.ResourceInspection.Pak.Study
{
    class Program
    {
        static string path = @"E:\Programs\PlayZone\Resource09.pak";
        static void Main(string[] args)
        {
            using (var fs = new FileStream(path, FileMode.Open,FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(fs, Encoding.Default, true))
            {
                PakFile pf = new PakFile(fs);
                foreach(var v in pf.Files)
                {
                    using (var fileStream = File.Create(v.Name))
                    {
                        v.GetStream().CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
            }
        }
    }
}
