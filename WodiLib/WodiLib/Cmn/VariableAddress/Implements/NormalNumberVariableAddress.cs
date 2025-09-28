// ========================================
// Project Name : WodiLib
// File Name    : NormalNumberVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(2000000, 2099999)] 通常変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 2000000, MaxValue = 2099999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(NormalNumberVariableAddress), typeof(VariableAddress) }
    )]
    public partial record NormalNumberVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>アドレスインデックス</summary>
        public NormalNumberVariableIndex VariableIndex => RawValue.SubInt(0, 5);

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public NormalNumberVariableAddress() : this(MinValue)
        {
        }
    }
}
