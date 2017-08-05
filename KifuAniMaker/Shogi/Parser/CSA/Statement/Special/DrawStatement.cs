using KifuAniMaker.Shogi.Moves.Situations;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement.Special
{
    public class DrawStatement : ICSAStatement
    {
        public Board Execute(Board board)
        {
            board.Moves.Add(new Draw(board.NextBlackWhite, board.NextMoveNumber));

            return board;
        }
    }
}
