// ========================================
// Project Name : WodiLib
// File Name    : Coordinate.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     座標
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = int.MaxValue)]
    [IntValueObjectJsonConvert]
    public partial record Coordinate
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public Coordinate() : this(0)
        {
        }
    }
}
