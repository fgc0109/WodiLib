// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : PartialClassGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
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

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var className = workState.Name;
            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    $"{accessibility} partial class {className} {{",
                },
                SourceTextFormatter.Format(
                    $"{__}",
                    ReplaceClassBodyTemplateLiteral(workState)
                ),
                new[]
                {
                    $"}}",
                }
            );
        }

        private static string[] ReplaceClassBodyTemplateLiteral(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var templateLiteral = propertyValues[TemplateContextAttribute.ClassBodyTemplateLiteral.Name]!;
            var args = propertyValues.GetArrayValue(TemplateContextAttribute.Args.Name)
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

        private protected override string HintName(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var outKey = propertyValues[TemplateContextAttribute.OutKey.Name]!;

            return $"{workState.FullName.ReplaceAngleBracketsToUnderscore()}_{outKey}";
        }

        private PartialClassGenerator()
        {
        }

        public static PartialClassGenerator Instance { get; } = new();
    }
}
