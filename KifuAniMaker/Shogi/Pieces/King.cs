using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class King : Piece
    {
        public King(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? "sgl11.png" : "sgl31.png";

        public override bool Promotable => false;

        public override string ToJapaneseString => "玉";
    }
}
