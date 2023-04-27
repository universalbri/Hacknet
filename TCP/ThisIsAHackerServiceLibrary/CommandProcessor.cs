using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using static TCPHelperLibrary.Scanner;
using static WAPHelperLibrary.WIFIAccessor;
using System.Runtime.InteropServices;
using WAPHelperLibrary;
using Windows.Devices.WiFi;
using Windows.Foundation.Metadata;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using TCPHelperLibrary;

namespace ThisIsAHackerServiceLibrary
{
    public class CommandProcessor
    {
        private const string IPv4Pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        //Regular Expression object    
        private Regex checkIPV4 = new Regex(IPv4Pattern); 
        public CommandProcessor()
        {
            _currentDirectory = Directory.GetCurrentDirectory();
        }
        private string _currentDirectory  = "";
        public string getCurrentDirectory
        { get { return "//" + _currentDirectory.Replace('\\', '/').Replace(":", "" ); } }

        public async Task<string> ProcessCommand(string command)
        {
            List<string> splitCommand = command.Split(' ').ToList<string>();

            string _return;

            switch (splitCommand[0].ToLower().Trim())
            {
                case "exe":
                    InternalCommandProcessor icp = new InternalCommandProcessor();
                    _return = "Available Executables:\r\n" + 
                                String.Join( "\r\n", icp.Commands.ToArray() );
                    break;
                case "remove":
                case "delete":
                case "del":
                case "rm":
                    try
                    {
                        if (splitCommand.Count > 1)
                        {
                            _return = doFileDelete(splitCommand[1]);
                        }
                        else
                        {
                            _return = "Not Enough Arguments";
                        }
                    }
                    catch
                    {
                        _return = "Unknown Error Encountered";
                    }
                    break;
                case "cd":
                    try
                    {
                        if (splitCommand.Count > 1)
                        {
                            _return = doDirectoryChange(splitCommand[1]);
                        }
                        else
                        {
                            _return = "Usage: cd [WHERE TO GO or .. TO GO BACK]";
                        }
                    }
                    catch
                    {
                        _return = "Invalid Path";
                    }
                    break;
                case "clear":
                case "cls":
                    _return = "cls";
                    break;
                // TODO: put 'unwinding' logic in here to allow > 1 connection depth
                case "dc":
                case "disconnect":
                    IPConfig.CurrentConnectedIP = IPConfig.getBaseIP();
                    IPConfig.CurrentConnectedHostName = IPConfig.getBaseHostName();
                    IPConfig.IsAdmin = true;
                    _return = "";
                    break;
                case "ls":
                case "dir":
                    string path = "";
                    try
                    {
                        if (splitCommand.Count > 0)
                        {
                            path = splitCommand[1];
                        }
                    }
                    catch
                    {
                        path = "";
                    }
                    _return = listDirectory(path);
                    break;
                case "drives":
                    _return = listDrives();
                    break;
                case "mv":
                case "move":
                    try
                    {
                        if (splitCommand.Count != 2)
                        {
                            _return = doFileMove(splitCommand[1], splitCommand[2]);
                        }
                        else
                        {
                            _return = "Not Enough Arguments. Usage: mc [FILE] [DESTINATION]";
                        }
                    }
                    catch
                    {
                        _return = "Unknown Error Encountered";
                    }
                    break;
                case "scan":
                    bool internalScan = true; //Default
                    try
                    {
                        if (splitCommand[1] == "-e")
                            internalScan = false;
                    }
                    catch
                    {
                        internalScan = true; 
                    }
                    if (internalScan)
                    {
                        _return = doInternalScan();
                    }
                    else
                    {
                        _return = doExternalScan();
                    }
                    break;
                case "scp":
                    _return = "SCP not implemented";
                    break;
                case "psi":
                    _return = "psi";
                    break;
                case "ps":
                    int startIndex;
                    try
                    {
                        startIndex = System.Convert.ToInt32(splitCommand[1]);
                    }
                    catch
                    {
                        startIndex = 1;
                    }
                    _return = getProcessList(startIndex);
                    break;
                case "wapscan":
                    _return = await doWapScan();
                    break;
                case "connections":
                    _return = "Connections not implemented";
                    break;
                case "connect":
                    try
                    {
                        // First check to see if it's a match for the IP. 
                        if (checkIPV4.IsMatch(splitCommand[1]))
                        {
                            IPConfig.CurrentConnectedIP = splitCommand[1];
                            _return = "";
                        }
                        else // not an ip, maybe a host. Try a host lookup 
                        {
                            string sEPHostName = splitCommand[1];
                            string sEPHostIP = "";
                            Task<IPHostEntry> task = Task<IPHostEntry>.Factory.StartNew(() =>
                            {
                                IPHostEntry domain= Dns.GetHostEntry(sEPHostName);
                                return domain;
                            });

                            bool success = task.Wait(1000);
                            if (success)
                            {
                                foreach( IPAddress ip in task.Result.AddressList )
                                {
                                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
                                    {
                                        sEPHostIP = ip.ToString();
                                        break;
                                    }
                                }
                                IPConfig.CurrentConnectedHostName = sEPHostName;
                                IPConfig.CurrentConnectedIP = sEPHostIP;
                                IPConfig.IsAdmin = false;
                                _return = "";
                            }
                            else
                            {
                                _return = string.Format("Invalid IP Format {0}", sEPHostIP); 
                            }
                            
                            
                        }
                    }
                    catch
                    {
                        _return = string.Format( "Could Not Find Devive at {0}", splitCommand[1] );
                    }
                    break;
                case "kill":
                    int nID;
                    try
                    {
                        nID = System.Convert.ToInt32(splitCommand[1]);
                    }
                    catch
                    {
                        nID = -1;
                    }
                    if (nID != -1)
                        _return = killProcessByID(nID);
                    else
                        _return = "Invalid PID";
                    break;
                case "probe":
                    _return = doProbe( IPConfig.CurrentConnectedIP );
                    break;
                case "help":
                    const int NUMDASHES = 26;
                    string sDASHES = (new string('-', NUMDASHES));
                    if (splitCommand.Count > 1)
                    {
                        if (splitCommand[1].ToLower() == "1")
                            _return = sDASHES + "\r\n" +
                                        showHelpPage(1) + sDASHES;
                        else if (splitCommand[1].ToLower() == "2")
                            _return = sDASHES + "\r\n" +
                                        showHelpPage(2) + sDASHES;
                        else if (splitCommand[1].ToLower() == "3")
                            _return = sDASHES + "\r\n" +
                                        showHelpPage(3) + sDASHES;
                        else
                            _return = "Additional help not implemented";
                    }
                    else
                        _return = sDASHES + "\r\n" +
                                        showHelpPage(1) + sDASHES;
                    break;
                default:
                    _return = string.Format( "No command {0} - Check Syntax", command.ToLower().Split(' ')[0] );
                    break;
            }
            return _return;
        }

        private string doProbe(string currentConnectedIP)
        {
            string _return = "";
            Scanner scanner = new Scanner();
            if (!string.IsNullOrEmpty(currentConnectedIP))
            {
                _return += string.Format("Probing {0}.............\r\n", currentConnectedIP);
                _return += "Probe Complete - Open ports:\r\n";
                _return += "--------------------------\r\n";
                foreach ( SupportedPort sp in scanner.GetRemotePorts(currentConnectedIP) )
                {
                    _return += string.Format("Port#: {0,-5} - {1}\r\n", sp.Port, sp.PortName);
                }
                _return += "--------------------------\r\n";
                _return += "Proxy Detected: NOT IMPLEMENTED\r\n";
                _return += "Firewall Detected: NOT IMPLEMENTED\r\n";

                return _return; 
            }

            return _return;
        }

        private string doFileMove(string sourceFile, string destination)
        {
            string _result = "";
            if (System.IO.File.Exists(sourceFile))
            {
                try
                {
                    System.IO.File.Move(sourceFile, destination);
                }
                catch
                {
                    _result = string.Format("Could not move {0}, check access rights", sourceFile);
                }
            }
            else
                _result = string.Format("File {0} not found!", sourceFile);

            return _result;
        }

        private string doFileDelete(string fileToDelete)
        {
            string _result = "";
            if (System.IO.File.Exists(fileToDelete))
            {
                try
                {
                    System.IO.File.Delete(fileToDelete);
                    _result = string.Format("Deleting {0}.... done", fileToDelete );
                }
                catch
                {
                    _result = string.Format("Could not delete {0}, check access rights", fileToDelete);
                }
            }
            else
                _result = string.Format("File {0} not found!", fileToDelete);

            return _result;
        }

        private string doDirectoryChange(string v)
        {
            Directory.SetCurrentDirectory(v);
            _currentDirectory = Directory.GetCurrentDirectory();
            return "";
        }

        private string listDrives()
        {
            string _return = "";
            var drives = Directory.GetLogicalDrives();

            foreach (string drive in drives)
            {
                _return += string.Format("{0}\r\n", drive);
            }

            return _return;
        }

        private string listDirectory( string sPath)
        {
            string _return = "";
            try
            {
                string sFixedPath = (sPath == "") ? Directory.GetCurrentDirectory() : sPath.Replace('/', '\\');
                var directories = Directory.GetDirectories(sFixedPath);
                int loo;
                foreach (string directory in directories)
                {
                    loo = directory.LastIndexOf('\\');
                    _return += string.Format("{0}\r\n", directory.Substring(loo + 1, directory.Length - loo - 1).ToUpper());
                }
                var files = Directory.GetFiles(sFixedPath);
                foreach (string file in files)
                {
                    loo = file.LastIndexOf('\\');
                    _return += string.Format("{0}\r\n", file.Substring(loo + 1, file.Length - loo - 1).ToLower());
                }
            }
            catch
            {
                _return = "Invalid Path Specified";
            }
            return _return;
        }

        private string killProcessByID(int nID)
        {
            Process processToShutdown = Process.GetProcessById(nID);
            string processName = processToShutdown.ProcessName;
            processToShutdown.Kill();
            return string.Format("Process {0}[{1}] Ended", nID, processName);
        }

        private string doInternalScan()
        {
            List<string> allConnections = new List<string>();
            string sReturn = "";
            TCPHelperLibrary.Scanner scanner = new TCPHelperLibrary.Scanner();

            if (true)
            {
                sReturn = "Scanning...\r\n";
                foreach (string line in scanner.GetInternalNetworkDevices())
                {
                    sReturn += line;
                }
                sReturn += "Scan Complete\r\n";
            }
            else
            {
                // TODO: FIX REMOTE HOST NOT IMPLEMENTED (YET)
            }

            return sReturn;
        }

        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);
        
        private const int MAXPROCESSESTODISPLAY = 40;
        private string getProcessList( int nPageNumber )
        {
            string _return = "";
            int MAXPAGES;
            Process[] processListInitial = Process.GetProcesses();
            List<Process> processList = new List<Process>();

            string currentLoggedInUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            // First pass, clear list of all non system processes, only user processes. 
            for (int i = processListInitial.Length -1; i >= 0; i--)
            {
                Process p = processListInitial[i];
                string user = GetProcessUser(p);
                if (user == currentLoggedInUser)
                    processList.Add(p);
            }

            MAXPAGES = (processList.Count / MAXPROCESSESTODISPLAY) + 1;
            if ( (nPageNumber - 1) < 0 ||
                (nPageNumber) >  MAXPAGES )
                {
                return "Invalid Page Specified";
                }
            else
            {
                _return = "UID            : PID           : NAME\r\n";
                int startIndex = (nPageNumber - 1) * MAXPROCESSESTODISPLAY;
                for (int x = startIndex; x < startIndex + MAXPROCESSESTODISPLAY &&
                                         x < processList.Count; x++)
                {
                    Process p = processList[x];
                    string user = GetProcessUser(p);
                    _return += string.Format("{0,-15}  {1,-15}  {2,-15}\r\n", user, p.Id, p.ProcessName);
                }

                return string.Format("{0}\r\nDisplaying Page {1} of {2}", _return,
                                nPageNumber, MAXPAGES);
            }
        }

        private string doExternalScan()
        {
            List<string> allConnections = new List<string>();

            string sEPHostName = "", sEPHostIP = "", sEPPort = "";
            string sReturn = "";
            TCPHelperLibrary.Scanner scanner = new TCPHelperLibrary.Scanner();
            List<TcpConnectionInformation> connections = scanner.GetActiveConnections();

            if (true)
            {
                sReturn = "Scanning...\r\n";
                foreach (TcpConnectionInformation ci in connections)
                {
                    sEPHostIP = ci.RemoteEndPoint.ToString().Split(':')[0];
                    sEPPort = ci.RemoteEndPoint.ToString().Split(':')[1];
                    if (checkIPV4.IsMatch(sEPHostIP) && !sEPHostIP.Equals("127.0.0.1"))
                    {
                        try
                        {
                            Task<string> task = Task<string>.Factory.StartNew(() =>
                            {
                                var domain = Dns.GetHostEntry(sEPHostIP);
                                return domain.HostName;
                            });

                            bool success = task.Wait(250);
                            if (success)
                            {
                                sEPHostName = task.Result;
                            }
                            else
                            {
                                sEPHostName = "Unresolved";
                            }
                        }
                        catch
                        {
                            sEPHostName = "Unresolved";
                        }
                        string sCurAddress = string.Format("Found Connection {0}: {1}@{2}\r\n", mapPortToName(sEPPort), sEPHostIP, sEPHostName);

                        // Toss Duplicates, Multiple local ports going out to the same destination. 
                        if (!allConnections.Contains(sCurAddress))
                        {
                            allConnections.Add(sCurAddress);
                            sReturn += sCurAddress;
                        }
                    }
                }
                sReturn += "Scan Complete\r\n";
            }
            else
            {
                // TODO: FIX REMOTE HOST NOT IMPLEMENTED (YET)
            }

            return sReturn;
        }

        private string mapPortToName(string sEPPort)
        {
            string _return;
            switch( System.Convert.ToInt32(sEPPort) )
                {
                case 21:
                    _return = "FTP";
                    break;
                case 22:
                    _return = "SSH";
                    break;
                case 23:
                    _return = "TELNET";
                    break;
                case 25:
                    _return = "SMTP";
                    break;
                case 80:
                    _return = "HTTP";
                    break;
                case 110:
                    _return = "POP";
                    break;
                case 139:
                    _return = "NETBIOS";
                    break;
                case 143:
                    _return = "IMAP";
                    break;
                case 443:
                    _return = "HTTPS";
                    break;
                case 587:
                    _return = "SMTP";
                    break;
                case 1024:
                    _return = "RPC";
                    break;
                case 9354:
                    _return = "VSTUDIO";
                    break;
                case 27021:
                    _return = "STEAM";
                    break;
                default:
                    _return = string.Format("{0}", System.Convert.ToInt32(sEPPort));
                    break;
            }
            return string.Format( "{0}({1})", new string( ' ', 7 - _return.Length ), _return);
        }

        private string showHelpPage(int page)
        {
            string _return = "";
            switch (page)
            {
                case 1:
                    _return += "Command List - Page 1 of 3\r\n";
                    _return += "\r\n";
                    _return += " help [PAGE NUMBER]\r\n";
                    _return += "   Displays the specified page of commands.\r\n";
                    _return += "\r\n";
                    _return += " scp [filename[OPTIONAL: destination]\r\n";
                    _return += "   Copies file named [filename[ from remote machine to specific local folder (/bin default)\r\n";
                    _return += "\r\n";
                    _return += " scan [-i(NTERNAL,DEFAULT)|-e(XTERNAL)]\r\n";
                    _return += "   Scans for links on the connected machine and adds them to the Map.\r\n";
                    _return += "\r\n";
                    _return += " rm [filenmae (or use * for all files in folder)]\r\n";
                    _return += "   Deletes specified file(s)\r\n";
                    _return += "\r\n";
                    _return += " ps [PAGE NUMBER]\r\n";
                    _return += "   Lists currently running processes and their PIDs\r\n";
                    _return += "\r\n";
                    _return += " psi\r\n";
                    _return += "   Lists currently running processes IN APP and their PIDs\r\n";
                    _return += "\r\n";
                    _return += " kill [PID]\r\n";
                    _return += "   Kills Process number [PID]\r\n";
                    _return += "\r\n";
                    _return += " ls\r\n";
                    _return += "   Lists all files in current directory\r\n";
                    _return += "\r\n";
                    _return += " cd [foldername]\r\n";
                    _return += "   Moves current working directory to the specified folder\r\n";
                    _return += "\r\n";
                    _return += " mv [FILE][DESTINATION]\r\n";
                    _return += "   Moves or renames [FILE] to [DESTINATION]\r\n";
                    _return += "   (i.e: mv hi.txt ../bin/hi.txt)\r\n";
                    _return += "\r\n";
                    _return += " connect [ip]\r\n";
                    _return += "   Connect to an External Computer\r\n";
                    _return += "\r\n";
                    _return += "\r\n";
                    _return += "help [PAGE NUMBER]\r\n";
                    _return += " Displays the specified page of commands.\r\n";
                    break;
                case 2:
                    _return += "Command List - Page 2 of 3\r\n";
                    _return += "\r\n";
                    _return += " probe\r\n";
                    _return += "   Scans the connected machine for\r\n";
                    _return += "     active ports and security level.\r\n";
                    _return += "\r\n";
                    _return += " exe\r\n";
                    _return += "   Lists all available executables in the local /bin/ folder (Includes hidden and embedded\r\n";
                    _return += "executables)\r\n";
                    _return += "\r\n";
                    _return += " disconnect\r\n";
                    _return += "   Terminate the current open connection. ALT: \"dc\"\r\n";
                    _return += "\r\n";
                    _return += " openCDTray\r\n";
                    _return += "   Opens the connected Computer's CD Tray\r\n";
                    _return += "\r\n";
                    _return += " closeCDTray\r\n";
                    _return += "   Closes the connected Computer's CD Tray\r\n";
                    _return += "\r\n";
                    _return += " reboot [OPTIONAL: -i]\r\n";
                    _return += "   Reboots the connected computer. The -i flag reboots instantly\r\n";
                    _return += "\r\n";
                    _return += " replace [filename]\"target\" \"replacement\"\r\n";
                    _return += "   Replaces the target text in the file with the replacement\r\n";
                    _return += "\r\n";
                    _return += " analyze\n";
                    _return += "   Performs an analysis pass on the firewall of the target machine\r\n";
                    _return += "\r\n";
                    _return += " solve [FIREWALL SOLUTION]\n";
                    _return += "   Attempts to solve the firewall of target machine to allow UDP Traffic\r\n";
                    _return += "\r\n";
                    _return += "\r\n";
                    _return += "help [PAGE NUMBER]\r\n";
                    _return += " Displays the specified page of commands.\r\n";
                    break;
                case 3:
                    _return += "Command List - Page 3 of 3\r\n";
                    _return += "\r\n";
                    _return += "login\r\n";
                    _return += "   Requests a username and password to log in to the connected system.\r\n";
                    _return += "\r\n";
                    _return += " upload [LOCAL FILE PATH]\r\n";
                    _return += "   Uploads the indicated file on your local machine to the current connected directory\r\n";
                    _return += "\r\n";
                    _return += " clear\r\n";
                    _return += "   Clears the terminal\r\n";
                    _return += "\r\n";
                    _return += " addNote [NOTE]\r\n";
                    _return += "   Add Note\r\n";
                    _return += "\r\n";
                    _return += " append [FILENAME][DATA]\r\n";
                    _return += "   Appends a line containing [DATA] to [FILENAME]\r\n";
                    _return += "\r\n";
                    _return += " shell\r\n";
                    _return += "   Opens a remote access shell on a target machine with Proxy overload\r\n";
                    _return += "\r\n";
                    _return += " drives\r\n";
                    _return += "   Retrieves a list of drives available on this system\r\n";
                    _return += "\r\n";
                    _return += "\r\n";
                    _return += "help [PAGE NUMBER]\r\n";
                    _return += " Displays the specified page of commands.\r\n";
                    break;
            }

            return _return;
        }

        private async Task<string> doWapScan()
        {
            string _result = "";

            WIFIAccessor wifi = new WIFIAccessor();
            var wfnws = await wifi.GetAccessPointsAsync();

            if (wfnws.Count > 0)
            {

                foreach (WiFiAvailableNetwork wan in wfnws)
                {
                    _result += string.Format("SSID={0}~DB={1}~KIND={2}~BARS={3}~UPTIME={4}|", 
                                    wan.Ssid, wan.NetworkRssiInDecibelMilliwatts, 
                                    wan.NetworkKind,wan.SignalBars, wan.Uptime );
                }
            }
            else
            {
                _result = string.Format("No DATA\n");
            }
            
            return _result;
        }
    }
}
