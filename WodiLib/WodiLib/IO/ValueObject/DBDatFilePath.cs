// ========================================
// Project Name : WodiLib
// File Name    : DBDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     データベースデータファイル名
    /// </summary>
    [FilePathStringObjectValue]
    public abstract partial record DBDatFilePath : FilePath
    {
        /// <summary>DB種別</summary>
        public abstract DatabaseKind DbKind { get; }
    }
}
