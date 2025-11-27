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
    public class DatabaseDataTableValidatorTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constructor

        /// <summary>
        ///     引数 initItems のデータ数が MinDataCapacity 以上 MaxDataCapacity 以下
        ///     かつ項目数が MinFieldCapacity 以上 MaxFieldCapacity 以下の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(TestData.MIN_ROW_CAPACITY, TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MIN_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        public static void ConstructorTest_Success(int initRowLength, int initColumnLength)
        {
            var initSettings = GetRowSettingsListForConstructorTest(initRowLength, initColumnLength);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings))
            );
        }

        /// <summary>
        ///     引数 initItems が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_ArgumentNull()
        {
            IDatabaseDataTableSettings initSettings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     initItems のデータ数が MinDataCapacity より少ない場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_ShorterData()
        {
            var initSettings = GetRowSettingsListForConstructorTest(
                TestData.MIN_ROW_CAPACITY - 1,
                TestData.INIT_COLUMN_LENGTH
            );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initItems のデータ数が MaxDataCapacity より多い場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_LongerData()
        {
            var initSettings = GetRowSettingsListForConstructorTest(
                TestData.MAX_ROW_CAPACITY + 1,
                TestData.INIT_COLUMN_LENGTH
            );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initItems の項目数が MinFieldCapacity より少ない場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Ignore("最小項目数 = 0 のためテスト不可")]
        public static void ConstructorTest_Failure_ShorterField()
        {
        }

        /// <summary>
        ///     initItems の項目数が MaxFieldCapacity より多い場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_LongerField()
        {
            var initSettings = GetRowSettingsListForConstructorTest(
                TestData.MAX_ROW_CAPACITY,
                TestData.MAX_COLUMN_CAPACITY + 1
            );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        private static IDatabaseDataTableSettings GetRowSettingsListForConstructorTest(
            int rowLength,
            int columnLength
        )
            => new DatabaseDataTableSettings(
                rowLength.Iterate<IDatabaseDataRowSettings>(dataId => new DatabaseDataRowSettings(
                            columnLength.Iterate(fieldId => new DatabaseFieldValue(dataId * 100 + fieldId)).ToList()
                        )
                    )
                    .ToList()
            );

        #endregion

        #region TestClass

        private static class TestData
        {
            public const int MAX_ROW_CAPACITY = 10000;
            public const int MIN_ROW_CAPACITY = 1;
            public const int MAX_COLUMN_CAPACITY = 100;
            public const int MIN_COLUMN_CAPACITY = 0;
            public const int INIT_ROW_LENGTH = 5;
            public const int INIT_COLUMN_LENGTH = 4;

            public static IReadOnlyList<DatabaseFieldType> InitFieldTypes =>
                INIT_COLUMN_LENGTH.Iterate(_ => DatabaseFieldType.Int).ToArray();
        }

        private static DatabaseDataTableValidator<IDatabaseDataTableSettings, IDatabaseDataRowSettings> GetTestInstance(
            int rowCount = TestData.INIT_ROW_LENGTH,
            int columnCount = TestData.INIT_COLUMN_LENGTH,
            DatabaseFieldType[]? initFieldTypes = null
        )
        {
            return new DatabaseDataTableValidator<IDatabaseDataTableSettings, IDatabaseDataRowSettings>(
                rowCountGetter: () => rowCount,
                columnCountGetter: () => columnCount,
                fieldTypesGetter: () => initFieldTypes ?? TestData.InitFieldTypes.ToArray()
            );
        }

        #endregion
    }
}
