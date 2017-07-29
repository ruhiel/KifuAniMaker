using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KifuAniMaker.Shogi.Parser.CSA;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using KifuAniMaker.Shogi;

namespace KifuAniMakerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static string NL = Environment.NewLine;
        [TestMethod]
        public void TestMethod1()
        {
            var board = new Board();
            board.InitBoard();
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var statementTypes = new[]
            {
                typeof(CommentStatement),
                typeof(MoveStatement),
                typeof(NullStatement),
                typeof(SetEndTime),
                typeof(SetEvent),
                typeof(SetOpening),
                typeof(SetPlayer),
                typeof(SetPosition),
                typeof(SetPositionPiece),
                typeof(SetPositionBulk),
                typeof(SetSite),
                typeof(SetStartTime),
                typeof(SetTime),
                typeof(SetTimeLimit),
                typeof(SetTurn),
                typeof(SetVersion),
                typeof(SpecialStatement),};

            var dic = new Dictionary<Type, string>()
            {
                { typeof(CommentStatement), "'コメント"},
                { typeof(MoveStatement), "+7776FU"},
                { typeof(NullStatement), "  "},
                { typeof(SetEndTime), "$END_TIME:2017/07/11 14:00:00"},
                { typeof(SetEvent), "$EVENT:加古川青流戦"},
                { typeof(SetOpening), "$OPENING:中飛車"},
                { typeof(SetPlayer), "N-藤井聡太 四段"},
                { typeof(SetPosition), "PI82HI22KA"},
                { typeof(SetPositionPiece), "P-22KA" },
                { typeof(SetPositionBulk), "P1-KY-KE-GI-KI-OU-KI-GI-KE-KY" },
                { typeof(SetSite), "$SITE:関西将棋会館"},
                { typeof(SetStartTime), "$START_TIME:2017/07/11 14:00:00"},
                { typeof(SetTime), "T12"},
                { typeof(SetTimeLimit), "$TIME_LIMIT:00:25+00"},
                { typeof(SetTurn), "+"},
                { typeof(SetVersion), "V2.2"},
                { typeof(SpecialStatement), "%TORYO"},
            };

            foreach(var patten in Perm(statementTypes, 3))
            {
                var str = string.Join(Environment.NewLine, patten.Select(x => dic[x]));
                System.Diagnostics.Trace.WriteLine("-------------------");
                System.Diagnostics.Trace.WriteLine(str);
                var enumerable = (IEnumerable<IEnumerable<ICSAStatement>>)typeof(CSAParser).InvokeMember("ParseDocument", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[] { str });

                Assert.IsTrue(enumerable.Any());

                var result = enumerable.First();
                var p = patten.Where(x => x != typeof(NullStatement));
                Assert.AreEqual(p.Count(), result.Count());

                Assert.IsTrue(p.Zip(result, (first, second) => (first, second)).All(x => x.Item1 == x.Item2.GetType()));
            }
        }

        public static IEnumerable<IEnumerable<T>> Perm<T>(IEnumerable<T> items, int? k = null)
        {
            if (k == null)
            {
                k = items.Count();
            }

            if (k == 0)
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                var i = 0;
                foreach (var x in items)
                {
                    var xs = items.Where((_, index) => i != index);
                    foreach (var c in Perm(xs, k - 1))
                    {
                        yield return Before(c, x);
                    }

                    i++;
                }
            }
        }

        // 要素をシーケンスに追加するユーティリティ
        public static IEnumerable<T> Before<T>(IEnumerable<T> items, T first)
        {
            yield return first;

            foreach (var i in items)
            {
                yield return i;
            }
        }
    }
}
