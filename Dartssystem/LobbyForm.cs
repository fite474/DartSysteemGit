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
        private Client client;

        internal Client Client { get => client; set => client = value; }

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

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        string response = Client.ReadTextMessage(Client.TCPClient);
        if (response.Contains("playerNames"))
        {
           // _clientnames.Add(response);
        }


    }


    private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("clicked: ");
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            string response = Client.ReadTextMessage(Client.TCPClient);
            if (response.Contains("playerNames"))
            {
                _clientnames.Add(response);
            }
            //client.ReadTextMessage(client.TCPClient);
            var item1 = new ListViewItem(new[] { "text1", "text2", "text3", "text4" });
            var players = _clientnames[0]; ;
            listView1.Items.Add(players);
        }
    }
}
