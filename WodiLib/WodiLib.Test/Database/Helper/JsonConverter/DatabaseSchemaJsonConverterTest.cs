using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseSchemaJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseSchema(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseSchema>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseSchema>(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseSchemaSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDatabaseSchemaSettings>(
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
                                           + "\"db_kind\":\"User\","
                                           + "\"type_table_list\":["
                                           + "{\"type_name\":\"TypeName\","
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
                                           + "\"items\":[{"
                                           + "\"data_name\":\"Data_Name\","
                                           + "\"items\":[0,1]"
                                           + "}]"
                                           + "}"
                                           + "]"
                                           + "}";

            public static readonly IDatabaseSchemaSettings Settings = new DatabaseSchemaSettings
            {
                DbKind = DatabaseKind.User,
                TypeTableList = new DatabaseTypeTableListSettings(
                    new IDatabaseTypeTableSettings[]
                    {
                        new DatabaseTypeTableSettings(
                            new IDatabaseNamedDataRowSettings[]
                            {
                                new DatabaseNamedDataRowSettings(
                                    new DatabaseFieldValue[]
                                    {
                                        new(0),
                                        new(1),
                                    }
                                )
                                {
                                    DataName = "Data_Name",
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
                        },
                    }
                ),
            };
        }
    }
}
