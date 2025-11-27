using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    public static class DBDatFileTestItemGenerator
    {
        #region CreateDatabaseDat

        public static DBDat GenerateDataBaseDat0Data()
        {
            return new DBDat(
                new DBDatSettings
                {
                    DbKind = DatabaseKind.User,
                    DataTableDefinitionList = new DatabaseDataTableWithDataNamingDefinitionListSettings(
                        new List<IDatabaseDataTableWithDataNamingDefinitionSettings>
                        {
                            GenerateData0Type00Setting(),
                            GenerateData0Type01Setting(),
                            GenerateData0Type02Setting(),
                            GenerateData0Type03Setting(),
                        }
                    ),
                }
            );
        }

        private static IDatabaseDataTableWithDataNamingDefinitionSettings GenerateData0Type00Setting()
        {
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.FirstStringData),
                DataTable = new DatabaseDataTableSettings(
                    new List<IDatabaseDataRowSettings>
                    {
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type0_Data0
                        ),
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type0_Data1
                        ),
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type0_Data2
                        ),
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type0_Data3
                        ),
                    }
                ),
            };
        }

        private static IDatabaseDataTableWithDataNamingDefinitionSettings GenerateData0Type01Setting()
        {
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual),
                DataTable = new DatabaseDataTableSettings(
                    new List<IDatabaseDataRowSettings>
                    {
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type1_Data0
                        ),
                    }
                ),
            };
        }

        private static IDatabaseDataTableWithDataNamingDefinitionSettings GenerateData0Type02Setting()
        {
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataNamingDefinition = new DatabaseDataNamingDefinition(
                    DatabaseDataNamingType.DesignatedType,
                    DatabaseKind.User,
                    4
                ),
                DataTable = new DatabaseDataTableSettings(
                    new List<IDatabaseDataRowSettings>
                    {
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type2_Data0
                        ),
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type2_Data1
                        ),
                    }
                ),
            };
        }

        private static IDatabaseDataTableWithDataNamingDefinitionSettings GenerateData0Type03Setting()
        {
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.EqualBefore),
                DataTable = new DatabaseDataTableSettings(
                    new List<IDatabaseDataRowSettings>
                    {
                        new DatabaseDataRowSettings(
                            TestItems.DatabaseFieldValueList.Udb_Type3_Data0
                        ),
                    }
                ),
            };
        }

        #endregion

        #region CreateCDatabaseDat

        public static DBDat GenerateCDatabaseData0Data()
        {
            return new DBDat(
                new DBDatSettings
                {
                    DbKind = DatabaseKind.Changeable,
                    DataTableDefinitionList = new DatabaseDataTableWithDataNamingDefinitionListSettings(
                        new List<IDatabaseDataTableWithDataNamingDefinitionSettings>
                        {
                            // CDB0
                            new DatabaseDataTableWithDataNamingDefinitionSettings
                            {
                                DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.Manual),
                                DataTable = new DatabaseDataTableSettings(
                                    new List<IDatabaseDataRowSettings>
                                    {
                                        new DatabaseDataRowSettings(
                                            TestItems.DatabaseFieldValueList.Cdb_Type0_Data0
                                        ),
                                        new DatabaseDataRowSettings(
                                            TestItems.DatabaseFieldValueList.Cdb_Type0_Data1
                                        ),
                                        new DatabaseDataRowSettings(
                                            TestItems.DatabaseFieldValueList.Cdb_Type0_Data2
                                        ),
                                    }
                                ),
                            },
                            // CDB1
                            new DatabaseDataTableWithDataNamingDefinitionSettings
                            {
                                DataNamingDefinition = new DatabaseDataNamingDefinition(
                                    DatabaseDataNamingType.DesignatedType,
                                    DatabaseKind.Changeable,
                                    4
                                ),
                                DataTable = new DatabaseDataTableSettings(
                                    new List<IDatabaseDataRowSettings>
                                    {
                                        new DatabaseDataRowSettings(
                                            TestItems.DatabaseFieldValueList.Cdb_Type1_Data0
                                        ),
                                    }
                                ),
                            },
                        }
                    ),
                }
            );
        }

        #endregion

        #region テスト用ファイル出力処理

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("Database0.dat", TestResources.Database0Dat),
            ("CDatabase0.dat", TestResources.CDatabase0Dat),
            ("Database1.dat", TestResources.Database1Dat),
            ("CDatabase1.dat", TestResources.CDatabase1Dat),
            ("SysDatabase1.dat", TestResources.SysDatabase1Dat),
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
