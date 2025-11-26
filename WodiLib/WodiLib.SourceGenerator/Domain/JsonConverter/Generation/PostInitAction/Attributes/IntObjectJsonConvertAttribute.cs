// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : IntValueObjectJsonConvertAttribute.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;

namespace WodiLib.SourceGenerator.Domain.JsonConverter.Generation.PostInitAction.Attributes
{
    /// <summary>
    ///     IntValueObject のJSONシリアライズ/デシリアライズ情報
    /// </summary>
    internal class IntValueObjectJsonConvertAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(IntValueObjectJsonConvertAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "IntValueObject のJSONシリアライズ/デシリアライズ情報";

        public override bool AllowMultiple => false;

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => Array.Empty<PropertyInfo>();

        private IntValueObjectJsonConvertAttribute()
        {
        }

        public static IntValueObjectJsonConvertAttribute Instance { get; } = new();
    }
}
