// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : JsonConverterGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Domain.JsonConverter.Generation.Main;
using WodiLib.SourceGenerator.Domain.JsonConverter.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.JsonConverter
{
    /// <summary>
    ///     JsonConverterGenerator
    /// </summary>
    [Generator]
    public class JsonConverterGenerator : GeneratorBase
    {
        private protected override string Key => "JsonConverter";

        private protected override IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; } =
        {
            // attributes
            ByteValueObjectJsonConvertAttribute.Instance,
            IntValueObjectJsonConvertAttribute.Instance,
            StringValueObjectJsonConvertAttribute.Instance,
            TypeSafeEnumJsonConvertAttribute.Instance,
        };

        private protected override IMainSourceAddable[] TargetAttributeInfoList { get; } =
        {
            ByteValueObjectJsonConverterGenerator.Instance,
            IntValueObjectJsonConverterGenerator.Instance,
            StringValueObjectJsonConverterGenerator.Instance,
            TypeSafeEnumJsonConverterGenerator.Instance,
        };
    }
}
