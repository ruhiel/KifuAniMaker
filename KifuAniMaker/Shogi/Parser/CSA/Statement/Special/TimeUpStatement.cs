using KifuAniMaker.Shogi.Moves.Situations;
using KifuAniMaker.Shogi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class TimeUpStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Add(new TimeUp(board.Moves.Last().BlackWhite.Reverse(), board.NextMoveNumber));

            return board;
        }
    }
}
