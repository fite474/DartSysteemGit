using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartssystem.Network
{
    public enum DataType
    {
        Login,
        Logout,
        AddBikeData,
        ChangeBike,
        SendEmergencyBreak,
        SendMessage,  //***y
        AddClient, //y
        StartSession, //y
        EndSession, //y
        GetClientsFromDoctor, //y
        GetClientSnapshots, //y
        OnlineClientEvent,
        UpdateClientsManual,
    }
}
