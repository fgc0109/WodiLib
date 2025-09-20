// ========================================
// Project Name : WodiLib
// File Name    : RestrictedCapacity2DListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限あり二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
    internal class RestrictedCapacity2DListValidator<TRowElementSettings, TListElementSettings>
        : Standard2DListValidator<TRowElementSettings, TListElementSettings>
        where TRowElementSettings : IListSettings<TListElementSettings>
    {
        public delegate int GetMinRowCapacityDelegate();

        public delegate int GetMaxRowCapacityDelegate();

        public delegate int GetMinColumnCapacityDelegate();

        public delegate int GetMaxColumnCapacityDelegate();

        private static string RowItemsName => "行数";
        private static string ColumnItemsName => "列数";

        protected GetMinRowCapacityDelegate MinRowCapacityGetter { get; }
        protected GetMaxRowCapacityDelegate MaxRowCapacityGetter { get; }
        protected GetMinColumnCapacityDelegate MinColumnCapacityGetter { get; }
        protected GetMaxColumnCapacityDelegate MaxColumnCapacityGetter { get; }

        public RestrictedCapacity2DListValidator(
            GetRowCountDelegate rowCountGetter,
            GetColumnCountDelegate columnCountGetter,
            GetMinRowCapacityDelegate minRowCapacityGetter,
            GetMaxRowCapacityDelegate maxRowCapacityGetter,
            GetMinColumnCapacityDelegate minColumnCapacityGetter,
            GetMaxColumnCapacityDelegate maxColumnCapacityGetter
        )
            : base(rowCountGetter, columnCountGetter)
        {
            MinRowCapacityGetter = minRowCapacityGetter;
            MaxRowCapacityGetter = maxRowCapacityGetter;
            MinColumnCapacityGetter = minColumnCapacityGetter;
            MaxColumnCapacityGetter = maxColumnCapacityGetter;
        }

        public override void Constructor(NamedValue<IEnumerable<TRowElementSettings>> initItems)
        {
            base.Constructor(initItems);

            var maxRowCapacity = MaxRowCapacityGetter.Invoke();
            var minRowCapacity = MinRowCapacityGetter.Invoke();
            var maxColumnCapacity = MaxColumnCapacityGetter.Invoke();
            var minColumnCapacity = MinColumnCapacityGetter.Invoke();

#if DEBUG
            try
            {
                RestrictedCapacity2DListValidationHelper.CapacityConfig(
                    ($"GetMinRowCapacity", minRowCapacity),
                    ($"GetMaxRowCapacity", maxRowCapacity),
                    ($"GetMinColumnCapacity", minColumnCapacity),
                    ($"GetMaxColumnCapacity", maxColumnCapacity)
                );
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(GetType().Name, ex);
            }
#endif

            RestrictedCapacity2DListValidationHelper.RowAndColCount<TRowElementSettings, TListElementSettings>(
                initItems.Value,
                minRowCapacity,
                maxRowCapacity,
                minColumnCapacity,
                maxColumnCapacity,
                initItems.Name
            );
        }

        public override void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            base.InsertRow(rowIndex, settings);

            RestrictedCapacity2DListValidationHelper.ItemMaxCount(
                RowCountGetter.Invoke() + settings.Value.Count(),
                MaxRowCapacityGetter.Invoke(),
                "行"
            );
        }

        public override void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            base.InsertColumn(columnIndex, settings);

            RestrictedCapacity2DListValidationHelper.ItemMaxCount(
                ColumnCountGetter.Invoke() + settings.Value.Count(),
                MaxColumnCapacityGetter.Invoke(),
                "列"
            );
        }

        public override void OverwriteRow(
            NamedValue<int> rowIndex,
            NamedValue<IEnumerable<TRowElementSettings>> settings
        )
        {
            base.OverwriteRow(rowIndex, settings);

            RestrictedCapacityListValidationHelper.OverwrittenCount(
                rowIndex.Value,
                settings.Value.Count(),
                RowCountGetter.Invoke(),
                MaxRowCapacityGetter.Invoke()
            );
        }

        public override void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            base.OverwriteColumn(columnIndex, settings);

            RestrictedCapacityListValidationHelper.OverwrittenCount(
                columnIndex.Value,
                settings.Value.Count(),
                ColumnCountGetter.Invoke(),
                MaxColumnCapacityGetter.Invoke()
            );
        }

        public override void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            base.RemoveRow(rowIndex, count);

            RestrictedCapacity2DListValidationHelper.ItemMinCount(
                RowCountGetter.Invoke() - count.Value,
                MinRowCapacityGetter.Invoke(),
                "行"
            );
        }

        public override void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            base.RemoveColumn(columnIndex, count);

            RestrictedCapacity2DListValidationHelper.ItemMinCount(
                ColumnCountGetter.Invoke() - count.Value,
                MinColumnCapacityGetter.Invoke(),
                "列"
            );
        }

        public override void AdjustRowLength(NamedValue<int> length)
        {
            var minRowCapacity = MinRowCapacityGetter.Invoke();
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < minRowCapacity,
                length.Name,
                minRowCapacity,
                length.Value
            );

            var maxRowCapacity = MaxRowCapacityGetter.Invoke();
            ThrowHelper.ValidateArgumentValueLessOrEqual(
                length.Value > maxRowCapacity,
                length.Name,
                maxRowCapacity,
                length.Value
            );
        }

        public override void AdjustColumnLength(NamedValue<int> length)
        {
            var minColumnCapacity = MinColumnCapacityGetter.Invoke();
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < minColumnCapacity,
                length.Name,
                minColumnCapacity,
                length.Value
            );

            var maxColumnCapacity = MaxColumnCapacityGetter.Invoke();
            ThrowHelper.ValidateArgumentValueLessOrEqual(
                length.Value > maxColumnCapacity,
                length.Name,
                maxColumnCapacity,
                length.Value
            );
        }

        public override void Reset(NamedValue<IEnumerable<TRowElementSettings>> settings, bool canChangeSize = true)
        {
            base.Reset(settings, canChangeSize);

            var rowCount = settings.Value.Count();
            var columnCount = rowCount > 0
                ? settings.Value.First().Settings.Count
                : 0;

            RestrictedCapacityListValidationHelper.ArgumentItemsCount(
                rowCount,
                MinRowCapacityGetter.Invoke(),
                MaxRowCapacityGetter.Invoke(),
                RowItemsName
            );

            RestrictedCapacityListValidationHelper.ArgumentItemsCount(
                columnCount,
                MinColumnCapacityGetter.Invoke(),
                MaxColumnCapacityGetter.Invoke(),
                ColumnItemsName
            );
        }
    }
}
