// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValueBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="DatabaseFieldValue"/> をバイナリ配列に変換するための処理定義クラス
    /// </summary>
    internal static class DatabaseFieldValueBinarySerializer
    {
        /// <summary>
        ///     バイナリ変換する。
        /// </summary>
        /// <param name="src">変換対象</param>
        /// <returns>変換したバイナリデータ</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        public static byte[] Serialize(this DatabaseFieldValue src)
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));

            if (src.Type == DatabaseFieldType.Int) return src.IntValue.RawValue.ToWoditorIntBytes();
            if (src.Type == DatabaseFieldType.String) return src.StringValue.RawValue.ToWoditorStringBytes();
            throw new ArgumentException(nameof(src)); // 通常発生しない
        }
    }
}
