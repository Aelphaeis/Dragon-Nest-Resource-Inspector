using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonNest.ResourceInspector.Pak
{
    public class FolderHeader : IHeader
    {
        public Int32 FileCount { get; set; }
        public String Name { get; set; }
        
    }
}
