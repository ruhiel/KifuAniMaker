using KifuAniMaker.Shogi.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetPositionBulk : ICSAStatement
    {
        public int Y { get; set; }

        public IEnumerable<Piece> Pieces { get; set; }

        public SetPositionBulk(int y, IEnumerable<Piece> pieces)
        {
            Y = y;
            Pieces = pieces;
        }
        public Board Execute(Board board)
        {
            if(board.SubBoard == null)
            {
                board.SubBoard = new Board();
            }
            foreach(var item in Pieces.Reverse().Select((piece, idx) => (piece, idx + 1)).Where(x => x.Item1 != null))
            {
                board.SetFromPieceBox(item.Item2, Y, item.Item1.GetType(), item.Item1.BW);
                board.SubBoard.SetFromPieceBox(item.Item2, Y, item.Item1.GetType(), item.Item1.BW);
            }
            return board;
        }

        public override string ToString() => nameof(SetPositionBulk);
    }
}
