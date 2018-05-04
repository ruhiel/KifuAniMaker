using KifuAniMaker.Shogi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class Win : Situation
    {
        public Win(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => $"{Number.ToString()}{BlackWhite.ToSymbol()}入玉宣言勝ち";

        public override string ToAsciiString() => $"{Number.ToString()}{BlackWhite.ToSymbol()}入玉宣言勝ち";
    }
}
