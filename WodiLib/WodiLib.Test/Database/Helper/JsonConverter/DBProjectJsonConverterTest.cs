using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DBProjectJsonConverterTest : TestFixtureBase
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
            var instance = new DBProject(
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
                execFunc: () => JsonSerializer.Deserialize<DBProject>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDBProject>(
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
                execFunc: () => JsonSerializer.Deserialize<DBProjectSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDBProjectSettings>(
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
                                           + "\"db_kind\":\"System\","
                                           + "\"project_type_list\":[{"
                                           + "\"type_name\":\"TestTypeName\","
                                           + "\"memo\":\"TestMemo\","
                                           + "\"data_name_list\":[\"DataName1\",\"DataName2\",\"DataName3\"],"
                                           + "\"field_metadata_list\":"
                                           + "["
                                           + "{\"field_name\":\"Field Name LoadFile\","
                                           + "\"special_setting_definition\":{"
                                           + "\"setting_type\":\"LoadFile\","
                                           + "\"folder_name\":\"FolderName\","
                                           + "\"is_omit_folder_name\":true"
                                           + "},"
                                           + "\"field_memo\":\"Field Memo LoadFile\""
                                           + "}"
                                           + "]"
                                           + "}]"
                                           + "}";

            public static readonly IDBProjectSettings Settings = new DBProjectSettings
            {
                DbKind = DatabaseKind.System,
                ProjectTypeList = new DatabaseProjectTypeListSettings(
                    new IDatabaseProjectTypeSettings[]
                    {
                        new DatabaseProjectTypeSettings
                        {
                            TypeName = "TestTypeName",
                            Memo = "TestMemo",
                            DataNameList = new DatabaseDataNameListSettings(
                                new DataName[]
                                {
                                    "DataName1",
                                    "DataName2",
                                    "DataName3",
                                }
                            ),
                            FieldMetadataList = new DatabaseFieldMetadataListSettings(
                                new IDatabaseFieldMetadataSettings[]
                                {
                                    new DatabaseFieldMetadataSettings
                                    {
                                        FieldName = "Field Name LoadFile",
                                        SpecialSettingDefinition =
                                            new DatabaseFieldSpecialSettingDefinitionLoadFileSettings
                                            {
                                                FolderName = "FolderName",
                                                IsOmitFolderName = true,
                                            },
                                        FieldMemo = "Field Memo LoadFile",
                                    },
                                }
                            ),
                        },
                    }
                ),
            };
        }
    }
}
