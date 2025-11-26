// ========================================
// Project Name : WodiLib
// File Name    : NormalNumberVariableIndex.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(0, 99999)] 通常変数インデックス
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 99999)]
    [IntValueObjectJsonConvert]
    public partial record NormalNumberVariableIndex
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public NormalNumberVariableIndex() : this(MinValue)
        {
        }
    }
}
