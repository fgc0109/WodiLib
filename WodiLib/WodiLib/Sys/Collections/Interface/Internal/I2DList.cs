// ========================================
// Project Name : WodiLib
// File Name    : I2DList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     WodiLib内部で使用する二次元リストインタフェース
    /// </summary>
    /// <remarks>
    ///     二次元リストの編集・参照が可能。
    /// </remarks>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TReadOnlyRowElement">行要素読取専用型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TEditableListElement">リスト要素型</typeparam>
    /// <typeparam name="TReadOnlyListElement">リスト要素読取専用型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal interface I2DList<
        TFixedRowElement,
        out TReadOnlyRowElement,
        in TRowElementSettings,
        TEditableListElement,
        out TReadOnlyListElement,
        in TListElementSettings>
        where TFixedRowElement : TReadOnlyRowElement
        where TReadOnlyRowElement : TRowElementSettings
        where TRowElementSettings : notnull
        where TEditableListElement : TReadOnlyListElement
        where TReadOnlyListElement : TListElementSettings
        where TListElementSettings : notnull
    {
        #region Properties

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.this[int]"/>
        public TFixedRowElement this[int rowIndex] { get; set; }

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.this[int,int]"/>
        public TEditableListElement this[int rowIndex, int columnIndex] { get; set; }

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.RowCount"/>
        public int RowCount { get; }

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.ColumnCount"/>
        public int ColumnCount { get; }

        #endregion

        #region Methods

        #region Capacity

        /// <summary>
        ///     最大行数を返す。
        /// </summary>
        /// <returns>最大行数</returns>
        public int GetMaxRowCapacity();

        /// <summary>
        ///     最小行数を返す。
        /// </summary>
        /// <returns>最小行数</returns>
        public int GetMinRowCapacity();

        /// <summary>
        ///     最大列数を返す。
        /// </summary>
        /// <returns>最大列数</returns>
        public int GetMaxColumnCapacity();

        /// <summary>
        ///     最小列数を返す。
        /// </summary>
        /// <returns>最小列数</returns>
        public int GetMinColumnCapacity();

        #endregion

        #region CRUD

        #region Row

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetRow"/>
        public TFixedRowElement GetRow(int rowIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetRowRange"/>
        public IEnumerable<TFixedRowElement> GetRowRange(int rowIndex, int count);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetRow"/>
        public TFixedRowElement SetRow(int rowIndex, TRowElementSettings settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetRowRange"/>
        public IEnumerable<TFixedRowElement> SetRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     二次元リストの末尾に行要素を追加する。
        /// </summary>
        /// <param name="settings">追加する行要素</param>
        /// <returns>追加した行要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public TFixedRowElement AddRow(TRowElementSettings settings);

        /// <summary>
        ///     二次元リストの末尾に行要素を追加する。
        /// </summary>
        /// <param name="settings">追加する行要素</param>
        /// <returns>追加した行要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> AddRowRange(IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     指定した行インデックスの位置に行要素を挿入する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/>)] 行インデックス</param>
        /// <param name="settings">追加する行要素</param>
        /// <returns>追加した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public TFixedRowElement InsertRow(int rowIndex, TRowElementSettings settings);

        /// <summary>
        ///     指定した行インデックスの位置に行要素を挿入する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/>)] 行インデックス</param>
        /// <param name="settings">追加する行要素</param>
        /// <returns>追加した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> InsertRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     指定した行インデックスを起点として、行要素の上書き/追加を行う。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/>)] 行インデックス</param>
        /// <param name="settings">上書き/追加行リスト</param>
        /// <returns>上書きした行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMaxRowCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> OverwriteRow(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveRow"/>
        public void MoveRow(int oldRowIndex, int newRowIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveRowRange"/>
        public void MoveRowRange(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     指定した行インデックスの行要素を削除する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>削除した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public TFixedRowElement RemoveRow(int rowIndex);

        /// <summary>
        ///     行要素の範囲を削除する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <param name="count">[Range(0, <see cref="RowCount"/>)] 削除する行数</param>
        /// <returns>削除した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の行要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって行数が <see cref="GetMinRowCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> RemoveRowRange(int rowIndex, int count);

        /// <summary>
        ///     行数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <returns>追加または削除した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> AdjustRowLength(int length);

        /// <summary>
        ///     行数が不足している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <returns>追加した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> AdjustRowLengthIfShort(int length);

        /// <summary>
        ///     行数が超過している場合、行数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinRowCapacity"/>, <see cref="GetMaxRowCapacity"/>)]
        ///     調整する行数
        /// </param>
        /// <returns>削除した行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TFixedRowElement> AdjustRowLengthIfLong(int length);

        #endregion

        #region Column

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetColumn"/>
        public IEnumerable<TEditableListElement> GetColumn(int columnIndex);

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetColumnRange"/>
        public IEnumerable<IEnumerable<TEditableListElement>> GetColumnRange(int columnIndex, int count);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetColumn"/>
        public IEnumerable<TEditableListElement> SetColumn(int columnIndex, IEnumerable<TListElementSettings> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetColumnRange"/>
        public IEnumerable<IEnumerable<TEditableListElement>> SetColumnRange(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     二次元リストの末尾に列要素を追加する。
        /// </summary>
        /// <param name="settings">追加する列要素</param>
        /// <returns>追加した列要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TEditableListElement> AddColumn(IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     二次元リストの末尾に列要素を追加する。
        /// </summary>
        /// <param name="settings">追加する列要素（外側のIEnumerableが列、内側のIEnumerableが各列の行要素）</param>
        /// <returns>追加した列要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> AddColumnRange(
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     指定した列インデックスの位置に列要素を挿入する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/>)] 列インデックス</param>
        /// <param name="settings">追加する列要素</param>
        /// <returns>追加した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<TEditableListElement> InsertColumn(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        );

        /// <summary>
        ///     指定した列インデックスの位置に列要素を挿入する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/>)] 列インデックス</param>
        /// <param name="settings">追加する列要素（外側のIEnumerableが列、内側のIEnumerableが各列の行要素）</param>
        /// <returns>追加した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> InsertColumnRange(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     指定した列インデックスを起点として、列要素の上書き/追加を行う。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/>)] 列インデックス</param>
        /// <param name="settings">上書き/追加列リスト（外側のIEnumerableが列、内側のIEnumerableが各列の行要素）</param>
        /// <returns>上書きした列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMaxColumnCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> OverwriteColumn(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveColumn"/>
        public void MoveColumn(int oldColumnIndex, int newColumnIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveColumnRange"/>
        public void MoveColumnRange(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     指定した列インデックスの列要素を削除する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>削除した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<TEditableListElement> RemoveColumn(int columnIndex);

        /// <summary>
        ///     列要素の範囲を削除する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <param name="count">[Range(0, <see cref="ColumnCount"/>)] 削除する列数</param>
        /// <returns>削除した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の列要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によって列数が <see cref="GetMinColumnCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> RemoveColumnRange(int columnIndex, int count);

        /// <summary>
        ///     列数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <returns>追加または削除した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLength(int length);

        /// <summary>
        ///     列数が不足している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <returns>追加した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLengthIfShort(int length);

        /// <summary>
        ///     列数が超過している場合、列数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="GetMinColumnCapacity"/>, <see cref="GetMaxColumnCapacity"/>)]
        ///     調整する列数
        /// </param>
        /// <returns>削除した列要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLengthIfLong(int length);

        #endregion

        #region Cell

        /// <inheritdoc cref="IReadOnly2DList{TReadOnlyRowElement, TReadOnlyListElement}.GetCell"/>
        public TEditableListElement GetCell(int rowIndex, int columnIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetCell"/>
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
        ///     <paramref name="settings"/> の行数が <see cref="GetMinRowCapacity"/> 未満
        ///     または <see cref="GetMaxRowCapacity"/> を超える場合、
        ///     列数が <see cref="GetMinColumnCapacity"/> 未満
        ///     または <see cref="GetMaxColumnCapacity"/> を超える場合。
        /// </exception>
        /// <remarks>
        ///     このメソッドは <paramref name="settings"/> の行数が
        ///     <see cref="GetMinRowCapacity"/> 以上 <see cref="GetMaxRowCapacity"/> 以下、
        ///     列数が <see cref="GetMinColumnCapacity"/> 以上 <see cref="GetMaxColumnCapacity"/> 以下であれば
        ///     成功する。<br/>
        ///     現在の行数・列数と一致しない場合エラーとしたい場合は、
        ///     容量固定型にキャストしてから同メソッドを呼び出す。
        /// </remarks>
        public IEnumerable<TFixedRowElement> Reset(
            IEnumerable<TRowElementSettings> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.Reset()"/>
        public IEnumerable<TFixedRowElement> Reset();

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public void Clear();

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

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateSetRow"/>
        public void ValidateSetRow(int rowIndex, TRowElementSettings settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateSetRowRange"/>
        public void ValidateSetRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateSetColumn"/>
        public void ValidateSetColumn(int columnIndex, IEnumerable<TListElementSettings> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateSetColumnRange"/>
        public void ValidateSetColumnRange(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateSetCell"/>
        public void ValidateSetCell(int rowIndex, int columnIndex, TListElementSettings settings);

        /// <summary>
        ///     <see cref="AddRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddRow" path="param|exception"/>
        public void ValidateAddRow(TRowElementSettings settings);

        /// <summary>
        ///     <see cref="AddRowRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddRowRange" path="param|exception"/>
        public void ValidateAddRowRange(IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     <see cref="InsertRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertRow" path="param|exception"/>
        public void ValidateInsertRow(int rowIndex, TRowElementSettings settings);

        /// <summary>
        ///     <see cref="InsertRowRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertRowRange" path="param|exception"/>
        public void ValidateInsertRowRange(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     <see cref="OverwriteRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="OverwriteRow" path="param|exception"/>
        public void ValidateOverwriteRow(int rowIndex, IEnumerable<TRowElementSettings> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateMoveRow"/>
        public void ValidateMoveRow(int oldRowIndex, int newRowIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateMoveRowRange"/>
        public void ValidateMoveRowRange(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="RemoveRow"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveRow" path="param|exception"/>
        public void ValidateRemoveRow(int rowIndex);

        /// <summary>
        ///     <see cref="RemoveRowRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveRowRange" path="param|exception"/>
        public void ValidateRemoveRowRange(int rowIndex, int count);

        /// <summary>
        ///     <see cref="AdjustRowLength"/>,
        ///     <see cref="AdjustRowLengthIfShort"/>,
        ///     <see cref="AdjustRowLengthIfLong"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustRowLength" path="param|exception"/>
        public void ValidateAdjustRowLength(int length);

        /// <summary>
        ///     <see cref="AddColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddColumn" path="param|exception"/>
        public void ValidateAddColumn(IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     <see cref="AddColumnRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddColumnRange" path="param|exception"/>
        public void ValidateAddColumnRange(IEnumerable<IEnumerable<TListElementSettings>> settings);

        /// <summary>
        ///     <see cref="InsertColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertColumn" path="param|exception"/>
        public void ValidateInsertColumn(int columnIndex, IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     <see cref="InsertColumnRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertColumnRange" path="param|exception"/>
        public void ValidateInsertColumnRange(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings);

        /// <summary>
        ///     <see cref="OverwriteColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="OverwriteColumn" path="param|exception"/>
        public void ValidateOverwriteColumn(int columnIndex, IEnumerable<IEnumerable<TListElementSettings>> settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateMoveColumn"/>
        public void ValidateMoveColumn(int oldColumnIndex, int newColumnIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ValidateMoveColumnRange"/>
        public void ValidateMoveColumnRange(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see cref="RemoveColumn"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveColumn" path="param|exception"/>
        public void ValidateRemoveColumn(int columnIndex);

        /// <summary>
        ///     <see cref="RemoveColumnRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveColumnRange" path="param|exception"/>
        public void ValidateRemoveColumnRange(int columnIndex, int count);

        /// <summary>
        ///     <see cref="AdjustColumnLength"/>,
        ///     <see cref="AdjustColumnLengthIfShort"/>,
        ///     <see cref="AdjustColumnLengthIfLong"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustColumnLength" path="param|exception"/>
        public void ValidateAdjustColumnLength(int length);

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
        ///     <see cref="Clear"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param|exception"/>
        public void ValidateClear();

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

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetRowInternal"/>
        public TFixedRowElement SetRowInternal(int rowIndex, TRowElementSettings settings);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetRowRangeInternal"/>
        public IEnumerable<TFixedRowElement> SetRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetColumnInternal"/>
        public IEnumerable<TEditableListElement> SetColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetColumnRangeInternal"/>
        public IEnumerable<IEnumerable<TEditableListElement>> SetColumnRangeInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.SetCellInternal"/>
        public TEditableListElement SetCellInternal(int rowIndex, int columnIndex, TListElementSettings settings);

        /// <summary>
        ///     <see cref="AddRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddRow" path="param|returns"/>
        public TFixedRowElement AddRowInternal(TRowElementSettings settings);

        /// <summary>
        ///     <see cref="AddRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddRowRange" path="param|returns"/>
        public IEnumerable<TFixedRowElement> AddRowRangeInternal(IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     <see cref="InsertRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertRow" path="param|returns"/>
        public TFixedRowElement InsertRowInternal(int rowIndex, TRowElementSettings settings);

        /// <summary>
        ///     <see cref="InsertRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertRowRange" path="param|returns"/>
        public IEnumerable<TFixedRowElement> InsertRowRangeInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     <see cref="OverwriteRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="OverwriteRow" path="param|returns"/>
        public IEnumerable<TFixedRowElement> OverwriteRowInternal(
            int rowIndex,
            IEnumerable<TRowElementSettings> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveRowInternal"/>
        public void MoveRowInternal(int oldRowIndex, int newRowIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveRowRangeInternal"/>
        public void MoveRowRangeInternal(int oldRowIndex, int newRowIndex, int count);

        /// <summary>
        ///     <see cref="RemoveRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveRow" path="param|returns"/>
        public TFixedRowElement RemoveRowInternal(int rowIndex);

        /// <summary>
        ///     <see cref="RemoveRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveRowRange" path="param|returns"/>
        public IEnumerable<TFixedRowElement> RemoveRowRangeInternal(int rowIndex, int count);

        /// <summary>
        ///     <see cref="AdjustRowLength"/>,
        ///     <see cref="AdjustRowLengthIfShort"/>,
        ///     <see cref="AdjustRowLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustRowLength" path="param|returns"/>
        public IEnumerable<TFixedRowElement> AdjustRowLengthInternal(int length);

        /// <summary>
        ///     <see cref="AddColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddColumn" path="param|returns"/>
        public IEnumerable<TEditableListElement> AddColumnInternal(IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     <see cref="AddColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddColumnRange" path="param|returns"/>
        public IEnumerable<IEnumerable<TEditableListElement>> AddColumnRangeInternal(
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     <see cref="InsertColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertColumn" path="param|returns"/>
        public IEnumerable<TEditableListElement> InsertColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        );

        /// <summary>
        ///     <see cref="InsertColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertColumnRange" path="param|returns"/>
        public IEnumerable<IEnumerable<TEditableListElement>> InsertColumnRangeInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     <see cref="OverwriteColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="OverwriteColumn" path="param|returns"/>
        public IEnumerable<IEnumerable<TEditableListElement>> OverwriteColumnInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveColumnInternal"/>
        public void MoveColumnInternal(int oldColumnIndex, int newColumnIndex);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.MoveColumnRangeInternal"/>
        public void MoveColumnRangeInternal(int oldColumnIndex, int newColumnIndex, int count);

        /// <summary>
        ///     <see cref="RemoveColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveColumn" path="param|returns"/>
        public IEnumerable<TEditableListElement> RemoveColumnInternal(int columnIndex);

        /// <summary>
        ///     <see cref="RemoveColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveColumnRange" path="param|returns"/>
        public IEnumerable<IEnumerable<TEditableListElement>> RemoveColumnRangeInternal(int columnIndex, int count);

        /// <summary>
        ///     <see cref="AdjustColumnLength"/>,
        ///     <see cref="AdjustColumnLengthIfShort"/>,
        ///     <see cref="AdjustColumnLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustColumnLength" path="param|returns"/>
        public IEnumerable<IEnumerable<TEditableListElement>> AdjustColumnLengthInternal(int length);

        /// <inheritdoc
        ///     cref="IFixedLength2DList{TFixedRowElement,TReadOnlyRowElement,TRowElementSettings,TEditableListElement,TReadOnlyListElement,TListElementSettings}.ResetInternal(IEnumerable{TRowElementSettings})"/>
        public IEnumerable<TFixedRowElement> ResetInternal(
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     <see cref="Clear"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param"/>
        public void ClearInternal();

        #endregion

        #endregion
    }
}
