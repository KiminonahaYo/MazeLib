using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace MazeLibTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MazeObject mazeObject;

            int iXSize;
            int iYSize;
            int iSeed;
            string strErrorMessaege;
            MazeErrorType? errorType;

            Console.WriteLine("------------------------------------");
            Console.WriteLine("迷路作成テストプログラム");
            Console.WriteLine("------------------------------------");
            while (true)
            {
                iXSize = Program.InputNumber("Xサイズを入力", "Xサイズ");
                iYSize = Program.InputNumber("Yサイズを入力", "Yサイズ");
                iSeed = Program.InputNumber("シード値を入力（指定しないときは-1にする）", "シード値");

                if (!MazeCreater.CheckMazeSetting(iXSize, iYSize, out strErrorMessaege, out errorType))
                {
                    Console.WriteLine(strErrorMessaege);
                    continue;
                }

                mazeObject = MazeCreater.CreateMaze(iXSize, iYSize, iSeed);
                Program.Show(mazeObject);
                if (Program.InputYesNo("迷路を自動で解きますか？"))
                {
                    if (mazeObject.Solve())
                    {
                        Program.Show(mazeObject);
                    }
                    else
                    {
                        Console.WriteLine("迷路が解けなかったようです…");
                    }
                }
                if (!Program.InputYesNo("続けますか？")) break;
            }
        }

        /// <summary>
        ///     整数値を入力します。
        /// </summary>
        /// <param name="strNavi">入力を促すメッセージ</param>
        /// <param name="strKind">入力値の種類</param>
        /// <returns>入力値</returns>
        static int InputNumber(string strNavi, string strKind)
        {
            string strValue;
            do
            {
                Console.Write("{0} > ", strNavi);
                strValue = Console.ReadLine();
            }
            while (!Program.CheckNumber(strKind, strValue));

            return int.Parse(strValue);
        }

        /// <summary>
        ///     入力値が整数かどうか確かめます。
        /// </summary>
        /// <param name="strKind">入力値の種類</param>
        /// <param name="strValue">値</param>
        /// <returns>true 整数である, false 整数でない</returns>
        static bool CheckNumber(string strKind, string strValue)
        {
            int iDummy;
            if (!int.TryParse(strValue, out iDummy))
            {
                Console.WriteLine("{0}は整数ではありません。", strKind);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     はい（y）かいいえ（n）を入力
        /// </summary>
        /// <param name="strNavi">入力を促すメッセージ</param>
        /// <param name="strErrorMessage">エラーメッセージ</param>
        /// <returns>true はい, false いいえ</returns>
        static bool InputYesNo(string strNavi)
        {
            string strInput;

            do
            {
                Console.Write("{0}（y/n） > ", strNavi);
                strInput = Console.ReadLine();
            }
            while (!Program.CheckYesNo(strInput));

            switch (strInput.ToUpper())
            {
                case "Y":
                {
                    return true;
                }
                case "N":
                {
                    return false;
                }
                default:
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     はい（y）かいいえ（n）の入力をチェック
        /// </summary>
        /// <param name="strInput">入力値</param>
        /// <returns>true 入力は適切, false 入力は不適切</returns>
        static bool CheckYesNo(string strInput)
        {
            switch (strInput.ToUpper())
            {
                case "Y":
                {
                    return true;
                }
                case "N":
                {
                    return true;
                }
                default:
                {
                    Console.WriteLine("はい（y）かいいえ（n）を入力してください！");
                    return false;
                }
            }
        }

        /// <summary>
        ///     迷路を表示する。
        /// </summary>
        /// <param name="mazeObject">迷路オブジェクト</param>
        static void Show(MazeObject mazeObject)
        {
            for (int y = 0; y < mazeObject.SizeY; y++)
            {
                for (int x = 0; x < mazeObject.SizeX; x++)
                {
                    if (mazeObject.Solved)
                    {
                        //迷路が解かれている
                        if (mazeObject.FieldArray[x, y] == MazeFieldArray.CellType.Wall)
                        {
                            //壁
                            Console.Write("■");
                        }
                        else if (mazeObject.FieldArray[x, y] == MazeFieldArray.CellType.Start)
                        {
                            //スタート
                            Console.Write("◇");
                        }
                        else if (mazeObject.FieldArray[x, y] == MazeFieldArray.CellType.Goal)
                        {
                            //ゴール
                            Console.Write("◇");
                        }
                        else
                        {
                            //道
                            if (mazeObject.SolveArray[x, y] == MazeSolveArray.CellType.Right)
                            {
                                //正しい道
                                Console.Write("〇");
                            }
                            else
                            {
                                //間違った道
                                Console.Write("　");
                            }
                        }
                    }
                    else
                    {
                        //迷路は解かれていない
                        if (mazeObject.FieldArray[x, y] == MazeFieldArray.CellType.Wall)
                        {
                            //壁
                            Console.Write("■");
                        }
                        else if (mazeObject.FieldArray[x, y] == MazeFieldArray.CellType.Start)
                        {
                            //スタート
                            Console.Write("◇");
                        }
                        else if (mazeObject.FieldArray[x, y] == MazeFieldArray.CellType.Goal)
                        {
                            //ゴール
                            Console.Write("◇");
                        }
                        else
                        {
                            //道
                            Console.Write("　");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
