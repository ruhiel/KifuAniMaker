using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Moves
{
    public class Resign : Move
    {
        public Resign(BlackWhite? bw = null) : base(bw)
        {
        }
    }
}
