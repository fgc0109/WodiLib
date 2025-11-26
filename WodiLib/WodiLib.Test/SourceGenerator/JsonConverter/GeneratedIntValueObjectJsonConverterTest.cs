using System.Text.Json;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.SourceGenerator.JsonConverter
{
    [TestFixture]
    public class GeneratedIntValueObjectJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [Test]
        public static void SerializeTest()
        {
            var instance = new ConditionRight(1234);
            var expected = $"1234";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(instance),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        [Test]
        public static void DeserializeTest()
        {
            var expected = new ConditionRight(1234);
            var jsonText = $"1234";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<ConditionRight>(jsonText),
                resultValueVerifier: ValueVerifier.AreEquals<ConditionRight?>(expected)
            );
        }
    }
}
