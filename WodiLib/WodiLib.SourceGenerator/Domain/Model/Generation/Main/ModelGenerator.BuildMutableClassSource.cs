// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelGenerator.BuildImmutableClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Model.Generation.Main
{
    internal partial class ModelGenerator
    {
        private static SourceFormatTargetBlock BuildMutableClassSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                // class start
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{modelInfo.Description}",
                    $"/// </summary>",
                    $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.MutableInfo.MutableModelClassName} : {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword},",
                    $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}>",
                    $"{{",
                },
                // Properties
                BuildMutableClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildConstructorSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildMutableClassDeepCloneSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildMutableClassSettingsInterfaceImplementsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.MutableModelProperties.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildConstructorSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.Constructors.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.Methods.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildMutableClassDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public new {modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
            );
        }
    }
}
