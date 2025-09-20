// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelGenerator.BuildMutableClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Model.Generation.Main
{
    internal partial class ModelGenerator
    {
        private static SourceFormatTargetBlock BuildImmutableClassSource(
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
                    $"/// {__}【読取専用】{modelInfo.Description}",
                    $"/// </summary>",
                    $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.ImmutableInfo.ImmutableModelClassName} : WodiLib.Sys.ModelBase,",
                    $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                    $"{__}WodiLib.Sys.IEqualityComparable<{modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}>,",
                    $"{__}WodiLib.Sys.IEqualityComparable<{modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}>,",
                    $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}>",
                },
                // TypeParamConstraints
                SourceTextFormatter.Format(
                    __,
                    modelInfo.TypeParamConstraints ?? Array.Empty<string>()
                ),
                new[]
                {
                    $"{{",
                },
                // ItemEquals
                BuildImmutableClassItemEqualsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildImmutableClassPropDeepCloneSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildImmutableClassItemEqualsSource(
            ModelInformation modelInfo
        )
        {
            var objectItemEqualsKeyword = (modelInfo.IsExtendClass, modelInfo.IsAbstract) switch
            {
                (true, _) => "override ",
                (_, true) => "virtual ",
                (_, false) => "",
            };

            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public bool ItemEquals({modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"public bool ItemEquals({modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});"
            );
        }

        private static SourceFormatTargetBlock BuildImmutableClassPropDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.If(
                !modelInfo.IsAbstract,
                __,
                $"/// <inheritdoc/>",
                $"public {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword} DeepClone() => new {modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}(this);",
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
            );
        }
    }
}
