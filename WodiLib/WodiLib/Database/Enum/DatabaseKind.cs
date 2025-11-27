// ========================================
// Project Name : WodiLib
// File Name    : DatabaseKind.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     DB種別
    /// </summary>
    [TypeSafeEnumJsonConvert]
    public partial class DatabaseKind : TypeSafeEnum<DatabaseKind>
    {
        /// <summary>可変DB</summary>
        public static readonly DatabaseKind Changeable;

        /// <summary>ユーザDB</summary>
        public static readonly DatabaseKind User;

        /// <summary>システムDB</summary>
        public static readonly DatabaseKind System;

        /// <summary>全ての要素</summary>
        public static IEnumerable<DatabaseKind> AllItems => EnumItems.AllEnums;

        static DatabaseKind()
        {
            Changeable = new DatabaseKind(
                nameof(Changeable),
                0x00
            );
            User = new DatabaseKind(
                nameof(User),
                0x02
            );
            System = new DatabaseKind(
                nameof(System),
                0x01
            );
        }

        private DatabaseKind(
            string id,
            byte code
        ) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>
        ///     対象DBコードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">DBコード</param>
        /// <returns>インスタンス</returns>
        /// <exception cref="ArgumentException">
        ///     存在しない値の場合。
        /// </exception>
        public static DatabaseKind FromCode(byte code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"DBKindの取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }
    }
}
