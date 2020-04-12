using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public class ClientData : IUserData
    {
        public string Name { get; }
        public string Id { get; }


        public ClientData(string name, string id)
        {
            Name = name;
            Id = id;

        }
    }
}
