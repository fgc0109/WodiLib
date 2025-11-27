using System;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class FieldValueListValidationHelperTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region ValidateUnifiedFieldType

        public static readonly object[][] ValidateUnifiedFieldTypeTest_Success_TestCaseSource =
        {
            // [target, type]
            new object[] { new[] { new DatabaseFieldValue(0) }, DatabaseFieldType.Int },
            new object[] { new[] { new DatabaseFieldValue("0") }, DatabaseFieldType.String },
        };

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateUnifiedFieldTypeTest_Success_TestCaseSource))]
        public static void ValidateUnifiedFieldTypeTest_Success(
            IEnumerable<DatabaseFieldValue> target,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionSuccess(() => FieldValueListValidationHelper.ValidateUnifiedFieldType(
                    (nameof(target), target),
                    type
                )
            );
        }

        /// <summary>
        ///     target, type が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("target")]
        [TestCase("type")]
        public static void ValidateUnifiedFieldTypeTest_Failure_ArgumentNull(string nullArgName)
        {
            NamedValue<IEnumerable<DatabaseFieldValue>> target = nullArgName == nameof(target)
                ? null!
                : new NamedValue<IEnumerable<DatabaseFieldValue>>(nameof(target), new[] { new DatabaseFieldValue(0) });
            DatabaseFieldType type = nullArgName == nameof(type)
                ? null!
                : DatabaseFieldType.Int;
            staticActionTestHelper.StaticActionFailure(
                () => FieldValueListValidationHelper.ValidateUnifiedFieldType(target, type),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        public static readonly object[][] ValidateUnifiedFieldTypeTest_Failure_FieldTypeNotMatch_TestCaseSource =
        {
            // [target, type]
            new object[] { new[] { new DatabaseFieldValue(0) }, DatabaseFieldType.String },
            new object[] { new[] { new DatabaseFieldValue("0"), new DatabaseFieldValue(0) }, DatabaseFieldType.String },
            new object[] { new[] { new DatabaseFieldValue("0") }, DatabaseFieldType.Int },
            new object[] { new[] { new DatabaseFieldValue(0), new DatabaseFieldValue("0") }, DatabaseFieldType.Int },
        };

        /// <summary>
        ///     src の値種別と type が異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCaseSource(nameof(ValidateUnifiedFieldTypeTest_Failure_FieldTypeNotMatch_TestCaseSource))]
        public static void ValidateUnifiedFieldTypeTest_Failure_FieldTypeNotMatch(
            IEnumerable<DatabaseFieldValue> target,
            DatabaseFieldType type
        )
        {
            staticActionTestHelper.StaticActionFailure(
                () => FieldValueListValidationHelper.ValidateUnifiedFieldType(
                    (nameof(target), target),
                    type
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion
    }
}
