using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using KifuAniMaker.Shogi.Pieces;
using KifuAniMaker.Shogi.Utils;
using KifuAniMaker.Shogi.Moves;
using System.Drawing.Imaging;
using KifuAniMaker.Shogi.Moves.Situations;

namespace KifuAniMaker.Shogi
{
    public class Board : IEnumerable<Piece>
    {
        private Piece[,] _Pieces;

        /// <summary>
        /// 手番
        /// </summary>
        public BlackWhite Turn { get; set; }

        /// <summary>
        /// 先手駒台
        /// </summary>
        private List<Piece> _BlackHands = new List<Piece>();

        /// <summary>
        /// 後手駒台
        /// </summary>
        private List<Piece> _WhiteHands = new List<Piece>();

        /// <summary>
        /// 駒台
        /// </summary>
        /// <returns></returns>
        private List<Piece> GetHands() => GetHands(Turn);

        /// <summary>
        /// 駒台
        /// </summary>
        /// <param name="bw"></param>
        /// <returns></returns>
        public List<Piece> GetHands(BlackWhite bw) => bw == BlackWhite.Black ? _BlackHands : _WhiteHands;

        /// <summary>
        /// 指し手リスト
        /// </summary>
        public List<Move> Moves { get; set; } = new List<Move>();

        /// <summary>
        /// 再生済み指し手リスト
        /// </summary>
        public List<Move> Moved { get; set; } = new List<Move>();

        /// <summary>
        /// 棋戦名
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 対局場所
        /// </summary>
        public string Site { get; set; }

        /// <summary>
        /// 開始日時
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 終了日時
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 戦型
        /// </summary>
        public string Opening { get; set; }

        /// <summary>
        /// 先手対局者
        /// </summary>
        public string BlackPlayer { get; set; }

        /// <summary>
        /// 後手対局者
        /// </summary>
        public string WhitePlayer { get; set; }

        /// <summary>
        /// 棋戦名
        /// </summary>
        public string Event { get; set; }

        /// <summary>
        /// 持ち時間
        /// </summary>
        public TimeSpan RemainTime { get; internal set; }

        /// <summary>
        /// 秒読み
        /// </summary>
        public TimeSpan SecondTime { get; internal set; }

        /// <summary>
        /// 継ぎ盤
        /// </summary>
        public Board SubBoard { get; set; }

        /// <summary>
        /// 駒箱
        /// </summary>
        public List<Piece> PieceBox { get; set; }

        /// <summary>
        /// 次の指し手番号
        /// </summary>
        public int NextMoveNumber => Moves.Any() ? Moves.Count + 1 : 1;

        /// <summary>
        /// 次の指し手先後
        /// </summary>
        public BlackWhite NextBlackWhite => Moves.Any() ? Moves.Last().BlackWhite.Reverse() : BlackWhite.Black;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Board()
        {
            _Pieces = new Piece[9, 9];
            Turn = BlackWhite.Black;
            PieceBox = new List<Piece>()
            {
                new King(BlackWhite.Black),
                new King(BlackWhite.White),
                new Gold(BlackWhite.Black),
                new Gold(BlackWhite.White),
                new Gold(BlackWhite.Black),
                new Gold(BlackWhite.White),
                new Silver(BlackWhite.Black),
                new Silver(BlackWhite.White),
                new Silver(BlackWhite.Black),
                new Silver(BlackWhite.White),
                new Knight(BlackWhite.Black),
                new Knight(BlackWhite.White),
                new Knight(BlackWhite.Black),
                new Knight(BlackWhite.White),
                new Lance(BlackWhite.Black),
                new Lance(BlackWhite.White),
                new Lance(BlackWhite.Black),
                new Lance(BlackWhite.White),
                new Rook(BlackWhite.Black),
                new Rook(BlackWhite.White),
                new Bishop(BlackWhite.Black),
                new Bishop(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
                new Pawn(BlackWhite.Black),
                new Pawn(BlackWhite.White),
            };
        }

        public void MoveToPieceBox(int x, int y)
        {
            var piece = this[x, y];

            this[x, y] = null;

            if(piece != null)
            {
                PieceBox.Add(piece);
            }
        }

        public void MoveToHandsFromPieceBox(Type t, BlackWhite bw)
        {
            var piece = PieceBox.First(a => a.GetType() == t);
            PieceBox.Remove(piece);
            piece.BW = bw;
            GetHands(bw).Add(piece);
        }

        public void MoveAllToHandsFromPieceBox(BlackWhite bw)
        {
            GetHands(bw).AddRange(PieceBox);

            PieceBox.Clear();
        }

        public void SetFromPieceBox(int x, int y, Type t, BlackWhite bw)
        {
            var piece = PieceBox.First(a => a.GetType() == t);
            PieceBox.Remove(piece);
            piece.BW = bw;
            this[x, y] = piece;
        }

        public void InitBoard()
        {
            SetFromPieceBox(5, 9, typeof(King), BlackWhite.Black);
            SetFromPieceBox(5, 1, typeof(King), BlackWhite.White);
            SetFromPieceBox(4, 9, typeof(Gold), BlackWhite.Black);
            SetFromPieceBox(6, 1, typeof(Gold), BlackWhite.White);
            SetFromPieceBox(6, 9, typeof(Gold), BlackWhite.Black);
            SetFromPieceBox(4, 1, typeof(Gold), BlackWhite.White);
            SetFromPieceBox(3, 9, typeof(Silver), BlackWhite.Black);
            SetFromPieceBox(7, 1, typeof(Silver), BlackWhite.White);
            SetFromPieceBox(7, 9, typeof(Silver), BlackWhite.Black);
            SetFromPieceBox(3, 1, typeof(Silver), BlackWhite.White);
            SetFromPieceBox(2, 9, typeof(Knight), BlackWhite.Black);
            SetFromPieceBox(8, 1, typeof(Knight), BlackWhite.White);
            SetFromPieceBox(8, 9, typeof(Knight), BlackWhite.Black);
            SetFromPieceBox(2, 1, typeof(Knight), BlackWhite.White);
            SetFromPieceBox(1, 9, typeof(Lance), BlackWhite.Black);
            SetFromPieceBox(9, 1, typeof(Lance), BlackWhite.White);
            SetFromPieceBox(9, 9, typeof(Lance), BlackWhite.Black);
            SetFromPieceBox(1, 1, typeof(Lance), BlackWhite.White);
            SetFromPieceBox(2, 8, typeof(Rook), BlackWhite.Black);
            SetFromPieceBox(8, 2, typeof(Rook), BlackWhite.White);
            SetFromPieceBox(8, 8, typeof(Bishop), BlackWhite.Black);
            SetFromPieceBox(2, 2, typeof(Bishop), BlackWhite.White);
            SetFromPieceBox(5, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(5, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(4, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(6, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(6, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(4, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(3, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(7, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(7, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(3, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(2, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(8, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(8, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(2, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(1, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(9, 3, typeof(Pawn), BlackWhite.White);
            SetFromPieceBox(9, 7, typeof(Pawn), BlackWhite.Black);
            SetFromPieceBox(1, 3, typeof(Pawn), BlackWhite.White);
        }

        public bool HasNext => Moves.Any();

        public void Next()
        {
            var move = Moves.First();
            Moves.Remove(move);
            Moved.Add(move);

            Turn = Turn.Reverse();
        }

        public Move Paint(string path)
        {
            Move firstMove;
            //画像ファイルを読み込んでImageオブジェクトを作成する
            using (var img = new Bitmap(@"img\japanese-chess-b02.png"))
            using (var g = Graphics.FromImage(img))
            {
                const float baseX = 30.0f;
                const float baseY = 130.0f;

                // 盤
                for (var i = 0; i < 9; i++)
                {
                    for (var j = 0; j < 9; j++)
                    {
                        var piece = _Pieces[i, j];
                        if (piece != null)
                        {
                            using (var img2 = new Bitmap(@"img\" + piece.ImageFile))
                            {
                                g.DrawImage(img2, new PointF(baseX + j * 60, baseY + i * 64));
                            }
                        }
                    }
                }

                // 駒台
                _BlackHands.Sort(ComparePiece);
                var groups1 = _BlackHands.GroupBy(x => x.GetType());
                for(var i = 0; i < groups1.Count(); i++)
                {
                    var group = groups1.ElementAt(i);
                    using (var img2 = new Bitmap(@"img\" + group.First().ImageFile))
                    {
                        g.DrawImage(img2, new PointF(i * 60, 750.0f));
                    }

                    DrawNum(g, group.Count(), i, BlackWhite.Black);
                }

                _WhiteHands.Sort(ComparePiece);
                var groups2 = _WhiteHands.GroupBy(x => x.GetType());
                for (var i = 0; i < groups2.Count(); i++)
                {
                    var group = groups2.ElementAt(i);
                    using (var img2 = new Bitmap(@"img\" + group.First().ImageFile))
                    {
                        g.DrawImage(img2, new PointF(i * 60, 0.0f));
                    }

                    DrawNum(g, group.Count(), i, BlackWhite.White);
                }

                g.DrawString($"{BlackWhite.White.ToSymbol()}{WhitePlayer}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 40);

                var list = Moved.Any() ? new List<Move>() { Moved.Last() }.Concat(Moves.Take(9)) : Moves.Take(10);

                foreach (var element in list.Select((move , index) => new { move, index }))
                {
                    g.DrawString(element.move.ToString(), new Font("MS UI Gothic", 24), Brushes.Black, 600, 100 + element.index * 60);
                }

                firstMove = list.First();

                if(Moved.Any() && !(firstMove is Resign))
                {
                    g.DrawRectangle(new Pen(Brushes.Red, 5), baseX + (9-firstMove.DestPosX) * 60, baseY + (firstMove.DestPosY - 1) * 64, 60, 64);
                }

                g.DrawString($"{BlackWhite.Black.ToSymbol()}{BlackPlayer}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 700);
                
                //作成した画像を保存する
                img.Save(path, ImageFormat.Png);
            }
            return firstMove;
        }

        private void DrawNum(Graphics g, int count, int index, BlackWhite bw)
        {
            if(count == 1)
            {
                return;
            }
            else if(count > 9)
            {
                var degit10 = count / 10;
                var degit1 = count % 10;
                using (var img10 = new Bitmap(@"img\" + "number3_4" + degit10 + ".png"))
                using (var img1 = new Bitmap(@"img\" + "number3_4" + degit1 + ".png"))
                {
                    g.DrawImage(img10, new PointF(index * 60 + 55, bw == BlackWhite.Black ? 750.0f : 0.0f));
                    g.DrawImage(img1, new PointF(index * 60 + 65, bw == BlackWhite.Black ? 750.0f : 0.0f));
                }

            }
            else
            {
                using (var img1 = new Bitmap(@"img\" + "number3_4" + count + ".png"))
                {
                    g.DrawImage(img1, new PointF(index * 60 + 55, bw == BlackWhite.Black ? 750.0f : 0.0f));
                }
            }
        }

        private int ComparePiece(Piece a, Piece b)
        {
            var array = new List<Type>() { typeof(King), typeof(Rook), typeof(Bishop), typeof(Gold), typeof(Silver), typeof(Knight), typeof(Lance), typeof(Pawn) };
            return array.IndexOf(a.GetType()) - array.IndexOf(b.GetType());
        }

        public Piece this[int x, int y]
        {
            get
            {
                if (x < 1 || x > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                if (y < 1 || y > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                x = 9 - x;
                y -= 1;
                return _Pieces[y, x];
            }
            set
            {
                if (x < 1 || x > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                if (y < 1 || y > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                x = 9 - x;
                y -= 1;
                _Pieces[y, x] = value;
            }
        }

        private void Drop(Move move)
        {
            var piece = GetHands().First(x => x.GetType() == move.Piece.GetType());
            this[move.DestPosX, move.DestPosY] = piece;
            GetHands().Remove(piece);
        }

        private void CapturesPiece(Piece piece)
        {
            piece.Reverse();
            piece.Promoted = false;
            GetHands().Add(piece);
        }

        private void MovePiece(Move move)
        {
            this[move.DestPosX, move.DestPosY] = this[move.SrcPosX.Value, move.SrcPosY.Value];

            this[move.SrcPosX.Value, move.SrcPosY.Value] = null;
        }

        public void Move()
        {
            var move = Moves.First();
            var destPosX = move.DestPosX;
            var destPosY = move.DestPosY;
            if(move is Situation)
            {
                Moves.Remove(move);
                Moved.Add(move);
                return;
            }
            if (move.IsDrop)
            {
                Drop(move);
            }
            else
            {
                if(move.IsSame)
                {
                    destPosX = Moved.Last().DestPosX;
                    destPosY = Moved.Last().DestPosY;
                }

                var piece = this[destPosX, destPosY];

                if(piece != null)
                {
                    // 駒取り
                    CapturesPiece(piece);
                }

                MovePiece(move);

                if (move.IsPromote)
                {
                    // 成
                    this[destPosX, destPosY].Promote();
                }
            }

            Next();
        }

        public IEnumerator<Piece> GetEnumerator()
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    yield return _Pieces[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var line = "";

            line += _WhiteHands.Any() ? (string.Join(" ", _WhiteHands.Select(x => x.ToString()))) : string.Empty;
            line += $"{Environment.NewLine}{Environment.NewLine}";

            line += $" 9  8  7  6  5  4  3  2  1 {Environment.NewLine}";
            for (var y = 1; y <= 9; y++)
            {
                for (var x = 9; x >= 1; x--)
                {
                    var p = this[x, y];
                    line += p?.ToString() ?? " * "; 
                }

                line += $" {y}{Environment.NewLine}";
            }

            line += $"{Environment.NewLine}{Environment.NewLine}";
            line += _BlackHands.Any() ? (string.Join(" ", _BlackHands.Select(x => x.ToString()))) : string.Empty;

            return line;
        }
    }
}
