// ========================================
// Project Name : WodiLib
// File Name    : CommonIntValueObjectAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.ComponentModel;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.Sys
{
    /// <summary>
    ///     汎用単一int値オブジェクトの設定属性
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class CommonIntValueObjectAttribute : IntValueObjectAttribute
    {
        /// <inheritdoc/>
        [DefaultValue(CastType.Implicit)]
        public override CastType CastType { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(true)]
        public override bool IsComparable { get; init; } = false;
    }
}
