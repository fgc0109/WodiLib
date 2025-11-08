// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValuesBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="DatabaseFieldValue"/> 列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseFieldValuesBinarySerializer
    {
        /// <summary>
        ///     <see cref="DatabaseFieldValue"/> 列挙をバイナリ配列に変換する。
        /// </summary>
        /// <remarks>
        ///     項目値種別は意識せず、列挙された順に変換して結合する。
        /// </remarks>
        /// <param name="src">処理対象</param>
        /// <returns>すべての項目値を変換したバイナリ配列</returns>
        public static byte[] Serialize(this IEnumerable<DatabaseFieldValue> src)
            => src.SelectMany(value => value.Serialize()).ToArray();
    }
}
