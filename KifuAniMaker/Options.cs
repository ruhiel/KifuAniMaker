using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker
{
    public class Options
    {
        [Option('o', HelpText = "出力動画ファイルパス")]
        public string OutputFile
        {
            get;
            set;
        }

        [Option('i', Required = true, HelpText = "入力棋譜ファイルパス")]
        public string InputFile
        {
            get;
            set;
        }

        [Option('f', DefaultValue = "csa", HelpText = "棋譜ファイル形式(csa|kif|ki2)")]
        public string Format
        {
            get;
            set;
        }

        [Option('m', DefaultValue = "", HelpText = "ffmpegディレクトリパス")]
        public string FFmpegPath
        {
            get;
            set;
        }

        [Option("if", DefaultValue = 1, HelpText = "入力FPS")]
        public int InputFps
        {
            get;
            set;
        }

        [Option("of", DefaultValue = 30, HelpText = "入力FPS")]
        public int OutputFps
        {
            get;
            set;
        }

        [Option("ffmpegoptions", DefaultValue = "-vcodec libx264 -pix_fmt yuv420p", HelpText = "ffmpegオプション")]
        public string FFmpegOptions
        {
            get;
            set;
        }

        //(3)HelpOption属性
        [HelpOption]
        public string GetUsage()
        {
            //ヘッダーの設定
            var head = new HeadingInfo("KifuAniMaker", "Version 1.0");
            var help = new HelpText(head);
            help.Copyright = new CopyrightInfo("Ruhiel", 2017);
            help.AddPreOptionsLine("KifuAniMaker");

            //全オプションを表示(1行間隔)
            help.AdditionalNewLineAfterOption = true;
            help.AddOptions(this);

            return help.ToString();
        }
    }
}
