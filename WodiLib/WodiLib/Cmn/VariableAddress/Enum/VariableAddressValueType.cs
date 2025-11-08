// ========================================
// Project Name : WodiLib
// File Name    : VariableAddressValueType.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     各情報アドレス情報種別
    /// </summary>
    public class VariableAddressValueType : TypeSafeEnum<VariableAddressValueType>
    {
        /// <summary>数値</summary>
        public static readonly VariableAddressValueType Numeric;

        /// <summary>文字列</summary>
        public static readonly VariableAddressValueType String;

        /// <summary>両方</summary>
        public static readonly VariableAddressValueType Both;

        /// <summary>全ての要素</summary>
        public static IEnumerable<VariableAddressValueType> AllItems => EnumItems.AllEnums;

        static VariableAddressValueType()
        {
            Numeric = new VariableAddressValueType(
                nameof(Numeric),
                0x01
            );
            String = new VariableAddressValueType(
                nameof(String),
                0x02
            );
            Both = new VariableAddressValueType(
                nameof(Both),
                0x03
            );
        }

        private VariableAddressValueType(string id, byte typeFlag) : base(id)
        {
            TypeFlag = typeFlag;
        }

        private byte TypeFlag { get; }

        /// <summary>
        ///     自身のタイプ種別に指定したタイプ種別が適合するか判定する。
        /// </summary>
        /// <param name="target">判定対象</param>
        /// <returns><paramref name="target"/> が <see langword="null"/> の場合 <see langword="false"/>、適合する場合 <see langword="true"/>。</returns>
        /// <remarks>
        ///     自身が <see cref="Both"/> の場合、<paramref name="target"/> が <see cref="Both"/>, <see cref="Numeric"/>,
        ///     <see cref="String"/> いずれの場合も <see langword="true"/>。
        ///     自身が <see cref="Numeric"/> の場合、targetが <see cref="Numeric"/> の場合のみ <see langword="true"/>。
        ///     自身が <see cref="String"/> の場合、targetが <see cref="String"/> の場合のみ <see langword="true"/>。
        /// </remarks>
        public bool CheckTypeInclude(VariableAddressValueType? target)
        {
            if (target is null) return false;

            if (this == Both) return true;
            return TypeFlag == target.TypeFlag;
        }
    }
}
