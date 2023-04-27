using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPHelperLibrary
{
    public class ProcessHandler
    {
        public ProcessHandler() { }

        public Process Create( string sExecutable, string arguments )
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo( sExecutable, arguments )
                {
                    FileName = sExecutable,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };

            return process;
        }
    }
}
