// ========================================
// Project Name : WodiLib
// File Name    : SystemVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(9000000, 9099999)] システム変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 9000000, MaxValue = 9099999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(SystemVariableAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record SystemVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>変数インデックス</summary>
        public SystemVariableIndex VariableIndex => RawValue.SubInt(0, 5);

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SystemVariableAddress() : this(MinValue)
        {
        }
    }
}
