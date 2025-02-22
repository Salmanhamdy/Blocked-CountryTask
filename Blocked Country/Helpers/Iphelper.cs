using System.Net.Sockets;
using System.Net;

namespace Blocked_Country.Helpers
{
    public static class Iphelper
    {
       public static bool IsValidIPv4(string ipString)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
        public static string iploopback(string ip) 
        {
            if (ip == "::1")
                return ip = "127.0.0.1";
            return ip;
        }
    }
}
