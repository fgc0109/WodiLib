// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelGenerator.BuildSettingsInterfaceSource.cs
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
        private static SourceFormatTargetBlock BuildSettingsInterfaceSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{modelInfo.Description}設定インタフェース",
                    $"/// </summary>",
                    $"{modelInfo.Accessibility} partial interface {modelInfo.SettingsInterfaceInfo.SettingsInterfaceName}{modelInfo.SettingsInterfaceInfo.ExtendsSettingsInterface}",
                    $"{{",
                },
                SourceTextFormatter.Format(
                    $"{__}",
                    modelInfo.Members.SettingsProperties.SelectMany(p => p.InterfaceDefinitionCode).ToArray()
                ),
                new[]
                {
                    $"}}",
                }
            );
        }
    }
}
