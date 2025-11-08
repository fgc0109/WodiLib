// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeOrderBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="DatabaseFieldType"/> インスタンスをシリアル化および逆シリアル化する処理実装クラス
    /// </summary>
    internal static class DatabaseTypeOrderBinarySerializer
    {
        /// <summary>
        ///     <see cref="DatabaseFieldType"/> 順列をシリアル化する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns><see cref="DatabaseFieldType"/> 順列を変換したバイナリ配列</returns>
        public static byte[] Serialize(this IEnumerable<DatabaseFieldType> src)
        {
            var result = new List<byte>();

            // 要素
            var cntDict = new Dictionary<DatabaseFieldType, int>
            {
                { DatabaseFieldType.Int, 0 },
                { DatabaseFieldType.String, 0 },
            };

            foreach (var fieldType in src)
            {
                var addValue = fieldType.TypeOrderStart + cntDict[fieldType];
                result.AddRange(addValue.ToWoditorIntBytes());

                cntDict[fieldType] += 1;
            }

            return result.ToArray();
        }
    }
}
