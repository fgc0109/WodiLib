// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeTableBinarySerializer.cs
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
    ///     <see cref="ReadOnlyDatabaseTypeTable"/> をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DatabaseTypeTableBinarySerializer
    {
        /// <inheritdoc cref="Serialize(ReadOnlyDatabaseTypeTable)"/>
        public static byte[] Serialize(this DatabaseTypeTable src)
            => Serialize((ReadOnlyDatabaseTypeTable)src);

        /// <summary>
        ///     <see cref="ReadOnlyDatabaseTypeTable"/> をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns><see cref="ReadOnlyDatabaseTypeTable"/> を変換したバイナリ配列</returns>
        public static byte[] Serialize(this ReadOnlyDatabaseTypeTable src)
        {
            var result = new List<byte>();

            result.AddRange(src.ToTypeDefinitionBinary());
            result.AddRange(src.ToDataDefinitionBinary());

            return result.ToArray();
        }

        /// <summary>
        ///     タイプ設定をバイナリ配列に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        private static IEnumerable<byte> ToTypeDefinitionBinary(this ReadOnlyDatabaseTypeTable src)
        {
            var result = new List<byte>();

            // タイプ名
            result.AddRange(((string)src.TypeName).ToWoditorStringBytes());

            // 項目数
            result.AddRange(src.FieldCount.ToWoditorIntBytes());

            // 項目名
            result.AddRange(
                src.FieldDefinitionList.Select(x => x).SerializeFieldNames()
            );

            // データ数
            result.AddRange(src.DataCount.ToWoditorIntBytes());

            // データ名
            result.AddRange(src.SelectMany(row => ((string)row.DataName).ToWoditorStringBytes()));

            // メモ
            result.AddRange(((string)src.Memo).ToWoditorStringBytes());

            // 特殊指定
            result.AddRange(
                src.FieldDefinitionList.SerializeSpecialSettingDescription()
            );

            return result;
        }

        /// <summary>
        ///     データ設定をバイナリリストに詰める。
        /// </summary>
        /// <param name="src">処理対象</param>
        private static IEnumerable<byte> ToDataDefinitionBinary(this ReadOnlyDatabaseTypeTable src)
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
            result.AddRange(src.FieldCount.ToWoditorIntBytes());

            // 設定種別 & 種別順列
            result.AddRange(
                src.FieldDefinitionList.SerializeFieldTypesAndOrder()
            );

            // データ数
            result.AddRange(src.DataCount.ToWoditorIntBytes());

            // データ設定値
            result.AddRange(src.SerializeValuesDividedType());

            return result;
        }

        /// <summary>
        ///     <see cref="DatabaseFieldValue"/> 列挙を値種別ごとにバイナリ配列に変換する。
        /// </summary>
        /// <remarks>
        ///     1行ごとに数値項目と文字列項目を個別にバイナリ変換し、最後に結合する。
        /// </remarks>
        /// <param name="src">処理対象</param>
        /// <returns>すべての項目値を変換したバイナリ配列</returns>
        public static byte[] SerializeValuesDividedType(this ReadOnlyDatabaseTypeTable src)
            => src.SelectMany(row => row.SerializeValuesDividedType()).ToArray();
    }
}
