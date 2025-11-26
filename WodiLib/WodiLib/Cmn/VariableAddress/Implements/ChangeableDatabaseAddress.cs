// ========================================
// Project Name : WodiLib
// File Name    : ChangeableDatabaseAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(1100000000, 1199999999)] 可変DBアドレス値
    /// </summary>
    [VariableAddress(MinValue = 1100000000, MaxValue = 1199999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(ChangeableDatabaseAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record ChangeableDatabaseAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ChangeableDatabaseAddress() : this(1100000000)
        {
        }
    }
}
