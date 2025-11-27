// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetFileWriter.cs
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
    public class DBTypeSetFileWriter : WoditorBinaryFileWriterBase<DBTypeSetFilePath, DBTypeSet>
    {
        /// <inheritdoc/>
        public DBTypeSetFileWriter(DBTypeSetFilePath filePath) : base(filePath)
        {
        }

        /// <inheritdoc/>
        protected override byte[] GetDataBytes(DBTypeSet data)
        {
            var result = new List<byte>();

            // ヘッダ
            result.AddRange(DBTypeSetFile.Header);

            // 項目数
            result.AddRange(data.TypeDefinition.FieldCount.ToWoditorIntBytes());

            // 設定種別 & 種別順列
            result.AddRange(data.TypeDefinition.FieldDefinitionList.SerializeFieldTypesAndOrder());

            // タイプ名
            result.AddRange(((string)data.TypeDefinition.TypeName).ToWoditorStringBytes());

            // 項目数
            result.AddRange(data.TypeDefinition.FieldCount.ToWoditorIntBytes());

            // 項目名
            result.AddRange(data.TypeDefinition.FieldDefinitionList.SerializeFieldNames());

            // メモ
            result.AddRange(((string)data.TypeDefinition.Memo).ToWoditorStringBytes());

            // 特殊指定
            result.AddRange(data.TypeDefinition.FieldDefinitionList.SerializeSpecialSettingDescription());

            return result.ToArray();
        }
    }
}
