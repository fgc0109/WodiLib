// ========================================
// Project Name : WodiLib
// File Name    : PropertyException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <inheritdoc/>
    /// <summary>
    ///     プロパティの例外
    /// </summary>
    public class PropertyException : Exception
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public PropertyException()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public PropertyException(string? message) : base(message)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="innerException">内包するエラー</param>
        public PropertyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
