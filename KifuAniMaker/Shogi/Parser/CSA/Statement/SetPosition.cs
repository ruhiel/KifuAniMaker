using System;
using System.Collections.Generic;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement
{
    public class SetPosition : ICSAStatement
    {
        private IEnumerable<(int, int, string)> _Pieces;

        public SetPosition(IEnumerable<(int, int, string)> pieces)
        {
            _Pieces = pieces;
        }

        public Board Execute(Board board)
        {
            board.InitBoard();
            if (board.SubBoard == null)
            {
                board.SubBoard = new Board();
            }
            board.SubBoard.InitBoard();

            foreach (var piece in _Pieces)
            {
                board.MoveToPieceBox(piece.Item1, piece.Item2);
                board.SubBoard.MoveToPieceBox(piece.Item1, piece.Item2);
            }

            return board;
        }

        public override string ToString() => nameof(SetPosition);
    }
}