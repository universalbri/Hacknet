using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using Windows.Devices;

namespace TCPHelper.System
{
    public class MemoryMetrics
    {
        public double Total;
        public double Used;
        public double Free;

        public MemoryMetrics() 
        {
            ComputerInfo ci = new ComputerInfo();

            Total = ci.TotalPhysicalMemory;
            Used = ci.TotalPhysicalMemory - ci.AvailablePhysicalMemory;
            Free = ci.AvailablePhysicalMemory;
        }


    }
}
