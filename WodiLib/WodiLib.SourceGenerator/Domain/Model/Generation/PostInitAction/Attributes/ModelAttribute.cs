// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ModelAttribute.cs
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
    ///     モデルクラスを生成する対象に付与する属性
    /// </summary>
    internal class ModelAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(ModelAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "モデルクラスを生成する対象（読取専用クラス）に付与する属性";

        public override string Remarks => @"モデルクラスに必要なメソッドをひととおり定義する。

作成されるのは以下。
<li>
<ul>設定インタフェース</ul>
<ul>設定DTO</ul>
<ul>モデルクラス</ul>
<ul>読取専用モデルクラス（インタフェースの付与と一部メソッドの実装）</ul>
</li>

モデルクラスは以下のインタフェースを自動実装する。（※）のついたインターフェースは abstract クラスの場合は実装しない。
<li>
<ul>INotifyPropertyChanged</ul>
<ul>IEqualityComparable（※）</ul>
<ul>IDeepCloneable（※）</ul>
</li>";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Description = new()
        {
            Name = nameof(Description),
            Type = "string",
            Summary = "クラス説明",
            Remarks = "クラスドキュメントの summary タグに与える文字列。読取専用クラスの場合、"
                      + "\"【読取専用】\"の文字を頭につける。",
            DefaultValue = "",
        };

        public static readonly PropertyInfo BaseModelClass = new()
        {
            Name = nameof(BaseModelClass),
            Type = typeof(Type).FullName!,
            Summary = "読取専用クラスが継承するモデルクラス（デフォルトでは ModelBase を継承する）。",
            Remarks = "\"NONE\" の場合何も継承しない。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo SettingsParameterTypes = new()
        {
            Name = nameof(SettingsParameterTypes),
            Type = typeof(string[]).FullName!,
            Summary = "設定インタフェース/設定DTOに付与する総称型の名前列挙。nullの場合元のクラスと同一。",
            DefaultValue = "null",
        };

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => new[]
            {
                Description,
                BaseModelClass,
                SettingsParameterTypes,
            };

        private ModelAttribute()
        {
        }

        public static ModelAttribute Instance { get; } = new();
    }
}
