// ========================================
// Project Name : WodiLib
// File Name    : TypeName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [NotNewLine] DBタイプ名
    /// </summary>
    [CommonOneLineStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class TypeName
    {
        /// <summary>デフォルト値</summary>
        public static readonly TypeName Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public TypeName() : this("")
        {
        }
    }
}
