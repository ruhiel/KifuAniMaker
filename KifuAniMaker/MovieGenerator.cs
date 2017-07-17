using KifuAniMaker.Shogi.Parser.CSA;
using ShellProgressBar;
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
                Console.WriteLine($"入力棋譜ファイル:{options.InputFile}");
                var images = new List<string>();

                // ファイルの最後まで読み込む
                var content = sr.ReadToEnd();

                // TODO:複数対応
                var boards = CSAParser.ParseContent(content);
                var board = boards.First();

                var maxTicks = board.Moves.Count;
                var pbarOptions = new ProgressBarOptions();
                pbarOptions.BackgroundColor = ConsoleColor.Cyan;
                pbarOptions.ProgressCharacter = '*';
                pbarOptions.DisplayTimeInRealTime = true;
                Console.WriteLine($"棋譜画像出力中");
                using (var pbar = new ProgressBar(maxTicks, "棋譜画像出力中", pbarOptions))
                {
                    foreach (var file in FrameFileEnumerator())
                    {
                        var move = board.Paint(file);
                        images.Add(file);
                        pbar.Tick($"{move.ToAsciiString()}");
                        if (!board.HasNext)
                        {
                            break;
                        }
                        board.Move();
                    }
                }
                
                Console.WriteLine($"棋譜画像出力完了");

                Console.WriteLine("棋譜動画出力中");

                var outFile = options.OutputFile ?? Path.Combine(Path.GetDirectoryName(options.InputFile), $"{Path.GetFileNameWithoutExtension(options.InputFile)}.mp4");

                var argument = $"-r {options.InputFps} -i {Path.Combine(Path.GetTempPath(), "result%03d.png")} {options.FFmpegOptions} -r {options.OutputFps} -y {outFile}";

                var ffmpeg = Path.Combine(options.FFmpegPath, "ffmpeg");
                var psInfo = new ProcessStartInfo()
                {
                    FileName = ffmpeg,    // 実行するファイル 
                    Arguments = argument,    // コマンドパラメータ（引数）
                    CreateNoWindow = true,    // コンソール・ウィンドウを開かない
                    UseShellExecute = false,  // シェル機能を使用しない
                };

                var p = Process.Start(psInfo);
                p.WaitForExit();
                Console.WriteLine($"ffmpeg実行結果:{p.ExitCode}");

                Console.WriteLine($"棋譜動画出力完了");

                Console.WriteLine($"出力動画ファイル:{outFile}");

                Console.WriteLine("棋譜画像出力中");
                foreach (var png in images)
                {
                    File.Delete(png);
                }

                Console.WriteLine("続行するには何かキーを押してください . . .");
                Console.ReadKey();
            }
        }

        public IEnumerable<string> FrameFileEnumerator()
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
