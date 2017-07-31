using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser
{
    public interface IParser
    {
        List<Board> ParseContent(string content);
    }
}
