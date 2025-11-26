using System.Text.Json;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.SourceGenerator.JsonConverter
{
    [TestFixture]
    public class GeneratedTypeSafeEnumJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [Test]
        public static void SerializeTest()
        {
            var instance = KeyboardCode.A;
            var expected = $"\"{instance.Id}\"";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(instance),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        [Test]
        public static void DeserializeTest()
        {
            var expected = KeyboardCode.C;
            var jsonText = $"\"{expected.Id}\"";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<KeyboardCode>(jsonText),
                resultValueVerifier: ValueVerifier.AreEquals<KeyboardCode?>(expected)
            );
        }
    }
}
