using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public abstract class TcpTask
    {
        public INetwork Network { get; }

        public IUserData UserData { get; }

        public abstract void Start();

        public abstract void Stop();

        public TcpTask(INetwork network, IUserData userData)
        {
            UserData = userData;
            Network = network;
        }
    }
}