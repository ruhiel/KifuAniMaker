using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class Silver : Piece
    {
        public Silver(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl25.png" : "sgl05.png") : (Promoted ? "sgl55.png" : "sgl35.png");

        public override bool Promotable => true;

        public override string ToJapaneseString => Promoted ? "成銀" : "銀";
    }
}
