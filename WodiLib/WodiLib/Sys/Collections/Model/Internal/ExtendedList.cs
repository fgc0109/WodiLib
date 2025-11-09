// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量制限のあるListクラス
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> をベースに、容量制限を設けた機能。
    ///         <see cref="ObservableCollection{T}"/> のCRUD各種処理に範囲指定バージョン（XXXRange メソッド）を追加している。
    ///         それ以外にもいくつかメソッドを追加している。
    ///     </para>
    ///     <para>
    ///         <typeparamref name="TElementImpl"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TListSettings">リストの入力パラメータ型</typeparam>
    /// <typeparam name="TElementImpl">リスト要素型</typeparam>
    /// <typeparam name="TElementSettings">リスト要素の入力パラメータ型</typeparam>
    internal class ExtendedList<TListSettings, TElementImpl, TElementSettings> :
        ModelBase,
        IExtendedList<TElementImpl, TElementSettings>,
        IEnumerable<TElementImpl>,
        INotifyCollectionChanged,
        IEqualityComparable<ExtendedList<TListSettings, TElementImpl, TElementSettings>>
        where TListSettings : IListSettings<TElementSettings>
        where TElementImpl : TElementSettings
        where TElementSettings : notnull
    {
        #region Delegates

        #region public

        /// <summary>
        ///     入力パラメータからリスト内部で保持するインスタンスを生成する処理
        /// </summary>
        public delegate TElementImpl BuildItemFromSettingsDelegate(int index, TElementSettings settings);

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

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

        /// <inheritdoc/>
        public TElementImpl this[int index]
        {
            [Pure] get => Get(index);
            set => Set(index, value);
        }

        /// <inheritdoc/>
        public int Count => Items.Count;

        #endregion

        #region private protected

        private protected SimpleList<TElementImpl> Items { get; }

        private protected IWodiLibListValidator<TListSettings, TElementSettings>? Validator { get; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private protected BuildItemFromSettingsDelegate BuildItemFromSettings { get; }

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        private readonly int minCapacity;
        private readonly int maxCapacity;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="itemsImpl">リスト実装インスタンス</param>
        /// <param name="minCapacity">容量最小値</param>
        /// <param name="maxCapacity">容量最大値</param>
        /// <param name="validator">各種引数検証バリデーター実装</param>
        /// <param name="buildItemFromSettings">入力パラメータからリスト内部で保持するインスタンスを生成する処理</param>
        internal ExtendedList(
            SimpleList<TElementImpl> itemsImpl,
            int minCapacity,
            int maxCapacity,
            IWodiLibListValidator<TListSettings, TElementSettings>? validator,
            BuildItemFromSettingsDelegate buildItemFromSettings
        )
        {
            ThrowHelper.ValidateArgumentNotNull(itemsImpl is null, nameof(itemsImpl));

            BuildItemFromSettings = buildItemFromSettings;

            this.maxCapacity = maxCapacity;
            this.minCapacity = minCapacity;

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

        #region Capacity

        /// <inheritdoc/>
        [Pure]
        public int GetMaxCapacity() => maxCapacity;

        /// <inheritdoc/>
        [Pure]
        public int GetMinCapacity() => minCapacity;

        #endregion

        #region IEnumerable

        /// <inheritdoc/>
        [Pure]
        public IEnumerator<TElementImpl> GetEnumerator()
            => Items.GetEnumerator();

        #endregion

        #region CRUD

        /// <inheritdoc/>
        [Pure]
        public TElementImpl Get(int index)
        {
            ValidateGet(index);
            return GetInternal(index);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TElementImpl> GetRange(int index, int count)
        {
            ValidateGetRange(index, count);
            return GetRangeInternal(index, count);
        }

        /// <inheritdoc/>
        public TElementImpl Set(int index, TElementSettings settings)
        {
            ValidateSet(index, settings);
            return SetInternal(index, settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> SetRange(int index, IEnumerable<TElementSettings>? settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings?.ToArray()!;
            ValidateSetRange(index, settingsArray);
            return SetRangeInternal(index, settingsArray);
        }

        /// <inheritdoc/>
        public TElementImpl Add(TElementSettings settings)
        {
            ValidateAdd(settings);
            return AddInternal(settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> AddRange(IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateAddRange(settingsArray);
            return AddRangeInternal(settingsArray);
        }

        /// <inheritdoc/>
        public TElementImpl Insert(int index, TElementSettings settings)
        {
            ValidateInsert(index, settings);
            return InsertInternal(index, settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> InsertRange(int index, IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateInsertRange(index, settingsArray);
            return InsertRangeInternal(index, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> Overwrite(int index, IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateOverwrite(index, settingsArray);
            return OverwriteInternal(index, settingsArray);
        }

        /// <inheritdoc/>
        public void Move(int oldIndex, int newIndex)
        {
            ValidateMove(oldIndex, newIndex);
            MoveInternal(oldIndex, newIndex);
        }

        /// <inheritdoc/>
        public void MoveRange(int oldIndex, int newIndex, int count)
        {
            ValidateMoveRange(oldIndex, newIndex, count);
            MoveRangeInternal(oldIndex, newIndex, count);
        }

        /// <inheritdoc/>
        public TElementImpl Remove(int index)
        {
            ValidateRemove(index);
            return RemoveInternal(index);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> RemoveRange(int index, int count)
        {
            ValidateRemoveRange(index, count);
            return RemoveRangeInternal(index, count);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> AdjustLength(int length)
        {
            ValidateAdjustLength(length);
            return AdjustLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> AdjustLengthIfShort(int length)
        {
            ValidateAdjustLength(length);
            if (Count >= length)
            {
                return Array.Empty<TElementImpl>();
            }

            return AdjustLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> AdjustLengthIfLong(int length)
        {
            ValidateAdjustLength(length);
            if (Count <= length)
            {
                return Array.Empty<TElementImpl>();
            }

            return AdjustLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> Reset(IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateReset(settingsArray);
            return ResetInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> ResetStrict(IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateResetStrict(settingsArray);
            return ResetStrictInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> Reset()
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

        #region Validation

        /// <inheritdoc/>
        public void ValidateGet(int index)
            => Validator?.Get((nameof(index), index), ("count", 1));

        /// <inheritdoc/>
        public void ValidateGetRange(int index, int count)
            => Validator?.Get((nameof(index), index), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateSet(int index, TElementSettings settings)
            => Validator?.Set((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateSetRange(int index, IEnumerable<TElementSettings> settings)
            => Validator?.Set((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAdd(TElementSettings settings)
            => Validator?.Insert(("index", Items.Count), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateAddRange(IEnumerable<TElementSettings> settings)
            => Validator?.Insert(("index", Items.Count), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsert(int index, TElementSettings settings)
            => Validator?.Insert((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateInsertRange(int index, IEnumerable<TElementSettings> settings)
            => Validator?.Insert((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateOverwrite(int index, IEnumerable<TElementSettings> settings)
            => Validator?.Overwrite((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateMove(int oldIndex, int newIndex)
            => Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex));

        /// <inheritdoc/>
        public void ValidateMoveRange(int oldIndex, int newIndex, int count)
            => Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateRemove(int index)
            => Validator?.Remove((nameof(index), index));

        /// <inheritdoc/>
        public void ValidateRemoveRange(int index, int count)
            => Validator?.Remove((nameof(index), index), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateAdjustLength(int length)
            => Validator?.AdjustLength((nameof(length), length));

        /// <inheritdoc/>
        public void ValidateReset(IEnumerable<TElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateResetStrict(IEnumerable<TElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings), canChangeSize: false);

        /// <inheritdoc/>
        public void ValidateReset()
            => Validator?.Reset();

        /// <inheritdoc/>
        public void ValidateClear()
            => Validator?.Clear();

        #endregion

        #region CRUD Core

        /// <inheritdoc/>
        [Pure]
        public TElementImpl GetInternal(int index)
            => Items.Get(index, 1).First();

        /// <inheritdoc/>
        [Pure]
        public IEnumerable<TElementImpl> GetRangeInternal(int index, int count)
            => Items.Get(index, count);

        /// <inheritdoc/>
        public TElementImpl SetInternal(int index, TElementSettings settings)
            => Items.Set(index, BuildItemFromSettings(index, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> SetRangeInternal(int index, IEnumerable<TElementSettings> settings)
            => Items.Set(
                index,
                settings.Select((dto, i) => BuildItemFromSettings(index + i, dto)).ToArray()
            );

        /// <inheritdoc/>
        public TElementImpl AddInternal(TElementSettings settings)
            => Items.Add(BuildItemFromSettings(Items.Count, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> AddRangeInternal(IEnumerable<TElementSettings> settings)
            => Items.Add(
                settings.Select((setting, i) => BuildItemFromSettings(Items.Count + i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public TElementImpl InsertInternal(int index, TElementSettings settings)
            => Items.Insert(index, BuildItemFromSettings(index, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> InsertRangeInternal(int index, IEnumerable<TElementSettings> settings)
            => Items.Insert(
                index,
                settings.Select((setting, i) => BuildItemFromSettings(index + i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> OverwriteInternal(int index, IEnumerable<TElementSettings> settings)
            => Items.Overwrite(
                index,
                settings.Select((setting, i) => BuildItemFromSettings(index + i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public void MoveInternal(int oldIndex, int newIndex)
            => Items.Move(oldIndex, newIndex);

        /// <inheritdoc/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count)
            => Items.Move(oldIndex, newIndex, count);

        /// <inheritdoc/>
        public TElementImpl RemoveInternal(int index)
        {
            var removeItem = Items[index];
            Items.RemoveAt(index);
            return removeItem;
        }

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> RemoveRangeInternal(int index, int count)
            => Items.Remove(index, count);

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> AdjustLengthInternal(int length)
            => Items.Adjust(length);

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> ResetInternal(IEnumerable<TElementSettings> settings)
            => Items.Reset(
                settings.Select((setting, i) => BuildItemFromSettings(i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> ResetStrictInternal(IEnumerable<TElementSettings> settings)
            => Items.Reset(
                settings.Select((setting, i) => BuildItemFromSettings(i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public IEnumerable<TElementImpl> ResetInternal()
            => Items.Reset(Items.Count);

        /// <inheritdoc/>
        public void ClearInternal()
            => Items.Reset(minCapacity);

        #endregion

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ExtendedList<TListSettings, TElementImpl, TElementSettings>? other)
        {
            if (ReferenceEquals(other, this)) return true;
            if (ReferenceEquals(other, null)) return false;

            return Items.SequenceEqual(
                other.Items,
                EqualityComparerFactory.Create<TElementImpl>()
            );
        }

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other)
            => ItemEquals(
                other as ExtendedList<TListSettings, TElementImpl, TElementSettings>
            );

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
        private void PropagateCollectionChangeEvent(SimpleList<TElementImpl> target)
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }

        #endregion

        #endregion
    }
}
