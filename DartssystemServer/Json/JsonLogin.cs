using Dartssystem.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Json
{
    class JsonLogin : IJsonData
    {

        public TcpClient TcpClient { get; set; }
        public string Name { get; set; }


    }
}
