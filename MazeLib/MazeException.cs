using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    ///     エラータイプ
    /// </summary>
    public enum MazeErrorType
    {
        /// <summary>スタート座標が不適切</summary>
        IllegalStartPosition,
        /// <summary>ゴール座標が不適切</summary>
        IllegalGoalPosition,
        /// <summary>サイズが不適切</summary>
        IllegalSize,
        /// <summary>スタート座標とゴール座標が不適切</summary>
        SameStartAndGoalPosition,
        /// <summary>迷路が作成されていない</summary>
        NotMadeMaze,
        /// <summary>迷路が解かれていない</summary>
        NotSolveMaze,
    }

    /// <summary>
    ///     迷路関連例外クラス
    /// </summary>
    public class MazeException : Exception
    {
        /// <summary>
        ///     迷路関連エラーの種類
        /// </summary>
        public MazeErrorType ErrorType { get; private set; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="errorType">エラー種類</param>
        public MazeException(string message, MazeErrorType errorType) : base(message)
        {
            this.ErrorType = errorType;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="innerException">内部発生例外</param>
        /// <param name="errorType">エラー種類</param>
        public MazeException(string message, Exception innerException, MazeErrorType errorType) : base(message, innerException)
        {
            this.ErrorType = errorType;
        }
    }
}
