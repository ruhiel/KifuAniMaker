using KifuAniMaker.Shogi.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetPositionBulk : ICSAStatement
    {
        public int Y { get; set; }

        public IEnumerable<Piece> Pieces { get; set; }

        public SetPositionBulk(int y, IEnumerable<Piece> pieces)
        {
            Y = y;
            Pieces = pieces;
        }
        public Board Execute(Board board)
        {
            return board;
        }
    }
}
