using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class MapTreeDataFilePathTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [TestCase("MapTree.dat")]
        [TestCase("./Map_Tree.dat")]
        [TestCase(@".\Data\MapTree.dat")]
        [TestCase(@"c:\MyProject\Data\MapTree.dat")]
        public static void ConstructorTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new ChangeableDatabaseDatFilePath(value),
                instanceVerifier: new ValueVerifier<ChangeableDatabaseDatFilePath>(instance =>
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
                factory: () => new ChangeableDatabaseDatFilePath(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }
    }
}
