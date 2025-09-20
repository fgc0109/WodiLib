// ========================================
// Project Name : WodiLib
// File Name    : SimpleListValueBuilder.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     <see cref="SimpleList{T}"/> 自身が要素インスタンスを作成するために使用するBuilder
    /// </summary>
    /// <typeparam name="T">リスト要素型</typeparam>
    internal class SimpleListValueBuilder<T>
    {
        private readonly Func<SimpleList<T>, int, T> builderImpl;

        /// <summary>
        ///     <see cref="SimpleList{T}"/> 自身を参照して要素を作成するためのBuilder
        /// </summary>
        /// <param name="buildFromSelfAndIndex">要素生成処理</param>
        public SimpleListValueBuilder(Func<SimpleList<T>, int, T> buildFromSelfAndIndex)
        {
            this.builderImpl = buildFromSelfAndIndex;
        }

        /// <summary>
        ///     インデックスのみ参照して要素を作成するためのBuilder
        /// </summary>
        /// <param name="buildFromIndex">要素生成処理</param>
        public SimpleListValueBuilder(Func<int, T> buildFromIndex)
        {
            builderImpl = (list, index) => buildFromIndex(index);
        }

        /// <summary>
        ///     インスタンスを生成する。
        /// </summary>
        /// <returns>インスタンス</returns>
        public T Build(SimpleList<T> list, int index) => builderImpl.Invoke(list, index);
    }
}
