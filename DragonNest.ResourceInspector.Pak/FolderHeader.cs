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
    }
}
