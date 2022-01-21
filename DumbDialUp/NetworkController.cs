using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DumbDialUp
{
    public class NetworkController
    {

        private NetworkInterface nw;
        
        public NetworkController()
        {
            // Get the adaptor in use
            nw = GetActiveNetworkInterface();

            // Disable it
            if (nw != null)
            {
                Disable();
            }
        }

        /// <summary>
        /// Gets the active network connection
        /// </summary>
        /// <returns></returns>
        private NetworkInterface GetActiveNetworkInterface()
        {
            NetworkInterface adapter = null;

            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Ignore tunnel and loopback devices
                if (ni.OperationalStatus == OperationalStatus.Up &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                    ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    adapter = ni;
                }
            }
            return adapter;
        }


        public void Enable()
        {
            ProcessStartInfo psi =
                   new ProcessStartInfo("netsh", "interface set interface \"" + nw.Name + "\" enable");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process p = new Process{StartInfo = psi};
            p.Start();
        }

        public void Disable()
        {
            ProcessStartInfo psi =
                new ProcessStartInfo("netsh", "interface set interface \"" + nw.Name + "\" disable");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process p = new Process{StartInfo = psi};
            p.Start();
        }
    }
}
