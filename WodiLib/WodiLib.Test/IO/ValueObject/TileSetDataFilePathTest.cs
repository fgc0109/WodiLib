using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class TileSetDataFilePathTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [TestCase("TileSetData.dat")]
        [TestCase("./TileSetData_.dat")]
        [TestCase(@".\Data\TileSetData.dat")]
        [TestCase(@"c:\MyProject\Data\TileSetData.dat")]
        public static void ConstructorTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new TileSetDataFilePath(value),
                instanceVerifier: new ValueVerifier<TileSetDataFilePath>(instance =>
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
                factory: () => new TileSetDataFilePath(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }
    }
}
