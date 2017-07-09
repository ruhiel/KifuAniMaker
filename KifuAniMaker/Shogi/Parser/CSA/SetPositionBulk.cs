using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetPositionBulk : ICSAStatement
    {
        public int Y { get; set; }

        public IEnumerable<string> Pieces { get; set; }

        public SetPositionBulk(int y, IEnumerable<string> pieces)
        {
            Y = y;
            Pieces = pieces;
        }
        public Board Execute(Board board)
        {
            var regex = new Regex(@"(?<bw>\-|\+)(?<piece>..)");
            for (var i = 0; i < 9; i++)
            {
                var match = regex.Match(Pieces.ElementAt(i));
                if(match.Success)
                {
                    var bw = match.Groups["bw"].Value.ToBlackWhite();
                }
            }
            throw new NotImplementedException();
        }
    }
}
