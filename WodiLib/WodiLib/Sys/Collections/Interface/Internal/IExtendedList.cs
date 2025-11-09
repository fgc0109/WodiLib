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
    ///     インタフェースの明確化・ドキュメントコメント切り出しのために定義する。
    /// </remarks>
    /// <typeparam name="TEditableElement">リスト要素型</typeparam>
    /// <typeparam name="TElementSettings">リスト要素設定DTO</typeparam>
    internal interface IExtendedList<TEditableElement, in TElementSettings>
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

        /// <summary>要素数</summary>
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

        /// <summary>
        ///     指定インデックスの要素を取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        public TEditableElement Get(int index);

        /// <summary>
        ///     指定範囲の要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
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
        ///     現在の要素数と一致しない場合エラーとしたい場合は、<see cref="ResetStrict"/> を使用する。
        /// </remarks>
        public IEnumerable<TEditableElement> Reset(IEnumerable<TElementSettings> settings);

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
        public IEnumerable<TEditableElement> ResetStrict(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     要素をデフォルト値で一新する。
        /// </summary>
        public IEnumerable<TEditableElement> Reset();

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public void Clear();

        #endregion

        #region Validate

        /// <summary>
        ///     <see cref="Get"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Get" path="param|exception"/>
        public void ValidateGet(int index);

        /// <summary>
        ///     <see cref="GetRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param|exception"/>
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
        ///     <see cref="ResetStrict"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ResetStrict" path="param|exception"/>
        public void ValidateResetStrict(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Reset()"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset()" path="param|exception"/>
        public void ValidateReset();

        /// <summary>
        ///     <see cref="Clear"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param|exception"/>
        public void ValidateClear();

        #endregion

        #region CRUD core

        /// <summary>
        ///     <see cref="Get"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Get" path="param"/>
        public TEditableElement GetInternal(int index);

        /// <summary>
        ///     <see cref="GetRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param"/>
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

        /// <summary>
        ///     <see cref="Reset(System.Collections.Generic.IEnumerable{TElementSettings})"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustLength" path="param|returns"/>
        public IEnumerable<TEditableElement> ResetInternal(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="ResetStrict"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ResetStrict" path="param"/>
        public IEnumerable<TEditableElement> ResetStrictInternal(IEnumerable<TElementSettings> settings);

        /// <summary>
        ///     <see cref="Reset()"/> メソッド処理中核。
        /// </summary>
        public IEnumerable<TEditableElement> ResetInternal();

        /// <summary>
        ///     <see cref="Clear"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param"/>
        public void ClearInternal();

        #endregion

        #endregion
    }
}
