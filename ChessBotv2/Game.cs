using Chess;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    
    public class Game
    {
        public ChessBoard board;
        public GameType Gametype { get; set; }
        public string Id { get; set; }
        public Bot bot { get; set; }
        public List<string> moveList { get; set; }
        public int turn { get; set; }

        public static string[,] Zones = new string[8, 8] {
                { "a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8" },
                { "b1", "b2", "b3", "b4", "b5", "b6", "b7", "b8" },
                { "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8" },
                { "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8"},
                { "e1", "e2", "e3", "e4", "e5", "e6", "e7", "e8" },
                { "f1", "f2", "f3", "f4", "f5", "f6", "f7", "f8" },
                { "g1", "g2", "g3", "g4", "g5", "g6", "g7", "g8" },
                { "h1", "h2", "h3", "h4", "h5", "h6", "h7", "h8" },
            };


        public Game()
        {
            bot = new Bot(this);
            Id=Guid.NewGuid().ToString();
            board=new ChessBoard() { AutoEndgameRules=AutoEndgameRules.None};
            moveList = new List<string>();
            
            //Console.Write("c,2 position: ");
            //Console.WriteLine(board["c2"]);
            //Console.Write("2,1(c2) position: ");
            //Console.WriteLine(board[new Position(2, 1)]);
            //Console.WriteLine("2,1 element of the board:" + board[Zones[2,1]]);            
            turn= 0;
        }
        public void PlayerMove(string move)
        {
            moveList.Add(move);
            board.Move(new Move(move[0] + "" + move[1], move[2] + "" + move[3]));
        }
        public static (int, int) GetCoordinateFromZone(string zone)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Zones[i, j] == zone)
                        return (i, j);
                }
            }
            throw new Exception("Invalid zone");
        }
        public string GetZoneFromCoordinate(int X, int Y)
        {
            return Zones[X, Y];
        }

        public (string pos1, string pos2) Converter(string stockfish)
        {
            return (pos1: stockfish[0] + "" + stockfish[1], pos2: stockfish[2] + "" + stockfish[3]);
        }
        public string Converter(string pos1, string pos2)
        {
            return pos1 + "" + pos2;
        }
             
    }
}
