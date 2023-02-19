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
        Random r = new Random();
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
                SetPlayerColors();
                Console.WriteLine("White player: " + White);
                var gamedataWhite = new { Opcode = 4, Gameid = Id, Fen = board.ToFen(), Playerid = Player1, Color=GetPlayerColor(Player1) };
                var gamedataBlack = new { Opcode = 4, Gameid = Id, Fen = board.ToFen(), Playerid = Player2, Color=GetPlayerColor(Player2)};
                Server.SendMessage(Player1.ToString(), JsonConvert.SerializeObject(gamedataWhite));
                Server.SendMessage(Player2.ToString(), JsonConvert.SerializeObject(gamedataBlack));
                //Sending players the basic possible moves
                var whitemoves = board.Moves();
                var wmovemsg = new { Opcode = 6, Custom = whitemoves };
                Server.SendMessage(Player1, JsonConvert.SerializeObject(wmovemsg));

            }
        }
        public void SetPlayerColors()
        {
            string whiteplayer;
            var val = r.Next(2);
            if (val % 2 == 0)
            {
                White= Player1.ToString();
            }
            else
            {
                White = Player2.ToString();
            }
        }

        public string GetPlayerColor(string playerid)
        {
            if (White == playerid)
            {
                return "white";
            }
            else
            {
                return "black";
            }
        }
        public void ExecuteIfGameEnded(string winningplayer)
        {
            if (board.IsEndGame)
            {
                if (winningplayer == Player1)
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 8, message = "Congratulation! You won the game!" }));
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 8, message = "You lost the game, Better luck next time!" }));
                }
                else if (winningplayer == Player2)
                {
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 8, message = "Congratulation! You won the game!" }));
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 8, message = "You lost the game, Better luck next time!" }));
                }
                //Döntetlen???
                else
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 8, message = "Its a Draw" }));
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 8, message = "Its a Draw" }));
                }
                
            }
                
        }
        public void ExecuteCastleIfNeeded(int OldcoordX, int OldcoordY, int NewcoordX, int NewcoordY)
        {
            if (OldcoordY == 4 && OldcoordX == 0) // Ha A lépő játékos a király
            {
                if (NewcoordY == 6 && NewcoordX == 0) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 0, NewY = 5 }));
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 0, NewY = 5 }));
                }
                if (NewcoordY == 2 && NewcoordX == 0) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 0, NewX = 0, NewY = 3 }));
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 0, NewX = 0, NewY = 3 }));

                }
            }
            if (OldcoordY == 4 && OldcoordX == 7) // Ha A lépő játékos a király
            {
                if (NewcoordY == 6 && NewcoordX == 7) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 7, OldY = 7, NewX = 7, NewY = 5 }));
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 7, OldY = 7, NewX = 7, NewY = 5 }));
                }
                if (NewcoordY == 2 && NewcoordX == 7) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 3, NewY = 7 }));
                    Server.SendMessage(Player2, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 3, NewY = 7 }));
                }
            }
        }
    }
}
