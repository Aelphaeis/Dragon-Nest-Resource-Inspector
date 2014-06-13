using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace DragonNest.ResourceInspector.Core
{
    [ServiceContract]
    interface DNRIService 
    {
        [OperationContract]
        void OpenDnt(string path);

        [OperationContract]
        void OpenPak(string path);

        [OperationContract]
        bool IsOnline();

        [OperationContract]
        void Activate();
    }
}
