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
        public bool Searching { get; set; }
        public Player(string id)
        {
            Id = id;
            Name = "Player";
            Searching= true;
        }
    }
}
