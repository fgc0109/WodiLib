// ========================================
// Project Name : WodiLib
// File Name    : DataName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [NotNewLine] データ名
    /// </summary>
    [CommonOneLineStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class DataName
    {
        /// <summary>デフォルト値</summary>
        public static readonly DataName Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DataName() : this("")
        {
        }
    }
}
