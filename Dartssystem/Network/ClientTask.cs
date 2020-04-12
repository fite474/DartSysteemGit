using Dartssystem.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dartssystem.Network
{
    class ClientTask : TcpTask
    {
        private ThreadSafeList<TcpTask> connnectedTasks;
        private ClientData currentUser;


        public ClientTask(INetwork network, ClientData currentUser, ThreadSafeList<TcpTask> connectedClients) : base(network, currentUser)
        {
            connnectedTasks = connectedClients;
            this.currentUser = currentUser;
        }

        public override void Start()
        {
            //OnTryPullDoctor();
            Network.OnReceivedData += OnReceivedNextRequest;
        }

        public override void Stop()
        {
            Network.OnReceivedData += OnReceivedNextRequest;
            connnectedTasks.RemoveAll((task) => task == this);
        }

        public void OnReceivedNextRequest(Datagram requestData)
        {
            switch (requestData.DataType)
            {
                case DataType.SendClientNames:
                    {
                        JsonClientNames d = JsonHelper.ToConcreteType<JsonClientNames>(requestData.Data);


                        SendToClient(new Datagram()
                        {
                            DataType = DataType.SendClientNames,
                            Data = new JsonGetClientNamesResponse()
                            {
                                //ClientNames = ;
                            }
                        });

                        break;
                    }
                case DataType.Logout:
                    Stop();
                    break;
            }
        }

        public void OnTryPullDoctor()
        {
            //bool found = false;
            //while (!found)
            //{
            //    Console.WriteLine("THID [Make client]: " + Thread.CurrentThread.ManagedThreadId);
            //    Thread.Sleep(200);
            //    doctor = GetDoctorFromClient();
            //    if (doctor != null)
            //    {
            //        found = true;
            //        LockHelper.ClientIdListLock.EnterReadLock();
            //        doctor.UpdateOnlineClients();
            //        LockHelper.ClientIdListLock.ExitReadLock();
            //    }
            //}
        }

        public void SendToClient(Datagram request)
        {
            Network.SendDatagram(request);
        }

        //private DoctorTask GetDoctorFromClient()
        //{
            //foreach (DoctorData currentDoctor in serverData.Doctors)
            //{

            //    foreach (string currentClient in currentDoctor.ClientIds)
            //    {
            //        if (currentClient == currentUser.Id) // De id van de client moet al bekend zijn door het eerste bericht
            //        {
            //            TcpTask doc = null;
            //            lock (AuthorizationTask.OnlineListSyncRoot)
            //            {
            //                try { doc = connnectedTasks.Find(x => x.UserData.Id == currentDoctor.Id); }// Werkt dit?
            //                catch (NullReferenceException e) { e.ToString(); }

            //                if (doc != null)
            //                {
            //                    return (DoctorTask)doc;
            //                }
            //                else return null;
            //            }
            //        }
            //    }

            //}
        //    return null;
        //}


    }
}