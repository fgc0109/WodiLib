using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldValueListJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region JSON

        #region Serialize

        [Test]
        public static void SerializeJsonTest_Int()
        {
            var instance = new DatabaseFieldValueList(
                SerializeTestItem.Settings_Int
            );
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    instance,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_Int)
            );
        }

        [Test]
        public static void SerializeJsonTest_String()
        {
            var instance = new DatabaseFieldValueList(
                SerializeTestItem.Settings_String
            );
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    instance,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.JsonText_String)
            );
        }

        #endregion

        #region Deserialize

        [Test]
        public static void DeserializeJsonTest_MutableList_Int()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldValueList>(
                    SerializeTestItem.JsonText_Int,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Int)
            );
        }

        [Test]
        public static void DeserializeJsonTest_FixedList_Int()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<FixedDatabaseFieldValueList>(
                    SerializeTestItem.JsonText_Int,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Int)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ReadOnlyList_Int()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldValueList>(
                    SerializeTestItem.JsonText_Int,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Int)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ListSettings_Int()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldValueListSettings>(
                    SerializeTestItem.JsonText_Int,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Int)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ListSettingsInterface_Int()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldValueListSettings>(
                    SerializeTestItem.JsonText_Int,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_Int)
            );
        }

        [Test]
        public static void DeserializeJsonTest_MutableList_String()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldValueList>(
                    SerializeTestItem.JsonText_String,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_String)
            );
        }

        [Test]
        public static void DeserializeJsonTest_FixedList_String()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<FixedDatabaseFieldValueList>(
                    SerializeTestItem.JsonText_String,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_String)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ReadOnlyList_String()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ReadOnlyDatabaseFieldValueList>(
                    SerializeTestItem.JsonText_String,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_String)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ListSettings_String()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldValueListSettings>(
                    SerializeTestItem.JsonText_String,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_String)
            );
        }

        [Test]
        public static void DeserializeJsonTest_ListSettingsInterface_String()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<IDatabaseFieldValueListSettings>(
                    SerializeTestItem.JsonText_String,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                )!,
                resultValueVerifier: ValueVerifier.AreItemEquals(SerializeTestItem.Settings_String)
            );
        }

        #endregion

        #endregion

        private static class SerializeTestItem
        {
            public const string JsonText_Int = "[0,1,2,3]";

            public static readonly IDatabaseFieldValueListSettings Settings_Int = new DatabaseFieldValueListSettings(
                new DatabaseFieldValue[]
                {
                    new(0),
                    new(1),
                    new(2),
                    new(3),
                }
            )
            {
                FieldType = DatabaseFieldType.Int,
            };

            public const string JsonText_String = "[\"Field 0\",\"Field 1\",\"Field 2\",\"Field 3\"]";

            public static readonly IDatabaseFieldValueListSettings Settings_String = new DatabaseFieldValueListSettings(
                new DatabaseFieldValue[]
                {
                    new("Field 0"),
                    new("Field 1"),
                    new("Field 2"),
                    new("Field 3"),
                }
            )
            {
                FieldType = DatabaseFieldType.String,
            };
        }
    }
}
