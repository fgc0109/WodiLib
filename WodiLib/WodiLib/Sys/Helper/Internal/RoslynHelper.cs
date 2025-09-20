// ========================================
// Project Name : WodiLib
// File Name    : RoslynHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     構文解析のためのHelperクラス
    /// </summary>
    internal static class RoslynHelper
    {
        /// <summary>
        ///     対象を強制的に <see langword="null"/> 許容参照型にする。
        /// </summary>
        /// <param name="target">対象</param>
        /// <typeparam name="T">対象の型</typeparam>
        /// <returns>
        ///     <paramref name="target"/>
        /// </returns>
        // ReSharper disable once ReturnTypeCanBeNotNullable
        public static T? AsNullable<T>(T target) => target;
    }
}
