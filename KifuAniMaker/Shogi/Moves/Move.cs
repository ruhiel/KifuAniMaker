using KifuAniMaker.Shogi.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves
{
    public class Move
    {
        public int? SrcPosX { get; set; }
        public int? SrcPosY { get; set; }

        public int DestPosX { get; set; }

        public int DestPosY { get; set; }

        private int _Time;

        public void SetTime(int time) => _Time = time;

        public BlackWhite? BlackWhite { get; set; }

        /// <summary>
        /// 駒
        /// </summary>
        public Piece Piece { get; set; }

        /// <summary>
        /// 打ちか
        /// </summary>
        public bool IsDrop => SrcPosX == 0 && SrcPosY == 0;

        /// <summary>
        /// 成りか
        /// </summary>
        public bool IsPromote { get; set; } = false;

        /// <summary>
        /// 同か
        /// </summary>
        public bool IsSame { get; set; } = false;

        public Move(BlackWhite? bw)
        {
            BlackWhite = bw;
        }
    }
}
