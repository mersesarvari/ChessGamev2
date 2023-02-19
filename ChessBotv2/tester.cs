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
            
            Console.WriteLine(bot.stockfish.GetBoardVisual());
            var bestmove = bot.stockfish.GetBestMove();
            Console.WriteLine("Bot best move: "+ bestmove);
            bestmove = bestmove.Remove(4);
            board.Move(bestmove);
            Console.WriteLine(board.ToAscii());

        }

    }
}
