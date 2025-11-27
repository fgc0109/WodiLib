// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Database
{
    /// <summary>
    ///     DBテーブル系クラス 引数検証クラス
    /// </summary>
    /// <remarks>
    ///     このクラスはデータのみを扱うテーブルで使用する検証処理のみを定義する。<br/>
    ///     DBタイプの他の設定と絡めて検証処理を行う必要がある場合はこのクラスを継承して利用する。
    /// </remarks>
    /// <typeparam name="TListSettings">リストの入力パラメータ型</typeparam>
    /// <typeparam name="TRowElementSettings">データ設定型</typeparam>
    internal class DatabaseDataTableValidator<TListSettings, TRowElementSettings> :
        RestrictedCapacity2DListValidator<TListSettings, TRowElementSettings, DatabaseFieldValue>
        where TListSettings : IListSettings<TRowElementSettings>
        where TRowElementSettings : IListSettings<DatabaseFieldValue>
    {
        public delegate DatabaseFieldType[] GetFieldTypesDelegate();

        protected GetFieldTypesDelegate FieldTypesGetter { get; }

        public DatabaseDataTableValidator(
            GetRowCountDelegate rowCountGetter,
            GetColumnCountDelegate columnCountGetter,
            GetFieldTypesDelegate fieldTypesGetter
        )
            : base(
                rowCountGetter,
                columnCountGetter,
                minRowCapacityGetter: () => DatabaseConst.MinDataLength,
                maxRowCapacityGetter: () => DatabaseConst.MaxDataLength,
                minColumnCapacityGetter: () => DatabaseConst.MinFieldLength,
                maxColumnCapacityGetter: () => DatabaseConst.MaxFieldLength,
                rowItemsName: "データ",
                columnItemsName: "項目"
            )
        {
            FieldTypesGetter = fieldTypesGetter;
        }

        public override void Constructor(NamedValue<TListSettings> initItems)
        {
            base.Constructor(initItems);

            DatabaseDataTableValidationHelper.ValidateItemType(
                Convert2DArrayValues(initItems.Value.Settings),
                Direction.Row
            );
        }

        public override void SetRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            base.SetRow(rowIndex, settings);

            DatabaseDataTableValidationHelper.ValidateItemType(
                Convert2DArrayValues(settings.Value),
                FieldTypesGetter.Invoke(),
                Direction.Row
            );
        }

        /// <inheritdoc/>
        /// <remarks>
        ///     列方向の値種別が統一されていれば、処理対象となるれるの現在の値種別と一致していなくても検証エラーとはしない。
        /// </remarks>
        public override void SetColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<DatabaseFieldValue>>> settings
        )
        {
            base.SetColumn(columnIndex, settings);

            DatabaseDataTableValidationHelper.ValidateItemType(
                settings.Value.To2DArray(),
                Direction.Column
            );
        }

        public override void SetCell(
            NamedValue<int> rowIndex,
            NamedValue<int> columnIndex,
            NamedValue<DatabaseFieldValue> item
        )
        {
            base.SetCell(rowIndex, columnIndex, item);

            var fieldType = GetRelevantFieldType(FieldTypesGetter, columnIndex.Value);
            DatabaseFieldValueValidationHelper.ValidateMatchFieldType(item, fieldType);
        }

        public override void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            base.InsertRow(rowIndex, settings);

            DatabaseDataTableValidationHelper.ValidateItemType(
                Convert2DArrayValues(settings.Value),
                FieldTypesGetter.Invoke(),
                Direction.Row
            );
        }

        public override void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<DatabaseFieldValue>>> settings
        )
        {
            base.InsertColumn(columnIndex, settings);

            DatabaseDataTableValidationHelper.ValidateItemType(
                settings.Value.To2DArray(),
                Direction.Column
            );
        }

        public override void OverwriteRow(
            NamedValue<int> rowIndex,
            NamedValue<IEnumerable<TRowElementSettings>> settings
        )
        {
            base.OverwriteRow(rowIndex, settings);

            DatabaseDataTableValidationHelper.ValidateItemType(
                Convert2DArrayValues(settings.Value),
                FieldTypesGetter.Invoke(),
                Direction.Row
            );
        }

        public override void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<DatabaseFieldValue>>> settings
        )
        {
            base.OverwriteColumn(columnIndex, settings);

            DatabaseDataTableValidationHelper.ValidateItemType(
                settings.Value.To2DArray(),
                Direction.Column
            );
        }

        public override void Reset(NamedValue<IEnumerable<TRowElementSettings>> settings, bool canChangeSize = true)
        {
            base.Reset(settings, canChangeSize);

            DatabaseDataTableValidationHelper.ValidateItemType(
                Convert2DArrayValues(settings.Value),
                Direction.Row
            );
        }

        private static DatabaseFieldValue[][] Convert2DArrayValues(IEnumerable<TRowElementSettings> rows)
        {
            return rows.Select(row => row.Settings)
                .To2DArray();
        }

        private static DatabaseFieldType GetRelevantFieldType(
            GetFieldTypesDelegate fieldTypesGetter,
            int columnIndex
        ) => fieldTypesGetter.Invoke()[columnIndex];
    }
}
