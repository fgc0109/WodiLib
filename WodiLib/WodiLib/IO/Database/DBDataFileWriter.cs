// ========================================
// Project Name : WodiLib
// File Name    : DBDataFileWriter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.Database;
using WodiLib.Sys;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBファイル書き出しクラス
    /// </summary>
    public class DBDataFileWriter : WoditorBinaryFileWriterBase<DBDataFilePath, DBData>
    {
        /// <inheritdoc/>
        public DBDataFileWriter(DBDataFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc/>
        protected override byte[] GetDataBytes(DBData data)
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(DBDataFile.Header);

            // データ数
            result.AddRange(data.DataTable.DataCount.ToWoditorIntBytes());

            // データ
            result.AddRange(data.DataTable.SerializeValuesDividedType());

            return result.ToArray();
        }
    }
}
