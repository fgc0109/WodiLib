// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量固定のList基底クラス
    /// </summary>
    /// <remarks>
    ///     機能概要は <seealso cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}"/> 参照。
    /// </remarks>
    /// <typeparam name="TEditableElement">リスト要素型（編集可能）</typeparam>
    /// <typeparam name="TReadOnlyElement">リスト要素型（読取専用）</typeparam>
    /// <typeparam name="TElementSettings">リスト内包型の入力パラメータ型</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class FixedLengthList<TEditableElement, TReadOnlyElement, TElementSettings> :
        ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>,
        IFixedLengthList<TEditableElement, TReadOnlyElement, TElementSettings>,
        IReadOnlyList<TEditableElement>,
        IEqualityComparable<FixedLengthList<TEditableElement, TReadOnlyElement, TElementSettings>>
        where TEditableElement : TReadOnlyElement, TElementSettings
        where TReadOnlyElement : TElementSettings
        where TElementSettings : notnull
    {
        #region Delegates

        #region public

        /// <summary>
        ///     入力パラメータからリスト内部で保持するインスタンスを生成する処理
        /// </summary>
        public delegate TEditableElement BuildItemFromSettingsDelegate(int index, TElementSettings settings);

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="IReadOnlyCollection{T}.Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public new TEditableElement this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        #endregion

        #region private protected

        private protected BuildItemFromSettingsDelegate BuildItemFromSettings { get; }

        private protected new IWodiLibListValidator<TElementSettings>? Validator
            => base.Validator;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="itemsImpl">リスト実装インスタンス</param>
        /// <param name="validator">各種引数検証バリデーター実装</param>
        /// <param name="buildItemFromSettings">入力パラメータからリスト内部で保持するインスタンスを生成する処理</param>
        internal FixedLengthList(
            SimpleList<TEditableElement> itemsImpl,
            IWodiLibListValidator<TElementSettings>? validator,
            BuildItemFromSettingsDelegate buildItemFromSettings
        ) : base(
            new Func<SimpleList<TEditableElement>>(() =>
                {
                    ThrowHelper.ValidateArgumentNotNull(buildItemFromSettings is null, nameof(buildItemFromSettings));

                    return itemsImpl;
                }
            )(),
            validator
        )
        {
            BuildItemFromSettings = buildItemFromSettings;
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public new IEnumerator<TEditableElement> GetEnumerator() => Items.Cast<TEditableElement>().GetEnumerator();

        #region CRUD

        /// <inheritdoc/>
        public new TEditableElement Get(int index)
        {
            ValidateGet(index);
            return GetInternal(index);
        }

        /// <inheritdoc/>
        public new IEnumerable<TEditableElement> GetRange(int index, int count)
        {
            ValidateGetRange(index, count);
            return GetRangeInternal(index, count);
        }

        /// <inheritdoc/>
        public TEditableElement Set(int index, TElementSettings settings)
        {
            ValidateSet(index, settings);
            return SetInternal(index, settings);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> SetRange(int index, IEnumerable<TElementSettings>? settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings?.ToArray()!;
            ValidateSetRange(index, settingsArray);
            return SetRangeInternal(index, settingsArray);
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
        public IEnumerable<TEditableElement> Reset(IEnumerable<TElementSettings>? settings)
        {
            var settingsArray = settings as TElementSettings[] ?? settings?.ToArray()!;
            ValidateReset(settingsArray);
            return ResetInternal(settingsArray);
        }

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> Reset()
        {
            ValidateReset();
            return ResetInternal();
        }

        #endregion

        #region Validation

        /// <inheritdoc/>
        public void ValidateSet(int index, TElementSettings settings)
            => Validator?.Set((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateSetRange(int index, IEnumerable<TElementSettings> settings)
            => Validator?.Set((nameof(index), index), (nameof(settings), settings));

        /// <inheritdoc/>
        public void ValidateMove(int oldIndex, int newIndex)
            => Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex));

        /// <inheritdoc/>
        public void ValidateMoveRange(int oldIndex, int newIndex, int count)
            => Validator?.Move((nameof(oldIndex), oldIndex), (nameof(newIndex), newIndex), (nameof(count), count));

        /// <inheritdoc/>
        public void ValidateReset(IEnumerable<TElementSettings> settings)
            => Validator?.Reset((nameof(settings), settings), canChangeSize: false);

        /// <inheritdoc/>
        public void ValidateReset()
            => Validator?.Reset();

        #endregion

        #region CRUD core

        /// <inheritdoc/>
        public new TEditableElement GetInternal(int index)
            => Items.Get(index, 1).First();

        /// <inheritdoc/>
        public new IEnumerable<TEditableElement> GetRangeInternal(int index, int count)
            => Items.Get(index, count);

        /// <inheritdoc/>
        public TEditableElement SetInternal(int index, TElementSettings settings)
            => Items.Set(index, BuildItemFromSettings(index, settings)).First();

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> SetRangeInternal(int index, IEnumerable<TElementSettings> settings)
            => Items.Set(
                index,
                settings.Select((dto, i) => BuildItemFromSettings(index + i, dto)).ToArray()
            );

        /// <inheritdoc/>
        public void MoveInternal(int oldIndex, int newIndex)
            => Items.Move(oldIndex, newIndex);

        /// <inheritdoc/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count)
            => Items.Move(oldIndex, newIndex, count);

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> ResetInternal(IEnumerable<TElementSettings> settings)
            => Items.Reset(settings.Select((dto, i) => BuildItemFromSettings(i, dto)).ToArray());

        /// <inheritdoc/>
        public IEnumerable<TEditableElement> ResetInternal()
            => Items.Reset(Items.Count);

        /// <inheritdoc/>
        public bool ItemEquals(FixedLengthList<TEditableElement, TReadOnlyElement, TElementSettings>? other)
            => ItemEquals((ReadOnlyExtendedList<TEditableElement, TReadOnlyElement, TElementSettings>?)other);

        #endregion

        #endregion

        #region Interface Implementations

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #endregion

        #endregion
    }
}
