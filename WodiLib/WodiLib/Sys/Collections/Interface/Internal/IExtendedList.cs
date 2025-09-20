// ========================================
// Project Name : WodiLib
// File Name    : IExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib 独自リストインタフェース
    /// </summary>
    /// <remarks>
    ///     リストの編集・参照が可能。
    /// </remarks>
    /// <typeparam name="TEditableElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TElementSettings">リスト要素設定DTO</typeparam>
    internal interface IExtendedList<TEditableElement, out TReadOnlyElement, in TElementSettings>
        where TEditableElement : TReadOnlyElement
        where TReadOnlyElement : TElementSettings
        where TElementSettings : notnull
    {
        #region Properties

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.this[int]"/>
        public TEditableElement this[int index] { get; set; }

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.Count"/>
        public int Count { get; }

        #endregion

        #region Methods

        #region Capacity

        /// <summary>
        ///     最大容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public int GetMaxCapacity();

        /// <summary>
        ///     最大容量を返す。
        /// </summary>
        /// <returns>容量</returns>
        public int GetMinCapacity();

        #endregion

        #region CRUD

        /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.Get"/>
        public TEditableElement Get(int index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.GetRange"/>
        public IEnumerable<TEditableElement> GetRange(int index, int count);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.Set"/>
        public TEditableElement Set(int index, TElementSettings settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.SetRange"/>
        public IEnumerable<TEditableElement> SetRange(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public TEditableElement Add(TElementSettings settings);

        /// <summary>
        ///     リストの末尾に要素を追加する。
        /// </summary>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TEditableElement> AddRange(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/>)] インデックス</param>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public TEditableElement Insert(int index, TElementSettings settings);

        /// <summary>
        ///     指定したインデックスの位置に要素を挿入する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/>)] インデックス</param>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TEditableElement> InsertRange(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     指定したインデックスを起点として、要素の上書き/追加を行う。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/>)] インデックス</param>
        /// <param name="settings">上書き/追加リスト</param>
        /// <returns>上書きした要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。
        /// </exception>
        /// <example>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10, 11, 12 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 11, 12 }
        ///     </code>
        ///     <code>
        ///     var target = new List&lt;int&gt; { 0, 1, 2, 3 };
        ///     var dst = new List&lt;int&gt; { 10 };
        ///     target.Overwrite(2, dst);
        ///     // target is { 0, 1, 10, 3 }
        ///     </code>
        /// </example>
        public IEnumerable<TEditableElement> Overwrite(int index, IEnumerable<TElementSettings> settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.Move"/>
        public void Move(int oldIndex, int newIndex);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.MoveRange"/>
        public void MoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     指定したインデックスの要素を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public TEditableElement Remove(int index);

        /// <summary>
        ///     要素の範囲を削除する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="Count"/>)] 削除する要素数</param>
        /// <returns>削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<TEditableElement> RemoveRange(int index, int count);

        /// <summary>
        ///     要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <returns>追加または削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TEditableElement> AdjustLength(int length);

        /// <summary>
        ///     要素数が不足している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TEditableElement> AdjustLengthIfShort(int length);

        /// <summary>
        ///     要素数が超過している場合、要素数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]
        ///     調整する要素数
        /// </param>
        /// <returns>削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TEditableElement> AdjustLengthIfLong(int length);

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
        ///     <paramref name="settings"/> の要素数が <see cref="GetMinCapacity"/> 未満
        ///     または <see cref="GetMaxCapacity"/> を超える場合。
        /// </exception>
        /// <remarks>
        ///     このメソッドは <paramref name="settings"/> の要素数が
        ///     <see cref="GetMinCapacity"/> 以上 <see cref="GetMaxCapacity"/> 以下であれば
        ///     成功する。<br/>
        ///     現在の要素数と一致しない場合エラーとしたい場合は、
        ///     容量固定型にキャストしてから同メソッドを呼び出す。
        /// </remarks>
        public IEnumerable<TEditableElement> Reset(IEnumerable<TElementSettings> settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.Reset()"/>
        public IEnumerable<TEditableElement> Reset();

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public void Clear();

        #endregion

        #region Validate

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.ValidateGet"/>
        public void ValidateGet(int index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.ValidateGetRange"/>
        public void ValidateGetRange(int index, int count);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.ValidateSet"/>
        public void ValidateSet(int index, TElementSettings settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.ValidateSetRange"/>
        public void ValidateSetRange(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Add"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Add" path="param|exception"/>
        public void ValidateAdd(TElementSettings settings);

        /// <summary>
        ///     <see cref="AddRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddRange" path="param|exception"/>
        public void ValidateAddRange(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Insert"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Insert" path="param|exception"/>
        public void ValidateInsert(int index, TElementSettings settings);

        /// <summary>
        ///     <see cref="InsertRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertRange" path="param|exception"/>
        public void ValidateInsertRange(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Overwrite"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Overwrite" path="param|exception"/>
        public void ValidateOverwrite(int index, IEnumerable<TElementSettings> settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.ValidateMove"/>
        public void ValidateMove(int oldIndex, int newIndex);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.ValidateMoveRange"/>
        public void ValidateMoveRange(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="Remove"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Remove" path="param|exception"/>
        public void ValidateRemove(int index);

        /// <summary>
        ///     <see cref="RemoveRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveRange" path="param|exception"/>
        public void ValidateRemoveRange(int index, int count);

        /// <summary>
        ///     <see cref="AdjustLength"/>,
        ///     <see cref="AdjustLengthIfShort"/>,
        ///     <see cref="AdjustLengthIfLong"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustLength" path="param|exception"/>
        public void ValidateAdjustLength(int length);

        /// <summary>
        ///     <see cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})" path="param|exception"/>
        public void ValidateReset(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Clear"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param|exception"/>
        public void ValidateClear();

        #endregion

        #region CRUD core

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetInternal"/>
        public TEditableElement GetInternal(int index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetRangeInternal"/>
        public IEnumerable<TEditableElement> GetRangeInternal(int index, int count);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.SetInternal"/>
        public TEditableElement SetInternal(int index, TElementSettings settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.SetRangeInternal"/>
        public IEnumerable<TEditableElement> SetRangeInternal(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Add"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Add" path="param|returns"/>
        public TEditableElement AddInternal(TElementSettings settings);

        /// <summary>
        ///     <see cref="AddRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddRange" path="param|returns"/>
        public IEnumerable<TEditableElement> AddRangeInternal(
            IEnumerable<TElementSettings> settings
        );

        /// <summary>
        ///     <see cref="Insert"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Insert" path="param|returns"/>
        public TEditableElement InsertInternal(int index, TElementSettings settings);

        /// <summary>
        ///     <see cref="InsertRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertRange" path="param|returns"/>
        public IEnumerable<TEditableElement> InsertRangeInternal(int index, IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Overwrite"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Overwrite" path="param|returns"/>
        public IEnumerable<TEditableElement> OverwriteInternal(int index, IEnumerable<TElementSettings> settings);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.MoveInternal"/>
        public void MoveInternal(int oldIndex, int newIndex);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.MoveRangeInternal"/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     <see cref="Remove"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Remove" path="param|returns"/>
        public TEditableElement RemoveInternal(int index);

        /// <summary>
        ///     <see cref="RemoveRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveRange" path="param|returns"/>
        public IEnumerable<TEditableElement> RemoveRangeInternal(int index, int count);

        /// <summary>
        ///     <see cref="AdjustLength"/>,
        ///     <see cref="AdjustLengthIfShort"/>,
        ///     <see cref="AdjustLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustLength" path="param|returns"/>
        public IEnumerable<TEditableElement> AdjustLengthInternal(int length);

        /// <inheritdoc cref="IFixedLengthList{TEditableElement,TReadOnlyElement,TElementSettings}.SetRangeInternal"/>
        public IEnumerable<TEditableElement> ResetInternal(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Clear"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param"/>
        public void ClearInternal();

        #endregion

        #endregion
    }
}
