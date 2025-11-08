// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeTableValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     <see cref="DatabaseTypeTable"/> のバリデーションクラス
    /// </summary>
    internal class DatabaseTypeTableValidator :
        DatabaseDataTableValidator<IDatabaseTypeTableSettings, IDatabaseNamedDataRowSettings>
    {
        /// <summary>
        ///     指定されたフィールドIDのフィールドタイプを変更可能かどうかを判定するデリゲート
        /// </summary>
        /// <param name="fieldId">判定対象のフィールドID</param>
        /// <param name="type">変更しようとするタイプ</param>
        /// <returns>フィールドタイプの変更可否</returns>
        public delegate bool DatabaseFieldTypeChangeValidator(FieldId fieldId, DatabaseFieldType type);

        protected DatabaseFieldTypeChangeValidator FieldTypeChangeValidator { get; }

        public DatabaseTypeTableValidator(
            GetRowCountDelegate rowCountGetter,
            GetColumnCountDelegate columnCountGetter,
            GetFieldTypesDelegate fieldTypesGetter,
            DatabaseFieldTypeChangeValidator fieldTypeChangeValidator
        ) : base(rowCountGetter, columnCountGetter, fieldTypesGetter)
        {
            FieldTypeChangeValidator = fieldTypeChangeValidator;
        }

        public override void SetColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<DatabaseFieldValue>>> settings
        )
        {
            base.SetColumn(columnIndex, settings);

            var values = settings.Value.To2DArray();
            var columnLength = values.Length;

            if (values.Length == 0) return;

            for (var c = 0; c < columnLength; c++)
            {
                FieldTypeChangeValidator.Invoke(columnIndex.Value + c, values[0][c].Type);
            }
        }

        public override void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<DatabaseFieldValue>>> settings
        )
        {
            base.OverwriteColumn(columnIndex, settings);

            var values = settings.Value.To2DArray();
            var replaceColumnLength = ColumnCountGetter.Invoke() - columnIndex.Value;

            for (var c = 0; c < replaceColumnLength; c++)
            {
                FieldTypeChangeValidator.Invoke(columnIndex.Value + c, values[0][c].Type);
            }
        }
    }
}
