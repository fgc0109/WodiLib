// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PartialClassGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.TemplateEngine.Generation.PostInitAction.Attributes;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.TemplateEngine.Generation.Main
{
    internal class PartialClassGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => TemplateContextAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var className = source.Name;
            var accessibility = AccessibilityConverter.ConvertSourceText(source.DeclaredAccessibility);

            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    $"{accessibility} partial class {className} {{",
                },
                SourceTextFormatter.Format(
                    $"{__}",
                    ReplaceClassBodyTemplateLiteral(selfAttributeData)
                ),
                new[]
                {
                    $"}}",
                }
            );
        }

        private static string[] ReplaceClassBodyTemplateLiteral(AttributeData selfAttributeData)
        {
            var templateLiteral =
                selfAttributeData.GetPropertyData<string>(TemplateContextAttribute.ClassBodyTemplateLiteral.Name)!;
            var args = selfAttributeData.GetArrayPropertyData(TemplateContextAttribute.Args.Name)
                       ?? Array.Empty<string>();

            // テンプレート文字列を順次置換
            var result = args.Aggregate(
                templateLiteral,
                (current, kv) =>
                {
                    var split = kv.Split('=');
                    var key = $"#{split[0]}#";
                    var value = split[1];

                    return current.Replace(key, value);
                }
            );

            // 改行コードで分割して返却
            return result.Split('\n');
        }

        private PartialClassGenerator()
        {
        }

        public static PartialClassGenerator Instance { get; } = new();
    }
}
