using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves.Situations
{
    public abstract class Situation : Move
    {
        public Situation(BlackWhite bw, int number) : base(bw, number)
        {
        }
    }
}
