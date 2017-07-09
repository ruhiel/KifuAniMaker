using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetEndTime : ICSAStatement
    {
        public DateTime Time { get; set; }
        public SetEndTime(DateTime time)
        {
            Time = time;
        }
        public Board Execute(Board board)
        {
            board.EndTime = Time;

            return board;
        }
    }
}
