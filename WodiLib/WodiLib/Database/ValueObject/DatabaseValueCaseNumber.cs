// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueCaseNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [Range(-9999999, 1400000000)] DB項目特殊指定選択肢番号
    /// </summary>
    [CommonIntValueObject(MinValue = -9999999, MaxValue = 1400000000)]
    [IntValueObjectJsonConvert]
    public partial class DatabaseValueCaseNumber
    {
        /// <summary>デフォルト値</summary>
        public static readonly DatabaseValueCaseNumber Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DatabaseValueCaseNumber() : this(0)
        {
        }
    }
}
