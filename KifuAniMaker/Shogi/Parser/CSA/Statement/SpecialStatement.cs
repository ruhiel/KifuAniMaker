using KifuAniMaker.Shogi.Parser.CSA.Statement.Special;
using System;

namespace KifuAniMaker.Shogi.Parser.CSA.Statement
{
    public static class SpecialStatement
    {
        public static ICSAStatement Create(string key, BlackWhite? bw)
        {
            switch(key)
            {
                case "TORYO":
                    return new ResignStatement();
                case "CHUDAN":
                    return new InterruptionStatement();
                case "SENNICHITE":
                    return new SennichiteStatement();
                case "TIME_UP":
                    return new TimeUpStatement();
                case "ILLEGAL_MOVE":
                    return new IllegalMoveStatement();
                case "ILLEGAL_ACTION":
                    return new IllegalActionStatement(bw.Value);
                case "JISHOGI":
                    return new JishogiStatement();
                case "KACHI":
                    return new WinStatement();
                case "HIKIWAKE":
                    return new DrawStatement();
                case "MATTA":
                    return new RetractMoveStatement();
                case "TSUMI":
                    return new CheckmateStatement();
                case "FUZUMI":
                    return new NotCheckmateStatement();
                case "ERROR":
                    return new ErrorStatement();
                default:
                    break;
            }

            throw new ArgumentException(key);
        }
    }
}