using KifuAniMaker.Shogi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class TimeUp : Situation
    {
        public TimeUp(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => $"{Number.ToString()}{BlackWhite.ToSymbol()}時間切れ";

        public override string ToAsciiString() => $"{Number.ToString()}{BlackWhite.ToSymbol()}時間切れ";
    }
}
