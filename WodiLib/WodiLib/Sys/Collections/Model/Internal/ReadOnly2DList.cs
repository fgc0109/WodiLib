// ========================================
// Project Name : WodiLib
// File Name    : ReadOnly2DList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読取専用の二次元リスト
    /// </summary>
    /// <typeparam name="TEditableRowElement">行要素型</typeparam>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TReadOnlyRowElement">行要素読取専用型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TEditableListElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyListElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal partial class ReadOnly2DList<
        TEditableRowElement,
        TFixedRowElement,
        TReadOnlyRowElement,
        TRowElementSettings,
        TEditableListElement,
        TReadOnlyListElement,
        TListElementSettings
    >
        : ModelBase,
            IReadOnly2DList<TReadOnlyRowElement, TReadOnlyListElement>,
            IReadOnlyList<TReadOnlyRowElement>,
            INotifyCollectionChanged,
            IEqualityComparable<ReadOnly2DList<
                TEditableRowElement,
                TFixedRowElement,
                TReadOnlyRowElement,
                TRowElementSettings,
                TEditableListElement,
                TReadOnlyListElement,
                TListElementSettings
            >>
        where TEditableRowElement : TFixedRowElement
        where TFixedRowElement : TReadOnlyRowElement
        where TReadOnlyRowElement : TRowElementSettings, INotifyPropertyChanged, INotifyCollectionChanged
        where TRowElementSettings : notnull
        where TEditableListElement : TReadOnlyListElement
        where TReadOnlyListElement : TListElementSettings
        where TListElementSettings : notnull
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement,TReadOnlyListElement}.this[int]"/>
        public TReadOnlyRowElement this[int rowIndex] => GetRow(rowIndex);

        /// <inheritdoc/>
        public TReadOnlyListElement this[int rowIndex, int columnIndex] => GetCell(rowIndex, columnIndex);

        /// <inheritdoc/>
        public int RowCount => Items.Count;

        /// <inheritdoc/>
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

        #region private protected

        private protected SimpleList<TEditableRowElement> Items { get; }

        private protected IWodiLib2DListValidator<TRowElementSettings, TListElementSettings>? Validator
            => config.Validator;

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

        internal ReadOnly2DList(
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

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public IEnumerator<TReadOnlyRowElement> GetEnumerator()
            => Items.Cast<TReadOnlyRowElement>().GetEnumerator();

        /// <inheritdoc/>
        public TReadOnlyRowElement GetRow(int rowIndex)
        {
            ValidateGetRow(rowIndex);
            return GetRowInternal(rowIndex);
        }

        /// <inheritdoc/>
        public IEnumerable<TReadOnlyRowElement> GetRowRange(int rowIndex, int count)
        {
            ValidateGetRowRange(rowIndex, count);
            return GetRowRangeInternal(rowIndex, count);
        }

        /// <inheritdoc/>
        public IEnumerable<TReadOnlyListElement> GetColumn(int columnIndex)
        {
            ValidateGetColumn(columnIndex);
            return GetColumnInternal(columnIndex);
        }

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TReadOnlyListElement>> GetColumnRange(int columnIndex, int count)
        {
            ValidateGetColumnRange(columnIndex, count);
            return GetColumnRangeInternal(columnIndex, count);
        }

        /// <inheritdoc/>
        public TReadOnlyListElement GetCell(int rowIndex, int columnIndex)
        {
            ValidateGetCell(rowIndex, columnIndex);
            return GetCellInternal(rowIndex, columnIndex);
        }

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
        public TReadOnlyRowElement GetRowInternal(int rowIndex)
            => Items.Get(rowIndex, 1).First();

        /// <inheritdoc/>
        public IEnumerable<TReadOnlyRowElement> GetRowRangeInternal(int rowIndex, int count)
            => Items.Get(rowIndex, count).Cast<TReadOnlyRowElement>();

        /// <inheritdoc/>
        public IEnumerable<TReadOnlyListElement> GetColumnInternal(int columnIndex)
            => Items.Select(row => (TReadOnlyListElement)((dynamic)row).GetInternal(columnIndex));

        /// <inheritdoc/>
        public IEnumerable<IEnumerable<TReadOnlyListElement>> GetColumnRangeInternal(int columnIndex, int count)
            => Items.Select(row
                    => (IEnumerable<TReadOnlyListElement>)((dynamic)row).GetRangeInternal(columnIndex, count)
                )
                .ToTransposedArray();

        /// <inheritdoc/>
        public TReadOnlyListElement GetCellInternal(int rowIndex, int columnIndex)
            => ((dynamic)Items.Get(rowIndex, 1).First()).GetInternal(columnIndex);

        /// <inheritdoc/>
        public bool ItemEquals(
            ReadOnly2DList<TEditableRowElement, TFixedRowElement, TReadOnlyRowElement, TRowElementSettings,
                TEditableListElement, TReadOnlyListElement, TListElementSettings>? other
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
        public bool ItemEquals(object? other) => ItemEquals(
            other as ReadOnly2DList<TEditableRowElement, TFixedRowElement, TReadOnlyRowElement, TRowElementSettings,
                TEditableListElement, TReadOnlyListElement, TListElementSettings>
        );

        #endregion

        #region Interface Implicit

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region IReadOnlyCollection

        int IReadOnlyCollection<TReadOnlyRowElement>.Count => RowCount;

        #endregion

        #endregion

        #region private

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
    }
}
