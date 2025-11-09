// ========================================
// Project Name : WodiLib
// File Name    : IFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     長さが固定されたListインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> をベースに、要素の参照・編集のみ許可した機能。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TEditableElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TElementSettings">リスト要素設定DTO</typeparam>
    internal interface IFixedLengthList<TEditableElement, out TReadOnlyElement, in TElementSettings>
        where TElementSettings : notnull
    {
        #region Properties

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public TEditableElement this[int index] { get; set; }

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.Count"/>
        public int Count { get; }

        #endregion

        #region Methods

        #region CRUD

        /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.Get"/>
        public TEditableElement Get(int index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.GetRange"/>
        public IEnumerable<TEditableElement> GetRange(int index, int count);

        /// <summary>
        ///     リストの要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public TEditableElement Set(int index, TElementSettings settings);

        /// <summary>
        ///     リストの連続した要素を更新する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<TEditableElement> SetRange(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">[Range(0, <see cref="Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。
        /// </exception>
        public void Move(int oldIndex, int newIndex);

        /// <summary>
        ///     指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldIndex">
        ///     [Range(0, <see cref="Count"/> - 1)]
        ///     移動する項目のインデックス開始位置
        /// </param>
        /// <param name="newIndex">
        ///     [Range(0, <see cref="Count"/> - 1)]
        ///     移動先のインデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="Count"/>)]
        ///     移動させる要素数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の要素数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="settings">リストに詰め直す要素</param>
        /// <returns>新たにリストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> の要素数が <see cref="Count"/> と
        ///     異なる場合。
        /// </exception>
        public IEnumerable<TEditableElement> Reset(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     要素をデフォルト値で一新する。
        /// </summary>
        public IEnumerable<TEditableElement> Reset();

        #endregion

        #region Validate

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.ValidateGet"/>
        public void ValidateGet(int index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.ValidateGetRange"/>
        public void ValidateGetRange(int index, int count);

        /// <summary>
        ///     <see cref="Set"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Set" path="param|exception"/>
        public void ValidateSet(int index, TElementSettings settings);

        /// <summary>
        ///     <see cref="SetRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetRange" path="param|exception"/>
        public void ValidateSetRange(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Move"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Move" path="param|exception"/>
        public void ValidateMove(int oldIndex, int newIndex);

        /// <summary>
        ///     <see cref="MoveRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveRange" path="param|exception"/>
        public void ValidateMoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})" path="param|exception"/>
        public void ValidateReset(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Reset()"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset()" path="param|exception"/>
        public void ValidateReset();

        #endregion

        #region CRUD core

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetInternal"/>
        public TEditableElement GetInternal(int index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetRangeInternal"/>
        public IEnumerable<TEditableElement> GetRangeInternal(int index, int count);

        /// <summary>
        ///     <see cref="Set"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Set" path="param"/>
        public TEditableElement SetInternal(int index, TElementSettings settings);

        /// <summary>
        ///     <see cref="SetRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetRange" path="param"/>
        public IEnumerable<TEditableElement> SetRangeInternal(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Move"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Move" path="param"/>
        public void MoveInternal(int oldIndex, int newIndex);

        /// <summary>
        ///     <see cref="MoveRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveRange" path="param"/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})" path="param"/>
        public IEnumerable<TEditableElement> ResetInternal(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Reset()"/> メソッド処理中核。
        /// </summary>
        public IEnumerable<TEditableElement> ResetInternal();

        #endregion

        #endregion
    }
}
