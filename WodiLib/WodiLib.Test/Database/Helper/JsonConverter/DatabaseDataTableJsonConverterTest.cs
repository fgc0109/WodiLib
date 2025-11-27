using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseDataTableJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseDataTable(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseDataTable>(
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
                execFunc: () => JsonSerializer.Deserialize<FixedDatabaseDataTable>(
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
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseDataTable>(
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
                execFunc: () => JsonSerializer.Deserialize<DatabaseDataTableSettings>(
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
                execFunc: () => JsonSerializer.Deserialize<IDatabaseDataTableSettings>(
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
                                           + "[\"Field 0-0\",\"Field 0-1\",2,\"Field 0-3\"],"
                                           + "[\"Field 1-0\",\"Field 1-1\",1002,\"Field 1-3\"],"
                                           + "[\"Field 2-0\",\"Field 2-1\",2002,\"Field 2-3\"],"
                                           + "[\"Field 3-0\",\"Field 3-1\",3002,\"Field 3-3\"]"
                                           + "]";

            public static readonly IDatabaseDataTableSettings Settings = new DatabaseDataTableSettings(
                new IDatabaseDataRowSettings[]
                {
                    new DatabaseDataRowSettings(
                        new DatabaseFieldValue[]
                        {
                            new("Field 0-0"),
                            new("Field 0-1"),
                            new(2),
                            new("Field 0-3"),
                        }
                    ),
                    new DatabaseDataRowSettings(
                        new DatabaseFieldValue[]
                        {
                            new("Field 1-0"),
                            new("Field 1-1"),
                            new(1002),
                            new("Field 1-3"),
                        }
                    ),
                    new DatabaseDataRowSettings(
                        new DatabaseFieldValue[]
                        {
                            new("Field 2-0"),
                            new("Field 2-1"),
                            new(2002),
                            new("Field 2-3"),
                        }
                    ),
                    new DatabaseDataRowSettings(
                        new DatabaseFieldValue[]
                        {
                            new("Field 3-0"),
                            new("Field 3-1"),
                            new(3002),
                            new("Field 3-3"),
                        }
                    ),
                }
            );
        }
    }
}
