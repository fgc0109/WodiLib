// ========================================
// Project Name : WodiLib
// File Name    : IFixed2DList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する長さが固定された二次元リストインタフェース
    /// </summary>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TReadOnlyRowElement">行要素読取専用型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TEditableListElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyListElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal interface IFixedLength2DList<
        TFixedRowElement,
        out TReadOnlyRowElement,
        in TRowElementSettings,
        TEditableListElement,
        out TReadOnlyListElement,
        in TListElementSettings>
    {
        #region Properties

        /// <summary>
        ///     行インデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定した行インデックスの行要素（長さ固定型）</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        public TFixedRowElement this[int rowIndex] { get; set; }

        /// <summary>
        ///     セルインデクサによるアクセス
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定した行・列インデックスのセル要素</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>, <paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        public TEditableListElement this[int rowIndex, int columnIndex] { get; set; }

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.RowCount"/>
        public int RowCount { get; }

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ColumnCount"/>
        public int ColumnCount { get; }

        #endregion

        #region Methods

        #region CRUD

        #region Row

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetRow"/>
        public TFixedRowElement GetRow(int rowIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetRowRange"/>
        public IEnumerable<TFixedRowElement> GetRowRange(int rowIndex, int count);

        /// <summary>
        ///     二次元リストの行要素を更新する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 更新行インデックス</param>
        /// <param name="settings">更新行要素</param>
        /// <returns>セットした行要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の行要素を編集しようとした場合。
        /// </exception>
        public TFixedRowElement SetRow(int rowIndex, TRowElementSettings settings);

        /// <summary>
        ///     二次元リストの連続した行要素を更新する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 更新開始行インデックス</param>
        /// <param name="settings">更新行要素</param>
        /// <returns>セットした行要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の行要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> SetRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     指定した行インデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRowIndex">[Range(0, <see cref="RowCount"/> - 1)] 移動する行のインデックス</param>
        /// <param name="newRowIndex">[Range(0, <see cref="RowCount"/> - 1)] 移動先の行インデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の行数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRowIndex"/>, <paramref name="newRowIndex"/> が指定範囲外の場合。
        /// </exception>
        public void MoveRow(int oldRowIndex, int newRowIndex);

        /// <summary>
        ///     指定した行インデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldRowIndex">
        ///     [Range(0, <see cref="RowCount"/> - 1)]
        ///     移動する行のインデックス開始位置
        /// </param>
        /// <param name="newRowIndex">
        ///     [Range(0, <see cref="RowCount"/> - 1)]
        ///     移動先の行インデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="RowCount"/>)]
        ///     移動させる行数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の行数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldRowIndex"/>, <paramref name="newRowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の行要素を移動しようとした場合。</exception>
        public void MoveRowRange(int oldRowIndex, int newRowIndex, int count);

        #endregion

        #region Column

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetColumn"/>
        public IEnumerable<TEditableListElement> GetColumn(int columnIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetColumnRange"/>
        public IEnumerable<IEnumerable<TEditableListElement>> GetColumnRange(int columnIndex, int count);

        /// <summary>
        ///     二次元リストの列要素を更新する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 更新列インデックス</param>
        /// <param name="settings">更新列要素</param>
        /// <returns>セットした列要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の列要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<TEditableListElement> SetColumn(int columnIndex, IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     二次元リストの連続した列要素を更新する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 更新開始列インデックス</param>
        /// <param name="settings">更新列要素（外側のIEnumerableが列、内側のIEnumerableが各列の行要素）</param>
        /// <returns>セットした列要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の列要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> SetColumnRange(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     指定した列インデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 移動する列のインデックス</param>
        /// <param name="newColumnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 移動先の列インデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身の列数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumnIndex"/>, <paramref name="newColumnIndex"/> が指定範囲外の場合。
        /// </exception>
        public void MoveColumn(int oldColumnIndex, int newColumnIndex);

        /// <summary>
        ///     指定した列インデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldColumnIndex">
        ///     [Range(0, <see cref="ColumnCount"/> - 1)]
        ///     移動する列のインデックス開始位置
        /// </param>
        /// <param name="newColumnIndex">
        ///     [Range(0, <see cref="ColumnCount"/> - 1)]
        ///     移動先の列インデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ColumnCount"/>)]
        ///     移動させる列数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身の列数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldColumnIndex"/>, <paramref name="newColumnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外の列要素を移動しようとした場合。</exception>
        public void MoveColumnRange(int oldColumnIndex, int newColumnIndex, int count);

        #endregion

        #region Cell

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetCell"/>
        public TEditableListElement GetCell(int rowIndex, int columnIndex);

        /// <summary>
        ///     二次元リストのセル要素を更新する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <param name="settings">更新セル要素</param>
        /// <returns>セットしたセル要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="rowIndex"/>, <paramref name="columnIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のセル要素を編集しようとした場合。
        /// </exception>
        public TEditableListElement SetCell(int rowIndex, int columnIndex, TListElementSettings settings);

        #endregion

        #region Reset

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="settings">二次元リストに詰め直す要素</param>
        /// <returns>新たに二次元リストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> の行数が <see cref="RowCount"/>、
        ///     列数が <see cref="ColumnCount"/> と異なる場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> Reset(
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     要素をデフォルト値で一新する。
        /// </summary>
        public IEnumerable<TFixedRowElement> Reset();

        #endregion

        #endregion

        #region Validate

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ValidateGetRow"/>
        public void ValidateGetRow(int rowIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ValidateGetRowRange"/>
        public void ValidateGetRowRange(int rowIndex, int count);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ValidateGetColumn"/>
        public void ValidateGetColumn(int columnIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ValidateGetColumnRange"/>
        public void ValidateGetColumnRange(int columnIndex, int count);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ValidateGetCell"/>
        public void ValidateGetCell(int rowIndex, int columnIndex);

        /// <summary>
        ///     <see cref="SetRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetRow" path="param|exception"/>
        public void ValidateSetRow(int rowIndex, TRowElementSettings settings);

        /// <summary>
        ///     <see cref="SetRowRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetRowRange" path="param|exception"/>
        public void ValidateSetRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     <see cref="SetColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetColumn" path="param|exception"/>
        public void ValidateSetColumn(int columnIndex, IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     <see cref="SetColumnRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetColumnRange" path="param|exception"/>
        public void ValidateSetColumnRange(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings);

        /// <summary>
        ///     <see cref="SetCell"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetCell" path="param|exception"/>
        public void ValidateSetCell(int rowIndex, int columnIndex, TListElementSettings settings);

        /// <summary>
        ///     <see cref="MoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveRow" path="param|exception"/>
        public void ValidateMoveRow(int oldRowIndex, int newRowIndex);

        /// <summary>
        ///     <see cref="MoveRowRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveRowRange" path="param|exception"/>
        public void ValidateMoveRowRange(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="MoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveColumn" path="param|exception"/>
        public void ValidateMoveColumn(int oldColumnIndex, int newColumnIndex);

        /// <summary>
        ///     <see cref="MoveColumnRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveColumnRange" path="param|exception"/>
        public void ValidateMoveColumnRange(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see
        ///         cref="Reset(System.Collections.Generic.IEnumerable{TRowElementSettings})"/>
        ///     メソッドの検証処理。
        /// </summary>
        /// <inheritdoc
        ///     cref="Reset(System.Collections.Generic.IEnumerable{TRowElementSettings})"
        ///     path="param|exception"/>
        public void ValidateReset(IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     <see cref="Reset()"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset()" path="param|exception"/>
        public void ValidateReset();

        #endregion

        #region CRUD core

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetRowInternal"/>
        public TFixedRowElement GetRowInternal(int rowIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetRowRangeInternal"/>
        public IEnumerable<TFixedRowElement> GetRowRangeInternal(int rowIndex, int count);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetColumnInternal"/>
        public IEnumerable<TEditableListElement> GetColumnInternal(int columnIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetColumnRangeInternal"/>
        public IEnumerable<IEnumerable<TEditableListElement>> GetColumnRangeInternal(int columnIndex, int count);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetCellInternal"/>
        public TEditableListElement GetCellInternal(int rowIndex, int columnIndex);

        /// <summary>
        ///     <see cref="SetRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetRow" path="param"/>
        public TFixedRowElement SetRowInternal(int rowIndex, TRowElementSettings settings);

        /// <summary>
        ///     <see cref="SetRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetRowRange" path="param"/>
        public IEnumerable<TFixedRowElement> SetRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     <see cref="SetColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetColumn" path="param"/>
        public IEnumerable<TEditableListElement> SetColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        );

        /// <summary>
        ///     <see cref="SetColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetColumnRange" path="param"/>
        public IEnumerable<IEnumerable<TEditableListElement>> SetColumnRangeInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     <see cref="SetCell"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetCell" path="param"/>
        public TEditableListElement SetCellInternal(int rowIndex, int columnIndex, TListElementSettings settings);

        /// <summary>
        ///     <see cref="MoveRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveRow" path="param"/>
        public void MoveRowInternal(int oldRowIndex, int newRowIndex);

        /// <summary>
        ///     <see cref="MoveRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveRowRange" path="param"/>
        public void MoveRowRangeInternal(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="MoveColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveColumn" path="param"/>
        public void MoveColumnInternal(int oldColumnIndex, int newColumnIndex);

        /// <summary>
        ///     <see cref="MoveColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveColumnRange" path="param"/>
        public void MoveColumnRangeInternal(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see
        ///         cref="Reset(System.Collections.Generic.IEnumerable{TRowElementSettings})"/>
        ///     メソッド処理中核。
        /// </summary>
        /// <inheritdoc
        ///     cref="Reset(System.Collections.Generic.IEnumerable{TRowElementSettings})"
        ///     path="param"/>
        public IEnumerable<TFixedRowElement> ResetInternal(
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     <see cref="Reset()"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset()" path="param|exception"/>
        public IEnumerable<TFixedRowElement> ResetInternal();

        #endregion

        #endregion
    }
}
