// ========================================
// Project Name : WodiLib
// File Name    : TileSetFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     XXX.title ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^.+\.tile$")]
    public partial record TileSetFilePath : FilePath
    {
    }
}
