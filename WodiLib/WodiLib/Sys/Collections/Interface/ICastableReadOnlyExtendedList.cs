// ========================================
// Project Name : WodiLib
// File Name    : ICastableReadOnlyExtendedList.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     読取専用リストにキャスト可能であることを表すインタフェース。
    /// </summary>
    /// <typeparam name="T">キャストする読取専用リスト型</typeparam>
    /// <typeparam name="TItem">キャストした読取専用リストの要素型</typeparam>
    public interface ICastableReadOnlyExtendedList<out T, TItem>
        where T : IReadOnlyExtendedList<TItem>
    {
        /// <summary>
        ///     読取専用リストにキャストする。
        /// </summary>
        /// <returns>自分自身と状態を同期する <typeparamref name="T"/> のインスタンス</returns>
        public T AsReadOnlyList();
    }
}
