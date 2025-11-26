// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ByteValueObjectJsonConverterGenerator.cs
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
    internal class ByteValueObjectJsonConverterGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute
            => ByteValueObjectJsonConvertAttribute.Instance;

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
            var byteValueObjectClassName = source.Name;
            var converterClassName = $"{byteValueObjectClassName}JsonConverter";

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
                    $"{accessibility} partial {classOrRecordOrStruct} {byteValueObjectClassName}{{}}",
                    $"/// <summary>",
                    $"/// {__}<see cref=\"{byteValueObjectClassName}\"/> インスタンスのJSONシリアライズ/デシリアライズクラス",
                    $"/// </summary>",
                    $"{accessibility} class {converterClassName} : JsonConverter<{byteValueObjectClassName}>{{",
                },
                SourceTextFormatter.Format(
                    __,
                    new[]
                    {
                        $"/// <inheritdoc/>",
                        $"public override {byteValueObjectClassName} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)",
                        $"{{",
                        $"{__}var rawValue = reader.GetByte();",
                        $"{__}return new {byteValueObjectClassName}(rawValue);",
                        $"}}",
                        $"/// <inheritdoc/>",
                        $"public override void Write(Utf8JsonWriter writer, {byteValueObjectClassName}? value, JsonSerializerOptions options)",
                        $"{{",
                        $"{__}if (value is null) {{",
                        $"{__}{__}writer.WriteNullValue();",
                        $"{__}{__}return;",
                        $"{__}}}",
                        $"{__}writer.WriteNumberValue(value.RawValue);",
                        $"}}",
                    }
                ),
                new[]
                {
                    $"}}",
                }
            );
        }

        private ByteValueObjectJsonConverterGenerator()
        {
        }

        public static ByteValueObjectJsonConverterGenerator Instance { get; } = new();
    }
}
