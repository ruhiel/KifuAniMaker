using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class Lance : Piece
    {
        public Lance(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl27.png" : "sgl07.png") : (Promoted ? "sgl57.png" : "sgl37.png");

        public override bool Promotable => true;

        public override string ToJapaneseString => Promoted ? "成香" : "香";
    }
}
