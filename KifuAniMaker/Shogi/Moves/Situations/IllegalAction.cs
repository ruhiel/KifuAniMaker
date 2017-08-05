using KifuAniMaker.Shogi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class IllegalAction : Situation
    {
        public IllegalAction(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => $"{BlackWhite.ToJapaneseString()}反則負け";

        public override string ToAsciiString() => $"{BlackWhite.ToJapaneseString()}反則負け";
    }
}
