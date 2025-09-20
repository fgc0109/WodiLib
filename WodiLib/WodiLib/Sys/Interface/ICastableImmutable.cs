// ========================================
// Project Name : WodiLib
// File Name    : ICastableImmutable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     自身を読取専用の型にキャスト可能であることを示すインタフェース。
    /// </summary>
    /// <typeparam name="T">読取専用の型</typeparam>
    public interface ICastableImmutable<out T>
    {
        /// <summary>
        ///     読取専用の型にキャストする。
        /// </summary>
        /// <returns>キャストしたインスタンス</returns>
        public T AsImmutable();
    }
}
