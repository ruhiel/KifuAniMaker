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
        // 先後手番
        public static Parser<string> BlackWhiteParser =
            from bw in Parse.Regex(@"\+|\-")
            select bw;

        // 駒
        public static Parser<string> PieceParser =
            from piece in Parse.Regex("(FU)|(KY)|(KE)|(GI)|(KI)|(KA)|(HI)|(OU)|(TO)|(NY)|(NK)|(NG)|(UM)|(RY)")
            select piece;

        private static IEnumerable<IEnumerable<IEnumerable<ICSAStatement>>> ParseDocumentContent(string content)
        {
            // 位置付き駒
            var pieceWithPositionParser =
                from postionX in Parse.Digit
                from postionY in Parse.Digit
                from piece in PieceParser
                select (int.Parse(postionX.ToString()), int.Parse(postionY.ToString()), piece);

            // 先後付き駒
            var pieceWithBlackWhite =
                from bw in BlackWhiteParser
                from piece in PieceParser
                select $"{bw}{piece}";

            // バージョン
            var versionParser =
                from v in Parse.Char('V')
                from version in Parse.Regex("[0-9.]+").Token()
                select (ICSAStatement)new SetVersion(version);

            // 対局者名
            var playerParser =
                from n in Parse.Char('N').Token()
                from bw in BlackWhiteParser
                from player in Parse.Regex(@"[^\r\n]+")
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetPlayer(bw.ToBlackWhite(), player);

            // 各種棋譜情報
            // 棋戦名
            var gameNameParser =
                from key in Parse.String(@"$EVENT:").Token()
                from @event in Parse.Regex(@"[^\r\n]+")
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetEvent(@event);

            // 対局場所
            var locationParser =
               from key in Parse.String(@"$SITE:").Token()
               from site in Parse.Regex(@"[^\r\n]+")
               from ret in Parse.Regex("[\r\n]+").Optional()
               select (ICSAStatement)new SetSite(site);

            // 対局開始日時
            var gameStartTimeParser =
                from key in Parse.String(@"$START_TIME:").Token()
                from datetime in Parse.Regex("[0-9/: ]+")
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetStartTime(DateTime.Parse(datetime));

            // 対局終了日時
            var gameEndTimeParser =
                from key in Parse.String(@"$END_TIME:").Token()
                from datetime in Parse.Regex("[0-9/: ]+")
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetEndTime(DateTime.Parse(datetime));

            // 持ち時間
            var remainTimeParser =
                from key in Parse.String(@"$TIME_LIMIT:").Token()
                from remainTimeMinute in Parse.Number
                from collon in Parse.Char(':')
                from remainTimeSecond in Parse.Number
                from plus in Parse.Char('+')
                from secondTime in Parse.Number
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetTimeLimit(new TimeSpan(0, int.Parse(remainTimeMinute), int.Parse(remainTimeSecond)), new TimeSpan(0, 0, int.Parse(secondTime)));

            // 戦型
            var openningNameParser =
                from key in Parse.String(@"$OPENING:").Token()
                from openingName in Parse.Regex(@"[^\r\n]+")
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetOpening(openingName);

            // 開始局面
            var startingPositionParser =
                from key in Parse.String("PI").Text()
                from pieces in pieceWithPositionParser.Many()
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetPosition(pieces);

            // 開始局面(一括)
            var startingPositionBulkParser =
                from p in Parse.Char('P')
                from num in Parse.Digit
                from pieces in (pieceWithBlackWhite.Or(Parse.Regex("[^\r\n]{3}"))).Repeat(9)
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetPositionBulk(int.Parse(num.ToString()), pieces.Select(x => x.WithBlackWhiteToPiece()));

            // 消費時間
            var timeParser =
                from t in Parse.Char('T')
                from time in Parse.Number
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetTime(int.Parse(time));

            // 先後手番
            var blackWhiteTurnParser =
                from bw in BlackWhiteParser
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SetTurn(bw.ToBlackWhite());

            // コメント
            var commentParser =
                from value in Parse.Regex("^'.*").Token()
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new CommentStatement(value);

            // 指し手
            var moveParser =
                from bw in BlackWhiteParser
                from prevPositionX in Parse.LetterOrDigit
                from prevPositionY in Parse.LetterOrDigit
                from nextPositionX in Parse.LetterOrDigit
                from nextPositionY in Parse.LetterOrDigit
                from piece in PieceParser.Token()
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new MoveStatement(bw.ToBlackWhite(),
                                            int.Parse(prevPositionX.ToString()),
                                            int.Parse(prevPositionY.ToString()),
                                            int.Parse(nextPositionX.ToString()),
                                            int.Parse(nextPositionY.ToString()),
                                            piece.ToPiece(bw.ToBlackWhite()));

            // 特殊な指し手、終局状況
            var specialMoveParser =
                from p in Parse.Char('%').Token()
                from key in Parse.Letter.Many().Text()
                from ret in Parse.Regex("[\r\n]+").Optional()
                select (ICSAStatement)new SpecialStatement(key);

            var nullParser =
                from value in Parse.Regex("^[^/].+").Or(Parse.Return(string.Empty)).Token()
                select (ICSAStatement)new NullStatement(value);

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
                                        .Or(moveParser)
                                        .Or(blackWhiteTurnParser)
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

            return
                from d in documentParser.Parse(content)
                select d;
        }

        private static IEnumerable<IEnumerable<ICSAStatement>> ParseDocument(string content)
        {
            foreach (var document in ParseDocumentContent(content))
            {
                yield return 
                    from statements in document
                                    from statement in statements
                                    select statement;
            }
        }

        public static List<Board> ParseContent(string content)
        {
            var boards = new List<Board>();
            foreach(var statementList in ParseDocument(content))
            {
                var board = new Board();

                foreach (var statement in statementList)
                {
                    board = statement.Execute(board);
                }

                boards.Add(board);
            }

            return boards;
        }
    }
}
