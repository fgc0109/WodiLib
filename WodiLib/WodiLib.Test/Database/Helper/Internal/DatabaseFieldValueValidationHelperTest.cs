using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldValueValidationHelperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region ValidateMatchFieldType

        public static readonly object[][] ValidateMatchFieldTypeTest_Success_TestCaseSource =
        {
            // [src, type]
            new object[] { new DatabaseFieldValue(0), DatabaseFieldType.Int },
            new object[] { new DatabaseFieldValue("0"), DatabaseFieldType.String },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateMatchFieldTypeTest_Success_TestCaseSource))]
        public static void ValidateMatchFieldTypeTest_Success(DatabaseFieldValue src, DatabaseFieldType type)
        {
            staticActionTestHelper.StaticActionSuccess(() => DatabaseFieldValueValidationHelper.ValidateMatchFieldType(
                    (nameof(src), src),
                    type
                )
            );
        }

        /// <summary>
        ///     src, type が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("src")]
        [TestCase("type")]
        public static void ValidateMatchFieldTypeTest_Failure_ArgumentNull(string nullArgName)
        {
            NamedValue<DatabaseFieldValue> src = nullArgName == nameof(src)
                ? null!
                : new NamedValue<DatabaseFieldValue>(nameof(src), new DatabaseFieldValue(0));
            DatabaseFieldType type = nullArgName == nameof(type)
                ? null!
                : DatabaseFieldType.Int;
            staticActionTestHelper.StaticActionFailure(
                () => DatabaseFieldValueValidationHelper.ValidateMatchFieldType(src, type),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        public static readonly object[][] ValidateMatchFieldTypeTest_Failure_FieldTypeNotMatch_TestCaseSource =
        {
            // [src, type]
            new object[] { new DatabaseFieldValue(0), DatabaseFieldType.String },
            new object[] { new DatabaseFieldValue("0"), DatabaseFieldType.Int },
        };

        /// <summary>
        ///     src の値種別と type が異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateMatchFieldTypeTest_Failure_FieldTypeNotMatch_TestCaseSource))]
        public static void ValidateMatchFieldTypeTest_Failure_FieldTypeNotMatch(
            DatabaseFieldValue src,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionFailure(
                () => DatabaseFieldValueValidationHelper.ValidateMatchFieldType(
                    (nameof(src), src),
                    type
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion
    }
}
