// ========================================
// Project Name : WodiLib
// File Name    : CharaChipFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace WodiLib.Cmn
{
    /// <summary>
    ///     [NotNewLine] キャラチップファイルパス
    /// </summary>
    public record CharaChipFilePath : FilePath
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public CharaChipFilePath(string value = "") : base(value)
        {
        }

        /// <summary>System.String から CharaChipFilePath への 暗黙的な型変換</summary>
        /// <param name="value">変換対象</param>
        /// <returns>変換結果</returns>
        [return: NotNullIfNotNull("value")]
        public static implicit operator CharaChipFilePath?(String? value) => value is null
            ? null
            : new CharaChipFilePath(value);

        /// <summary>CharaChipFilePath から System.String への 暗黙的な型変換</summary>
        /// <param name="value">変換対象</param>
        /// <returns>変換結果</returns>
        [return: NotNullIfNotNull("value")]
        public static implicit operator String?(CharaChipFilePath? value) => value?.RawValue;

        /// <summary>+ 演算子</summary>
        /// <param name="left">左項</param>
        /// <param name="right">右項</param>
        /// <returns>演算結果</returns>
        public static FilePath operator +(CharaChipFilePath left, CharaChipFilePath right)
            => new CharaChipFilePath(left.RawValue + right.RawValue);

        /// <inheritdoc/>
        public override string ToString()
        {
            return RawValue;
        }
    }
}
