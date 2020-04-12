using Dartssystem.Json;
using DartssystemServer.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    class AuthorizationTask
    {
        public static object OnlineListSyncRoot;
        private ThreadSafeList<TcpTask> currentTasks;
        private INetwork network;

        public AuthorizationTask(INetwork network, ThreadSafeList<TcpTask> currentTasks)
        {
            OnlineListSyncRoot = new object();
            this.network = network;
            this.currentTasks = currentTasks;
            network.OnReceivedData += OnAuthorizeConnection;
        }

        public void OnAuthorizeConnection(Datagram received)
        {
            TcpTask currentTask = null;
            if (received.DataType == DataType.Login)
            {
                var loginRequest = JsonHelper.ToConcreteType<JsonLogin>(received.Data);
               
                    //moet voor client lists in server data want we voegen data toe maar lezen ook.
                    LockHelper.ClientListLock.EnterReadLock();
                 ClientData currentUser = new ClientData(loginRequest.TcpClient, loginRequest.Name);
                
                    //ClientData currentUser = FindClientWithPassword(loginRequest.Id, loginRequest.Password);
                    LockHelper.ClientListLock.ExitReadLock();
                    if (currentUser != null)
                    {
                        // ZORGT VOOR DEADLOCK!!!
                        ClientTask clientTask = new ClientTask(network, currentUser, currentTasks);
                        currentTask = clientTask;
                        currentTasks.Add(clientTask);
                    }
                
            }


            var response = new Datagram();
            response.DataType = DataType.Login;
            if (currentTask != null)
            {
                response.Data = new JsonResponse
                {
                    Error = "200",
                    Message = "LoginOK"
                };
                network.SendDatagram(response);
                currentTask.Start();
            }
            else
            {
                response.Data = new JsonResponse
                {
                    Error = "500",
                    Message = "LoginWrong"
                };
                network.SendDatagram(response);
            }

        }





    }
}
