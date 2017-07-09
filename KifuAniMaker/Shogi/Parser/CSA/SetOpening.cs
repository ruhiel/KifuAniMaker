using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetOpening : ICSAStatement
    {
        public string Opening { get; set; }

        public SetOpening(string opening)
        {
            Opening = opening;
        }


        public Board Execute(Board board)
        {
            board.Opening = Opening;
            return board;
        }
    }
}
