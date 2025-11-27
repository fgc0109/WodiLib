// ========================================
// Project Name : WodiLib
// File Name    : DBSettingFolderName.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.JsonConverter.Attributes;
using WodiLib.Sys;

namespace WodiLib.Database
{
    /// <summary>
    ///     [NotNewLine] DB設定フォルダ名
    /// </summary>
    [CommonOneLineStringValueObject]
    [StringValueObjectJsonConvert]
    public partial class DBSettingFolderName
    {
        /// <summary>デフォルト値</summary>
        public static readonly DBSettingFolderName Default = new();

        /// <summary>
        ///     デフォルトコンストラクタ
        /// </summary>
        public DBSettingFolderName() : this("")
        {
        }
    }
}
