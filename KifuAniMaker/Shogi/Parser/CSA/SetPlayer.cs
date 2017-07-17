using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetPlayer : ICSAStatement
    {
        public BlackWhite BlackWhite { get; set; }
        public string Player { get; set; }
        
        public SetPlayer(BlackWhite bw, string player)
        {
            BlackWhite = bw;
            Player = player;
        }
        public Board Execute(Board board)
        {
            if(BlackWhite == BlackWhite.Black)
            {
                board.BlackPlayer = Player;
            }
            else
            {
                board.WhitePlayer = Player;
            }

            return board;
        }

        public override string ToString() => nameof(SetPlayer);
    }
}
