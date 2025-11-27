// ========================================
// Project Name : WodiLib
// File Name    : CommonEventDatFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     CommonEvent.Datファイル名
    /// </summary>
    [FilePathStringObjectValue(
        SafetyPattern = @"^(.+\\)?CommonEvent\.dat$"
    )]
    public partial record CommonEventDatFilePath : FilePath
    {
    }
}
