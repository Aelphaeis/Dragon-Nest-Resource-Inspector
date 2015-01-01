using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonNest.ResourceInspector.Pak
{
    public class FolderHeader : IHeader
    {
        public static IHeader Find(Dictionary<String, IHeader> Tree, String Path)
        {
            var headerPath = Path.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<String, IHeader> index = Tree;
            for (int i = 0, limit = headerPath.Length -1; i < limit; i++)
                index = (index[headerPath[i]] as FolderHeader).Files;

            return index[headerPath[headerPath.Length - 1]];
        }

        public virtual FolderHeader Parent { get; set; }
        public virtual Int32 FileCount { get; set; }
        public virtual String Name { get; set; }
        public virtual String Path { get; set; }

        public virtual IHeader this[String Path]
        {
            get
            {
                return Find(Files, Path);
            }
        }

        public virtual Dictionary<String, IHeader> Files { get; set; }

        public FolderHeader()
        {
            Files = new Dictionary<string, IHeader>();
        }

        public static IEnumerable<IHeader> GetDeepFileList(Dictionary<String, IHeader> root, Boolean IncludeFolders = true)
        {
            foreach (var file in root.Values)
            {
                if(file is FolderHeader)
                {
                    if (IncludeFolders)
                        yield return file;
                    foreach (var child in GetDeepFileList((file as FolderHeader).Files, IncludeFolders))
                        yield return child;
                }
                else
                    yield return file;
            }
        }

    }
}
