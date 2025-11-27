// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldMetadataSettingsDtoTransformHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseFieldMetadataSettings"/> 拡張クラス
    /// </summary>
    internal static class DatabaseFieldMetadataSettingsDtoTransformHelper
    {
        /// <summary>
        ///     <see cref="IDatabaseFieldMetadataSettings"/> インスタンスを
        ///     <see cref="IDatabaseFieldDefinitionSettings"/> インスタンスに変換する。
        /// </summary>
        /// <param name="definitionSettings"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        public static IDatabaseFieldDefinitionSettings TransformMetadataSettings(
            this IDatabaseFieldMetadataSettings definitionSettings,
            DatabaseFieldType fieldType
        )
        {
            ThrowHelper.ValidateArgumentNotNull(definitionSettings is null, nameof(definitionSettings));
            ThrowHelper.ValidateArgumentNotNull(fieldType is null, nameof(fieldType));

            return new DatabaseFieldDefinitionSettings
            {
                FieldName = definitionSettings.FieldName,
                FieldMemo = definitionSettings.FieldMemo,
                SpecialSettingDefinition = definitionSettings.SpecialSettingDefinition,
                FieldType = fieldType,
            };
        }
    }
}
