using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;

namespace WAPHelperLibrary
{
    public class WIFIAccessor
    {
        private IReadOnlyList<WiFiAdapter> wiFiAdapters;
        public WIFIAccessor() { }

        public async Task<List<WiFiAvailableNetwork>> GetAccessPointsAsync()
        {
            List<string> apList = new List<string>();
            List<WiFiAvailableNetwork> an = new List<WiFiAvailableNetwork>();

            var result = await WiFiAdapter.RequestAccessAsync();
            if (result == WiFiAccessStatus.Allowed)
            {
                wiFiAdapters = await WiFiAdapter.FindAllAdaptersAsync();
                foreach (var adapter in wiFiAdapters)
                {
                    foreach (var network in adapter.NetworkReport.AvailableNetworks)
                    {
                        if (apList.Count == 0 || !apList.Contains(network.Ssid) )
                            {
                            an.Add(network);
                            apList.Add(network.Ssid);
                            }
                    }
                }
            }
            an.Sort((n1, n2) => n1.Ssid.CompareTo(n2.Ssid));
            return an;
        }
    }
}
