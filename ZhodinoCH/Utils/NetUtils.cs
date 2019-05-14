﻿using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ZhodinoCH
{
    class NetUtils
    {

        public static IPAddress LocalIPAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public static string GetLocalName()
        {
            return Dns.GetHostName() + " / " + LocalIPAddress().ToString();
        }

    }
}
