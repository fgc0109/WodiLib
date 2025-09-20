// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildSettingsDtoSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal abstract partial class ListGeneratorBase
    {
        private static SourceFormatTargetBlock BuildSettingsDtoSource(ModelInformation modelInfo)
        {
            var objectItemEqualsKeyword = (modelInfo.IsExtendClass, modelInfo.IsAbstract) switch
            {
                (true, _) => "override ",
                (_, true) => "virtual ",
                (_, false) => "",
            };

            return SourceTextFormatter.Format(
                "",
                // -----,
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{modelInfo.Description}設定DTO",
                    $"/// </summary>",
                    $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial record {modelInfo.SettingsDtoInfo.SettingsDtoName}(IReadOnlyList<{modelInfo.ElementSettingsType}> Settings) : {modelInfo.SettingsDtoInfo.ExtendsSettingsDto}",
                    $"{{",
                },
                SourceTextFormatter.Format(
                    $"{__}",
                    modelInfo.Members.SettingsProperties.SelectMany(p => p.ImplementationRecordCode).ToArray()
                ),
                new[]
                {
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}{modelInfo.SettingsDtoInfo.SettingsInterfaceCompareCode}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public {objectItemEqualsKeyword}bool ItemEquals(object? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                    $"}}",
                }
            );
        }
    }
}
