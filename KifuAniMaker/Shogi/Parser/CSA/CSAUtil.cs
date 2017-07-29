using KifuAniMaker.Shogi.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprache;
using KifuAniMaker.Shogi.Moves;
using KifuAniMaker.Shogi.Utils;

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
            { "AL", (typeof(All), false) },
        };

        private static Dictionary<(Type type, bool promoted), string> _StrMap = new Dictionary<(Type type, bool promoted), string>()
        {
            { (typeof(Pawn), false), "FU"},
            { (typeof(Lance), false), "KY"},
            { (typeof(Knight), false), "KE"},
            { (typeof(Silver), false), "GI"},
            { (typeof(Gold), false), "KI"},
            { (typeof(Bishop), false), "KA"},
            { (typeof(Rook), false), "HI"},
            { (typeof(King), false), "OU"},
            { (typeof(Pawn), true), "TO"},
            { (typeof(Lance), true), "NY"},
            { (typeof(Knight), true), "NK"},
            { (typeof(Silver), true), "NG"},
            { (typeof(Bishop), true), "UM"},
            { (typeof(Rook), true), "RY"},
            { (typeof(All), false), "AL"},
        };

        public static Piece ToPiece(this string piece, BlackWhite bw)
        {
            var value = _PieceMap[piece];
            return (Piece)Activator.CreateInstance(value.type, bw, value.promoted);
        }

        public static Piece Clone(this Piece piece) => (Piece)Activator.CreateInstance(piece.GetType(), piece.BW, piece.Promoted);

        public static string ToCSAString(this BlackWhite bw) => bw == BlackWhite.Black ? "+" : "-";

        public static string ToPieceString(this Type t, bool promoted) => _StrMap[(t, promoted)];

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

        public static void AddMove(this Board board, MoveStatement moveStatement)
        {
            var number = board.Moves.Any() ? board.Moves.Count + 1 : 1;
            var move = new Move(board.Moves.Any() ? board.Moves.Last().BlackWhite.Reverse() : board.Turn, number);

            move.SrcPosX = moveStatement.PrevPositionX;
            move.SrcPosY = moveStatement.PrevPositionY;
            move.DestPosX = moveStatement.NextPositionX;
            move.DestPosY = moveStatement.NextPositionY;

            // 継ぎ盤を使って判定
            move.IsPromote = move.IsDrop ? false : (
                !board.SubBoard[moveStatement.PrevPositionX, moveStatement.PrevPositionY].Promoted && 
                    moveStatement.Piece.Promoted);

            move.Piece = moveStatement.Piece.Clone();

            if(move.IsPromote)
            {
                // CSAの場合、成りの場合すでに成駒になっているので、
                // 成桂成ると表記されるのを防ぐため戻す
                move.Piece.Promoted = false;
            }

            move.IsSame = board.Moves.LastOrDefault()?.DestPosX == moveStatement.NextPositionX &&
                                board.Moves.LastOrDefault()?.DestPosY == moveStatement.NextPositionY;

            board.Moves.Add(move);

            // 継ぎ盤更新
            board.SubBoard.Moves.Add(move);
            board.SubBoard.Move();
        }
    }
}
