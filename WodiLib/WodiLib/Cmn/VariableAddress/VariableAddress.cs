// ========================================
// Project Name : WodiLib
// File Name    : VariableAddress.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Sys;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     変数呼び出し値基底クラス
    /// </summary>
    [VariableAddressBase(MinValue = 1000000, MaxValue = 2000000000)]
    [VariableAddressGapCalculatable]
    [VariableAddressAddAndSubtractableInt]
    public abstract partial record VariableAddress
    {
        #region Constants

        /// <summary>変数種別</summary>
        public abstract VariableAddressValueType ValueType { get; }

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Operators

        #region from

        #region implicit

        /// <summary>
        ///     int -> <see cref="VariableAddress"/> への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator VariableAddress(int src)
        {
            return VariableAddressFactory.Create(src);
        }

        #endregion

        #endregion

        #region to

        #region implicit

        /// <summary>
        ///     <see cref="VariableAddress"/> -> int への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        public static implicit operator int(VariableAddress src)
        {
            ThrowHelper.InvalidCastIf(
                src is null,
                () => ErrorMessage.InvalidCastFromNull(nameof(src), nameof(ChangeableDatabaseAddress))
            );

            return src.RawValue;
        }

        #endregion

        #endregion

        #region binary

        /// <summary>++ 演算子</summary>
        /// <param name="src"/>
        /// <returns>演算結果</returns>
        public static VariableAddress operator ++(VariableAddress src) => src.RawValue++;

        /// <summary>-- 演算子</summary>
        /// <param name="src"/>
        /// <returns>演算結果</returns>
        public static VariableAddress operator --(VariableAddress src) => src.RawValue--;

        #endregion

        #endregion
    }
}
