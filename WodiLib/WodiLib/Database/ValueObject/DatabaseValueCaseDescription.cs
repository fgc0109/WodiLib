// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueCaseDescription.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [NotNewLine] DB項目特殊指定選択肢文字列
    /// </summary>
    [CommonOneLineStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class DatabaseValueCaseDescription
    {
        /// <summary>デフォルト値</summary>
        public static readonly DatabaseValueCaseDescription Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DatabaseValueCaseDescription() : this("")
        {
        }
    }
}
