using KifuAniMaker.Shogi.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetPositionPiece : ICSAStatement
    {
        private BlackWhite _BW;
        private IEnumerable<(int, int, string)> _Pieces;

        public SetPositionPiece(BlackWhite blackWhite, IEnumerable<(int, int, string)> pieces)
        {
            _BW = blackWhite;
            _Pieces = pieces;
        }

        public Board Execute(Board board)
        {
            foreach(var piece in _Pieces)
            {
                if(piece.Item1 == 0 && piece.Item2 == 0)
                {
                    var type = piece.Item3.ToPieceType();
                    if(type == typeof(All))
                    {
                        board.MoveAllToHandsFromPieceBox(_BW);
                    }
                    else
                    {
                        board.MoveToHandsFromPieceBox(type, _BW);
                    }
                }
                else
                {
                    board.SetFromPieceBox(piece.Item1, piece.Item2, piece.Item3.ToPieceType(), _BW);
                }
            }

            return board;
        }
    }
}
