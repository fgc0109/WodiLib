// ========================================
// Project Name : WodiLib
// File Name    : BinaryFormatterException.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Runtime.Serialization;

namespace WodiLib.IO
{
    /// <summary>
    ///     ファイルシリアライズ時に発生するエラー
    /// </summary>
    public class BinaryFormatterException : SerializationException
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public BinaryFormatterException(string message) : base(message)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException">発生した例外</param>
        public BinaryFormatterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
