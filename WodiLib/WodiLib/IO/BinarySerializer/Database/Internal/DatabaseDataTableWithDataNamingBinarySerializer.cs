// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableWithDataNamingBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinition"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseDataTableWithDataNamingBinarySerializer
    {
        /// <inheritdoc cref="Serialize(IEnumerable{ReadOnlyDatabaseDataTableWithDataNamingDefinition})"/>
        public static byte[] Serialize(this IEnumerable<DatabaseDataTableWithDataNamingDefinition> src)
            => Serialize(src.Select(item => (ReadOnlyDatabaseDataTableWithDataNamingDefinition)item));

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinition"/> 列挙をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての <see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinition"/> を変換したバイナリ配列</returns>
        public static byte[] Serialize(this IEnumerable<ReadOnlyDatabaseDataTableWithDataNamingDefinition> src)
            => src.SelectMany(Serialize).ToArray();

        /// <inheritdoc cref="Serialize(ReadOnlyDatabaseDataTableWithDataNamingDefinitionList)"/>
        public static byte[] Serialize(this DatabaseDataTableWithDataNamingDefinitionList src)
            => Serialize((ReadOnlyDatabaseDataTableWithDataNamingDefinitionList)src);

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinitionList"/> をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns><see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinitionList"/> を変換したバイナリ配列</returns>
        public static byte[] Serialize(this ReadOnlyDatabaseDataTableWithDataNamingDefinitionList src)
            => src.SelectMany(Serialize).ToArray();

        /// <inheritdoc cref="Serialize(ReadOnlyDatabaseDataTableWithDataNamingDefinition)"/>
        public static byte[] Serialize(this DatabaseDataTableWithDataNamingDefinition src)
            => Serialize((ReadOnlyDatabaseDataTableWithDataNamingDefinition)src);

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinition"/> をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns><see cref="ReadOnlyDatabaseDataTableWithDataNamingDefinition"/> を変換したバイナリ配列</returns>
        public static byte[] Serialize(this ReadOnlyDatabaseDataTableWithDataNamingDefinition src)
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(DatabaseDataTableWithDataNamingReader.Header);

            // データ名の指定方法
            if (src.DataNamingDefinition.NamingType == DatabaseDataNamingType.DesignatedType)
            {
                // 指定方法＝指定DBの指定タイプ の場合、DBタイプによる値 + タイプID
                if (src.DataNamingDefinition.ReferDatabaseDefinition is null)
                {
                    throw new NullReferenceException(
                        ErrorMessage.NotNull(
                            $"{nameof(src)}.{nameof(src.DataNamingDefinition)}.{nameof(src.DataNamingDefinition.ReferDatabaseDefinition)}"
                        )
                    );
                }

                var typeCode = src.DataNamingDefinition.ReferDatabaseDefinition.ToTypeCode();
                result.AddRange(typeCode.ToBytes(Endian.Woditor));
            }
            else
            {
                // 指定方法≠指定DBの指定タイプ の場合、指定方法種別コードのみ
                result.AddRange(src.DataNamingDefinition.NamingType.Code.ToWoditorIntBytes());
            }

            // 項目数
            result.AddRange(src.DataTable.FieldCount.ToWoditorIntBytes());

            // 設定種別 & 種別順列
            result.AddRange(src.DataTable.GetFieldTypes().Serialize());

            // データ数
            result.AddRange(src.DataTable.DataCount.ToWoditorIntBytes());

            // データ設定値
            result.AddRange(src.DataTable.SerializeValuesDividedType());

            return result.ToArray();
        }
    }
}
