// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValueValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseFieldValue"/> 検証処理クラス
    /// </summary>
    internal static class DatabaseFieldValueValidationHelper
    {
        public static void ValidateMatchFieldType(
            NamedValue<DatabaseFieldValue> src,
            DatabaseFieldType type
        )
        {
            ThrowHelper.ValidateArgumentNotNull(src is null, nameof(src));
            ThrowHelper.ValidateArgumentNotNull(src.Value is null, src.Name);
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));

            if (src.Value.Type == type)
            {
                return;
            }

            throw new ArgumentException(
                ErrorMessage.NotExecute($"{src.Name}の値種別が${type.Id}ではないため")
            );
        }
    }
}
