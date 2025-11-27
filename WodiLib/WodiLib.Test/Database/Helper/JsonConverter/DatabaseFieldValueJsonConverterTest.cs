using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldValueJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region JSON

        #region Serialize

        [Test]
        public static void SerializeJsonTest_IntValue()
        {
            var instance = new DatabaseFieldValue(123);
            const string expected = "123";
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
        public static void SerializeJsonTest_StringValue()
        {
            var instance = new DatabaseFieldValue("Value");
            const string expected = "\"Value\"";
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
        public static void DeserializeJsonTest_IntValue()
        {
            const string jsonText = "123";
            var expected = new DatabaseFieldValue(123);
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldValue>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals<DatabaseFieldValue?>(expected)
            );
        }

        [Test]
        public static void DeserializeJsonTest_StringValue()
        {
            const string jsonText = "\"StringValue\"";
            var expected = new DatabaseFieldValue("StringValue");
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseFieldValue>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals<DatabaseFieldValue?>(expected)
            );
        }

        #endregion

        #endregion
    }
}
