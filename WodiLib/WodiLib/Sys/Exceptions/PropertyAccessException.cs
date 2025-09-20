// ========================================
// Project Name : WodiLib
// File Name    : PropertyAccessException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;

namespace WodiLib.Sys
{
    /// <inheritdoc/>
    /// <summary>
    ///     プロパティアクセス禁止例外
    /// </summary>
    public class PropertyAccessException : PropertyException
    {
        /// <inheritdoc/>
        public PropertyAccessException()
        {
        }

        /// <inheritdoc/>
        public PropertyAccessException(string? message) : base(message)
        {
        }

        /// <inheritdoc/>
        public PropertyAccessException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
