// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldValue.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Text;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DB項目値
    /// </summary>
    public partial class DatabaseFieldValue : IEquatable<DatabaseFieldValue>
    {
        #region Constants

        /// <summary>数値に変換できない理由</summary>
        private static readonly string NotCastIntReason = $"{nameof(Type)}が{nameof(DatabaseFieldType.Int)}ではないため";

        /// <summary>文字列に変換できない理由</summary>
        private static readonly string NotCastStringReason = $"{nameof(Type)}が{nameof(DatabaseFieldType.String)}ではないため";

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        #region public

        /// <summary>設定種別</summary>
        public DatabaseFieldType Type { get; }

        /// <summary>数値設定値</summary>
        /// <exception cref="PropertyAccessException">
        ///     設定種別が <see cref="DatabaseFieldType.Int"/> ではない場合。
        /// </exception>
        public DatabaseValueInt IntValue
        {
            get
            {
                ThrowHelper.ValidatePropertyAccess(
                    Type != DatabaseFieldType.Int,
                    NotCastIntReason
                );
                return intValue;
            }
        }

        private readonly DatabaseValueString stringValue = "";

        /// <summary>文字列設定値</summary>
        /// <exception cref="PropertyAccessException">
        ///     設定種別が <see cref="DatabaseFieldType.String"/> ではない場合。
        /// </exception>
        public DatabaseValueString StringValue
        {
            get
            {
                ThrowHelper.ValidatePropertyAccess(
                    Type != DatabaseFieldType.String,
                    NotCastStringReason
                );
                return stringValue;
            }
        }

        #endregion

        #region private

        private readonly DatabaseValueInt intValue = 0;

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="intValue">数値設定値</param>
        public DatabaseFieldValue(DatabaseValueInt intValue)
        {
            ThrowHelper.ValidateArgumentNotNull(intValue is null, nameof(intValue));

            Type = DatabaseFieldType.Int;
            this.intValue = intValue;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="stringValue">文字列設定値</param>
        /// <exception cref="ArgumentNullException">stringValueがnullの場合</exception>
        public DatabaseFieldValue(DatabaseValueString stringValue)
        {
            ThrowHelper.ValidateArgumentNotNull(stringValue is null, nameof(stringValue));

            Type = DatabaseFieldType.String;
            this.stringValue = stringValue;
        }

        #endregion

        #region Convenience

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="fieldType">項目設定方法種別</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="fieldType"/> が <see langword="null"/> の場合。
        /// </exception>
        public DatabaseFieldValue(DatabaseFieldType fieldType)
        {
            ThrowHelper.ValidateArgumentNotNull(fieldType is null, nameof(fieldType));

            Type = fieldType;
            if (fieldType == DatabaseFieldType.Int)
            {
                intValue = 0;
            }
            else if (fieldType == DatabaseFieldType.String)
            {
                stringValue = "";
            }
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     自身の設定種別を基にデフォルト値を返却する。
        /// </summary>
        /// <returns>
        ///     <see cref="DatabaseValueInt"/> または <see cref="DatabaseValueString"/>。
        ///     どちらのインスタンスを返却するかは <see cref="Type"/> による。
        /// </returns>
        public DatabaseFieldValue GetDefaultValue()
        {
            return new DatabaseFieldValue(Type);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Type.GetHashCode();

                if (Type == DatabaseFieldType.Int)
                {
                    hashCode = (hashCode * 397) ^ intValue.GetHashCode();
                }
                else
                {
                    hashCode = (hashCode * 397) ^ stringValue.GetHashCode();
                }

                return hashCode;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Type);
            if (Type == DatabaseFieldType.Int)
            {
                sb.Append(IntValue);
            }
            else if (Type == DatabaseFieldType.String)
            {
                sb.Append(StringValue);
            }

            return sb.ToString();
        }

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(DatabaseFieldValue? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (Type != obj.Type) return false;

            if (Type == DatabaseFieldType.Int)
            {
                if (intValue != obj.intValue) return false;
            }
            else
            {
                if (stringValue != obj.stringValue) return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
            => Equals(obj as DatabaseFieldValue);

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Operators

        /// <summary>
        ///     <see cref="DatabaseValueInt"/> から <see cref="DatabaseFieldValue"/> への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DatabaseFieldValue(DatabaseValueInt src)
        {
            var result = new DatabaseFieldValue(src);
            return result;
        }

        /// <summary>
        ///     <see cref="DatabaseFieldValue"/> から <see cref="DatabaseValueInt"/> への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     <paramref name="src"/> の <see cref="Type"/> が
        ///     <see cref="DatabaseFieldType.Int"/> ではない場合
        /// </exception>
        public static implicit operator DatabaseValueInt(DatabaseFieldValue src)
        {
            ThrowHelper.InvalidCastIf(
                src.Type != DatabaseFieldType.Int,
                () => NotCastIntReason
            );

            return src.IntValue;
        }

        /// <summary>
        ///     DatabaseValueString から DatabaseFieldValue への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        public static implicit operator DatabaseFieldValue(DatabaseValueString src)
        {
            var result = new DatabaseFieldValue(src);
            return result;
        }

        /// <summary>
        ///     DatabaseFieldValue から DatabaseValueString への暗黙的な型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        /// <exception cref="InvalidCastException">
        ///     <paramref name="src"/> の <see cref="Type"/> が
        ///     <see cref="DatabaseFieldType.String"/> ではない場合
        /// </exception>
        public static implicit operator DatabaseValueString(DatabaseFieldValue src)
        {
            ThrowHelper.InvalidCastIf(
                src.Type != DatabaseFieldType.String,
                () => NotCastIntReason
            );

            return src.StringValue;
        }

        /// <summary>== 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が同一要素である場合 <see langword="true"/></returns>
        public static bool operator ==(DatabaseFieldValue? left, DatabaseFieldValue? right)
        {
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        /// <summary>!= 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns><paramref name="left"/> と <paramref name="right"/> が同一要素ではない場合 <see langword="true"/></returns>
        public static bool operator !=(DatabaseFieldValue? left, DatabaseFieldValue? right) => !(left == right);

        #endregion
    }
}
