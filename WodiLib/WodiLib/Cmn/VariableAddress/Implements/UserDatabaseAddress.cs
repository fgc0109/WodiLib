// ========================================
// Project Name : WodiLib
// File Name    : UserDatabaseAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1000000000, 1099999999)] ユーザDBアドレス値
    /// </summary>
    [VariableAddress(MinValue = 1000000000, MaxValue = 1099999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(UserDatabaseAddress), typeof(VariableAddress) }
    )]
    public partial record UserDatabaseAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public UserDatabaseAddress() : this(MinValue)
        {
        }
    }
}
