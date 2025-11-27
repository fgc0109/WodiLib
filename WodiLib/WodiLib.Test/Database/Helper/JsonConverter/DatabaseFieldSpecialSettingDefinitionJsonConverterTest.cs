using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region JSON

        #region Serialize

        #region Normal

        [Test]
        public static void SerializeJsonTest_Normal()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldSpecialSettingDefinition(
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

        #endregion

        #region LoadFile

        [Test]
        public static void SerializeJsonTest_LoadFile_NoInitValue()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldSpecialSettingDefinition(
                        SerializeTestItem.Settings_LoadFile_NoInitValue
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_LoadFile_NoInitValue)
            );
        }

        [Test]
        public static void SerializeJsonTest_LoadFile_HasInitValue()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldSpecialSettingDefinition(
                        SerializeTestItem.Settings_LoadFile_HasInitValue
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_LoadFile_HasInitValue)
            );
        }

        #endregion

        #region DatabaseReference

        [Test]
        public static void SerializeJsonTest_DatabaseReference_HasAdditionalCase()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldSpecialSettingDefinition(
                        SerializeTestItem.Settings_DatabaseReference_HasAdditionalCase
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(
                    SerializeTestItem.JsonText_DatabaseReference_HasAdditionalCase
                )
            );
        }

        [Test]
        public static void SerializeJsonTest_DatabaseReference_NoAdditionalCase()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldSpecialSettingDefinition(
                        SerializeTestItem.Settings_DatabaseReference_NoAdditionalCase
                    ),
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(
                    SerializeTestItem.JsonText_DatabaseReference_NoAdditionalCase
                )
            );
        }

        #endregion

        #region Manual

        [Test]
        public static void SerializeJsonTest_Manual()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    new DatabaseFieldSpecialSettingDefinition(
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

        #endregion

        #region Deserialize

        #region Normal

        [Test]
        public static void DeserializeJsonTest_Normal_BaseMutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinition>(
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
        public static void DeserializeJsonTest_Normal_BaseImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldSpecialSettingDefinition>(
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
        public static void DeserializeJsonTest_Normal_BaseSettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinitionSettings>(
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
        public static void DeserializeJsonTest_Normal_BaseSettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldSpecialSettingDefinitionSettings>(
                    SerializeTestItem.JsonText_Normal,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Normal)
            );
        }

        [Ignore("各具象クラスへの直接デシリアライズは不可能、DatabaseFieldSpecialSettingDefinition などにデシリアライズしてから TryCast する必要あり")]
        public static void DeserializeJsonTest_Normal_NormalMutableClass()
        {
        }

        #endregion

        #region LoadFile

        [Test]
        public static void DeserializeJsonTest_LoadFile_HasInitValue_BaseMutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinition>(
                    SerializeTestItem.JsonText_LoadFile_HasInitValue,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile_HasInitValue)
            );
        }

        [Test]
        public static void DeserializeJsonTest_LoadFile_HasInitValue_BaseImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldSpecialSettingDefinition>(
                    SerializeTestItem.JsonText_LoadFile_HasInitValue,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile_HasInitValue)
            );
        }

        [Test]
        public static void DeserializeJsonTest_LoadFile_HasInitValue_BaseSettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinitionSettings>(
                    SerializeTestItem.JsonText_LoadFile_HasInitValue,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile_HasInitValue)
            );
        }

        [Test]
        public static void DeserializeJsonTest_LoadFile_HasInitValue_BaseSettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldSpecialSettingDefinitionSettings>(
                    SerializeTestItem.JsonText_LoadFile_HasInitValue,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_LoadFile_HasInitValue)
            );
        }

        [Ignore("各具象クラスへの直接デシリアライズは不可能、DatabaseFieldSpecialSettingDefinition などにデシリアライズしてから TryCast する必要あり")]
        public static void DeserializeJsonTest_LoadFile_HasInitValue_LoadFileMutableClass()
        {
        }

        #endregion

        #region DatabaseReference

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_HasAdditionalCase_BaseMutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinition>(
                    SerializeTestItem.JsonText_DatabaseReference_HasAdditionalCase,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(
                    SerializeTestItem.Settings_DatabaseReference_HasAdditionalCase
                )
            );
        }

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_HasAdditionalCase_BaseImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldSpecialSettingDefinition>(
                    SerializeTestItem.JsonText_DatabaseReference_HasAdditionalCase,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(
                    SerializeTestItem.Settings_DatabaseReference_HasAdditionalCase
                )
            );
        }

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_HasAdditionalCase_BaseSettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinitionSettings>(
                    SerializeTestItem.JsonText_DatabaseReference_HasAdditionalCase,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(
                    SerializeTestItem.Settings_DatabaseReference_HasAdditionalCase
                )
            );
        }

        [Test]
        public static void DeserializeJsonTest_DatabaseReference_HasAdditionalCase_BaseSettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldSpecialSettingDefinitionSettings>(
                    SerializeTestItem.JsonText_DatabaseReference_HasAdditionalCase,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(
                    SerializeTestItem.Settings_DatabaseReference_HasAdditionalCase
                )
            );
        }

        [Ignore("各具象クラスへの直接デシリアライズは不可能、DatabaseFieldSpecialSettingDefinition などにデシリアライズしてから TryCast する必要あり")]
        public static void DeserializeJsonTest_DatabaseReference_HasAdditionalCase_DatabaseReferenceMutableClass()
        {
        }

        #endregion

        #region Manual

        [Test]
        public static void DeserializeJsonTest_Manual_BaseMutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinition>(
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
        public static void DeserializeJsonTest_Manual_BaseImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldSpecialSettingDefinition>(
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
        public static void DeserializeJsonTest_Manual_BaseSettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldSpecialSettingDefinitionSettings>(
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
        public static void DeserializeJsonTest_Manual_BaseSettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldSpecialSettingDefinitionSettings>(
                    SerializeTestItem.JsonText_Manual,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Manual)
            );
        }

        [Ignore("各具象クラスへの直接デシリアライズは不可能、DatabaseFieldSpecialSettingDefinition などにデシリアライズしてから TryCast する必要あり")]
        public static void DeserializeJsonTest_Manual_ManualMutableClass()
        {
        }

        #endregion

        #endregion

        #endregion


        private static class SerializeTestItem
        {
            public const string JsonText_Normal = "{\"setting_type\":\"Normal\","
                                                  + "\"init_value\":4"
                                                  + "}";

            public static readonly IDatabaseFieldSpecialSettingDefinitionSettings Settings_Normal =
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionNormalSettings
                    {
                        InitValue = 4,
                    }
                );

            public const string JsonText_LoadFile_NoInitValue = "{\"setting_type\":\"LoadFile\","
                                                                + "\"folder_name\":\"C:\\\\foo\\\\bar\","
                                                                + "\"is_omit_folder_name\":false"
                                                                + "}";

            public static readonly IDatabaseFieldSpecialSettingDefinitionSettings Settings_LoadFile_NoInitValue =
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        InitValue = 0,
                        FolderName = "C:\\foo\\bar",
                        IsOmitFolderName = false,
                    }
                );

            public const string JsonText_LoadFile_HasInitValue = "{\"setting_type\":\"LoadFile\","
                                                                 + "\"init_value\":4,"
                                                                 + "\"folder_name\":\"dir\\\\path\","
                                                                 + "\"is_omit_folder_name\":true"
                                                                 + "}";

            public static readonly IDatabaseFieldSpecialSettingDefinitionSettings Settings_LoadFile_HasInitValue =
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                    {
                        InitValue = 4,
                        FolderName = "dir\\path",
                        IsOmitFolderName = true,
                    }
                );

            public const string JsonText_DatabaseReference_HasAdditionalCase = "{\"setting_type\":\"ReferDatabase\","
                                                                               + "\"init_value\":4,"
                                                                               + "\"database_refer_kind\":\"CommonEvent\","
                                                                               + "\"database_db_type_id\":13,"
                                                                               + "\"is_use_additional_items\":true,"
                                                                               + "\"additional_case1\":\"case1\","
                                                                               + "\"additional_case2\":\"case2\","
                                                                               + "\"additional_case3\":\"case3\""
                                                                               + "}";

            public static readonly IDatabaseFieldSpecialSettingDefinitionSettings
                Settings_DatabaseReference_HasAdditionalCase =
                    new DatabaseFieldSpecialSettingDefinitionSettings(
                        new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                        {
                            InitValue = 4,
                            DatabaseReferKind = DatabaseReferType.CommonEvent,
                            DatabaseDbTypeId = 13,
                            IsUseAdditionalItems = true,
                            AdditionalCase1 = "case1",
                            AdditionalCase2 = "case2",
                            AdditionalCase3 = "case3",
                        }
                    );

            public const string JsonText_DatabaseReference_NoAdditionalCase = "{\"setting_type\":\"ReferDatabase\","
                                                                              + "\"init_value\":1,"
                                                                              + "\"database_refer_kind\":\"System\","
                                                                              + "\"database_db_type_id\":8,"
                                                                              + "\"is_use_additional_items\":false"
                                                                              + "}";

            public static readonly IDatabaseFieldSpecialSettingDefinitionSettings
                Settings_DatabaseReference_NoAdditionalCase =
                    new DatabaseFieldSpecialSettingDefinitionSettings(
                        new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                        {
                            InitValue = 1,
                            DatabaseReferKind = DatabaseReferType.System,
                            DatabaseDbTypeId = 8,
                            IsUseAdditionalItems = false,
                        }
                    );

            public const string JsonText_Manual = "{\"setting_type\":\"Manual\","
                                                  + "\"init_value\":2,"
                                                  + "\"special_cases\":["
                                                  + "{"
                                                  + "\"case_number\":1,"
                                                  + "\"description\":\"Case 1\""
                                                  + "},"
                                                  + "{"
                                                  + "\"case_number\":2,"
                                                  + "\"description\":\"Case 2\""
                                                  + "},"
                                                  + "{"
                                                  + "\"case_number\":3,"
                                                  + "\"description\":\"Case 3\""
                                                  + "}"
                                                  + "]"
                                                  + "}";

            public static readonly IDatabaseFieldSpecialSettingDefinitionSettings Settings_Manual =
                new DatabaseFieldSpecialSettingDefinitionSettings(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings
                    {
                        InitValue = 2,
                        SpecialCases = new DatabaseValueCaseListSettings(
                            new DatabaseValueCase[]
                            {
                                new(1, "Case 1"),
                                new(2, "Case 2"),
                                new(3, "Case 3"),
                            }
                        ),
                    }
                );
        }
    }
}
