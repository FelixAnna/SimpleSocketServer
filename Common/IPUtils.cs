using System.Net;
using System.Net.Sockets;

namespace Common
{
    public static class IPUtils
    {
        public static string GetIpAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || string.Equals(ipAddress, "*"))
            {

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
            }

            return ipAddress;
        }
    }
}
