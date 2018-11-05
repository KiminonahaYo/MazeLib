﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    ///     迷路の答え用データクラス
    /// </summary>
    public class MazeSolveArray : MazeArrayBase<MazeSolveArray.CellType>
    {
        /// <summary>
        ///     迷路フィールド種類
        /// </summary>
        public enum CellType
        {
            /// <summary>
            ///     正しい道
            /// </summary>
            Right,
            /// <summary>
            ///     間違っている道
            /// </summary>
            Wrong,
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        internal MazeSolveArray(int sizeX, int sizeY) : base(sizeX, sizeY)
        {
        }
    }
}
