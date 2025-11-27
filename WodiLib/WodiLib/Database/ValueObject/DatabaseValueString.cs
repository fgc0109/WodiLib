// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueString.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DB設定文字列
    /// </summary>
    [CommonAnyStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class DatabaseValueString
    {
        /// <summary>デフォルト値</summary>
        public static readonly DatabaseValueString Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DatabaseValueString() : this("")
        {
        }
    }
}
