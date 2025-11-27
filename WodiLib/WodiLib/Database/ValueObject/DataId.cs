// ========================================
// Project Name : WodiLib
// File Name    : DataId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [Range(0, 9999)] DBデータID
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 9999)]
    [IntValueObjectJsonConvert]
    public partial class DataId
    {
        /// <summary>デフォルト値</summary>
        public static readonly DataId Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DataId() : this(0)
        {
        }
    }
}
