// ========================================
// Project Name : WodiLib
// File Name    : DatabaseValueInt.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [Range(int.MinValue, int.MaxValue)]
    ///     [SafetyRange(-999999, 1400000000)]
    ///     DB設定数値
    /// </summary>
    [CommonIntValueObject(SafetyMinValue = -999999, SafetyMaxValue = 1400000000)]
    [IntValueObjectJsonConvert]
    public partial class DatabaseValueInt
    {
        /// <summary>デフォルト値</summary>
        public static readonly DatabaseValueInt Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DatabaseValueInt() : this(0)
        {
        }
    }
}
