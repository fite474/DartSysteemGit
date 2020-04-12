using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dartssystem
{
    public partial class LobbyForm : Form
    {
       // Client client;
        private static System.Timers.Timer timer;
        public List<String> _clientnames = new List<string>();
       
        internal Client Client { get; set; }

        public LobbyForm()
        {
            InitializeComponent();
            if (Client!=null)
            {
                Client = new Client();
            }

            //listView1.
            
            SetTimer();

        }

    private void SetTimer()
    {
        timer = new System.Timers.Timer(2000);
        timer.Elapsed += Timer_Elapsed;
        timer.AutoReset = true;
    }

        public void UpdatePlayerList()
        {
            
            //string response = Client.ReadTextMessage(Client.TCPClient);
            //if (response.Contains("playerNames"))
            //{
            //    _clientnames.Add(response);
            //}
            //foreach (string waitingPlayers in _clientnames)
            //{
            //    //var players = _clientnames[0]; ;
            //    listView1.Items.Add(waitingPlayers);
            //}
        }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        string response = Client.ReadTextMessage(Client.TCPClient);
        if (response.Contains("playerNames"))
        {
                Console.WriteLine("dsd");
           // _clientnames.Add(response);
        }


    }


    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("clicked: ");
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count != 0)
            {
                Client.WriteTextMessage(Client.TCPClient, "+SendName+");
            }
            


            string response = Client.ReadTextMessage(Client.TCPClient);
            if (response.Contains("playerNames"))
            {
                _clientnames.Add(response);
            }
            foreach (string waitingPlayers in _clientnames)
            {
                //var players = _clientnames[0]; ;
                listView1.Items.Add(waitingPlayers);
            }
            Console.WriteLine("sdd");
            
        }
    }
}
