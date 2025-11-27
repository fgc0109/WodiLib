using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DBDataJsonConverterTest : TestFixtureBase
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
            var instance = new DBData(
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
                execFunc: () => JsonSerializer.Deserialize<DBData>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDBData>(
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
                execFunc: () => JsonSerializer.Deserialize<DBDataSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDBDataSettings>(
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
                                           + "\"data_table\":["
                                           + "{\"data_name\":\"Data0Name\","
                                           + "\"items\":["
                                           + "\"Field 0-0\","
                                           + "\"Field 0-1\","
                                           + "2,"
                                           + "\"Field 0-3\""
                                           + "]},"
                                           + "{\"data_name\":\"Data1Name\","
                                           + "\"items\":["
                                           + "\"Field 1-0\","
                                           + "\"Field 1-1\","
                                           + "102,"
                                           + "\"Field 1-3\""
                                           + "]}"
                                           + "]"
                                           + "}";

            public static readonly IDBDataSettings Settings = new DBDataSettings
            {
                DataTable = new DatabaseNamedDataTableSettings(
                    new IDatabaseNamedDataRowSettings[]
                    {
                        new DatabaseNamedDataRowSettings(
                            new DatabaseFieldValue[]
                            {
                                new("Field 0-0"),
                                new("Field 0-1"),
                                new(2),
                                new("Field 0-3"),
                            }
                        )
                        {
                            DataName = "Data0Name",
                        },
                        new DatabaseNamedDataRowSettings(
                            new DatabaseFieldValue[]
                            {
                                new("Field 1-0"),
                                new("Field 1-1"),
                                new(102),
                                new("Field 1-3"),
                            }
                        )
                        {
                            DataName = "Data1Name",
                        },
                    }
                ),
            };
        }
    }
}
