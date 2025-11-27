// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataNamingTypeMapper.cs
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
    ///     特定の設定値に基づいて <see cref="DatabaseDataNamingType"/> オブジェクトを取得するためのメソッド実装クラス
    /// </summary>
    internal static class DatabaseDataNamingTypeMapper
    {
        /// <summary>
        ///     設定値から <see cref="DatabaseDataNamingType"/> オブジェクトを取得する。
        /// </summary>
        /// <param name="value">引数特殊指定値</param>
        /// <returns>
        ///     <see cref="DatabaseDataNamingType"/>
        /// </returns>
        /// <exception cref="ArgumentException">存在しない値の場合。</exception>
        public static DatabaseDataNamingType FromSettingsValue(int value)
        {
            if ($"{value}".Length == 5)
                // 値が5桁の場合は DesignatedType の可能性がある
                try
                {
                    var _ = DatabaseKindMapper.FromDBDataSettingTypeCode(value.SubInt(4, 1));
                    // DB種別が取得できる場合は DesignatedType
                    return DatabaseDataNamingType.DesignatedType;
                }
                catch
                {
                    // DB種別が取得できない場合は適切な値ではない
                    throw CreateArgumentExceptionBecauseSettingsValueNotFound(value);
                }

            if (value == -1)
            {
                // 便宜上 "-1" を定義しているが、設定値 = -1 は取得させない
                throw CreateArgumentExceptionBecauseSettingsValueNotFound(value);
            }

            try
            {
                return DatabaseDataNamingType.AllItems.First(x => x.Code == value);
            }
            catch
            {
                throw CreateArgumentExceptionBecauseSettingsValueNotFound(value);
            }
        }

        private static ArgumentException CreateArgumentExceptionBecauseSettingsValueNotFound(int value)
        {
            return new ArgumentException($"{nameof(DatabaseDataNamingType)}の取得に失敗しました。条件値：{value}");
        }
    }
}
