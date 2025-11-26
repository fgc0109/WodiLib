using System.Text.Json;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.SourceGenerator.JsonConverter
{
    [TestFixture]
    public class GeneratedStringValueObjectJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [Test]
        public static void SerializeTest()
        {
            var instance = new CharaChipFilePath("TestPath");
            var expected = $"\"TestPath\"";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(instance),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        [Test]
        public static void DeserializeTest()
        {
            var expected = new CharaChipFilePath("TestPath");
            var jsonText = $"\"TestPath\"";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<CharaChipFilePath>(jsonText),
                resultValueVerifier: ValueVerifier.AreEquals<CharaChipFilePath?>(expected)
            );
        }
    }
}
