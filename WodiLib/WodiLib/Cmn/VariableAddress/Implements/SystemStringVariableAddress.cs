// ========================================
// Project Name : WodiLib
// File Name    : SystemStringVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(9900000, 9999999)] システム文字列変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 9900000, MaxValue = 9999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(SystemStringVariableAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record SystemStringVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.String;

        /// <summary>変数インデックス</summary>
        public SystemStringVariableIndex VariableIndex => RawValue.SubInt(0, 5);

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SystemStringVariableAddress() : this(MinValue)
        {
        }
    }
}
