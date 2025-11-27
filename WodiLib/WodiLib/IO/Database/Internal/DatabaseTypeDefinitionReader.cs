// ========================================
// Project Name : WodiLib
// File Name    : DatabaseTypeDefinitionReader.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.IO
{
    /// <summary>
    ///     DBタイプ設定読み込みクラス
    /// </summary>
    internal static class DatabaseTypeDefinitionReader
    {
        #region Properties

        /// <summary>ロガー</summary>
        private static WodiLibLogger WodiLibLogger { get; } = WodiLibLogger.GetInstance();

        #endregion

        // _/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

        #region Methods

        /// <summary>
        ///     DBタイプ設定を読み込み、返す。
        /// </summary>
        /// <remarks>
        ///     データ名リストを含んでいる場合は、返却タプル内の dataNameList にその一覧を含んで返す。
        ///     読み込むデータにデータ名一覧が含まれているかどうかは <paramref name="hasDataNameList"/> で判断する。
        /// </remarks>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="length">DBタイプ設定数</param>
        /// <param name="hasDataNameList">データ名リスト含有フラグ</param>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルが仕様と異なる場合。
        /// </exception>
        public static ReadResultItem Read(FileReadStatus status, int length, bool hasDataNameList)
        {
            WodiLibLogger.Debug(FileIOMessage.StartCommonRead(typeof(DatabaseTypeDefinitionReader), ""));

            var result = new ReadResultItem(
                new List<DatabaseTypeDefinitionSettings>(),
                new List<DatabaseDataNameListSettings>()
            );
            for (var i = 0; i < length; i++)
            {
                ReadOneTypeDefinition(status, hasDataNameList, result);
            }

            WodiLibLogger.Debug(FileIOMessage.EndCommonRead(typeof(DatabaseTypeDefinitionReader), ""));

            return result;
        }

        #endregion

        #region private

        /// <summary>
        ///     DBタイプ設定一つ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="isReadDataNameList">データ名リスト読み込みフラグ</param>
        /// <param name="readResult">結果格納インスタンス</param>
        /// <exception cref="BinaryFormatterException">
        ///     バイナリデータがファイル仕様と異なる場合。
        /// </exception>
        private static void ReadOneTypeDefinition(
            FileReadStatus status,
            bool isReadDataNameList,
            ReadResultItem readResult
        )
        {
            WodiLibLogger.Debug(FileIOMessage.StartCommonRead(typeof(DatabaseTypeDefinitionReader), "DBタイプ設定"));

            var definitionSettings = new DatabaseTypeDefinitionSettings();
            var dataNameListSettings = new DatabaseDataNameListSettings();

            // DBタイプ名
            ReadTypeName(status, definitionSettings);

            // 項目名
            var itemNames = ReadItemName(status);

            if (isReadDataNameList)
            {
                // データ名
                ReadDataName(status, out dataNameListSettings);
            }

            // メモ
            ReadMemo(status, definitionSettings);

            // 特殊指定
            var specialSettingTypes = ReadItemSpecialSettingType(status);

            // 項目メモ
            var itemMemos = ReadItemMemo(status);

            // 特殊指定文字列パラメータ
            var valueDescriptionLists = ReadSpecialStringValue(status);

            // 特殊指定数値パラメータ
            var valueCaseNumberLists = ReadSpecialNumberValue(status);

            // 初期値
            var initValues = ReadItemInitValue(status);

            // 特殊指定セット
            SetFieldDefinition(
                definitionSettings,
                specialSettingTypes,
                itemNames,
                itemMemos,
                valueDescriptionLists,
                valueCaseNumberLists,
                initValues
            );

            WodiLibLogger.Debug(FileIOMessage.EndCommonRead(typeof(DatabaseTypeDefinitionReader), "DBタイプ設定"));

            readResult.TypeDefinitionSettingsList.Add(definitionSettings);
            readResult.DataNameListSettingsList.Add(dataNameListSettings);
        }

        /// <summary>
        ///     タイプ名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="settings">結果格納インスタンス</param>
        private static void ReadTypeName(FileReadStatus status, DatabaseTypeDefinitionSettings settings)
        {
            var typeName = status.ReadString();
            status.AddOffset(typeName.ByteLength);

            settings.TypeName = typeName.String;

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "タイプ名",
                    settings.TypeName
                )
            );
        }

        /// <summary>
        ///     項目名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>項目名リスト</returns>
        private static List<FieldName> ReadItemName(FileReadStatus status)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "項目数",
                    length
                )
            );

            var result = new List<FieldName>();

            for (var i = 0; i < length; i++)
            {
                var name = status.ReadString();
                status.AddOffset(name.ByteLength);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseTypeDefinitionReader),
                        $"  項目名{i,2}",
                        name.String
                    )
                );

                result.Add(name.String);
            }

            return result;
        }

        /// <summary>
        ///     データ名
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="settings">結果格納インスタンス</param>
        private static void ReadDataName(FileReadStatus status, out DatabaseDataNameListSettings settings)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "データ数",
                    length
                )
            );

            var dataNameList = new List<DataName>();

            for (var i = 0; i < length; i++)
            {
                var name = status.ReadString();
                status.AddOffset(name.ByteLength);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseTypeDefinitionReader),
                        $"  データ名{i,4}",
                        name.String
                    )
                );

                dataNameList.Add(name.ToString());
            }

            settings = new DatabaseDataNameListSettings(dataNameList);
        }

        /// <summary>
        ///     メモ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <param name="settings">結果格納インスタンス</param>
        private static void ReadMemo(FileReadStatus status, DatabaseTypeDefinitionSettings settings)
        {
            var memo = status.ReadString();
            status.AddOffset(memo.ByteLength);

            settings.Memo = memo.String;

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "メモ",
                    memo.String
                )
            );
        }

        /// <summary>
        ///     項目特殊指定
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>項目項目特殊指定リスト</returns>
        private static List<DatabaseFieldSpecialSettingType> ReadItemSpecialSettingType(FileReadStatus status)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "項目特殊指定数",
                    length
                )
            );

            var result = new List<DatabaseFieldSpecialSettingType>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadByte();
                status.IncreaseByteOffset();

                var type = DatabaseFieldSpecialSettingType.FromByte(value);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseTypeDefinitionReader),
                        $"  項目特殊指定{i,2}",
                        type
                    )
                );

                result.Add(type);
            }

            return result;
        }

        /// <summary>
        ///     項目メモ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>項目名リスト</returns>
        private static List<FieldMemo> ReadItemMemo(FileReadStatus status)
        {
            var length = status.ReadInt();
            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "項目メモ数",
                    length
                )
            );

            var result = new List<FieldMemo>();

            for (var i = 0; i < length; i++)
            {
                var value = status.ReadString();
                status.AddOffset(value.ByteLength);

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseTypeDefinitionReader),
                        $"  項目メモ{i,2}",
                        value.String
                    )
                );

                result.Add(value.String);
            }

            return result;
        }

        /// <summary>
        ///     特殊指定文字列パラメータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>特殊指定文字列パラメータリスト</returns>
        private static List<List<DatabaseValueCaseDescription>> ReadSpecialStringValue(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "特殊指定文字列パラメータ数",
                    length
                )
            );

            var result = new List<List<DatabaseValueCaseDescription>>();

            for (var i = 0; i < length; i++)
            {
                var descriptionLength = status.ReadInt();
                status.IncreaseIntOffset();

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseTypeDefinitionReader),
                        $"  項目{i,2}のパラメータ数",
                        descriptionLength
                    )
                );

                var paramList = new List<DatabaseValueCaseDescription>();
                for (var j = 0; j < descriptionLength; j++)
                {
                    var value = status.ReadString();
                    status.AddOffset(value.ByteLength);

                    WodiLibLogger.Debug(
                        FileIOMessage.SuccessRead(
                            typeof(DatabaseTypeDefinitionReader),
                            $"    パラメータ{j,2}",
                            value.String
                        )
                    );

                    paramList.Add(value.String);
                }

                result.Add(paramList);
            }

            return result;
        }

        /// <summary>
        ///     特殊指定数値パラメータ
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>特殊指定数値パラメータリスト</returns>
        private static List<List<DatabaseValueCaseNumber>> ReadSpecialNumberValue(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "特殊指定数値パラメータ数",
                    length
                )
            );

            var result = new List<List<DatabaseValueCaseNumber>>();

            for (var i = 0; i < length; i++)
            {
                var descriptionLength = status.ReadInt();
                status.IncreaseIntOffset();

                WodiLibLogger.Debug(
                    FileIOMessage.SuccessRead(
                        typeof(DatabaseTypeDefinitionReader),
                        $"  項目{i,2}のパラメータ数",
                        descriptionLength
                    )
                );

                var paramList = new List<DatabaseValueCaseNumber>();
                for (var j = 0; j < descriptionLength; j++)
                {
                    var value = status.ReadInt();
                    status.IncreaseIntOffset();

                    WodiLibLogger.Debug(
                        FileIOMessage.SuccessRead(
                            typeof(DatabaseTypeDefinitionReader),
                            $"    パラメータ{j,2}",
                            value
                        )
                    );

                    paramList.Add(value);
                }

                result.Add(paramList);
            }

            return result;
        }

        /// <summary>
        ///     初期値
        /// </summary>
        /// <param name="status">読み込み経過状態</param>
        /// <returns>初期値リスト</returns>
        private static List<DatabaseValueInt> ReadItemInitValue(FileReadStatus status)
        {
            var length = status.ReadInt();

            status.IncreaseIntOffset();

            WodiLibLogger.Debug(
                FileIOMessage.SuccessRead(
                    typeof(DatabaseTypeDefinitionReader),
                    "項目初期値数",
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
                        typeof(DatabaseTypeDefinitionReader),
                        $"  初期値{i,2}",
                        value
                    )
                );

                result.Add(value);
            }

            return result;
        }

        /// <summary>
        ///     DB項目設定セット
        /// </summary>
        /// <param name="setting">結果格納インスタンス</param>
        /// <param name="specialSettingTypes">特殊指定種別リスト</param>
        /// <param name="itemNames">項目名リスト</param>
        /// <param name="itemMemos">項目メモリスト</param>
        /// <param name="descriptionLists">特殊指定選択肢文字列リスト</param>
        /// <param name="caseNumberLists">特殊指定選択肢数値リスト</param>
        /// <param name="initValues">初期値リスト</param>
        /// <exception cref="BinaryFormatterException">
        ///     ファイルフォーマットが不正の場合。
        /// </exception>
        private static void SetFieldDefinition(
            DatabaseTypeDefinitionSettings setting,
            IReadOnlyList<DatabaseFieldSpecialSettingType> specialSettingTypes,
            IReadOnlyList<FieldName> itemNames,
            IReadOnlyList<FieldMemo> itemMemos,
            IReadOnlyList<IReadOnlyList<DatabaseValueCaseDescription>> descriptionLists,
            IReadOnlyList<List<DatabaseValueCaseNumber>> caseNumberLists,
            IReadOnlyList<DatabaseValueInt> initValues
        )
        {
            // 項目名、項目メモ、文字列パラメータ、数値パラメータ、初期値の長さが一致する必要がある
            var itemNamesCount = itemNames.Count;
            var itemMemosCount = itemMemos.Count;
            var descriptionListsCount = descriptionLists.Count;
            var caseNumberListsCount = caseNumberLists.Count;
            var initValuesCount = initValues.Count;

            if (itemNamesCount != itemMemosCount
                || itemNamesCount != descriptionListsCount
                || itemNamesCount != caseNumberListsCount
                || itemNamesCount != initValuesCount)
                throw new BinaryFormatterException(
                    "項目名、項目メモ、文字列パラメータ、数値パラメータ、初期値の要素数が一致しません。（"
                    + $"項目名数：{itemNamesCount}, 項目メモ数：{itemMemosCount},"
                    + $"文字列パラメータ数：{descriptionListsCount}, 数値パラメータ数：{caseNumberListsCount}"
                    + $"初期値数：{initValuesCount}）"
                );

            // 特殊指定数が項目数より少ない場合は不正
            var specialSettingTypesCount = specialSettingTypes.Count;

            if (specialSettingTypesCount < itemNamesCount)
                throw new BinaryFormatterException(
                    $"特殊指定種別の要素数が不正です。（要素数：{specialSettingTypesCount}）"
                );

            var fieldDefinitions = new List<IDatabaseFieldDefinitionSettings>();
            for (var i = 0; i < itemNamesCount; i++)
            {
                var thisDescriptions = descriptionLists[i];
                var thisCaseNumbers = caseNumberLists[i];
                var initValue = initValues[i];

                var thisItemSettingType = specialSettingTypes[i];

                DatabaseFieldSpecialSettingDefinitionSettings specialSettingDefinitionSettings;
                try
                {
                    specialSettingDefinitionSettings = MakeSpecialSettingDefinitionSettings(
                        thisItemSettingType,
                        thisCaseNumbers,
                        thisDescriptions,
                        initValue
                    );
                }
                catch (Exception ex)
                {
                    throw new BinaryFormatterException(
                        $"項目{i,2}の特殊指定タイプと特殊指定パラメータが一致しません。" + "詳細はInnerExceptionを確認してください。",
                        ex
                    );
                }

                var thisItemName = itemNames[i];

                var fieldDefinition = new DatabaseFieldDefinitionSettings
                {
                    FieldName = thisItemName,
                    FieldMemo = itemMemos[i],
                    SpecialSettingDefinition = specialSettingDefinitionSettings,
                };

                fieldDefinitions.Add(fieldDefinition);
            }

            setting.FieldDefinitionList = new DatabaseFieldDefinitionListSettings(fieldDefinitions);
        }

        /// <summary>
        ///     選択肢一覧を生成する。
        /// </summary>
        /// <param name="type">特殊指定タイプ</param>
        /// <param name="numbers">選択肢番号リスト</param>
        /// <param name="descriptions">選択肢文字列リスト</param>
        /// <param name="initValue">初期値</param>
        /// <returns>選択肢リスト</returns>
        /// <exception cref="BinaryFormatterException">
        ///     選択肢番号リストまたは文字列リストが不正の場合。
        /// </exception>
        private static DatabaseFieldSpecialSettingDefinitionSettings MakeSpecialSettingDefinitionSettings(
            DatabaseFieldSpecialSettingType type,
            IReadOnlyList<DatabaseValueCaseNumber> numbers,
            IReadOnlyList<DatabaseValueCaseDescription> descriptions,
            DatabaseFieldValue initValue
        )
        {
            if (type == DatabaseFieldSpecialSettingType.Normal)
            {
                return MakeSpecialSettingDefinitionSettingsNormal(numbers, descriptions, initValue);
            }

            if (type == DatabaseFieldSpecialSettingType.LoadFile)
            {
                return MakeSpecialSettingDefinitionSettingsLoadFile(numbers, descriptions, initValue);
            }

            if (type == DatabaseFieldSpecialSettingType.ReferDatabase)
            {
                return MakeSpecialSettingDefinitionSettingsReferDatabase(numbers, descriptions, initValue);
            }

            if (type == DatabaseFieldSpecialSettingType.Manual)
            {
                return MakeSpecialSettingDefinitionSettingsManual(numbers, descriptions, initValue);
            }

            // 通常ここには来ない
            throw new InvalidOperationException(
                "定義されていない特殊指定タイプです。"
            );
        }

        private static DatabaseFieldSpecialSettingDefinitionSettings MakeSpecialSettingDefinitionSettingsNormal(
            IReadOnlyList<DatabaseValueCaseNumber> numbers,
            IReadOnlyList<DatabaseValueCaseDescription> descriptions,
            DatabaseFieldValue initValue
        )
        {
            var type = DatabaseFieldSpecialSettingType.Normal;

            if (numbers.Count > 0)
            {
                WodiLibLogger.Warning($"特殊設定タイプ：{type}：指定されている数値パラメータが無視されます。");
                for (var i = 0; i < numbers.Count; i++)
                {
                    WodiLibLogger.Warning($"  数値パラメータ{i,2}：{numbers[i]}");
                }
            }

            if (descriptions.Count > 0)
            {
                WodiLibLogger.Warning($"特殊設定タイプ：{type}：指定されている文字列パラメータが無視されます。");
                for (var i = 0; i < descriptions.Count; i++)
                {
                    WodiLibLogger.Warning($"  文字列パラメータ{i,2}：{descriptions[i]}");
                }
            }

            var settings = new DatabaseFieldSpecialSettingDefinitionNormalSettings
            {
                InitValue = initValue,
            };
            return new DatabaseFieldSpecialSettingDefinitionSettings(settings);
        }

        private static DatabaseFieldSpecialSettingDefinitionSettings MakeSpecialSettingDefinitionSettingsLoadFile(
            IReadOnlyList<DatabaseValueCaseNumber> numbers,
            IReadOnlyList<DatabaseValueCaseDescription> descriptions,
            DatabaseFieldValue initValue
        )
        {
            var type = DatabaseFieldSpecialSettingType.LoadFile;

            if (numbers.Count < 1)
            {
                throw new BinaryFormatterException(
                    $"特殊設定タイプ：{type}： 数値パラメータが不足しています。（パラメータ数：{numbers.Count}）"
                );
            }

            if (descriptions.Count < 1)
            {
                throw new BinaryFormatterException(
                    $"特殊設定タイプ：{type}： 文字列パラメータが不足しています。（パラメータ数：{descriptions.Count}）"
                );
            }

            if (numbers.Count > 1)
            {
                WodiLibLogger.Warning($"特殊設定タイプ：{type}：指定されている数値パラメータが無視されます。");
                for (var i = 1; i < numbers.Count; i++)
                {
                    WodiLibLogger.Warning($"  数値パラメータ{i,2}：{numbers[i]}");
                }
            }

            if (descriptions.Count > 1)
            {
                WodiLibLogger.Warning($"特殊設定タイプ：{type}：指定されている文字列パラメータが無視されます。");
                for (var i = 1; i < descriptions.Count; i++)
                {
                    WodiLibLogger.Warning($"  文字列パラメータ{i,2}：{descriptions[i]}");
                }
            }

            var settings = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
            {
                FolderName = descriptions[0].RawValue,
                IsOmitFolderName = numbers[0] == 1,
                InitValue = initValue,
            };
            return new DatabaseFieldSpecialSettingDefinitionSettings(settings);
        }

        private static DatabaseFieldSpecialSettingDefinitionSettings
            MakeSpecialSettingDefinitionSettingsReferDatabase(
                IReadOnlyList<DatabaseValueCaseNumber> numbers,
                IReadOnlyList<DatabaseValueCaseDescription> descriptions,
                DatabaseFieldValue initValue
            )
        {
            var type = DatabaseFieldSpecialSettingType.ReferDatabase;

            if (numbers.Count < 3)
            {
                throw new BinaryFormatterException(
                    $"特殊設定タイプ：{type}： 数値パラメータが不足しています。（パラメータ数：{numbers.Count}）"
                );
            }

            var settings = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                DatabaseReferKind = DatabaseReferType.FromCode(numbers[0]),
                DatabaseDbTypeId = new TypeId(numbers[1]),
                IsUseAdditionalItems = numbers[2] == 1,
                InitValue = initValue,
            };

            if (!settings.IsUseAdditionalItems)
            {
                return new DatabaseFieldSpecialSettingDefinitionSettings(settings);
            }

            if (descriptions.Count < 3)
            {
                throw new BinaryFormatterException(
                    $"特殊設定タイプ：{type}： 文字列パラメータが不足しています。（パラメータ数：{descriptions.Count}）"
                );
            }


            if (descriptions.Count > 3)
            {
                WodiLibLogger.Warning($"特殊設定タイプ：{type}：指定されている文字列パラメータが無視されます。");
                for (var i = 3; i < descriptions.Count; i++)
                {
                    WodiLibLogger.Warning($"  文字列パラメータ{i,2}：{descriptions[i]}");
                }
            }

            settings.AdditionalCase1 = descriptions[0].RawValue;
            settings.AdditionalCase2 = descriptions[1].RawValue;
            settings.AdditionalCase3 = descriptions[2].RawValue;

            return new DatabaseFieldSpecialSettingDefinitionSettings(settings);
        }

        private static DatabaseFieldSpecialSettingDefinitionSettings MakeSpecialSettingDefinitionSettingsManual(
            IReadOnlyList<DatabaseValueCaseNumber> numbers,
            IReadOnlyList<DatabaseValueCaseDescription> descriptions,
            DatabaseFieldValue initValue
        )
        {
            var type = DatabaseFieldSpecialSettingType.Manual;

            // 選択肢番号数と文字列数が一致しない場合は不正
            if (numbers.Count != descriptions.Count)
                throw new ArgumentException(
                    $"特殊設定タイプ：{type}： 文字列パラメータ数と数値パラメータ数が一致しません。"
                    + $"（文字列パラメータ数：{descriptions.Count}、数値パラメータ数：{numbers.Count}）"
                );

            var cases = numbers.Zip(descriptions)
                .Select(zip => new DatabaseValueCase(zip.Item1, zip.Item2))
                .ToList();
            var settings = new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                SpecialCases = new DatabaseValueCaseListSettings(cases),
                InitValue = initValue,
            };
            return new DatabaseFieldSpecialSettingDefinitionSettings(settings);
        }

        #endregion

        #region Records

        /// <summary>
        ///     <see cref="DatabaseTypeDefinitionReader.Read"/> メソッドが返すDBタイプ設定リスト一つ分のデータ型
        /// </summary>
        /// <param name="TypeDefinitionSettingsList">タイプ設定</param>
        /// <param name="DataNameListSettingsList">データ名リスト設定</param>
        public record ReadResultItem(
            List<DatabaseTypeDefinitionSettings> TypeDefinitionSettingsList,
            List<DatabaseDataNameListSettings> DataNameListSettingsList
        );

        #endregion
    }
}
