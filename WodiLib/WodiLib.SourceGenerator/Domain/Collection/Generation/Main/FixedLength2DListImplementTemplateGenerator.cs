// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : FixedLength2DListImplementTemplateGenerator.cs
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
    internal class FixedLength2DListImplementTemplateGenerator : TwoDListGeneratorBase
    {
        private protected override bool IsRestrictedCapacityList => false;

        public static FixedLength2DListImplementTemplateGenerator Instance { get; } = new();
    }
}
