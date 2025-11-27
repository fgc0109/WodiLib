// ========================================
// Project Name : WodiLib
// File Name    : DBTypeSetFileReader.cs
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
    public class DBTypeSetFileReader : WoditorFileReaderBase<DBTypeSetFilePath, DBTypeSet>
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
        public DBTypeSetFileReader(DBTypeSetFilePath filePath) : base(filePath)
        {
            ReadStatus = new FileReadStatus(FilePath);
        }

        #endregion

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     ファイルを同期的に読み込む
        /// </summary>
        /// <returns>読み込んだデータ</returns>
        /// <exception cref="InvalidOperationException">
        ///     ファイルが正しく読み込めなかった場合
        /// </exception>
        public override DBTypeSet ReadSync()
        {
            lock (readLock)
            {
                WodiLibLogger.Info(FileIOMessage.StartFileRead(GetType()));

                var dbTypeSetSettings = new DBTypeSetSettings();

                // ヘッダチェック
                ReadHeader(ReadStatus);

                // 項目種別
                ReadValueType(ReadStatus, out var itemTypes);

                // タイプ設定
                ReadTypeSetting(ReadStatus, itemTypes, out var typeDefinitionSettings);
                dbTypeSetSettings.TypeDefinition = typeDefinitionSettings;

                WodiLibLogger.Info(FileIOMessage.EndFileRead(GetType()));

                return new DBTypeSet(dbTypeSetSettings);
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
            foreach (var b in DBTypeSetFile.Header)
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
                    typeof(DBTypeSetFileReader),
                    "ヘッダ"
                )
            );
        }

        /// <summary>
        ///     設定種別 &amp; 種別順列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="itemTypes">取得した項目種別リスト格納先</param>
        private void ReadValueType(FileReadStatus status, out List<DatabaseFieldType> itemTypes)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DBTypeSetFileReader),
                    "項目数",
                    length
                )
            );

            var countDic = new Dictionary<DatabaseFieldType, int>
            {
                { DatabaseFieldType.Int, 0 },
                { DatabaseFieldType.String, 0 },
            };

            itemTypes = new List<DatabaseFieldType>();

            for (var i = 0; i < length; i++)
            {
                var settingCode = status.ReadInt();
                status.IncreaseIntOffset();

                var itemType = DatabaseFieldTypeMapper.FromSettingsValue(settingCode);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DBTypeSetFileReader),
                        $"  項目{i,2}設定種別",
                        itemType
                    )
                );

                // 項目タイプ数集計
                countDic[itemType]++;

                // 種別順位は無視する

                itemTypes.Add(itemType);
            }

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DBTypeSetFileReader),
                    "項目設定種別"
                )
            );
        }

        private void ReadTypeSetting(
            FileReadStatus status,
            IReadOnlyList<DatabaseFieldType> itemTypes,
            out DatabaseTypeDefinitionSettings typeDefinitionSettings
        )
        {
            WodiLibLogger.Debug(
                FileIOMessage.StartCommonRead(
                    typeof(DBTypeSetFileReader),
                    "タイプ設定"
                )
            );

            var typeDefinitionReadResult = DatabaseTypeDefinitionReader.Read(
                status,
                length: 1,
                hasDataNameList: false
            );
            typeDefinitionSettings = typeDefinitionReadResult.TypeDefinitionSettingsList[0];

            if (typeDefinitionSettings.FieldDefinitionList.Settings.Count != itemTypes.Count)
            {
                throw new BinaryFormatterException(
                    $"項目値種別数と項目設定数が一致しません。"
                );
            }

            for (var i = 0; i < typeDefinitionSettings.FieldDefinitionList.Settings.Count; i++)
            {
                var original = typeDefinitionSettings.FieldDefinitionList.Settings[i];
                typeDefinitionSettings.FieldDefinitionList.Settings[i]
                    = new DatabaseFieldDefinitionSettings
                    {
                        FieldMemo = original.FieldMemo,
                        FieldName = original.FieldName,
                        SpecialSettingDefinition = original.SpecialSettingDefinition,
                        FieldType = itemTypes[i],
                    };
            }

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DBTypeSetFileReader),
                    "タイプ設定リスト"
                )
            );
        }

        #endregion

        #endregion
    }
}
