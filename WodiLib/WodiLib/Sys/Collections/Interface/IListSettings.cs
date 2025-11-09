// ========================================
// Project Name : WodiLib
// File Name    : IListSettings.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     リスト設定インタフェース
    /// </summary>
    /// <typeparam name="TListElementSettings"></typeparam>
    public interface IListSettings<TListElementSettings>
    {
        /// <summary>
        ///     要素の設定DTOインタフェース列挙
        /// </summary>
        public IList<TListElementSettings> Settings { get; }
    }
}
