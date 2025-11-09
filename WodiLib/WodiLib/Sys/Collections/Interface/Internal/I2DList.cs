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
    ///     インタフェースの明確化・ドキュメントコメント切り出しのために定義する。
    /// </remarks>
    /// <typeparam name="TFixedRowElement">行要素長さ固定型</typeparam>
    /// <typeparam name="TRowElementSettings">行要素設定DTO</typeparam>
    /// <typeparam name="TListElementImpl">リスト要素型</typeparam>
    /// <typeparam name="TListElementSettings">リスト要素設定DTO</typeparam>
    internal interface I2DList<
        TFixedRowElement,
        in TRowElementSettings,
        TListElementImpl,
        in TListElementSettings>
        where TFixedRowElement : TRowElementSettings
        where TListElementImpl : TListElementSettings
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
        public TListElementImpl this[int rowIndex, int columnIndex] { get; set; }

        /// <summary>行数</summary>
        public int RowCount { get; }

        /// <summary>列数</summary>
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

        /// <summary>
        ///     指定行インデックスの行要素を取得する。
        /// </summary>
        /// <param name="rowIndex">[Range(0, <see cref="RowCount"/> - 1)] 行インデックス</param>
        /// <returns>指定行の行要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="rowIndex"/> が指定範囲外の場合。
        /// </exception>
        public TFixedRowElement GetRow(int rowIndex);

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

        /// <summary>
        ///     指定列インデックスの列要素を取得する。
        /// </summary>
        /// <param name="columnIndex">[Range(0, <see cref="ColumnCount"/> - 1)] 列インデックス</param>
        /// <returns>指定列の要素リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="columnIndex"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<TListElementImpl> GetColumn(int columnIndex);

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
        public IEnumerable<IEnumerable<TListElementImpl>> GetColumnRange(int columnIndex, int count);

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
        public IEnumerable<TListElementImpl> SetColumn(int columnIndex, IEnumerable<TListElementSettings> settings);

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
        public IEnumerable<IEnumerable<TListElementImpl>> SetColumnRange(
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
        public IEnumerable<TListElementImpl> AddColumn(IEnumerable<TListElementSettings> settings);

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
        public IEnumerable<IEnumerable<TListElementImpl>> AddColumnRange(
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
        public IEnumerable<TListElementImpl> InsertColumn(
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
        public IEnumerable<IEnumerable<TListElementImpl>> InsertColumnRange(
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
        public IEnumerable<IEnumerable<TListElementImpl>> OverwriteColumn(
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
        public IEnumerable<TListElementImpl> RemoveColumn(int columnIndex);

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
        public IEnumerable<IEnumerable<TListElementImpl>> RemoveColumnRange(int columnIndex, int count);

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
        public IEnumerable<IEnumerable<TListElementImpl>> AdjustColumnLength(int length);

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
        public IEnumerable<IEnumerable<TListElementImpl>> AdjustColumnLengthIfShort(int length);

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
        public IEnumerable<IEnumerable<TListElementImpl>> AdjustColumnLengthIfLong(int length);

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
        public TListElementImpl GetCell(int rowIndex, int columnIndex);

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
        public TListElementImpl SetCell(int rowIndex, int columnIndex, TListElementSettings settings);

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
        ///     <see cref="ResetStrict"/> を利用する。
        /// </remarks>
        public IEnumerable<TFixedRowElement> Reset(
            IEnumerable<TRowElementSettings> settings
        );

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
        public IEnumerable<TFixedRowElement> ResetStrict(
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     要素をデフォルト値で一新する。
        /// </summary>
        public IEnumerable<TFixedRowElement> Reset();

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public void Clear();

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
        ///     <see cref="Reset(IEnumerable{TRowElementSettings})"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset(IEnumerable{TRowElementSettings})" path="param|exception"/>
        public void ValidateReset(IEnumerable<TRowElementSettings> settings);

        /// <summary>
        ///     <see cref="ResetStrict"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="ResetStrict" path="param|exception"/>
        public void ValidateResetStrict(IEnumerable<TRowElementSettings> settings);

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
        ///     <see cref="GetRow"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRow" path="param"/>
        public TFixedRowElement GetRowInternal(int rowIndex);

        /// <summary>
        ///     <see cref="GetRowRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRowRange" path="param"/>
        public IEnumerable<TFixedRowElement> GetRowRangeInternal(int rowIndex, int count);

        /// <summary>
        ///     <see cref="GetColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetColumn" path="param"/>
        public IEnumerable<TListElementImpl> GetColumnInternal(int columnIndex);

        /// <summary>
        ///     <see cref="GetColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetColumnRange" path="param"/>
        public IEnumerable<IEnumerable<TListElementImpl>> GetColumnRangeInternal(int columnIndex, int count);

        /// <summary>
        ///     <see cref="GetCell"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetCell" path="param"/>
        public TListElementImpl GetCellInternal(int rowIndex, int columnIndex);

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
        public IEnumerable<TListElementImpl> SetColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        );

        /// <summary>
        ///     <see cref="SetColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetColumnRange" path="param"/>
        public IEnumerable<IEnumerable<TListElementImpl>> SetColumnRangeInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     <see cref="SetCell"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetCell" path="param"/>
        public TListElementImpl SetCellInternal(int rowIndex, int columnIndex, TListElementSettings settings);

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
        public IEnumerable<TListElementImpl> AddColumnInternal(IEnumerable<TListElementSettings> settings);

        /// <summary>
        ///     <see cref="AddColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddColumnRange" path="param|returns"/>
        public IEnumerable<IEnumerable<TListElementImpl>> AddColumnRangeInternal(
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     <see cref="InsertColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertColumn" path="param|returns"/>
        public IEnumerable<TListElementImpl> InsertColumnInternal(
            int columnIndex,
            IEnumerable<TListElementSettings> settings
        );

        /// <summary>
        ///     <see cref="InsertColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertColumnRange" path="param|returns"/>
        public IEnumerable<IEnumerable<TListElementImpl>> InsertColumnRangeInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

        /// <summary>
        ///     <see cref="OverwriteColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="OverwriteColumn" path="param|returns"/>
        public IEnumerable<IEnumerable<TListElementImpl>> OverwriteColumnInternal(
            int columnIndex,
            IEnumerable<IEnumerable<TListElementSettings>> settings
        );

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
        ///     <see cref="RemoveColumn"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveColumn" path="param|returns"/>
        public IEnumerable<TListElementImpl> RemoveColumnInternal(int columnIndex);

        /// <summary>
        ///     <see cref="RemoveColumnRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveColumnRange" path="param|returns"/>
        public IEnumerable<IEnumerable<TListElementImpl>> RemoveColumnRangeInternal(int columnIndex, int count);

        /// <summary>
        ///     <see cref="AdjustColumnLength"/>,
        ///     <see cref="AdjustColumnLengthIfShort"/>,
        ///     <see cref="AdjustColumnLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustColumnLength" path="param|returns"/>
        public IEnumerable<IEnumerable<TListElementImpl>> AdjustColumnLengthInternal(int length);

        /// <summary>
        ///     <see cref="Reset(IEnumerable{TRowElementSettings})"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Reset(IEnumerable{TRowElementSettings})" path="param"/>
        public IEnumerable<TFixedRowElement> ResetInternal(
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     <see cref="ResetStrict"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="ResetStrict" path="param|returns"/>
        public IEnumerable<TFixedRowElement> ResetStrictInternal(
            IEnumerable<TRowElementSettings> settings
        );

        /// <summary>
        ///     <see cref="Reset()"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset()" path="param|exception"/>
        public IEnumerable<TFixedRowElement> ResetInternal();

        /// <summary>
        ///     <see cref="Clear"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param"/>
        public void ClearInternal();

        #endregion

        #endregion
    }
}
