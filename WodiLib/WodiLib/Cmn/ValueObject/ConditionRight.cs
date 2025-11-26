// ========================================
// Project Name : WodiLib
// File Name    : ConditionRight.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(-999999, 999999)] 条件右辺
    /// </summary>
    [CommonIntValueObject(MinValue = -999999, MaxValue = 999999)]
    [IntValueObjectJsonConvert]
    public partial record ConditionRight
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ConditionRight() : this(0)
        {
        }
    }
}
