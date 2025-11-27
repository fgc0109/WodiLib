// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingDefinitionValidationHelper.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="IDatabaseFieldSpecialSettingDefinition"/> 検証処理Helperクラス
    /// </summary>
    internal static class DatabaseFieldSpecialSettingDefinitionValidationHelper
    {
        /// <summary>
        ///     <see cref="IReadOnlyDatabaseFieldSpecialSettingDefinition"/> と
        ///     <see cref="DatabaseFieldType"/> の関係を引数として検証する。
        /// </summary>
        /// <param name="definitionSettings">DB項目特殊設定</param>
        /// <param name="type">DB項目値種別</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="definitionSettings"/>, <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="definitionSettings"/> が <paramref name="type"/> を受け付ける設定ではない場合。
        /// </exception>
        public static void ValidateDefinitionAndTypeAsArgs(
            NamedValue<IDatabaseFieldSpecialSettingDefinitionSettings> definitionSettings,
            NamedValue<DatabaseFieldType> type
        )
        {
            ThrowHelper.ValidateArgumentNotNull(definitionSettings is null, nameof(definitionSettings));
            ThrowHelper.ValidateArgumentNotNull(definitionSettings.Value is null, nameof(definitionSettings.Value));
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));
            ThrowHelper.ValidateArgumentNotNull(type.Value is null, nameof(type.Value));

            // 受付可否の判定を DatabaseFieldSpecialSettingDefinition に定義しているため、
            // 一旦判定用の DatabaseFieldSpecialSettingDefinition を作成
            var definition = DatabaseFieldSpecialSettingDefinitionFactory.Create(definitionSettings.Value);

            ValidateDefinitionAndTypeAsArgs(
                new NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition>(definitionSettings.Name, definition),
                type
            );
        }

        /// <summary>
        ///     <see cref="IReadOnlyDatabaseFieldSpecialSettingDefinition"/> と
        ///     <see cref="DatabaseFieldType"/> の関係を引数として検証する。
        /// </summary>
        /// <param name="definition">DB項目特殊設定</param>
        /// <param name="type">DB項目値種別</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="definition"/>, <paramref name="type"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="definition"/> が <paramref name="type"/> を受け付ける設定ではない場合。
        /// </exception>
        public static void ValidateDefinitionAndTypeAsArgs(
            NamedValue<IReadOnlyDatabaseFieldSpecialSettingDefinition> definition,
            NamedValue<DatabaseFieldType> type
        )
        {
            ThrowHelper.ValidateArgumentNotNull(definition is null, nameof(definition));
            ThrowHelper.ValidateArgumentNotNull(definition.Value is null, nameof(definition.Value));
            ThrowHelper.ValidateArgumentNotNull(type is null, nameof(type));
            ThrowHelper.ValidateArgumentNotNull(type.Value is null, nameof(type.Value));

            ThrowHelper.InvalidOperationIf(
                !definition.Value.CanChangeFieldType(type.Value),
                () => $"{definition.Name} は {type.Value.Id} を受け付ける設定ではないため"
            );
        }
    }
}
