// ========================================
// Project Name : WodiLib
// File Name    : DatabaseNamedDataRowBinarySerializer.cs
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
    ///     <see cref="ReadOnlyDatabaseNamedDataRow"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseNamedDataRowBinarySerializer
    {
        /// <inheritdoc cref="SerializeValuesDividedType(ReadOnlyDatabaseNamedDataRow)"/>
        public static byte[] SerializeValuesDividedType(this DatabaseNamedDataRow src)
            => SerializeValuesDividedType((ReadOnlyDatabaseNamedDataRow)src);

        /// <summary>
        ///     <see cref="DatabaseFieldValue"/> 列挙を値種別ごとにバイナリ配列に変換する。
        /// </summary>
        /// <remarks>
        ///     数値項目と文字列項目を個別にバイナリ変換し、最後に結合する。
        /// </remarks>
        /// <param name="src">処理対象</param>
        /// <returns>すべての項目値を変換したバイナリ配列</returns>
        public static byte[] SerializeValuesDividedType(this ReadOnlyDatabaseNamedDataRow src)
        {
            var result = new List<byte>();

            var groupedSrc = GroupByType(src);

            // データ名
            result.AddRange(src.DataName.RawValue.ToWoditorStringBytes());

            // 数値項目数
            result.AddRange(groupedSrc[DatabaseFieldType.Int].Count.ToWoditorIntBytes());

            // 数値項目
            result.AddRange(groupedSrc[DatabaseFieldType.Int].Serialize());

            // 文字列項目数
            result.AddRange(groupedSrc[DatabaseFieldType.String].Count.ToWoditorIntBytes());

            // 文字列項目
            result.AddRange(groupedSrc[DatabaseFieldType.String].Serialize());

            return result.ToArray();
        }


        /// <summary>
        ///     <see cref="DatabaseFieldValue"/> 列挙を項目値種別でグループ化する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>項目値種別ごとに値をグループ化したDictionaryインスタンス</returns>
        private static Dictionary<DatabaseFieldType, List<DatabaseFieldValue>> GroupByType(
            IEnumerable<DatabaseFieldValue> src
        )
        {
            var groupByTypeSeed = new Dictionary<DatabaseFieldType, List<DatabaseFieldValue>>
            {
                { DatabaseFieldType.Int, new List<DatabaseFieldValue>() },
                { DatabaseFieldType.String, new List<DatabaseFieldValue>() },
            };

            var groupByTypeResult = src.Aggregate(
                groupByTypeSeed,
                (dict, value) =>
                {
                    if (value.Type == DatabaseFieldType.Int)
                    {
                        dict[DatabaseFieldType.Int].Add(value);
                    }
                    else
                    {
                        dict[DatabaseFieldType.String].Add(value);
                    }

                    return dict;
                }
            );
            return groupByTypeResult;
        }
    }
}
