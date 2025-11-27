// ========================================
// Project Name : WodiLib
// File Name    : EditorIniFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     Editor.ini ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?Editor\.ini$")]
    public partial record EditorIniFilePath : FilePath
    {
    }
}
