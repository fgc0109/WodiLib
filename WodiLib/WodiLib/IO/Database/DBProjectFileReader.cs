// ========================================
// Project Name : WodiLib
// File Name    : DBProjectFileReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBプロジェクトデータファイル読み込みクラス
    /// </summary>
    public class DBProjectFileReader : WoditorFileReaderBase<DatabaseProjectFilePath, DBProject>
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
        public DBProjectFileReader(DatabaseProjectFilePath filePath, DatabaseKind dbKind) : base(filePath)
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
        public override DBProject ReadSync()
        {
            lock (readLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileRead(GetType()));

                var settings = new DBProjectSettings();

                ReadTypeSettingList(ReadStatus, settings);

                // DB種別
                settings.DbKind = DBKind;

                WodiLibLogger.Info(FileIOMessage.EndFileRead(GetType()));

                return new DBProject(settings);
            }
        }

        #endregion

        #region private

        /// <summary>
        ///     タイプ設定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="settings">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadTypeSettingList(FileReadStatus status, DBProjectSettings settings)
        {
            WodiLibLogger.Debug(
                FileIOMessage.StartCommonRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "タイプ設定リスト"
                )
            );

            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DBProjectFileReader),
                    "タイプ設定数",
                    length
                )
            );

            var readResult = DatabaseTypeDefinitionReader.Read(
                status,
                length,
                hasDataNameList: true
            );

            settings.ProjectTypeList = ConvertTypeListSettings(readResult);

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "タイプ設定リスト"
                )
            );
        }

        private static IDatabaseProjectTypeListSettings ConvertTypeListSettings(
            DatabaseTypeDefinitionReader.ReadResultItem readResult
        )
        {
            var typeSettings = readResult.TypeDefinitionSettingsList.Zip(readResult.DataNameListSettingsList)
                .Select(ConvertTypeDefinitionToProjectType)
                .ToArray();
            return new DatabaseProjectTypeListSettings(typeSettings);
        }

        private static IDatabaseProjectTypeSettings ConvertTypeDefinitionToProjectType(
            (DatabaseTypeDefinitionSettings, DatabaseDataNameListSettings) tuple
        )
        {
            var result = new DatabaseProjectTypeSettings
            {
                TypeName = tuple.Item1.TypeName,
                Memo = tuple.Item1.Memo,
                FieldMetadataList = tuple.Item1.FieldDefinitionList.TransformMetadataSettings(),
                DataNameList = tuple.Item2,
            };

            return result;
        }

        #endregion

        #endregion
    }
}
