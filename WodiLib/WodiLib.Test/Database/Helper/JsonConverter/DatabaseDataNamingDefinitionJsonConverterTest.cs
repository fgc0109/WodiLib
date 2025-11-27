using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseDataNamingDefinitionJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region JSON

        #region Serialize

        [Test]
        public static void SerializeJsonTest_DesignatedType()
        {
            var instance = new DatabaseDataNamingDefinition(
                DatabaseDataNamingType.DesignatedType,
                DatabaseKind.Changeable,
                new TypeId(3)
            );
            const string expected = "{\"naming_type\":\"DesignatedType\",\"db_kind\":\"Changeable\",\"type_id\":3}";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    instance,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        [Test]
        public static void SerializeJsonTest_Manual()
        {
            var instance = new DatabaseDataNamingDefinition(
                DatabaseDataNamingType.Manual,
                DatabaseKind.Changeable
            );
            const string expected = "{\"naming_type\":\"Manual\"}";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    instance,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region Deserialize

        [Test]
        public static void DeserializeJsonTest_DesignatedType()
        {
            const string jsonText = "{\"naming_type\":\"DesignatedType\",\"db_kind\":\"Changeable\",\"type_id\":3}";
            var expected = new DatabaseDataNamingDefinition(
                DatabaseDataNamingType.DesignatedType,
                DatabaseKind.Changeable,
                new TypeId(3)
            );
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseDataNamingDefinition>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals<DatabaseDataNamingDefinition?>(expected)
            );
        }

        [Test]
        public static void DeserializeJsonTest_Manual()
        {
            const string jsonText = "{\"naming_type\":\"Manual\"}";
            var expected = new DatabaseDataNamingDefinition(
                DatabaseDataNamingType.Manual
            );
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseDataNamingDefinition>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals<DatabaseDataNamingDefinition?>(expected)
            );
        }

        #endregion

        #endregion
    }
}
