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
        public TcpClient TCPClient { get; set; }
        public Client()
        {
            //TCPClient = new TcpClient("192.168.178.15", 1330);
            //TCPClient = new TcpClient("192.168.1.141", 25565);
            TCPClient = new TcpClient("80.114.189.94", 25565);
            
        }
        public void WriteTextMessage(TcpClient client, string message)
        {
            var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
            stream.WriteLine(message);
            stream.Flush();
        }

        public string ReadTextMessage(TcpClient client)
        {
            StreamReader stream = new StreamReader(client.GetStream(), Encoding.ASCII);
            try
            {
                string line = stream.ReadLine(); ;
                return line;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR");
                return "ERROR";
            }
        }
    }
}
