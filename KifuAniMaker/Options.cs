using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        [HelpOption('h', "help")]
        public string GetUsage()
        {
            var title = ((System.Reflection.AssemblyTitleAttribute)Attribute.GetCustomAttribute(
                            System.Reflection.Assembly.GetExecutingAssembly(),
                            typeof(System.Reflection.AssemblyTitleAttribute))).Title;
            //ヘッダーの設定
            var head = new HeadingInfo(
                            title,
                            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

            var asmcpy =
                (System.Reflection.AssemblyCopyrightAttribute)
                Attribute.GetCustomAttribute(
                System.Reflection.Assembly.GetExecutingAssembly(),
                typeof(System.Reflection.AssemblyCopyrightAttribute));

            var regex = new Regex(@".+\s+(\d+)\s+(.+)");
            var m = regex.Match(asmcpy.Copyright);

            var help = new HelpText(head);
            if(m.Success)
            {
                help.Copyright = new CopyrightInfo(m.Groups[2].Value, int.Parse(m.Groups[1].Value));
            }
            
            help.AddPreOptionsLine("オプション一覧");

            //全オプションを表示(1行間隔)
            help.AdditionalNewLineAfterOption = true;
            help.AddOptions(this);

            return help.ToString();
        }
    }
}
