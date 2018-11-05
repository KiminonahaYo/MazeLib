using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    ///     迷路作成静的クラス
    /// </summary>
    public static class MazeCreater
    {
        /// <summary>
        ///     迷路を作成する。
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <returns>迷路オブジェクト</returns>
        public static MazeObject CreateMaze(int sizeX, int sizeY)
        {
            return MazeCreater.CreateMaze(sizeX, sizeY, -1);
        }

        /// <summary>
        ///     迷路を作成する。
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <param name="seed">シード値</param>
        /// <returns>迷路オブジェクト</returns>
        public static MazeObject CreateMaze(int sizeX, int sizeY, int seed)
        {
            return MazeCreater.CreateMaze(sizeX, sizeY, 2, 2, sizeX - 3, sizeY - 3, seed);
        }

        /// <summary>
        ///     迷路を作成する。
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <param name="startX">スタートX座標</param>
        /// <param name="startY">スタートY座標</param>
        /// <param name="goalX">ゴールX座標</param>
        /// <param name="goalY">ゴールY座標</param>
        /// <returns>迷路オブジェクト</returns>
        public static MazeObject CreateMaze(int sizeX, int sizeY, int startX, int startY, int goalX, int goalY)
        {
            return MazeCreater.CreateMaze(sizeX, sizeY, startX, startY, goalX, goalY, -1);
        }

        /// <summary>
        ///     迷路を作成する。
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <param name="startX">スタートX座標</param>
        /// <param name="startY">スタートY座標</param>
        /// <param name="goalX">ゴールX座標</param>
        /// <param name="goalY">ゴールY座標</param>
        /// <param name="seed">シード値</param>
        /// <returns>迷路オブジェクト</returns>
        public static MazeObject CreateMaze(int sizeX, int sizeY, int startX, int startY, int goalX, int goalY, int seed)
        {
            MazeObject ret = new MazeObject(sizeX, sizeY, startX, startY, goalX, goalY, seed);

            ret.Make();

            return ret;
        }

        /// <summary>
        ///     迷路の設定が適切かどうか確かめます。
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <param name="strErrorMessage">エラーメッセージ</param>
        /// <param name="errorType">エラーの種類</param>
        /// <returns>true 適切である, false 適切でない</returns>
        public static bool CheckMazeSetting(int sizeX, int sizeY, out string strErrorMessage, out MazeErrorType? errorType)
        {
            return MazeCreater.CheckMazeSetting(sizeX, sizeY, 2, 2, sizeX - 3, sizeY - 3, out strErrorMessage, out errorType);
        }

        /// <summary>
        ///     迷路の設定が適切かどうか確かめます。
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        /// <param name="startX">スタートX座標</param>
        /// <param name="startY">スタートY座標</param>
        /// <param name="goalX">ゴールX座標</param>
        /// <param name="goalY">ゴールY座標</param>
        /// <param name="strErrorMessage">エラーメッセージ</param>
        /// <param name="errorType">エラーの種類</param>
        /// <returns>true 適切である, false 適切でない</returns>
        public static bool CheckMazeSetting(int sizeX, int sizeY, int startX, int startY, int goalX, int goalY, out string strErrorMessage, out MazeErrorType? errorType)
        {
            strErrorMessage = "";
            errorType = null;

            //座標に関する判定（長すぎるのでいくつかに分けている）
            //スタート座標が偶数かどうか
            bool isgood_start_gusu =
                startX % 2 == 0 &&
                startY % 2 == 0;
            //スタート座標が適切か（ここでは最小のみを判定）
            bool isgood_start_pos_min =
                startX >= 2 &&
                startY >= 2;
            //スタート座標が適切か（ここでは最大のみを判定）
            bool isgood_start_pos_max =
                startX <= sizeX - 3 &&
                startY <= sizeY - 3;

            //ゴール座標が偶数かどうか
            bool isgood_goal_gusu =
                goalX % 2 == 0 &&
                goalY % 2 == 0;
            //ゴール座標が適切か（ここでは最小のみを判定）
            bool isgood_goal_pos_min =
                goalX >= 2 &&
                goalY >= 2;
            //ゴール座標が適切か（ここでは最大のみを判定）
            bool isgood_goal_pos_max =
                goalX <= sizeX - 3 &&
                goalY <= sizeY - 3;

            //スタート座標とゴール座標が異なるかどうか
            bool isdifferent_between_startandgoal =
                startX != goalX ||
                startY != goalY;

            //座標が適切かどうか（↑の判定をまとめている。）
            bool isgood_pos =
                isgood_start_gusu &&
                isgood_start_pos_min &&
                isgood_start_pos_max &&
                isgood_goal_gusu &&
                isgood_goal_pos_min &&
                isgood_goal_pos_max &&
                isdifferent_between_startandgoal;

            //サイズが適切かどうか
            //迷路のサイズが奇数かどうか
            bool isgood_size_kisu =
                sizeX % 2 == 1 &&
                sizeY % 2 == 1;
            //迷路のサイズが適切かどうか
            bool isgood_size =
                sizeX >= 5 &&
                sizeY >= 5;

            //判定をまとめ、設定された値が適切かどうかを返却する。
            if (isgood_size_kisu && isgood_size && isgood_pos)
            {
                //設定された値は適切
                return true;
            }
            else
            {
                //設定された値は不適切
                if (!isgood_size_kisu) { errorType = MazeErrorType.IllegalSize; strErrorMessage = string.Format("迷路のサイズのXサイズ、Yサイズのどちらかが奇数ではありません。\n\nXサイズ：{0}\nYサイズ：{1}", sizeX, sizeY); }
                else if (!isgood_size) { errorType = MazeErrorType.IllegalSize; strErrorMessage = string.Format("迷路のサイズが小さすぎます。\n\nXサイズ：{0}\nYサイズ：{1}", sizeX, sizeY); }
                else if (!isgood_start_gusu) { errorType = MazeErrorType.IllegalStartPosition; strErrorMessage = string.Format("スタート座標のX座標・Y座標のどちらかが偶数ではありません。\n\nX座標：{0}, Y座標：{1}", startX, startY); }
                else if (!isgood_start_pos_min) { errorType = MazeErrorType.IllegalStartPosition; strErrorMessage = string.Format("スタート座標が小さすぎます。 \n\nX座標：{0}, Y座標：{1}", startX, startY); }
                else if (!isgood_start_pos_max) { errorType = MazeErrorType.IllegalStartPosition; strErrorMessage = string.Format("スタート座標が大きすぎます。 \n\nX座標：{0}, Y座標：{1}", startX, startY); }
                else if (!isgood_goal_gusu) { errorType = MazeErrorType.IllegalGoalPosition; strErrorMessage = string.Format("ゴール座標のX座標・Y座標のどちらかが偶数ではありません。\n\nX座標：{0}, Y座標：{1}", goalX, goalY); }
                else if (!isgood_goal_pos_min) { errorType = MazeErrorType.IllegalGoalPosition; strErrorMessage = string.Format("ゴール座標が小さすぎます。 \n\nX座標：{0}, Y座標：{1}", goalX, goalY); }
                else if (!isgood_goal_pos_max) { errorType = MazeErrorType.IllegalGoalPosition; strErrorMessage = string.Format("ゴール座標が大きすぎます。 \n\nX座標：{0}, Y座標：{1}", goalX, goalY); }
                else if (!isdifferent_between_startandgoal) { errorType = MazeErrorType.SameStartAndGoalPosition; strErrorMessage = string.Format("スタート座標とゴール座標が同一です。\n\nスタート座標：({0}, {1})\nゴール座標：({2}, {3})", startX, startY, goalX, goalY); }

                return false;
            }
        }
    }
}
