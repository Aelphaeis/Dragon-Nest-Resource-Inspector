using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonNest.ResourceInspector.Pak
{
    public class FolderHeader : IHeader
    {
        public Int32 FileCount { get; private set; }
        public String Name { get; set; }
        public String Path { get; set; }

        public Dictionary<String, IHeader> Files { get; set; }

        public FolderHeader()
        {
            Files = new Dictionary<string, IHeader>();
        }


      
    }
}
