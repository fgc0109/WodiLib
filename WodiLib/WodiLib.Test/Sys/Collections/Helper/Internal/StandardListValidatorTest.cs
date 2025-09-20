using System;
using System.Collections.Generic;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

// ReSharper disable RedundantCast

namespace WodiLib.Test.Sys.Collections
{
    [TestFixture]
    public class CommonListValidatorTest
    {
        private static Logger logger = null!;

        private static PureActionTestHelper pureActionTestHelper = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            pureActionTestHelper = new PureActionTestHelper(logger);
        }

        #region Methods

        #region Constructor

        /// <summary>
        ///     引数 initItems の要素数が 0 以上の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(1000)]
        public static void ConstructorTest_Success(int initItemsLength)
        {
            var initItems = initItemsLength.Iterate(i => new StubModel(i.ToString()));
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initItems), initItems))
            );
        }

        /// <summary>
        ///     引数 initItems が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_ArgumentNull()
        {
            IEnumerable<IStubModelSettings> initItems = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: instance => instance.Constructor((nameof(initItems), initItems)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentNullException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentNullException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Insert

        /// <summary>
        ///     引数 index, items がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(TestData.INIT_LENGTH, 0)]
        [TestCase(TestData.INIT_LENGTH, 100)]
        public static void InsertTest_Success(int index, int itemsLength)
        {
            var items = itemsLength.Iterate(i => new StubModel((i + 100).ToString()));
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Insert((nameof(index), index), (nameof(items), items))
            );
        }

        /// <summary>
        ///     引数 items が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void InsertTest_Failure_ArgumentNull()
        {
            const int index = 0;
            IEnumerable<IStubModelSettings> items = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Insert((nameof(index), index), (nameof(items), items)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 index がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void InsertTest_Failure_OutOfRangeIndex(int index)
        {
            var items = Array.Empty<StubModel>();
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Insert((nameof(index), index), (nameof(items), items)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Overwrite

        /// <summary>
        ///     引数 index, items がリスト範囲内の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, 10)]
        [TestCase(TestData.INIT_LENGTH, 0)]
        [TestCase(TestData.INIT_LENGTH, 100)]
        public static void OverwriteTest_Success(int index, int itemsLength)
        {
            var items = itemsLength.Iterate(i => new StubModel((i + 100).ToString()));
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Overwrite((nameof(index), index), (nameof(items), items))
            );
        }

        /// <summary>
        ///     引数 items が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteTest_Failure_ArgumentNull()
        {
            const int index = 0;
            IEnumerable<IStubModelSettings> items = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Overwrite((nameof(index), index), (nameof(items), items)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 index がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void OverwriteTest_Failure_OutOfRangeIndex(int index)
        {
            var items = Array.Empty<StubModel>();
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Overwrite((nameof(index), index), (nameof(items), items)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentException))
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Remove

        /// <summary>
        ///     引数 index, count がリスト範囲内であり、除去した結果要素数が 0 未満とならない場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0, 0)]
        [TestCase(0, TestData.INIT_LENGTH)]
        [TestCase(TestData.INIT_LENGTH - 1, 0)]
        [TestCase(TestData.INIT_LENGTH - 1, 1)]
        public static void RemoveTest_Success(int index, int count)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Remove((nameof(index), index), (nameof(count), count))
            );
        }

        /// <summary>
        ///     引数 index がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void RemoveTest_Failure_OutOfRangeIndex(int index)
        {
            const int count = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Remove((nameof(index), index), (nameof(count), count)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     引数 count がリスト範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [TestCase(-1)]
        [TestCase(TestData.INIT_LENGTH + 1)]
        public static void RemoveTest_Failure_OutOfRangeCount(int count)
        {
            const int index = 0;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Remove((nameof(index), index), (nameof(count), count)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        /// <summary>
        ///     要素を除去した結果要素数が 0 未満となる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Ignore("通常のリストは要素数0未満になるような指定は index, count の範囲チェックでエラーとなる")]
        public static void RemoveTest_Failure_ShorterItem()
        {
            const int index = 0;
            const int count = TestData.INIT_LENGTH + 1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Remove((nameof(index), index), (nameof(count), count)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region AdjustLength

        /// <summary>
        ///     引数 length が 0 以上の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(100)]
        public static void AdjustLengthTest_Success(int length)
        {
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.AdjustLength((nameof(length), length))
            );
        }

        /// <summary>
        ///     引数 length が 0 未満の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        [Test]
        public static void AdjustLengthTest_Failure_ShorterItem()
        {
            const int length = -1;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.AdjustLength((nameof(length), length)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region Reset

        #region Settings - CanChangeSize

        /// <summary>
        ///     引数 items の要素数が 0 以上の場合、
        ///     処理が正常に終了すること。
        /// </summary>
        [TestCase(0)]
        [TestCase(100)]
        public static void ResetTest_SettingsCanChangeSize_Success(int itemsLength)
        {
            var items = itemsLength.Iterate(i => new StubModel(i.ToString()));
            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(items), items))
            );
        }

        /// <summary>
        ///     引数 items が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_SettingsCanChangeSize_Failure_ArgumentNull()
        {
            IEnumerable<IStubModelSettings> items = null!;
            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(),
                execAction: target => target.Reset((nameof(items), items)),
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region Settings - CannotChangeSize

        /// <summary>
        ///     引数 items の要素数が現在のリストの要素数と同じ場合、
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数 items の要素数が現在のリストの要素数と異なる場合、
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
                verifyException: ExceptionVerifier.IsType(typeof(ArgumentException))
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

        private static StandardListValidator<IStubModelSettings> GetTestInstance(
            int count = TestData.INIT_LENGTH
        )
        {
            return new StandardListValidator<IStubModelSettings>(
                countGetter: () => count
            );
        }

        private static class TestData
        {
            public const int INIT_LENGTH = 5;
        }

        #endregion
    }
}
