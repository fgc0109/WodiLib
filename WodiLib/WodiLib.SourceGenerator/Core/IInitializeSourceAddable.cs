// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IInitializeSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     <see cref="IncrementalGeneratorPostInitializationContext"/> に関連クラスのソースを追加できることを示すインタフェース
    /// </summary>
    internal interface IInitializeSourceAddable
    {
        /// <summary>
        ///     ソースを追加する。
        /// </summary>
        /// <param name="context">追加先コンテキスト</param>
        /// <param name="logger">ロガー</param>
        public void Emit(IncrementalGeneratorPostInitializationContext context, ILogger logger);
    }
}
