// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ParameterSymbolExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core.Extensions
{
    /// <summary>
    ///     <see cref="IParameterSymbol"/> 拡張クラス
    /// </summary>
    internal static class ParameterSymbolExtension
    {
        /// <summary>
        ///     <see cref="IParameterSymbol.RefKind"/> をソースコード出力用の文字列に変換して返す。
        /// </summary>
        /// <param name="symbol">対象</param>
        /// <returns>ソースコードに出力するための文字列</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="IParameterSymbol.RefKind"/> が不適切な値の場合。
        /// </exception>
        public static string ToRefKindString(this IParameterSymbol symbol)
        {
            return symbol.RefKind switch
            {
                RefKind.None => " ",
                RefKind.Ref => "ref ",
                RefKind.Out => "out ",
                RefKind.In => " in",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
