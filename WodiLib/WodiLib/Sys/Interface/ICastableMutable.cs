// ========================================
// Project Name : WodiLib
// File Name    : ICastableMutable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Sys
{
    /// <summary>
    ///     自身を編集用の型にキャスト可能であることを示すインタフェース。
    /// </summary>
    /// <typeparam name="T">編集用の型</typeparam>
    public interface ICastableMutable<out T>
    {
        /// <summary>
        ///     編集用の型にキャストする。
        /// </summary>
        /// <returns>キャストしたインスタンス</returns>
        public T AsMutable();
    }
}
