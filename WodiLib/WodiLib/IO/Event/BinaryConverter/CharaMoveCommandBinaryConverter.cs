// ========================================
// Project Name : WodiLib
// File Name    : CharaMoveCommandBinaryConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     <see cref="CharaMoveCommand"/> をバイナリ配列に変換するための処理定義クラス
    /// </summary>
    public static class CharaMoveCommandBinaryConverter
    {
        /// <summary>
        /// 終了バイトコード
        /// </summary>
        public static byte[] EndBlockCode => new byte[] { 0x01, 0x00 };

        /// <inheritdoc cref="ToBinary(ReadOnlyCharaMoveCommand)"/>
        public static byte[] ToBinary(this CharaMoveCommand src)
            => ToBinary(((ICastableImmutable<ReadOnlyCharaMoveCommand>)src).AsImmutable());

        /// <summary>
        /// バイナリ変換する。
        /// </summary>
        /// <param name="src">変換対象</param>
        /// <returns>変換したバイナリデータ</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="src"/> が <see langword="null"/> の場合。
        /// </exception>
        public static byte[] ToBinary(this ReadOnlyCharaMoveCommand src)
        {
            var result = new List<byte> { src.CommandCode.Code, src.ValueLengthByte };
            // 動作コマンドコード
            // 変数の数
            // 変数
            foreach (var value in src.ListCommandValues())
            {
                result.AddRange(value.ToBytes(Endian.Woditor));
            }

            // 終端コード
            result.AddRange(EndBlockCode);
            return result.ToArray();
        }
    }
}
