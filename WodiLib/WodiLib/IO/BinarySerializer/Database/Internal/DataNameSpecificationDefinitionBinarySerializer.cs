// ========================================
// Project Name : WodiLib
// File Name    : DataNameSpecificationDefinitionBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="DataNameSpecificationDefinition"/> およびその列挙をバイナリ配列に変換するためのHelperクラス
    /// </summary>
    internal static class DataNameSpecificationDefinitionBinarySerializer
    {
        /// <summary>
        ///     <see cref="DataNameSpecificationDefinition"/> をタイプコード値（DBタイプによる値 + タイプID）に変換する。
        /// </summary>
        /// <param name="src">処理対象</param>
        /// <returns>すべての <see cref="DataNameSpecificationDefinition"/> を変換したバイナリ配列</returns>
        public static int ToTypeCode(this DataNameSpecificationDefinition src)
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));

            return DatabaseKindMapper.ToDBDataSettingTypeCode(src.DatabaseKind) * 10000 + src.TypeId.RawValue;
        }
    }
}
