// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldDefinitionSettingsDtoTransformHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseFieldDefinitionSettings"/> 拡張クラス
    /// </summary>
    internal static class DatabaseFieldDefinitionSettingsDtoTransformHelper
    {
        /// <summary>
        ///     <see cref="IDatabaseFieldDefinitionListSettings"/> インスタンスを
        ///     <see cref="IDatabaseFieldMetadataListSettings"/> インスタンスに変換する。
        /// </summary>
        /// <param name="definitionListSettings"></param>
        /// <returns></returns>
        public static IDatabaseFieldMetadataListSettings TransformMetadataSettings(
            this IDatabaseFieldDefinitionListSettings definitionListSettings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(definitionListSettings is null, nameof(definitionListSettings));
            return new DatabaseFieldMetadataListSettings(
                definitionListSettings.Settings.Select(TransformMetadataSettings)
                    .ToArray()
            );
        }

        /// <summary>
        ///     <see cref="IDatabaseFieldDefinitionSettings"/> インスタンスを
        ///     <see cref="IDatabaseFieldMetadataSettings"/> インスタンスに変換する。
        /// </summary>
        /// <param name="definitionSettings"></param>
        /// <returns></returns>
        public static IDatabaseFieldMetadataSettings TransformMetadataSettings(
            this IDatabaseFieldDefinitionSettings definitionSettings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(definitionSettings is null, nameof(definitionSettings));

            return new DatabaseFieldMetadataSettings
            {
                FieldName = definitionSettings.FieldName,
                FieldMemo = definitionSettings.FieldMemo,
                SpecialSettingDefinition = definitionSettings.SpecialSettingDefinition,
            };
        }
    }
}
