// ========================================
// Project Name : WodiLib
// File Name    : SimpleList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 内部で使用する基本リストクラス。
    ///     基本的なメソッドを定義したクラス。
    /// </summary>
    internal class SimpleList<T> : ObservableCollection<T>,
        ISimpleList<T>,
        IDeepCloneable<SimpleList<T>>
    {
        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        internal SimpleList(SimpleListValueBuilder<T> valueBuilder, IEnumerable<T>? initValues = null) : base(
            initValues ?? Array.Empty<T>()
        )
        {
            ThrowHelper.ValidateArgumentNotNull(valueBuilder is null, nameof(valueBuilder));

            ValueBuilder = valueBuilder;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region private

        private SimpleListValueBuilder<T> ValueBuilder { get; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public IEnumerable<T> Get(int index, int count)
        {
            return Items.Skip(index).Take(count);
        }

        /// <inheritdoc/>
        public IEnumerable<T> Set(int index, params T[] items)
        {
            switch (items.Length)
            {
                case 0:
                    return items;
                case 1:
                    SetItem(index, items[0]);
                    return items;
            }

            CheckReentrancy();
            items.ForEach((item, offset) => { Items[index + offset] = item; });

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();

            return items;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Add(params T[] items)
        {
            Insert(Count, items);
            return items;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Insert(int index, params T[] items)
        {
            switch (items.Length)
            {
                case 0:
                    return items;
                case 1:
                    InsertItem(index, items[0]);
                    return items;
            }

            CheckReentrancy();
            items.ForEach((item, offset) => { Items.Insert(index + offset, item); });

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));
            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();

            return items;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Overwrite(int index, params T[] items)
        {
            switch (items.Length)
            {
                case 0:
                    return items;
                case 1 when index < Count:
                    SetItem(index, items[0]);
                    return items;
                case 1 when index == Count:
                    InsertItem(index, items[0]);
                    return items;
            }

            var overwriteParam = OverwriteParam<T>.Factory.Create(Items, index, items);

            CheckReentrancy();

            overwriteParam.ReplaceNewItems.ForEach((item, offset) => { Items[index + offset] = item; }
            );
            overwriteParam.InsertItems.ForEach(item => { Items.Add(item); }
            );

            if (overwriteParam.InsertItems.Length > 0)
                OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
            return items;
        }

        /// <inheritdoc/>
        public new void Move(int oldIndex, int newIndex)
            => Move(oldIndex, newIndex, 1);

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex, int count)
        {
            if (oldIndex == newIndex)
            {
                return;
            }

            switch (count)
            {
                case 0:
                    return;
                case 1:
                    MoveItem(oldIndex, newIndex);
                    return;
            }

            CheckReentrancy();

            var movedItems = Get(oldIndex, count).ToList();
            count.Range()
                .ForEach(_ => { Items.RemoveAt(oldIndex); }
                );
            movedItems.ForEach((moveItem, offset) => { Items.Insert(newIndex + offset, moveItem); }
            );

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();
        }

        /// <inheritdoc/>
        public IEnumerable<T> Remove(int index, int count)
        {
            switch (count)
            {
                case 0:
                    return Array.Empty<T>();
                case 1:
                    var removeItem = Items[index];
                    RemoveItem(index);
                    return new[] { removeItem };
            }

            var removeItems = Get(index, count).ToArray();

            count.Range()
                .ForEach(_ => { Items.RemoveAt(index); }
                );


            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));
            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();

            return removeItems;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Adjust(int length)
        {
            if (Count == length) return Array.Empty<T>();
            if (Count > length)
            {
                return AdjustIfLong(length);
            }

            // Count < length
            return AdjustIfShort(length);
        }

        /// <inheritdoc/>
        public IEnumerable<T> AdjustIfLong(int length)
        {
            if (Count <= length) return Array.Empty<T>();
            return Remove(length, Count - length);
        }

        /// <inheritdoc/>
        public IEnumerable<T> AdjustIfShort(int length)
        {
            if (Count >= length) return Array.Empty<T>();

            var addItems = (length - Count).Iterate(i => ValueBuilder.Build(this, Count + i));
            return Add(addItems.ToArray());
        }

        /// <inheritdoc/>
        public IEnumerable<T> Reset(params T[] items)
        {
            CheckReentrancy();

            var isCountChange = Count != items.Length;

            Items.Clear();
            items.ForEach(item => { Items.Add(item); });

            if (isCountChange) OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();

            return items;
        }

        /// <inheritdoc/>
        public IEnumerable<T> Reset(int length)
        {
            CheckReentrancy();

            var isCountChange = Count != length;

            // リセット後のインスタンス作成のためにリセット直前の情報を利用したい場合があるため、
            // リセット後の要素を作成してから保持しているデータをクリアする。
            var newItems = length.Iterate(i => ValueBuilder.Build(this, i)).ToArray();
            Items.Clear();
            newItems.ForEach(item => { Items.Add(item); });

            if (isCountChange) OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(nameof(Count)));

            OnPropertyChanged(PropertyChangedEventArgsCache.GetInstance(ListConstant.IndexerName));
            OnCollectionReset();

            return Items;
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ISimpleList<T>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return this.SequenceEqual(other, EqualityComparerFactory.Create<T>());
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other)
        {
            if (other is SimpleList<T> castedSimpleList) return ItemEquals(castedSimpleList);

            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (other is IEnumerable<T> castedEnumerable) return this.SequenceEqual(castedEnumerable);

            return Equals(other);
        }

        /// <inheritdoc/>
        public SimpleList<T> DeepClone()
        {
            var result = new SimpleList<T>(ValueBuilder, this);
            return result;
        }

        #endregion

        #region Interface Implementations

        #region IDeepCloneable

        object IDeepCloneable.DeepClone() => DeepClone();
        ISimpleList<T> IDeepCloneable<ISimpleList<T>>.DeepClone() => DeepClone();

        #endregion

        #endregion

        #region private

        private void OnCollectionReset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #endregion
    }
}
