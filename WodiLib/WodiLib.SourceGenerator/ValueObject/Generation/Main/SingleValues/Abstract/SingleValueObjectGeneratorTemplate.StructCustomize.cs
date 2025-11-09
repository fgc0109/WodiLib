// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SingleValueObjectGeneratorTemplate.StructCustomize.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.Abstract.SingleValueObjectAttribute;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract
{
    internal abstract partial class SingleValueObjectGeneratorTemplate
    {
        /// <summary>
        ///     対象が構造体の場合の出力カスタマイズ
        /// </summary>
        private class StructCustomize : ISourceCustomizer
        {
            /// <summary>
            ///     インスタンス（シングルトン）
            /// </summary>
            public static StructCustomize Instance { get; } = new();

            private StructCustomize()
            {
            }

            /// <inheritdoc/>
            public SourceFormatTarget SourceFormatTargetEqualsObject(
                SemanticModel semanticModel,
                BaseTypeDeclarationSyntax typeDecl,
                INamedTypeSymbol source,
                AttributeData selfAttributeData,
                ILogger logger
            )
            {
                var className = source.Name;
                return $"public override bool Equals(object? obj) => obj is {className} other && Equals(other);";
            }

            /// <inheritdoc/>
            public SourceFormatTarget SourceFormatTargetEqualsOther(
                SemanticModel semanticModel,
                BaseTypeDeclarationSyntax typeDecl,
                INamedTypeSymbol source,
                AttributeData selfAttributeData,
                ILogger logger
            )
            {
                var className = source.ClassName();
                var propertyName = selfAttributeData.GetPropertyDataRecursive<string>(MyAttr.PropertyName.Name);

                return $"public bool Equals({className} other) => {propertyName}.Equals(other.{propertyName});";
            }
        }
    }
}
