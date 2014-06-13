using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonNest.ResourceInspector.Pak
{
    //struct PakHeader
    //{
    //    uchar Signature[0x20];
    //    uint  Null[0x38];
    //    uint  Unk;                      //0x0B
    //    uint  FileCount;
    //    uint  TableOffset <format=hex>;
    //    uint  Unk2;
    //    uint  Null2[0xBC];
    //} Header;
    public class PakHeader
    {
        public const string Identifier = "EyedentityGames Packing File 0.1";
        public uint FileCount;
        public uint TableOffset;
      
    }
}
