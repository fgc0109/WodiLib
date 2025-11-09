// ========================================
// Project Name : WodiLib
// File Name    : Endian.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;

namespace WodiLib.Sys
{
    /// <summary>
    ///     エンディアン
    /// </summary>
    public class Endian : TypeSafeEnum<Endian>
    {
        /// <summary>ビッグエンディアン</summary>
        public static readonly Endian Big;

        /// <summary>リトルエンディアン</summary>
        public static readonly Endian Little;

        /// <summary>全ての要素</summary>
        public static IEnumerable<Endian> AllItems => EnumItems.AllEnums;

        static Endian()
        {
            Big = new Endian("Big");
            Little = new Endian("Little");
        }

        private Endian(string id) : base(id)
        {
        }

        /// <summary>
        ///     現在の環境で使用されているエンディアン
        /// </summary>
        public static Endian Environment
        {
            get
            {
                if (BitConverter.IsLittleEndian) return Little;
                return Big;
            }
        }

        /// <summary>
        ///     ウディタ内部で使用されるエンディアン
        /// </summary>
        public static Endian Woditor => Little;
    }
}
