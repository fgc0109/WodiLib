// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressBaseAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.ComponentModel;
using WodiLib.SourceGenerator.ValueObject.Attributes;
using WodiLib.SourceGenerator.ValueObject.Enums;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     変数アドレス値基底クラス自動生成用属性
    /// </summary>
    internal class VariableAddressBaseAttribute : IntValueObjectAttribute
    {
        /// <inheritdoc/>
        [DefaultValue(true)]
        public override bool IsComparable { get; init; } = false;

        /// <inheritdoc/>
        [DefaultValue(IntegralNumericOperation.Compare)]
        public override IntegralNumericOperation Operations { get; init; } = default!;

        /// <inheritdoc/>
        [DefaultValue(new[] { typeof(VariableAddress), typeof(int) })]
        public override Type[]? CompareOtherTypes { get; init; }
    }
}
