// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXX.dbtypeset ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^.+\.dbtypeset$")]
    public partial record DBTypeSetFilePath : FilePath
    {
    }
}
