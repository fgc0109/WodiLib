using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldDefinitionJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region JSON

        #region Serialize

        [Test]
        public static void SerializeJsonTest_Normal()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldDefinition(
                        SerializeTestItem.Settings_Normal
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_Normal)
            );
        }

        [Test]
        public static void SerializeJsonTest_LoadFile()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldDefinition(
                        SerializeTestItem.Settings_LoadFile
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_LoadFile)
            );
        }

        [Test]
        public static void SerializeJsonTest_DatabaseReference()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldDefinition(
                        SerializeTestItem.Settings_DatabaseReference
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_DatabaseReference)
            );
        }

        [Test]
        public static void SerializeJsonTest_Manual()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldDefinition(
                        SerializeTestItem.Settings_Manual
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_Manual)
            );
        }

        #endregion

        #region Deserialize

        #region Normal

        [Test]
        public static void DeserializeJsonTest_Normal_MutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_Normal,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Normal)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Normal_ImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_Normal,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Normal)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Normal_SettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_Normal,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Normal)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Normal_SettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_Normal,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Normal)
            );
        }

        #endregion

        #region LoadFile

        [Test]
        public static void DeserializeJsonTest_LoadFile_MutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_LoadFile,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile)
            );
        }

        [Test]
        public static void DeserializeJsonTest_LoadFile_ImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_LoadFile,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile)
            );
        }

        [Test]
        public static void DeserializeJsonTest_LoadFile_SettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_LoadFile,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile)
            );
        }

        [Test]
        public static void DeserializeJsonTest_LoadFile_SettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_LoadFile,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile)
            );
        }

        #endregion

        #region DatabaseReference

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_MutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_DatabaseReference,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_DatabaseReference)
            );
        }

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_ImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_DatabaseReference,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_DatabaseReference)
            );
        }

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_SettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_DatabaseReference,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_DatabaseReference)
            );
        }

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_SettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_DatabaseReference,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_DatabaseReference)
            );
        }

        #endregion

        #region Manual

        [Test]
        public static void DeserializeJsonTest_Manual_MutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_Manual,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Manual)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Manual_ImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldDefinition>(
                    SerializeTestItem.JsonText_Manual,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Manual)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Manual_SettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_Manual,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Manual)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Manual_SettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldDefinitionSettings>(
                    SerializeTestItem.JsonText_Manual,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Manual)
            );
        }

        #endregion

        #endregion

        #endregion

        private static class SerializeTestItem
        {
            public const string JsonText_Normal = "{\"field_name\":\"Field Name Normal\","
                                                  + "\"field_type\":\"String\","
                                                  + "\"special_setting_definition\":{"
                                                  + "\"setting_type\":\"Normal\","
                                                  + "\"init_value\":20"
                                                  + "},"
                                                  + "\"field_memo\":\"Field Memo Normal\""
                                                  + "}";

            public static readonly IDatabaseFieldDefinitionSettings Settings_Normal =
                new DatabaseFieldDefinitionSettings
                {
                    FieldName = "Field Name Normal",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = 20,
                    },
                    FieldMemo = "Field Memo Normal",
                };

            public const string JsonText_LoadFile = "{\"field_name\":\"Field Name LoadFile\","
                                                    + "\"field_type\":\"String\","
                                                    + "\"special_setting_definition\":{"
                                                    + "\"setting_type\":\"LoadFile\","
                                                    + "\"folder_name\":\"FolderName\","
                                                    + "\"is_omit_folder_name\":true"
                                                    + "},"
                                                    + "\"field_memo\":\"Field Memo LoadFile\""
                                                    + "}";

            public static readonly IDatabaseFieldDefinitionSettings Settings_LoadFile =
                new DatabaseFieldDefinitionSettings
                {
                    FieldName = "Field Name LoadFile",
                    FieldType = DatabaseFieldType.String,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        FolderName = "FolderName",
                        IsOmitFolderName = true,
                    },
                    FieldMemo = "Field Memo LoadFile",
                };

            public const string JsonText_DatabaseReference = "{\"field_name\":\"Field Name DatabaseReference\","
                                                             + "\"field_type\":\"Int\","
                                                             + "\"special_setting_definition\":{"
                                                             + "\"setting_type\":\"ReferDatabase\","
                                                             + "\"init_value\":-1,"
                                                             + "\"database_refer_kind\":\"Changeable\","
                                                             + "\"database_db_type_id\":2,"
                                                             + "\"is_use_additional_items\":true,"
                                                             + "\"additional_case1\":\"Case 1\","
                                                             + "\"additional_case2\":\"Case 2\","
                                                             + "\"additional_case3\":\"Case 3\""
                                                             + "},"
                                                             + "\"field_memo\":\"Field Memo DatabaseReference\""
                                                             + "}";

            public static readonly IDatabaseFieldDefinitionSettings Settings_DatabaseReference =
                new DatabaseFieldDefinitionSettings
                {
                    FieldName = "Field Name DatabaseReference",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                    {
                        InitValue = -1,
                        DatabaseReferKind = DatabaseReferType.Changeable,
                        DatabaseDbTypeId = 2,
                        IsUseAdditionalItems = true,
                        AdditionalCase1 = "Case 1",
                        AdditionalCase2 = "Case 2",
                        AdditionalCase3 = "Case 3",
                    },
                    FieldMemo = "Field Memo DatabaseReference",
                };

            public const string JsonText_Manual = "{\"field_name\":\"Field Name Manual\","
                                                  + "\"field_type\":\"Int\","
                                                  + "\"special_setting_definition\":{"
                                                  + "\"setting_type\":\"Manual\","
                                                  + "\"init_value\":3,"
                                                  + "\"special_cases\":["
                                                  + "{\"case_number\":1,\"description\":\"Case 1\"},"
                                                  + "{\"case_number\":2,\"description\":\"Case 2\"},"
                                                  + "{\"case_number\":3,\"description\":\"Case 3\"}"
                                                  + "]"
                                                  + "},"
                                                  + "\"field_memo\":\"Field Memo Manual\""
                                                  + "}";

            public static readonly IDatabaseFieldDefinitionSettings Settings_Manual =
                new DatabaseFieldDefinitionSettings
                {
                    FieldName = "Field Name Manual",
                    FieldType = DatabaseFieldType.Int,
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionManualSettings
                    {
                        InitValue = 3,
                        SpecialCases = new DatabaseValueCaseListSettings(
                            new DatabaseValueCase[]
                            {
                                new(1, "Case 1"),
                                new(2, "Case 2"),
                                new(3, "Case 3"),
                            }
                        ),
                    },
                    FieldMemo = "Field Memo Manual",
                };
        }
    }
}
