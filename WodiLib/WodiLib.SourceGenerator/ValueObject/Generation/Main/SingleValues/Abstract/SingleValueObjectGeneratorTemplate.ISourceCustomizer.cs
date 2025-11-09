// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.ISourceCustomizer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.SourceBuilder;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    internal abstract partial class SingleValueObjectGeneratorTemplate
    {
        /// <summary>
        ///     出力ソース個別カスタマイズ処理
        /// </summary>
        private interface ISourceCustomizer
        {
            /// <summary>
            ///     object 型のインスタンスとの比較コード
            /// </summary>
            /// <returns></returns>
            SourceFormatTarget SourceFormatTargetEqualsObject(
                SemanticModel semanticModel,
                BaseTypeDeclarationSyntax typeDecl,
                INamedTypeSymbol source,
                AttributeData selfAttributeData,
                ILogger logger
            );

            /// <summary>
            ///     同じ型のインスタンスとの比較コード
            /// </summary>
            /// <returns></returns>
            SourceFormatTarget SourceFormatTargetEqualsOther(
                SemanticModel semanticModel,
                BaseTypeDeclarationSyntax typeDecl,
                INamedTypeSymbol source,
                AttributeData selfAttributeData,
                ILogger logger
            );
        }
    }
}
