// ========================================
// Project Name : WodiLib
// File Name    : PropertyNullException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <inheritdoc/>
    /// <summary>
    ///     Nullを許容していないプロパティにNullが渡されたときの例外
    /// </summary>
    public class PropertyNullException : PropertyException
    {
        /// <inheritdoc/>
        public PropertyNullException()
        {
        }

        /// <inheritdoc/>
        public PropertyNullException(string? message) : base(message)
        {
        }

        /// <inheritdoc/>
        public PropertyNullException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
