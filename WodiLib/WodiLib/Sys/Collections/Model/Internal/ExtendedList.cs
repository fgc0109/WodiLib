// ========================================
// Project Name : WodiLib
// File Name    : ExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    ///         <typeparamref name="TReadOnlyElement"/> が変更通知を行うクラスだった場合、
    ///         通知を受け取ると自身の "Items[]" プロパティ変更通知を行う。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TEditableElement">リスト要素型（編集可能）</typeparam>
    /// <typeparam name="TReadOnlyElement">リスト要素型（読取専用）</typeparam>
    /// <typeparam name="TElementSettings">リスト内包型の入力パラメータ型</typeparam>
    internal class ExtendedList<TEditableElement, TReadOnlyElement, TElementSettings> :
        FixedLengthList<TEditableElement, TReadOnlyElement, TElementSettings>,
        IExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>,
        IEqualityComparable<ExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>>
        where TEditableElement : TReadOnlyElement, TElementSettings
        where TReadOnlyElement : TElementSettings
        where TElementSettings : notnull
    {
        #region Fields

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
            SimpleList<TEditableElement> itemsImpl,
            int minCapacity,
            int maxCapacity,
            IWodiLibListValidator<TElementSettings>? validator,
            BuildItemFromSettingsDelegate buildItemFromSettings
        ) : base(itemsImpl, validator, buildItemFromSettings)
        {
            this.maxCapacity = maxCapacity;
            this.minCapacity = minCapacity;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        #region Capacity

        /// <inheritdoc/>
        public int GetMaxCapacity() => maxCapacity;

        /// <inheritdoc/>
        public int GetMinCapacity() => minCapacity;

        #endregion

        #region CRUD

        /// <inheritdoc/>
        public TEditableElement Add(TElementSettings settings)
        {
            ValidateAdd(settings);
            return AddInternal(settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> AddRange(IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateAddRange(settingsArray);
            return AddRangeInternal(settingsArray);
        }

        /// <inheritdoc/>
        public TEditableElement Insert(int index, TElementSettings settings)
        {
            ValidateInsert(index, settings);
            return InsertInternal(index, settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> InsertRange(int index, IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateInsertRange(index, settingsArray);
            return InsertRangeInternal(index, settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> Overwrite(int index, IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
            ValidateOverwrite(index, settingsArray);
            return OverwriteInternal(index, settingsArray);
        }

        /// <inheritdoc/>
        public TEditableElement Remove(int index)
        {
            ValidateRemove(index);
            return RemoveInternal(index);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> RemoveRange(int index, int count)
        {
            ValidateRemoveRange(index, count);
            return RemoveRangeInternal(index, count);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> AdjustLength(int length)
        {
            ValidateAdjustLength(length);
            return AdjustLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> AdjustLengthIfShort(int length)
        {
            ValidateAdjustLength(length);
            if (Count >= length)
            {
                return Array.Empty<TEditableElement>();
            }

            return AdjustLengthInternal(length);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> AdjustLengthIfLong(int length)
        {
            ValidateAdjustLength(length);
            if (Count <= length)
            {
                return Array.Empty<TEditableElement>();
            }

            return AdjustLengthInternal(length);
        }

        /// <inheritdoc
        ///     cref="IExtendedList{TEditableElement,TReadOnlyElement,TElementSettings}.Reset(System.Collections.Generic.IEnumerable{TElementSettings})"/>
        public new IEnumerable<TEditableElement> Reset(IEnumerable<TElementSettings> settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings.ToArray();
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

        #region Validation

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
        public void ValidateRemove(int index)
            => Validator?.Remove((nameof(index), index));

        /// <inheritdoc/>
        public void ValidateRemoveRange(int index, int count)
            => Validator?.Remove((nameof(index), index), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateAdjustLength(int length)
            => Validator?.AdjustLength((nameof(length), length));

        /// <inheritdoc cref="Reset"/>
        public new void ValidateReset(IEnumerable<TElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateClear()
            => Validator?.Clear();

        #endregion

        #region CRUD Core

        /// <inheritdoc/>
        public TEditableElement AddInternal(TElementSettings settings)
            => Items.Add(BuildItemFromSettings(Items.Count, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> AddRangeInternal(IEnumerable<TElementSettings> settings)
            => Items.Add(
                settings.Select((setting, i) => BuildItemFromSettings(Items.Count + i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public TEditableElement InsertInternal(int index, TElementSettings settings)
            => Items.Insert(index, BuildItemFromSettings(index, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> InsertRangeInternal(int index, IEnumerable<TElementSettings> settings)
            => Items.Insert(
                index,
                settings.Select((setting, i) => BuildItemFromSettings(index + i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> OverwriteInternal(int index, IEnumerable<TElementSettings> settings)
            => Items.Overwrite(
                index,
                settings.Select((setting, i) => BuildItemFromSettings(index + i, setting)).ToArray()
            );


        /// <inheritdoc/>
        public TEditableElement RemoveInternal(int index)
        {
            var removeItem = Items[index];
            Items.RemoveAt(index);
            return removeItem;
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> RemoveRangeInternal(int index, int count)
            => Items.Remove(index, count);

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> AdjustLengthInternal(int length)
            => Items.Adjust(length);

        /// <inheritdoc cref="Reset"/>
        public new IEnumerable<TEditableElement> ResetInternal(IEnumerable<TElementSettings> settings)
            => Items.Reset(
                settings.Select((setting, i) => BuildItemFromSettings(i, setting)).ToArray()
            );

        /// <inheritdoc/>
        public void ClearInternal()
            => Items.Reset(minCapacity);

        #endregion

        /// <inheritdoc/>
        public bool ItemEquals(ExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>? other)
            => ItemEquals((ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>?)other);

        #endregion

        #endregion
    }
}
