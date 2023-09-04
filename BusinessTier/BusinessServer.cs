using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using BusinessTierInterface;
using ServerInterface;
using System.Threading;
using System.Runtime.CompilerServices;

namespace BusinessTier
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServer : BusinessServerInterface
    {
        private DBServerInterface foob;
        private uint LogNumber;
        public BusinessServer()
        {
            LogNumber = 0;
            ChannelFactory<DBServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DBServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            Log("New business server created");
        }
        public int GetNumEntries()
        {
            Log("Method call => GetNumEntries()");
            return foob.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string bitmapString)
        {
            Log($"Method call => GetValuesForEntry() index: {index}");
            foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName, out bitmapString);
        }

        public void Search(string term, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out string bitmapString)
        {
            Log($"Method call => Search() term: {term}");

            acctNo = 0; pin = 0; bal = 0;
            fName = null; lName = null; bitmapString = null;

            int numOfEntries = foob.GetNumEntries();

            Log($"Search started");
            for (int i = 0; i < numOfEntries; i++)
            {
                foob.GetValuesForEntry(i, out _, out _, out _, out _, out string currLName, out _);
                if (currLName.ToLower() == term.ToLower())
                {
                    Log($"Search succeeded");
                    Thread.Sleep(2000);
                    foob.GetValuesForEntry(i, out acctNo, out pin, out bal, out fName, out lName, out bitmapString);
                    break;
                }
            }

            if (fName == null)
            {
                Log($"Search failed");
                throw new FaultException<SearchFault>(new SearchFault() { Message = "Not Found"}, new FaultReason("Not Found"));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(string logString)
        {
            Console.WriteLine($"Log: {LogNumber} : {DateTime.Now} : {logString}");
            LogNumber++;
        }
    }
}
