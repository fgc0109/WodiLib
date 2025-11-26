// ========================================
// Project Name : WodiLib
// File Name    : SideLength.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     辺の長さ
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = int.MaxValue)]
    [IntValueObjectJsonConvert]
    public partial record SideLength
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SideLength() : this(0)
        {
        }
    }
}
