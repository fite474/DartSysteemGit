using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DartsystemServer
{
    class Program
    {
        public class Lobby
        {
            private TcpListener _listener;
            //testlobby
            // Clients objects
            private List<TcpClient> _clients = new List<TcpClient>();
            private List<TcpClient> _waitingLobby = new List<TcpClient>();
            private List<String> _clientnames = new List<string>();

            // Game stuff
            //private Dictionary<TcpClient, IGame> _gameClientIsIn = new Dictionary<TcpClient, IGame>();
            //private List<IGame> _games = new List<IGame>();
            //private List<Thread> _gameThreads = new List<Thread>();
            //private IGame _nextGame;

            public readonly string Name;
            public readonly int Port;
            public bool Running { get; private set; }


            public Lobby(string name, int port)
            {
                // Set some of the basic data
                Name = name;
                Port = port;
                Running = false;

                // Create the listener
                _listener = new TcpListener(IPAddress.Any, Port);
            }

            public void Run()
            {
                Console.WriteLine("Starting the \"{0}\" Game(s) Server on port {1}.", Name, Port);
                Console.WriteLine("Press Ctrl-C to shutdown the server at any time.");

                //// Start the next game
                //// (current only the Guess My Number Game)
                //_nextGame = new GuessMyNumberGame(this);

                // Start running the server
                _listener.Start();
                Running = true;
                List<Task> newConnectionTasks = new List<Task>();
                Console.WriteLine("Waiting for incommming connections...");

                while (Running)
                {
                    // Handle any new clients
                    if (_listener.Pending())
                        newConnectionTasks.Add(_handleNewConnection());

                    foreach (TcpClient client in _waitingLobby.ToArray())
                    {
                        NetworkStream stream = client.GetStream();
                        // Buffer to store the response bytes.
                        Byte[] data = new Byte[256];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        // Read the first batch of the TcpServer response bytes.
                        //Int32 bytes = stream.Read(data, 0, data.Length);
                        //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        //Console.WriteLine("Received: {0}", responseData);
                        //if (responseData.Contains("start"))//       Equals("start"))
                        //{
                        //    Console.WriteLine("start game");
                        //    //make match
                        //}
                       

                        //EndPoint endPoint = client.Client.RemoteEndPoint;
                        //bool disconnected = false;

                        //// Check for graceful first
                        //Packet p = ReceivePacket(client).GetAwaiter().GetResult();
                        //disconnected = (p?.Command == "bye");

                        //// Then ungraceful
                        //disconnected |= IsDisconnected(client);

                        //if (disconnected)
                        //{
                        //    HandleDisconnectedClient(client);
                        //    Console.WriteLine("Client {0} has disconnected from the Game(s) Server.", endPoint);
                        //}
                    }

                    //    // Once we have enough clients for the next game, add them in and start the game
                    //    if (_waitingLobby.Count >= _nextGame.RequiredPlayers)
                    //    {
                    //        // Get that many players from the waiting lobby and start the game
                    //        int numPlayers = 0;
                    //        while (numPlayers < _nextGame.RequiredPlayers)
                    //        {
                    //            // Pop the first one off
                    //            TcpClient player = _waitingLobby[0];
                    //            _waitingLobby.RemoveAt(0);

                    //            // Try adding it to the game.  If failure, put it back in the lobby
                    //            if (_nextGame.AddPlayer(player))
                    //                numPlayers++;
                    //            else
                    //                _waitingLobby.Add(player);
                    //        }

                    //        // Start the game in a new thread!
                    //        Console.WriteLine("Starting a \"{0}\" game.", _nextGame.Name);
                    //        Thread gameThread = new Thread(new ThreadStart(_nextGame.Run));
                    //        gameThread.Start();
                    //        _games.Add(_nextGame);
                    //        _gameThreads.Add(gameThread);

                    //        // Create a new game
                    //        _nextGame = new GuessMyNumberGame(this);
                    //    }

                    //    // Check if any clients have disconnected in waiting, gracefully or not
                    //    // NOTE: This could (and should) be parallelized
                    //    foreach (TcpClient client in _waitingLobby.ToArray())
                    //    {
                    //        EndPoint endPoint = client.Client.RemoteEndPoint;
                    //        bool disconnected = false;

                    //        // Check for graceful first
                    //        Packet p = ReceivePacket(client).GetAwaiter().GetResult();
                    //        disconnected = (p?.Command == "bye");

                    //        // Then ungraceful
                    //        disconnected |= IsDisconnected(client);

                    //        if (disconnected)
                    //        {
                    //            HandleDisconnectedClient(client);
                    //            Console.WriteLine("Client {0} has disconnected from the Game(s) Server.", endPoint);
                    //        }
                    //    }


                    //    // Take a small nap
                    //    Thread.Sleep(10);
                    //}

                    //// In the chance a client connected but we exited the loop, give them 1 second to finish
                    //Task.WaitAll(newConnectionTasks.ToArray(), 1000);

                    //// Shutdown all of the threads, regardless if they are done or not
                    //foreach (Thread thread in _gameThreads)
                    //    thread.Abort();

                    //// Disconnect any clients still here
                    //Parallel.ForEach(_clients, (client) =>
                    //{
                    //    DisconnectClient(client, "The Game(s) Server is being shutdown.");
                    //});

                    //// Cleanup our resources
                    //_listener.Stop();

                    //// Info
                    //Console.WriteLine("The server has been shut down.");
                }

            }

            private async Task _handleNewConnection()
            {
                // Get the new client using a Future
                TcpClient newClient = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("New connection from {0}.", newClient.Client.RemoteEndPoint);

                // Store them and put them in the waiting lobby
                _clients.Add(newClient);
                _waitingLobby.Add(newClient);

                


                // Send a welcome message
                string msg = String.Format("Welcome to the \"{0}\" Games Server.\n", Name);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                NetworkStream stream = newClient.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                //await SendPacket(newClient, new Packet("message", msg));
                Console.WriteLine("Sent: {0}", msg);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Receiveeed: {0}", responseData);
                if (responseData.Contains("SendName"))
                {
                    sendAllWaitingClients();
                    //responsedata - sendname
                }
                _clientnames.Add(responseData);
                
               //sendAllWaitingClients();
                //send all clients
            }

            private void sendAllWaitingClients()
            {
                string playerName = "";
                int playerNumber = 0;

                foreach (TcpClient client in _waitingLobby.ToArray())
                {

                    playerName = _clientnames[playerNumber];
                    string msg = String.Format("playerNames {0}", playerName);
                    playerNumber++;
                    NetworkStream stream = client.GetStream();


                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                    stream.Write(data, 0, data.Length);
                    //await SendPacket(newClient, new Packet("message", msg));
                    Console.WriteLine("Sent: {0}", msg);
                    // Buffer to store the response bytes.
                    //data = new Byte[256];

                    //// String to store the response ASCII representation.
                    //String responseData = String.Empty;

                    //// Read the first batch of the TcpServer response bytes.
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);
                    //if (responseData.Contains("start"))//       Equals("start"))
                    //{
                    //    Console.WriteLine("start game");
                    //    //make match
                    //}


                }
            }


                public void WriteTextMessage(TcpClient client, string message)
            {
                var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
                stream.WriteLine(message);
                stream.Flush();
            }

            public static Lobby lobby;
            static void Main(string[] args)
            {

                IPAddress host = IPAddress.Parse("127.0.0.1");
                lobby = new Lobby("NAAM", 25565);
                lobby.Run();
                //TcpListener listener = new System.Net.Sockets.TcpListener(IPAddress.Any, 1330);


                //listener.Start();

                bool ipIsOk = IPAddress.TryParse("127.0.0.1", out host);

                if (!ipIsOk)
                {
                    Console.WriteLine("ip adres kan niet geparsed worden.");
                    Environment.Exit(1);
                }

                //while (true)
                //{
                //    Console.WriteLine("Waiting for connection...");
                //    TcpClient client1 = listener.AcceptTcpClient();
                //    Console.WriteLine("Client 1 has connected...");
                //    TcpClient client2 = listener.AcceptTcpClient();
                //    Console.WriteLine("client 2 had connected! Lets go");
                //    HandleClientThread task = new HandleClientThread(client1, client2);
                //}
            }
            public delegate void ParameterizedThreadStart(Object obj);

            public void DoWork(Object data)
            {

            }


            internal class HandleClientThread
            {
                public HandleClientThread(object obj1, object obj2)
                {
                    Console.WriteLine("In HandleClientThread");
                    List<TcpClient> clients = new List<TcpClient>();
                    List<string> names = new List<string>();
                    List<int> scoresPlayer1 = new List<int>();
                    List<int> scoresPlayer2 = new List<int>();
                    int startingscore = 501;
                    int currentscoreP1 = startingscore;
                    int currentscoreP2 = startingscore;
                    int BestOfLegs = 5;
                    int p1LegsWon = 0;
                    int p2LegsWon = 0;

                    clients.Add(obj1 as TcpClient);
                    clients.Add(obj2 as TcpClient);

                    int legCounter = 1;

                    Console.WriteLine("Waiting for player 1 to enter name..");
                    string recievedP1 = ReadTextMessage(clients[0]);
                    Console.WriteLine($"recieved name player 1: {recievedP1}\n");
                    Console.WriteLine("Waiting for player 2 to enter name..");
                    string recievedP2 = ReadTextMessage(clients[1]);
                    Console.WriteLine($"recieved name player 2: {recievedP2}");

                    names.Add(recievedP1);
                    names.Add(recievedP2);

                    WriteTextMessage(clients[0], $"0{names[0]}-{names[1]}");
                    WriteTextMessage(clients[1], $"1{names[0]}-{names[1]}");

                    while (legCounter <= BestOfLegs) // gameLoop
                    {

                        //Send updated info to clients
                        // info client 1:
                        string startMess = $"2{currentscoreP1}-{currentscoreP2}";

                        foreach (TcpClient client in clients)
                        {
                            WriteTextMessage(client, startMess);
                        }
                        while (true) //Loop for 1 leg
                        {
                            TcpClient clientOnThrow;
                            TcpClient clientOffThrow;

                            if (legCounter % 2 == 1)
                            {
                                //Uneven legs so player 1 can start
                                clientOnThrow = clients[0];
                                clientOffThrow = clients[1];

                            }
                            else
                            {
                                //Even leg, player 2 starts
                                clientOnThrow = clients[1];
                                clientOffThrow = clients[0];
                            }

                            //send a message to player on throw to throw first
                            WriteTextMessage(clientOnThrow, "THROW");//clientOnThrow

                            //send a message to player off throw to wait
                            WriteTextMessage(clientOffThrow, "WAIT");//clientOffThrow

                            //waiting for first player to respond with a score and parsing that score to integer
                            string score1 = ReadTextMessage(clientOnThrow);
                            int p1Scored = int.Parse(score1);
                            p1Scored = CheckScore(p1Scored, currentscoreP1);
                            scoresPlayer1.Add(p1Scored);
                            currentscoreP1 -= p1Scored;

                            if (currentscoreP1 == 0)
                            {
                                if (clientOnThrow.Equals(clients[0]))
                                {
                                    p1LegsWon++;
                                }
                                else
                                {
                                    p2LegsWon++;
                                }
                                break;
                            }


                            //updating console about the score and calculating score left
                            Console.WriteLine($"First player scored {p1Scored}");

                            //composing string to send to clients
                            string currentscore = MakeCurrentScoreString(scoresPlayer1, scoresPlayer2, currentscoreP1, currentscoreP2);
                            foreach (TcpClient client in clients)
                            {
                                WriteTextMessage(client, currentscore);
                            }

                            //send a message to player off throw to throw 
                            WriteTextMessage(clientOffThrow, "THROW"); //clientOffThrow

                            //send a message to waiting player on throw to wait
                            WriteTextMessage(clientOnThrow, "WAIT"); // clientOnThrow

                            //waiting for second player to respond with their score and parse it to integer
                            string scoreRecP2 = ReadTextMessage(clientOffThrow);
                            int p2Scored = int.Parse(scoreRecP2);
                            p2Scored = CheckScore(p2Scored, currentscoreP2);
                            scoresPlayer2.Add(p2Scored);
                            currentscoreP2 -= p2Scored;
                            if (currentscoreP2 == 0)
                            {
                                if (clientOffThrow.Equals(clients[0]))
                                {
                                    p1LegsWon++;
                                }
                                else
                                {
                                    p2LegsWon++;
                                }
                            }

                            //updating console about the score and calculating score left
                            Console.WriteLine($"{names[1]} scored {p2Scored}");


                            //updating the clients about the score
                            currentscore = MakeCurrentScoreString(scoresPlayer1, scoresPlayer2, currentscoreP1, currentscoreP2);
                            foreach (TcpClient client in clients)
                            {
                                WriteTextMessage(client, currentscore);
                            }

                        }
                        legCounter++;
                        foreach (TcpClient client in clients)
                        {
                            WriteTextMessage(client, $"5{p1LegsWon}-{p2LegsWon}");
                        }



                    }
                    foreach (TcpClient client in clients)
                    {
                        client.Close();
                    }
                    Console.WriteLine("Connection closed");
                }

                private string MakeCurrentScoreString(List<int> p1scores, List<int> p2Scores, int curScore1, int curScore2)
                {
                    string scoreUpdate = $"2+{curScore1}-";
                    int totalscoredp1 = 0;
                    foreach (int score in p1scores)
                    {
                        totalscoredp1 += score;
                        int left = 501 - totalscoredp1;
                        scoreUpdate += $"{score}={left}_";
                    }
                    scoreUpdate = scoreUpdate.Substring(0, scoreUpdate.Length - 1);

                    scoreUpdate += $"+{curScore2}-";
                    int totalscoredP2 = 0;
                    foreach (int score in p2Scores)
                    {
                        totalscoredP2 += score;
                        int left = 501 - totalscoredP2;
                        scoreUpdate += $"{score}={left}_";
                    }
                    scoreUpdate = scoreUpdate.Substring(0, scoreUpdate.Length - 1);



                    return scoreUpdate;
                }

                private int CheckScore(int score, int playerscore)
                {

                    //impossible throws out of reach
                    if (score < 0 || score > 180)
                    {
                        score = 0;
                    }
                    else if (score > 159 && score < 180)
                    {
                        if (score == 163 || score == 166 || score == 169 || score == 172 || score == 173 || score == 175 ||
                                score == 176 || score == 178 || score == 179)
                        {
                            score = 0;
                        }
                    }


                    if (score == playerscore)
                    {
                        //leg done
                    }
                    else if (score == playerscore - 1)
                    {
                        //bust
                        score = 0;
                    }

                    return score;
                }

                public static void WriteTextMessage(TcpClient client, string message)
                {
                    var stream = new StreamWriter(client.GetStream(), Encoding.ASCII);
                    stream.WriteLine(message);

                    stream.Flush();
                }
                public static string ReadTextMessage(TcpClient client)
                {
                    StreamReader stream = new StreamReader(client.GetStream(), Encoding.ASCII);
                    string line = stream.ReadLine();

                    return line;
                }


            }
        }
    }
}

