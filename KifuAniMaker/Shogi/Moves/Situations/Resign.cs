using KifuAniMaker.Shogi.Moves.Situations;
using KifuAniMaker.Shogi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves
{
    public class Resign : Situation
    {
        public Resign(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => $"{Number.ToString()}{BlackWhite.ToSymbol()}投了";

        public override string ToAsciiString() => $"{Number.ToString()}{BlackWhite.ToAsciiSymbol()}投了";
    }
}
