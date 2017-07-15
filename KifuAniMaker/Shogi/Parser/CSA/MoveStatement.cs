using System;
using KifuAniMaker.Shogi.Pieces;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    internal class MoveStatement : ICSAStatement
    {
        private BlackWhite _BlackWhite;
        private int _PrevPositionX;
        private int _PrevPositionY;
        private int _NextPositionX;
        private int _NextPositionY;
        private Piece _Piece;

        public MoveStatement(BlackWhite bw, int prevPositionX, int prevPositionY, int nextPositionX, int nextPositionY, Piece piece)
        {
            _BlackWhite = bw;
            _PrevPositionX = prevPositionX;
            _PrevPositionY = prevPositionY;
            _NextPositionX = nextPositionX;
            _NextPositionY = nextPositionY;
            _Piece = piece;
        }

        public Board Execute(Board board)
        {
            return board;
        }
    }
}