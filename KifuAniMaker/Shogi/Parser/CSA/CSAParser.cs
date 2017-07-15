using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifuAniMaker.Shogi.Parser.CSA
{
    /// <summary>
    /// http://www.computer-shogi.org/protocol/record_v22.html
    /// </summary>
    public class CSAParser
    {
        public static void ParseContent(string content)
        {
            // 先後手番
            var blackWhiteParser =
                from bw in Parse.Regex(@"\+|\-")
                select bw;

            // 駒
            var pieceParser =
                from piece in Parse.Regex("(FU)|(KY)|(KE)|(GI)|(KI)|(KA)|(HI)|(OU)|(TO)|(NY)|(NK)|(NG)|(UM)|(RY)")
                select piece;

            // 位置付き駒
            var pieceWithPositionParser =
                from postion in Parse.Regex(@"\d{2}")
                from piece in pieceParser
                select $"{postion}{piece}";

            // 先後付き駒
            var pieceWithBlackWhite =
                from bw in blackWhiteParser
                from piece in pieceParser
                select $"{bw}{piece}";

            // 先後位置付き駒
            var pieceFullParser =
                from bw in blackWhiteParser
                from postion in Parse.Regex(@"\d{2}")
                from piece in pieceParser
                select $"{bw}{postion}{piece}";

            // バージョン
            var versionParser =
                from v in Parse.Char('V')
                from version in Parse.Regex("[0-9.]+")
                select (ICSAStatement)new SetVersion(version);

            // 対局者名
            var playerParser =
                from n in Parse.Char('N').Token()
                from bw in blackWhiteParser
                from player in Parse.Regex(".+").Token()
                select (ICSAStatement)new SetPlayer(bw.ToBlackWhite(), player);

            // 各種棋譜情報
            // 棋戦名
            var gameNameParser =
                from key in Parse.String(@"$EVENT:").Token()
                from @event in Parse.Regex(".+").Token()
                select (ICSAStatement)new SetEvent(@event);

             // 対局場所
             var locationParser =
                from key in Parse.String(@"$SITE:").Token()
                from site in Parse.Regex(".+").Token()
                select (ICSAStatement)new SetSite(site);

            // 対局開始日時
            var gameStartTimeParser =
                from key in Parse.String(@"$START_TIME:").Token()
                from datetime in Parse.Regex("[0-9/: ]+").Token()
                select (ICSAStatement)new SetStartTime(DateTime.Parse(datetime));

            // 対局終了日時
            var gameEndTimeParser =
                from key in Parse.String(@"$END_TIME:").Token()
                from datetime in Parse.Regex("[0-9/: ]+").Token()
                select (ICSAStatement)new SetEndTime(DateTime.Parse(datetime));

            // 持ち時間
            var remainTimeParser =
                from key in Parse.String(@"$TIME_LIMIT:").Token()
                from remainTimeMinute in Parse.Number
                from collon in Parse.Char(':')
                from remainTimeSecond in Parse.Number
                from plus in Parse.Char('+')
                from secondTime in Parse.Number
                select (ICSAStatement)new SetTimeLimit(new TimeSpan(0, int.Parse(remainTimeMinute), int.Parse(remainTimeSecond)), new TimeSpan(0, 0, int.Parse(secondTime)));

            // 戦型
            var openningNameParser =
                from key in Parse.String(@"$OPENING:").Token()
                from openingName in Parse.Regex(".+").Token()
                select (ICSAStatement)new SetOpening(openingName);

            // 開始局面
            var startingPositionParser =
                from key in Parse.String("PI").Text()
                from pieces in pieceWithPositionParser.Many()
                select (ICSAStatement)new SetPosition(pieces);

            // 開始局面(一括)
            var startingPositionBulkParser =
                from p in Parse.Char('P')
                from num in Parse.Regex("[0-9]")
                from pieces in (pieceWithBlackWhite.Or(Parse.Regex("..."))).Repeat(9)
                from ret in Parse.String("\r\n")
                select (ICSAStatement)new SetPositionBulk(int.Parse(num), pieces);

            // 消費時間
            var timeParser =
                from t in Parse.Char('T')
                from time in Parse.Number
                select (ICSAStatement)new SetTime(int.Parse(time));

            // 先後手番
            var blackWhiteTurnParser =
                from bw in blackWhiteParser
                from ret in Parse.String("\r\n")
                select (ICSAStatement)new SetTurn(bw.ToBlackWhite());

            // コメント
            var commentParser =
                from value in Parse.Regex("^'.*").Token()
                select (ICSAStatement)new NullStatement();

            // 指し手
            var moveParser =
                from bw in blackWhiteParser.Token()
                from prevPositionX in Parse.LetterOrDigit
                from prevPositionY in Parse.LetterOrDigit
                from nextPositionX in Parse.LetterOrDigit
                from nextPositionY in Parse.LetterOrDigit
                from piece in pieceParser.Token()
                select (ICSAStatement)new MoveStatement(bw.ToBlackWhite(),
                                            int.Parse(prevPositionX.ToString()),
                                            int.Parse(prevPositionY.ToString()),
                                            int.Parse(nextPositionX.ToString()),
                                            int.Parse(nextPositionY.ToString()),
                                            piece.ToPiece(bw.ToBlackWhite()));

            // 特殊な指し手、終局状況
            var specialMoveParser =
                from p in Parse.Char('%').Token()
                from key in Parse.Regex(".+").Token()
                select (ICSAStatement)new SpecialStatement(key);

            var nullParser =
                from value in Parse.Regex(".*").Or(Parse.Return(string.Empty)).Token()
                select (ICSAStatement)new NullStatement();

            var oneStatementParser = versionParser
                                        .Or(playerParser)
                                        .Or(gameNameParser)
                                        .Or(locationParser)
                                        .Or(gameStartTimeParser)
                                        .Or(gameEndTimeParser)
                                        .Or(remainTimeParser)
                                        .Or(openningNameParser)
                                        .Or(startingPositionBulkParser)
                                        .Or(startingPositionParser)
                                        .Or(timeParser)
                                        .Or(commentParser)
                                        .Or(blackWhiteTurnParser)
                                        .Or(moveParser)
                                        .Or(specialMoveParser)
                                        .Or(nullParser);

            var moreStateParser =
                from comma in Parse.Char(',').Token()
                from st in oneStatementParser
                select st;

            var statementParser =
                from st in oneStatementParser.Once()
                from mst in moreStateParser.Many()
                select st.Concat(mst);

            var oneRecordParser = statementParser.Many();

            var moreRecordParser = from separtor in Parse.Regex(@"/\r\n")
                             from rec in oneRecordParser
                                   select rec;

            var documentParser =
                (from one in oneRecordParser.Once()
                 from more in moreRecordParser.Many()
                 select one.Concat(more)).End();      

            foreach (var redords in documentParser.Parse(content))
            {
                foreach(var statements in redords)
                {
                    foreach(var statement in statements)
                    {
                        Console.WriteLine(statement);
                    }
                }
            }
        }
    }
}
