﻿using KifuAniMaker.Shogi.Moves.Situations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public class Interruption : Situation
    {
        public Interruption(BlackWhite bw, int number) : base(bw, number)
        {
        }

        public override string ToString() => $"中断";

        public override string ToAsciiString() => $"中断";
    }
}
