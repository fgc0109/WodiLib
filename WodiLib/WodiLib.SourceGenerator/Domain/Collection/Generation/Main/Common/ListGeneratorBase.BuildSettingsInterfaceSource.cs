// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildSettingsInterfaceSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    /// <summary>
    ///     テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal abstract partial class ListGeneratorBase
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
