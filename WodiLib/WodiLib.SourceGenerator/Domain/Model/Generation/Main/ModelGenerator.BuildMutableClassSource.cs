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
                    $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.MutableInfo.MutableModelClassName} : ModelBase,",
                    $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                    $"{__}IEqualityComparable<{modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}>,",
                    $"{__}IEqualityComparable<{modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}>,",
                    $"{__}IDeepCloneable<{modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}>",
                    $"{{",
                },
                // ItemEquals
                BuildMutableClassItemEqualsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildMutableClassDeepCloneSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // SettingsInterface Implements
                BuildMutableClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Implicit Type Conversion Operator
                BuildImplicitTypeConversionOperatorSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildMutableClassItemEqualsSource(
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
                $"public bool ItemEquals({modelInfo.SettingsDtoInfo.SettingsDtoName}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});"
            );
        }

        private static SourceFormatTargetBlock BuildMutableClassDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public {modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object IDeepCloneable.DeepClone() => DeepClone();"
            );
        }

        private static SourceFormatTargetBlock BuildMutableClassSettingsInterfaceImplementsSource(
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

        private static SourceFormatTargetBlock BuildImplicitTypeConversionOperatorSource(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                __,
                $"private {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}? immutableInstance = null;",
                $"",
                $"/// <summary>",
                $"/// {__}読取専用クラスへの暗黙的型変換",
                $"/// </summary>",
                $"/// <param name=\"src\">変換元</param>",
                $"/// <returns>変換したインスタンス</returns>",
                $"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]",
                $"public static implicit operator {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}?({modelInfo.MutableInfo.MutableModelClassNameWithoutInOutKeyword}? src)",
                $"{{",
                $"{__}if (src is null) return null;",
                $"{__}src.immutableInstance ??= new {modelInfo.ImmutableInfo.ImmutableModelClassNameWithoutInOutKeyword}(src);",
                $"{__}return src.immutableInstance;",
                $"}}"
            );
        }
    }
}
