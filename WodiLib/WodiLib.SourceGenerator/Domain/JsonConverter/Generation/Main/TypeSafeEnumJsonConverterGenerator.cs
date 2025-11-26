// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeSafeEnumJsonConverterGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Domain.JsonConverter.Generation.PostInitAction.Attributes;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.JsonConverter.Generation.Main
{
    internal class TypeSafeEnumJsonConverterGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute => TypeSafeEnumJsonConvertAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTImportUsingSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    "using System;",
                    "using System.Linq;",
                    "using System.Text.Json;",
                    "using System.Text.Json.Serialization;",
                }
            );
        }

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(
            SemanticModel semanticModel,
            BaseTypeDeclarationSyntax typeDecl,
            INamedTypeSymbol source,
            AttributeData selfAttributeData,
            ILogger logger
        )
        {
            var accessibility = AccessibilityConverter.ConvertSourceText(source.DeclaredAccessibility);
            var typeSafeEnumClassName = source.Name;
            var converterClassName = $"{typeSafeEnumClassName}JsonConverter";

            var classOrRecordOrStruct = source.IsRecord
                ? "record"
                : source.TypeKind == TypeKind.Class
                    ? "class"
                    : "struct";

            return SourceTextFormatter.Format(
                "",
                new[]
                {
                    $"[JsonConverter(typeof({converterClassName}))]",
                    $"{accessibility} partial {classOrRecordOrStruct} {typeSafeEnumClassName}{{}}",
                    $"/// <summary>",
                    $"/// {__}<see cref=\"{typeSafeEnumClassName}\"/> インスタンスのJSONシリアライズ/デシリアライズクラス",
                    $"/// </summary>",
                    $"{accessibility} class {converterClassName} : JsonConverter<{typeSafeEnumClassName}>{{",
                },
                SourceTextFormatter.Format(
                    __,
                    new[]
                    {
                        $"/// <inheritdoc/>",
                        $"public override {typeSafeEnumClassName}? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)",
                        $"{{",
                        $"{__}var id = reader.GetString();",
                        $"{__}if (id is null) return null;",
                        $"{__}return {typeSafeEnumClassName}.AllItems.First(item => item.Id == id);",
                        $"}}",
                        $"/// <inheritdoc/>",
                        $"public override void Write(Utf8JsonWriter writer, {typeSafeEnumClassName}? value, JsonSerializerOptions options)",
                        $"{{",
                        $"{__}if (value is null) {{",
                        $"{__}{__}writer.WriteNullValue();",
                        $"{__}{__}return;",
                        $"{__}}}",
                        $"{__}writer.WriteStringValue(value.Id);",
                        $"}}",
                    }
                ),
                new[]
                {
                    $"}}",
                }
            );
        }

        private TypeSafeEnumJsonConverterGenerator()
        {
        }

        public static TypeSafeEnumJsonConverterGenerator Instance { get; } = new();
    }
}
