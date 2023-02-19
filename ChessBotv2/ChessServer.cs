using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Collections;
using System.ComponentModel;


namespace ChessBotv2
{
    public class ChessServer : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            var currentid = ID;
            Server.Players.Add(new Player(currentid));
            Console.WriteLine($"[connected]: {currentid}");
            Server.SendMessage(ID, JsonConvert.SerializeObject(new Message() { Opcode = 0, Playerid = ID }));

        }
        protected override void OnMessage(MessageEventArgs e)
        {

            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            if (d.Opcode == 5)
            {
                var currentgame = Server.multiGames.FirstOrDefault(x => x.Id == d.Gameid);
                if (currentgame is MultiplayerGame)
                {
                    var old = MultiplayerGame.Zones[d.OldcoordY, d.OldcoordX];
                    var n = MultiplayerGame.Zones[d.NewcoordY, d.NewcoordX];
                    if ((currentgame as MultiplayerGame).board.IsValidMove(d.From + "" + d.To))
                    {
                        (currentgame as MultiplayerGame).PlayerMove(d.From + "" + d.To);
                    }
                }
                else
                {
                    throw new Exception("Error game is invalid.");
                }
            }
            if (d.Opcode == 4)
            {
                var currentgame = Server.singleGames.FirstOrDefault(x => x.Id == d.Gameid);
                if (currentgame is SingleplayerGame)
                {
                    var old = SingleplayerGame.Zones[d.OldcoordY, d.OldcoordX];
                    var n = SingleplayerGame.Zones[d.NewcoordY, d.NewcoordX];
                    if (currentgame.board.IsValidMove(old + "" + n))
                    {
                        //Checking castlemove for the player
                        if (d.OldcoordY == 4 && d.OldcoordX == 0) // Ha A lépő játékos a király
                        {
                            if (d.NewcoordY == 6 && d.NewcoordX == 0) // Ha jobbra castle
                            {
                                Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 0, NewY = 5 }));
                            }
                            if (d.NewcoordY == 2 && d.NewcoordX == 0) // Ha jobbra castle
                            {
                                Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 0, NewX = 0, NewY = 3 }));
                            }
                        }
                        currentgame.PlayerMove(old + n);
                        //Castle move check

                        Console.WriteLine("Player moved: " + old + n);
                        Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 5, OldX=d.OldcoordX, OldY=d.OldcoordY, NewX=d.NewcoordX, NewY=d.NewcoordY }));
                        if (currentgame.board.IsEndGame)
                        {
                            Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 8, message="Congratulation! You won the game!" }));
                        }
                        string botmove =currentgame.bot.GetBestMove();
                        currentgame.PlayerMove(botmove);
                        //Le kell checkolni később hogy tényleg 4 elemű e a karakterkód, mert lehet 5 elemű is

                        var botmovecoordsold = Game.GetCoordinateFromZone(botmove[0] +""+ botmove[1]);
                        var botmovecoordsnew = Game.GetCoordinateFromZone(botmove[2] + "" + botmove[3]);
                        if (old[0] == 4 && old[1] == 7) // Ha A lépő játékos a király
                        {
                            if (n[0] == 6 && n[1] == 7) // Ha jobbra castle
                            {
                                Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 7, OldY = 7, NewX = 7, NewY = 5 }));
                            }
                            if (n[0] == 2 && n[1] == 7) // Ha jobbra castle
                            {
                                Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 5, OldX = 0, OldY = 7, NewX = 7, NewY = 3 }));
                            }
                        }
                        Console.WriteLine("Bot moved: "+botmove);
                        Server.SendMessage(
                            d.Playerid, 
                            JsonConvert.SerializeObject(
                                new { 
                                    Opcode = 5, 
                                    OldX = botmovecoordsold.Item2, 
                                    OldY = botmovecoordsold.Item1, 
                                    NewX = botmovecoordsnew.Item2, 
                                    NewY = botmovecoordsnew.Item1,
                                    Possiblemoves = currentgame.board.Moves()
                                }));
                        if (currentgame.board.IsEndGame)
                        {
                            Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 8, message = "You lost the game, Better luck next time!" }));
                        }
                    }
                }
            }
        }
        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("[Disconnected] :" + ID);
            Server.Players.Remove(Server.Players.FirstOrDefault(x => x.Id == ID));
            Server.SendMessage(ID, JsonConvert.SerializeObject(new Message() { Opcode = 0, Playerid = ID }));
        }

    }
}
