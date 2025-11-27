using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    public static class DBTypeFileTestItemGenerator
    {
        public static DBType GenerateUDB0DBType()
        {
            var result = new DBType(
                new DBTypeSettings
                {
                    TypeMetadataTable = new DatabaseTypeMetadataTableSettings(
                        new List<IDatabaseNamedDataRowSettings>
                        {
                            // データ0
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Udb_Type0_Data0
                            )
                            {
                                DataName = "文字列",
                            },
                            // データ1
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Udb_Type0_Data1
                            )
                            {
                                DataName = "7",
                            },
                            // データ2
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Udb_Type0_Data2
                            )
                            {
                                DataName = "うでぃた",
                            },
                            // データ3
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Udb_Type0_Data3
                            )
                            {
                                DataName = "",
                            },
                        }
                    )
                    {
                        TypeName = TestItems.DatabaseProjectTypeSettings.Udb_Type0.TypeName,
                        Memo = TestItems.DatabaseProjectTypeSettings.Udb_Type0.Memo,
                        DataNamingDefinition = TestItems.DataNamingDefinition.Udb_Type0,
                        FieldMetadataList = new DatabaseFieldMetadataListSettings(
                            new List<IDatabaseFieldMetadataSettings>
                            {
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field0.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field1.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field2.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field3.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field4.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field5.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field6.TransformMetadataSettings(),
                            }
                        ),
                    },
                }
            );
            return result;
        }

        public static DBType GenerateCDB0DBType()
        {
            var result = new DBType(
                new DBTypeSettings
                {
                    TypeMetadataTable = new DatabaseTypeMetadataTableSettings(
                        new List<IDatabaseNamedDataRowSettings>
                        {
                            // データ0
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Cdb_Type0_Data0
                            )
                            {
                                DataName = "a",
                            },
                            // データ1
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Cdb_Type0_Data1
                            )
                            {
                                DataName = "b",
                            },
                            // データ2
                            new DatabaseNamedDataRowSettings(
                                TestItems.DatabaseFieldValueList.Cdb_Type0_Data2
                            )
                            {
                                DataName = "c",
                            },
                        }
                    )
                    {
                        TypeName = TestItems.DatabaseProjectTypeSettings.Cdb_Type0.TypeName,
                        Memo = TestItems.DatabaseProjectTypeSettings.Cdb_Type0.Memo,
                        DataNamingDefinition = TestItems.DataNamingDefinition.Cdb_Type0,
                        FieldMetadataList = new DatabaseFieldMetadataListSettings(
                            new List<IDatabaseFieldMetadataSettings>
                            {
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field0.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field1.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field2.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field3.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field4.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field5.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field6.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field7.TransformMetadataSettings(),
                                TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field8.TransformMetadataSettings(),
                            }
                        ),
                    },
                }
            );
            return result;
        }

        #region テスト用ファイル出力処理

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("タイプ(データ含む)_000_UDB0.dbtype", TestResources.UDB0DBType),
            ("タイプ(データ含む)_000_あいうえお.dbtype", TestResources.CDB0DBType),
            ("タイプ(データ含む)_002_┣ 主人公行動AI.dbtype", TestResources.CDB2DBType),
            ("タイプ(データ含む)_008_状態設定.dbtype", TestResources.UDB8DBType),
        };

        /// <summary>
        ///     ファイルを tmp フォルダに出力する。
        /// </summary>
        public static void OutputFile()
        {
            TestDirHelper.OutputFiles(TestFiles);
        }

        /// <summary>
        ///     ファイルを削除する。
        /// </summary>
        public static void DeleteFile()
        {
            TestDirHelper.DeleteFiles(TestFiles);
        }

        #endregion
    }
}
