using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThisIsAHackerServiceLibrary
{
    public class InternalCommandProcessor
    {
        private List<string> _commands = new List<string>();
        public ReadOnlyCollection<string> Commands 
        { 
            get { return _commands.AsReadOnly(); } 
        }  
        
        public InternalCommandProcessor() 
        {
            _commands.Add("PortHack");
            _commands.Add("FormBomb");
            _commands.Add("Shell");
            _commands.Add("SSHcrack");
            _commands.Add("FTPBounce");
            _commands.Add("eosDeviceScan");
            _commands.Add("SQL_Memcorrupt");
            _commands.Add("Sequencer");
            _commands.Add("ThemeChanger");
            _commands.Add("Decypher");
            _commands.Add("DECHead");
            _commands.Add("KBT_PortTest");
            _commands.Add("TraceKill");
            _commands.Add("Hacknet");
            _commands.Add("SecurityTracer");
            _commands.Add("Clock");
            _commands.Add("FTPSprint");
            _commands.Add("TorrentStreamInjector");
            _commands.Add("SSLTrojan");
            _commands.Add("SignalScramble");
            _commands.Add("MemForensics");
            _commands.Add("MemDumpGenerator");
        }

        public bool IsInternalCommand( string commandName) 
        { 
            if( _commands.Contains(commandName, StringComparer.OrdinalIgnoreCase ) ) { return true; }
            else { return false; }  
        }
    }
}
