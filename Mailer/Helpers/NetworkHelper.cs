using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Mailer.Helpers
{
    public static class NetworkHelper
    {
        public static List<string> GetLocalIpAddresses()
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return null;


            var host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).Select(a => a.ToString())
                .ToList();
        }
    }
}