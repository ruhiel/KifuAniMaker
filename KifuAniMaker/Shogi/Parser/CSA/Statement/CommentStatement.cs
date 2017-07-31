using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement
{
    public class CommentStatement : ICSAStatement
    {
        private string _Comment;
        public CommentStatement(string comment)
        {
            _Comment = comment;
        }

        public Board Execute(Board board) => board;

        public override string ToString() => nameof(CommentStatement);
    }
}
