// ========================================
// Project Name : WodiLib
// File Name    : IReadOnlyExtendedList.cs
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
    ///     WodiLib 独自読み取り専用リストインタフェース
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="ObservableCollection{T}"/> をベースに、読取専用の制限を設けた機能。
    ///     </para>
    /// </remarks>
    /// <typeparam name="TReadOnlyElement">リスト要素型</typeparam>
    internal interface IReadOnlyExtendedList<out TReadOnlyElement>
    {
        #region Properties

        /// <summary>
        ///     インデクサによるアクセス
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public TReadOnlyElement this[int index] { get; }

        /// <summary>要素数</summary>
        public int Count { get; }

        #endregion

        #region Methods

        #region CRUD

        /// <summary>
        ///     指定インデックスの要素を取得する。
        /// </summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> が指定範囲外の場合。
        /// </exception>
        public TReadOnlyElement Get(int index);

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
        public IEnumerable<TReadOnlyElement> GetRange(int index, int count);

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

        #endregion

        #region CRUD core

        /// <summary>
        ///     <see cref="Get"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="Get" path="param"/>
        public TReadOnlyElement GetInternal(int index);

        /// <summary>
        ///     <see cref="GetRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetRange" path="param"/>
        public IEnumerable<TReadOnlyElement> GetRangeInternal(int index, int count);

        #endregion

        #endregion
    }
}
