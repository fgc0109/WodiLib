// ========================================
// Project Name : WodiLib
// File Name    : UserDatabaseDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     ユーザデータベースデータファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?DataBase\.dat$")]
    public partial record UserDatabaseDatFilePath : DBDatFilePath
    {
        /// <summary>DB種別</summary>
        public override DatabaseKind DbKind => DatabaseKind.System;
    }
}
