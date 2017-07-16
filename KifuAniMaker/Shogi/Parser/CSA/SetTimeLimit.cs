﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetTimeLimit : ICSAStatement
    {
        public TimeSpan RemainTime { get; set; }

        public TimeSpan SecondTime { get; set; }

        public SetTimeLimit(TimeSpan remainTime, TimeSpan secondTime)
        {
            RemainTime = remainTime;

            SecondTime = secondTime;
        }

        public Board Execute(Board board)
        {
            board.RemainTime = RemainTime;
            board.SecondTime = SecondTime;

            return board;
        }
    }
}
