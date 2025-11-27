using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseValueCaseJsonConverterTest : TestFixtureBase
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
            var instance = new DatabaseValueCase(
                21,
                "CaseValue"
            );
            const string expected = "{\"case_number\":21,\"description\":\"CaseValue\"}";
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
        public static void DeserializeJsonTest()
        {
            const string jsonText = "{\"case_number\":21,\"description\":\"CaseValue\"}";
            var expected = new DatabaseValueCase(
                21,
                "CaseValue"
            );
            ;
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DatabaseValueCase>(
                    jsonText,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals<DatabaseValueCase?>(expected)
            );
        }

        #endregion

        #endregion
    }
}
