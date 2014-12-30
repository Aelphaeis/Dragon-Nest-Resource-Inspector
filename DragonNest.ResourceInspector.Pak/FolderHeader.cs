using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonNest.ResourceInspector.Pak
{
    public class FolderHeader : IHeader
    {
        public virtual Int32 FileCount { get; set; }
        public virtual String Name { get; set; }
        public virtual String Path { get; set; }

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
