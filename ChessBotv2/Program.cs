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
            Server.Instance.AddWebSocketService<ConnectionManager>("/singleplayer");
            Server.Instance.AddWebSocketService<ConnectionManager>("/multiplayer");

            Thread t = new Thread(() => Server.CreateBotGame());

            t.Start();
            Console.ReadKey();
            Server.Instance.Stop();
        }
    }
}