using System;
using System.Collections.Generic;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    internal class SetPosition : ICSAStatement
    {
        private IEnumerable<string> _Pieces;

        public SetPosition(IEnumerable<string> pieces)
        {
            _Pieces = pieces;
        }

        public Board Execute(Board board)
        {
            board.InitBoard();

            return board;
        }
    }
}