// ========================================
// Project Name : WodiLib
// File Name    : EventCommandBinaryConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.Event;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    /// <see cref="EventCommand"/> をバイナリ配列に変換するための処理定義クラス
    /// </summary>
    public static class EventCommandBinaryConverter
    {
        /// <summary>動作指定なしフラグ値</summary>
        private const byte FlgNotHasActionEntry = 0x00;

        /// <summary>動作指定ありフラグ値</summary>
        private const byte FlgHasActionEntry = 0x01;

        /// <inheritdoc cref="ToBinary(WodiLib.Event.ReadOnlyEventCommand)"/>
        public static byte[] ToBinary(this EventCommand src)
            => ToBinary(((ICastableImmutable<ReadOnlyEventCommand>)src).AsImmutable());

        /// <summary>バイナリデータに変換する。</summary>
        /// <returns>バイナリデータ</returns>
        public static byte[] ToBinary(this ReadOnlyEventCommand src)
        {
            src.OutputVersionWarningLogIfNeed();

            var result = new List<byte>();

            // 数値変数
            result.AddRange(src.MakeNumberVariableBytes());
            // インデントの深さ
            result.AddRange(src.MakeIndentBytes());
            // 文字列変数
            result.AddRange(src.MakeStringVariableBytes());
            // 動作指定コマンド
            result.AddRange(src.MakeActionEntryBytes());

            return result.ToArray();
        }

        private static byte[] MakeNumberVariableBytes(this ReadOnlyEventCommand src)
        {
            var args = src.AllNumberArgList;

            var resultSeed = new List<byte>();

            var argsLengthByte = (byte)args.Count;
            resultSeed.Add(argsLengthByte);

            return args.Select(x => x.ToWoditorIntBytes())
                .Aggregate(
                    resultSeed,
                    (n, elem) =>
                    {
                        n.AddRange(elem);
                        return n;
                    }
                )
                .ToArray();
        }

        private static byte[] MakeIndentBytes(this ReadOnlyEventCommand src)
        {
            return new[] { src.Indent.ToSByte().ToByte() };
        }

        private static byte[] MakeStringVariableBytes(this ReadOnlyEventCommand src)
        {
            var args = src.AllStringArgList;

            var resultSeed = new List<byte>();

            var argsLengthByte = (byte)args.Count;
            resultSeed.Add(argsLengthByte);

            return args.Select(x => new WoditorString(x).StringByte)
                .Aggregate(
                    resultSeed,
                    (n, elem) =>
                    {
                        n.AddRange(elem);
                        return n;
                    }
                )
                .ToArray();
        }

        private static byte[] MakeActionEntryBytes(this ReadOnlyEventCommand src)
        {
            var result = new List<byte>();
            if (src.ActionEntry is null)
            {
                result.Add(FlgNotHasActionEntry);
                return result.ToArray();
            }

            result.Add(FlgHasActionEntry);
            result.AddRange(src.ActionEntry.ToBinary());
            return result.ToArray();
        }
    }
}
