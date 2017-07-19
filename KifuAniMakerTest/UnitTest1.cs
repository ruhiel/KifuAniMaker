using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KifuAniMaker.Shogi.Parser.CSA;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace KifuAniMakerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var line = $"T10{Environment.NewLine}T12{Environment.NewLine}T11";

            var result = (IEnumerable<IEnumerable<ICSAStatement>>)typeof(CSAParser).InvokeMember("ParseDocument", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[] { line });

            Assert.IsTrue(result.Any());

            Assert.IsTrue(result.First().All(x => x is SetTime));
        }
    }
}
