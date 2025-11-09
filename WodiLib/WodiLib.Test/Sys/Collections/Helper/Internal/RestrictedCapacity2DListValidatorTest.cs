using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys.Collections
{
    [TestFixture]
    public class RestrictedCapacity2DListValidatorTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Constructor

        /// <summary>
        ///     引数 initItems の行数・列数が MinRowCapacity/MinColumnCapacity 以上
        ///     MaxRowCapacity/MaxColumnCapacity 以下の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(TestData.MIN_ROW_CAPACITY, TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MIN_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        public static void ConstructorTest_Success(int rowCount, int columnCount)
        {
            var initItems = CreateTestData(rowCount, columnCount);
            var initSettings = new StubRestrictedCapacity2DListSettings(initItems.ToList());
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(rowCount, columnCount),
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
            IStubRestrictedCapacity2DListSettings initSettings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 initItems の列数が不揃いの場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_JuggedColumnSize()
        {
            var initItems = CreateJuggedTestData(TestData.DEFAULT_ROW_COUNT, TestData.DEFAULT_COLUMN_COUNT);
            var initSettings = new StubRestrictedCapacity2DListSettings(initItems.ToList());
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 initItems の行数が MinRowCapacity より少ない場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_ShorterRow()
        {
            var initItems = CreateTestData(TestData.MIN_ROW_CAPACITY - 1, TestData.DEFAULT_COLUMN_COUNT);
            var initSettings = new StubRestrictedCapacity2DListSettings(initItems.ToList());
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 initItems の行数が MaxRowCapacity より多い場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_LongerRow()
        {
            var initItems = CreateTestData(TestData.MAX_ROW_CAPACITY + 1, TestData.DEFAULT_COLUMN_COUNT);
            var initSettings = new StubRestrictedCapacity2DListSettings(initItems.ToList());
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 initItems の列数が MinColumnCapacity より少ない場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_ShorterColumn()
        {
            var initItems = CreateTestData(TestData.DEFAULT_ROW_COUNT, TestData.MIN_COLUMN_CAPACITY - 1);
            var initSettings = new StubRestrictedCapacity2DListSettings(initItems.ToList());
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 initItems の列数が MaxColumnCapacity より多い場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_LongerColumn()
        {
            var initItems = CreateTestData(TestData.DEFAULT_ROW_COUNT, TestData.MAX_COLUMN_CAPACITY + 1);
            var initSettings = new StubRestrictedCapacity2DListSettings(initItems.ToList());
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region GetRow

        /// <summary>
        ///     引数 rowIndex, count がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 0)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 1)]
        public static void GetRowTest_Success(int rowIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.GetRow((nameof(rowIndex), rowIndex), (nameof(count), count))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT)]
        public static void GetRowTest_Failure_OutOfRangeIndex(int rowIndex)
        {
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void GetRowTest_Failure_OutOfRangeCount(int count)
        {
            const int rowIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 rowIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 2)]
        public static void GetRowTest_Failure_OutOfRangeIndexAndCount(int rowIndex, int count)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     空リストから行を取得しようとした場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [Test]
        public static void GetRowTest_Failure_WhenEmpty()
        {
            const int rowIndex = 0;
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.GetRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region GetColumn

        /// <summary>
        ///     引数 columnIndex, count がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 0)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 1)]
        public static void GetColumnTest_Success(int columnIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.GetColumn((nameof(columnIndex), columnIndex), (nameof(count), count))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT)]
        public static void GetColumnTest_Failure_OutOfRangeIndex(int columnIndex)
        {
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void GetColumnTest_Failure_OutOfRangeCount(int count)
        {
            const int columnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 columnIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 2)]
        public static void GetColumnTest_Failure_OutOfRangeIndexAndCount(int columnIndex, int count)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     空リストから列を取得しようとした場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [Test]
        public static void GetColumnTest_Failure_WhenEmpty()
        {
            const int columnIndex = 0;
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.GetColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region GetCell

        /// <summary>
        ///     引数 rowIndex, columnIndex がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT - 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 0)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, TestData.DEFAULT_COLUMN_COUNT - 1)]
        public static void GetCellTest_Success(int rowIndex, int columnIndex)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.GetCell((nameof(rowIndex), rowIndex), (nameof(columnIndex), columnIndex))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT)]
        public static void GetCellTest_Failure_OutOfRangeRowIndex(int rowIndex)
        {
            const int columnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetCell((nameof(rowIndex), rowIndex), (nameof(columnIndex), columnIndex)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT)]
        public static void GetCellTest_Failure_OutOfRangeColumnIndex(int columnIndex)
        {
            const int rowIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.GetCell((nameof(rowIndex), rowIndex), (nameof(columnIndex), columnIndex)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     空リストからセルを取得しようとした場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [Test]
        public static void GetCellTest_Failure_WhenEmpty()
        {
            const int rowIndex = 0;
            const int columnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.GetCell((nameof(rowIndex), rowIndex), (nameof(columnIndex), columnIndex)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region SetRow

        /// <summary>
        ///     引数 rowIndex, settings がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 1)]
        [TestCase(0, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 1)]
        public static void SetRowTest_Success(int rowIndex, int settingsCount)
        {
            var settings = CreateTestData(settingsCount, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void SetRowTest_Failure_ArgumentNull()
        {
            const int rowIndex = 0;
            IEnumerable<IStubRestrictedCapacityListSettings> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT)]
        public static void SetRowTest_Failure_OutOfRangeIndex(int rowIndex)
        {
            var settings = CreateTestData(1, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 rowIndex, settings がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(0, TestData.DEFAULT_ROW_COUNT + 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 2)]
        public static void SetRowTest_Failure_OutOfRangeIndexAndSettingsCount(int rowIndex, int settingsCount)
        {
            var settings = CreateTestData(settingsCount, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 settings の列数がリストの現在の列数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void SetRowTest_Failure_InvalidColumnSize(int columnCount)
        {
            const int rowIndex = 1;
            var settings = CreateTestData(1, columnCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region SetColumn

        /// <summary>
        ///     引数 columnIndex, settings がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 1)]
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 1)]
        public static void SetColumnTest_Success(int columnIndex, int settingsCount)
        {
            var settings = CreateColumnTestData(settingsCount, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.SetColumn((nameof(columnIndex), columnIndex), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void SetColumnTest_Failure_ArgumentNull()
        {
            const int columnIndex = 0;
            IEnumerable<IStubModelSettings> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT)]
        public static void SetColumnTest_Failure_OutOfRangeIndex(int columnIndex)
        {
            var settings = CreateColumnTestData(1, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 columnIndex, settings がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT + 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 2)]
        public static void SetColumnTest_Failure_OutOfRangeIndexAndSettingsCount(int columnIndex, int settingsCount)
        {
            var settings = CreateColumnTestData(settingsCount, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 settings の行数がリストの現在の行数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void SetColumnTest_Failure_InvalidRowSize(int rowCount)
        {
            const int columnIndex = 1;
            var settings = CreateColumnTestData(1, rowCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region SetCell

        /// <summary>
        ///     引数 rowIndex, columnIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, TestData.DEFAULT_COLUMN_COUNT - 1)]
        public static void SetCellTest_Success(int rowIndex, int columnIndex)
        {
            var settings = new StubModelSettings { StringValue = "test" };
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.SetCell(
                    (nameof(rowIndex), rowIndex),
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                )
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void SetCellTest_Failure_ArgumentNull()
        {
            const int rowIndex = 0;
            const int columnIndex = 0;
            IStubModelSettings settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetCell(
                    (nameof(rowIndex), rowIndex),
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT)]
        public static void SetCellTest_Failure_OutOfRangeRowIndex(int rowIndex)
        {
            const int columnIndex = 0;
            var settings = new StubModelSettings { StringValue = "test" };
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetCell(
                    (nameof(rowIndex), rowIndex),
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT)]
        public static void SetCellTest_Failure_OutOfRangeColumnIndex(int columnIndex)
        {
            const int rowIndex = 0;
            var settings = new StubModelSettings { StringValue = "test" };
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.SetCell(
                    (nameof(rowIndex), rowIndex),
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region InsertRow

        /// <summary>
        ///     引数 rowIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.MAX_ROW_CAPACITY - TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT, 0)]
        [TestCase(TestData.DEFAULT_ROW_COUNT, TestData.MAX_ROW_CAPACITY - TestData.DEFAULT_ROW_COUNT)]
        public static void InsertRowTest_Success_WhenNotEmpty(int rowIndex, int settingsCount)
        {
            var settings = CreateTestData(settingsCount, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     空リストに対して引数 rowIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        public static void InsertRowTest_Success_WhenEmpty(int settingsCount, int columnCount)
        {
            const int rowIndex = 0;
            var settings = CreateTestData(settingsCount, columnCount);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void InsertRowTest_Failure_ArgumentNull()
        {
            const int rowIndex = 0;
            IEnumerable<IStubRestrictedCapacityListSettings> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void InsertRowTest_Failure_OutOfRangeIndex(int rowIndex)
        {
            var settings = CreateTestData(1, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 settings の列数がリストの現在の列数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void InsertRowTest_Failure_InvalidColumnSize(int columnCount)
        {
            const int rowIndex = 1;
            var settings = CreateTestData(1, columnCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     空リストに対して引数 settings の列数が不揃いな場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void InsertRowTest_Failure_JuggedColumnSize()
        {
            const int rowIndex = 0;
            var settings = CreateJuggedTestData(TestData.DEFAULT_ROW_COUNT, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     要素追加によって要素数が MaxRowCapacity を超える場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void InsertRowTest_Failure_LongerItem()
        {
            const int rowIndex = 0;
            const int itemsLength = TestData.MAX_ROW_CAPACITY - TestData.DEFAULT_ROW_COUNT + 1;
            var settings = CreateTestData(itemsLength, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region InsertColumn

        /// <summary>
        ///     空ではないリストに対して引数 columnIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.MAX_COLUMN_CAPACITY - TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT, 0)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT, TestData.MAX_COLUMN_CAPACITY - TestData.DEFAULT_COLUMN_COUNT)]
        public static void InsertColumnTest_Success_WhenNotEmpty(int columnIndex, int settingsCount)
        {
            var settings = CreateColumnTestData(settingsCount, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.InsertColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                )
            );
        }

        /// <summary>
        ///     空リストの場合、
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void InsertColumnTest_Failure_WhenEmpty()
        {
            const int columnIndex = 0;
            var settings = CreateColumnTestData(1, 1);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.InsertColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void InsertColumnTest_Failure_ArgumentNull()
        {
            const int columnIndex = 0;
            IEnumerable<IStubModelSettings> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void InsertColumnTest_Failure_OutOfRangeIndex(int columnIndex)
        {
            var settings = CreateColumnTestData(1, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 settings の行数がリストの現在の行数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void InsertColumnTest_Failure_InvalidRowSize(int rowCount)
        {
            const int columnIndex = 1;
            var settings = CreateColumnTestData(1, rowCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     要素追加によって要素数が MaxColumnCapacity を超える場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void InsertColumnTest_Failure_LongerItem()
        {
            const int columnIndex = 0;
            const int itemsLength = TestData.MAX_COLUMN_CAPACITY - TestData.DEFAULT_COLUMN_COUNT + 1;
            var settings = CreateColumnTestData(itemsLength, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.InsertColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region OverwriteRow

        /// <summary>
        ///     空ではないリストに対して引数 rowIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.MAX_ROW_CAPACITY)]
        [TestCase(TestData.DEFAULT_ROW_COUNT, 0)]
        [TestCase(TestData.DEFAULT_ROW_COUNT, TestData.MAX_ROW_CAPACITY - TestData.DEFAULT_ROW_COUNT)]
        public static void OverwriteRowTest_Success_WhenNotEmpty(int rowIndex, int settingsCount)
        {
            var settings = CreateTestData(settingsCount, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     空リストに対して引数 rowIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(1, TestData.MAX_COLUMN_CAPACITY)]
        [TestCase(1, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        public static void OverwriteRowTest_Success_WhenEmpty(int settingsCount, int columnCount)
        {
            const int rowIndex = 0;
            var settings = CreateTestData(settingsCount, columnCount);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Failure_ArgumentNull()
        {
            const int rowIndex = 0;
            IEnumerable<IStubRestrictedCapacityListSettings> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     空リストに対して引数 settings の列数が不揃いな場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Failure_JuggedColumnSize()
        {
            const int rowIndex = 0;
            var settings = CreateJuggedTestData(TestData.DEFAULT_ROW_COUNT, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void OverwriteRowTest_Failure_OutOfRangeIndex(int rowIndex)
        {
            var settings = CreateTestData(0, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 settings の列数がリストの現在の列数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void OverwriteRowTest_Failure_InvalidColumnSize(int columnCount)
        {
            const int rowIndex = 1;
            var settings = CreateTestData(1, columnCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     要素追加によって要素数が MaxRowCapacity を超える場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Failure_LongerItem()
        {
            const int rowIndex = 0;
            var settings = CreateTestData(TestData.MAX_ROW_CAPACITY + 1, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteRow((nameof(rowIndex), rowIndex), (nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region OverwriteColumn

        /// <summary>
        ///     空ではないリストに対して引数 columnIndex, settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT, 0)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT, 2)]
        public static void OverwriteColumnTest_Success_WhenNotEmpty(int columnIndex, int settingsCount)
        {
            var settings = CreateColumnTestData(settingsCount, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                )
            );
        }

        /// <summary>
        ///     空リストの場合、
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 0)]
        [TestCase(2, 3)]
        public static void OverwriteColumnTest_Failure_WhenEmpty(int settingsCount, int rowCount)
        {
            const int columnIndex = 0;
            var settings = CreateColumnTestData(settingsCount, rowCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.OverwriteColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteColumnTest_Failure_ArgumentNull()
        {
            const int columnIndex = 0;
            IEnumerable<IEnumerable<IStubModelSettings>> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void OverwriteColumnTest_Failure_OutOfRangeIndex(int columnIndex)
        {
            var settings = CreateColumnTestData(0, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 settings の行数がリストの現在の行数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void OverwriteColumnTest_Failure_InvalidRowSize(int rowCount)
        {
            const int columnIndex = 1;
            var settings = CreateColumnTestData(1, rowCount);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     要素追加によって要素数が MaxColumnCapacity を超える場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteColumnTest_Failure_LongerItem()
        {
            const int columnIndex = 0;
            var settings = CreateColumnTestData(TestData.MAX_COLUMN_CAPACITY + 1, TestData.DEFAULT_ROW_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.OverwriteColumn(
                    (nameof(columnIndex), columnIndex),
                    (nameof(settings), settings)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region MoveRow

        /// <summary>
        ///     引数 oldRowIndex, newRowIndex, count がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(0, TestData.DEFAULT_ROW_COUNT - 1, 0)]
        [TestCase(0, TestData.DEFAULT_ROW_COUNT - 1, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 0, 1)]
        public static void MoveRowTest_Success(int oldRowIndex, int newRowIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.MoveRow(
                    (nameof(oldRowIndex), oldRowIndex),
                    (nameof(newRowIndex), newRowIndex),
                    (nameof(count), count)
                )
            );
        }

        /// <summary>
        ///     引数 oldRowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT)]
        public static void MoveRowTest_Failure_OutOfRangeOldIndex(int oldRowIndex)
        {
            const int newRowIndex = 0;
            const int count = 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveRow(
                    (nameof(oldRowIndex), oldRowIndex),
                    (nameof(newRowIndex), newRowIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 newRowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void MoveRowTest_Failure_OutOfRangeNewIndex(int newRowIndex)
        {
            const int oldRowIndex = 0;
            const int count = 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveRow(
                    (nameof(oldRowIndex), oldRowIndex),
                    (nameof(newRowIndex), newRowIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void MoveRowTest_Failure_OutOfRangeCount(int count)
        {
            const int oldRowIndex = 0;
            const int newRowIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveRow(
                    (nameof(oldRowIndex), oldRowIndex),
                    (nameof(newRowIndex), newRowIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 oldRowIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 2)]
        public static void MoveRowTest_Failure_OutOfRangeOldIndexAndCount(int oldRowIndex, int count)
        {
            const int newRowIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveRow(
                    (nameof(oldRowIndex), oldRowIndex),
                    (nameof(newRowIndex), newRowIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 newRowIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 2)]
        public static void MoveRowTest_Failure_OutOfRangeNewIndexAndCount(int newRowIndex, int count)
        {
            const int oldRowIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveRow(
                    (nameof(oldRowIndex), oldRowIndex),
                    (nameof(newRowIndex), newRowIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region MoveColumn

        /// <summary>
        ///     引数 oldColumnIndex, newColumnIndex, count がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT - 1, 0)]
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT - 1, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 0, 1)]
        public static void MoveColumnTest_Success(int oldColumnIndex, int newColumnIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.MoveColumn(
                    (nameof(oldColumnIndex), oldColumnIndex),
                    (nameof(newColumnIndex), newColumnIndex),
                    (nameof(count), count)
                )
            );
        }

        /// <summary>
        ///     引数 oldColumnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT)]
        public static void MoveColumnTest_Failure_OutOfRangeOldIndex(int oldColumnIndex)
        {
            const int newColumnIndex = 0;
            const int count = 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveColumn(
                    (nameof(oldColumnIndex), oldColumnIndex),
                    (nameof(newColumnIndex), newColumnIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 newColumnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void MoveColumnTest_Failure_OutOfRangeNewIndex(int newColumnIndex)
        {
            const int oldColumnIndex = 0;
            const int count = 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveColumn(
                    (nameof(oldColumnIndex), oldColumnIndex),
                    (nameof(newColumnIndex), newColumnIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void MoveColumnTest_Failure_OutOfRangeCount(int count)
        {
            const int oldColumnIndex = 0;
            const int newColumnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveColumn(
                    (nameof(oldColumnIndex), oldColumnIndex),
                    (nameof(newColumnIndex), newColumnIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 oldColumnIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 2)]
        public static void MoveColumnTest_Failure_OutOfRangeOldIndexAndCount(int oldColumnIndex, int count)
        {
            const int newColumnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveColumn(
                    (nameof(oldColumnIndex), oldColumnIndex),
                    (nameof(newColumnIndex), newColumnIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 newColumnIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 2)]
        public static void MoveColumnTest_Failure_OutOfRangeNewIndexAndCount(int newColumnIndex, int count)
        {
            const int oldColumnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.MoveColumn(
                    (nameof(oldColumnIndex), oldColumnIndex),
                    (nameof(newColumnIndex), newColumnIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region RemoveRow

        /// <summary>
        ///     引数 rowIndex, count がリスト範囲内であり、除去した結果要素数が MinRowCapacity 未満とならない場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.DEFAULT_ROW_COUNT - TestData.MIN_ROW_CAPACITY)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 0)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 1)]
        public static void RemoveRowTest_Success(int rowIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count))
            );
        }

        /// <summary>
        ///     引数 rowIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT)]
        public static void RemoveRowTest_Failure_OutOfRangeIndex(int rowIndex)
        {
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void RemoveRowTest_Failure_OutOfRangeCount(int count)
        {
            const int rowIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 rowIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_ROW_COUNT)]
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1, 2)]
        public static void RemoveRowTest_Failure_OutOfRangeIndexAndCount(int rowIndex, int count)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     要素を除去した結果要素数が MinRowCapacity 未満となる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void RemoveRowTest_Failure_ShorterItem()
        {
            const int rowIndex = 0;
            const int count = TestData.DEFAULT_ROW_COUNT - TestData.MIN_ROW_CAPACITY + 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveRow((nameof(rowIndex), rowIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region RemoveColumn

        /// <summary>
        ///     引数 columnIndex, count がリスト範囲内であり、除去した結果要素数が MinColumnCapacity 未満とならない場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.DEFAULT_COLUMN_COUNT - TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 0)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 1)]
        public static void RemoveColumnTest_Success(int columnIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count))
            );
        }

        /// <summary>
        ///     引数 columnIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT)]
        public static void RemoveColumnTest_Failure_OutOfRangeIndex(int columnIndex)
        {
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void RemoveColumnTest_Failure_OutOfRangeCount(int count)
        {
            const int columnIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 columnIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.DEFAULT_COLUMN_COUNT)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1, 2)]
        public static void RemoveColumnTest_Failure_OutOfRangeIndexAndCount(int columnIndex, int count)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     要素を除去した結果要素数が MinColumnCapacity 未満となる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void RemoveColumnTest_Failure_ShorterItem()
        {
            const int columnIndex = 0;
            const int count = TestData.DEFAULT_COLUMN_COUNT - TestData.MIN_COLUMN_CAPACITY + 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.RemoveColumn((nameof(columnIndex), columnIndex), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region AdjustRowLength

        /// <summary>
        ///     引数 length が MinRowCapacity 以上 MaxRowCapacity 以下である場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(TestData.MIN_ROW_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY)]
        public static void AdjustRowLengthTest_Success(int length)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.AdjustRowLength((nameof(length), length))
            );
        }

        /// <summary>
        ///     引数 length が MinRowCapacity 未満または MaxRowCapacity を超える場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(TestData.MIN_ROW_CAPACITY - 1)]
        [TestCase(TestData.MAX_ROW_CAPACITY + 1)]
        public static void AdjustRowLengthTest_Failure_ArgumentOutOfRange(int length)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.AdjustRowLength((nameof(length), length)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region AdjustColumnLength

        /// <summary>
        ///     引数 length が MinColumnCapacity 以上 MaxColumnCapacity 以下の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_COLUMN_CAPACITY)]
        public static void AdjustColumnLengthTest_Success(int length)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.AdjustColumnLength((nameof(length), length))
            );
        }

        /// <summary>
        ///     引数 length が MinColumnCapacity 未満または MaxColumnCapacity を超える場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(TestData.MIN_COLUMN_CAPACITY - 1)]
        [TestCase(TestData.MAX_COLUMN_CAPACITY + 1)]
        public static void AdjustColumnLengthTest_Failure_ArgumentOutOfRange(int length)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.AdjustColumnLength((nameof(length), length)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Reset

        #region Settings - CanChangeSize

        /// <summary>
        ///     引数 settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(TestData.MIN_ROW_CAPACITY, TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MIN_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MIN_COLUMN_CAPACITY)]
        [TestCase(TestData.MAX_ROW_CAPACITY, TestData.MAX_COLUMN_CAPACITY)]
        public static void ResetTest_SettingsCanChangeSize_Success(int rowCount, int columnCount)
        {
            var settings = CreateTestData(rowCount, columnCount);
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(settings), settings))
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCanChangeSize_Failure_ArgumentNull()
        {
            IEnumerable<IStubRestrictedCapacityListSettings> settings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 settings の列数が不揃いな場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCanChangeSize_Failure_JuggedColumnSize()
        {
            var settings = CreateJuggedTestData(TestData.DEFAULT_ROW_COUNT, TestData.DEFAULT_COLUMN_COUNT);
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(rowCount: 0, columnCount: 0, minRowCapacity: 0, minColumnCapacity: 0),
                execAction: target => target.Reset((nameof(settings), settings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Settings - CannotChangeSize

        /// <summary>
        ///     引数 settings が有効な場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCannotChangeSize_Success()
        {
            var settings = CreateTestData(TestData.DEFAULT_ROW_COUNT, TestData.DEFAULT_COLUMN_COUNT);
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(settings), settings), canChangeSize)
            );
        }

        /// <summary>
        ///     引数 public static void SettingsDtoConstructorTest_Success_NoParam、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCannotChangeSize_Failure_ArgumentNull()
        {
            IEnumerable<IStubRestrictedCapacityListSettings> settings = null!;
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(settings), settings), canChangeSize),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 settings の行数が現在のリストの行数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_ROW_COUNT - 1)]
        [TestCase(TestData.DEFAULT_ROW_COUNT + 1)]
        public static void ResetTest_SettingsCannotChangeSize_Failure_InvalidRowSize(int rowCount)
        {
            var settings = CreateTestData(rowCount, TestData.DEFAULT_COLUMN_COUNT);
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(settings), settings), canChangeSize),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 settings の列数が現在のリストの列数と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.DEFAULT_COLUMN_COUNT - 1)]
        [TestCase(TestData.DEFAULT_COLUMN_COUNT + 1)]
        public static void ResetTest_SettingsCannotChangeSize_Failure_InvalidColumnSize(int columnCount)
        {
            var settings = CreateTestData(TestData.DEFAULT_ROW_COUNT, columnCount);
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(settings), settings), canChangeSize),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region NoParams

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void ResetTest_NoParams_Success()
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Reset()
            );
        }

        #endregion

        #endregion

        #region Clear

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void ClearTest_Success()
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Clear()
            );
        }

        #endregion

        #endregion

        #region TestClass

        private static RestrictedCapacity2DListValidator<IStubRestrictedCapacity2DListSettings,
                IStubRestrictedCapacityListSettings, IStubModelSettings>
            GetTestInstance(
                int rowCount = TestData.DEFAULT_ROW_COUNT,
                int columnCount = TestData.DEFAULT_COLUMN_COUNT,
                int minRowCapacity = TestData.MIN_ROW_CAPACITY,
                int maxRowCapacity = TestData.MAX_ROW_CAPACITY,
                int minColumnCapacity = TestData.MIN_COLUMN_CAPACITY,
                int maxColumnCapacity = TestData.MAX_COLUMN_CAPACITY
            )
        {
            return new RestrictedCapacity2DListValidator<IStubRestrictedCapacity2DListSettings,
                IStubRestrictedCapacityListSettings, IStubModelSettings>(
                rowCountGetter: () => rowCount,
                columnCountGetter: () => columnCount,
                minRowCapacityGetter: () => minRowCapacity,
                maxRowCapacityGetter: () => maxRowCapacity,
                minColumnCapacityGetter: () => minColumnCapacity,
                maxColumnCapacityGetter: () => maxColumnCapacity
            );
        }

        private static IEnumerable<IStubRestrictedCapacityListSettings> CreateTestData(int rowCount, int columnCount)
        {
            return rowCount.Iterate(r => new StubRestrictedCapacityListSettings(
                    columnCount.Iterate<IStubModelSettings>(c => new StubModelSettings { StringValue = $"item_{r}_{c}" }
                        )
                        .ToList()
                )
            );
        }

        private static IEnumerable<IEnumerable<IStubModelSettings>> CreateColumnTestData(int columnCount, int rowCount)
        {
            return columnCount.Iterate(c
                => rowCount.Iterate(r => new StubModelSettings { StringValue = $"item_{r}_{c}" })
            );
        }

        private static IEnumerable<IStubRestrictedCapacityListSettings> CreateJuggedTestData(
            int rowCount,
            int columnCount
        )
        {
            return rowCount.Iterate(r => r % 2 == 0
                ? CreateTestData(1, columnCount).First()
                : CreateTestData(1, columnCount + 1).First()
            );
        }

        private static class TestData
        {
            public const int MAX_ROW_CAPACITY = 10;
            public const int MIN_ROW_CAPACITY = 1;
            public const int MAX_COLUMN_CAPACITY = 7;
            public const int MIN_COLUMN_CAPACITY = 2;
            public const int DEFAULT_ROW_COUNT = 3;
            public const int DEFAULT_COLUMN_COUNT = 4;
        }

        #endregion
    }
}
