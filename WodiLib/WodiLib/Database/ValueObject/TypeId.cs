// ========================================
// Project Name : WodiLib
// File Name    : TypeId.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [Range(0, 99)] DBタイプID
    /// </summary>
    [CommonIntValueObject(MinValue = 0, MaxValue = 99)]
    [IntValueObjectJsonConvert]
    public partial class TypeId
    {
        /// <summary>デフォルト値</summary>
        public static readonly TypeId Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public TypeId() : this(0)
        {
        }
    }
}
