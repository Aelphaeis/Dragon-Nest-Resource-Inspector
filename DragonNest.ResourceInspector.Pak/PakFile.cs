using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DragonNest.ResourceInspector.Pak
{
    public class PakFile : FolderHeader
    {
        public const string Identifier = "EyedentityGames Packing File 0.1";

        public Int32 TableOffset { get; set; }

        //IList<FileHeader> Files { get; set; }

        //public Dictionary<String,IHeader> Files { get; set; }

        public PakFile() : base()
        {
        }

        //public PakFile(Stream stream)
        //    : this()
        //{
        //    //If we can't seek, we can't parse the stream
        //    if (!stream.CanSeek)
        //        throw new Exception("Unable to Seek through Stream");
        //    //We don't own the stream, so leave open is set to true
        //    using (var reader = new BinaryReader(stream, Encoding.Default, true))
        //    {
        //        //If we don't have the signature then that means we are dealing with an unknown file type
        //        if (PakHeader.Identifier != Encoding.ASCII.GetString(reader.ReadBytes(0x20)))
        //            throw new Exception("Invalid File Format");

        //        if (stream is FileStream)
        //        {
        //            Path = ((FileStream)stream).Name;
        //            Name = Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Last();
        //        }

        //        //This is where the FileCount and File Offset are stored.
        //        stream.Position = 0x104L;
        //        Header.FileCount = reader.ReadUInt32();
        //        Header.TableOffset = reader.ReadUInt32();

        //        //We'll begin reading our file headers at the first offset Position
        //        stream.Position = Header.TableOffset;

        //        // file = ne


        //        for (int i = 0; i < Header.FileCount; i++)
        //        {
        //            var header = FileHeader.FromBinaryReader(reader);
        //            header.file = this;
        //            Files.Add(header);
        //        }
        //    }
        //}

        public PakFile LoadPak(Stream stream) 
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
                FileCount = Convert.ToInt32(reader.ReadUInt32());
                TableOffset = Convert.ToInt32(reader.ReadUInt32());

                //set read location for file header
                stream.Position = TableOffset;
                //Read our files.
                for(int i = 0; i < FileCount; i++)
                {
                    var fileInfo = FileHeader.FromBinaryReader(reader);
                    fileInfo.file = this;

                    //Get the folder path + file name
                    var headerPath = fileInfo.Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);

                    var folderPath = String.Empty;
                    var folderIndex = Files;

                    //Transverse folders to get to folder which is suppose to hold file.
                    for(int c = 0, limit = headerPath.Length - 1; c < limit; c++)
                    {
                        //Construct Folder Path For Each Folder.
                        folderPath += headerPath[c] + @"\";

                        //Create folder if it doesn't exisit
                        if (!folderIndex.ContainsKey(headerPath[c])) 
                            folderIndex.Add(headerPath[c], new FolderHeader() { Name = headerPath[c], Path = folderPath});

                        //Set current folder to the next folder in the path.
                        folderIndex = (folderIndex[headerPath[c]] as FolderHeader).Files;
                    }

                    //This line proves that there are duplicates with the same pak file (Remove if statement)
                    if (!folderIndex.ContainsKey(fileInfo.Name))
                        folderIndex.Add(fileInfo.Name, fileInfo);
                    else
                        folderIndex[fileInfo.Name] = fileInfo;
                }
            }
            return this;
        }

        public static Dictionary<String, IHeader> Merge(IEnumerable<PakFile> files)
        {
            var result = new Dictionary<String, IHeader>();
            var folderIndex = result;

            foreach(var pak in files.OrderBy(p => p.Name))
            {
                foreach(IHeader header in GetDeepFileList(pak.Files, true))
                {
                    folderIndex = result;
                    var headerPath = header.Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                    for(int c = 0, limit = headerPath.Length - 1; c < limit; c++)
                    {
                        if (!folderIndex.ContainsKey(headerPath[c]))
                            folderIndex.Add(headerPath[c], new FolderHeader() { Name = headerPath[c], Path = header.Path });
                        folderIndex = (folderIndex[headerPath[c]] as FolderHeader).Files;
                    }

                    if (!folderIndex.ContainsKey(header.Name))
                        folderIndex.Add(header.Name, header);
                }
            }
            return result;
        }
    }
}
