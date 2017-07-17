using System;
using System.Collections.Generic;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    internal class SetPosition : ICSAStatement
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
                board[piece.Item1, piece.Item2] = null;
                board.SubBoard[piece.Item1, piece.Item2] = null;
            }

            return board;
        }

        public override string ToString() => nameof(SetPosition);
    }
}