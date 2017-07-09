using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetGameName : ICSAStatement
    {
        public string GameName { get; set; }

        public SetGameName(string gameName)
        {
            GameName = gameName;
        }

        public Board Execute(Board board)
        {
            board.GameName = GameName;

            return board;
        }
    }
}
