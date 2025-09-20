// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : FixedLengthListImplementTemplateGenerator.cs
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
    internal class FixedLengthListImplementTemplateGenerator : ListGeneratorBase
    {
        private protected override bool IsRestrictedCapacityList => false;

        public static FixedLengthListImplementTemplateGenerator Instance { get; } = new();
    }
}
