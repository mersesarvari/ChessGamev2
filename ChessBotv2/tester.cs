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
        public List<ChessBoardWithId> BoardList { get; set; }
        public tester()
        {
            BoardList= new List<ChessBoardWithId>();
        }
        public void DrawChessBoard(int index)
        {
            Console.WriteLine(BoardList[index].Board.ToAscii());
        }
        public void MoveBoard(int index, string move)
        {
            BoardList[index].Board.Move(move);
        }
        public void AddChessBoard()
        {
            BoardList.Add(new ChessBoardWithId());
        }
    }

    public class ChessBoardWithId
    {
        public Guid Id { get; set; }
        public ChessBoard Board { get; set; }

        public ChessBoardWithId()
        {
            Id = Guid.NewGuid();
            this.Board = new ChessBoard();
        }
    }
}
