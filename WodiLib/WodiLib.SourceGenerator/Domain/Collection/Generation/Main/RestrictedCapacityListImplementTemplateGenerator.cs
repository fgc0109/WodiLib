// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplateGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main
{
    /// <summary>
    ///     テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal class RestrictedCapacityListImplementTemplateGenerator : ListGeneratorBase
    {
        private protected override bool IsRestrictedCapacityList => true;

        public static RestrictedCapacityListImplementTemplateGenerator Instance { get; } = new();
    }
}
