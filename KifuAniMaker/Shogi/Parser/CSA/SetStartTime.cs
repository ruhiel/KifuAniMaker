using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetStartTime : ICSAStatement
    {
        public DateTime Time { get; set; }
        public SetStartTime(DateTime time)
        {
            Time = time;
        }
        public Board Execute(Board board)
        {
            board.StartTime = Time;

            return board;
        }
    }
}
