using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldDefinitionListJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseFieldDefinitionList(
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
        public static void DeserializeJsonTest_MutableList()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinitionList>(
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
        public static void DeserializeJsonTest_FixedList()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<FixedDatabaseFieldDefinitionList>(
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
        public static void DeserializeJsonTest_ReadOnlyList()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldDefinitionList>(
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
        public static void DeserializeJsonTest_ListSettings()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldDefinitionListSettings>(
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
        public static void DeserializeJsonTest_ListSettingsInterface()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldDefinitionListSettings>(
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
            public const string JsonText = "["
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
                                           + "]";

            public static readonly IDatabaseFieldDefinitionListSettings Settings =
                new DatabaseFieldDefinitionListSettings(
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
                );
        }
    }
}
