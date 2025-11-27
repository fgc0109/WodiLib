// ========================================
// Project Name : WodiLib
// File Name    : ChangeableDatabaseDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     可変データベースデータファイル名
    /// </summary>
    [FilePathStringObjectValue(
        SafetyPattern = @"^(.+\\)?CDataBase\.dat$"
    )]
    public partial record ChangeableDatabaseDatFilePath : DBDatFilePath
    {
        /// <summary>DB種別</summary>
        public override DatabaseKind DbKind => DatabaseKind.Changeable;
    }
}
