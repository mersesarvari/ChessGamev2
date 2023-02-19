using Chess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    public class SingleplayerGame :Game
    {
        public string Player1 { get; set; }        

        public SingleplayerGame()
        {
            Gametype=GameType.Singleplayer;
        }

        public void AddPlayer(string player1)
        {
            Player1 = player1;
        }
        public void PlayerMove(string move)
        {
            moveList.Add(move);
            board.Move(move);
            Console.WriteLine("Player moved:");
            Console.WriteLine(board.ToAscii());
        }

        public void StartGame()
        {
            Console.WriteLine("Singleplayer game has started");
            var moves = board.Moves();
            var gamedata = new { Opcode = 4, Gameid = Id, Fen = board.ToFen(), Playerid = Player1};
            Server.SendMessage(Player1, JsonConvert.SerializeObject(gamedata));
            var bmovemsg = new { Opcode = 6, Moves = moves };
            Server.SendMessage(Player1, JsonConvert.SerializeObject(bmovemsg));
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
