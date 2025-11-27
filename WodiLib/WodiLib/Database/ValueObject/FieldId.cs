// ========================================
// Project Name : WodiLib
// File Name    : FieldId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [Range(0, 99)] DB項目ID
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 99)]
    [IntValueObjectJsonConvert]
    public partial class FieldId
    {
        /// <summary>デフォルト値</summary>
        public static readonly FieldId Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public FieldId() : this(0)
        {
        }
    }
}
