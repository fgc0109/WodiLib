using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseDataNamingTypeMapperTest : TestFixtureBase
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
            new object[] { 0, DatabaseDataNamingType.Manual },
            new object[] { 1, DatabaseDataNamingType.FirstStringData },
            new object[] { 2, DatabaseDataNamingType.EqualBefore },
            new object[] { 10000, DatabaseDataNamingType.DesignatedType },
            new object[] { 24567, DatabaseDataNamingType.DesignatedType },
            new object[] { 39999, DatabaseDataNamingType.DesignatedType },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromSettingsValueTest_Success_TestCaseSource))]
        public static void FromSettingsValueTest_Success(int value, DatabaseDataNamingType expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataNamingTypeMapper.FromSettingsValue(value),
                resultValueVerifier: ValueVerifier<DatabaseDataNamingType>.AreEquals(expected)
            );
        }

        /// <summary>
        ///     value が不適切な値の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(-12345)]
        [TestCase(-9999)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(3)]
        [TestCase(9999)]
        [TestCase(40000)]
        public static void FromSettingsValueTest_Failure_UnknownValue(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseDataNamingTypeMapper.FromSettingsValue(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion
    }
}
