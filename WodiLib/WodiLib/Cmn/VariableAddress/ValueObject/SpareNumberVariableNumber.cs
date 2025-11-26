// ========================================
// Project Name : WodiLib
// File Name    : SpareNumberVariableNumber.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(0, 9)] 予備変数番号
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 9)]
    [IntValueObjectJsonConvert]
    public partial record SpareNumberVariableNumber
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SpareNumberVariableNumber() : this(MinValue)
        {
        }
    }
}
