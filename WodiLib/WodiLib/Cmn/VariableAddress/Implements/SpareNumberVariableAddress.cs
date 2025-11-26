// ========================================
// Project Name : WodiLib
// File Name    : SpareNumberVariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [Range(2100000, 2999999)] 予備変数アドレス値
    /// </summary>
    [VariableAddress(MinValue = 2100000, MaxValue = 2999999)]
    [VariableAddressGapCalculatable(
        OtherTypes = new[] { typeof(SpareNumberVariableAddress), typeof(VariableAddress) }
    )]
    [IntValueObjectJsonConvert]
    public partial record SpareNumberVariableAddress : VariableAddress
    {
        /// <summary>変数種別</summary>
        public override VariableAddressValueType ValueType
            => VariableAddressValueType.Numeric;

        /// <summary>予備変数番号</summary>
        public SpareNumberVariableNumber VariableNumber => RawValue.SubInt(5, 1);

        /// <summary>変数インデックス</summary>
        public SpareNumberVariableIndex VariableIndex => RawValue.SubInt(0, 5);

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SpareNumberVariableAddress() : this(MinValue)
        {
        }
    }
}
