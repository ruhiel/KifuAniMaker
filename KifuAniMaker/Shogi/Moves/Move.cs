﻿using KifuAniMaker.Shogi.Pieces;
using KifuAniMaker.Shogi.Utils;
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

        public BlackWhite BlackWhite { get; set; }

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

        public int Number { get; set; }

        public Move(BlackWhite bw, int number)
        {
            BlackWhite = bw;
            Number = number;
        }

        private string PosString => IsSame ? "同　" : $"{DestPosX.ToJapaneseStringX()}{DestPosY.ToJapaneseStringY()}";

        private string PromoteString => IsPromote ? "成" : string.Empty;

        private string DropString => IsDrop ? "打" : string.Empty;

        public override string ToString() => $"{Number.ToString()}{BlackWhite.ToSymbol()}{PosString}{Piece.ToJapaneseString}{PromoteString}{DropString}{SrcString}";

        public virtual string ToAsciiString() => $"{Number.ToString()}{BlackWhite.ToAsciiSymbol()}{PosString}{Piece.ToJapaneseString}{PromoteString}{DropString}{SrcString}";

        private string SrcString => IsDrop ? string.Empty : $"({SrcPosX.Value}{SrcPosY.Value})";
    }
}
