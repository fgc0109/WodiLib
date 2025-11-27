// ========================================
// Project Name : WodiLib
// File Name    : DBProjectFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBプロジェクトデータファイル書き出しクラス
    /// </summary>
    public class DBProjectFileWriter : WoditorBinaryFileWriterBase<DatabaseProjectFilePath, DBProject>
    {
        /// <inheritdoc/>
        public DBProjectFileWriter(DatabaseProjectFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc/>
        protected override byte[] GetDataBytes(DBProject data)
        {
            var result = new List<byte>();

            result.AddRange(data.ProjectTypeList.Serialize());

            return result.ToArray();
        }
    }
}
