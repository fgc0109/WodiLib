// ========================================
// Project Name : WodiLib
// File Name    : SystemDatabaseDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     システムデータベースデータファイル名
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?SysDataBase\.dat$")]
    public partial record SystemDatabaseDatFilePath : DBDatFilePath
    {
        /// <summary>DB種別</summary>
        public override DatabaseKind DbKind => DatabaseKind.System;
    }
}
