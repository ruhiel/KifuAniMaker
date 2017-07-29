using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class All : Piece
    {
        public All(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => string.Empty;

        public override bool Promotable => false;

        public override string ToJapaneseString => "全て";
    }
}
