// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : SettingsPropertyAttribute.cs
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
    ///     設定インタフェースとして抽出する対象のプロパティに付与する属性
    /// </summary>
    internal class SettingsPropertyAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(SettingsPropertyAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "設定インタフェースとして抽出する対象のプロパティに付与する属性";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo ForceAbstract = new()
        {
            Name = nameof(ForceAbstract),
            Type = typeof(bool).FullName!,
            Summary = "abstract 修飾子を付与することを強制する",
            Remarks = "デフォルト値:false<br/>\r\n"
                      + "対象のモデルクラスが abstract クラスの場合のみ有効",
            DefaultValue = false,
        };

        public static readonly PropertyInfo ForceVirtual = new()
        {
            Name = nameof(ForceVirtual),
            Type = typeof(bool).FullName!,
            Summary = "virtual 修飾子を付与することを強制する",
            Remarks = "デフォルト値:false<br/>",
            DefaultValue = false,
        };

        public static readonly PropertyInfo SetterAccessibility = new()
        {
            Name = nameof(SetterAccessibility),
            Type = typeof(string).FullName!,
            Summary = "setter に付与するアクセス修飾子",
            Remarks = "デフォルト値: \"public\"<br/>\r\n"
                      + "\"NONE\" にすると setter を設けない。",
            DefaultValue = "\"public\"",
        };

        public static readonly PropertyInfo ReturnType = new()
        {
            Name = nameof(ReturnType),
            Type = typeof(Type).FullName!,
            Summary = "プロパティが返す型。null の場合、元となるプロパティと同じ型を返す",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo DefaultValue = new()
        {
            Name = nameof(DefaultValue),
            Type = typeof(string).FullName!,
            Summary = "設定DTOの初期値。null の場合 \"default\" が設定される。",
            DefaultValue = "null",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Property;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                ForceAbstract,
                ForceVirtual,
                SetterAccessibility,
                ReturnType,
                DefaultValue,
            };

        private SettingsPropertyAttribute()
        {
        }

        public static SettingsPropertyAttribute Instance { get; } = new();
    }
}
