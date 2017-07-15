using KifuAniMaker.Shogi.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public static class CSAUtil
    {
        private static Dictionary<string, (Type type, bool promoted)> _PieceMap = new Dictionary<string, (Type, bool)>()
        {
            { "FU", (typeof(Pawn), false) },
            { "KY", (typeof(Lance), false) },
            { "KE", (typeof(Knight), false) },
            { "GI", (typeof(Silver), false) },
            { "KI", (typeof(Gold), false) },
            { "KA", (typeof(Bishop), false) },
            { "HI", (typeof(Rook), false) },
            { "OU", (typeof(King), false) },
            { "TO", (typeof(Pawn), true) },
            { "NY", (typeof(Lance), true) },
            { "NK", (typeof(Knight), true) },
            { "NG", (typeof(Silver), true) },
            { "UM", (typeof(Bishop), true) },
            { "RY", (typeof(Rook), true) },
        };

        public static Piece ToPiece(this string piece, BlackWhite bw)
        {
            var value = _PieceMap[piece];
            return (Piece)Activator.CreateInstance(value.type, bw, value.promoted);
        }

        public static Piece WithBlackWhiteToPiece(this string piece)
        {
            try
            {
                // 先後付き駒
                var pieceWithBlackWhite =
                    from bw in CSAParser.BlackWhiteParser
                    from p in CSAParser.PieceParser
                    select p.ToPiece(bw.ToBlackWhite());

                return pieceWithBlackWhite.Parse(piece);
            }
            catch (ParseException)
            {
                return null;
            }
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
