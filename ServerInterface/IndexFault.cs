using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace ServerInterface
{
    [DataContract]
    public class IndexFault
    {
        private string message;

        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
