using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Dartssystem
{
    public partial class DartsScoreboard : Form
    {
        Client client;
        //Client client2;
        System.Timers.Timer timer;
        Label playernamelabel;
        public DartsScoreboard()
        {
            InitializeComponent();
            client = new Client();
           // client2 = new Client();
            SetTimer();
        }

        private void SetTimer()
        {
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            string response = client.ReadTextMessage(client.TCPClient);

            if (response.StartsWith("0"))//speler is speler 1
            {
                //Naamgerelateerde zaken

                int dashindex = response.IndexOf('-');
                string playerName = response.Substring(1, dashindex - 1);
                Player1NameLabel.Invoke((MethodInvoker)(() => Player1NameLabel.Text = playerName));
                Player2NameLabel.Invoke((MethodInvoker)(() => Player2NameLabel.Text = response.Substring(dashindex + 1)));

                button1.Invoke((MethodInvoker)(() => button1.Visible = false));
                button1.Invoke((MethodInvoker)(() => button1.Enabled = false));
                Naamveld.Invoke((MethodInvoker)(() => Naamveld.Visible = false));
                Naamveld.Invoke((MethodInvoker)(() => Naamveld.Enabled = false));

                player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Enabled = true));
                player1ConfirmButton.Invoke((MethodInvoker)(() => player1ConfirmButton.Enabled = true));

                /*player2ScoreInputTextbox.Invoke((MethodInvoker)(() => player2ScoreInputTextbox.Enabled = false));
                player2ConfirmButton.Invoke((MethodInvoker)(() => player2ConfirmButton.Enabled = false));
                player2ScoreInputTextbox.Invoke((MethodInvoker)(() => player2ScoreInputTextbox.Visible = false));
                player2ConfirmButton.Invoke((MethodInvoker)(() => player2ConfirmButton.Visible = false));*/
            }
            else if (response.StartsWith("1")) // speler is speler 2
            {
                int dashindex = response.IndexOf('-');
                string playerName = response.Substring(1, dashindex - 1);
                Player2NameLabel.Invoke((MethodInvoker)(() => Player2NameLabel.Text = playerName));
                Player1NameLabel.Invoke((MethodInvoker)(() => Player1NameLabel.Text = response.Substring(dashindex + 1)));

                button1.Invoke((MethodInvoker)(() => button1.Visible = false));
                button1.Invoke((MethodInvoker)(() => button1.Enabled = false));
                Naamveld.Invoke((MethodInvoker)(() => Naamveld.Visible = false));
                Naamveld.Invoke((MethodInvoker)(() => Naamveld.Enabled = false));

                player2ScoreInputTextbox.Invoke((MethodInvoker)(() => player2ScoreInputTextbox.Enabled = true));
                player2ConfirmButton.Invoke((MethodInvoker)(() => player2ConfirmButton.Enabled = true));

                player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Enabled = false));
                player1ConfirmButton.Invoke((MethodInvoker)(() => player1ConfirmButton.Enabled = false));
                player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Visible = false));
                player1ConfirmButton.Invoke((MethodInvoker)(() => player1ConfirmButton.Visible = false));
            }
            else if(response.StartsWith("2+")) //Scoreupdate
            {
                string cutResponse = response.Substring(2);
                int plusindex = cutResponse.IndexOf('+');
                string p1Scores = cutResponse.Substring(0, plusindex);
                string p2Scores = cutResponse.Substring(plusindex + 1);
                string[] scoresp1 = p1Scores.Split('-'); //split score left from previous throws 
                string[] scoresp2 = p2Scores.Split('-');

                Player1ScoreLeftLabel.Invoke((MethodInvoker)(() => Player1ScoreLeftLabel.Text = scoresp1[0]));
                Player2ScoreLeftLabel.Invoke((MethodInvoker)(() => Player2ScoreLeftLabel.Text = scoresp2[0]));

                if(scoresp1.Length > 1)
                {
                    string[] previousThrows = scoresp1[1].Split('_');
                    p1Scores = "";
                    foreach (string score in previousThrows)
                    {
                        string[] splitted = score.Split('=');
                        if(int.Parse(splitted[0]) > 100)
                        {
                            p1Scores += $"{splitted[0]}   ||    {splitted[1]}\n";
                        }
                        else
                        {
                            p1Scores += $"{splitted[0]}    ||    {splitted[1]}\n";
                        }
                        
                    }
                }
                else
                {
                    p1Scores = "";
                }
                
                if(scoresp2.Length > 1)
                {
                    string[] previousThrows = scoresp2[1].Split('_');
                    p2Scores = "";
                    foreach (string score in previousThrows)
                    {
                        string[] splitted = score.Split('=');
                        if (int.Parse(splitted[0]) > 100)
                        {
                            p2Scores += $"{splitted[0]}   ||    {splitted[1]}\n";
                        }
                        else 
                        { 
                            p2Scores += $"{splitted[0]}    ||    {splitted[1]}\n";
                        }
                    }
                }
                else
                {
                    p2Scores = "";
                }

                Player1ScoredLabel.Invoke((MethodInvoker)(() => Player1ScoredLabel.Text = p1Scores));
                player2ScoredLabel.Invoke((MethodInvoker)(() => player2ScoredLabel.Text = p2Scores));

            }
            else if (response.StartsWith("2")) //leg start update
            {
                int dashindex = response.IndexOf('-');
                string p1score = response.Substring(1, dashindex - 1);
                string p2score = response.Substring(dashindex + 1);
                Player1ScoreLeftLabel.Invoke((MethodInvoker)(() => Player1ScoreLeftLabel.Text = p1score));
                Player2ScoreLeftLabel.Invoke((MethodInvoker)(() => Player2ScoreLeftLabel.Text = p2score));
            }
            else if (response.Equals("THROW"))
            {
                player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Enabled = true));
                player1ConfirmButton.Invoke((MethodInvoker)(() => player1ConfirmButton.Enabled = true));
                p1Waitlabel.Invoke((MethodInvoker)(() => p1Waitlabel.Text = "Jouw beurt!"));
            }
            else if (response.Equals("WAIT"))
            {
                p1Waitlabel.Invoke((MethodInvoker)(() => p1Waitlabel.Text = "Wachten op tegenstander.."));
            }
            else if(response.StartsWith("5")) //end of a leg, new leg score update
            {
                response = response.Substring(1);
                LegsScoredLabel.Invoke((MethodInvoker)(() => LegsScoredLabel.Text = response));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void player1ConfirmButton_Click(object sender, EventArgs e)
        {
            string player1scoreinput = player1ScoreInputTextbox.Text.ToLower();
            client.WriteTextMessage(client.TCPClient, player1scoreinput);

            player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Text = ""));

            player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Enabled = false));
            player1ConfirmButton.Invoke((MethodInvoker)(() => player1ConfirmButton.Enabled = false));

            player2ScoreInputTextbox.Invoke((MethodInvoker)(() => player2ScoreInputTextbox.Enabled = true));
            player2ConfirmButton.Invoke((MethodInvoker)(() => player2ConfirmButton.Enabled = true));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String name = Naamveld.Text;
            Naamveld.Invoke((MethodInvoker)(() => Naamveld.Text = ""));
            client.WriteTextMessage(client.TCPClient, name);
        }

        private void player2ConfirmButton_Click(object sender, EventArgs e)
        {
            string player2scoreinput = player2ScoreInputTextbox.Text.ToLower();
            client.WriteTextMessage(client.TCPClient, player2scoreinput);

            player2ScoreInputTextbox.Invoke((MethodInvoker)(() => player2ScoreInputTextbox.Text = ""));

            player1ScoreInputTextbox.Invoke((MethodInvoker)(() => player1ScoreInputTextbox.Enabled = true));
            player1ConfirmButton.Invoke((MethodInvoker)(() => player1ConfirmButton.Enabled = true));

            player2ScoreInputTextbox.Invoke((MethodInvoker)(() => player2ScoreInputTextbox.Enabled = false));
            player2ConfirmButton.Invoke((MethodInvoker)(() => player2ConfirmButton.Enabled = false));
        }
    }
}
