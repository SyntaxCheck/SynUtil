using System.Collections.Generic;
using System.Net;

namespace SynUtil.Network
{
    public class ResolveHostname
    {
        public static List<string> GetIpsForHostName(string host)
        {
            List<string> ips = new List<string>();

            IPAddress[] addresslist = Dns.GetHostAddresses(host);

            foreach (IPAddress ip in addresslist)
            {
                ips.Add(ip.ToString());
            }

            return ips;
        }
    }
}
