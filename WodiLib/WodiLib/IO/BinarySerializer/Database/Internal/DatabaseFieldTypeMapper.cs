// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldTypeMapper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     特定の設定値に基づいて <see cref="DatabaseFieldType"/> オブジェクトを取得するためのメソッド実装クラス
    /// </summary>
    internal static class DatabaseFieldTypeMapper
    {
        /// <summary>
        ///     設定値から <see cref="DatabaseFieldType"/> オブジェクトを取得する。
        /// </summary>
        /// <param name="value">設定値</param>
        /// <returns>
        ///     <see cref="DatabaseFieldType"/>
        /// </returns>
        /// <exception cref="ArgumentException">存在しない値の場合。</exception>
        public static DatabaseFieldType FromSettingsValue(int value)
        {
            try
            {
                return DatabaseFieldType.AllItems.First(x => x.Code == value.SubInt(3, 1));
            }
            catch (Exception)
            {
                throw new ArgumentException($"{nameof(DatabaseFieldType)}の取得に失敗しました。条件値：{value}");
            }
        }
    }
}
