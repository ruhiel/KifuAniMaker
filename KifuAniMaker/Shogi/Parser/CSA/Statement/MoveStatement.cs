using System;
using KifuAniMaker.Shogi.Pieces;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement
{
    public class MoveStatement : ICSAStatement
    {
        private BlackWhite _BlackWhite;
        public int PrevPositionX { get; set; }
        public int PrevPositionY { get; set; }
        public int NextPositionX { get; set; }
        public int NextPositionY { get; set; }
        public Piece Piece { get; set; }

        public MoveStatement(BlackWhite bw, int prevPositionX, int prevPositionY, int nextPositionX, int nextPositionY, Piece piece)
        {
            _BlackWhite = bw;
            PrevPositionX = prevPositionX;
            PrevPositionY = prevPositionY;
            NextPositionX = nextPositionX;
            NextPositionY = nextPositionY;
            Piece = piece;
        }

        public Board Execute(Board board)
        {
            board.AddMove(this);

            return board;
        }

        public override string ToString() => nameof(MoveStatement);
    }
}