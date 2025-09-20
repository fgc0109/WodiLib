// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MutableMethodAttribute.cs
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
    ///     編集可能モデルクラスとして抽出する対象のメソッドに付与する属性
    /// </summary>
    internal class MutableMethodAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(MutableMethodAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "編集可能モデルクラスとして抽出する対象のメソッドに付与する属性";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo ReturnType = new()
        {
            Name = nameof(ReturnType),
            Type = typeof(Type).FullName!,
            Summary = "メソッドが返す型。null の場合、元となるプロパティと同じ型を返す",
            Remarks = "指定する場合、"
                      + "読取専用クラスが返却するインスタンスが as 演算子により"
                      + "変換できることが必須条件となる。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo Accessibility = new()
        {
            Name = nameof(Accessibility),
            Type = typeof(string).FullName!,
            Summary = "アクセス修飾子",
            Remarks = "デフォルト値: \"public\"",
            DefaultValue = "\"public\"",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Method;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                ReturnType,
                Accessibility,
            };

        private MutableMethodAttribute()
        {
        }

        public static MutableMethodAttribute Instance { get; } = new();
    }
}
