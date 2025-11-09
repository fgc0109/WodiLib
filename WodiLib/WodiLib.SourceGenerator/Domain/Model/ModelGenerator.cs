// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core;
using WodiLib.SourceGenerator.Domain.Model.Generation.Main;
using WodiLib.SourceGenerator.Domain.Model.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.Domain.Model
{
    /// <summary>
    ///     ModelクラスGenerator
    /// </summary>
    [Generator]
    public class ValueObjectGenerator : GeneratorBase
    {
        private protected override string Key => "Domain.Model";

        private protected override IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; } =
        {
            // attributes
            ModelAttribute.Instance,
            ImmutableMethodAttribute.Instance,
            ImmutablePropertyAttribute.Instance,
            SettingsPropertyAttribute.Instance,
        };

        private protected override IMainSourceAddable[] TargetAttributeInfoList { get; } =
        {
            ModelGenerator.Instance,
        };
    }
}
