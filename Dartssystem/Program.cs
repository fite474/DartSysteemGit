using Dartssystem.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dartssystem
{
    class Program
    {
        private BlockingCollection<Datagram> networkMessageQueue;
        private ServerNetwork serverNetwork;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            new Program();
            
        }

        public Program()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            serverNetwork = new ServerNetwork(new Networking(IPAddress.Loopback.ToString(), 25565));
            networkMessageQueue = new BlockingCollection<Datagram>();
            serverNetwork.OnReceiveResponse += OnHandleResponse;
            //Application.Run(new DartsScoreboard());
            Application.Run(new StartingForm());
        }


        public void OnHandleResponse(Datagram response)
        {
            networkMessageQueue.Add(response);
        }
    }
}
