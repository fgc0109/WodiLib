// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntValueObjectGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues.Abstract;
using WodiLib.SourceGenerator.ValueObject.Generation.PostInitAction.Attributes;

namespace WodiLib.SourceGenerator.ValueObject.Generation.Main.SingleValues
{
    /// <summary>
    ///     単一 <see cref="int"/> 値を持つ値オブジェクトジェネレータ
    /// </summary>
    internal class IntValueObjectGenerator : IntegralValueObjectGeneratorTemplate
    {
        /// <inheritdoc/>
        public override InitializeAttributeSourceAddable TargetAttribute => IntValueObjectAttribute.Instance;

        /// <inheritdoc/>
        private protected override Type WrapType => typeof(int);

        private protected override string GetMinDefaultValue()
            => (string)IntValueObjectAttribute.MinValue.DefaultValue!;

        private protected override string GetMaxDefaultValue()
            => (string)IntValueObjectAttribute.MaxValue.DefaultValue!;

        private protected override string GetSafetyMinDefaultValue()
            => (string)IntValueObjectAttribute.SafetyMinValue.DefaultValue!;

        private protected override string GetSafetyMaxDefaultValue()
            => (string)IntValueObjectAttribute.SafetyMaxValue.DefaultValue!;


        /// <summary>インスタンス（シングルトン）</summary>
        public static IntValueObjectGenerator Instance { get; } = new();
    }
}
