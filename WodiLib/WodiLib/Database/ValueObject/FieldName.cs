// ========================================
// Project Name : WodiLib
// File Name    : FieldName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [NotNewLine] DB項目名
    /// </summary>
    [CommonOneLineStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class FieldName
    {
        /// <summary>デフォルト値</summary>
        public static readonly FieldName Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public FieldName() : this("")
        {
        }
    }
}
