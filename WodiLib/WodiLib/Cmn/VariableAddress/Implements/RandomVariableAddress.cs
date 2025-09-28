// ========================================
// Project Name : WodiLib
// File Name    : RandomVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(8000000, 8999999)] ランダム変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 8000000, MaxValue = 8999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(RandomVariableAddress), typeof(VariableAddress) }
    )]
    public partial record RandomVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>ランダム量</summary>
        public RandomVariableValue RandomValue => RawValue.SubInt(0, 6);

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public RandomVariableAddress() : this(MinValue)
        {
        }
    }
}
