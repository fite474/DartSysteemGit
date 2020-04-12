using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Dartssystem.Json;
using DartssystemServer.Json;

namespace Dartssystem.Network
{
    public delegate void ReceiveResponse(Datagram jsonResponse);

    class ServerNetwork : IDisposable
    {
        private INetwork network;

        public event ReceiveResponse OnReceiveResponse;

        public ServerNetwork(INetwork network)
        {
            this.network = network;
            network.StartReceivingDatagrams();
            network.OnReceivedData += HandleResponse;
        }

        public void Dispose()
        {
            network.Dispose();
        }

        public void RequestLogin(TcpClient tcpClient, string name)
        {
            var loginRequest = new Datagram();
            loginRequest.DataType = DataType.Login;
            loginRequest.Data = new JsonLogin
            {
                TcpClient = tcpClient,
                Name = name
            };
            network.SendDatagram(loginRequest);
            //TODO check if sync is not needed

        }





        public void HandleResponse(Datagram jsonResponse)
        {
            Task.Run(() => OnReceiveResponse?.Invoke(jsonResponse));
        }
    }
}
