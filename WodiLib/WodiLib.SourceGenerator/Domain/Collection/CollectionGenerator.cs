// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : CollectionGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Domain.Collection.Generation.Main;
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Collection
{
    /// <summary>
    ///     CollectionクラスGenerator
    /// </summary>
    [Generator]
    public class ValueObjectGenerator : GeneratorBase
    {
        private protected override string Key => "Domain_Collection";

        private protected override IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; } =
        {
            // attributes
            FixedLength2DListImplementTemplateAttribute.Instance,
            FixedLengthListImplementTemplateAttribute.Instance,
            FixedLengthListMethodAttribute.Instance,
            FixedLengthListPropertyAttribute.Instance,
            RestrictedCapacity2DListImplementTemplateAttribute.Instance,
            RestrictedCapacityListImplementTemplateAttribute.Instance,
        };

        private protected override IMainSourceAddable[] TargetAttributeInfoList { get; } =
        {
            FixedLength2DListImplementTemplateGenerator.Instance,
            FixedLengthListImplementTemplateGenerator.Instance,
            RestrictedCapacity2DListImplementTemplateGenerator.Instance,
            RestrictedCapacityListImplementTemplateGenerator.Instance,
        };
    }
}
