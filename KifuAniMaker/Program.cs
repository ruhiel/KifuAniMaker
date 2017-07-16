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
            using (var sr = new System.IO.StreamReader(Path.Combine(Directory.GetCurrentDirectory(),@"TextFile1.csa"), Encoding.Default))
            {
                // ファイルの最後まで読み込む
                var content = sr.ReadToEnd();

                var board = CSAParser.ParseContent(content);

                board.Paint(Path.Combine(Directory.GetCurrentDirectory(), @"test.png"));
            }
        }
    }
}
