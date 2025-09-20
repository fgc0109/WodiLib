// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.cs
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
    ///     容量制限のある二次元リスト
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         外側のリストを「行（Row）」、内側のリストを「列（Column）」として扱う。<br/>
    ///         すべての行について列数は常に同じ値を取り続ける。
    ///     </para>
    ///     <para>
    ///         このクラスは WodiLib 内部でのみ使用する。WodiLib外部に見せる際はWrapperクラスを別途定義する。
    ///         使用箇所によって「行」や「列」の呼び方を変えたいことがあり、それに合わせて
    ///         メソッド名も適切なものを公開したいため。
    ///     </para>
    ///     <para>
    ///         内部状態について、
    ///         行数 > 0 かつ 列数 == 0 の状況にはなりうるが、行数 == 0 かつ 列数 > 0 の状況にはなりえない。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TEditableRowElement">行要素型</typeparam>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TReadOnlyRowElement">行要素読取専用型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TEditableListElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyListElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal class TwoDimensionalList<
        TEditableRowElement,
        TFixedRowElement,
        TReadOnlyRowElement,
        TRowElementSettings,
        TEditableListElement,
        TReadOnlyListElement,
        TListElementSettings
    > : FixedLength2DList<TEditableRowElement, TFixedRowElement, TReadOnlyRowElement, TRowElementSettings,
            TEditableListElement, TReadOnlyListElement, TListElementSettings>,
        I2DList<TFixedRowElement, TReadOnlyRowElement, TRowElementSettings, TEditableListElement,
            TReadOnlyListElement, TListElementSettings>
        where TEditableRowElement : TFixedRowElement
        where TFixedRowElement : TReadOnlyRowElement
        where TReadOnlyRowElement : TRowElementSettings, INotifyPropertyChanged, INotifyCollectionChanged
        where TRowElementSettings : notnull
        where TEditableListElement : TReadOnlyListElement
        where TReadOnlyListElement : TListElementSettings
        where TListElementSettings : notnull
    {
        #region Constructors

        #region Required

        internal TwoDimensionalList(SimpleList<TEditableRowElement> itemsImpl, Config config) : base(itemsImpl, config)
        {
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region Capacity

        /// <inheritdoc/>
        public int GetMaxRowCapacity() => MaxRowCapacity;

        /// <inheritdoc/>
        public int GetMinRowCapacity() => MinRowCapacity;

        /// <inheritdoc/>
        public int GetMaxColumnCapacity() => MaxColumnCapacity;

        /// <inheritdoc/>
        public int GetMinColumnCapacity() => MinColumnCapacity;

        #endregion

        #region CRUD

        /// <inheritdoc/>
        public TFixedRowElement AddRow(TRowElementSettings settings)
        {
            ValidateAddRow(settings);
            return AddRowInternal(settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AddRowRange(IEnumerable<TRowElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateAddRowRange(settingsArray);
            return AddRowRangeInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> AddColumn(IEnumerable<TListElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateAddColumn(settingsArray);
            return AddColumnInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> AddColumnRange(
            IEnumerable<IEnumerable<TListElementSettings>> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateAddColumnRange(settingsArray);
            return AddColumnRangeInternal(settingsArray);
        }

        /// <inheritdoc/>
        public TFixedRowElement InsertRow(int rowIndex, TRowElementSettings settings)
        {
            ValidateInsertRow(rowIndex, settings);
            return InsertRowInternal(rowIndex, settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> InsertRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateInsertRowRange(rowIndex, settingsArray);
            return InsertRowRangeInternal(rowIndex, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> InsertColumn(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateInsertColumn(columnIndex, settingsArray);
            return InsertColumnInternal(columnIndex, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> InsertColumnRange(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateInsertColumnRange(columnIndex, settingsArray);
            return InsertColumnRangeInternal(columnIndex, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> OverwriteRow(int rowIndex, IEnumerable<TRowElementSettings> settings)
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateOverwriteRow(rowIndex, settingsArray);
            return OverwriteRowInternal(rowIndex, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> OverwriteColumn(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateOverwriteColumn(columnIndex, settingsArray);
            return OverwriteColumnInternal(columnIndex, settingsArray);
        }

        /// <inheritdoc/>
        public TFixedRowElement RemoveRow(int rowIndex)
        {
            ValidateRemoveRow(rowIndex);
            return RemoveRowInternal(rowIndex);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> RemoveRowRange(int rowIndex, int count)
        {
            ValidateRemoveRowRange(rowIndex, count);
            return RemoveRowRangeInternal(rowIndex, count);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> RemoveColumnRange(int columnIndex, int count)
        {
            ValidateRemoveColumnRange(columnIndex, count);
            return RemoveColumnRangeInternal(columnIndex, count);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AdjustRowLength(int length)
        {
            ValidateAdjustRowLength(length);
            return AdjustRowLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AdjustRowLengthIfShort(int length)
        {
            ValidateAdjustRowLength(length);
            if (RowCount >= length)
            {
                return Array.Empty<TFixedRowElement>();
            }

            return AdjustRowLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AdjustRowLengthIfLong(int length)
        {
            ValidateAdjustRowLength(length);
            if (RowCount <= length)
            {
                return Array.Empty<TFixedRowElement>();
            }

            return AdjustRowLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> RemoveColumn(int columnIndex)
        {
            ValidateRemoveColumn(columnIndex);
            return RemoveColumnInternal(columnIndex);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLength(int length)
        {
            ValidateAdjustColumnLength(length);
            return AdjustColumnLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLengthIfShort(int length)
        {
            ValidateAdjustColumnLength(length);
            if (ColumnCount >= length)
            {
                return Array.Empty<IEnumerable<TEditableListElement>>();
            }

            return AdjustColumnLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLengthIfLong(int length)
        {
            ValidateAdjustColumnLength(length);
            if (ColumnCount <= length)
            {
                return Array.Empty<IEnumerable<TEditableListElement>>();
            }

            return AdjustColumnLengthInternal(length);
        }

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="settings">二次元リストに詰め直す要素</param>
        /// <returns>新たに二次元リストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> の行数が <see cref="GetMinRowCapacity"/> 未満
        ///     または <see cref="GetMaxRowCapacity"/> を超える場合、
        ///     列数が <see cref="GetMinColumnCapacity"/> 未満
        ///     または <see cref="GetMaxColumnCapacity"/> を超える場合。
        /// </exception>
        /// <remarks>
        ///     このメソッドは <paramref name="settings"/> の行数が
        ///     <see cref="GetMinRowCapacity"/> 以上 <see cref="GetMaxRowCapacity"/> 以下、
        ///     列数が <see cref="GetMinColumnCapacity"/> 以上 <see cref="GetMaxColumnCapacity"/> 以下であれば
        ///     成功する。<br/>
        ///     現在の行数・列数と一致しない場合エラーとしたい場合は、
        ///     容量固定型にキャストしてから同メソッドを呼び出す。
        /// </remarks>
        public new IEnumerable<TFixedRowElement> Reset(
            IEnumerable<TRowElementSettings> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateReset(settingsArray);
            return ResetInternal(settingsArray);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            ValidateClear();
            ClearInternal();
        }

        #endregion

        #region Validate

        /// <inheritdoc/>
        public void ValidateAddRow(TRowElementSettings settings)
            => Validator?.InsertRow(("rowIndex", RowCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAddRowRange(IEnumerable<TRowElementSettings> settings)
            => Validator?.InsertRow(("rowIndex", RowCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertRow(int rowIndex, TRowElementSettings settings)
            => Validator?.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings)
            => Validator?.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateOverwriteRow(int rowIndex, IEnumerable<TRowElementSettings> settings)
            => Validator?.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateRemoveRow(int rowIndex)
            => Validator?.RemoveRow((nameof(rowIndex), rowIndex));

        /// <inheritdoc/>
        public void ValidateRemoveRowRange(int rowIndex, int count)
            => Validator?.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateAdjustRowLength(int length)
            => Validator?.AdjustRowLength((nameof(length), length));

        /// <inheritdoc/>
        public void ValidateAddColumn(IEnumerable<TListElementSettings> settings)
            => Validator?.InsertColumn(("columnIndex", ColumnCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAddColumnRange(IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.InsertColumn(("columnIndex", ColumnCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertColumn(int columnIndex, IEnumerable<TListElementSettings> settings)
            => Validator?.InsertColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertColumnRange(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.InsertColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateOverwriteColumn(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.OverwriteColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateRemoveColumn(int columnIndex)
            => Validator?.RemoveColumn((nameof(columnIndex), columnIndex));

        /// <inheritdoc/>
        public void ValidateRemoveColumnRange(int columnIndex, int count)
            => Validator?.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateAdjustColumnLength(int length)
            => Validator?.AdjustColumnLength((nameof(length), length));

        /// <inheritdoc/>
        public new void ValidateReset(IEnumerable<TRowElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateClear()
            => Validator?.Clear();

        #endregion

        #region CLUD core

        /// <inheritdoc/>
        public TFixedRowElement AddRowInternal(TRowElementSettings settings)
            => Items.Add(BuildRowFromSettings(RowCount, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AddRowRangeInternal(IEnumerable<TRowElementSettings> settings)
            => Items.Add(settings.Select((s, i) => BuildRowFromSettings(RowCount + i, s)).ToArray())
                .Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public TFixedRowElement InsertRowInternal(int rowIndex, TRowElementSettings settings)
            => Items.Insert(rowIndex, BuildRowFromSettings(RowCount, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> InsertRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        )
            => Items.Insert(rowIndex, settings.Select((s, i) => BuildRowFromSettings(rowIndex + i, s)).ToArray())
                .Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> OverwriteRowInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        )
            => Items.Overwrite(rowIndex, settings.Select((s, i) => BuildRowFromSettings(rowIndex + i, s)).ToArray())
                .Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public TFixedRowElement RemoveRowInternal(int rowIndex)
            => Items.Remove(rowIndex, 1).First();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> RemoveRowRangeInternal(int rowIndex, int count)
            => Items.Remove(rowIndex, count).Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AdjustRowLengthInternal(int length)
            => Items.Adjust(length).Cast<TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> AddColumnInternal(IEnumerable<TListElementSettings> settings)
        {
            var result = new List<TEditableListElement>();
            var settingsArray = settings.ToArray();

            for (var i = 0; i < RowCount; i++)
            {
                var element = ((dynamic)Items[i]).AddInternal(settingsArray[i]);
                result.Add(element);
            }

            OnItemsChanged();

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> AddColumnRangeInternal(
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
                var elements = ((dynamic)Items[i]).AddRangeInternal(settingsArray[i]);
                result.Add(elements);
            }

            OnItemsChanged();

            return result.ToTransposedArray();
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> InsertColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        )
        {
            var result = new List<TEditableListElement>();
            var settingsArray = settings.ToArray();

            for (var i = 0; i < RowCount; i++)
            {
                var element = ((dynamic)Items[i]).InsertInternal(columnIndex, settingsArray[i]);
                result.Add(element);
            }

            OnItemsChanged();

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> InsertColumnRangeInternal(
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
                var elements = ((dynamic)Items[i]).InsertRangeInternal(columnIndex, settingsArray[i]);
                result.Add(elements);
            }

            OnItemsChanged();

            return result.ToTransposedArray();
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> OverwriteColumnInternal(
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
                var elements = ((dynamic)Items[i]).OverwriteInternal(columnIndex, settingsArray[i]);
                result.Add(elements);
            }

            OnItemsChanged();

            return result.ToTransposedArray();
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableListElement> RemoveColumnInternal(int columnIndex)
        {
            var result = new List<TEditableListElement>();

            for (var i = 0; i < RowCount; i++)
            {
                var element = ((dynamic)Items[i]).RemoveInternal(columnIndex);
                result.Add(element);
            }

            OnItemsChanged();

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> RemoveColumnRangeInternal(int columnIndex, int count)
        {
            if (count == 0)
            {
                return Array.Empty<IEnumerable<TEditableListElement>>();
            }

            var result = new List<IEnumerable<TEditableListElement>>();

            for (var i = 0; i < RowCount; i++)
            {
                var elements = ((dynamic)Items[i]).RemoveRangeInternal(columnIndex, count);
                result.Add(elements);
            }

            OnItemsChanged();

            return result.ToTransposedArray();
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLengthInternal(int length)
        {
            if (ColumnCount == length)
            {
                return Array.Empty<IEnumerable<TEditableListElement>>();
            }

            var result = new List<IEnumerable<TEditableListElement>>();

            for (var i = 0; i < RowCount; i++)
            {
                var elements = ((dynamic)Items[i]).AdjustLengthInternal(length);
                result.Add(elements);
            }

            OnItemsChanged();

            return result.ToTransposedArray();
        }

        /// <inheritdoc/>
        public void ClearInternal()
        {
            var beforeColumnCount = ColumnCount;
            Items.Reset(
                MinRowCapacity.Iterate(rowIndex => BuildRowFromSettings(
                            rowIndex,
                            BuildRowSettingsFromRowIndex(rowIndex, MinColumnCapacity)
                        )
                    )
                    .ToArray()
            );
            if (ColumnCount != beforeColumnCount)
            {
                OnColumnSizeChanged();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
