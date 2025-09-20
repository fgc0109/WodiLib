// ========================================
// Project Name : WodiLib
// File Name    : DuplicateItemReferenceException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <summary>
    ///     Listに登録しようとした要素と同じ参照の要素がすでに登録されている場合のエラー。
    /// </summary>
    internal class DuplicateItemReferenceException : Exception
    {
        public DuplicateItemReferenceException() : base(
            "一つのListに同一参照の要素を複数登録することは出来ません。（Hint：ディープクローンの登録は可能。）"
        )
        {
        }
    }
}
