using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetTurn : ICSAStatement
    {
        public BlackWhite Turn { get; set; }

        public SetTurn(BlackWhite bw)
        {
            Turn = bw;
        }

        public Board Execute(Board board)
        {
            board.Turn = Turn;

            return board;
        }
    }
}
