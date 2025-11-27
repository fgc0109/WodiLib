using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Enum
{
    [TestFixture]
    public class DatabaseKindTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     ID が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Id()
        {
            Assert.AreEqual(DatabaseKind.Changeable.Id, "Changeable");
            Assert.AreEqual(DatabaseKind.User.Id, "User");
            Assert.AreEqual(DatabaseKind.System.Id, "System");
        }

        /// <summary>
        ///     Code が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Code()
        {
            Assert.AreEqual(DatabaseKind.Changeable.Code, 0);
            Assert.AreEqual(DatabaseKind.User.Code, 2);
            Assert.AreEqual(DatabaseKind.System.Code, 1);
        }

        #endregion

        #region Methods

        #region FromCode

        private static readonly object[][] FromCodeTest_Success_TestCaseSource =
        {
            new object[] { (byte)0, DatabaseKind.Changeable },
            new object[] { (byte)1, DatabaseKind.System },
            new object[] { (byte)2, DatabaseKind.User },
        };

        /// <summary>
        ///     Code 文字列から DBKind インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromCodeTest_Success_TestCaseSource))]
        public static void FromCodeTest_Success(byte code, DatabaseKind expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseKind.FromCode(code),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     引数 code が定義されていない値の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void FromCodeTest_Failure_ArgumentInappropriate()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseKind.FromCode(3),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
