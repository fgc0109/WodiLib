using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseTypeTableValidatorTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region SetColumn

        /// <summary>
        ///     すべての列要素の値種別が変更可能な値種別の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        public static void SetColumnTest_Success()
        {
            var settings = 2.Iterate(c =>
                TestData.INIT_ROW_LENGTH.Iterate(r => new DatabaseFieldValue(r * 1000 + c))
            );
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: instance => instance.SetColumn(("columnIndex", 0), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     変更不可能な値種別が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        public static void SetColumnTest_Failure_InvalidFieldType()
        {
            var settings = 2.Iterate(c =>
                TestData.INIT_ROW_LENGTH.Iterate(r => new DatabaseFieldValue($"{r * 1000 + c}"))
            );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.SetColumn(("columnIndex", 0), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region OverwriteColumn

        /// <summary>
        ///     すべての列要素の値種別が変更可能な値種別の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        public static void OverwriteColumnTest_Success()
        {
            var settings = 2.Iterate(c =>
                TestData.INIT_ROW_LENGTH.Iterate(r => new DatabaseFieldValue(r * 1000 + c))
            );
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: instance => instance.OverwriteColumn(("columnIndex", 0), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     変更不可能な値種別が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        public static void OverwriteColumnTest_Failure_InvalidFieldType()
        {
            var settings = 2.Iterate(c =>
                TestData.INIT_ROW_LENGTH.Iterate(r => new DatabaseFieldValue($"{r * 1000 + c}"))
            );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.OverwriteColumn(("columnIndex", 0), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region TestClass

        private static DatabaseTypeTableValidator GetTestInstance(
            int rowCount = TestData.INIT_ROW_LENGTH,
            int columnCount = TestData.INIT_COLUMN_LENGTH,
            DatabaseTypeTableValidator.DatabaseFieldTypeChangeValidator? fieldTypeChangeValidator = null
        )
        {
            return new DatabaseTypeTableValidator(
                rowCountGetter: () => rowCount,
                columnCountGetter: () => columnCount,
                fieldTypesGetter: () => TestData.InitFieldTypes.ToArray(),
                fieldTypeChangeValidator: fieldTypeChangeValidator ?? ValidateDatabaseFieldTypeChange
            );
        }

        private static class TestData
        {
            public const int INIT_ROW_LENGTH = 5;
            public const int INIT_COLUMN_LENGTH = 5;

            public static IReadOnlyList<DatabaseFieldType> InitFieldTypes =>
                INIT_COLUMN_LENGTH.Iterate(_ => DatabaseFieldType.Int).ToArray();
        }

        private static bool ValidateDatabaseFieldTypeChange(FieldId fieldId, DatabaseFieldType type)
            => type == DatabaseFieldType.Int;

        #endregion
    }
}
