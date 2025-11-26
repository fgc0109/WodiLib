// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : StringValueObjectJsonConvertAttribute.cs
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
    ///     StringValueObject のJSONシリアライズ/デシリアライズ情報
    /// </summary>
    internal class StringValueObjectJsonConvertAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(StringValueObjectJsonConvertAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "StringValueObject のJSONシリアライズ/デシリアライズ情報";

        public override bool AllowMultiple => false;

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => Array.Empty<PropertyInfo>();

        private StringValueObjectJsonConvertAttribute()
        {
        }

        public static StringValueObjectJsonConvertAttribute Instance { get; } = new();
    }
}
