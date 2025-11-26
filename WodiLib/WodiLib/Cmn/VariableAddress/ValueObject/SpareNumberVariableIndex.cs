// ========================================
// Project Name : WodiLib
// File Name    : SpareNumberVariableIndex.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(0, 99999)] 予備変数インデックス
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 99999)]
    [IntValueObjectJsonConvert]
    public partial record SpareNumberVariableIndex
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SpareNumberVariableIndex() : this(MinValue)
        {
        }
    }
}
