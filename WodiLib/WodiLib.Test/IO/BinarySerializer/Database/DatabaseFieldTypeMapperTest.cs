using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseFieldTypeMapperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region FromSettingsValue

        private static readonly object[][] FromSettingsValueTest_Success_TestCaseSource =
        {
            // [value, expected]
            new object[] { 1000, DatabaseFieldType.Int },
            new object[] { 1999, DatabaseFieldType.Int },
            new object[] { 2000, DatabaseFieldType.String },
            new object[] { 2999, DatabaseFieldType.String },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromSettingsValueTest_Success_TestCaseSource))]
        public static void FromSettingsValueTest_Success(int value, DatabaseFieldType expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseFieldTypeMapper.FromSettingsValue(value),
                resultValueVerifier: ValueVerifier<DatabaseFieldType>.AreEquals(expected)
            );
        }

        /// <summary>
        ///     value が不適切な値の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(999)]
        [TestCase(3000)]
        public static void FromSettingsValueTest_Failure_UnknownValue(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseFieldTypeMapper.FromSettingsValue(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
