// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldMetadataBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using Endian = WodiLib.Sys.Endian;
using IntExtension = WodiLib.Sys.IntExtension;
using LinqExtension = WodiLib.Sys.LinqExtension;
using StringExtension = WodiLib.Sys.StringExtension;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="ReadOnlyDatabaseFieldMetadata"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseFieldMetadataBinarySerializer
    {
        /// <inheritdoc cref="SerializeFieldNames(IEnumerable{ReadOnlyDatabaseFieldMetadata})"/>
        public static byte[] SerializeFieldNames(this IEnumerable<DatabaseFieldMetadata> src)
            => SerializeFieldNames(src.Select(item => (ReadOnlyDatabaseFieldMetadata)item));

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseFieldMetadata"/> 列挙中の <see cref="ReadOnlyDatabaseFieldMetadata.FieldName"/>
        ///     をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての <see cref="ReadOnlyDatabaseFieldMetadata.FieldName"/> を変換したバイナリ配列</returns>
        public static byte[] SerializeFieldNames(this IEnumerable<ReadOnlyDatabaseFieldMetadata> src)
        {
            return src.SelectMany(row => StringExtension.ToWoditorStringBytes(row.FieldName)).ToArray();
        }

        /// <inheritdoc cref="SerializeSpecialSettingDescription(IEnumerable{ReadOnlyDatabaseFieldMetadata})"/>
        public static byte[] SerializeSpecialSettingDescription(
            this IEnumerable<DatabaseFieldMetadata> src
        ) => SerializeSpecialSettingDescription(src.Select(item => (ReadOnlyDatabaseFieldMetadata)item));


        /// <summary>
        ///     <see cref="ReadOnlyDatabaseFieldMetadata"/> 列挙中の特殊指定情報をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての特殊指定情報を変換したバイナリ配列</returns>
        public static byte[] SerializeSpecialSettingDescription(
            this IEnumerable<ReadOnlyDatabaseFieldMetadata> src
        )
        {
            var needFieldLength = DatabaseConst.MaxFieldLength;

            var result = new List<byte>();
            var definitionArray = src.ToArray();
            var fieldLength = definitionArray.Length;

            // 特殊指定数
            result.AddRange(IntExtension.ToWoditorIntBytes(needFieldLength));

            // 特殊指定
            var settingTypeList = definitionArray.Select(x => x.SpecialSettingDefinition.SettingType);
            result.AddRange(settingTypeList.Select(valueType => valueType.Code));
            // 足りない分を「特殊な指定方法を使用しない」で埋める
            for (var i = fieldLength; i < needFieldLength; i++)
            {
                result.Add(DatabaseFieldSpecialSettingType.Normal.Code);
            }

            // 項目メモ数
            result.AddRange(IntExtension.ToWoditorIntBytes(fieldLength));

            // 項目メモ
            result.AddRange(
                definitionArray.SelectMany(x => StringExtension.ToWoditorStringBytes(x.FieldMemo))
            );

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
            result.AddRange(IntExtension.ToWoditorIntBytes(specialCaseDescriptions.Count));

            // 特殊指定文字列パラメータ
            specialCaseDescriptions.ForEach(x =>
                {
                    // 文字列パラメータ数
                    result.AddRange(IntExtension.ToBytes(x.Count, Endian.Woditor));
                    // 文字列パラメータ
                    LinqExtension.ForEach(
                        x,
                        y =>
                            result.AddRange(StringExtension.ToWoditorStringBytes(y))
                    );
                }
            );

            // 特殊指定数値パラメータ数
            result.AddRange(IntExtension.ToWoditorIntBytes(specialCaseNumbers.Count));

            // 特殊指定数値パラメータ
            specialCaseNumbers.ForEach(x =>
                {
                    // 数値パラメータ数
                    result.AddRange(IntExtension.ToWoditorIntBytes(x.Count));
                    // 数値パラメータ
                    LinqExtension.ForEach(
                        x,
                        y =>
                            result.AddRange(IntExtension.ToWoditorIntBytes((int)y))
                    );
                }
            );

            // 初期値数
            result.AddRange(IntExtension.ToWoditorIntBytes(initValues.Count));

            // 初期値
            result.AddRange(initValues.Serialize());

            return result.ToArray();
        }
    }
}
