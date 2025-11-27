using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    public static class DatabaseSchemaTestItemGenerator
    {
        #region UDB0

        public static DatabaseSchema GenerateUDB0MergedData()
        {
            return new DatabaseSchema(
                new DatabaseSchemaSettings
                {
                    DbKind = DatabaseKind.User,
                    TypeTableList = new DatabaseTypeTableListSettings(
                        new List<IDatabaseTypeTableSettings>
                        {
                            GenerateUDB0Type0Data(),
                            GenerateUDB0Type1Data(),
                            GenerateUDB0Type2Data(),
                            GenerateUDB0Type3Data(),
                        }
                    ),
                }
            );
        }

        private static IDatabaseTypeTableSettings GenerateUDB0Type0Data()
        {
            return new DatabaseTypeTableSettings(
                new List<IDatabaseNamedDataRowSettings>
                {
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type0_Data0
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type0_Data0,
                    },
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type0_Data1
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type0_Data1,
                    },
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type0_Data2
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type0_Data2,
                    },
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type0_Data3
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type0_Data3,
                    },
                }
            )
            {
                TypeName = TestItems.DatabaseProjectTypeSettings.Udb_Type0.TypeName,
                Memo = TestItems.DatabaseProjectTypeSettings.Udb_Type0.Memo,
                DataNamingDefinition = TestItems.DataNamingDefinition.Udb_Type0,
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new List<IDatabaseFieldDefinitionSettings>
                    {
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field0,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field1,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field2,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field3,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field4,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field5,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type0_Field6,
                    }
                ),
            };
        }

        private static IDatabaseTypeTableSettings GenerateUDB0Type1Data()
        {
            return new DatabaseTypeTableSettings(
                new List<IDatabaseNamedDataRowSettings>
                {
                    // Data 0
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type1_Data0
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type1_Data0,
                    },
                }
            )
            {
                TypeName = TestItems.DatabaseProjectTypeSettings.Udb_Type1.TypeName,
                Memo = TestItems.DatabaseProjectTypeSettings.Udb_Type1.Memo,
                DataNamingDefinition = TestItems.DataNamingDefinition.Udb_Type1,
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new List<IDatabaseFieldDefinitionSettings>()
                ),
            };
        }

        private static IDatabaseTypeTableSettings GenerateUDB0Type2Data()
        {
            return new DatabaseTypeTableSettings(
                new List<IDatabaseNamedDataRowSettings>
                {
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type2_Data0
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type2_Data0,
                    },
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type2_Data1
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type2_Data1,
                    },
                }
            )
            {
                TypeName = TestItems.DatabaseProjectTypeSettings.Udb_Type2.TypeName,
                Memo = TestItems.DatabaseProjectTypeSettings.Udb_Type2.Memo,
                DataNamingDefinition = TestItems.DataNamingDefinition.Udb_Type2,
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new List<IDatabaseFieldDefinitionSettings>
                    {
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type2_Field0,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type2_Field1,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type2_Field2,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type2_Field3,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type2_Field4,
                    }
                ),
            };
        }

        private static IDatabaseTypeTableSettings GenerateUDB0Type3Data()
        {
            return new DatabaseTypeTableSettings(
                new List<IDatabaseNamedDataRowSettings>
                {
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Udb_Type3_Data0
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Udb_Type3_Data0,
                    },
                }
            )
            {
                TypeName = TestItems.DatabaseProjectTypeSettings.Udb_Type3.TypeName,
                Memo = TestItems.DatabaseProjectTypeSettings.Udb_Type3.Memo,
                DataNamingDefinition = TestItems.DataNamingDefinition.Udb_Type3,
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new List<IDatabaseFieldDefinitionSettings>
                    {
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type3_Field0,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type3_Field1,
                        TestItems.DatabaseFieldDefinitionSettings.Udb_Type3_Field2,
                    }
                ),
            };
        }

        #endregion

        #region CDB0

        public static DatabaseSchema GenerateCDB0MergedData()
        {
            return new DatabaseSchema(
                new DatabaseSchemaSettings
                {
                    DbKind = DatabaseKind.Changeable,
                    TypeTableList = new DatabaseTypeTableListSettings(
                        new List<IDatabaseTypeTableSettings>
                        {
                            GenerateCDB0Type0Data(),
                            GenerateCDB0Type1Data(),
                        }
                    ),
                }
            );
        }

        private static IDatabaseTypeTableSettings GenerateCDB0Type0Data()
        {
            return new DatabaseTypeTableSettings(
                new List<IDatabaseNamedDataRowSettings>
                {
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Cdb_Type0_Data0
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Cdb_Type0_Data0,
                    },
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Cdb_Type0_Data1
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Cdb_Type0_Data1,
                    },
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Cdb_Type0_Data2
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Cdb_Type0_Data2,
                    },
                }
            )
            {
                TypeName = TestItems.DatabaseProjectTypeSettings.Cdb_Type0.TypeName,
                Memo = TestItems.DatabaseProjectTypeSettings.Cdb_Type0.Memo,
                DataNamingDefinition = TestItems.DataNamingDefinition.Cdb_Type0,
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new List<IDatabaseFieldDefinitionSettings>
                    {
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field0,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field1,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field2,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field3,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field4,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field5,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field6,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field7,
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type0_Field8,
                    }
                ),
            };
        }

        private static IDatabaseTypeTableSettings GenerateCDB0Type1Data()
        {
            return new DatabaseTypeTableSettings(
                new List<IDatabaseNamedDataRowSettings>
                {
                    new DatabaseNamedDataRowSettings(
                        TestItems.DatabaseFieldValueList.Cdb_Type1_Data0
                    )
                    {
                        DataName = TestItems.DatabaseDataName.Cdb_Type1_Data0,
                    },
                }
            )
            {
                TypeName = TestItems.DatabaseProjectTypeSettings.Cdb_Type1.TypeName,
                Memo = TestItems.DatabaseProjectTypeSettings.Cdb_Type1.Memo,
                DataNamingDefinition = TestItems.DataNamingDefinition.Cdb_Type1,
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new List<IDatabaseFieldDefinitionSettings>
                    {
                        TestItems.DatabaseFieldDefinitionSettings.Cdb_Type1_Field0,
                    }
                ),
            };
        }

        #endregion

        #region テスト用ファイル出力処理

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("Database.dat", TestResources.Database0Dat),
            ("CDatabase.dat", TestResources.CDatabase0Dat),
            ("Database.project", TestResources.Database0Project),
            ("CDatabase.project", TestResources.CDatabase0Project),
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
