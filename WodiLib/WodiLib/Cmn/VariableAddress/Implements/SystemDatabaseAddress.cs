// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1300000000, 1399999999)]
    ///     [SafetyRange(1300000000, 1399999920)]
    ///     システムDB変アドレス値
    /// </summary>
    [VariableAddress(
        MinValue = 1300000000,
        MaxValue = 1399999999,
        SafetyMinValue = 1300000000,
        SafetyMaxValue = 1399999920
    )]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(SystemDatabaseAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record SystemDatabaseAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SystemDatabaseAddress() : this(MinValue)
        {
        }
    }
}
