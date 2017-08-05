using KifuAniMaker.Shogi.Moves.Situations;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class CheckmateStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Add(new Checkmate(board.NextBlackWhite, board.NextMoveNumber));

            return board;
        }
    }
}
