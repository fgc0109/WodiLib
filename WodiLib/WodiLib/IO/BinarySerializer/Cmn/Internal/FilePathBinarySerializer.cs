// ========================================
// Project Name : WodiLib
// File Name    : FilePathBinarySerializer.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.IO;
using WodiLib.Cmn;
using WodiLib.IO.Sys;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="FilePath"/> インスタンスをシリアル化および逆シリアル化する処理実装クラス
    /// </summary>
    internal static class FilePathBinarySerializer
    {
        /// <summary>
        ///     対象インスタンスをStreamに書き込む。
        /// </summary>
        /// <param name="src">書き込み対象</param>
        /// <param name="stream">書き込み先</param>
        public static void Write(FilePath src, Stream stream)
        {
            var woditorStr = new WoditorString(src.RawValue);
            WoditorStringBinarySerializer.Write(woditorStr, stream);
        }

        // public static FilePath Read(Stream stream)
        // {
        // }
    }
}
