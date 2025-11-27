// ========================================
// Project Name : WodiLib
// File Name    : DBTypeFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBプロジェクトデータファイル読み込みクラス
    /// </summary>
    public class DBTypeFileReader : WoditorFileReaderBase<DBTypeFilePath, DBType>
    {
        #region Properties

        #region private

        /// <summary>ファイル読み込みステータス</summary>
        private FileReadStatus ReadStatus { get; }

        /// <summary>ロガー</summary>
        private WodiLibLogger WodiLibLogger { get; } = WodiLibLogger.GetInstance();

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Fields

        private readonly object readLock = new();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Constructors

        #region Required

        /// <inheritdoc/>
        public DBTypeFileReader(DBTypeFilePath filePath) : base(filePath)
        {
            ReadStatus = new FileReadStatus(filePath);
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public override DBType ReadSync()
        {
            lock (readLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileRead(GetType()));

                var typeTableSettings =
                    new DatabaseTypeMetadataTableSettings(new List<IDatabaseNamedDataRowSettings>());

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // タイプ設定
                ReadTypeSetting(ReadStatus, typeTableSettings, out var dataNameListSettings);

                // データ設定
                ReadDataSetting(ReadStatus, dataNameListSettings, typeTableSettings);

                WodiLibLogger.Info(FileIOMessage.EndFileRead(GetType()));

                var typeSettings = new DBTypeSettings
                {
                    TypeMetadataTable = typeTableSettings,
                };
                return new DBType(typeSettings);
            }
        }

        #endregion

        #region private

        /// <summary>
        ///     ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in DBTypeFile.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）"
                    );
                }

                status.IncreaseByteOffset();
            }

            WodiLibLogger.Debug(
                FileIOMessage.CheckOk(
                    typeof(DBTypeFileReader),
                    "ヘッダ"
                )
            );
        }

        /// <summary>
        ///     タイプ設定 $amp; データ名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="typeTableSettings">結果格納先(タイプ設定)</param>
        /// <param name="dataNameListSettings">結果格納先(データ名)</param>
        private void ReadTypeSetting(
            FileReadStatus status,
            DatabaseTypeMetadataTableSettings typeTableSettings,
            out DatabaseDataNameListSettings dataNameListSettings
        )
        {
            WodiLibLogger.Debug(
                FileIOMessage.StartCommonRead(
                    typeof(DBTypeFileReader),
                    "タイプ設定"
                )
            );

            var readResult = DatabaseTypeDefinitionReader.Read(
                status,
                length: 1,
                hasDataNameList: true
            );
            var typeDefinitionSettings = readResult.TypeDefinitionSettingsList[0];
            typeTableSettings.TypeName = typeDefinitionSettings.TypeName;
            typeTableSettings.Memo = typeDefinitionSettings.Memo;
            typeTableSettings.FieldMetadataList =
                typeDefinitionSettings.FieldDefinitionList.TransformMetadataSettings();

            dataNameListSettings = readResult.DataNameListSettingsList[0];

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DBTypeFileReader),
                    "タイプ設定"
                )
            );
        }

        /// <summary>
        ///     データ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="dataNameList">データ名</param>
        /// <param name="typeTableSettings">読み込み結果インスタンス(データ名の設定方法)</param>
        private void ReadDataSetting(
            FileReadStatus status,
            DatabaseDataNameListSettings dataNameList,
            DatabaseTypeMetadataTableSettings typeTableSettings
        )
        {
            WodiLibLogger.Debug(
                FileIOMessage.StartCommonRead(
                    typeof(DBTypeFileReader),
                    "データ設定"
                )
            );

            var readResult = DatabaseDataTableWithDataNamingReader.Read(
                status,
                length: 1
            );
            var definitionSettings = readResult.Settings[0];
            typeTableSettings.DataNamingDefinition = definitionSettings.DataNamingDefinition;

            definitionSettings.DataTable.Settings.Zip(dataNameList.Settings)
                .ForEach(dataSet =>
                    {
                        var (dataRow, name) = dataSet;
                        var rowSettings = new DatabaseNamedDataRowSettings(dataRow.Settings)
                        {
                            DataName = name,
                        };
                        typeTableSettings.Settings.Add(rowSettings);
                    }
                );

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DBTypeFileReader),
                    "データ設定"
                )
            );
        }

        #endregion

        #endregion
    }
}
