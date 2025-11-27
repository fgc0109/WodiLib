using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Enum
{
    [TestFixture]
    public class DatabaseReferTypeTest : TestFixtureBase
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
            Assert.AreEqual(DatabaseReferType.Changeable.Id, "Changeable");
            Assert.AreEqual(DatabaseReferType.User.Id, "User");
            Assert.AreEqual(DatabaseReferType.System.Id, "System");
            Assert.AreEqual(DatabaseReferType.CommonEvent.Id, "CommonEvent");
        }

        /// <summary>
        ///     Code が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Code()
        {
            Assert.AreEqual(DatabaseReferType.Changeable.Code, 2);
            Assert.AreEqual(DatabaseReferType.User.Code, 1);
            Assert.AreEqual(DatabaseReferType.System.Code, 0);
            Assert.AreEqual(DatabaseReferType.CommonEvent.Code, 3);
        }

        #endregion

        #region Methods

        #region FromCode

        private static readonly object[][] FromCodeTest_Success_TestCaseSource = new[]
        {
            new object[] { 0, DatabaseReferType.System },
            new object[] { 1, DatabaseReferType.User },
            new object[] { 2, DatabaseReferType.Changeable },
            new object[] { 3, DatabaseReferType.CommonEvent },
        };

        /// <summary>
        ///     Code 文字列から DatabaseReferType インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromCodeTest_Success_TestCaseSource))]
        public static void FromCodeTest_Success(int code, DatabaseReferType? expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseReferType.FromCode(code),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     引数 code が定義されていない値の場合、ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void FromCodeTest_Failure_ArgumentInappropriate()
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseReferType.FromCode(4),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
