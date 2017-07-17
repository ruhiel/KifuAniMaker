using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetSite : ICSAStatement
    {
        public string Site { get; set; }
        public SetSite(string site)
        {
            Site = site;
        }

        public Board Execute(Board board)
        {
            board.Site = Site;

            return board;
        }

        public override string ToString() => nameof(SetSite);
    }
}
