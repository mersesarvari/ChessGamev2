using System;
using Chess;


namespace ChessBotv2
{
    public enum GameType
    {
        Multiplayer,
        Singleplayer,
    }
    internal class Program
    {
        static void Main(string[] args)
        {

            Bot.GetBot();
            Server.Instance.Start();
            Console.WriteLine("Server started on ws://localhost:5000");
            Server.Instance.AddWebSocketService<SingleplayerSocket>("/singleplayer");
            Server.Instance.AddWebSocketService<MultiplayerSocket>("/multiplayer");

            Thread singlelobby = new Thread(() => Server.CreateBotGame());
            Thread multilobby = new Thread(() => Server.CreateMultiplayerGame());

            singlelobby.Start();
            multilobby.Start();
            Console.ReadKey();
            Server.Instance.Stop();
        }
    }
}