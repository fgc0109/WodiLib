// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseProjectFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     システムデータベースプロジェクトファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?SysDataBase\.project$")]
    public partial record SystemDatabaseProjectFilePath : DatabaseProjectFilePath
    {
        /// <summary>DB種別</summary>
        public override DatabaseKind DbKind => DatabaseKind.System;
    }
}
