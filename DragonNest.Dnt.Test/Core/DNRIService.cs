using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace DragonNest.ResourceInspection.Core
{
    [ServiceContract]
    interface DNRIService 
    {
        [OperationContract]
        void OpenDnt(string path);
    }
}
