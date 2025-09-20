// ========================================
// Project Name : WodiLib
// File Name    : FixedLength2DList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

// ReSharper disable ConditionalAccessQualifierIsNonNullableAccordingToAPIContract

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量固定の二次元リスト
    /// </summary>
    /// <typeparam name="TEditableRowElement">行要素型</typeparam>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TReadOnlyRowElement">行要素読取専用型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TEditableListElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyListElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal class FixedLength2DList<
        TEditableRowElement,
        TFixedRowElement,
        TReadOnlyRowElement,
        TRowElementSettings,
        TEditableListElement,
        TReadOnlyListElement,
        TListElementSettings
    > : ReadOnly2DList<TEditableRowElement, TFixedRowElement, TReadOnlyRowElement, TRowElementSettings,
            TEditableListElement, TReadOnlyListElement, TListElementSettings>,
        IFixedLength2DList<TFixedRowElement, TReadOnlyRowElement, TRowElementSettings, TEditableListElement,
            TReadOnlyListElement, TListElementSettings>
        where TEditableRowElement : TFixedRowElement
        where TFixedRowElement : TReadOnlyRowElement
        where TReadOnlyRowElement : TRowElementSettings, INotifyPropertyChanged, INotifyCollectionChanged
        where TRowElementSettings : notnull
        where TEditableListElement : TReadOnlyListElement
        where TReadOnlyListElement : TListElementSettings
        where TListElementSettings : notnull
    {
        #region Properties

        /// <inheritdoc/>
        public new TFixedRowElement this[int rowIndex]
        {
            get => GetRow(rowIndex);
            set => SetRow(rowIndex, value);
        }

        /// <inheritdoc/>
        public new TEditableListElement this[int rowIndex, int columnIndex]
        {
            get => GetCell(rowIndex, columnIndex);
            set => SetCell(rowIndex, columnIndex, value);
        }

        /// <summary>すべての編集可能型行要素</summary>
        public TFixedRowElement[] EditableRows => Items.Cast<TFixedRowElement>().ToArray();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <inheritdoc/>
        internal FixedLength2DList(SimpleList<TEditableRowElement> itemsImpl, Config config) : base(
            itemsImpl,
            config
        )
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region CRUD

        /// <inheritdoc/>
        public new TFixedRowElement GetRow(int rowIndex)
        {
            ValidateGetRow(rowIndex);
            return GetRowInternal(rowIndex);
        }

        /// <inheritdoc/>
        public new IEnumerable<TFixedRowElement> GetRowRange(int rowIndex, int count)
        {
            ValidateGetRowRange(rowIndex, count);
            return GetRowRangeInternal(rowIndex, count);
        }

        /// <inheritdoc/>
        public new TEditableListElement GetCell(int rowIndex, int columnIndex)
        {
            ValidateGetCell(rowIndex, columnIndex);
            return GetCellInternal(rowIndex, columnIndex);
        }

        /// <inheritdoc/>
        public TFixedRowElement SetRow(int rowIndex, TRowElementSettings settings)
        {
            ValidateSetRow(rowIndex, settings);
            return SetRowInternal(rowIndex, settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> SetRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateSetRowRange(rowIndex, settingsArray);
            return SetRowRangeInternal(rowIndex, settingsArray);
        }

        /// <inheritdoc/>
        public TEditableListElement SetCell(int rowIndex, int columnIndex, TListElementSettings settings)
        {
            ValidateSetCell(rowIndex, columnIndex, settings);
            return SetCellInternal(rowIndex, columnIndex, settings);
        }

        /// <inheritdoc/>
        public void MoveRow(int oldRowIndex, int newRowIndex)
        {
            ValidateMoveRow(oldRowIndex, newRowIndex);
            MoveRowInternal(oldRowIndex, newRowIndex);
        }

        /// <inheritdoc/>
        public void MoveRowRange(int oldRowIndex, int newRowIndex, int count)
        {
            ValidateMoveRowRange(oldRowIndex, newRowIndex, count);
            MoveRowRangeInternal(oldRowIndex, newRowIndex, count);
        }

        /// <inheritdoc/>
        public new IEnumerable<TEditableListElement> GetColumn(int columnIndex)
        {
            ValidateGetColumn(columnIndex);
            return GetColumnInternal(columnIndex);
        }

        /// <inheritdoc/>
        public new IEnumerable<IEnumerable<TEditableListElement>> GetColumnRange(int columnIndex, int count)
        {
            ValidateGetColumnRange(columnIndex, count);
            return GetColumnRangeInternal(columnIndex, count);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> SetColumn(int columnIndex, IEnumerable<TListElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateSetColumn(columnIndex, settingsArray);
            return SetColumnInternal(columnIndex, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> SetColumnRange(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        )
        {
            var settingsArray = settings?.To2DArray() ?? null!;

            ValidateSetColumnRange(columnIndex, settingsArray);
            return SetColumnRangeInternal(columnIndex, settingsArray);
        }

        /// <inheritdoc/>
        public void MoveColumn(int oldColumnIndex, int newColumnIndex)
        {
            ValidateMoveColumn(oldColumnIndex, newColumnIndex);
            MoveColumnInternal(oldColumnIndex, newColumnIndex);
        }

        /// <inheritdoc/>
        public void MoveColumnRange(int oldColumnIndex, int newColumnIndex, int count)
        {
            ValidateMoveColumnRange(oldColumnIndex, newColumnIndex, count);
            MoveColumnRangeInternal(oldColumnIndex, newColumnIndex, count);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> Reset(IEnumerable<TRowElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateReset(settingsArray);
            return ResetInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> Reset()
        {
            ValidateReset();
            return ResetInternal();
        }

        #endregion

        #region Validation

        /// <inheritdoc/>
        public void ValidateSetRow(int rowIndex, TRowElementSettings settings)
            => Validator?.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateSetRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings)
            => Validator?.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateSetColumn(int columnIndex, IEnumerable<TListElementSettings> settings)
            => Validator?.SetColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateSetColumnRange(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.SetColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateSetCell(int rowIndex, int columnIndex, TListElementSettings settings)
            => Validator?.SetCell(
                (nameof(rowIndex), rowIndex),
                (nameof(columnIndex), columnIndex),
                (nameof(settings), settings)
            );

        /// <inheritdoc/>
        public void ValidateMoveRow(int oldRowIndex, int newRowIndex)
            => Validator?.MoveRow((nameof(oldRowIndex), oldRowIndex), (nameof(newRowIndex), newRowIndex));

        /// <inheritdoc/>
        public void ValidateMoveRowRange(int oldRowIndex, int newRowIndex, int count)
            => Validator?.MoveRow(
                (nameof(oldRowIndex), oldRowIndex),
                (nameof(newRowIndex), newRowIndex),
                (nameof(count), count)
            );

        /// <inheritdoc/>
        public void ValidateMoveColumn(int oldColumnIndex, int newColumnIndex)
            => Validator?.MoveColumn(
                (nameof(oldColumnIndex), oldColumnIndex),
                (nameof(newColumnIndex), newColumnIndex)
            );

        /// <inheritdoc/>
        public void ValidateMoveColumnRange(int oldColumnIndex, int newColumnIndex, int count)
            => Validator?.MoveColumn(
                (nameof(oldColumnIndex), oldColumnIndex),
                (nameof(newColumnIndex), newColumnIndex),
                (nameof(count), count)
            );

        /// <inheritdoc/>
        public void ValidateReset(IEnumerable<TRowElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings), canChangeSize: false);

        /// <inheritdoc/>
        public void ValidateReset()
            => Validator?.Reset();

        #endregion

        #region CRUD core

        /// <inheritdoc/>
        public new TFixedRowElement GetRowInternal(int rowIndex)
            => Items.Get(rowIndex, 1).First();

        /// <inheritdoc/>
        public new IEnumerable<TFixedRowElement> GetRowRangeInternal(int rowIndex, int count)
            => Items.Get(rowIndex, count).Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public new IEnumerable<TEditableListElement> GetColumnInternal(int columnIndex)
            => Items.Select(row
                => (TEditableListElement)((dynamic)row).GetInternal(columnIndex)
            );

        /// <inheritdoc/>
        public new IEnumerable<IEnumerable<TEditableListElement>> GetColumnRangeInternal(int columnIndex, int count)
            => Items.Select(row
                    => (IEnumerable<TEditableListElement>)((dynamic)row)
                    .GetRangeInternal(
                        columnIndex,
                        count
                    )
                )
                .ToTransposedArray();

        /// <inheritdoc/>
        public new TEditableListElement GetCellInternal(int rowIndex, int columnIndex)
            => ((dynamic)Items[rowIndex]).GetInternal(columnIndex);

        /// <inheritdoc/>
        public TFixedRowElement SetRowInternal(int rowIndex, TRowElementSettings settings)
            => Items.Set(rowIndex, BuildRowFromSettings(rowIndex, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> SetRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        )
            => Items.Set(rowIndex, settings.Select((s, i) => BuildRowFromSettings(i, s)).ToArray())
                .Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> SetColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        )
        {
            var result = new List<TEditableListElement>();
            var settingsArray = settings.ToArray();

            for (var i = 0; i < RowCount; i++)
            {
                var element = ((dynamic)Items[i]).SetInternal(columnIndex, settingsArray[i]);
                result.Add(element);
            }

            OnItemsChanged();

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> SetColumnRangeInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        )
        {
            var settingsArray = settings.ToTransposedArray();
            if (settingsArray.Length == 0)
            {
                return Array.Empty<IEnumerable<TEditableListElement>>();
            }

            var result = new List<IEnumerable<TEditableListElement>>();

            for (var i = 0; i < RowCount; i++)
            {
                var elements = ((dynamic)Items[i]).SetRangeInternal(columnIndex, settingsArray[i]);
                result.Add(elements);
            }

            OnItemsChanged();

            return result.ToTransposedArray();
        }

        /// <inheritdoc/>
        public TEditableListElement SetCellInternal(int rowIndex, int columnIndex, TListElementSettings settings)
            => ((dynamic)Items[rowIndex]).SetInternal(columnIndex, settings);

        /// <inheritdoc/>
        public void MoveRowInternal(int oldRowIndex, int newRowIndex)
            => Items.Move(oldRowIndex, newRowIndex);

        /// <inheritdoc/>
        public void MoveRowRangeInternal(int oldRowIndex, int newRowIndex, int count)
            => Items.Move(oldRowIndex, newRowIndex, count);

        /// <inheritdoc/>
        public void MoveColumnInternal(int oldColumnIndex, int newColumnIndex)
        {
            if (oldColumnIndex == newColumnIndex) return;

            Items.ForEach(row => ((dynamic)row).MoveInternal(oldColumnIndex, newColumnIndex));
            OnItemsChanged();
        }

        /// <inheritdoc/>
        public void MoveColumnRangeInternal(int oldColumnIndex, int newColumnIndex, int count)
        {
            if (oldColumnIndex == newColumnIndex) return;
            if (count == 0) return;

            Items.ForEach(row => ((dynamic)row).MoveRangeInternal(oldColumnIndex, newColumnIndex, count));
            OnItemsChanged();
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> ResetInternal(
            IEnumerable<TRowElementSettings> settings
        )
        {
            var beforeColumnCount = ColumnCount;

            var result = Items.Reset(settings.Select((s, i) => BuildRowFromSettings(i, s)).ToArray())
                .Cast<TFixedRowElement>();

            if (ColumnCount != beforeColumnCount)
            {
                OnColumnSizeChanged();
            }

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> ResetInternal()
            => Items.Reset(Items.Count).Cast<TFixedRowElement>();

        #endregion

        #endregion

        #region private protected

        /// <summary>
        ///     列に対する操作を行ったときの "Item[]" プロパティ変更通知を行う。
        /// </summary>
        /// <remarks>
        ///     特定の行の "Item[]" プロパティ変更通知を検知して列変更時の "Item[]" プロパティ変更通知に変換することはできないので
        ///     "Item[]" プロパティ変更通知は個別に行う必要がある。<br/>
        ///     その際に使用するメソッド。
        /// </remarks>
        private protected void OnItemsChanged()
        {
            NotifyPropertyChanged(ListConstant.IndexerName);
        }

        /// <summary>
        ///     列サイズ変更通知を行う。
        /// </summary>
        /// <remarks>
        ///     1行目のインスタンスを入れ替えつつ列数も変化する場合に使用する。
        /// </remarks>
        private protected void OnColumnSizeChanged()
        {
            NotifyPropertyChanged(nameof(ColumnCount));
        }

        #endregion

        #endregion
    }
}
