// ========================================
// Project Name : WodiLib
// File Name    : DatabaseFieldSpecialSettingType.cs
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
    ///     DB項目値特殊指定タイプ
    /// </summary>
    [TypeSafeEnumJsonConvert]
    public partial class DatabaseFieldSpecialSettingType : TypeSafeEnum<DatabaseFieldSpecialSettingType>
    {
        /// <summary>特殊な指定方法を使用しない</summary>
        public static readonly DatabaseFieldSpecialSettingType Normal;

        /// <summary>ファイル読み込み</summary>
        public static readonly DatabaseFieldSpecialSettingType LoadFile;

        /// <summary>データベース参照</summary>
        public static readonly DatabaseFieldSpecialSettingType ReferDatabase;

        /// <summary>選択肢を手動生成</summary>
        public static readonly DatabaseFieldSpecialSettingType Manual;

        /// <summary>全ての要素</summary>
        public static IEnumerable<DatabaseFieldSpecialSettingType> AllItems => EnumItems.AllEnums;

        static DatabaseFieldSpecialSettingType()
        {
            Normal = new DatabaseFieldSpecialSettingType(nameof(Normal), 0x00);
            LoadFile = new DatabaseFieldSpecialSettingType(nameof(LoadFile), 0x01);
            ReferDatabase = new DatabaseFieldSpecialSettingType(nameof(ReferDatabase), 0x02);
            Manual = new DatabaseFieldSpecialSettingType(nameof(Manual), 0x03);
        }

        private DatabaseFieldSpecialSettingType(string id, byte code) : base(id)
        {
            Code = code;
        }

        /// <summary>コード値</summary>
        public byte Code { get; }

        /// <summary>
        ///     コード値からインスタンスを取得する。
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>インスタンス</returns>
        /// <exception cref="ArgumentException">
        ///     不適切な値の場合。
        /// </exception>
        public static DatabaseFieldSpecialSettingType FromByte(byte code)
        {
            try
            {
                return AllItems.First(x => x.Code == code);
            }
            catch
            {
                throw new ArgumentException($"{nameof(DatabaseFieldSpecialSettingType)}の取得に失敗しました。条件値：{code}");
            }
        }
    }
}
