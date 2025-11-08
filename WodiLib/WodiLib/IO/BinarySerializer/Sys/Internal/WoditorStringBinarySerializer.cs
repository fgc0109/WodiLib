// ========================================
// Project Name : WodiLib
// File Name    : WoditorStringBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.IO;
using System.Linq;
using WodiLib.Sys;

namespace WodiLib.IO.Sys
{
    /// <summary>
    ///     <see cref="WoditorString"/> インスタンスをシリアル化および逆シリアル化する処理実装クラス
    /// </summary>
    internal static class WoditorStringBinarySerializer
    {
        /// <summary>
        ///     対象インスタンスをStreamに書き込む。
        /// </summary>
        /// <param name="src">書き込み対象</param>
        /// <param name="stream">書き込み先</param>
        public static void Write(WoditorString src, Stream stream)
        {
            stream.Write(src.StringByte.ToArray(), 0, src.ByteLength);
        }

        // public static WoditorString Read(Stream stream)
        // {
        // }
    }
}
