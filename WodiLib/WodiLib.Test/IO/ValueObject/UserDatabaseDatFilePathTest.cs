using System;
using NUnit.Framework;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.ValueObject
{
    [TestFixture]
    public class UserDatabaseDatFilePathTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        [TestCase("DataBase.dat")]
        [TestCase("./CDataBase.dat")]
        [TestCase(@".\Data\DataBase.dat")]
        [TestCase(@"c:\MyProject\Data\DataBase.dat")]
        public static void ConstructorTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new UserDatabaseDatFilePath(value),
                instanceVerifier: new ValueVerifier<UserDatabaseDatFilePath>(instance =>
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
                factory: () => new UserDatabaseDatFilePath(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }
    }
}
