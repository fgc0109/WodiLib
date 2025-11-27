using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Enum
{
    [TestFixture]
    public class DatabaseFieldTypeTest : TestFixtureBase
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
            Assert.AreEqual(DatabaseFieldType.Int.Id, "Int");
            Assert.AreEqual(DatabaseFieldType.String.Id, "String");
        }

        /// <summary>
        ///     Code が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Code()
        {
            Assert.AreEqual(DatabaseFieldType.Int.Code, 1);
            Assert.AreEqual(DatabaseFieldType.String.Code, 2);
        }

        /// <summary>
        ///     TypeOrderStart が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_TypeOrderStart()
        {
            Assert.AreEqual(DatabaseFieldType.Int.TypeOrderStart, 1000);
            Assert.AreEqual(DatabaseFieldType.String.TypeOrderStart, 2000);
        }

        #endregion

        #region Methods

        #region FromCode

        private static readonly object[][] FromCodeTest_Success_TestCaseSource =
        {
            // [code, expected]
            new object[] { 1, DatabaseFieldType.Int },
            new object[] { 2, DatabaseFieldType.String },
        };

        /// <summary>
        ///     Code 値から DatabaseFieldType インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromCodeTest_Success_TestCaseSource))]
        public static void FromCodeTest_Success(int code, DatabaseFieldType expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldType.FromCode(code),
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
                execFunc: () => DatabaseFieldType.FromCode(0),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
