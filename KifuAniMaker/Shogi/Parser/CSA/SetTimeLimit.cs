using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetTimeLimit : ICSAStatement
    {
        public string RemainTime { get; set; }

        public string SecondTime { get; set; }

        public SetTimeLimit(string remainTime, string secondTime)
        {
            RemainTime = remainTime;

            SecondTime = secondTime;
        }

        public Board Execute(Board board)
        {
            return board;
        }
    }
}
