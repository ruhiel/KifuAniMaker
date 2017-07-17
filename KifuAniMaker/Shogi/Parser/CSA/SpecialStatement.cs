using KifuAniMaker.Shogi.Moves;
using KifuAniMaker.Shogi.Utils;
using System;
using System.Linq;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    internal class SpecialStatement : ICSAStatement
    {
        private string key;

        public SpecialStatement(string key)
        {
            this.key = key;
        }

        public Board Execute(Board board)
        {
            switch(key)
            {
                case "TORYO":
                    var number = board.Moves.Any() ? board.Moves.Count + 1 : 1;
                    board.Moves.Add(new Resign(board.Moves.Any() ? board.Moves.Last().BlackWhite.Reverse() : BlackWhite.Black, number));
                    break;
                case "CHUDAN":
                    break;
                default:
                    throw new ArgumentException();
            }

            return board;
        }

        public override string ToString() => nameof(SpecialStatement) + " " + key;
    }
}