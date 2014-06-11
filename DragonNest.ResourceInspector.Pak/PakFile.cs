using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DragonNest.ResourceInspection.Pak
{
    public class PakFile
    {
        public PakHeader Header { get; set; }
        public IList<FileHeader> Files { get; set; }

        public PakFile()
        {
            Header = new PakHeader();
            Files = new List<FileHeader>();
        }

        public PakFile(Stream stream) : this()
        {
            //If we can't seek, we can't parse the stream
            if (!stream.CanSeek)
                throw new Exception("Unable to Seek through Stream");
            //We don't own the stream, so leave open is set to true
            using(var reader = new BinaryReader(stream,Encoding.Default, true))
            {
                //If we don't have the signature then that means we are dealing with an unknown file type
                if (PakHeader.Identifier != Encoding.ASCII.GetString(reader.ReadBytes(0x20)))
                    throw new Exception("Invalid File Format");

                //This is where the FileCount and File Offset are stored.
                stream.Position = 0x104L;
                Header.FileCount = reader.ReadUInt32();
                Header.TableOffset = reader.ReadUInt32();

                //We'll begin reading our file headers at the first offset Position
                stream.Position = Header.TableOffset;
                for (int i = 0; i < Header.FileCount; i++)
                    Files.Add(FileHeader.FromBinaryReader(reader));
            }
        }
    }
}
