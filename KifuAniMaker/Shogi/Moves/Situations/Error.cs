using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class Error : Situation
    {
        public Error(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => "エラー";

        public override string ToAsciiString() => "エラー";
    }
}
