// ========================================
// Project Name : WodiLib
// File Name    : MpsFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXX.mps ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^.+\.mps$")]
    public partial record MpsFilePath : FilePath
    {
    }
}
