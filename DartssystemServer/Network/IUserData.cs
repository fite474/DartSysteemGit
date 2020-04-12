using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public interface IUserData
    {
        TcpClient TcpClient { get;  }
        string Name { get; }
    }
}
