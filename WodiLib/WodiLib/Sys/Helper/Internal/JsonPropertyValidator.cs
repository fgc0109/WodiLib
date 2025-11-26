// ========================================
// Project Name : WodiLib
// File Name    : JsonPropertyValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    ///     JSONオブジェクトのプロパティ検証Helper
    /// </summary>
    internal class JsonPropertyValidator
    {
        /// <summary>
        ///     <see langword="null"/> だったプロパティ名の一覧
        /// </summary>
        public IReadOnlyList<string> NullPropertyNameList => nullPropertyNameList;

        private readonly List<string> nullPropertyNameList = new();

        /// <summary>
        ///     <paramref name="obj"/> が <see langword="null"/> かどうかをチェックし、
        ///     <see langword="null"/> の場合は <paramref name="propertyName"/> を内部リストに追加する。
        /// </summary>
        /// <param name="obj">チェック対象のオブジェクト</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>オブジェクトが <see langword="null"/> かどうか</returns>
        public bool IsNull([NotNullWhen(false)] object? obj, string propertyName)
        {
            if (obj is null)
            {
                nullPropertyNameList.Add(propertyName);
            }

            return obj is null;
        }
    }
}
