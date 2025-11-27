// ========================================
// Project Name : WodiLib
// File Name    : DBDatFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBファイル書き出しクラス
    /// </summary>
    public class DBDatFileWriter : WoditorBinaryFileWriterBase<DBDatFilePath, DBDat>
    {
        /// <inheritdoc/>
        public DBDatFileWriter(DBDatFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc/>
        protected override byte[] GetDataBytes(DBDat data)
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(DatabaseDatFile.Header);

            // DBデータ設定
            result.AddRange(data.DataTableDefinitionList.Serialize());

            // ファイルフッタ
            result.AddRange(DatabaseDatFile.Footer);

            return result.ToArray();
        }
    }
}
