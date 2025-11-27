using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseTypeMetadataJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseTypeMetadata(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeMetadata>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseTypeMetadata>(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeMetadataSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDatabaseTypeMetadataSettings>(
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
                                           + "\"data_naming_definition\":{\"naming_type\":\"EqualBefore\"},"
                                           + "\"field_metadata_list\":["
                                           + "{\"field_name\":\"Field Name 0\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"Normal\","
                                           + "\"init_value\":0"
                                           + "},"
                                           + "\"field_memo\":\"Field Memo 0\""
                                           + "},"
                                           + "{\"field_name\":\"Field Name 1\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"LoadFile\","
                                           + "\"folder_name\":\"FolderName\","
                                           + "\"is_omit_folder_name\":true"
                                           + "},"
                                           + "\"field_memo\":\"Field Memo 1\""
                                           + "}"
                                           + "]"
                                           + "}";

            public static readonly IDatabaseTypeMetadataSettings Settings = new DatabaseTypeMetadataSettings
            {
                TypeName = "TestTypeName",
                Memo = "TestMemo",
                DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.EqualBefore),
                FieldMetadataList = new DatabaseFieldMetadataListSettings(
                    new IDatabaseFieldMetadataSettings[]
                    {
                        new DatabaseFieldMetadataSettings
                        {
                            FieldName = "Field Name 0",
                            SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings
                            {
                                InitValue = 0,
                            },
                            FieldMemo = "Field Memo 0",
                        },
                        new DatabaseFieldMetadataSettings
                        {
                            FieldName = "Field Name 1",
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
