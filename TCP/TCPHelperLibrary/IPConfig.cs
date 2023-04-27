using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;

namespace TCPHelperLibrary
{
    public static class IPConfig
    {
        private static string m_sMyIP = "";
        private static string m_sMyHostName = "";
        private static string m_sCurrentConnectedIP = "";
        private static string m_sCurrentConnectedHostName = "localhost";
        private static bool m_bIsAdmin;

        public static bool Initialize()
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            m_sMyHostName = ipProperties.HostName;

            // this trick for determining a host name quickly came from
            // https://stackoverflow.com/questions/6803073/get-local-ip-address
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                m_sMyIP = endPoint.Address.ToString();
            }

            m_sCurrentConnectedIP = m_sMyIP;
            m_sCurrentConnectedHostName = m_sMyHostName;

            m_bIsAdmin = true; // Local host always admin
            return true;
        }

        public static string getBaseIP() { return m_sMyIP; }
        public static string getBaseHostName() { return m_sMyHostName; }

        public static string CurrentConnectedIP
        {
            get { return m_sCurrentConnectedIP; }
            set { m_sCurrentConnectedIP = value; }
        }

        public static string CurrentConnectedHostName
        {
            get { return m_sCurrentConnectedHostName; }
            set { m_sCurrentConnectedHostName = value; }
        }

        public static bool IsAdmin
        {
            get { return m_bIsAdmin; }
            set { m_bIsAdmin = value; }
        }
    }
}
