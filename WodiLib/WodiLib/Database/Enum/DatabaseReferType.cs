// ========================================
// Project Name : WodiLib
// File Name    : DatabaseReferType.cs
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
    ///     DB項目特殊指定「DB参照」の参照先
    /// </summary>
    [TypeSafeEnumJsonConvert]
    public partial class DatabaseReferType : TypeSafeEnum<DatabaseReferType>
    {
        /// <summary>可変DB</summary>
        public static readonly DatabaseReferType Changeable;

        /// <summary>ユーザDB</summary>
        public static readonly DatabaseReferType User;

        /// <summary>システムDB</summary>
        public static readonly DatabaseReferType System;

        /// <summary>コモンイベント</summary>
        public static readonly DatabaseReferType CommonEvent;

        /// <summary>全ての要素</summary>
        public static IEnumerable<DatabaseReferType> AllItems => EnumItems.AllEnums;

        static DatabaseReferType()
        {
            Changeable = new DatabaseReferType(nameof(Changeable), 2);
            User = new DatabaseReferType(nameof(User), 1);
            System = new DatabaseReferType(nameof(System), 0);
            CommonEvent = new DatabaseReferType(nameof(CommonEvent), 3);
        }

        private DatabaseReferType(string id, int code) : base(id)
        {
            Code = code;
        }

        /// <summary>DBデータ設定種別コード</summary>
        public int Code { get; }

        /// <summary>
        ///     DBデータ種別設定コードからオブジェクトを取得する。
        /// </summary>
        /// <param name="code">引数特殊指定値</param>
        /// <returns>インスタンス</returns>
        /// <exception cref="ArgumentException">
        ///     存在しない値の場合。
        /// </exception>
        public static DatabaseReferType FromCode(int code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch (Exception)
            {
                throw new ArgumentException($"{nameof(DatabaseReferType)}の取得に失敗しました。条件値：{code}");
            }
        }
    }
}
