// ========================================
// Project Name : WodiLib
// File Name    : ValidateHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     検証結果Helper
    /// </summary>
    [Obsolete]
    internal static class ValidateHelper
    {
        /// <summary>
        ///     値が指定範囲内に含まれるかどうかを判定する。
        /// </summary>
        /// <param name="target">検証対象</param>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        /// <returns>
        ///     <paramref name="target"/> が <paramref name="min"/>未満 または
        ///     <paramref name="max"/>を超える場合、<see langword="false"/>
        /// </returns>
        public static bool ValueRange(int target, int min, int max)
            => min <= target && target <= max;
    }
}
