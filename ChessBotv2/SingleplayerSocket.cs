﻿using System;
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
using Chess;

namespace ChessBotv2
{
    public class SingleplayerSocket : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            var currentid = ID;
            Server.Players.Add(new Player(currentid, false));
            Console.WriteLine($"[Singleplayer-Connected]: {currentid}");
            Server.SendMessage(ID, JsonConvert.SerializeObject(new { Opcode = 0, Playerid = ID }));

        }
        protected override void OnMessage(MessageEventArgs e)
        {

            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            //Singleplayer Game move command
            if (d.Opcode == 4 && Server.singleGames.First(x => x.Id == d.Gameid)!=null)
            {
                var currentgame = Server.singleGames.FirstOrDefault(x => x.Id == d.Gameid);
                if (currentgame is SingleplayerGame)
                {
                    var old = Game.Zones[d.OldcoordY, d.OldcoordX];
                    var n = Game.Zones[d.NewcoordY, d.NewcoordX];
                    if (currentgame.board.IsValidMove(old + "" + n))
                    {
                        //Checking castlemove for the player
                        currentgame.ExecuteCastleIfNeeded(d.OldcoordX, d.OldcoordY, d.NewcoordX, d.NewcoordY);

                        //Player Moving
                        currentgame.PlayerMove(old + n);
                        Console.WriteLine("Player moved: " + old + n);
                        Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 5, OldX=d.OldcoordX, OldY=d.OldcoordY, NewX=d.NewcoordX, NewY=d.NewcoordY, Fen=currentgame.board.ToFen()}));
                        //Player WON
                        if (currentgame.board.IsEndGame)
                        {
                            Server.SendMessage(d.Playerid, JsonConvert.SerializeObject(new { Opcode = 8, message="Congratulation! You won the game!" }));
                        }
                        //BOT MOVE
                        string botmove =currentgame.bot.GetBestMove();
                        currentgame.PlayerMove(botmove);

                        //Le kell checkolni később hogy tényleg 4 elemű e a karakterkód, mert lehet 5 elemű is
                        var botmovecoordsold = Game.GetCoordinateFromZone(botmove[0] +""+ botmove[1]);
                        var botmovecoordsnew = Game.GetCoordinateFromZone(botmove[2] + "" + botmove[3]);

                        //Checking castle. MB not a good method. Have to check it later...
                        currentgame.ExecuteCastleIfNeeded(old[0], old[1], n[0], n[1]);

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
                                    Fen = currentgame.board.ToFen()
                                }));
                        Server.SendMessage(currentgame.Player1, JsonConvert.SerializeObject(new { Opcode = 6, Possiblemoves = currentgame.board.Moves() }));
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
            Console.WriteLine("[Disconnected-singleplayer] :" + ID);
            Server.Players.Remove(Server.Players.FirstOrDefault(x => x.Id == ID));
            Server.SendMessage(ID, JsonConvert.SerializeObject(new Message() { Opcode = 0, Playerid = ID }));
        }

    }
}