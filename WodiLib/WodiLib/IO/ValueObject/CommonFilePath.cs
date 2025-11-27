// ========================================
// Project Name : WodiLib
// File Name    : CommonFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXX.common ファイルパス
    /// </summary>
    [FilePathStringObjectValue(
        SafetyPattern = @"^(.+\\)?CommonEvent\.dat$"
    )]
    public partial record CommonFilePath : FilePath
    {
    }
}
