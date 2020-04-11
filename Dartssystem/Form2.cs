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
        private static System.Timers.Timer timer;
        public StartingForm()
        {
            InitializeComponent();
            client = new Client();
            if(client!=null)
            {
                WelkomLabel.Text = "Verbonden met server";
            }
            //gamescreen = new DartsScoreboard();
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
            string response = client.ReadTextMessage(client.TCPClient);
            if(response.Equals("0000"))
            {
                //gamescreen.ShowDialog();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = playerNameTextbox.Text;
            client.WriteTextMessage(client.TCPClient, name);
            

        }

        
    }
}
