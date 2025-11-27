// ========================================
// Project Name : WodiLib
// File Name    : DatabaseDataTableWithDataNamingReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBテーブルデータ &amp; データ名の設定方法 読み込みクラス
    /// </summary>
    internal static class DatabaseDataTableWithDataNamingReader
    {
        #region Constants

        /// <summary>ファイルヘッダ</summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static readonly byte[] Header =
        {
            0xFE, 0xFF, 0xFF, 0xFF,
        };

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Properties

        /// <summary>ロガー</summary>
        private static WodiLibLogger WodiLibLogger { get; } = WodiLibLogger.GetInstance();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        #region public

        /// <summary>
        ///     DBテーブルデータ &amp; データ名の設定方法を読み込み、返す。
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">DBデータ数</param>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルが仕様と異なる場合。
        /// </exception>
        public static DatabaseDataTableWithDataNamingDefinitionListSettings Read(FileReadStatus status, int length)
        {
            WodiLibLogger.Debug(FileIOMessage.StartCommonRead(typeof(DatabaseDataTableWithDataNamingReader), ""));

            var list = new List<IDatabaseDataTableWithDataNamingDefinitionSettings>();
            for (var i = 0; i < length; i++)
            {
                ReadOneDBTypeSetting(status, list);
            }

            WodiLibLogger.Debug(FileIOMessage.EndCommonRead(typeof(DatabaseDataTableWithDataNamingReader), ""));

            return new DatabaseDataTableWithDataNamingDefinitionListSettings(list);
        }

        #endregion

        #region private

        /// <summary>
        ///     DBタイプ設定一つ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <exception cref="BinaryFormatterException">
        ///     バイナリデータがファイル仕様と異なる場合。
        /// </exception>
        private static void ReadOneDBTypeSetting(
            FileReadStatus status,
            ICollection<IDatabaseDataTableWithDataNamingDefinitionSettings> result
        )
        {
            WodiLibLogger.Debug(
                FileIOMessage.StartCommonRead(typeof(DatabaseDataTableWithDataNamingReader), "DBタイプ設定")
            );

            var settings = new DatabaseDataTableWithDataNamingDefinitionSettings();

            // ヘッダ
            ReadHeader(status);

            // データIDの設定方法
            ReadDataSettingType(status, settings);

            // 設定種別 & 種別順列
            ReadValueType(status, out var types);

            // DBデータ設定値
            ReadDataSettingValue(status, settings, types);

            WodiLibLogger.Debug(FileIOMessage.EndCommonRead(typeof(DatabaseDataTableWithDataNamingReader), "DBタイプ設定"));

            result.Add(settings);
        }

        /// <summary>
        ///     ヘッダ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルヘッダが仕様と異なる場合。
        /// </exception>
        private static void ReadHeader(FileReadStatus status)
        {
            foreach (var b in Header)
            {
                if (status.ReadByte() != b)
                {
                    throw new BinaryFormatterException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{status.Offset}）"
                    );
                }

                status.IncreaseByteOffset();
            }

            WodiLibLogger.Debug($"{nameof(DBDatFileReader)} ヘッダチェックOK");
        }

        /// <summary>
        ///     データIDの設定方法
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="settings">結果格納DTO</param>
        private static void ReadDataSettingType(
            FileReadStatus status,
            DatabaseDataTableWithDataNamingDefinitionSettings settings
        )
        {
            var typeCode = status.ReadInt();
            status.IncreaseIntOffset();

            var dataNamingType = DatabaseDataNamingTypeMapper.FromSettingsValue(typeCode);

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "データID設定方法",
                    dataNamingType
                )
            );

            var definition = BuildDataNameSpecificationDefinitionIfDataNamingTypeIsDesignated(
                dataNamingType,
                typeCode
            );
            var namingDefinition = new DatabaseDataNamingDefinition(dataNamingType, definition);

            settings.DataNamingDefinition = namingDefinition;
        }

        /// <summary>
        ///     設定種別 &amp; 種別順列
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="itemTypes">取得した項目種別リスト格納先</param>
        private static void ReadValueType(FileReadStatus status, out List<DatabaseFieldType> itemTypes)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "項目数",
                    length
                )
            );

            itemTypes = new List<DatabaseFieldType>();

            for (var i = 0; i < length; i++)
            {
                var settingCode = status.ReadInt();
                status.IncreaseIntOffset();

                var itemType = DatabaseFieldTypeMapper.FromSettingsValue(settingCode);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseDataTableWithDataNamingReader),
                        $"  項目{i,2}設定種別",
                        itemType
                    )
                );

                // 種別順位は無視する

                itemTypes.Add(itemType);
            }

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "項目設定種別"
                )
            );
        }

        /// <summary>
        ///     DBデータ設定値
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="settings">結果格納DTO</param>
        /// <param name="itemTypes">項目種別リスト</param>
        private static void ReadDataSettingValue(
            FileReadStatus status,
            DatabaseDataTableWithDataNamingDefinitionSettings settings,
            IEnumerable<DatabaseFieldType> itemTypes
        )
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "データ数",
                    length
                )
            );

            var itemTypeArr = itemTypes.ToArray();

            var numberItemCount = itemTypeArr.Count(x => x == DatabaseFieldType.Int);
            var stringItemCount = itemTypeArr.Count(x => x == DatabaseFieldType.String);

            var valuesList = new List<IDatabaseDataRowSettings>();

            for (var i = 0; i < length; i++)
            {
                ReadOneDataSettingValue(status, valuesList, itemTypeArr, numberItemCount, stringItemCount);
            }

            settings.DataTable = new DatabaseDataTableSettings(valuesList);
        }

        /// <summary>
        ///     DBデータ設定値ひとつ分
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <param name="itemTypes">項目種別リスト</param>
        /// <param name="numberItemCount">数値項目数</param>
        /// <param name="stringItemCount">文字列項目数</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static void ReadOneDataSettingValue(
            FileReadStatus status,
            ICollection<IDatabaseDataRowSettings> result,
            IEnumerable<DatabaseFieldType> itemTypes,
            int numberItemCount,
            int stringItemCount
        )
        {
            WodiLibLogger.Debug(
                FileIOMessage.StartCommonRead(typeof(DatabaseDataTableWithDataNamingReader), "データ設定値")
            );

            var numberItems = new List<DatabaseValueInt>();
            var stringItems = new List<DatabaseValueString>();

            for (var i = 0; i < numberItemCount; i++)
            {
                var numberItem = status.ReadInt();
                status.IncreaseIntOffset();

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseDataTableWithDataNamingReader),
                        $"  数値項目{i,2}",
                        numberItem
                    )
                );

                numberItems.Add(numberItem);
            }

            for (var i = 0; i < stringItemCount; i++)
            {
                var stringItem = status.ReadString();
                status.AddOffset(stringItem.ByteLength);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseDataTableWithDataNamingReader),
                        $"  文字列項目{i,2}",
                        stringItem
                    )
                );

                stringItems.Add(stringItem.String);
            }

            var valueList = new List<DatabaseFieldValue>();

            var numberIndex = 0;
            var stringIndex = 0;
            foreach (var itemType in itemTypes)
            {
                if (itemType == DatabaseFieldType.Int)
                {
                    valueList.Add(numberItems[numberIndex]);
                    numberIndex++;
                }
                else if (itemType == DatabaseFieldType.String)
                {
                    valueList.Add(stringItems[stringIndex]!);
                    stringIndex++;
                }
                else
                {
                    // 通常ここへは来ない
                    throw new BinaryFormatterException("未対応のデータ種別です。");
                }
            }

            var rowSettings = new DatabaseDataRowSettings(valueList);
            result.Add(rowSettings);

            WodiLibLogger.Debug(
                FileIOMessage.EndCommonRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "データ設定値"
                )
            );
        }

        /// <summary>
        ///     データ名の設定方法が「指定DBの指定タイプ」の場合、DB種別及びタイプIDを表すコード値よりデータ設定方法追加情報インスタンスを生成する。
        /// </summary>
        /// <param name="dataNamingType">データ名設定方法</param>
        /// <param name="dbTypeCode">DB種別およびタイプIDを表すコード値</param>
        private static DataNameSpecificationDefinition?
            BuildDataNameSpecificationDefinitionIfDataNamingTypeIsDesignated(
                DatabaseDataNamingType dataNamingType,
                int dbTypeCode
            )
        {
            if (dataNamingType != DatabaseDataNamingType.DesignatedType)
            {
                return null;
            }

            var databaseKind = DatabaseKindFromSettingTypeCode(dbTypeCode);

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "DB種別",
                    databaseKind
                )
            );

            var typeId = TypeIdFromSettingTypeCode(dbTypeCode);

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseDataTableWithDataNamingReader),
                    "タイプID",
                    typeId
                )
            );

            return new DataNameSpecificationDefinition
            {
                TypeId = typeId,
                DatabaseKind = databaseKind,
            };
        }

        /// <summary>
        ///     データIDの設定方法コードからDB種別を取得する。
        /// </summary>
        /// <param name="code">設定種別コード</param>
        /// <returns>DB種別</returns>
        private static DatabaseKind DatabaseKindFromSettingTypeCode(int code)
        {
            var dbKindCode = (byte)code.SubInt(4, 1);
            return DatabaseKindMapper.FromDBDataSettingTypeCode(dbKindCode);
        }

        /// <summary>
        ///     データIDの設定方法コードからタイプIDを取得する。
        /// </summary>
        /// <param name="code">設定種別コード</param>
        /// <returns>タイプID</returns>
        private static TypeId TypeIdFromSettingTypeCode(int code)
        {
            return code.SubInt(0, 4);
        }

        #endregion

        #endregion
    }
}
