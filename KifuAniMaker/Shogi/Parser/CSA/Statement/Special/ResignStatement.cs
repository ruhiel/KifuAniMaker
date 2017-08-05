using KifuAniMaker.Shogi.Moves;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class ResignStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Add(new Resign(board.NextBlackWhite, board.NextMoveNumber));

            return board;
        }
    }
}
