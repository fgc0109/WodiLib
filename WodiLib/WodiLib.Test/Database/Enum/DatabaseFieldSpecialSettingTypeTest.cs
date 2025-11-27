using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Enum
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingTypeTest : TestFixtureBase
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
            Assert.AreEqual(DatabaseFieldSpecialSettingType.Normal.Id, "Normal");
            Assert.AreEqual(DatabaseFieldSpecialSettingType.LoadFile.Id, "LoadFile");
            Assert.AreEqual(DatabaseFieldSpecialSettingType.ReferDatabase.Id, "ReferDatabase");
            Assert.AreEqual(DatabaseFieldSpecialSettingType.Manual.Id, "Manual");
        }

        /// <summary>
        ///     Code が正しく取得できること。
        /// </summary>
        [Test]
        public static void ConstValueTest_Code()
        {
            Assert.AreEqual(DatabaseFieldSpecialSettingType.Normal.Code, 0);
            Assert.AreEqual(DatabaseFieldSpecialSettingType.LoadFile.Code, 1);
            Assert.AreEqual(DatabaseFieldSpecialSettingType.ReferDatabase.Code, 2);
            Assert.AreEqual(DatabaseFieldSpecialSettingType.Manual.Code, 3);
        }

        #endregion

        #region Methods

        #region FromCode

        private static readonly object[][] FromByteTest_Success_CaseSource =
        {
            // [byte, expected]
            new object[] { (byte)0, DatabaseFieldSpecialSettingType.Normal },
            new object[] { (byte)1, DatabaseFieldSpecialSettingType.LoadFile },
            new object[] { (byte)2, DatabaseFieldSpecialSettingType.ReferDatabase },
            new object[] { (byte)3, DatabaseFieldSpecialSettingType.Manual },
        };

        /// <summary>
        ///     Code 値から DatabaseFieldSpecialSettingType インスタンスが正しく取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromByteTest_Success_CaseSource))]
        public static void FromByteTest_Success(byte code, DatabaseFieldSpecialSettingType expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldSpecialSettingType.FromByte(code),
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
                execFunc: () => DatabaseFieldSpecialSettingType.FromByte(4),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
