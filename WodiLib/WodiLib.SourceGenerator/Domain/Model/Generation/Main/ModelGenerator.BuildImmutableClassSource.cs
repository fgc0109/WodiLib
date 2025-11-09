// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelGenerator.BuildMutableClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
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
                $"/// <summary>",
                $"/// {__}【読取専用】{modelInfo.Description}",
                $"/// </summary>",
                $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.ImmutableInfo.ImmutableModelClassName} : WodiLib.Sys.ModelBase,",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                $"{__}WodiLib.Sys.IEqualityComparable<{modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}>,",
                $"{__}WodiLib.Sys.IEqualityComparable<{modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}>,",
                $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}>",
                // TypeParamConstraints
                SourceTextFormatter.Format(
                    __,
                    modelInfo.TypeParamConstraints ?? Array.Empty<string>()
                ),
                new[]
                {
                    $"{{",
                },
                // Properties
                BuildImmutablePropertiesSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Fields
                BuildImmutableFieldsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildImmutableConstructorsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildImmutableMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // ItemEquals
                BuildImmutableClassItemEqualsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildImmutableClassPropDeepCloneSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // SettingsInterface Implements
                BuildImmutableClassSettingsInterfaceImplementsSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildImmutablePropertiesSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.ImmutableModelProperties.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildImmutableFieldsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"internal {modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword} MutableInstance {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildImmutableConstructorsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"internal {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}({modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword} MutableInstance)",
                $"{{",
                $"{__}this.MutableInstance = MutableInstance;",
                $"{__}PropagatePropertyChangeEvent(MutableInstance);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildImmutableMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.PureMethods.SelectMany(p => p.ImplementationCode).ToArray()
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
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.SettingsDtoInfo.SettingsDtoName}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => MutableInstance.ItemEquals(other);"
            );
        }

        private static SourceFormatTargetBlock BuildImmutableClassSettingsInterfaceImplementsSource(
            ModelInformation modelInfo
        )
        {
            var targetProperties =
                modelInfo.Members.SettingsProperties.Where(definition => definition.IsOverrideReturnType);

            return SourceTextFormatter.Format(
                __,
                targetProperties.Select(definition => definition.GetInterfaceImplementCode
                    )
                    .ToArray()
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
                $"[Pure]",
                $"public {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword} DeepClone() => new {modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}(this);",
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
            );
        }
    }
}
