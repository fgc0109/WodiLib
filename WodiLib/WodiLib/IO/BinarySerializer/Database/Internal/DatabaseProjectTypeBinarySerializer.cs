// ========================================
// Project Name : WodiLib
// File Name    : DatabaseProjectTypeBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="ReadOnlyDatabaseProjectType"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseProjectTypeBinarySerializer
    {
        /// <summary>
        ///     <see cref="ReadOnlyDatabaseProjectType"/> 列挙をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての <see cref="ReadOnlyDatabaseProjectType"/> を変換したバイナリ配列</returns>
        public static byte[] Serialize(this IEnumerable<ReadOnlyDatabaseProjectType> src)
            => src.SelectMany(ToBinary).ToArray();

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseProjectType"/> をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns><see cref="ReadOnlyDatabaseProjectType"/> を変換したバイナリ配列</returns>
        public static byte[] ToBinary(this ReadOnlyDatabaseProjectType src)
        {
            var result = new List<byte>();

            // タイプ名
            result.AddRange(((string)src.TypeName).ToWoditorStringBytes());

            // 項目数
            result.AddRange(src.FieldCount.ToWoditorIntBytes());

            // 項目名
            result.AddRange(src.FieldDefinitionList.SerializeFieldNames());

            // データ数
            result.AddRange(src.DataCount.ToWoditorIntBytes());

            // データ名
            result.AddRange(src.DataNameList.SelectMany(name => ((string)name).ToWoditorStringBytes()));

            // メモ
            result.AddRange(((string)src.Memo).ToWoditorStringBytes());

            // 特殊指定
            result.AddRange(src.FieldDefinitionList.SerializeSpecialSettingDescription());

            return result.ToArray();
        }
    }
}
