using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class Draw : Situation
    {
        public Draw(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => "入玉により引き分け";

        public override string ToAsciiString() => "入玉により引き分け";
    }
}
