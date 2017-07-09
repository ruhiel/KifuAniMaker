using KifuAniMaker.Shogi.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public static class CSAUtil
    {
        private static Dictionary<string, Type> _Map = new Dictionary<string, Type>()
        {
            { "", typeof(Bishop) }
        };

        public static Piece CreatePiece(string piece, BlackWhite bw)
        {
            var type = _Map[piece];
            return (Piece)Activator.CreateInstance(type, bw);
        }
        public static BlackWhite ToBlackWhite(this string str)
        {
            if(str == "+")
            {
                return BlackWhite.Black;
            }
            else if(str == "-")
            {
                return BlackWhite.White;
            }
            else
            {
                throw new ArgumentException(str);
            }
        }
    }
}
