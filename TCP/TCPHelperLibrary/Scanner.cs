using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Diagnostics;
using System.DirectoryServices;

namespace TCPHelperLibrary
{
    public class Scanner
    {
        private List<SupportedPort> ports = new List<SupportedPort>();

        public struct SupportedPort
        {
            public int Port;
            public string PortName;
        }

        public Scanner() 
        {
            ports.Add(new SupportedPort() { Port = 21, PortName = "FTP" });
            ports.Add(new SupportedPort() { Port = 22, PortName = "SSH" });
            ports.Add(new SupportedPort() { Port = 23, PortName = "TELNET" });
            ports.Add(new SupportedPort() { Port = 25, PortName = "SMTP" });
            ports.Add(new SupportedPort() { Port = 53, PortName = "DNS" });
            ports.Add(new SupportedPort() { Port = 80, PortName = "HTTP" });
            ports.Add(new SupportedPort() { Port = 110, PortName = "POP" });
            ports.Add(new SupportedPort() { Port = 443, PortName = "SSL" });
            ports.Add(new SupportedPort() { Port = 593, PortName = "SSDCOM" });
            ports.Add(new SupportedPort() { Port = 902, PortName = "VMWARE" });
            ports.Add(new SupportedPort() { Port = 1025, PortName = "Microsoft RPC" });
            ports.Add(new SupportedPort() { Port = 1433, PortName = "SQL Server" });
            ports.Add(new SupportedPort() { Port = 3306, PortName = "MySQL" });
            ports.Add(new SupportedPort() { Port = 5900, PortName = "VNC Server" });
            ports.Add(new SupportedPort() { Port = 6665, PortName = "IRC" });
            ports.Add(new SupportedPort() { Port = 6669, PortName = "IRC" });
            ports.Add(new SupportedPort() { Port = 12345, PortName = "NetBUS" });
        }

        public List<IPEndPoint> GetActiveListeners()
        {
            List<IPEndPoint> connections = new List<IPEndPoint>();

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            IPEndPoint[] tcpConnections = ipProperties.GetActiveTcpListeners();

            foreach (IPEndPoint ipEndPointInfo in tcpConnections)
            {
                connections.Add(ipEndPointInfo);
            }

            return connections;
        }

        public List<TcpConnectionInformation> GetActiveConnections()
        {
            List<TcpConnectionInformation> connections = new List<TcpConnectionInformation>();

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpInfo in tcpConnections)
            {
                connections.Add( tcpInfo );
            }

            return connections;
        }

        // Scan timeout period to wait for connection to remote port
        public int SCANTIMEOUT = 250;

        public List<SupportedPort> GetRemotePorts(string remoteIP)
        {
            List<SupportedPort> supportPorts = new List<SupportedPort>();
            
            foreach (SupportedPort port in ports)
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = socket.BeginConnect(remoteIP, port.Port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(SCANTIMEOUT, true);

                if (socket.Connected)
                {
                    supportPorts.Add(port);
                    socket.EndConnect(result);
                }
                else
                {
                    socket.Close();
                }
            }

            return supportPorts;
        }

        public List<string> GetInternalNetworkDevices()
        {
            List<string> lines = new List<string>();
            List<string> filter = new List<string>();

            // Identify Computers
            List<string> nwDeviceCheck = GetNetworkDevices();

            // Create IP list to search for and filter out from the ARP devices we know is a computer. 
            foreach (string s in nwDeviceCheck)
            {
                lines.Add( s );
                filter.Add(s.Split('@')[1].Split('\r')[0]);
            }

            // Identify Other Devices.... 
            ProcessHandler ph = new ProcessHandler();
            Process p = ph.Create("arp.exe", "-a");
            p.Start();

            while (!p.StandardOutput.EndOfStream)
            {
                bool bFilter = false;
                string line = p.StandardOutput.ReadLine() + "\r\n";
                foreach (string s in filter)
                {
                    if (line.Contains(s))
                    {
                        bFilter = true;
                        break;
                    }
                }
                
                // TODO: Figure out how to translate a device/MACID to a Device name
                // LIKE IN Windows Network Discovery 
                if (!bFilter && !line.Contains("Internet Address") && !(line.Trim().Length == 0))
                {
                    string sDevice = string.Format("Found Device   : {0}@{1}\r\n",
                        line.Substring(24, 22).Trim(), line.Substring(0, 24).Trim());
                    lines.Add(sDevice);
                }
            }

            return lines;
        }

        public List<string> GetNetworkDevices()
        {
            List<string> nwDevices = new List<string>();
            DirectoryEntry root = new DirectoryEntry("WinNT:"); // 
            foreach (DirectoryEntry computers in root.Children)
            {
                foreach (DirectoryEntry computer in computers.Children)
                {
                    string computerName = computer.Name;
                    if (computerName != "Schema")
                    {
                        IPAddress[] localIPs = null;
                        string localIP = "";
                        string sComputer;

                        try
                        {
                            localIPs = Dns.GetHostAddresses(computerName);
                        }
                        catch
                        {
                            // Exception is usually host not found, in which case the computer
                            // was turned off. I found this out JUST now add this code
                            // when my dad went to bed his computer was off (just physically checked). 
                        }

                        if (localIPs != null)
                        {
                            foreach (IPAddress ip in localIPs)
                            {
                                if (ip.AddressFamily == AddressFamily.InterNetwork)  // IPv4 addresses only
                                {
                                    localIP = ip.ToString();
                                    break;
                                }
                            }
                            sComputer = string.Format("Found Computer : {0}@{1}\r\n", computer.Name, localIP);

                        }
                        else
                        {
                            sComputer = string.Format("Found Computer : {0}@[NOT TURNED ON]\r\n", computer.Name);
                        }
                        nwDevices.Add(sComputer);
                    }
                }
            }

            return nwDevices;
        }
    }
}
