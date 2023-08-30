using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BusinessTierInterface;
using DataTierInterface;

namespace BusinessTier
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServer : BusinessServerInterface
    {
        private DBServerInterface foob;
        public BusinessServer()
        {
            ChannelFactory<DBServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DBServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }
        public int GetNumEntries()
        {
            return foob.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string bitmapString)
        {
            foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out bitmapString);
        }
    }
}
