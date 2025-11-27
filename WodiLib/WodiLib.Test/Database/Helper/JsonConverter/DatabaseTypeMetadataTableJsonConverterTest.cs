using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseTypeMetadataTableJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseTypeMetadataTable(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeMetadataTable>(
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
                execFunc: () => JsonSerializer.Deserialize<FixedDatabaseTypeMetadataTable>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseTypeMetadataTable>(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeMetadataTableSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDatabaseTypeMetadataTableSettings>(
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
                                           + "\"type_name\":\"TypeName\","
                                           + "\"memo\":\"Type Memo\","
                                           + "\"data_naming_definition\":{"
                                           + "\"naming_type\":\"DesignatedType\","
                                           + "\"db_kind\":\"User\","
                                           + "\"type_id\":1"
                                           + "},"
                                           + "\"field_metadata_list\":["
                                           + "{"
                                           + "\"field_name\":\"Field0\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"Manual\","
                                           + "\"init_value\":0,"
                                           + "\"special_cases\":[]"
                                           + "},"
                                           + "\"field_memo\":\"Field0 Memo\""
                                           + "},"
                                           + "{"
                                           + "\"field_name\":\"Field1\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"Normal\","
                                           + "\"init_value\":0"
                                           + "},"
                                           + "\"field_memo\":\"Field1 Memo\""
                                           + "}"
                                           + "],"
                                           + "\"items\":["
                                           + "{"
                                           + "\"data_name\":\"Data0\","
                                           + "\"items\":[0,\"Data 0 Field 1\"]"
                                           + "},"
                                           + "{"
                                           + "\"data_name\":\"Data1\","
                                           + "\"items\":[100,\"Data 1 Field 1\"]"
                                           + "}"
                                           + "]}";

            public static readonly IDatabaseTypeMetadataTableSettings Settings = new DatabaseTypeMetadataTableSettings(
                new IDatabaseNamedDataRowSettings[]
                {
                    new DatabaseNamedDataRowSettings(
                        new DatabaseFieldValue[]
                        {
                            new(0),
                            new("Data 0 Field 1"),
                        }
                    )
                    {
                        DataName = "Data0",
                    },
                    new DatabaseNamedDataRowSettings(
                        new DatabaseFieldValue[]
                        {
                            new(100),
                            new("Data 1 Field 1"),
                        }
                    )
                    {
                        DataName = "Data1",
                    },
                }
            )
            {
                TypeName = "TypeName",
                DataNamingDefinition = new DatabaseDataNamingDefinition(
                    DatabaseDataNamingType.DesignatedType,
                    DatabaseKind.User,
                    new TypeId(1)
                ),
                FieldMetadataList = new DatabaseFieldMetadataListSettings(
                    new IDatabaseFieldMetadataSettings[]
                    {
                        new DatabaseFieldMetadataSettings
                        {
                            FieldName = "Field0",
                            SpecialSettingDefinition =
                                new DatabaseFieldSpecialSettingDefinitionManualSettings
                                {
                                    InitValue = 0,
                                },
                            FieldMemo = "Field0 Memo",
                        },
                        new DatabaseFieldMetadataSettings
                        {
                            FieldName = "Field1",
                            SpecialSettingDefinition =
                                new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                            FieldMemo = "Field1 Memo",
                        },
                    }
                ),
                Memo = "Type Memo",
            };
        }
    }
}
