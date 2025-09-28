// ========================================
// Project Name : WodiLib
// File Name    : ThisMapEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1100000, 1100009)] このマップイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 1100000, MaxValue = 1100009)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(ThisMapEventVariableAddress), typeof(VariableAddress) }
    )]
    public partial record ThisMapEventVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ThisMapEventVariableAddress() : this(MinValue)
        {
        }
    }
}
