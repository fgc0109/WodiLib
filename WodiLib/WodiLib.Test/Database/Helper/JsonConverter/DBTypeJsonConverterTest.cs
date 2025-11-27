using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DBTypeJsonConverterTest : TestFixtureBase
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
            var instance = new DBType(
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
                execFunc: () => JsonSerializer.Deserialize<DBType>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDBType>(
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
                execFunc: () => JsonSerializer.Deserialize<DBTypeSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDBTypeSettings>(
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
                                           + "\"type_metadata_table\":{"
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
                                           + "\"items\":[{"
                                           + "\"data_name\":\"\","
                                           + "\"items\":[0,\"Field1\"]"
                                           + "}]"
                                           + "}"
                                           + "}";

            public static readonly IDBTypeSettings Settings = new DBTypeSettings
            {
                TypeMetadataTable = new DatabaseTypeMetadataTableSettings(
                    new IDatabaseNamedDataRowSettings[]
                    {
                        new DatabaseNamedDataRowSettings(
                            new DatabaseFieldValue[]
                            {
                                new(0),
                                new("Field1"),
                            }
                        ),
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
                },
            };
        }
    }
}
