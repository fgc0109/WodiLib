// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="ReadOnlyDatabaseDataTable"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseDataTableBinarySerializer
    {
        /// <inheritdoc cref="SerializeValuesDividedType(ReadOnlyDatabaseDataTable)"/>
        public static byte[] SerializeValuesDividedType(this DatabaseDataTable src)
            => SerializeValuesDividedType((ReadOnlyDatabaseDataTable)src);

        /// <summary>
        ///     <see cref="DatabaseFieldValue"/> 列挙を値種別ごとにバイナリ配列に変換する。
        /// </summary>
        /// <remarks>
        ///     1行ごとに数値項目と文字列項目を個別にバイナリ変換し、最後に結合する。
        /// </remarks>
        /// <param name="src">処理対象</param>
        /// <returns>すべての項目値を変換したバイナリ配列</returns>
        public static byte[] SerializeValuesDividedType(this ReadOnlyDatabaseDataTable src)
            => src.SelectMany(row => row.SerializeValuesDividedType()).ToArray();
    }
}
