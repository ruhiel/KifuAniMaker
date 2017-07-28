using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetPositionPiece : ICSAStatement
    {
        public Board Execute(Board board) => board;
    }
}
