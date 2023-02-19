using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ChessBotv2
{
    public class Server
    {
        public static WebSocketServer Instance = new WebSocketServer("ws://localhost:5000");
        public static List<Player> Players = new List<Player>();
        public static List<IWebSocketSession> Sessions = new List<IWebSocketSession>();
        public static List<SingleplayerGame> singleGames = new List<SingleplayerGame>();
        public static List<MultiplayerGame> multiGames = new List<MultiplayerGame>();


        public static void SendMessage(string id, string message)
        {
            foreach (var item in Server.Instance.WebSocketServices.Hosts)
            {   if(id!="Bot")
                item.Sessions.SendTo(message, id);
            }
        }
        public static void Broadcast(string message)
        {
            foreach (var item in Server.Instance.WebSocketServices.Hosts)
            {
                item.Sessions.Broadcast(message);
            }
        }

        public static void MatchPlayers() 
        {
            while (true)
            {
                //Getting players in the lobby
                var playersinlobby = Server.Players.FindAll(x => x.Searching==true);
                if (playersinlobby.Count >= 2)
                {
                    MultiplayerGame currentgame = new MultiplayerGame(100000);
                    currentgame.AddPlayers(playersinlobby[0].Id, playersinlobby[1].Id);
                    playersinlobby[0].Searching = false;
                    playersinlobby[1].Searching = false;

                    Console.WriteLine("[Game created]:" + currentgame.Id);
                    currentgame.StartGame();
                    Server.multiGames.Add(currentgame);

                }
                else
                {
                }
                Thread.Sleep(3000);
            }
        }
        public static void CreateBotGame()
        {
            
            while (true)
            {
                var playersinlobby = Server.Players.FindAll(x => x.Searching==true);
                if (playersinlobby.Count > 0)
                {
                    SingleplayerGame currentgame = new SingleplayerGame();
                    currentgame.AddPlayer(playersinlobby[0].Id);
                    playersinlobby[0].Searching = false;

                    Console.WriteLine("[Bot game created]:" + currentgame.Id);
                    currentgame.StartGame();
                    Server.singleGames.Add(currentgame);
                    
                }
                else
                {
                }
                Thread.Sleep(3000);
            }
            
        }


    }
}
