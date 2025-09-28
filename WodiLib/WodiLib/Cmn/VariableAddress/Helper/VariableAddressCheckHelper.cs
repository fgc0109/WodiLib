// ========================================
// Project Name : WodiLib
// File Name    : IntExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     アドレス変数チェック用Helper
    /// </summary>
    public static class VariableAddressCheckHelper
    {
        /// <summary>
        ///     この数値が変数アドレス値として適切か厳密に判定する。
        /// </summary>
        /// <remarks>
        ///     簡易チェックを行いたい場合は、<see cref="IsVariableAddressSimpleCheck"/> を使用する。
        ///     例えば、 9180048 は <see cref="IsVariableAddress"/> では <see langword="false"/> となるが、
        ///     <see cref="IsVariableAddressSimpleCheck"/> では <see langword="true"/> となる。
        /// </remarks>
        /// <param name="value">対象</param>
        /// <returns>変数アドレス値として適切な場合 <see langword="true"/></returns>
        public static bool IsVariableAddress(int value)
        {
            return VariableAddressFactory.TryCreate(value, out _);
        }

        /// <summary>
        ///     この数値が変数アドレス値として適切か簡易判定する。
        /// </summary>
        /// <remarks>
        ///     厳密なチェックを行いたい場合は、<see cref="IsVariableAddress"/> を使用する。
        ///     例えば、 9180048 は <see cref="IsVariableAddress"/> では <see langword="false"/> となるが、
        ///     <see cref="IsVariableAddressSimpleCheck"/> では <see langword="true"/> となる。
        /// </remarks>
        /// <param name="value">対象</param>
        /// <returns>変数アドレス値として適切な場合 <see langword="true"/></returns>
        public static bool IsVariableAddressSimpleCheck(int value)
        {
            return value.IsBetween(VariableAddress.MinValue, VariableAddress.MaxValue);
        }

        /// <summary>
        ///     この数値が数値変数アドレス値として適切か判定する。
        /// </summary>
        /// <param name="value">対象</param>
        /// <returns>数値変数アドレス値として適切な場合 <see langword="false"/></returns>
        public static bool IsNumericVariableAddress(int value)
        {
            if (!VariableAddressFactory.TryCreate(value, out var variableAddress)) return false;
            return variableAddress.ValueType.CheckTypeInclude(VariableAddressValueType.Numeric);
        }
    }
}
