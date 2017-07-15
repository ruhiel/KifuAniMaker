using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Pieces
{
    public class Gold : Piece
    {
        public Gold(BlackWhite bw, bool promoted = false) : base(bw, promoted)
        {
        }

        public override string ImageFile => BW == BlackWhite.Black ? "sgl04.png" : "sgl34.png";

        public override bool Promotable => false;
    }
}
