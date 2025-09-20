// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacity2DListImplementTemplateGenerator.cs
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
    internal class RestrictedCapacity2DListImplementTemplateGenerator : TwoDListGeneratorBase
    {
        private protected override bool IsRestrictedCapacityList => true;

        public static RestrictedCapacity2DListImplementTemplateGenerator Instance { get; } = new();
    }
}
