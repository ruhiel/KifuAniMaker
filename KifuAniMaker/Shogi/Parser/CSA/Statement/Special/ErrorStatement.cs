using KifuAniMaker.Shogi.Moves.Situations;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class ErrorStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Add(new Error(board.NextBlackWhite, board.NextMoveNumber));

            return board;
        }
    }
}
