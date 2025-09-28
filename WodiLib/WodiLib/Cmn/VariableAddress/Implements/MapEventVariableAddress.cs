// ========================================
// Project Name : WodiLib
// File Name    : MapEventVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1000000, 1099999)] マップイベントセルフ変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 1000000, MaxValue = 1099999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(MapEventVariableAddress), typeof(VariableAddress) }
    )]
    public partial record MapEventVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public MapEventVariableAddress() : this(MinValue)
        {
        }
    }
}
