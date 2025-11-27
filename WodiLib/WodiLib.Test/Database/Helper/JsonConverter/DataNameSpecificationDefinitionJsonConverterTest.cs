using System.Text.Json;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DataNameSpecificationDefinitionJsonConverterTest : TestFixtureBase
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
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(
                    SerializeTestItem.instance,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals(SerializeTestItem.jsonTest)
            );
        }

        #endregion

        #region Deserialize

        [Test]
        public static void DeserializeJsonTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<DataNameSpecificationDefinition>(
                    SerializeTestItem.jsonTest,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    }
                ),
                resultValueVerifier: ValueVerifier.AreEquals<DataNameSpecificationDefinition?>(
                    SerializeTestItem.instance
                )
            );
        }

        #endregion

        #endregion

        private static class SerializeTestItem
        {
            public const string jsonTest = "{\"database_kind\":\"System\",\"type_id\":8}";

            public static readonly DataNameSpecificationDefinition instance = new(
                DatabaseKind.System,
                new TypeId(8)
            );
        }
    }
}
