// ========================================
// Project Name : WodiLib
// File Name    : MapTreeDataFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     MapTree.dat ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?MapTree\.dat$")]
    public partial record MapTreeDataFilePath : FilePath
    {
    }
}
