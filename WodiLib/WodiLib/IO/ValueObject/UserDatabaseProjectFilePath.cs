// ========================================
// Project Name : WodiLib
// File Name    : UserDatabaseProjectFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     ユーザデータベースプロジェクトファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?DataBase\.project$")]
    public partial record UserDatabaseProjectFilePath : DatabaseProjectFilePath
    {
        /// <summary>DB種別</summary>
        public override DatabaseKind DbKind => DatabaseKind.System;
    }
}
