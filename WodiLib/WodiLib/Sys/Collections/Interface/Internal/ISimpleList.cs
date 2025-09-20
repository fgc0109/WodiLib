// ========================================
// Project Name : WodiLib
// File Name    : ISimpleList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WodiLib.Sys.Collections
{
    internal interface ISimpleList<T> :
        IEqualityComparable<ISimpleList<T>>,
        IDeepCloneable<ISimpleList<T>>,
        INotifyPropertyChanged,
        INotifyCollectionChanged,
        IList<T>
    {
        /// <summary>
        ///     GetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="count">要素数</param>
        /// <returns>指定した範囲の要素</returns>
        IEnumerable<T> Get(int index, int count);

        /// <summary>
        ///     SetRange メソッドの処理本体
        /// </summary>
        /// <param name="index">更新開始インデックス</param>
        /// <param name="items">更新要素</param>
        /// <returns>セットした要素</returns>
        IEnumerable<T> Set(int index, params T[] items);

        /// <summary>
        ///     AddRange メソッドの処理本体
        /// </summary>
        /// <param name="items">挿入要素</param>
        /// <returns>追加した要素</returns>
        IEnumerable<T> Add(params T[] items);

        /// <summary>
        ///     InsertRange メソッドの処理本体
        /// </summary>
        /// <param name="index">挿入先インデックス</param>
        /// <param name="items">挿入要素</param>
        /// <returns>追加した要素</returns>
        IEnumerable<T> Insert(int index, params T[] items);

        /// <summary>
        ///     Overwrite メソッドの処理本体
        /// </summary>
        /// <param name="index">上書き開始インデックス</param>
        /// <param name="items">上書き要素</param>
        /// <returns>上書きした要素</returns>
        IEnumerable<T> Overwrite(int index, params T[] items);

        /// <summary>
        ///     MoveRange メソッドの処理本体
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        void Move(int oldIndex, int newIndex);

        /// <summary>
        ///     MoveRange メソッドの処理本体
        /// </summary>
        /// <param name="oldIndex">移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">移動先のインデックス開始位置</param>
        /// <param name="count">移動させる要素数</param>
        void Move(int oldIndex, int newIndex, int count);

        /// <summary>
        ///     RemoveRange メソッドの処理本体
        /// </summary>
        /// <param name="index">除去開始インデックス</param>
        /// <param name="count">除去する要素数</param>
        /// <returns>削除した要素</returns>
        IEnumerable<T> Remove(int index, int count);

        /// <summary>
        ///     AdjustLength メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        /// <returns>追加または削除した要素</returns>
        IEnumerable<T> Adjust(int length);

        /// <summary>
        ///     AdjustLengthIfLong メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        /// <returns>削除した要素</returns>
        IEnumerable<T> AdjustIfLong(int length);

        /// <summary>
        ///     AdjustLengthIfShort メソッドの処理本体
        /// </summary>
        /// <param name="length">要素数</param>
        /// <returns>追加した要素</returns>
        IEnumerable<T> AdjustIfShort(int length);

        /// <summary>
        ///     Reset メソッドの処理本体
        /// </summary>
        /// <param name="items">初期化要素</param>
        /// <returns>初期化要素</returns>
        IEnumerable<T> Reset(params T[] items);

        /// <summary>
        ///     Reset メソッドの処理本体
        /// </summary>
        /// <remarks>
        ///     各要素の初期化子は
        ///     あらかじめ指定された初期化処理によって作成する。
        /// </remarks>
        /// <param name="length">初期化要素数</param>
        /// <returns>初期化要素</returns>
        IEnumerable<T> Reset(int length);
    }
}
