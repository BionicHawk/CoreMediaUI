using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreMediaUI.Source
{
    public static class GetDNS {
        private static string _hostname = Dns.GetHostName();
        public static IPAddress? workingAddress { get; private set; } = null;
        public static readonly ushort workingAPIPort = 3001;
        public static readonly ushort wiringMouseControllerPort;

        public static void GetListDNS() {
            var addresses  = Dns.GetHostAddresses(_hostname);
            foreach (var address in addresses) {
                Console.WriteLine(address.ToString());
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                    workingAddress = address; 
                }
            }
        }
    }
}
