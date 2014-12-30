using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DragonNest.ResourceInspector.Pak
{
    public class PakFile
    {
        public String Path { get; set; }
        public String Name { get; set; }
        public PakHeader Header { get; set; }
        public IList<FileHeader> Files { get; set; }

        Dictionary<String,IHeader> files { get; set; }

        public PakFile()
        {
            Header = new PakHeader();
            Files = new List<FileHeader>();
            files = new Dictionary<String, IHeader>();
        }

        public PakFile(Stream stream)
            : this()
        {
            //If we can't seek, we can't parse the stream
            if (!stream.CanSeek)
                throw new Exception("Unable to Seek through Stream");
            //We don't own the stream, so leave open is set to true
            using (var reader = new BinaryReader(stream, Encoding.Default, true))
            {
                //If we don't have the signature then that means we are dealing with an unknown file type
                if (PakHeader.Identifier != Encoding.ASCII.GetString(reader.ReadBytes(0x20)))
                    throw new Exception("Invalid File Format");

                if (stream is FileStream)
                {
                    Path = ((FileStream)stream).Name;
                    Name = Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Last();
                }



                //This is where the FileCount and File Offset are stored.
                stream.Position = 0x104L;
                Header.FileCount = reader.ReadUInt32();
                Header.TableOffset = reader.ReadUInt32();

                //We'll begin reading our file headers at the first offset Position
                stream.Position = Header.TableOffset;

                // file = ne


                for (int i = 0; i < Header.FileCount; i++)
                {
                    var header = FileHeader.FromBinaryReader(reader);
                    header.file = this;
                    Files.Add(header);
                }
            }
        }

        public void LoadPak(Stream stream) 
        {
            //check if we can parse the stream, break if we can't
            if (!stream.CanSeek)
                throw new Exception("Unable to Seek through Stream");
            //create reader from stream
            using(var reader = new BinaryReader(stream, Encoding.Default, true))
            {
                //check if format is correct
                if (PakHeader.Identifier != Encoding.ASCII.GetString(reader.ReadBytes(0x20)))
                    throw new Exception("Invalid File Format");

                //Read filecount and Table offset
                stream.Position = 0x104L;
                Header.FileCount = reader.ReadUInt32();
                Header.TableOffset = reader.ReadUInt32();

                //set read location for file header
                stream.Position = Header.TableOffset;
                //Read our files.
                for(int i = 0; i < Header.FileCount; i++)
                {
                    var fileInfo = FileHeader.FromBinaryReader(reader);
                    fileInfo.file = this;

                    var headerPath = fileInfo.Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                    var tempPath = String.Empty;
                    var folderIndex = files;

                    for(int c = 0, limit = headerPath.Length - 1; c < limit; c++)
                    {
                        if (!folderIndex.ContainsKey(headerPath[c]))
                            folderIndex.Add(headerPath[c], new FolderHeader() { Name = headerPath[c]});
                        folderIndex = (folderIndex[headerPath[c]] as FolderHeader).Files;
                    }

                    //This shows that there are duplicates;
                    if (!folderIndex.ContainsKey(fileInfo.Name))
                        folderIndex.Add(fileInfo.Name, fileInfo);
                    else
                        folderIndex[fileInfo.Name] = fileInfo;
                }
            }
        }
    }
}
