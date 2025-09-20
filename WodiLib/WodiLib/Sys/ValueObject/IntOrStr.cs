// ========================================
// Project Name : WodiLib
// File Name    : IntOrStr.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Sys
{
    /// <summary>
    ///     int、stringを持つ値オブジェクト
    /// </summary>
    public class IntOrStr :
        IEquatable<IntOrStr>,
        IDeepCloneable<IntOrStr>
    {
        #region Computed

        #region public

        /// <summary>数値保有フラグ</summary>
        public bool HasInt =>
            InstanceIntOrStrType == IntOrStrType.Int || InstanceIntOrStrType == IntOrStrType.IntAndStr;

        /// <summary>文字列保有フラグ</summary>
        public bool HasStr =>
            InstanceIntOrStrType == IntOrStrType.Str || InstanceIntOrStrType == IntOrStrType.IntAndStr;

        /// <summary>数値/文字列いずれかのみ保有フラグ</summary>
        public bool IsOneSideValue => HasInt != HasStr;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <summary>
        ///     保有する値の種類
        /// </summary>
        public IntOrStrType InstanceIntOrStrType { get; }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly int numValue;
        private readonly string? strValue;

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region required

        private IntOrStr(int numValue, string? strValue, IntOrStrType instanceIntOrStrType)
        {
            if (instanceIntOrStrType == IntOrStrType.IntAndStr || instanceIntOrStrType == IntOrStrType.Str)
            {
                ThrowHelper.ValidateArgumentNotNull(strValue is null, nameof(strValue));
            }

            this.numValue = numValue;
            this.strValue = strValue;
            InstanceIntOrStrType = instanceIntOrStrType;
        }

        #endregion

        #region convenience

        #region public

        /// <summary>
        ///     <see langword="int"/> 値を持つインスタンスを生成するコンストラクタ
        /// </summary>
        /// <param name="intValue">設定値</param>
        public IntOrStr(int intValue) : this(intValue, null, IntOrStrType.Int)
        {
        }

        /// <summary>
        ///     <see langword="string"/> 値を持つインスタンスを生成するコンストラクタ
        /// </summary>
        /// <param name="strValue">設定値</param>
        public IntOrStr(string strValue) : this(0, strValue, IntOrStrType.Str)
        {
        }

        /// <summary>
        ///     <see langword="int"/>, <see langword="string"/> どちらの値も持つインスタンスを生成するコンストラクタ
        /// </summary>
        /// <param name="numValue">int設定値</param>
        /// <param name="strValue">string設定値</param>
        public IntOrStr(int numValue, string strValue) : this(numValue, strValue, IntOrStrType.IntAndStr)
        {
        }

        /// <summary>
        ///     <see langword="int"/>, <see langword="string"/> どちらも持たないインスタンスを生成するコンストラクタ
        /// </summary>
        public IntOrStr() : this(0, null, IntOrStrType.None)
        {
        }

        #endregion

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = numValue;
                hashCode = (hashCode * 397)
                           ^ (strValue != null
                               ? strValue.GetHashCode()
                               : 0);
                hashCode = (hashCode * 397) ^ InstanceIntOrStrType.GetHashCode();
                return hashCode;
            }
        }

        /// <inheritdoc/>
        public bool Equals(IntOrStr? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return numValue == other.numValue
                   && strValue == other.strValue
                   && InstanceIntOrStrType.Equals(other.InstanceIntOrStrType);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => Equals(obj as IntOrStr);

        /// <summary>
        ///     <see langword="int"/> に変換する。
        /// </summary>
        /// <returns>保有する数値</returns>
        /// <exception cref="InvalidCastException">保有する値がintではない場合</exception>
        public int ToInt()
        {
            if (!HasInt) throw new InvalidCastException();
            return numValue;
        }

        /// <summary>
        ///     <see langword="string"/> に変換する。
        /// </summary>
        /// <returns>保有する文字列</returns>
        /// <exception cref="InvalidCastException">保有する値がstringではない場合</exception>
        public string ToStr()
        {
            if (!HasStr) throw new InvalidCastException();
            return strValue!;
        }

        /// <summary>
        ///     内容を文字列化する。
        /// </summary>
        /// <returns>文字列化した内容</returns>
        public string ToValueString()
        {
            if (InstanceIntOrStrType == IntOrStrType.IntAndStr) return $"({ToInt()}, {ToStr()})";
            if (InstanceIntOrStrType == IntOrStrType.Int) return $"{ToInt()}";
            if (InstanceIntOrStrType == IntOrStrType.Str) return ToStr();
            return "";
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Type: {InstanceIntOrStrType}, Value: \"{ToValueString()}\"";
        }

        /// <summary>
        ///     自分自身が保有する値とマージした結果インスタンスを返す。
        /// </summary>
        /// <remarks>
        ///     <pre>すでに文字列を保有している場合、両方所有状態にする</pre>
        ///     <pre>数値を保有している場合はその値を上書きする。</pre>
        /// </remarks>
        /// <param name="value">設定値</param>
        public IntOrStr Merged(int value)
        {
            if (HasStr)
            {
                return new IntOrStr(value, ToStr());
            }

            return new IntOrStr(value);
        }

        /// <summary>
        ///     自分自身が保有する値とマージした結果インスタンスを返す。
        /// </summary>
        /// <remarks>
        ///     <pre>すでに数値を保有している場合、両方所有状態にする</pre>
        ///     <pre>文字列を保有している場合はその値を上書きする。</pre>
        /// </remarks>
        /// <param name="value">設定値</param>
        public IntOrStr Merged(string value)
        {
            if (HasInt)
            {
                return new IntOrStr(ToInt(), value);
            }

            return new IntOrStr(value);
        }

        /// <summary>
        ///     数値、文字列を引数で与えられたインスタンスの内容で上書きする。
        /// </summary>
        /// <param name="value">設定値</param>
        public IntOrStr Merged(IntOrStr value)
        {
            if (
                value.InstanceIntOrStrType == IntOrStrType.IntAndStr
                || InstanceIntOrStrType == IntOrStrType.None
            )
            {
                return value.DeepClone();
            }

            if (value.InstanceIntOrStrType == IntOrStrType.Int)
            {
                return InstanceIntOrStrType == IntOrStrType.Int
                    ? value.DeepClone()
                    : new IntOrStr(value.ToInt(), ToStr());
            }

            if (value.InstanceIntOrStrType == IntOrStrType.Str)
            {
                return InstanceIntOrStrType == IntOrStrType.Str
                    ? value.DeepClone()
                    : new IntOrStr(ToInt(), value.ToStr());
            }

            return DeepClone();
        }

        /// <inheritdoc/>
        public IntOrStr DeepClone()
        {
            if (InstanceIntOrStrType == IntOrStrType.Int) return new IntOrStr(numValue);
            if (InstanceIntOrStrType == IntOrStrType.Str) return new IntOrStr(strValue!);
            if (InstanceIntOrStrType == IntOrStrType.None) return new IntOrStr();
            return new IntOrStr(numValue, strValue!);
        }

        #endregion

        #region Interface Implementations

        #region IDeepCloneable

        object IDeepCloneable.DeepClone() => DeepClone();

        #endregion

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Operators

        #region equality

        /// <summary>== 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>
        ///     <paramref name="left"/> と <paramref name="right"/> が同一要素である場合 <see langword="true"/>
        /// </returns>
        public static bool operator ==(IntOrStr? left, IntOrStr? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>!= 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>
        ///     <paramref name="left"/> と <paramref name="right"/> が同一要素ではない場合 <see langword="true"/>
        /// </returns>
        public static bool operator !=(IntOrStr? left, IntOrStr? right) => !(left == right);

        #endregion

        #region from

        #region implicit

        /// <summary>
        ///     <see langword="int"/> -> <see cref="IntOrStr"/> 暗黙型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        public static implicit operator IntOrStr(int src)
        {
            return new IntOrStr(src);
        }

        /// <summary>
        ///     <see langword="int"/>? -> <see cref="IntOrStr"/> 暗黙型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        [return: NotNullIfNotNull("src")]
        public static implicit operator IntOrStr?(int? src)
        {
            if (src is null) return null;
            return new IntOrStr(src.Value);
        }

        /// <summary>
        ///     <see langword="string"/> -> <see cref="IntOrStr"/> 暗黙型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換した値</returns>
        [return: NotNullIfNotNull("src")]
        public static implicit operator IntOrStr?(string? src)
        {
            if (src is null) return null;
            return new IntOrStr(src);
        }

        /// <summary>
        ///     Tuple&lt;<see langword="int"/>, <see langword="string"/>> -> <see cref="IntOrStr"/> 暗黙型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        [return: NotNullIfNotNull("tuple")]
        public static implicit operator IntOrStr?(Tuple<int, string>? tuple)
        {
            if (tuple is null) return null;
            return new IntOrStr(tuple.Item1, tuple.Item2);
        }

        /// <summary>
        ///     (<see langword="int"/>, <see langword="string"/>) -> <see cref="IntOrStr"/> 暗黙型変換
        /// </summary>
        /// <param name="tuple">変換元</param>
        /// <returns>変換した値</returns>
        [return: NotNullIfNotNull("tuple")]
        public static implicit operator IntOrStr?(ValueTuple<int, string>? tuple)
        {
            if (tuple is null) return null;
            return new IntOrStr(tuple.Value.Item1, tuple.Value.Item2);
        }

        #endregion

        #endregion

        #endregion
    }
}
