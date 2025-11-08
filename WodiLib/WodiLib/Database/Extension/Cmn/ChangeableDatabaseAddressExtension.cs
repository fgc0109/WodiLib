// ========================================
// Project Name : WodiLib
// File Name    : ChangeableDatabaseAddressExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="ChangeableDatabaseAddress"/> 拡張クラス
    /// </summary>
    public static class ChangeableDatabaseAddressExtension
    {
        /// <summary>タイプIDを取得する。</summary>
        /// <returns>タイプID</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        public static TypeId GetTypeId(this ChangeableDatabaseAddress src)
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));
            return src.RawValue.SubInt(6, 2);
        }

        /// <summary>データIDを取得する。</summary>
        /// <returns>データID</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        public static DataId GetDataId(this ChangeableDatabaseAddress src)
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));
            return src.RawValue.SubInt(2, 4);
        }

        /// <summary>項目IDを取得する。</summary>
        /// <returns>項目ID</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        public static FieldId GetFieldId(this ChangeableDatabaseAddress src)
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));
            return src.RawValue.SubInt(0, 2);
        }
    }
}
