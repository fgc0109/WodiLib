// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplateAttribute.cs
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
    ///     テンプレートを用いたリスト実装クラス生成情報
    /// </summary>
    internal class RestrictedCapacityListImplementTemplateAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(RestrictedCapacityListImplementTemplateAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "テンプレートを用いた容量制限のあるリスト実装クラス生成情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Description = new()
        {
            Name = nameof(Description),
            Type = "string",
            Summary = "クラス説明",
            DefaultValue = "",
        };

        public static readonly PropertyInfo ElementType = new()
        {
            Name = nameof(ElementType),
            Type = typeof(Type).FullName!,
            Summary = "リスト要素型（編集可能）",
            DefaultValue = null,
        };

        public static readonly PropertyInfo ReadOnlyElementType = new()
        {
            Name = nameof(ReadOnlyElementType),
            Type = typeof(Type).FullName!,
            Summary = "リスト要素型（読取専用）",
            Remarks = "null の場合 ElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo SettingsType = new()
        {
            Name = nameof(SettingsType),
            Type = typeof(Type).FullName!,
            Summary = "リスト内包型の入力パラメータ型",
            Remarks = "null の場合 ElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo MaxCapacity = new()
        {
            Name = nameof(MaxCapacity),
            Type = $"object",
            Summary = "最大容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true,
        };

        public static readonly PropertyInfo MinCapacity = new()
        {
            Name = nameof(MinCapacity),
            Type = $"object",
            Summary = "最小容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = 0,
        };

        public static readonly PropertyInfo BaseModelClass = new()
        {
            Name = nameof(BaseModelClass),
            Type = typeof(Type).FullName!,
            Summary = "読取専用クラスが継承するモデルクラス（デフォルトでは ModelBase を継承する）",
            Remarks = "\"NONE\" の場合何も継承しない。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo UseConstructorExpansion = new()
        {
            Name = nameof(UseConstructorExpansion),
            Type = typeof(bool).FullName!,
            Summary = "protected virtual partial void DoConstructorExpansion メソッドを定義するか",
            DefaultValue = "false",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Description,
                ElementType,
                ReadOnlyElementType,
                SettingsType,
                MaxCapacity,
                MinCapacity,
                BaseModelClass,
                UseConstructorExpansion,
            };

        private RestrictedCapacityListImplementTemplateAttribute()
        {
        }

        public static RestrictedCapacityListImplementTemplateAttribute Instance { get; } = new();
    }
}
