// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : MutableConstructorAttribute.cs
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
    ///     編集可能モデルクラスのコンストラクタとして抽出する対象のコンストラクタに付与する属性
    /// </summary>
    internal class MutableConstructorAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(MutableConstructorAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary
            => "編集可能モデルクラスのコンストラクタとして抽出する対象のコンストラクタに付与する属性";

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Constructor;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => Array.Empty<PropertyInfo>();

        private MutableConstructorAttribute()
        {
        }

        public static MutableConstructorAttribute Instance { get; } = new();
    }
}
