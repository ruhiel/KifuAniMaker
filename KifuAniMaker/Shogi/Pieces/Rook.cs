using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class Rook : Piece
    {
        public Rook(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl22.png" : "sgl02.png") : (Promoted ? "sgl51.png" : "sgl32.png");

        public override bool Promotable => true;

        public override string ToJapaneseString => Promoted ? "龍" : "飛";
    }
}
