// ========================================
// Project Name : WodiLib
// File Name    : FieldValueListValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseFieldValue"/> リストの検証Helperクラス
    /// </summary>
    internal static class FieldValueListValidationHelper
    {
        /// <summary>
        ///     DB項目値種別の検証処理を行う。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <param name="type">一致すべき値種別</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="target"/> のいずれかの要素の値種別が <paramref name="type"/> と異なる場合。
        /// </exception>
        public static void ValidateUnifiedFieldType(
            NamedValue<IEnumerable<DatabaseFieldValue>> target,
            DatabaseFieldType type
        )
        {
            ThrowHelper.ValidateArgumentNotNull(target is null, nameof(target));
            ThrowHelper.ValidateArgumentNotNull(target.Value is null, target.Name);
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            var valueArray = target.Value.ToArray();
            if (valueArray.Length == 0)
            {
                return;
            }

            ThrowHelper.ValidateArgumentUnsuitable(
                valueArray.Any(value => value.Type != type),
                target.Name,
                $"値種別が {type} で統一されていないため"
            );
        }
    }
}
