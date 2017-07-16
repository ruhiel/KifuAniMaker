using KifuAniMaker.Shogi.Parser.CSA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                new MovieGenerator().MakeAnimation(options);
            }
        }
    }
}
