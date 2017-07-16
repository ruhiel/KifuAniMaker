using KifuAniMaker.Shogi.Parser.CSA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker
{
    public class MovieGenerator
    {
        public void MakeAnimation(Options options)
        {
            using (var sr = new StreamReader(options.InputFile, Encoding.Default))
            {
                var images = new List<string>();

                // ファイルの最後まで読み込む
                var content = sr.ReadToEnd();

                var board = CSAParser.ParseContent(content);

                foreach(var file in FrameFileEnumrator())
                {
                    board.Paint(file);
                    images.Add(file);
                    if (!board.HasNext)
                    {
                        break;
                    }
                    board.Move();
                }

                foreach (var png in images)
                {
                    File.Delete(png);
                }
            }

            /*
            //var record = KifParserFactory.Create(options).ReadFile();

            Console.WriteLine("棋譜画像出力中");
            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine($"棋譜画像出力完了:{sw.Elapsed}");
            Console.WriteLine("棋譜動画出力中");
            sw.Restart();

            var outFile = options.OutputFile ?? Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(options.InputFile)}.mp4");

            var argument = $"-r 1 -i {Path.Combine(Path.GetTempPath(), "result%d.png")} -vcodec libx264 -pix_fmt yuv420p -r 30 -y {outFile}";

            var psInfo = new ProcessStartInfo()
            {
                FileName = @"ffmpeg",    // 実行するファイル 
                Arguments = argument,    // コマンドパラメータ（引数）
                CreateNoWindow = true,    // コンソール・ウィンドウを開かない
                UseShellExecute = false,  // シェル機能を使用しない
            };

            var p = Process.Start(psInfo);
            p.WaitForExit();
            Console.WriteLine($"ffmpeg実行結果:{p.ExitCode}");


            Console.WriteLine($"棋譜動画出力完了:{sw.Elapsed}");
            */
        }

        public IEnumerable<string> FrameFileEnumrator()
        {
            uint index = 0;
            while (true)
            {
                yield return Path.Combine(Path.GetTempPath(), $"result{index:D3}.png");
                index++;
            }
        }
    }
}
