// ========================================
// Project Name : WodiLib
// File Name    : ReadOnlyExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読取専用のListクラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IReadOnlyExtendedList{T}"/> 参照。
    /// </remarks>
    /// <typeparam name="TEditableElement">リスト要素型（編集可能）</typeparam>
    /// <typeparam name="TReadOnlyElement">リスト要素型</typeparam>
    /// <typeparam name="TElementSettings">リスト内包型の入力パラメータ型</typeparam>
    internal class ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings> : ModelBase,
        IReadOnlyExtendedList<TReadOnlyElement>,
        IReadOnlyList<TReadOnlyElement>,
        INotifyCollectionChanged,
        IEqualityComparable<ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>>
        where TEditableElement : TReadOnlyElement, TElementSettings
        where TReadOnlyElement : TElementSettings
        where TElementSettings : notnull
    {
        #region Events

        #region public

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <inheritdoc cref="IReadOnlyList{T}.this"/>
        public TReadOnlyElement this[int index] => Get(index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.Count"/>
        public int Count => Items.Count;

        #endregion

        #region private protected

        private protected SimpleList<TEditableElement> Items { get; }

        private protected IWodiLibListValidator<TElementSettings>? Validator { get; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="itemsImpl">リスト実装インスタンス</param>
        /// <param name="validator">各種引数検証バリデーター実装</param>
        internal ReadOnlyExtendedList(
            SimpleList<TEditableElement> itemsImpl,
            IWodiLibListValidator<TElementSettings>? validator
        )
        {
            ThrowHelper.ValidateArgumentNotNull(itemsImpl is null, nameof(itemsImpl));

            Items = itemsImpl;
            Validator = validator;

            PropagatePropertyChangeEvent(Items);
            PropagateCollectionChangeEvent(Items);
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public IEnumerator<TReadOnlyElement> GetEnumerator()
            => Items.Cast<TReadOnlyElement>().GetEnumerator();

        /// <inheritdoc/>
        public TReadOnlyElement Get(int index)
        {
            ValidateGet(index);
            return GetInternal(index);
        }

        /// <inheritdoc/>
        public IEnumerable<TReadOnlyElement> GetRange(int index, int count)
        {
            ValidateGetRange(index, count);
            return GetRangeInternal(index, count);
        }

        /// <inheritdoc/>
        public void ValidateGet(int index)
        {
            Validator?.Get((nameof(index), index), ("count", 1));
        }

        /// <inheritdoc/>
        public void ValidateGetRange(int index, int count)
        {
            Validator?.Get((nameof(index), index), (nameof(count), count));
        }

        /// <inheritdoc/>
        public TReadOnlyElement GetInternal(int index)
            => Items.Get(index, 1).First();

        /// <inheritdoc/>
        public IEnumerable<TReadOnlyElement> GetRangeInternal(int index, int count)
            => Items.Get(index, count).Cast<TReadOnlyElement>();

        /// <inheritdoc/>
        public bool ItemEquals(ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return Items.SequenceEqual(
                other.Items,
                EqualityComparerFactory.Create<TEditableElement>()
            );
        }

        /// <inheritdoc/>
        public bool ItemEquals(object? other)
            => ItemEquals(other as ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>);

        #endregion

        #region Interface Implicit

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion

        #region private

        /// <summary>
        ///     <see cref="SimpleList{T}"/> が通知した
        ///     <see cref="INotifyCollectionChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <param name="target">対象</param>
        private void PropagateCollectionChangeEvent(SimpleList<TEditableElement> target)
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }

        #endregion

        #endregion
    }
}
