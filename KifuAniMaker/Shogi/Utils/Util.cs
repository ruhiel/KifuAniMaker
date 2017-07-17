using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Utils
{
    public static class Util
    {
        private static string[] XStrings = new string [] {"１", "２", "３", "４", "５", "６", "７", "８", "９"};

        private static string[] YStrings = new string[] { "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        public static BlackWhite Reverse(this BlackWhite bw) => bw == BlackWhite.Black ? BlackWhite.White : BlackWhite.Black;

        public static string ToSymbol(this BlackWhite bw) => bw == BlackWhite.Black ? "☗" : "☖";

        public static string ToAsciiSymbol(this BlackWhite bw) => bw == BlackWhite.Black ? "▲" : "△";

        public static string ToJapaneseStringX(this int x) => XStrings[x - 1];

        public static string ToJapaneseStringY(this int y) => YStrings[y - 1];
    }
}
