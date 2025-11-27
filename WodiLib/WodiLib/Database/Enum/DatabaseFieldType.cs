// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldType.cs
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
    ///     項目の設定方法種別
    /// </summary>
    [TypeSafeEnumJsonConvert]
    public partial class DatabaseFieldType : TypeSafeEnum<DatabaseFieldType>
    {
        /// <summary>数値</summary>
        public static readonly DatabaseFieldType Int;

        /// <summary>文字列</summary>
        public static readonly DatabaseFieldType String;

        /// <summary>全ての要素</summary>
        public static IEnumerable<DatabaseFieldType> AllItems => EnumItems.AllEnums;

        static DatabaseFieldType()
        {
            Int = new DatabaseFieldType(nameof(Int), 1);
            String = new DatabaseFieldType(nameof(String), 2);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        private DatabaseFieldType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public int Code { get; }

        /// <summary>種別順列基準値</summary>
        public int TypeOrderStart => Code * 1000;

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>インスタンス</returns>
        /// <exception cref="ArgumentException">
        ///     存在しない値の場合。
        /// </exception>
        public static DatabaseFieldType FromCode(int code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch (Exception)
            {
                var exception = new ArgumentException($"{nameof(DatabaseFieldType)}の取得に失敗しました。条件値：{code}");
                throw exception;
            }
        }
    }
}
