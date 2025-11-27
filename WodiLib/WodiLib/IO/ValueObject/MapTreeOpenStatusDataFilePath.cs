// ========================================
// Project Name : WodiLib
// File Name    : MapTreeOpenStatusDataFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     MapTreeOpenState.dat ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?MapTreeOpenStatus\.dat$")]
    public partial record MapTreeOpenStatusDataFilePath : FilePath
    {
    }
}
