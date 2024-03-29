﻿using Chess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    public class SingleplayerGame :Game
    {
        Random r = new Random();
        public string Player1 { get; set; }        

        public SingleplayerGame()
        {
            Gametype=GameType.Singleplayer;
        }

        public void AddPlayer(string player1)
        {
            Player1 = player1;
        }
        

        public void StartGame()
        {
            Console.WriteLine("Singleplayer game has started");
            var moves = board.Moves();
            SetPlayerColors();
            Console.WriteLine("White player: "+White);
            var gamedata = new { Opcode = 4, Gameid = Id, Fen = board.ToFen(), Playerid = Player1, Color = GetPlayerColor() };
            Server.SendMessage(Player1, JsonConvert.SerializeObject(gamedata));
            if (GetPlayerColor() == "white")
            {
                var bmovemsg = new { Opcode = 6, Possiblemoves = moves };
                Server.SendMessage(Player1, JsonConvert.SerializeObject(bmovemsg));
            }
            else
            {
                //BOT MOVE
                string botmove = bot.GetBestMove();
                Move(botmove);

                Console.WriteLine("Bot moved: " + botmove);
                Server.SendMessage(Player1, JsonConvert.SerializeObject(new {Opcode=5, Fen=board.ToFen()}));
                var wmovemsg = new { Opcode = 6, Possiblemoves = board.Moves() };
                Server.SendMessage(Player1, JsonConvert.SerializeObject(wmovemsg));
            }
            
        }
        public void SetPlayerColors()
        {
            string whiteplayer;
            var val = r.Next(2);
            if (val % 2 == 0)
            {
                White = Player1.ToString();
            }
            else
            {
                White = "bot";
            }
        }

        public string GetPlayerColor()
        {
            if (White == Player1)
            {
                return "white";
            }
            else
            {
                return "black";
            }
        }
        public void ExecuteCastleIfNeeded(int OldcoordX, int OldcoordY, int NewcoordX, int NewcoordY)
        {
            if (OldcoordY == 4 && OldcoordX == 0) // Ha A lépő játékos a király
            {
                if (NewcoordY == 6 && NewcoordX == 0) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 0, NewY = 5 }));
                }
                if (NewcoordY == 2 && NewcoordX == 0) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 0, NewX = 0, NewY = 3 }));
                }
            }
            if (OldcoordX == 4 && OldcoordY == 7) // Ha A lépő játékos a király
            {
                if (NewcoordX == 6 && NewcoordY == 7) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 7, OldY = 7, NewX = 7, NewY = 5 }));
                }
                if (NewcoordX == 2 && NewcoordY == 7) // Ha jobbra castle
                {
                    Server.SendMessage(Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 3, NewY = 7 }));
                }
            }
        }
    }
}
