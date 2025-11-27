// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataNamingType.cs
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
    ///     データの設定方法種別
    /// </summary>
    [TypeSafeEnumJsonConvert]
    public partial class DatabaseDataNamingType : TypeSafeEnum<DatabaseDataNamingType>
    {
        /// <summary>手動で設定</summary>
        public static readonly DatabaseDataNamingType Manual;

        /// <summary>最初の文字列データと同じ</summary>
        public static readonly DatabaseDataNamingType FirstStringData;

        /// <summary>1つ前のタイプのデータIDと同じ</summary>
        public static readonly DatabaseDataNamingType EqualBefore;

        /// <summary>指定DBの指定タイプから</summary>
        public static readonly DatabaseDataNamingType DesignatedType;

        /// <summary>全ての要素</summary>
        public static IEnumerable<DatabaseDataNamingType> AllItems => EnumItems.AllEnums;

        static DatabaseDataNamingType()
        {
            Manual = new DatabaseDataNamingType(nameof(Manual), 0);
            FirstStringData = new DatabaseDataNamingType(nameof(FirstStringData), 1);
            EqualBefore = new DatabaseDataNamingType(nameof(EqualBefore), 2);
            DesignatedType = new DatabaseDataNamingType(nameof(DesignatedType), -1);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="code">コード値</param>
        private DatabaseDataNamingType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public int Code { get; }

        /// <summary>
        ///     対象コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns>インスタンス</returns>
        /// <exception cref="ArgumentException">
        ///     存在しない値の場合。
        /// </exception>
        public static DatabaseDataNamingType FromCode(int code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch
            {
                throw new ArgumentException($"{nameof(DatabaseDataNamingType)}の取得に失敗しました。条件値：{code}");
            }
        }
    }
}
