
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    ///     迷路関連配列の基底クラス
    /// </summary>
    /// <typeparam name="T">列挙体</typeparam>
    public abstract class MazeArrayBase<T> where T : Enum
    {
        /// <summary>
        ///     内部データ配列
        /// </summary>
        private T[,] cells = null;


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="sizeX">Xサイズ</param>
        /// <param name="sizeY">Yサイズ</param>
        public MazeArrayBase(int sizeX, int sizeY)
        {
            this.cells = new T[sizeX, sizeY];
        }

        /// <summary>
        ///     迷路のセルの種類を取得します。
        /// </summary>
        /// <param name="posX">X座標（0から開始）</param>
        /// <param name="posY">Y座標（0から開始）</param>
        /// <returns>セルの種類</returns>
        public T this[int posX, int posY]
        {
            get
            {
                this.CheckPosition(posX, posY);

                return this.cells[posX, posY];
            }
            internal set
            {
                this.CheckPosition(posX, posY);

                this.cells[posX, posY] = value;
            }
        }

        /// <summary>
        ///     セル座標を精査します。
        /// </summary>
        /// <param name="sizeX">X座標（0から開始）</param>
        /// <param name="sizeY">Y座標（0から開始）</param>
        private void CheckPosition(int sizeX, int sizeY)
        {
            if (sizeX < 0 || sizeY < 0) throw new IndexOutOfRangeException();
            if (sizeX >= this.cells.GetLength(0) || sizeY >= this.cells.GetLength(1)) throw new IndexOutOfRangeException();
        }
    }
}
