using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class Knight : Piece
    {
        public Knight(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl26.png" : "sgl06.png") : (Promoted ? "sgl56.png" :"sgl36.png");

        public override bool Promotable => true;

        public override string ToJapaneseString => Promoted ? "成桂" : "桂";
    }
}
