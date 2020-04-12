using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public class ClientData : IUserData
    {
        public string Name { get; }
        public TcpClient TcpClient { get; }


        public ClientData(TcpClient tcpClient, string name)
        {
            Name = name;
            TcpClient = tcpClient;

        }
    }
}
