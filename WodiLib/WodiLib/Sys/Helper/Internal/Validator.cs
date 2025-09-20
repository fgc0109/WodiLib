// ========================================
// Project Name : WodiLib
// File Name    : Validator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     検証処理実装クラス
    /// </summary>
    internal static class Validator
    {
        /// <summary>
        /// </summary>
        /// <param name="target"></param>
        /// <param name="itemName"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="target"/> が <paramref name="min"/> 未満または <paramref name="max"/> を超える場合。
        /// </exception>
        public static void ValidateArgumentValueRange(int target, string itemName, int min, int max)
        {
            ThrowHelper.ValidateArgumentValueRange(
                !target.IsBetween(min, max),
                itemName,
                target,
                min,
                max
            );
        }
    }
}
