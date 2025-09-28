// ========================================
// Project Name : WodiLib
// File Name    : ThisCommonEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1600000, 1600099)] このコモンイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 1600000, MaxValue = 1600099)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(ThisCommonEventVariableAddress), typeof(VariableAddress) }
    )]
    public partial record ThisCommonEventVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ThisCommonEventVariableAddress() : this(MinValue)
        {
        }
    }
}
