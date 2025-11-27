// ========================================
// Project Name : WodiLib
// File Name    : TileSetDataFilePath.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.Cmn;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     TileSetData.dat ファイルパス
    /// </summary>
    [FilePathStringObjectValue(SafetyPattern = @"^(.+\\)?TileSetData\.dat$")]
    public partial record TileSetDataFilePath : FilePath
    {
    }
}
