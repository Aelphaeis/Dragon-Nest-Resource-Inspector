using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DragonNest.ResourceInspection.Pak
{
    //struct FileHeader
    //{
    //    char FileName[0x100];
    //    uint SizeDummy      <format=hex>;
    //    uint OriginalSize   <format=hex>;
    //    uint CompressedSize <format=hex>;
    //    uint FileOffset     <format=hex>;
    //    uint Unknown        <format=hex>;
    //    uint Null[10];
    //};
    public struct FileHeader
    {
        public string FileName;
        public uint SizeDummy;    //  <format=hex>;
        public uint OriginalSize;//  <format=hex>;
        public uint CompressedSize;// <format=hex>;
        public uint FileOffset;//  <format=hex>;
        public uint Unknown;
        public uint padding;

        //public static FileHeader FromStream(Stream stream)
        //{

        //}
        public static FileHeader FromBinaryReader(BinaryReader reader)
        {
            FileHeader header = new FileHeader();
            header.FileName = Encoding.ASCII.GetString(reader.ReadBytes(0x100));
            header.SizeDummy = reader.ReadUInt32();
            header.OriginalSize = reader.ReadUInt32();
            header.CompressedSize = reader.ReadUInt32();
            header.FileOffset = reader.ReadUInt32();
            header. Unknown = reader.ReadUInt32();
            reader.ReadBytes(sizeof(uint) * 10);
            return header;
        }
    }
}
