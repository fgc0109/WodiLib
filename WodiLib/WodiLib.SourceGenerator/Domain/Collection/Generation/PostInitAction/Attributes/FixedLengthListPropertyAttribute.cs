// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ImmutablePropertyAttribute.cs
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
    ///     容量固定クラスのプロパティとして抽出する対象に付与する属性
    /// </summary>
    internal class FixedLengthListPropertyAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(FixedLengthListPropertyAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "容量固定クラスとして抽出する対象に付与する属性";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo ReturnType = new()
        {
            Name = nameof(ReturnType),
            Type = typeof(Type).FullName!,
            Summary = "プロパティが返す型。null の場合、元となるプロパティと同じ型を返す",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo Accessibility = new()
        {
            Name = nameof(Accessibility),
            Type = typeof(string).FullName!,
            Summary = "setterのアクセス修飾子",
            Remarks = "デフォルト値: \"public\"<br/>\r\n"
                      + "\"NONE\" にすると setter を設けない。",
            DefaultValue = "\"public\"",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Property;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                ReturnType,
                Accessibility,
            };

        private FixedLengthListPropertyAttribute()
        {
        }

        public static FixedLengthListPropertyAttribute Instance { get; } = new();
    }
}
