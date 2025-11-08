// ========================================
// Project Name : WodiLib
// File Name    : Direction.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     方向
    /// </summary>
    public class Direction : TypeSafeEnum<Direction>
    {
        /// <summary>行</summary>
        public static Direction Row { get; }

        /// <summary>列</summary>
        public static Direction Column { get; }

        /// <summary>未指定</summary>
        public static Direction None { get; }

        /// <summary>全ての要素</summary>
        public static IEnumerable<Direction> AllItems => EnumItems.AllEnums;

        static Direction()
        {
            Row = new Direction(nameof(Row));
            Column = new Direction(nameof(Column));
            None = new Direction(nameof(None));
        }

        private Direction(string id) : base(id)
        {
        }
    }
}
