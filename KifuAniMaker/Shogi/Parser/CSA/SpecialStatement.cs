using KifuAniMaker.Shogi.Moves;
using System;

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
                    board.Moves.Add(new Resign());
                    break;
                case "CHUDAN":
                    break;
                default:
                    throw new ArgumentException();
            }

            return board;
        }
    }
}