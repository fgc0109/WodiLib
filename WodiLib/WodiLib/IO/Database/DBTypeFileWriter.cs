// ========================================
// Project Name : WodiLib
// File Name    : DBTypeFileWriter.cs
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
    public class DBTypeFileWriter : WoditorBinaryFileWriterBase<DBTypeFilePath, DBType>
    {
        /// <inheritdoc/>
        public DBTypeFileWriter(DBTypeFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc/>
        protected override byte[] GetDataBytes(DBType data)
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(DBTypeFile.Header);

            // 要素
            result.AddRange(data.TypeMetadataTable.Serialize());

            return result.ToArray();
        }
    }
}
