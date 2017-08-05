using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class Jishogi : Situation
    {
        public Jishogi(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => "持将棋";

        public override string ToAsciiString() => "持将棋";
    }
}
