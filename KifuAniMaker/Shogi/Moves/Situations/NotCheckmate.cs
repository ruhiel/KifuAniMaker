using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class NotCheckmate : Situation
    {
        public NotCheckmate(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => "不詰";

        public override string ToAsciiString() => "不詰";
    }
}
