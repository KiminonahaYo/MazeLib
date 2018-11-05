using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    ///     迷路データオブジェクト
    /// </summary>
    public class MazeObject
    {
        private MazeFieldArray fieldArray;
        /// <summary>迷路フィールドを取得または設定します。</summary>
        public MazeFieldArray FieldArray
        {
            get
            {
                if (!this.Made) throw new MazeException("迷路が作成されていません。", MazeErrorType.NotMadeMaze);

                return this.fieldArray;
            }
            private set
            {
                this.fieldArray = value;
            }
        }

        /// <summary>迷路の答え</summary>
        public MazeSolveArray solveArray;
        /// <summary>迷路の答えを取得または設定します。</summary>
        public MazeSolveArray SolveArray
        {
            get
            {
                if (!this.Solved) throw new MazeException("迷路が解かれていません。", MazeErrorType.NotSolveMaze);

                return this.solveArray;
            }
            private set
            {
                this.solveArray = value;
            }
        }

        /// <summary>
        ///     迷路が作成されたかどうか
        /// </summary>
        public bool Made
        {
            get
            {
                if (this.fieldArray == null) return false;

                return true;
            }
        }

        /// <summary>迷路が解かれたかどうか</summary>
        public bool Solved
        {
            get
            {
                if (this.solveArray == null) return false;

                return true;
            }
        }

        /// <summary>スタート座標X</summary>
        public int StartX { get; private set; }
        /// <summary>スタート座標Y</summary>
        public int StartY { get; private set; }
        /// <summary>ゴール座標X</summary>
        public int GoalX { get; private set; }
        /// <summary>ゴール座標Y</summary>
        public int GoalY { get; private set; }
        /// <summary>シード値</summary>
        public int Seed { get; private set; }
        /// <summary>Xサイズ</summary>
        public int SizeX { get; private set; }
        /// <summary>Yサイズ</summary>
        public int SizeY { get; private set; }

        /// <summary>
        ///     ランダムオブジェクト
        /// </summary>
        public Random random = null;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <param name="startX">スタート座標X</param>
        /// <param name="startY">スタート座標Y</param>
        /// <param name="goalX">ゴール座標X</param>
        /// <param name="goalY">ゴール座標Y</param>
        /// <param name="seed">シード値</param>
        internal MazeObject(int sizeX, int sizeY, int startX, int startY, int goalX, int goalY, int seed)
        {
            this.FieldArray = null;
            this.SolveArray = null;
            this.SizeX = sizeX;
            this.SizeY = sizeY;
            this.StartX = startX;
            this.StartY = startY;
            this.GoalX = goalX;
            this.GoalY = goalY;
            this.Seed = seed;
        }

        /// <summary>
        ///     迷路を作成する。
        /// </summary>
        internal void Make()
        {
            if (!MazeCreater.CheckMazeSetting(this.SizeX, this.SizeY, this.StartX, this.StartY, this.GoalX, this.GoalY, out string strErrorMessage, out MazeErrorType? errorType)) throw new MazeException(strErrorMessage, errorType.Value);

            this.FieldArray = new MazeFieldArray(this.SizeX, this.SizeY);

            //シード値がマイナス値の場合はシード値をランダムに作成する。
            Random randomSeed = new Random();
            if (this.Seed < 0) this.Seed = randomSeed.Next();

            this.random = new Random(this.Seed);

            this.MakeInit();
            this.MakeMain();
            this.MakeFinalize();
        }

        /// <summary>
        ///     迷路を解く。
        /// </summary>
        /// <returns>true 成功, false 失敗</returns>
        public bool Solve()
        {
            if (this.Solved) return true;

            this.SolveArray = new MazeSolveArray(this.SizeX, this.SizeY);
            MazeSolveArrayInner solveArrayInner = new MazeSolveArrayInner(this.SizeX, this.SizeY);

            this.SolveInit(solveArrayInner);
            if (!this.SolveMain(solveArrayInner)) { this.SolveArray = null; return false; }
            this.SolveFinalize(solveArrayInner);

            return true;
        }

        #region 迷路を作成する
        /// <summary>
        ///     迷路作成の事前準備を行う。
        /// </summary>
        private void MakeInit()
        {
            //いちばん外側を道、それ以外（中）を壁にする。
            for (int x = 0; x < this.SizeX; x++)
            {
                for (int y = 0; y < this.SizeY; y++)
                {
                    if (x == 0 ||
                        y == 0 ||
                        x == this.SizeX - 1 ||
                        y == this.SizeY - 1)
                    {
                        this.FieldArray[x, y] = MazeFieldArray.CellType.Road;
                    }
                    else
                    {
                        this.FieldArray[x, y] = MazeFieldArray.CellType.Wall;
                    }
                }
            }

            //座標(2, 2)を道にする（ここから穴を広げていく）
            this.FieldArray[2, 2] = MazeFieldArray.CellType.Road;
        }

        /// <summary>
        ///     迷路を作成する。
        /// </summary>
        private void MakeMain()
        {
            int mazeType = this.random.Next() % 10;


            if (mazeType >= 0 && mazeType < 6)
            {
                //タイプ１
                //正解ルートが真ん中を突き抜ける傾向がある。
                //ゴール
                //↑←
                //　↑←
                //　　↑←
                //　　　↑←
                //　　　　↑←
                //　　　　　↑←スタート　
                
                //迷路を作成
                for (int i = 2; i <= Math.Max(this.SizeX, this.SizeY) - 3; i += 2)
                {
                    for (int xi = 2; xi <= this.SizeX - 3 && xi < i + 2; xi += 2)
                    {
                        for (int yi = 2; yi <= this.SizeY - 3 && yi < xi + 2; yi += 2)
                        {
                            this.MakeSub(xi, yi);
                        }
                    }
                    for (int yi = 2; yi <= this.SizeY - 3 && yi < i + 2; yi += 2)
                    {
                        for (int xi = 2; xi <= this.SizeX - 3 && xi < yi + 2; xi += 2)
                        {
                            this.MakeSub(xi, yi);
                        }
                    }
                }
                //迷路が作られていない場所を埋める
                for (int xi = 2; xi <= this.SizeX - 3; xi += 2)
                {
                    for (int yi = 2; yi <= this.SizeY - 3; yi += 2)
                    {
                        this.MakeSub(xi, yi);
                    }
                }
            }
            else if (mazeType >= 6 && mazeType < 8)
            {
                //タイプ２
                //正解ルートがスタート位置から見て左に寄る傾向がある
                //ゴール
                //↑
                //↑
                //↑
                //↑
                //↑
                //↑←←←←←スタート
                
                //迷路を作成
                for (int xi = 2; xi <= this.SizeX - 3; xi += 2)
                {
                    for (int yi = 2; yi <= this.SizeY - 3; yi += 2)
                    {
                        this.MakeSub(xi, yi);
                    }
                }
                //迷路が作られていない場所を埋める
                for (int xi = 2; xi <= this.SizeX - 3; xi += 2)
                {
                    for (int yi = 2; yi <= this.SizeY - 3; yi += 2)
                    {
                        this.MakeSub(xi, yi);
                    }
                }
            }
            else
            {
                //タイプ３
                //正解ルートがスタート位置から見て正面に寄る傾向がある
                //ゴール←←←←
                //　　　　　　↑
                //　　　　　　↑
                //　　　　　　↑
                //　　　　　　↑
                //　　　　　　↑
                //　　　スタート
                
                //迷路を作成
                for (int yi = 2; yi <= this.SizeY - 3; yi += 2)
                {
                    for (int xi = 2; xi <= this.SizeX - 3; xi += 2)
                    {
                        this.MakeSub(xi, yi);
                    }
                }
                //迷路が作られていない場所を埋める
                for (int yi = 2; yi <= this.SizeY - 3; yi += 2)
                {
                    for (int xi = 2; xi <= this.SizeX - 3; xi += 2)
                    {
                        this.MakeSub(xi, yi);
                    }
                }
            }
        }

        /// <summary>
        ///     迷路を作成する。（サブメソッド）
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        private void MakeSub(int posX, int posY)
        {
            //スタート位置が壁だったらサブメソッドを終了する。
            if (this.FieldArray[posX, posY] == MazeFieldArray.CellType.Wall) return;

            (int, int) rot;
            int rotsign;
            int rotStart;
            int rotNo;

            //原則無限ループとして、処理が終わったらreturnで戻る。
            while (true)
            {
                rotStart = this.random.Next() % 4;
                rotsign = this.random.NextDouble() < 0.5 ? 1 : -1;

                for (int iLoop1 = 0; iLoop1 < 4; iLoop1++)
                {
                    //方向番号を求め、向き（ベクトル）を取得（rot.Item1 : X, rot.Item2 : Y）
                    rotNo = ((rotStart + iLoop1) * rotsign + 4) % 4;
                    rot = this.GetRotation(rotNo);

                    //向いた先が壁だったら
                    if (this.FieldArray[posX + rot.Item1 * 2, posY + rot.Item2 * 2] == MazeFieldArray.CellType.Wall)
                    {
                        //そこを道にする。
                        this.FieldArray[posX + rot.Item1, posY + rot.Item2] = MazeFieldArray.CellType.Road;
                        this.FieldArray[posX + rot.Item1 * 2, posY + rot.Item2 * 2] = MazeFieldArray.CellType.Road;

                        posX += rot.Item1 * 2;
                        posY += rot.Item2 * 2;

                        //向いた先が壁だったらもうループする必要はないので、forループをぬける。
                        break;
                    }

                    //どこを向いても道である場合、このサブメソッドを終了する。
                    if (iLoop1 == 3) return;
                }
            }
        }

        /// <summary>
        ///     迷路作成の後処理をする。
        /// </summary>
        private void MakeFinalize()
        {
            //スタートとゴールを配置
            this.FieldArray[this.StartX, this.StartY] = MazeFieldArray.CellType.Start;
            this.FieldArray[this.GoalX, this.GoalY] = MazeFieldArray.CellType.Goal;
        }
        #endregion

        #region 迷路を解く
        /// <summary>
        ///     迷路を解くための準備を行う。
        /// </summary>
        /// <param name="solveArrayInner">迷路の答えを示す内部データ</param>
        private void SolveInit(MazeSolveArrayInner solveArrayInner)
        {
            //答えのデータを初期化する。
            for (int x = 0; x < this.SizeX; x++)
            {
                for (int y = 0; y < this.SizeY; y++)
                {
                    solveArrayInner[x, y] = MazeSolveArrayInner.CellType.Wrong;
                }
            }
        }

        /// <summary>
        ///     迷路を解く。
        /// </summary>
        /// <param name="solveArrayInner">迷路の答えを示す内部データ</param>
        /// <returns></returns>
        private bool SolveMain(MazeSolveArrayInner solveArrayInner)
        {
            (int, int) curPos = (this.StartX, this.StartY); //迷路の現在位置を初期化（スタート位置にする）
            (int, int) rot;     

            //スタート位置を答えの道にする。
            solveArrayInner[curPos.Item1, curPos.Item2] = MazeSolveArrayInner.CellType.Right;

            //迷路を解く。
            while (true)
            {
                //最大4回向きを変える。
                for (int rotNo = 0; rotNo < 4; rotNo++)
                {
                    //向き番号からベクトル座標を得る
                    rot = this.GetRotation(rotNo);

                    //先が道だったらとりあえず進む
                    if (curPos.Item1 + rot.Item1 >= 0 &&
                        curPos.Item1 + rot.Item1 < this.SizeX &&
                        curPos.Item2 + rot.Item2 >= 0 &&
                        curPos.Item2 + rot.Item2 < this.SizeY &&
                        (
                            this.fieldArray[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeFieldArray.CellType.Goal ||
                            this.fieldArray[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeFieldArray.CellType.Start ||
                            this.fieldArray[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeFieldArray.CellType.Road
                        ) &&
                        solveArrayInner[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeSolveArrayInner.CellType.Wrong
                        )
                    {
                        //★答えの道にする
                        solveArrayInner[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] = MazeSolveArrayInner.CellType.Right;
                        curPos.Item1 += rot.Item1;
                        curPos.Item2 += rot.Item2;

                        break;
                    }

                    //★行き止まりだったら
                    if (rotNo == 3)
                    {
                        //最大4回向きを変える。
                        for (int rotNo2 = 0; rotNo2 < 4; rotNo2++)
                        {
                            rot = this.GetRotation(rotNo2);

                            //今まで来た道を戻る。
                            if (curPos.Item1 + rot.Item1 >= 0 &&
                                curPos.Item1 + rot.Item1 < this.SizeX &&
                                curPos.Item2 + rot.Item2 >= 0 &&
                                curPos.Item2 + rot.Item2 < this.SizeY &&
                                (
                                    this.fieldArray[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeFieldArray.CellType.Goal ||
                                    this.fieldArray[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeFieldArray.CellType.Start ||
                                    this.fieldArray[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeFieldArray.CellType.Road
                                ) &&
                                solveArrayInner[curPos.Item1 + rot.Item1, curPos.Item2 + rot.Item2] == MazeSolveArrayInner.CellType.Right
                                )
                            {
                                //ダミーのフラグを立てる。（その道は２度と進まないようになる）
                                solveArrayInner[curPos.Item1, curPos.Item2] = MazeSolveArrayInner.CellType.Temp;
                                curPos.Item1 += rot.Item1;
                                curPos.Item2 += rot.Item2;
                                break;
                            }

                            if (rotNo2 == 3)
                            {
                                //★★解けない迷路であった…
                                return false;
                            }
                        }
                    }
                }

                if (curPos.Item1 == this.GoalX && curPos.Item2 == this.GoalY) break;
            }

            //★★迷路を解くことができた！
            return true;
        }

        /// <summary>
        ///     迷路を解くための後処理をする。
        /// </summary>
        /// <param name="solveArrayInner">迷路の答えを示す内部データ</param>
        private void SolveFinalize(MazeSolveArrayInner solveArrayInner)
        {
            for (int x = 0; x < this.SizeX; x++)
            {
                for (int y = 0; y < this.SizeY; y++)
                {
                    switch (solveArrayInner[x, y])
                    {
                        case MazeSolveArrayInner.CellType.Right:
                        {
                            //正解ルート
                            this.SolveArray[x, y] = MazeSolveArray.CellType.Right;
                            break;
                        }
                        case MazeSolveArrayInner.CellType.Wrong:
                        case MazeSolveArrayInner.CellType.Temp:
                        {
                            //間違ったルートまたは壁
                            this.SolveArray[x, y] = MazeSolveArray.CellType.Wrong;
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region 共通処理
        /// <summary>
        ///     向きの番号からX座標, Y座標を取得する。
        /// </summary>
        /// <param name="rotNo">0 上, 1 下, 2 左, 3 右</param>
        /// <returns>X, Y</returns>
        private (int, int) GetRotation(int rotNo)
        {
            switch (rotNo)
            {
                case 0: return (0, -1);
                case 1: return (0, 1);
                case 2: return (-1, 0);
                case 3: return (1, 0);
                default: return (0, 0);
            }
        }
        #endregion
    }
}
