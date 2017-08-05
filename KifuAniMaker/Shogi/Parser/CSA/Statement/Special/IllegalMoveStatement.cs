using KifuAniMaker.Shogi.Moves.Situations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class IllegalMoveStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Add(new IllegalMove(board.NextBlackWhite, board.NextMoveNumber));

            return board;
        }
    }
}
