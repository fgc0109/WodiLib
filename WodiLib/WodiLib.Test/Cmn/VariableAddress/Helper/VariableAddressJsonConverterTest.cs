using System.Text.Json;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.Helper
{
    [TestFixture]
    public class VariableAddressJsonConverterTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [Test]
        public static void SerializeTest()
        {
            var instance = VariableAddressFactory.Create(1024002409);
            var expected = $"1024002409";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Serialize(instance),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        [Test]
        public static void DeserializeTest()
        {
            var expected = VariableAddressFactory.Create(1024002409);
            var jsonText = $"1024002409";

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => JsonSerializer.Deserialize<VariableAddress>(jsonText),
                resultValueVerifier: ValueVerifier.AreEquals<VariableAddress?>(expected)
            );
        }
    }
}
