using Chess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    public class MultiplayerGame:Game
    {

        public int Time1 { get; set; }
        public int Time2 { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public MultiplayerGame(int time)
        {
            Gametype = GameType.Multiplayer;
            Time1 = time;
            Time2 = time;
        }
        public void AddPlayers(string player1, string player2)
        {
            Player1 = player1;
            Player2 = player2;
        }
        public void StartGame()
        {
            if (Gametype == GameType.Multiplayer && Player1.ToString().Length > 0 && Player2.ToString().Length > 0)
            {
                //Fel kellett cserélni itt a színeket valamiért. tuti valami bug...
                var gamedataWhite = new Message() { Opcode = 4, Gameid = Id, Fen = board.ToFen(), Playerid = Player1 };
                var gamedataBlack = new Message() { Opcode = 4, Gameid = Id, Fen = board.ToFen(), Playerid = Player1 };
                Server.SendMessage(Player1.ToString(), JsonConvert.SerializeObject(gamedataWhite));
                Server.SendMessage(Player2.ToString(), JsonConvert.SerializeObject(gamedataBlack));
                //Sending players the basic possible moves
                var whitemoves = board.Moves();
                var wmovemsg = new Message() { Opcode = 6, Custom = whitemoves };
                Server.SendMessage(Player1, JsonConvert.SerializeObject(wmovemsg));

            }
        }
        public void PlayerMove(string move)
        {
            moveList.Add(move);
            board.Move(new Move(move[0] + "" + move[1], move[2] + "" + move[3]));
        }
        public string GetActivePlayer()
        {
            if (turn % 2 == 0)
            {
                return Player1;
            }
            else return Player2;
        }

        public void BroadcastMessage(Message message)
        {
            if (Player1.Count() >= 0)
            {
                Server.SendMessage(Player1, JsonConvert.SerializeObject(message));
            }
            if (Player2.Count() >= 0)
            {
                Server.SendMessage(Player2, JsonConvert.SerializeObject(message));
            }
        }

        public void SendMessage(Message message, string id)
        {
            Server.SendMessage(id, JsonConvert.SerializeObject(message));
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
