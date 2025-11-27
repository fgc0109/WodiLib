using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    public static class DBTypeSetFileTestItemGenerator
    {
        public static DBTypeSet GenerateUDB0Data()
        {
            return new DBTypeSet(
                new DBTypeSetSettings
                {
                    TypeDefinition = new DatabaseTypeDefinitionSettings
                    {
                        TypeName = TestItems.DatabaseProjectTypeSettings.Udb_Type0.TypeName,
                        Memo = TestItems.DatabaseProjectTypeSettings.Udb_Type0.Memo,
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
                    },
                }
            );
        }

        public static DBTypeSet GenerateCDB0Data()
        {
            return new DBTypeSet(
                new DBTypeSetSettings
                {
                    TypeDefinition = new DatabaseTypeDefinitionSettings
                    {
                        TypeName = TestItems.DatabaseProjectTypeSettings.Cdb_Type0.TypeName,
                        Memo = TestItems.DatabaseProjectTypeSettings.Cdb_Type0.Memo,
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
                    },
                }
            );
        }

        #region テスト用ファイル出力処理

        /// <summary>テストファイルデータ</summary>
        public static readonly IEnumerable<(string, byte[])> TestFiles = new List<(string, byte[])>
        {
            ("タイプ設定_000_UDB0.dbtypeset", TestResources.UDB0DBTypeSet),
            ("タイプ設定_000_あいうえお.dbtypeset", TestResources.CDB0DBTypeSet),
            ("タイプ設定_002_┣ 主人公行動AI.dbtypeset", TestResources.CDB2DBTypeSet),
            ("タイプ設定_008_状態設定.dbtypeset", TestResources.UDB8DBTypeSet),
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
