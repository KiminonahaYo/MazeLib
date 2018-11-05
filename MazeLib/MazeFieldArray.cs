using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    ///     迷路フィールド用データクラス
    /// </summary>
    public class MazeFieldArray : MazeArrayBase<MazeFieldArray.CellType>
    {
        /// <summary>
        ///     迷路フィールド種類
        /// </summary>
        public enum CellType
        {
            /// <summary>道</summary>
            Road,
            /// <summary>壁</summary>
            Wall,
            /// <summary>スタート</summary>
            Start,
            /// <summary>ゴール</summary>
            Goal
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        internal MazeFieldArray(int sizeX, int sizeY) : base(sizeX, sizeY)
        {
        }
    }
}
