// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacity2DListImplementTemplateAttribute.cs
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
    ///     テンプレートを用いた二次元リスト実装クラス生成情報
    /// </summary>
    internal class RestrictedCapacity2DListImplementTemplateAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(RestrictedCapacity2DListImplementTemplateAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "テンプレートを用いた容量制限のある二次元リスト実装クラス生成情報";

        public override bool AllowMultiple => false;

        public static readonly PropertyInfo Description = new()
        {
            Name = nameof(Description),
            Type = "string",
            Summary = "クラス説明",
            DefaultValue = "",
        };

        public static readonly PropertyInfo RowElementType = new()
        {
            Name = nameof(RowElementType),
            Type = typeof(Type).FullName!,
            Summary = "リスト行要素型（編集可能）",
            DefaultValue = null,
        };

        public static readonly PropertyInfo FixedRowElementType = new()
        {
            Name = nameof(FixedRowElementType),
            Type = typeof(Type).FullName!,
            Summary = "リスト行要素型（容量固定）",
            Remarks = "null の場合 RowElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo ReadOnlyRowElementType = new()
        {
            Name = nameof(ReadOnlyRowElementType),
            Type = typeof(Type).FullName!,
            Summary = "リスト行要素型（読取専用）",
            Remarks = "null の場合 RowElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo CellElementType = new()
        {
            Name = nameof(CellElementType),
            Type = typeof(Type).FullName!,
            Summary = "リストセル要素型（編集可能）",
            DefaultValue = null,
        };

        public static readonly PropertyInfo ReadOnlyCellElementType = new()
        {
            Name = nameof(ReadOnlyCellElementType),
            Type = typeof(Type).FullName!,
            Summary = "リストセル要素型（読取専用）",
            Remarks = "null の場合 ElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo RowSettingsType = new()
        {
            Name = nameof(RowSettingsType),
            Type = typeof(Type).FullName!,
            Summary = "リスト行の入力パラメータ型",
            Remarks = "null の場合 RowElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo CellSettingsType = new()
        {
            Name = nameof(CellSettingsType),
            Type = typeof(Type).FullName!,
            Summary = "リストセルの入力パラメータ型",
            Remarks = "null の場合 CellElementType と同じ。",
            DefaultValue = null,
        };

        public static readonly PropertyInfo MaxRowCapacity = new()
        {
            Name = nameof(MaxRowCapacity),
            Type = $"object",
            Summary = "行最大容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true,
        };

        public static readonly PropertyInfo MinRowCapacity = new()
        {
            Name = nameof(MinRowCapacity),
            Type = $"object",
            Summary = "行最小容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = 0,
        };

        public static readonly PropertyInfo MaxColumnCapacity = new()
        {
            Name = nameof(MaxColumnCapacity),
            Type = $"object",
            Summary = "列最大容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = "int.MaxValue",
            DefaultValueAsSourceCode = true,
        };

        public static readonly PropertyInfo MinColumnCapacity = new()
        {
            Name = nameof(MinColumnCapacity),
            Type = $"object",
            Summary = "列最小容量",
            Remarks = "与えた値を ToString した結果をソースコードとして埋め込む。",
            DefaultValue = 0,
        };

        public static readonly PropertyInfo BaseModelClass = new()
        {
            Name = nameof(BaseModelClass),
            Type = typeof(Type).FullName!,
            Summary = "読取専用クラスが継承するモデルクラス（デフォルトでは ModelBase を継承する）。",
            Remarks = "\"NONE\" の場合何も継承しない。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo RowPhysicalName = new()
        {
            Name = nameof(RowPhysicalName),
            Type = "string",
            Summary = "\"行\"の物理名（デフォルト値：\"Row\"）",
            Remarks = "出力されるソースのメソッド名・引数名に使用する。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo RowLogicalName = new()
        {
            Name = nameof(RowLogicalName),
            Type = "string",
            Summary = "\"行\"の論理名（デフォルト値：\"行\"）",
            Remarks = "出力されるソースのドキュメントコメントに使用する。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo ColumnPhysicalName = new()
        {
            Name = nameof(ColumnPhysicalName),
            Type = "string",
            Summary = "\"列\"の物理名（デフォルト値：\"Column\"）",
            Remarks = "出力されるソースのメソッド名・引数名に使用する。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo ColumnLogicalName = new()
        {
            Name = nameof(ColumnLogicalName),
            Type = "string",
            Summary = "\"列\"の論理名（デフォルト値：\"列\"）",
            Remarks = "出力されるソースのドキュメントコメントに使用する。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo CellPhysicalName = new()
        {
            Name = nameof(CellPhysicalName),
            Type = "string",
            Summary = "\"セル\"の物理名（デフォルト値：\"Cell\"）",
            Remarks = "出力されるソースのメソッド名・引数名に使用する。",
            DefaultValue = "null",
        };

        public static readonly PropertyInfo CellLogicalName = new()
        {
            Name = nameof(CellLogicalName),
            Type = "string",
            Summary = "\"セル\"の論理名（デフォルト値：\"セル\"）",
            Remarks = "出力されるソースのドキュメントコメントに使用する。",
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
                RowElementType,
                FixedRowElementType,
                ReadOnlyRowElementType,
                CellElementType,
                ReadOnlyCellElementType,
                RowSettingsType,
                CellSettingsType,
                MaxRowCapacity,
                MinRowCapacity,
                MaxColumnCapacity,
                MinColumnCapacity,
                BaseModelClass,
                RowPhysicalName,
                RowLogicalName,
                ColumnPhysicalName,
                ColumnLogicalName,
                CellPhysicalName,
                CellLogicalName,
            };

        private RestrictedCapacity2DListImplementTemplateAttribute()
        {
        }

        public static RestrictedCapacity2DListImplementTemplateAttribute Instance { get; } = new();
    }
}
