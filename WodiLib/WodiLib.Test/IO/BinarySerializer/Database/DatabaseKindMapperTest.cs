using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.IO;
using WodiLib.Test.Tools;

namespace WodiLib.Test.IO.BinarySerializer.Database
{
    [TestFixture]
    public class DatabaseKindMapperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region FromSpecialArgCode

        private static readonly object[][] FromSpecialArgCodeTest_Success_TestCaseSource =
        {
            // [value, expected]
            new object[] { 0, DatabaseKind.System },
            new object[] { 1, DatabaseKind.User },
            new object[] { 2, DatabaseKind.Changeable },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromSpecialArgCodeTest_Success_TestCaseSource))]
        public static void FromSpecialArgCodeTest_Success(int value, DatabaseKind expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseKindMapper.FromSpecialArgCode(value),
                resultValueVerifier: ValueVerifier<DatabaseKind>.AreEquals(expected)
            );
        }

        /// <summary>
        ///     value が不適切な値の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(3)]
        public static void FromSpecialArgCodeTest_Failure_UnknownValue(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseKindMapper.FromSpecialArgCode(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region FromDBDataSettingTypeCode

        private static readonly object[][] FromDBDataSettingTypeCodeTest_Success_TestCaseSource =
        {
            // [value, expected]
            new object[] { 1, DatabaseKind.System },
            new object[] { 2, DatabaseKind.User },
            new object[] { 3, DatabaseKind.Changeable },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(FromDBDataSettingTypeCodeTest_Success_TestCaseSource))]
        public static void FromDBDataSettingTypeCodeTest_Success(int value, DatabaseKind expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseKindMapper.FromDBDataSettingTypeCode(value),
                resultValueVerifier: ValueVerifier<DatabaseKind>.AreEquals(expected)
            );
        }

        /// <summary>
        ///     value が不適切な値の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(4)]
        public static void FromDBDataSettingTypeCodeTest_Failure_UnknownValue(int value)
        {
            staticFunctionTestHelper.StaticFuncFailure(
                execFunc: () => DatabaseKindMapper.FromDBDataSettingTypeCode(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region ToDBDataSettingTypeCode

        private static readonly object[][] ToDBDataSettingTypeCodeTest_Success_TestCaseSource =
        {
            // [value, expected]
            new object[] { DatabaseKind.System, 1 },
            new object[] { DatabaseKind.User, 2 },
            new object[] { DatabaseKind.Changeable, 3 },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        [TestCaseSource(nameof(ToDBDataSettingTypeCodeTest_Success_TestCaseSource))]
        public static void ToDBDataSettingTypeCodeTest_Success(DatabaseKind value, int expected)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseKindMapper.ToDBDataSettingTypeCode(value),
                resultValueVerifier: ValueVerifier<int>.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
