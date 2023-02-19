using Chess;
using Stockfish.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    public class Bot
    {
        public IStockfish stockfish = new Stockfish.NET.Stockfish(@"Stockfish\stockfish12.exe");
        public Game game { get; set; }

        public Bot(Game game)
        {
            this.game = game;
        }


        public string GetBestMove()
        {
            stockfish.SetPosition(game.moveList.ToArray());
            return stockfish.GetBestMove();
        }

        public static void GetBot()
        {
            string currentDirName = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo current = new DirectoryInfo(currentDirName);
            var ggparent = current.Parent.Parent.Parent.FullName + @"\Stockfish\";
            if (!Directory.Exists(@"Stockfish"))
                Directory.CreateDirectory(@"Stockfish");
            if (!File.Exists(current + @"/Stockfish/stockfish12.exe"))
            {
                File.Copy(ggparent + @"/stockfish12.exe", current.FullName + @"/Stockfish/stockfish12.exe");
            }
        }
           
    }
}
