using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Dartssystem
{
    class Client
    {
        public List<String> _clientnames = new List<string>();
        public TcpClient TCPClient { get; set; }
        public Client()
        {
            //TCPClient = new TcpClient("192.168.178.15", 1330);
            //TCPClient = new TcpClient("192.168.1.141", 25565);
            TCPClient = new TcpClient("80.114.189.94", 25565);
            
        }
        public void WriteTextMessage(TcpClient client, string message)
        {
            NetworkStream stream = client.GetStream();


            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            //var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            //stream.WriteLine(message);
            //stream.Flush();
        }

        public string ReadTextMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            Byte[] data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Client Received: {0}", responseData);



            //StreamReader stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            //try
            //{
            //    string line = stream.ReadLine();
            //    if (line.Contains("playerNames"))
            //    {
            //        _clientnames.Add(line);
            //    }

            return responseData;//line; ;
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("ERROR");
            //    return "ERROR";
            //}
        }
    }
}
