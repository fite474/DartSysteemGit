using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DartssystemServer.Network
{
    public delegate void ReceivedData(Datagram datagram);

    public class Networking : INetwork
    {
        private object eventSyncLock;
        private TcpClient client;
        private NetworkStream stream;
        private string messageBuffer;
        private byte[] receiveBuffer;

        private const string NetworkPostfix = "\r\n\r\n";


        public event ReceivedData OnReceivedData;

        event ReceivedData INetwork.OnReceivedData
        {
            add
            {
                lock (eventSyncLock)
                {
                    OnReceivedData += value;
                }
            }
            remove
            {
                lock (eventSyncLock)
                {
                    OnReceivedData -= value;
                }
            }
        }

        public Networking(string ip, int port)
        {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
            eventSyncLock = new object();

        }

        public Networking(TcpClient client)
        {
            this.client = client;
            stream = client.GetStream();
            eventSyncLock = new object();
        }


        public void SendDatagram(Datagram jsonData)
        {
            lock (LockHelper.NetworkWriteLock)
            {
                string jsonString = JsonConvert.SerializeObject(jsonData);
                string encryptedString = Crypto.Encrypt(jsonString);
                byte[] jsonStringByteData = Encoding.UTF8.GetBytes(jsonString + NetworkPostfix);
                client.GetStream().BeginWrite(jsonStringByteData, 0, jsonStringByteData.Length, null, null);
            }

        }


        public void StartReceivingDatagrams()
        {
            receiveBuffer = new byte[1024];
            stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnHandleNextPacket, null);
        }


        private Datagram DecodeToJson(string encodedMessage)
        {
            string decryptedMessage = Crypto.Decrypt(encodedMessage);
            return JsonConvert.DeserializeObject<Datagram>(decryptedMessage);
        }

        private void ProcessMessages(string[] currentMessages)
        {
            foreach (var currentMessage in currentMessages)
            {
                try
                {
                    Datagram datagram = DecodeToJson(currentMessage);
                    OnReceivedData?.Invoke(datagram);
                }
                catch (JsonReaderException e)
                {
                    Console.WriteLine("[TcpNetwork] one message was discarded because it was malformed");
                }
            }
        }


        private void OnHandleNextPacketOld(IAsyncResult handlePacketResult)
        {
            int bytesReceived = stream.EndRead(handlePacketResult);
            string incompleteMessage = string.Empty;
            messageBuffer += Encoding.UTF8.GetString(receiveBuffer.Take(bytesReceived).ToArray());
            int lastIndex = messageBuffer.LastIndexOf(NetworkPostfix);
            if (lastIndex < 0)
            {
                incompleteMessage = messageBuffer;
                stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnHandleNextPacketOld, null);
                return;
            }
            //string minus de index tekens kan ook endswith gebruiken
            if (lastIndex < messageBuffer.Length - 5)
            {
                incompleteMessage = messageBuffer.Substring(lastIndex);
                messageBuffer = messageBuffer.Substring(0, lastIndex);
            }
            string[] s = messageBuffer.Split(new string[] { NetworkPostfix }, StringSplitOptions.None);
            ProcessMessages(messageBuffer.Split(new string[] { NetworkPostfix }, StringSplitOptions.None));
            messageBuffer = incompleteMessage;

        }

        private void OnHandleNextPacket(IAsyncResult handlePacketResult)
        {
            int bytesReceived = stream.EndRead(handlePacketResult);
            string tempBuffer = string.Concat(
                messageBuffer,
                Encoding.UTF8.GetString(receiveBuffer.Take(bytesReceived).ToArray())
            );
            string[] messages = tempBuffer.Split(new string[] { NetworkPostfix }, StringSplitOptions.None);
            if (messages.Length > 0)
            {
                ProcessMessages(
                    messages.Take(messages.Length - 1).ToArray()
                );

                messageBuffer = messages.Last();
            }
            stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnHandleNextPacket, null);

        }

        public void StopReceivingDatagrams()
        {
            //TODO cancel requests
        }


        public void Dispose()
        {
            client.Close();
        }

        #region Deprecated
        [Obsolete]
        public string Send(string request)
        {
            stream = client.GetStream();
            byte[] responseBuffer;
            byte[] commmand = Encoding.UTF8.GetBytes(request);
            lock (this)
            {
                stream.Write(BitConverter.GetBytes(commmand.Length), 0, 4);
                stream.Write(commmand, 0, commmand.Length);

                byte[] buffer = new byte[4];

                stream.Read(buffer, 0, buffer.Length);

                //The first 4 bytes determine the length of the buffer
                int bufferLength = BitConverter.ToInt32(buffer.Take(4).ToArray(), 0);
                int byteCount = 0;


                responseBuffer = new byte[bufferLength];

                int defaultFill = 1024;
                while (byteCount != bufferLength)
                {
                    //Array that is to be inserted into the responsebuffer. 
                    byte[] tempArray;
                    if ((bufferLength - byteCount) < defaultFill)
                    {
                        tempArray = new byte[bufferLength - byteCount];
                    }
                    else
                    {
                        tempArray = new byte[defaultFill];
                    }
                    //The receivedBytes is the actual amount of bytes that fills the tempArray.
                    //Using this, we can add only the actual values to the responseBuffer, and filter out the null values 
                    int receivedBytes = stream.Read(tempArray, 0, tempArray.Length);

                    //Using receivedBytes as a length, we only add the actual values to the responseBuffer (as described above)
                    Array.Copy(tempArray, 0, responseBuffer, byteCount, receivedBytes);
                    byteCount += receivedBytes;

                }
            }
            return Encoding.Default.GetString(responseBuffer);
        }
        #endregion


    }
}

