// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MutablePropertyAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Domain.Model.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     読取専用モデルクラスに抽出する対象に付与する属性
    /// </summary>
    internal class ImmutablePropertyAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(ImmutablePropertyAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "読取専用モデルクラスに抽出する対象に付与する属性";

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
            Summary = "プロパティが返す型。null の場合、元となるプロパティと同じ型を返す",
            DefaultValue = "null",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Property;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Accessibility,
                ReturnType,
            };

        private ImmutablePropertyAttribute()
        {
        }

        public static ImmutablePropertyAttribute Instance { get; } = new();
    }
}
