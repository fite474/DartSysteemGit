using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dartssystem
{
    public partial class StartingForm : Form
    {
        Client client;
        Form gamescreen;
        LobbyForm lobbyForm;
        public String Name { get; set; }
        private static System.Timers.Timer timer;
        public StartingForm()
        {
            InitializeComponent();
            this.client = new Client();
            if(client!=null)
            {
                WelkomLabel.Text = "Verbonden met server";
            }
            //gamescreen = new DartsScoreboard();
            //lobbyForm = new LobbyForm();
            //lobbyForm.Client = this.client;
           // SetTimer();
            
        }


        private void SetTimer()
        {
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           // lobbyForm.ShowDialog();
            // string response = client.ReadTextMessage(client.TCPClient);
            //if(response.Equals("0000"))
            //{
            //    //gamescreen.ShowDialog();
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Name = playerNameTextbox.Text;
            //string response = client.ReadTextMessage(client.TCPClient);
            //Console.WriteLine(response);
            //string name = ("+AddName+" + playerNameTextbox.Text);
            //client.WriteTextMessage(client.TCPClient, name);
            //string response = client.ReadTextMessage(client.TCPClient);
            //if (response.Contains("PlayerNames"))
            //{
                lobbyForm.ShowDialog();
            //}
            //lobbyForm.UpdatePlayerList();
            
            

        }

        
    }
}
