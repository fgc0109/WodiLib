// ========================================
// Project Name : WodiLib
// File Name    : MemberInfoAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(9180010, 9180059)] 仲間情報アドレス値
    /// </summary>
    [VariableAddress(MinValue = 9180010, MaxValue = 9180059)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(MemberInfoAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record MemberInfoAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>仲間ID</summary>
        public MemberId MemberId => RawValue.SubInt(1, 1);

        /// <summary>取得情報</summary>
        public InfoAddressInfoType InfoType => InfoAddressInfoType.FromCode(RawValue.SubInt(0, 1));

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MemberInfoAddress() : this(MinValue)
        {
        }
    }
}
