// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TypeSafeEnumJsonConvertAttribute.cs
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
    ///     TypeSafeEnum のJSONシリアライズ/デシリアライズ情報
    /// </summary>
    internal class TypeSafeEnumJsonConvertAttribute : InitializeAttributeSourceAddable
    {
        /// <inheritdoc/>
        public override string AttributeName => nameof(TypeSafeEnumJsonConvertAttribute);

        /// <inheritdoc/>
        public override string NameSpace => GenerationConst.NameSpaces.Attributes;

        /// <inheritdoc/>
        public override string Summary => "TypeSafeEnum のJSONシリアライズ/デシリアライズ情報";

        public override bool AllowMultiple => false;

        /// <inheritdoc/>
        public override AttributeTargets AttributeTargets
            => AttributeTargets.Class;

        /// <inheritdoc/>
        public override IEnumerable<PropertyInfo> Properties()
            => Array.Empty<PropertyInfo>();

        private TypeSafeEnumJsonConvertAttribute()
        {
        }

        public static TypeSafeEnumJsonConvertAttribute Instance { get; } = new();
    }
}
