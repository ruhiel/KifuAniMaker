using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class RetractMoveStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Remove(board.Moves.Last());

            return board;
        }
    }
}
