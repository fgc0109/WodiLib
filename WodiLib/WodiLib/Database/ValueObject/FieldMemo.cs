// ========================================
// Project Name : WodiLib
// File Name    : FieldMemo.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     項目メモ
    /// </summary>
    [CommonAnyStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class FieldMemo
    {
        /// <summary>デフォルト値</summary>
        public static readonly FieldMemo Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public FieldMemo() : this("")
        {
        }
    }
}
