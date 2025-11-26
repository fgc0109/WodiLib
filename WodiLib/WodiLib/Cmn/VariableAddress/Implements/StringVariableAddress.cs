// ========================================
// Project Name : WodiLib
// File Name    : StringVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(3000000, 3999999)] 文字列変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 3000000, MaxValue = 3999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(StringVariableAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record StringVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.String;

        /// <summary>変数インデックス</summary>
        public StringVariableIndex VariableIndex => RawValue.SubInt(0, 6);

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public StringVariableAddress() : this(MinValue)
        {
        }
    }
}
