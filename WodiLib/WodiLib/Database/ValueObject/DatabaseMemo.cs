// ========================================
// Project Name : WodiLib
// File Name    : DatabaseMemo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     データベースメモ
    /// </summary>
    [CommonAnyStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class DatabaseMemo
    {
        /// <summary>デフォルト値</summary>
        public static readonly DatabaseMemo Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DatabaseMemo() : this("")
        {
        }
    }
}
