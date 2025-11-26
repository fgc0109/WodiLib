// ========================================
// Project Name : WodiLib
// File Name    : CommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(15000000, 15999999)] コモンイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 15000000, MaxValue = 15999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(CommonEventVariableAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record CommonEventVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public CommonEventVariableAddress() : this(MinValue)
        {
        }
    }
}
