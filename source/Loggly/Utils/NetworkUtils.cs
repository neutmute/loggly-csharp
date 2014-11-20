using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Loggly.Utils
{
    public class NetworkUtils
    {
        #region host information
        public static string GetHostIPAddress()
        {
            var addressess = Dns.GetHostAddresses(Environment.MachineName);
            foreach (var addrs in addressess)
            {
                if (addrs.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return addrs.ToString();
                }
            }

            //If no IP Address is found then return machine name
            return Environment.MachineName;
        }
        #endregion
    }
}
