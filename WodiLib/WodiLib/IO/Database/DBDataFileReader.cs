// ========================================
// Project Name : WodiLib
// File Name    : DBDataFileReader.cs
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
    public class DBDataFileReader : WoditorFileReaderBase<DBDataFilePath, DBData>
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

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="filePath">読み込みファイルパス</param>
        /// <exception cref="ArgumentNullException">filePathがnullの場合</exception>
        public DBDataFileReader(DBDataFilePath filePath) : base(filePath)
        {
            ReadStatus = new FileReadStatus(FilePath);
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public override DBData ReadSync()
        {
            lock (readLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileRead(GetType()));

                var settings = new DBDataSettings
                {
                    DataTable = new DatabaseNamedDataTableSettings(new List<IDatabaseNamedDataRowSettings>()),
                };

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // DBデータ
                ReadDbData(ReadStatus, settings);

                WodiLibLogger.Info(FileIOMessage.EndFileRead(GetType()));

                return new DBData(settings);
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
            foreach (var b in DBDataFile.Header)
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
                    typeof(DBDataFileReader),
                    "ヘッダ"
                )
            );
        }

        private void ReadDbData(FileReadStatus status, DBDataSettings settings)
        {
            // データ数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DBDataFileReader),
                    "データ数数",
                    length
                )
            );

            // DBデータ
            for (var i = 0; i < length; i++)
            {
                var rowSettings = new DatabaseNamedDataRowSettings();

                // データ名
                var dataName = status.ReadString();
                status.AddOffset(dataName.ByteLength);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DBDataFileReader),
                        "データ名",
                        dataName.String
                    )
                );

                rowSettings.DataName = dataName.String;

                // 数値項目
                ReadDbDataIntValues(status, out var intValues);
                foreach (var value in intValues)
                {
                    rowSettings.Settings.Add(value);
                }

                // 文字列項目
                ReadDbDataStringValues(status, out var stringValues);
                foreach (var value in stringValues)
                {
                    rowSettings.Settings.Add(value);
                }

                settings.DataTable.Settings.Add(rowSettings);
            }
        }

        /// <summary>
        ///     DBデータの数値項目
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="values">読み込み結果</param>
        private void ReadDbDataIntValues(FileReadStatus status, out IEnumerable<DatabaseValueInt> values)
        {
            // 数値項目数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DBDataFileReader),
                    "数値項目数",
                    length
                )
            );

            var result = new List<DatabaseValueInt>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadInt();
                status.IncreaseIntOffset();

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DBDataFileReader),
                        $"  数値項目{i,2}",
                        value
                    )
                );

                result.Add(value);
            }

            values = result;
        }


        /// <summary>
        ///     DBデータの文字列項目
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="values">読み込み結果</param>
        private void ReadDbDataStringValues(FileReadStatus status, out IEnumerable<DatabaseValueString> values)
        {
            // 数値項目数
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DBDataFileReader),
                    "文字列項目数",
                    length
                )
            );

            var result = new List<DatabaseValueString>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadString();
                status.AddOffset(value.ByteLength);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DBDataFileReader),
                        $"  文字列項目{i,2}",
                        value
                    )
                );

                DatabaseValueString databaseValueString = value.String;
                result.Add(databaseValueString);
            }

            values = result;
        }

        #endregion

        #endregion
    }
}
