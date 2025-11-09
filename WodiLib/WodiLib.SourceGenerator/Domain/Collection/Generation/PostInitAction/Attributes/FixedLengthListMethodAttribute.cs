// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ImmutableMethodAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     容量固定クラスに抽出する対象のメソッドに付与する属性
    /// </summary>
    internal class FixedLengthListMethodAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(FixedLengthListMethodAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "容量固定クラスに抽出する対象のメソッドに付与する属性";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Accessibility = new()
        {
            Name = nameof(Accessibility),
            Type = typeof(string).FullName!,
            Summary = "アクセス修飾子",
            Remarks = "デフォルト値: \"public\"",
            DefaultValue = "\"public\"",
        };

        public static readonly PropertyInfo ReturnType = new()
        {
            Name = nameof(ReturnType),
            Type = typeof(Type).FullName!,
            Summary = "メソッドが返す型。null の場合、元となるプロパティと同じ型を返す",
            DefaultValue = "null",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Method;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Accessibility,
                ReturnType,
            };

        private FixedLengthListMethodAttribute()
        {
        }

        public static FixedLengthListMethodAttribute Instance { get; } = new();
    }
}
