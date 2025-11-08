// ========================================
// Project Name : WodiLib
// File Name    : DatabaseKindMapper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    ///     特定の設定値に基づいて <see cref="DatabaseKind"/> オブジェクトを取得するためのメソッド実装クラス
    /// </summary>
    internal static class DatabaseKindMapper
    {
        /// <summary>
        ///     引数特殊指定値から <see cref="DatabaseKind"/> オブジェクトを取得する。
        /// </summary>
        /// <param name="value">引数特殊指定値</param>
        /// <returns>
        ///     <see cref="DatabaseKind"/>
        /// </returns>
        /// <exception cref="ArgumentException">存在しない値の場合。</exception>
        public static DatabaseKind FromSpecialArgCode(int value)
        {
            return value switch
            {
                0 => DatabaseKind.System,
                1 => DatabaseKind.User,
                2 => DatabaseKind.Changeable,
                _ => throw new ArgumentException($"{nameof(DatabaseKind)}の取得に失敗しました。条件値：{value}"),
            };
        }

        /// <summary>
        ///     DBデータ種別設定コードから <see cref="DatabaseKind"/> オブジェクトを取得する。
        /// </summary>
        /// <param name="value">引数特殊指定値</param>
        /// <returns>
        ///     <see cref="DatabaseKind"/>
        /// </returns>
        /// <exception cref="ArgumentException">存在しない値の場合。</exception>
        public static DatabaseKind FromDBDataSettingTypeCode(int value)
        {
            return value switch
            {
                1 => DatabaseKind.System,
                2 => DatabaseKind.User,
                3 => DatabaseKind.Changeable,
                _ => throw new ArgumentException($"{nameof(DatabaseKind)}の取得に失敗しました。条件値：{value}"),
            };
        }

        /// <summary>
        ///     DBデータ種別設定コードから <see cref="DatabaseKind"/> オブジェクトを取得する。
        /// </summary>
        /// <param name="value">引数特殊指定値</param>
        /// <returns>
        ///     <see cref="DatabaseKind"/>
        /// </returns>
        /// <exception cref="ArgumentException">存在しない値の場合。</exception>
        public static int ToDBDataSettingTypeCode(DatabaseKind value)
        {
            if (value == DatabaseKind.System) return 1;
            if (value == DatabaseKind.User) return 2;
            if (value == DatabaseKind.Changeable) return 3;
            throw new ArgumentException($"{nameof(DatabaseKind)}の取得に失敗しました。条件値：{value}");
        }
    }
}
