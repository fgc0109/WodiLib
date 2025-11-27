using System.Collections.Generic;
using WodiLib.Database;

namespace WodiLib.Test.IO.TestData.Database
{
    internal static partial class TestItems
    {
        internal static class DatabaseFieldDefinitionSettings
        {
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field0;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field1;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field2;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field3;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field4;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field5;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type0_Field6;

            public static readonly IDatabaseFieldDefinitionSettings Udb_Type2_Field0;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type2_Field1;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type2_Field2;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type2_Field3;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type2_Field4;

            public static readonly IDatabaseFieldDefinitionSettings Udb_Type3_Field0;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type3_Field1;
            public static readonly IDatabaseFieldDefinitionSettings Udb_Type3_Field2;

            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field0;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field1;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field2;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field3;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field4;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field5;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field6;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field7;
            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type0_Field8;

            public static readonly IDatabaseFieldDefinitionSettings Cdb_Type1_Field0;

            static DatabaseFieldDefinitionSettings()
            {
                Udb_Type0_Field0 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "設定項目0",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = 0,
                    },
                    FieldMemo = "",
                };

                Udb_Type0_Field1 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "文字列項目",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };

                Udb_Type0_Field2 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "ファイル名設定1",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        IsOmitFolderName = false,
                        FolderName = "MapChip",
                    },
                    FieldMemo = "",
                };

                Udb_Type0_Field3 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "ファイル名設定2",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        IsOmitFolderName = true,
                        FolderName = "MapData",
                    },
                    FieldMemo = "",
                };

                Udb_Type0_Field4 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "DBから",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition =
                        new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                        {
                            DatabaseReferKind = DatabaseReferType.User,
                            DatabaseDbTypeId = 4,
                            InitValue = 23,
                            IsUseAdditionalItems = false,
                        },
                    FieldMemo = "",
                };

                Udb_Type0_Field5 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "DBから　その2",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition =
                        new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                        {
                            DatabaseReferKind = DatabaseReferType.Changeable,
                            DatabaseDbTypeId = 1,
                            InitValue = 322,
                            IsUseAdditionalItems = true,
                            AdditionalCase1 = "Minus1",
                            AdditionalCase2 = "Minus2",
                            AdditionalCase3 = "Minus3",
                        },
                    FieldMemo = "",
                };

                Udb_Type0_Field6 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "手動生成",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionManualSettings
                    {
                        InitValue = 0,
                        SpecialCases = new DatabaseValueCaseListSettings(
                            new List<DatabaseValueCase>
                            {
                                new(0, "Zero"),
                                new(3, "さん"),
                                new(10, "１０"),
                                new(9, "nine"),
                            }
                        ),
                    },
                    FieldMemo = "",
                };

                Udb_Type2_Field0 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "文字列項目",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Udb_Type2_Field1 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "文字列項目2",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Udb_Type2_Field2 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "文字列項目3",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Udb_Type2_Field3 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "文字列項目4",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Udb_Type2_Field4 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "文字列項目5",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };

                Udb_Type3_Field0 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Udb_Type3_Field1 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Udb_Type3_Field2 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "項目",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };

                Cdb_Type0_Field0 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "ItemName",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = 255,
                    },
                    FieldMemo = "",
                };
                Cdb_Type0_Field1 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "Field2",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Cdb_Type0_Field2 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
                Cdb_Type0_Field3 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "FilePath",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        FolderName = "CharaChip",
                        IsOmitFolderName = false,
                    },
                    FieldMemo = "",
                };
                Cdb_Type0_Field4 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = 321,
                    },
                    FieldMemo = "",
                };
                Cdb_Type0_Field5 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition =
                        new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                        {
                            DatabaseReferKind = DatabaseReferType.System,
                            DatabaseDbTypeId = 0,
                            InitValue = 65535,
                            IsUseAdditionalItems = false,
                        },
                    FieldMemo = "",
                };
                Cdb_Type0_Field6 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = 255,
                    },
                    FieldMemo = "",
                };
                Cdb_Type0_Field7 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "Case",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionManualSettings
                    {
                        InitValue = 0,
                        SpecialCases = new DatabaseValueCaseListSettings(
                            new List<DatabaseValueCase>
                            {
                                new(0, "選択肢1"),
                                new(1, "選択肢2"),
                                new(2, "選択肢3"),
                            }
                        ),
                    },
                    FieldMemo = "",
                };
                Cdb_Type0_Field8 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "NormalString",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };

                Cdb_Type1_Field0 = new WodiLib.Database.DatabaseFieldDefinitionSettings
                {
                    FieldName = "ItemField",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldMemo = "",
                };
            }
        }
    }
}
