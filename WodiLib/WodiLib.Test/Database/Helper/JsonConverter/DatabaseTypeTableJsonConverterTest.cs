using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseTypeTableJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseTypeTable(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeTable>(
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
                execFunc: () => JsonSerializer.Deserialize<FixedDatabaseTypeTable>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseTypeTable>(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseTypeTableSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDatabaseTypeTableSettings>(
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
                                           + "\"field_definition_list\":["
                                           + "{"
                                           + "\"field_name\":\"Field0\","
                                           + "\"field_type\":\"Int\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"Manual\","
                                           + "\"init_value\":0,"
                                           + "\"special_cases\":[]"
                                           + "},"
                                           + "\"field_memo\":\"Field0 Memo\""
                                           + "},"
                                           + "{"
                                           + "\"field_name\":\"Field1\","
                                           + "\"field_type\":\"String\","
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

            public static readonly IDatabaseTypeTableSettings Settings = new DatabaseTypeTableSettings(
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
                FieldDefinitionList = new DatabaseFieldDefinitionListSettings(
                    new IDatabaseFieldDefinitionSettings[]
                    {
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Field0",
                            FieldType = DatabaseFieldType.Int,
                            SpecialSettingDefinition =
                                new DatabaseFieldSpecialSettingDefinitionManualSettings
                                {
                                    InitValue = 0,
                                },
                            FieldMemo = "Field0 Memo",
                        },
                        new DatabaseFieldDefinitionSettings
                        {
                            FieldName = "Field1",
                            FieldType = DatabaseFieldType.String,
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
