// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldDefinitionBinarySerializer.cs
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
    ///     <see cref="ReadOnlyDatabaseFieldDefinition"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseFieldDefinitionBinarySerializer
    {
        /// <summary>
        ///     <see cref="ReadOnlyDatabaseFieldDefinition"/> 列挙中の <see cref="ReadOnlyDatabaseFieldDefinition.FieldName"/>
        ///     をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての <see cref="ReadOnlyDatabaseFieldDefinition.FieldName"/> を変換したバイナリ配列</returns>
        public static byte[] SerializeFieldNames(this IEnumerable<ReadOnlyDatabaseFieldDefinition> src)
        {
            return src.SelectMany(row => ((string)row.FieldName).ToWoditorStringBytes()).ToArray();
        }

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseFieldDefinition"/> 列挙中の <see cref="ReadOnlyDatabaseFieldDefinition.FieldType"/>
        ///     とその順列をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての <see cref="ReadOnlyDatabaseFieldDefinition.FieldType"/> とその順列を変換したバイナリ配列</returns>
        public static byte[] SerializeFieldTypesAndOrder(
            this IEnumerable<ReadOnlyDatabaseFieldDefinition> src
        )
            => src.Select(row => row.FieldType).Serialize();

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseFieldDefinition"/> 列挙中の特殊指定情報をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての特殊指定情報を変換したバイナリ配列</returns>
        public static byte[] SerializeSpecialSettingDescription(
            this IEnumerable<ReadOnlyDatabaseFieldDefinition> src
        )
        {
            var needFieldLength = DatabaseConst.MaxFieldLength;

            var result = new List<byte>();
            var definitionArray = src.ToArray();
            var fieldLength = definitionArray.Length;

            // 特殊指定数
            result.AddRange(needFieldLength.ToWoditorIntBytes());

            // 特殊指定
            var settingTypeList = definitionArray.Select(x => x.SpecialSettingDefinition.SettingType);
            result.AddRange(settingTypeList.Select(valueType => valueType.Code));
            // 足りない分を「特殊な指定方法を使用しない」で埋める
            for (var i = fieldLength; i < needFieldLength; i++)
            {
                result.Add(DatabaseFieldSpecialSettingType.Normal.Code);
            }

            // 項目メモ数
            result.AddRange(fieldLength.ToWoditorIntBytes());

            // 項目メモ
            result.AddRange(definitionArray.SelectMany(x => ((string)x.FieldMemo).ToWoditorStringBytes()));

            // ---------- 特殊指定文字列パラメータ、特殊指定内パラメータ、初期値

            var specialCaseDescriptions = new List<IReadOnlyList<DatabaseValueCaseDescription>>();
            var specialCaseNumbers = new List<IReadOnlyList<DatabaseValueCaseNumber>>();
            var initValues = new List<DatabaseFieldValue>();

            var useDataList = definitionArray.Select(x => x.SpecialSettingDefinition);
            foreach (var data in useDataList)
            {
                var cases = data.GetSpecialCases()
                    .Aggregate(
                        (new List<DatabaseValueCaseDescription>(), new List<DatabaseValueCaseNumber>()),
                        (acc, @case) =>
                        {
                            acc.Item1.Add(@case.Description);
                            acc.Item2.Add(@case.CaseNumber);
                            return acc;
                        }
                    );

                specialCaseDescriptions.Add(cases.Item1);
                specialCaseNumbers.Add(cases.Item2);
                initValues.Add(data.InitValue);
            }

            // 特殊指定文字列パラメータ数
            result.AddRange(specialCaseDescriptions.Count.ToWoditorIntBytes());

            // 特殊指定文字列パラメータ
            specialCaseDescriptions.ForEach(x =>
                {
                    // 文字列パラメータ数
                    result.AddRange(x.Count.ToBytes(Endian.Woditor));
                    // 文字列パラメータ
                    x.ForEach(y =>
                        result.AddRange(((string)y).ToWoditorStringBytes())
                    );
                }
            );

            // 特殊指定数値パラメータ数
            result.AddRange(specialCaseNumbers.Count.ToWoditorIntBytes());

            // 特殊指定数値パラメータ
            specialCaseNumbers.ForEach(x =>
                {
                    // 数値パラメータ数
                    result.AddRange(x.Count.ToWoditorIntBytes());
                    // 数値パラメータ
                    x.ForEach(y =>
                        result.AddRange(((int)y).ToWoditorIntBytes())
                    );
                }
            );

            // 初期値数
            result.AddRange(initValues.Count.ToWoditorIntBytes());

            // 初期値
            result.AddRange(initValues.Serialize());

            return result.ToArray();
        }
    }
}
