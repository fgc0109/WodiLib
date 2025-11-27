using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Enum
{
    [TestFixture]
    public class DatabaseDataNamingTypeTest : TestFixtureBase
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
            Assert.AreEqual(DatabaseDataNamingType.Manual.Id, "Manual");
            Assert.AreEqual(DatabaseDataNamingType.FirstStringData.Id, "FirstStringData");
            Assert.AreEqual(DatabaseDataNamingType.EqualBefore.Id, "EqualBefore");
            Assert.AreEqual(DatabaseDataNamingType.DesignatedType.Id, "DesignatedType");
        }

        /// <summary>
        ///     Code が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Code()
        {
            Assert.AreEqual(DatabaseDataNamingType.Manual.Code, 0);
            Assert.AreEqual(DatabaseDataNamingType.FirstStringData.Code, 1);
            Assert.AreEqual(DatabaseDataNamingType.EqualBefore.Code, 2);
            Assert.AreEqual(DatabaseDataNamingType.DesignatedType.Code, -1);
        }

        #endregion

        #region Methods

        #region FromCode

        private static readonly object[][] FromCodeTest_Success_TestCaseSource = new[]
        {
            // [code, expected]
            new object[] { 0, DatabaseDataNamingType.Manual },
            new object[] { 1, DatabaseDataNamingType.FirstStringData },
            new object[] { 2, DatabaseDataNamingType.EqualBefore },
            new object[] { -1, DatabaseDataNamingType.DesignatedType },
        };

        /// <summary>
        ///     Code 文字列から DatabaseDataNamingType インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromCodeTest_Success_TestCaseSource))]
        public static void FromCodeTest_Success(int code, DatabaseDataNamingType expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingType.FromCode(code),
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
                execFunc: () => DatabaseDataNamingType.FromCode(3),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
