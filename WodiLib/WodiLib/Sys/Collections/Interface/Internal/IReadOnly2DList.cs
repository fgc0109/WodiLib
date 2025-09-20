// ========================================
// Project Name : WodiLib
// File Name    : IReadOnly2DList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する読み取り専用二次元リストインタフェース
    /// </summary>
    /// <typeparam name="TReadOnlyRowElement">行要素読取専用型</typeparam>
    /// <typeparam name="TReadOnlyListElement">リスト要素読取専用型</typeparam>
    internal interface IReadOnly2DList<out TReadOnlyRowElement, out TReadOnlyListElement>
    {
        #region Properties

        /// <summary>
        ///     行インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定した行インデックスの行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>が指定範囲外の場合。
        /// </exception>
        public TReadOnlyRowElement this[int rowIndex] { get; }

        /// <summary>
        ///     セルインデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定した行・列インデックスのセル要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="columnIndex"/>が指定範囲外の場合。
        /// </exception>
        public TReadOnlyListElement this[int rowIndex, int columnIndex] { get; }

        /// <summary>行数</summary>
        public int RowCount { get; }

        /// <summary>列数</summary>
        public int ColumnCount { get; }

        #endregion

        #region Methods

        #region CRUD

        #region Row

        /// <summary>
        ///     指定行インデックスの行要素を取得する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定行の行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        public TReadOnlyRowElement GetRow(int rowIndex);

        /// <summary>
        ///     指定範囲の行要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="count">[Range(0, <see cref="RowCount"/>)] 行数</param>
        /// <returns>指定範囲の行要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の行要素を取得しようとした場合。</exception>
        public IEnumerable<TReadOnlyRowElement> GetRowRange(int rowIndex, int count);

        #endregion

        #region Column

        /// <summary>
        ///     指定列インデックスの列要素を取得する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定列の要素リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TReadOnlyListElement> GetColumn(int columnIndex);

        /// <summary>
        ///     指定範囲の列要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <param name="count">[Range(0, <see cref="ColumnCount"/>)] 列数</param>
        /// <returns>指定範囲の列要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の列要素を取得しようとした場合。</exception>
        public IEnumerable<IEnumerable<TReadOnlyListElement>> GetColumnRange(int columnIndex, int count);

        #endregion

        #region Cell

        /// <summary>
        ///     指定行・列インデックスのセル要素を取得する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定行・列のセル要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        public TReadOnlyListElement GetCell(int rowIndex, int columnIndex);

        #endregion

        #endregion

        #region Validate

        /// <summary>
        ///     <see cref="GetRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetRow" path="param|exception"/>
        public void ValidateGetRow(int rowIndex);

        /// <summary>
        ///     <see cref="GetRowRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetRowRange" path="param|exception"/>
        public void ValidateGetRowRange(int rowIndex, int count);

        /// <summary>
        ///     <see cref="GetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetColumn" path="param|exception"/>
        public void ValidateGetColumn(int columnIndex);

        /// <summary>
        ///     <see cref="GetColumnRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetColumnRange" path="param|exception"/>
        public void ValidateGetColumnRange(int columnIndex, int count);

        /// <summary>
        ///     <see cref="GetCell"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetCell" path="param|exception"/>
        public void ValidateGetCell(int rowIndex, int columnIndex);

        #endregion

        #region CRUD core

        /// <summary>
        ///     <see cref="GetRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRow" path="param"/>
        public TReadOnlyRowElement GetRowInternal(int rowIndex);

        /// <summary>
        ///     <see cref="GetRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRowRange" path="param"/>
        public IEnumerable<TReadOnlyRowElement> GetRowRangeInternal(int rowIndex, int count);

        /// <summary>
        ///     <see cref="GetColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetColumn" path="param"/>
        public IEnumerable<TReadOnlyListElement> GetColumnInternal(int columnIndex);

        /// <summary>
        ///     <see cref="GetColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetColumnRange" path="param"/>
        public IEnumerable<IEnumerable<TReadOnlyListElement>> GetColumnRangeInternal(int columnIndex, int count);

        /// <summary>
        ///     <see cref="GetCell"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetCell" path="param"/>
        public TReadOnlyListElement GetCellInternal(int rowIndex, int columnIndex);

        #endregion

        #endregion
    }
}
