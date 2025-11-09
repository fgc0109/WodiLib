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
    public class FixedLengthListValidatorTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region Constructor

        /// <summary>
        ///     引数 initItems の要素数が MinCapacity 以上 MaxCapacity 以下の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(TestData.MIN_CAPACITY)]
        [TestCase(TestData.MAX_CAPACITY)]
        public static void ConstructorTest_Success(int initItemsLength)
        {
            var initSettings =
                new StubFixedLengthListSettings(
                    initItemsLength.Iterate<IStubModelSettings>(i => new StubModel(i.ToString())).ToArray()
                );
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
            IStubFixedLengthListSettings initSettings = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     initItems の要素数が MinCapacity より少ない場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_ShorterItem()
        {
            var initSettings =
                new StubFixedLengthListSettings(
                    (TestData.MIN_CAPACITY - 1).Iterate<IStubModelSettings>(i => new StubModel(i.ToString())).ToArray()
                );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initItems の要素数が MaxCapacity より多い場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_LongerItem()
        {
            var initSettings =
                new StubFixedLengthListSettings(
                    (TestData.MAX_CAPACITY + 1).Iterate<IStubModelSettings>(i => new StubModel(i.ToString())).ToArray()
                );
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Get

        /// <summary>
        ///     引数 index, count がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.INIT_LENGTH)]
        [TestCase(TestData.INIT_LENGTH - 1, 0)]
        [TestCase(TestData.INIT_LENGTH - 1, 1)]
        public static void GetTest_Success(int index, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Get((nameof(index), index), (nameof(count), count))
            );
        }

        /// <summary>
        ///     引数 index がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH)]
        public static void GetTest_Failure_OutOfRangeIndex(int index)
        {
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Get((nameof(index), index), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void GetTest_Failure_OutOfRangeCount(int count)
        {
            const int index = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Get((nameof(index), index), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 index, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.INIT_LENGTH)]
        [TestCase(TestData.INIT_LENGTH - 1, 2)]
        public static void GetTest_Failure_OutOfRangeIndexAndCount(int index, int count)
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Get((nameof(index), index), (nameof(count), count)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Set

        /// <summary>
        ///     引数 index, items がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.INIT_LENGTH)]
        [TestCase(TestData.INIT_LENGTH - 1, 0)]
        [TestCase(TestData.INIT_LENGTH - 1, 1)]
        public static void SetTest_Success(int index, int itemLength)
        {
            var items = itemLength.Iterate(i => new StubModel((i + 100).ToString()));
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Set((nameof(index), index), (nameof(items), items))
            );
        }

        /// <summary>
        ///     引数 items が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void SetTest_Failure_ArgumentNull()
        {
            const int index = 0;
            IEnumerable<IStubModelSettings> items = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Set((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 index がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH)]
        public static void SetTest_Failure_OutOfRangeIndex(int index)
        {
            var items = Array.Empty<StubModel>();
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Set((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 index, items がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(0, TestData.INIT_LENGTH + 1)]
        [TestCase(TestData.INIT_LENGTH - 1, 2)]
        public static void SetTest_Failure_OutOfRangeIndexAndItemLength(int index, int itemsLength)
        {
            var items = itemsLength.Iterate(i => new StubModel((i + 100).ToString()));
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Set((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Insert

        /// <summary>
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void InsertTest_Failure_InvalidOperation()
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Insert(("index", 0), ("items", null!)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #region Overwrite

        /// <summary>
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteTest_Failure_InvalidOperation()
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Overwrite(("index", 0), ("items", null!)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #region Move

        /// <summary>
        ///     引数 oldIndex, newIndex, count がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0, 0)]
        [TestCase(0, 0, TestData.INIT_LENGTH)]
        [TestCase(0, TestData.INIT_LENGTH - 1, 0)]
        [TestCase(0, TestData.INIT_LENGTH - 1, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(TestData.INIT_LENGTH - 1, 0, 1)]
        public static void MoveTest_Success(int oldIndex, int newIndex, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Move(
                    (nameof(oldIndex), oldIndex),
                    (nameof(newIndex), newIndex),
                    (nameof(count), count)
                )
            );
        }

        /// <summary>
        ///     引数 oldIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH)]
        public static void MoveTest_Failure_OutOfRangeOldIndex(int oldIndex)
        {
            const int newIndex = 0;
            const int count = 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Move(
                    (nameof(oldIndex), oldIndex),
                    (nameof(newIndex), newIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 newIndex がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void MoveTest_Failure_OutOfRangeNewIndex(int newIndex)
        {
            const int oldIndex = 0;
            const int count = 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Move(
                    (nameof(oldIndex), oldIndex),
                    (nameof(newIndex), newIndex),
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
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void MoveTest_Failure_OutOfRangeCount(int count)
        {
            const int oldIndex = 0;
            const int newIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Move(
                    (nameof(oldIndex), oldIndex),
                    (nameof(newIndex), newIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 oldIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.INIT_LENGTH)]
        [TestCase(TestData.INIT_LENGTH - 1, 2)]
        public static void MoveTest_Failure_OutOfRangeOldIndexAndCount(int oldIndex, int count)
        {
            const int newIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Move(
                    (nameof(oldIndex), oldIndex),
                    (nameof(newIndex), newIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数 newIndex, count がリスト範囲外の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(1, TestData.INIT_LENGTH)]
        [TestCase(TestData.INIT_LENGTH - 1, 2)]
        public static void MoveTest_Failure_OutOfRangeNewIndexAndCount(int newIndex, int count)
        {
            const int oldIndex = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Move(
                    (nameof(oldIndex), oldIndex),
                    (nameof(newIndex), newIndex),
                    (nameof(count), count)
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Remove

        /// <summary>
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void RemoveTest_Failure_InvalidOperation()
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Remove(("index", 0), ("count", 0)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #region AdjustLength

        /// <summary>
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void AdjustLengthTest_Failure_InvalidOperation()
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.AdjustLength(("length", TestData.INIT_LENGTH)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #region Reset

        #region Settings - CanChangeSize

        /// <summary>
        ///     canChangeSize が true の場合、
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCanChangeSize_Failure_InvalidOperation()
        {
            var items = TestData.INIT_LENGTH.Iterate(i => new StubModel(i.ToString()));
            const bool canChangeSize = true;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(items), items), canChangeSize),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #region Settings - CannotChangeSize

        /// <summary>
        ///     canChangeSize が false かつ
        ///     引数 items の要素数が Capacity と一致する場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCannotChangeSize_Success()
        {
            var items = TestData.INIT_LENGTH.Iterate(i => new StubModel(i.ToString()));
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(items), items), canChangeSize)
            );
        }

        /// <summary>
        ///     引数 items が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCannotChangeSize_Failure_ArgumentNull()
        {
            IEnumerable<IStubModelSettings> items = null!;
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(items), items), canChangeSize),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 items の要素数が Capacity と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(TestData.INIT_LENGTH - 1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void ResetTest_SettingsCannotChangeSize_Failure_InvalidItemSize(int itemLength)
        {
            var items = itemLength.Iterate(i => new StubModel(i.ToString()));
            const bool canChangeSize = false;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(items), items), canChangeSize),
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
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void ClearTest_Failure_InvalidOperation()
        {
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Clear(),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
            );
        }

        #endregion

        #endregion

        #region TestClass

        private static FixedLengthListValidator<IStubFixedLengthListSettings, IStubModelSettings> GetTestInstance(
            int count = TestData.INIT_LENGTH,
            int maxCapacity = TestData.MAX_CAPACITY,
            int minCapacity = TestData.MIN_CAPACITY
        )
        {
            return new FixedLengthListValidator<IStubFixedLengthListSettings, IStubModelSettings>(
                countGetter: () => count,
                maxCapacityGetter: () => maxCapacity,
                minCapacityGetter: () => minCapacity
            );
        }

        private static class TestData
        {
            public const int MAX_CAPACITY = 10;
            public const int MIN_CAPACITY = 3;
            public const int INIT_LENGTH = 5;
        }

        #endregion
    }
}
