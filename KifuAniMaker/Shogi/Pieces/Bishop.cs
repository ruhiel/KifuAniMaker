using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl23.png" : "sgl03.png") : (Promoted ? "sgl53.png" :"sgl33.png");

        public override bool Promotable => true;

        public override string ToJapaneseString => Promoted ? "馬" : "角";
    }
}
