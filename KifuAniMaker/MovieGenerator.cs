using KifuAniMaker.Shogi;
using KifuAniMaker.Shogi.Parser.CSA;
using ShellProgressBar;
using Sprache;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker
{
    public class MovieGenerator
    {
        public void MakeAnimation(Options options)
        {
            string content;
            Console.WriteLine($"入力棋譜ファイル:{options.InputFile}");
            try
            {
                content = ReadFile(options.InputFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"棋譜ファイルの読み込みに失敗しました。{e.Message}");
                return;
            }

            List<Board> boards;

            try
            {
                boards = CSAParser.ParseContent(content);

                if(!boards.Any())
                {
                    Console.WriteLine($"棋譜ファイル内に棋譜が存在しませんでした。");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"棋譜ファイルの解析に失敗しました。{e.Message}");
                return;
            }

            // TODO:複数対応
            var board = boards.First();

            var maxTicks = board.Moves.Count;
            var pbarOptions = new ProgressBarOptions();
            pbarOptions.BackgroundColor = ConsoleColor.Cyan;
            pbarOptions.ProgressCharacter = '*';
            pbarOptions.DisplayTimeInRealTime = true;
            var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);

            Console.WriteLine($"棋譜画像出力中");

            var images = new List<string>();

            using (var pbar = new ProgressBar(maxTicks, "棋譜画像出力中", pbarOptions))
            {
                foreach (var file in FrameFileEnumerator(tempDir))
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

            var argument = $"-r {options.InputFps} -i {Path.Combine(tempDir, "result%03d.png")} {options.FFmpegOptions} -r {options.OutputFps} -y {outFile}";

            var ffmpeg = Path.Combine(options.FFmpegPath, "ffmpeg");

            var psInfo = new ProcessStartInfo()
            {
                FileName = ffmpeg,    // 実行するファイル 
                Arguments = argument,    // コマンドパラメータ（引数）
                CreateNoWindow = true,    // コンソール・ウィンドウを開かない
                UseShellExecute = false,  // シェル機能を使用しない
            };

            Process process;
            try
            {
                process = Process.Start(psInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ffmpegの実行に失敗しました。{e.Message}");
                return;
            }

            process.WaitForExit();
            Console.WriteLine($"ffmpeg実行結果:{process.ExitCode}");

            Console.WriteLine($"棋譜動画出力完了");

            Console.WriteLine($"出力動画ファイル:{outFile}");

            Console.WriteLine("棋譜画像出力中");
            foreach (var png in images)
            {
                File.Delete(png);
            }

            Directory.Delete(tempDir);

            Console.WriteLine("続行するには何かキーを押してください . . .");
            Console.ReadKey();
        }

        private string ReadFile(string fileName)
        {
            if(fileName.StartsWith("http"))
            {
                using (var wc = new WebClient())
                using (var st = wc.OpenRead(fileName))
                using (var sr = new StreamReader(st, Encoding.Default))
                {
                    return sr.ReadToEnd();
                }
            }
            else
            {
                using (var sr = new StreamReader(fileName, Encoding.Default))
                {
                    return sr.ReadToEnd();
                }
            }


        }

        public IEnumerable<string> FrameFileEnumerator(string dir)
        {
            uint index = 0;
            while (true)
            {
                yield return Path.Combine(dir, $"result{index:D3}.png");
                index++;
            }
        }
    }
}
