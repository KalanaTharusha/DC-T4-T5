using DataTierInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BusinessTierInterface;

namespace BusinessTier
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Business server started");

            ServiceHost host;

            NetTcpBinding tcp = new NetTcpBinding();

            host = new ServiceHost(typeof(BusinessServer));

            host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://0.0.0.0:8101/BusinessService");
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
