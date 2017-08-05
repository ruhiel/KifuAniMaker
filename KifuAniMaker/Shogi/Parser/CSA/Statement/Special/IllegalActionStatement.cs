using KifuAniMaker.Shogi.Moves.Situations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class IllegalActionStatement : ICSAStatement
    {
        private BlackWhite _BW;
        public IllegalActionStatement(BlackWhite bw)
        {
            _BW = bw;
        }

        public Board Execute(Board board)
        {
            board.Moves.Add(new IllegalAction(_BW, board.NextMoveNumber));

            return board;
        }
    }
}
