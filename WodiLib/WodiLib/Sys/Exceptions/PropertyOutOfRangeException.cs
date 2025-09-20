// ========================================
// Project Name : WodiLib
// File Name    : PropertyOutOfRangeException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <inheritdoc/>
    /// <summary>
    ///     プロパティに許容範囲外の値が渡されたときの例外
    /// </summary>
    public class PropertyOutOfRangeException : PropertyException
    {
        /// <inheritdoc/>
        public PropertyOutOfRangeException()
        {
        }

        /// <inheritdoc/>
        public PropertyOutOfRangeException(string? message) : base(message)
        {
        }

        /// <inheritdoc/>
        public PropertyOutOfRangeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
