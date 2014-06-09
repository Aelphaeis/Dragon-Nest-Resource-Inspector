using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DragonNest.ResourceInspector.Pak;

namespace DragonNest.ResourceInspection.Pak.Study
{
    class Program
    {
        static string path = @"E:\Programs\PlayZone\Resource09.pak";
        static void Main(string[] args)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            using(BinaryReader br = new BinaryReader(fs))
            {
                //Each Pack file has a signature. If not, it is invalid.
                var SignatureByteArray = br.ReadBytes(0x20);
                var Signature = Encoding.ASCII.GetString(SignatureByteArray);
                if (Signature != PakHeader.Identifier)
                    throw new Exception("Invalid File Format");

                //This is where the File Count and Files address is.
                fs.Position = 0x0104L;
                PakHeader pakHeader = new PakHeader();
                pakHeader.FileCount = br.ReadUInt32();
                fs.Position = pakHeader.TableOffset = br.ReadUInt32();

                List<FileHeader> fileheaders = new List<FileHeader>();
                for (int i = 0; i < pakHeader.FileCount; i++)
                    fileheaders.Add(FileHeader.FromBinaryReader(br));
            }
        }
    }
}
