// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ValueObjectGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.ValueObject.Generation.Main.MultiValues;
using WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Enums;

namespace WodiLib.SourceGenerator.ValueObject
{
    /// <summary>
    ///     値オブジェクトSourceGenerator
    /// </summary>
    [Generator]
    public class ValueObjectGenerator : GeneratorBase
    {
        private protected override string Key => "Operation";

        private protected override IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; } =
        {
            // attributes
            ByteValueObjectAttribute.Instance,
            IntValueObjectAttribute.Instance,
            MultiValueObjectAttribute.Instance,
            StringValueObjectAttribute.Instance,
            // enums
            CastType.Instance,
            IntegralNumericOperation.Instance,
        };

        private protected override IMainSourceAddable[] TargetAttributeInfoList { get; } =
        {
            StringValueObjectGenerator.Instance,
            IntValueObjectGenerator.Instance,
            ByteValueObjectGenerator.Instance,
            MultiValueObjectGenerator.Instance,
        };
    }
}
