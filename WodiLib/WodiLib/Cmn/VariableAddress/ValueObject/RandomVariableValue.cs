// ========================================
// Project Name : WodiLib
// File Name    : RandomVariableValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(0, 999999)] ランダム変数ランダム量
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 999999)]
    [IntValueObjectJsonConvert]
    public partial record RandomVariableValue
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public RandomVariableValue() : this(MinValue)
        {
        }
    }
}
