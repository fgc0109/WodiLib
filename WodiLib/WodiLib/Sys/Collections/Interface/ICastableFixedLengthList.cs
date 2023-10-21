// ========================================
// Project Name : WodiLib
// File Name    : ICastableFixedLengthList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量固定リストにキャスト可能であることを表すインタフェース。
    /// </summary>
    /// <typeparam name="T">キャストする容量固定リスト型</typeparam>
    /// <typeparam name="TItem">キャストした容量固定リストの要素型</typeparam>
    public interface ICastableFixedLengthList<out T, TItem>
        where T : IFixedLengthList<TItem>
    {
        /// <summary>
        ///     容量固定リストにキャストする。
        /// </summary>
        /// <returns>自分自身と状態を同期する <typeparamref name="T"/> のインスタンス</returns>
        public T AsFixedLengthList();
    }
}
