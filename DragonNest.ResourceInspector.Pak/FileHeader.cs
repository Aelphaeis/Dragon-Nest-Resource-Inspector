using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zlib;
namespace DragonNest.ResourceInspector.Pak
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
        public static FileHeader FromBinaryReader(BinaryReader reader)
        {
            FileHeader header = new FileHeader();
            header.Path = Encoding.ASCII.GetString(reader.ReadBytes(0x100));
            header.SizeDummy = reader.ReadUInt32();
            header.OriginalSize = reader.ReadUInt32();
            header.CompressedSize = reader.ReadUInt32();
            header.FileOffset = reader.ReadUInt32();
            header.Unknown = reader.ReadUInt32();
            reader.ReadBytes(sizeof(uint) * 10);
            return header;
        }
        public string Name 
        { 
            get 
            { 
                return Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim('\0'); 
            } 
        }
        public string Path { get; set; }
        public uint SizeDummy { get; set; }    //  <format=hex>;
        public uint OriginalSize { get; set; }//  <format=hex>;
        public uint CompressedSize { get; set; }// <format=hex>;
        public uint FileOffset { get; set; }//  <format=hex>;
        public uint Unknown { get; set; }
        public uint padding { get; set;}

        internal PakFile file;

        public Stream GetStream()
        {

            if (file == null)
                throw new NotSupportedException("File Header must be associated with pak file to get filestream");

            using(var fs = File.Open(file.Path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader =  new BinaryReader(fs))
            {
                fs.Position = FileOffset;
                var data = reader.ReadBytes(Convert.ToInt32(CompressedSize));
                return new MemoryStream(ZlibStream.UncompressBuffer(data));
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
