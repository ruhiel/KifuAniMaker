using System;
using System.Linq;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetTime : ICSAStatement
    {
        private int _Time;

        public SetTime(int time)
        {
            _Time = time;
        }

        public Board Execute(Board board)
        {
            board.Moves?.LastOrDefault()?.SetTime(_Time);

            return board;
        }

        public override string ToString() => nameof(SetTime);
    }
}