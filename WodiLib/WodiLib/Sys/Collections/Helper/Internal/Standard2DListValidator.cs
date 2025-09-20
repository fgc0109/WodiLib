// ========================================
// Project Name : WodiLib
// File Name    : Standard2DListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     二次元リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TRowElementSettings">行要素設定型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定型</typeparam>
    internal class Standard2DListValidator<TRowElementSettings, TListElementSettings>
        : IWodiLib2DListValidator<TRowElementSettings, TListElementSettings>
        where TRowElementSettings : IListSettings<TListElementSettings>
    {
        public delegate int GetRowCountDelegate();

        public delegate int GetColumnCountDelegate();

        protected GetRowCountDelegate RowCountGetter { get; }
        protected GetColumnCountDelegate ColumnCountGetter { get; }

        public Standard2DListValidator(
            GetRowCountDelegate rowCountGetter,
            GetColumnCountDelegate columnCountGetter
        )
        {
            RowCountGetter = rowCountGetter;
            ColumnCountGetter = columnCountGetter;
        }

        public virtual void Constructor(NamedValue<IEnumerable<TRowElementSettings>> initItems)
        {
            ThrowHelper.ValidateArgumentNotNull(initItems.Value is null, initItems.Name);
            TwoDListValidationHelper.ItemNotNull<TRowElementSettings, TListElementSettings>(initItems);
            TwoDListValidationHelper.ItemsNotJag<TRowElementSettings, TListElementSettings>(initItems.Value);
        }

        public virtual void GetRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());

            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ListValidationHelper.Count(count, namedRowCount);
            ListValidationHelper.Range(rowIndex, count, namedRowCount);
        }

        public virtual void GetColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ListValidationHelper.Count(count, namedColumnCount);
            ListValidationHelper.Range(columnIndex, count, namedColumnCount);
        }

        public virtual void GetCell(NamedValue<int> rowIndex, NamedValue<int> columnIndex)
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
        }

        public virtual void SetRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());

            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull<TRowElementSettings, TListElementSettings>(settings);
            TwoDListValidationHelper.ColumnSizeEqual<TRowElementSettings, TListElementSettings>(
                settings,
                RowCountGetter(),
                ColumnCountGetter()
            );
            ListValidationHelper.Range(
                rowIndex,
                ($"{nameof(settings)}の要素数", settings.Value.Count()),
                namedRowCount
            );
        }

        public virtual void SetColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull(settings);
            TwoDListValidationHelper.RowSizeEqual(
                settings,
                RowCountGetter()
            );
            ListValidationHelper.Range(
                columnIndex,
                ($"{nameof(settings)}の要素数", settings.Value.Count()),
                namedColumnCount
            );
        }

        public virtual void SetCell(
            NamedValue<int> rowIndex,
            NamedValue<int> columnIndex,
            NamedValue<TListElementSettings> settings
        )
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
        }

        public virtual void InsertRow(NamedValue<int> rowIndex, NamedValue<IEnumerable<TRowElementSettings>> settings)
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());

            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull<TRowElementSettings, TListElementSettings>(settings);
            TwoDListValidationHelper.ColumnSizeEqual<TRowElementSettings, TListElementSettings>(
                settings,
                RowCountGetter(),
                ColumnCountGetter()
            );
            ListValidationHelper.InsertIndex(rowIndex, namedRowCount);
        }

        public virtual void InsertColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            TwoDListValidationHelper.ColumnSizeNotZero(namedColumnCount);

            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull(settings);
            TwoDListValidationHelper.RowSizeEqual(
                settings,
                RowCountGetter()
            );
            ListValidationHelper.InsertIndex(columnIndex, namedColumnCount);
        }

        public virtual void OverwriteRow(
            NamedValue<int> rowIndex,
            NamedValue<IEnumerable<TRowElementSettings>> settings
        )
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());

            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull<TRowElementSettings, TListElementSettings>(settings);
            TwoDListValidationHelper.ColumnSizeEqual<TRowElementSettings, TListElementSettings>(
                settings,
                RowCountGetter(),
                ColumnCountGetter()
            );
            ListValidationHelper.InsertIndex(rowIndex, namedRowCount);
        }

        public virtual void OverwriteColumn(
            NamedValue<int> columnIndex,
            NamedValue<IEnumerable<IEnumerable<TListElementSettings>>> settings
        )
        {
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            TwoDListValidationHelper.ColumnSizeNotZero(namedColumnCount);

            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull(settings);
            TwoDListValidationHelper.RowSizeEqual(
                settings,
                RowCountGetter()
            );
            ListValidationHelper.InsertIndex(columnIndex, namedColumnCount);
        }

        public virtual void MoveRow(NamedValue<int> oldRowIndex, NamedValue<int> newRowIndex, NamedValue<int> count)
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());

            ListValidationHelper.ItemCountNotZero(namedRowCount);
            ListValidationHelper.SelectIndex(oldRowIndex, namedRowCount);
            ListValidationHelper.InsertIndex(newRowIndex, namedRowCount);
            ListValidationHelper.Count(count, namedRowCount);
            ListValidationHelper.Range(oldRowIndex, count, namedRowCount);
            ListValidationHelper.Range(count, newRowIndex, namedRowCount);
        }

        public virtual void MoveColumn(
            NamedValue<int> oldColumnIndex,
            NamedValue<int> newColumnIndex,
            NamedValue<int> count
        )
        {
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            ListValidationHelper.ItemCountNotZero(namedColumnCount);
            ListValidationHelper.SelectIndex(oldColumnIndex, namedColumnCount);
            ListValidationHelper.InsertIndex(newColumnIndex, namedColumnCount);
            ListValidationHelper.Count(count, namedColumnCount);
            ListValidationHelper.Range(oldColumnIndex, count, namedColumnCount);
            ListValidationHelper.Range(count, newColumnIndex, namedColumnCount);
        }

        public virtual void RemoveRow(NamedValue<int> rowIndex, NamedValue<int> count)
        {
            var namedRowCount = new NamedValue<int>("RowCount", RowCountGetter.Invoke());

            ListValidationHelper.SelectIndex(rowIndex, namedRowCount);
            ListValidationHelper.Count(count, namedRowCount);
            ListValidationHelper.Range(rowIndex, count, namedRowCount);
        }

        public virtual void RemoveColumn(NamedValue<int> columnIndex, NamedValue<int> count)
        {
            var namedColumnCount = new NamedValue<int>("ColumnCount", ColumnCountGetter.Invoke());

            ListValidationHelper.SelectIndex(columnIndex, namedColumnCount);
            ListValidationHelper.Count(count, namedColumnCount);
            ListValidationHelper.Range(columnIndex, count, namedColumnCount);
        }

        public virtual void AdjustRowLength(NamedValue<int> length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < 0,
                nameof(length),
                0,
                length.Value
            );
        }

        public virtual void AdjustColumnLength(NamedValue<int> length)
        {
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < 0,
                nameof(length),
                0,
                length.Value
            );
        }

        public virtual void Reset(NamedValue<IEnumerable<TRowElementSettings>> settings, bool canChangeSize = true)
        {
            ThrowHelper.ValidateArgumentNotNull(settings.Value is null, settings.Name);
            TwoDListValidationHelper.ItemNotNull<TRowElementSettings, TListElementSettings>(settings);

            if (canChangeSize)
            {
                TwoDListValidationHelper.ItemsNotJag<TRowElementSettings, TListElementSettings>(settings.Value);
            }
            else
            {
                TwoDListValidationHelper.SizeEqual<TRowElementSettings, TListElementSettings>(
                    settings,
                    RowCountGetter(),
                    ColumnCountGetter()
                );
            }
        }

        public virtual void Reset()
        {
            // 無条件で可能
        }

        public virtual void Clear()
        {
            // 無条件で可能
        }
    }
}
