using Chess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBotv2
{
    public class tester
    {
        ChessBoard board;
        Bot bot;
        Game game;
        public tester()
        {
            board = new ChessBoard();
            game = new Game();
            bot = new Bot(game);
        }

        public void PawnPromotionTest()
        {
            //Seems like it working
            string fenstring = "8/P7/8/8/8/8/8/1K4k1 w - - 0 1";
            board = ChessBoard.LoadFromFen(fenstring);
            bot.stockfish.SetFenPosition(fenstring);
            
            var bestmove = bot.stockfish.GetBestMove();

            Console.WriteLine("Bot best move: "+ bestmove);
            var asd = bestmove.Remove(4) + "=" + bestmove[4].ToString().ToUpper();
            board.Move(asd);
            Console.WriteLine(board.ToAscii());

        }

    }
}
