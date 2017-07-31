using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement
{
    public class NullStatement : ICSAStatement
    {
        private string _Command;
        public NullStatement(string command)
        {
            _Command = command;
        }
        public Board Execute(Board board) => board;

        public override string ToString() => nameof(NullStatement) + _Command;
    }
}
