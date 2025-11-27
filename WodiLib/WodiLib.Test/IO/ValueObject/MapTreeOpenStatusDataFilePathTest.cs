using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class MapTreeOpenStatusDataFilePathTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [TestCase("MapTreeOpenStatus.dat")]
        [TestCase("./MapTreeOpenStatus_.dat")]
        [TestCase(@".\Data\MapTreeOpenStatus.dat")]
        [TestCase(@"c:\MyProject\Data\MapTreeOpenStatus.dat")]
        public static void ConstructorTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new MapTreeOpenStatusDataFilePath(value),
                instanceVerifier: new ValueVerifier<MapTreeOpenStatusDataFilePath>(instance =>
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
                factory: () => new MapTreeOpenStatusDataFilePath(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }
    }
}
