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
            Server.Instance.AddWebSocketService<ChessServer>("/chess");
            
            Thread t = new Thread(() => Server.CreateBotGame());

            t.Start();
            Console.ReadKey();
            Server.Instance.Stop();

            //tester t = new tester();
            //t.AddChessBoard();
            //t.AddChessBoard();
            //t.AddChessBoard();

            //t.MoveBoard(0, "d2d4");
            //t.DrawChessBoard(0);
            //t.MoveBoard(1, "a2a4");
            //t.DrawChessBoard(1);

            //ChessBoard board = new ChessBoard();
            //board.Draw();
            //Console.WriteLine(board.ToAscii());
            //Console.WriteLine(board.ToFen());
            //Console.ReadKey();
        }
    }
}