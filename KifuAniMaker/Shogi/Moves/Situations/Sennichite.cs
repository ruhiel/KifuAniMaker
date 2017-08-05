using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class Sennichite : Situation
    {
        public Sennichite(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => "千日手";

        public override string ToAsciiString() => "千日手";
    }
}
