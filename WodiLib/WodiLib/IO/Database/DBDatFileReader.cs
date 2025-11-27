// ========================================
// Project Name : WodiLib
// File Name    : DBDatFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBファイル読み込みクラス
    /// </summary>
    public class DBDatFileReader : WoditorFileReaderBase<DBDatFilePath, DBDat>
    {
        #region Properties

        #region private

        /// <summary>読み込みDB種別</summary>
        private DatabaseKind DBKind { get; }

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
        /// <param name="dbKind">読み込みDB種別</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filePath"/>, <paramref name="dbKind"/> が <see langword="null"/> の場合。
        /// </exception>
        public DBDatFileReader(DBDatFilePath filePath, DatabaseKind dbKind) : base(filePath)
        {
            ThrowHelper.ValidateArgumentNotNull(dbKind is null, nameof(dbKind));

            DBKind = dbKind;
            ReadStatus = new FileReadStatus(filePath);
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <inheritdoc/>
        public override DBDat ReadSync()
        {
            lock (readLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileRead(GetType()));

                var settings = new DBDatSettings();

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // DBデータ
                ReadDBData(ReadStatus, settings);

                // フッタチェック
                ReadFooter(ReadStatus);

                // DB種別
                settings.DbKind = DBKind;

                WodiLibLogger.Info(FileIOMessage.EndFileRead(GetType()));

                return new DBDat(settings);
            }
        }

        #endregion

        #region private

        /// <summary>
        ///     ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">
        ///     ファイルヘッダが仕様と異なる場合。
        /// </exception>
        private void ReadHeader(FileReadStatus status)
        {
            foreach (var b in DatabaseDatFile.Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）"
                    );
                }

                status.IncreaseByteOffset();
            }

            WodiLibLogger.Debug($"{nameof(DBDatFileReader)} ヘッダチェックOK");
        }

        /// <summary>
        ///     DBデータ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="data">結果格納インスタンス</param>
        private void ReadDBData(FileReadStatus status, DBDatSettings data)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "タイプ数",
                    length
                )
            );

            var dataTableWithDataNamingDefinitionListSettings =
                DatabaseDataTableWithDataNamingReader.Read(status, length);
            data.DataTableDefinitionList = dataTableWithDataNamingDefinitionListSettings;

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DBDatFileReader),
                    "DBデータ設定"
                )
            );
        }

        /// <summary>
        ///     フッタ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ファイルフッタが仕様と異なる場合</exception>
        private void ReadFooter(FileReadStatus status)
        {
            foreach (var b in DatabaseDatFile.Footer)
            {
                if (status.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルフッタがファイル仕様と異なります（offset:{status.Offset}）"
                    );
                }

                status.IncreaseByteOffset();
            }

            WodiLibLogger.Debug($"{nameof(DBDatFileReader)} フッタチェックOK");
        }

        #endregion

        #endregion
    }
}
