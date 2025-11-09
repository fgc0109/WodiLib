// ========================================
// Project Name : WodiLib
// File Name    : TwoDimensionalList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
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
    /// <typeparam name="TListSettings">リストの入力パラメータ型</typeparam>
    /// <typeparam name="TEditableRowElement">行要素型</typeparam>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TEditableListElement">リスト要素型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal partial class TwoDimensionalList<
        TListSettings,
        TEditableRowElement,
        TFixedRowElement,
        TRowElementSettings,
        TEditableListElement,
        TListElementSettings
    > : ModelBase,
        I2DList<
            TFixedRowElement,
            TRowElementSettings,
            TEditableListElement,
            TListElementSettings
        >,
        IEnumerable<TFixedRowElement>,
        INotifyCollectionChanged,
        IEqualityComparable<TwoDimensionalList<
            TListSettings,
            TEditableRowElement,
            TFixedRowElement,
            TRowElementSettings,
            TEditableListElement,
            TListElementSettings
        >>
        where TListSettings : IListSettings<TRowElementSettings>
        where TEditableRowElement : TRowElementSettings, INotifyPropertyChanged
        where TFixedRowElement : TRowElementSettings
        where TRowElementSettings : notnull
        where TEditableListElement : TListElementSettings
        where TListElementSettings : notnull
    {
        #region Events

        #region public

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <inheritdoc/>
        public TFixedRowElement this[int rowIndex]
        {
            [Pure] get => GetRow(rowIndex);
            set => SetRow(rowIndex, value);
        }

        /// <inheritdoc/>
        public TEditableListElement this[int rowIndex, int columnIndex]
        {
            [Pure] get => GetCell(rowIndex, columnIndex);
            set => SetCell(rowIndex, columnIndex, value);
        }

        /// <inheritdoc/>
        [Pure]
        public int RowCount => Items.Count;

        /// <inheritdoc/>
        [Pure]
        public int ColumnCount
        {
            get
            {
                if (Items.Count == 0)
                {
                    return 0;
                }

                return ((dynamic)Items[0]).Count;
            }
        }

        #endregion

        #region internal

        internal IWodiLib2DListValidator<TListSettings, TRowElementSettings, TListElementSettings>? Validator
            => config.Validator;

        #endregion

        #region private protected

        private protected SimpleList<TEditableRowElement> Items { get; }

        private protected Config.BuildRowSettingsFromRowIndexDelegate BuildRowSettingsFromRowIndex
            => config.RowSettingsFactoryRowIndex;

        private protected Config.BuildRowFromSettingsDelegate BuildRowFromSettings => config.RowFactoryFromSettings;

        private protected int MaxRowCapacity => config.MaxRowCapacity;

        private protected int MinRowCapacity => config.MinRowCapacity;

        private protected int MaxColumnCapacity => config.MaxColumnCapacity;

        private protected int MinColumnCapacity => config.MinColumnCapacity;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly Config config;

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        private PropertyChangedEventHandler? firstRowPropertyChangedHandler;

        private TEditableRowElement? currentFirstRowElement;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        internal TwoDimensionalList(
            SimpleList<TEditableRowElement> itemsImpl,
            Config config
        )
        {
            ThrowHelper.ValidateArgumentNotNull(itemsImpl is null, nameof(itemsImpl));
            ThrowHelper.ValidateArgumentNotNull(config is null, nameof(config));

            Items = itemsImpl;
            this.config = config;

            PropagatePropertyChangeEvent(
                Items,
                (_, name) =>
                {
                    if (name == nameof(Items.Count))
                    {
                        return new[] { nameof(RowCount) };
                    }

                    return new[] { name };
                }
            );
            PropagateCollectionChangeEvent(Items);
            SetupFirstRowColumnCountWatcher();
        }

        /// <summary>
        ///     <see cref="SimpleList{T}"/> が通知した
        ///     <see cref="INotifyCollectionChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <param name="target">対象</param>
        private void PropagateCollectionChangeEvent(SimpleList<TEditableRowElement> target)
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }

        /// <summary>
        ///     0行目の要素のColumnCount変更を監視し、ColumnCountプロパティ変更通知を行う。
        /// </summary>
        private void SetupFirstRowColumnCountWatcher()
        {
            Items.CollectionChanged += OnItemsCollectionChanged;
            SetupFirstRowWatcher();
        }

        /// <summary>
        ///     Items のコレクション変更時に0行目の監視対象を更新する。
        /// </summary>
        /// <param name="sender">送信者</param>
        /// <param name="e">イベント引数</param>
        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // 0行目に影響がある変更の場合のみ処理
            var affectsFirstRow = e.Action switch
            {
                NotifyCollectionChangedAction.Add when e.NewStartingIndex == 0 => true,
                NotifyCollectionChangedAction.Remove when e.OldStartingIndex == 0 => true,
                NotifyCollectionChangedAction.Replace when e.NewStartingIndex == 0 || e.OldStartingIndex == 0 => true,
                NotifyCollectionChangedAction.Move when e.NewStartingIndex == 0 || e.OldStartingIndex == 0 => true,
                NotifyCollectionChangedAction.Reset => true,
                _ => false,
            };

            if (affectsFirstRow)
            {
                SetupFirstRowWatcher();
            }
        }

        /// <summary>
        ///     0行目の要素に対するColumnCount監視を設定する。
        /// </summary>
        private void SetupFirstRowWatcher()
        {
            // 既存のハンドラを解除
            if (firstRowPropertyChangedHandler is not null && currentFirstRowElement is not null)
            {
                currentFirstRowElement.PropertyChanged -= firstRowPropertyChangedHandler;
                currentFirstRowElement = default!;
            }

            // 新しいハンドラを設定
            if (Items.Count > 0)
            {
                firstRowPropertyChangedHandler = (_, e) =>
                {
                    if (e.PropertyName == nameof(Items.Count))
                    {
                        NotifyPropertyChanged(nameof(ColumnCount));
                    }
                };
                Items[0].PropertyChanged += firstRowPropertyChangedHandler;
                currentFirstRowElement = Items[0];
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region Capacity

        /// <inheritdoc/>
        [Pure]
        public int GetMaxRowCapacity() => MaxRowCapacity;

        /// <inheritdoc/>
        [Pure]
        public int GetMinRowCapacity() => MinRowCapacity;

        /// <inheritdoc/>
        [Pure]
        public int GetMaxColumnCapacity() => MaxColumnCapacity;

        /// <inheritdoc/>
        [Pure]
        public int GetMinColumnCapacity() => MinColumnCapacity;

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        [Pure]
        public IEnumerator<TFixedRowElement> GetEnumerator()
            => Items.Select(row => row.Cast<TEditableRowElement, TFixedRowElement>()).GetEnumerator();

        #endregion

        #region CRUD

        /// <inheritdoc/>
        [Pure]
        public TFixedRowElement GetRow(int rowIndex)
        {
            ValidateGetRow(rowIndex);
            return GetRowInternal(rowIndex);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TFixedRowElement> GetRowRange(int rowIndex, int count)
        {
            ValidateGetRowRange(rowIndex, count);
            return GetRowRangeInternal(rowIndex, count);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TEditableListElement> GetColumn(int columnIndex)
        {
            ValidateGetColumn(columnIndex);
            return GetColumnInternal(columnIndex);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<IEnumerable<TEditableListElement>> GetColumnRange(int columnIndex, int count)
        {
            ValidateGetColumnRange(columnIndex, count);
            return GetColumnRangeInternal(columnIndex, count);
        }

        /// <inheritdoc/>
        [Pure]
        public TEditableListElement GetCell(int rowIndex, int columnIndex)
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
        public TEditableListElement SetCell(int rowIndex, int columnIndex, TListElementSettings settings)
        {
            ValidateSetCell(rowIndex, columnIndex, settings);
            return SetCellInternal(rowIndex, columnIndex, settings);
        }

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
        public IEnumerable<TEditableListElement> RemoveColumn(int columnIndex)
        {
            ValidateRemoveColumn(columnIndex);
            return RemoveColumnInternal(columnIndex);
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

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> Reset(
            IEnumerable<TRowElementSettings> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateReset(settingsArray);
            return ResetInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> ResetStrict(
            IEnumerable<TRowElementSettings> settings
        )
        {
            var settingsArray = settings?.ToArray() ?? null!;

            ValidateResetStrict(settingsArray);
            return ResetStrictInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> Reset()
        {
            ValidateReset();
            return ResetInternal();
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
        public void ValidateGetRow(int rowIndex)
            => Validator?.GetRow((nameof(rowIndex), rowIndex), ("count", 1));

        /// <inheritdoc/>
        public void ValidateGetRowRange(int rowIndex, int count)
            => Validator?.GetRow((nameof(rowIndex), rowIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateGetColumn(int columnIndex)
            => Validator?.GetColumn((nameof(columnIndex), columnIndex), ("count", 1));

        /// <inheritdoc/>
        public void ValidateGetColumnRange(int columnIndex, int count)
            => Validator?.GetColumn((nameof(columnIndex), columnIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateGetCell(int rowIndex, int columnIndex)
            => Validator?.GetCell((nameof(rowIndex), rowIndex), (nameof(columnIndex), columnIndex));

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
        public void ValidateAddRow(TRowElementSettings settings)
            => Validator?.InsertRow(("rowIndex", RowCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAddRowRange(IEnumerable<TRowElementSettings> settings)
            => Validator?.InsertRow(("rowIndex", RowCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAddColumn(IEnumerable<TListElementSettings> settings)
            => Validator?.InsertColumn(("columnIndex", ColumnCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAddColumnRange(IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.InsertColumn(("columnIndex", ColumnCount), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertRow(int rowIndex, TRowElementSettings settings)
            => Validator?.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings)
            => Validator?.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertColumn(int columnIndex, IEnumerable<TListElementSettings> settings)
            => Validator?.InsertColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertColumnRange(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.InsertColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateOverwriteRow(int rowIndex, IEnumerable<TRowElementSettings> settings)
            => Validator?.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateOverwriteColumn(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings)
            => Validator?.OverwriteColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings));

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
        public void ValidateRemoveRow(int rowIndex)
            => Validator?.RemoveRow((nameof(rowIndex), rowIndex));

        /// <inheritdoc/>
        public void ValidateRemoveRowRange(int rowIndex, int count)
            => Validator?.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateRemoveColumn(int columnIndex)
            => Validator?.RemoveColumn((nameof(columnIndex), columnIndex));

        /// <inheritdoc/>
        public void ValidateRemoveColumnRange(int columnIndex, int count)
            => Validator?.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateAdjustRowLength(int length)
            => Validator?.AdjustRowLength((nameof(length), length));

        /// <inheritdoc/>
        public void ValidateAdjustColumnLength(int length)
            => Validator?.AdjustColumnLength((nameof(length), length));

        /// <inheritdoc/>
        public void ValidateReset(IEnumerable<TRowElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateResetStrict(IEnumerable<TRowElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings), canChangeSize: false);

        /// <inheritdoc/>
        public void ValidateReset()
            => Validator?.Reset();

        /// <inheritdoc/>
        public void ValidateClear()
            => Validator?.Clear();

        #endregion

        #region CLUD core

        /// <inheritdoc/>
        [Pure]
        public TFixedRowElement GetRowInternal(int rowIndex)
            => Items.Get(rowIndex, 1).First().Cast<TEditableRowElement, TFixedRowElement>();

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TFixedRowElement> GetRowRangeInternal(int rowIndex, int count)
            => Items.Get(rowIndex, count).Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TEditableListElement> GetColumnInternal(int columnIndex)
            => Items.Select(row
                => (TEditableListElement)((dynamic)row).GetInternal(columnIndex)
            );

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<IEnumerable<TEditableListElement>> GetColumnRangeInternal(int columnIndex, int count)
            => Items.Select(row
                    => (IEnumerable<TEditableListElement>)((dynamic)row)
                    .GetRangeInternal(
                        columnIndex,
                        count
                    )
                )
                .ToTransposedArray();

        /// <inheritdoc/>
        [Pure]
        public TEditableListElement GetCellInternal(int rowIndex, int columnIndex)
            => ((dynamic)Items[rowIndex]).GetInternal(columnIndex);

        /// <inheritdoc/>
        public TFixedRowElement SetRowInternal(int rowIndex, TRowElementSettings settings)
            => Items.Set(rowIndex, BuildRowFromSettings(rowIndex, settings))
                .First()
                .Cast<TEditableRowElement, TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> SetRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        )
            => Items.Set(rowIndex, settings.Select((s, i) => BuildRowFromSettings(i, s)).ToArray())
                .Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

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
        public TFixedRowElement AddRowInternal(TRowElementSettings settings)
            => Items.Add(BuildRowFromSettings(RowCount, settings))
                .First()
                .Cast<TEditableRowElement, TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> AddRowRangeInternal(IEnumerable<TRowElementSettings> settings)
            => Items.Add(settings.Select((s, i) => BuildRowFromSettings(RowCount + i, s)).ToArray())
                .Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

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
        public TFixedRowElement InsertRowInternal(int rowIndex, TRowElementSettings settings)
            => Items.Insert(rowIndex, BuildRowFromSettings(RowCount, settings))
                .First()
                .Cast<TEditableRowElement, TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> InsertRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        )
            => Items.Insert(rowIndex, settings.Select((s, i) => BuildRowFromSettings(rowIndex + i, s)).ToArray())
                .Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

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
        public IEnumerable<TFixedRowElement> OverwriteRowInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        )
            => Items.Overwrite(rowIndex, settings.Select((s, i) => BuildRowFromSettings(rowIndex + i, s)).ToArray())
                .Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

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
        public TFixedRowElement RemoveRowInternal(int rowIndex)
            => Items.Remove(rowIndex, 1).First().Cast<TEditableRowElement, TFixedRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> RemoveRowRangeInternal(int rowIndex, int count)
            => Items.Remove(rowIndex, count).Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

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
        public IEnumerable<TFixedRowElement> AdjustRowLengthInternal(int length)
            => Items.Adjust(length).Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

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
        public IEnumerable<TFixedRowElement> ResetInternal(
            IEnumerable<TRowElementSettings> settings
        )
        {
            var beforeColumnCount = ColumnCount;

            var result = Items.Reset(settings.Select((s, i) => BuildRowFromSettings(i, s)).ToArray())
                .Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

            if (ColumnCount != beforeColumnCount)
            {
                OnColumnSizeChanged();
            }

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> ResetStrictInternal(
            IEnumerable<TRowElementSettings> settings
        )
        {
            var result = Items.Reset(settings.Select((s, i) => BuildRowFromSettings(i, s)).ToArray())
                .Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

            return result;
        }

        /// <inheritdoc/>
        public IEnumerable<TFixedRowElement> ResetInternal()
            => Items.Reset(Items.Count).Select(row => row.Cast<TEditableRowElement, TFixedRowElement>());

        /// <inheritdoc/>
        public void ClearInternal()
        {
            var beforeColumnCount = ColumnCount;
            Items.Reset(
                MinRowCapacity.Iterate(rowIndex => BuildRowFromSettings(
                            rowIndex,
                            BuildRowSettingsFromRowIndex(rowIndex, MinColumnCapacity, Items)
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

        #region ItemEquals

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(
            TwoDimensionalList<
                TListSettings,
                TEditableRowElement,
                TFixedRowElement,
                TRowElementSettings,
                TEditableListElement,
                TListElementSettings
            >? other
        )
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return Items.SequenceEqual(
                other.Items,
                EqualityComparerFactory.Create<TEditableRowElement>()
            );
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => ItemEquals(
            other as TwoDimensionalList<
                TListSettings,
                TEditableRowElement,
                TFixedRowElement,
                TRowElementSettings,
                TEditableListElement,
                TListElementSettings
            >
        );

        #endregion

        #endregion

        #region Interface Implicit

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
