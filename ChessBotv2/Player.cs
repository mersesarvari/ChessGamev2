using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool SearchingSingleplayer { get; set; }

        public bool SearchingMultiplayer { get; set; }
        public Player(string id, bool multiplayer)
        {
            Id = id;
            Name = "Player";
            if (multiplayer)
            {
                SearchingSingleplayer = false;
                SearchingMultiplayer = true;
            }
            else
            {
                SearchingSingleplayer = true;
                SearchingMultiplayer = false;
            }
            
        }
    }
}
