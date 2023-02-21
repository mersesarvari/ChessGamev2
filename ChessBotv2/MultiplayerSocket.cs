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
using Chess;

namespace ChessBotv2
{
    public class MultiplayerSocket : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            var currentid = ID;
            Server.Players.Add(new Player(currentid, true));
            Console.WriteLine($"[Multiplayer-Connected]: {currentid}");
            Server.SendMessage(ID, JsonConvert.SerializeObject(new { Opcode = 0, Playerid = ID }));

        }
        protected override void OnMessage(MessageEventArgs e)
        {

            var d = JsonConvert.DeserializeObject<Message>(e.Data);
            //Multiplayer Game.
            if (d.Opcode == 4)
            {
                var currentgame = Server.multiGames.FirstOrDefault(x => x.Id == d.Gameid);
                if (currentgame is MultiplayerGame)
                {
                    var old = Game.Zones[d.OldcoordY, d.OldcoordX];
                    var n = Game.Zones[d.NewcoordY, d.NewcoordX];
                    if (currentgame.board.IsValidMove(old + "" + n))
                    {
                        //Checking castlemove for the player
                        currentgame.ExecuteCastleIfNeeded(d.OldcoordX, d.OldcoordY, d.NewcoordX, d.NewcoordY);

                        //Player Moving
                        currentgame.Move(old + n);
                        Console.WriteLine("Player moved: " + old + n);
                        Server.SendMessage(currentgame.Player1, JsonConvert.SerializeObject(new { Opcode = 5, OldX = d.OldcoordX, OldY = d.OldcoordY, NewX = d.NewcoordX, NewY = d.NewcoordY, Fen = currentgame.board.ToFen() }));
                        Server.SendMessage(currentgame.Player2, JsonConvert.SerializeObject(new { Opcode = 5, OldX = d.OldcoordX, OldY = d.OldcoordY, NewX = d.NewcoordX, NewY = d.NewcoordY, Fen = currentgame.board.ToFen() }));
                        if (ID == currentgame.Player1)
                        {
                            Server.SendMessage(currentgame.Player2, JsonConvert.SerializeObject(new { Opcode = 6, Possiblemoves = currentgame.board.Moves() }));
                        }
                        else if (ID == currentgame.Player2)
                        {
                            Server.SendMessage(currentgame.Player1, JsonConvert.SerializeObject(new { Opcode = 6, Possiblemoves = currentgame.board.Moves() }));
                        }
                        else
                        {
                            throw new Exception("A PlayerID és a SocketID nem egyezik meg egyáltalán");
                        }
                        //Player WON
                        currentgame.ExecuteIfGameEnded(d.Playerid);
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
