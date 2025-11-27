using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class CommonFilePathTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [TestCase("Common0000.common")]
        [TestCase("./common0000_to1234.common")]
        [TestCase(@".\Data\Common0000.common")]
        [TestCase(@"c:\MyProject\Data\Common0000.common")]
        public static void ConstructorTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new CommonFilePath(value),
                instanceVerifier: new ValueVerifier<CommonFilePath>(instance =>
                    {
                        // インスタンスが意図したとおり作成されること
                        Assert.AreEqual(instance.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     引数が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorStringTest_Failure_NullArgs()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new CommonFilePath(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }
    }
}
