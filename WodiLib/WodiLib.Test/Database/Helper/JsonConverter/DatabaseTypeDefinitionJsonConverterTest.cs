using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseTypeDefinitionJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region JSON

        #region Serialize

        [Test]
        public static void SerializeJsonTest()
        {
            var instance = new DatabaseTypeDefinition(
                SerializeTestItem.Settings
            );
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    instance,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText)
            );
        }

        #endregion

        #region Deserialize

        [Test]
        public static void DeserializeJsonTest_MutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeDefinition>(
                    SerializeTestItem.JsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ImmutableClass()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseTypeDefinition>(
                    SerializeTestItem.JsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings)
            );
        }

        [Test]
        public static void DeserializeJsonTest_SettingsDto()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeDefinitionSettings>(
                    SerializeTestItem.JsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings)
            );
        }

        [Test]
        public static void DeserializeJsonTest_SettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseTypeDefinitionSettings>(
                    SerializeTestItem.JsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings)
            );
        }

        #endregion

        #endregion

        private static class SerializeTestItem
        {
            public const string JsonText = "{"
                                           + "\"type_name\":\"TestTypeName\","
                                           + "\"memo\":\"TestMemo\","
                                           + "\"field_definition_list\":["
                                           + "{\"field_name\":\"Field Name 0\","
                                           + "\"field_type\":\"String\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"Normal\","
                                           + "\"init_value\":0"
                                           + "},"
                                           + "\"field_memo\":\"Field Memo 0\""
                                           + "},"
                                           + "{\"field_name\":\"Field Name 1\","
                                           + "\"field_type\":\"String\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"LoadFile\","
                                           + "\"folder_name\":\"FolderName\","
                                           + "\"is_omit_folder_name\":true"
                                           + "},"
                                           + "\"field_memo\":\"Field Memo 1\""
                                           + "}"
                                           + "]"
                                           + "}";

            public static readonly IDatabaseTypeDefinitionSettings Settings = new DatabaseTypeDefinitionSettings
            {
                TypeName = "TestTypeName",
                Memo = "TestMemo",
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new IDatabaseFieldDefinitionSettings[]
                    {
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Field Name 0",
                            FieldType = DatabaseFieldType.String,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                            {
                                InitValue = 0,
                            },
                            FieldMemo = "Field Memo 0",
                        },
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Field Name 1",
                            FieldType = DatabaseFieldType.String,
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                            {
                                FolderName = "FolderName",
                                IsOmitFolderName = true,
                            },
                            FieldMemo = "Field Memo 1",
                        },
                    }
                ),
            };
        }
    }
}
