// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MultiValueObjectGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using MyAttr =
    WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes.MultiValueObjectAttribute;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.MultiValues
{
    internal class MultiValueObjectGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => MyAttr.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var thisTypeName = source.ClassName();

            var properties = GetInitializeProperties(source)
                .ToList();

            return SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $"{DefinitionSource(source)} {thisTypeName}",
                    $"{{",
                },
                SourceTextFormatter.Format(IndentSpace, ConstructorSource(properties, source)),
                new SourceFormatTarget[]
                {
                    $"}}",
                }
            );
        }

        /// <summary>
        ///     定義宣言部のソースを生成する。
        /// </summary>
        /// <returns>ソースコード文字列</returns>
        private static string DefinitionSource(INamedTypeSymbol source)
        {
            var resultBuilder = new StringBuilder();

            var accessibility = AccessibilityConverter.ConvertSourceText(source.DeclaredAccessibility);
            resultBuilder.Append(accessibility);
            resultBuilder.Append(" partial ");
            if (source.IsRecord)
            {
                resultBuilder.Append("record");
            }
            else if (source.TypeKind == TypeKind.Class)
            {
                resultBuilder.Append("class");
            }
            else
            {
                resultBuilder.Append("struct");
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        ///     初期化対象のプロパティ一覧を取得する。
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<IPropertySymbol> GetInitializeProperties(INamedTypeSymbol source)
        {
            // public なプロパティのうち init アクセサを持たないプロパティを対象とする。

            var properties = source.GetMembers()
                .Where(member => member.Kind == SymbolKind.Property
                                 && member.DeclaredAccessibility == Accessibility.Public
                )
                .Cast<IPropertySymbol>()
                .ToList();

            var initMethods = source.GetMembers()
                .Where(member => member.Kind == SymbolKind.Method
                                 && member.DeclaredAccessibility == Accessibility.Public
                                 && ((member as IMethodSymbol)?.IsInitOnly ?? false)
                );
            var initPropertiesNames = initMethods.Select(method => method.Name.Substring(4) /* "set_" を除去 */)
                .ToList();

            return properties.Where(prop => !initPropertiesNames.Contains(prop.Name));
        }

        /// <summary>
        ///     コンストラクタ部のソースブロックを生成する。
        /// </summary>
        /// <returns>ソースコード文字列ブロック</returns>
        private static SourceFormatTargetBlock ConstructorSource(
            IEnumerable<IPropertySymbol> properties,
            INamedTypeSymbol source
        )
        {
            var propertyArray = properties.ToArray();

            var typeName = source.Name;
            var argsDocComments = propertyArray.Select(prop =>
                $"/// {Tag.Param(prop.Name.ToLowerFirstChar(), Tag.See.Cref(prop.Name))}"
            );
            var argsCode = string.Join(
                ",",
                propertyArray.Select(prop => $"{prop.Type} {prop.Name.ToLowerFirstChar()}")
            );
            var validateNullArgCodes = propertyArray.Where(prop => prop.Type.TypeKind == TypeKind.Class)
                .Select(prop =>
                    $"{__}if ({prop.Name.ToLowerFirstChar()} is null) throw new System.ArgumentNullException(nameof({prop.Name.ToLowerFirstChar()}));"
                );
            var propertySetCodes = propertyArray.Select(prop => $"{__}{prop.Name} = {prop.Name.ToLowerFirstChar()};");

            return SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $"/// <summary>",
                    $"/// {__}コンストラクタ",
                    $"/// </summary>",
                },
                new SourceFormatTargetBlock(argsDocComments.ToArray()),
                new SourceFormatTarget[]
                {
                    $"public {typeName}({argsCode}) {{",
                },
                new SourceFormatTargetBlock(validateNullArgCodes.ToArray()),
                new SourceFormatTargetBlock(propertySetCodes.ToArray()),
                new SourceFormatTarget[]
                {
                    $"}}",
                }
            );
        }

        private MultiValueObjectGenerator()
        {
        }

        public static MultiValueObjectGenerator Instance { get; } = new();
    }
}
