﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Diagnostics;
using KifuAniMaker.Shogi.Pieces;
using KifuAniMaker.Shogi.Utils;
using KifuAniMaker.Shogi.Moves;

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
        private List<Piece> _BlackHands;

        /// <summary>
        /// 後手駒台
        /// </summary>
        private List<Piece> _WhiteHands;

        /// <summary>
        /// 駒台
        /// </summary>
        /// <returns></returns>
        private List<Piece> GetHands() => Turn == BlackWhite.Black ? _BlackHands : _WhiteHands;

        /// <summary>
        /// 指し手リスト
        /// </summary>
        public List<Move> Moves { get; set; }

        /// <summary>
        /// 再生済み指し手リスト
        /// </summary>
        public List<Move> Moved { get; set; }

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
        public int Limit { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Board()
        {
            _Pieces = new Piece[9, 9];
            Turn = BlackWhite.Black;
        }

        public void InitBoard()
        {
            _BlackHands = new List<Piece>();
            _WhiteHands = new List<Piece>();

            this[5, 9] = new King(BlackWhite.Black);
            this[5, 1] = new King(BlackWhite.White);
            this[4, 9] = new Gold(BlackWhite.Black);
            this[6, 1] = new Gold(BlackWhite.White);
            this[6, 9] = new Gold(BlackWhite.Black);
            this[4, 1] = new Gold(BlackWhite.White);
            this[3, 9] = new Silver(BlackWhite.Black);
            this[7, 1] = new Silver(BlackWhite.White);
            this[7, 9] = new Silver(BlackWhite.Black);
            this[3, 1] = new Silver(BlackWhite.White);
            this[2, 9] = new Knight(BlackWhite.Black);
            this[8, 1] = new Knight(BlackWhite.White);
            this[8, 9] = new Knight(BlackWhite.Black);
            this[2, 1] = new Knight(BlackWhite.White);
            this[1, 9] = new Lance(BlackWhite.Black);
            this[9, 1] = new Lance(BlackWhite.White);
            this[9, 9] = new Lance(BlackWhite.Black);
            this[1, 1] = new Lance(BlackWhite.White);
            this[2, 8] = new Rook(BlackWhite.Black);
            this[8, 2] = new Rook(BlackWhite.White);
            this[8, 8] = new Bishop(BlackWhite.Black);
            this[2, 2] = new Bishop(BlackWhite.White);
            this[5, 7] = new Pawn(BlackWhite.Black);
            this[5, 3] = new Pawn(BlackWhite.White);
            this[4, 7] = new Pawn(BlackWhite.Black);
            this[6, 3] = new Pawn(BlackWhite.White);
            this[6, 7] = new Pawn(BlackWhite.Black);
            this[4, 3] = new Pawn(BlackWhite.White);
            this[3, 7] = new Pawn(BlackWhite.Black);
            this[7, 3] = new Pawn(BlackWhite.White);
            this[7, 7] = new Pawn(BlackWhite.Black);
            this[3, 3] = new Pawn(BlackWhite.White);
            this[2, 7] = new Pawn(BlackWhite.Black);
            this[8, 3] = new Pawn(BlackWhite.White);
            this[8, 7] = new Pawn(BlackWhite.Black);
            this[2, 3] = new Pawn(BlackWhite.White);
            this[1, 7] = new Pawn(BlackWhite.Black);
            this[9, 3] = new Pawn(BlackWhite.White);
            this[9, 7] = new Pawn(BlackWhite.Black);
            this[1, 3] = new Pawn(BlackWhite.White);
        }

        public void Next()
        {
            var move = Moves.First();
            Moves.Remove(move);
            Moved.Add(move);

            Turn = Turn.Reverse();
        }

        /*
        public string Paint(int idx, Record record)
        {
            var path = Path.Combine(Path.GetTempPath(), $"result{idx.ToString()}.png");

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

                if (record.Moves[idx].ActionString != "投了")
                {
                    g.DrawRectangle(new Pen(Brushes.Red, 2), new Rectangle((int)baseX + (9 - record.Moves[idx].DestPosX) * 60, (int)baseY + (record.Moves[idx].DestPosY - 1) * 64, 60, 64));
                }

                g.DrawString($"▽{record.WhitePlayer}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 40);

                foreach (var element in record.Moves.Skip(idx).Take(10).Select((move , index) => new { move, index }))
                {
                    g.DrawString($"{element.move.MoveNum} {element.move.BlackWhite.ToSymbol()} {element.move.MoveString}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 100 + element.index * 60);
                }

                g.DrawString($"▲{record.BlackPlayer}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 700);

                //作成した画像を保存する
                img.Save(path, ImageFormat.Png);
            }

            var tmp = Path.Combine(Path.GetDirectoryName(path) , Path.GetFileName(path) + ".tmp");

            if (File.Exists(tmp))
            {
                File.Delete(tmp);
            }

            File.Move(path, tmp);

            using (var bmp = new Bitmap(tmp))
            {
                var resizeWidth = (int)(bmp.Width * _Rate);

                var resizeHeight = (int)(bmp.Height * _Rate);

                using (var resizeBmp = new Bitmap(resizeWidth, resizeHeight))
                using (var g = Graphics.FromImage(resizeBmp))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bmp, 0, 0, resizeWidth, resizeHeight);

                    resizeBmp.Save(path, ImageFormat.Png);
                }
            }

            File.Delete(tmp);

            return path;
        }
        */

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

        public void Move()
        {
            var move = Moves.First();
            var destPosX = move.DestPosX;
            var destPosY = move.DestPosY;
            if(move is Resign)
            {
                return;
            }
            if (move.IsDrop)
            {
                var piece = GetHands().First(x => x.GetType() == move.Piece.GetType());
                this[destPosX, destPosY] = piece;
                GetHands().Remove(piece);
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
                    piece.Reverse();
                    piece.Promoted = false;
                    GetHands().Add(piece);
                }

                this[destPosX, destPosY] = this[move.SrcPosX.Value, move.SrcPosY.Value];

                if (move.IsPromote)
                {
                    // 成
                    this[destPosX, destPosY].Promote();
                }

                this[move.SrcPosX.Value, move.SrcPosY.Value] = null;
            }

            Next();
        }

        public void MakeAnimation(Options options)
        {
            var images = new List<string>();

            //var record = KifParserFactory.Create(options).ReadFile();

            Console.WriteLine("棋譜画像出力中");
            var sw = new Stopwatch();
            sw.Start();
            /*
            for (var i = 0; i < record.Moves.Count; i++)
            {
                Move(record.Moves[i]);
                //images.Add(Paint(i, record));
            }*/
            Console.WriteLine($"棋譜画像出力完了:{sw.Elapsed}");
            Console.WriteLine("棋譜動画出力中");
            sw.Restart();

            var outFile = options.OutputFile ?? Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(options.InputFile)}.mp4");

            var argument = $"-r 1 -i {Path.Combine(Path.GetTempPath(), "result%d.png")} -vcodec libx264 -pix_fmt yuv420p -r 30 -y {outFile}";

            var psInfo = new ProcessStartInfo()
            {
                FileName = @"ffmpeg",    // 実行するファイル 
                Arguments = argument,    // コマンドパラメータ（引数）
                CreateNoWindow = true,    // コンソール・ウィンドウを開かない
                UseShellExecute = false,  // シェル機能を使用しない
            };

            var p = Process.Start(psInfo);
            p.WaitForExit();
            Console.WriteLine($"ffmpeg実行結果:{p.ExitCode}");

            foreach (var png in images)
            {
                File.Delete(png);
            }
            Console.WriteLine($"棋譜動画出力完了:{sw.Elapsed}");
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

        /// <summary>
        /// 複数の画像をGIFアニメーションとして保存する
        /// </summary>
        /// <param name="savePath">保存先のファイルのパス</param>
        /// <param name="imageFiles">GIFに追加する画像ファイルのパス</param>
        public void CreateAnimatedGif(string savePath, IEnumerable<string> imageFiles)
        {
            //GifBitmapEncoderを作成する
            var encoder = new GifBitmapEncoder();

            foreach (var f in imageFiles)
            {
                //画像ファイルからBitmapFrameを作成する
                var bmpFrame =
                    BitmapFrame.Create(new Uri(f, UriKind.RelativeOrAbsolute));
                //フレームに追加する
                encoder.Frames.Add(bmpFrame);
            }

            //書き込むファイルを開く
            using (var outputFileStrm = new FileStream(savePath,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //保存する
                encoder.Save(outputFileStrm);
            }
        }
    }
}
