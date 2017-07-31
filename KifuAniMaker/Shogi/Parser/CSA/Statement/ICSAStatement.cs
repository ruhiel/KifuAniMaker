using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement
{
    public interface ICSAStatement
    {
        Board Execute(Board board);
    }
}
