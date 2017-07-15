﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    public class SetVersion : ICSAStatement
    {
        public string Version { get; set; }
        public SetVersion(string version)
        {
            Version = version;
        }

        public Board Execute(Board board) => board;
    }
}
