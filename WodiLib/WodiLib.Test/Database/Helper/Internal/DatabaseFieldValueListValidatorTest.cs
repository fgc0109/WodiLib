using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Helper
{
    [TestFixture]
    public class DatabaseFieldValueListValidatorTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constructor

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Success()
        {
            var initItems = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };
            var initSettings = new DatabaseFieldValueListSettings(initItems);

            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings))
            );
        }

        /// <summary>
        ///     initValues の値種別が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_WithMismatchedFieldType()
        {
            var initItems = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };
            var initSettings = new DatabaseFieldValueListSettings(initItems);

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.String),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initValues の値が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる値が混ざっている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_WhenInitItemsContainDifferentFieldType()
        {
            var initItems = new DatabaseFieldValue[]
            {
                new(1),
                new("2"),
                new(3),
                new(4),
            };
            var initSettings = new DatabaseFieldValueListSettings(initItems);

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Constructor((nameof(initSettings), initSettings)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Set

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void SetTest_Success()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Set((nameof(index), index), (nameof(items), items))
            );
        }

        /// <summary>
        ///     initValues の値種別が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void SetTest_Failure_WithMismatchedFieldType()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.String),
                execAction: instance => instance.Set((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initValues の値が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる値が混ざっている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void SetTest_Failure_WhenInitItemsContainDifferentFieldType()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new("2"),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Set((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Insert

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void InsertTest_Success()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Insert((nameof(index), index), (nameof(items), items))
            );
        }

        /// <summary>
        ///     initValues の値種別が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void InsertTest_Failure_WithMismatchedFieldType()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.String),
                execAction: instance => instance.Insert((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initValues の値が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる値が混ざっている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void InsertTest_Failure_WhenInitItemsContainDifferentFieldType()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new("2"),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Insert((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Overwrite

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void OverwriteTest_Success()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Overwrite((nameof(index), index), (nameof(items), items))
            );
        }

        /// <summary>
        ///     initValues の値種別が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteTest_Failure_WithMismatchedFieldType()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.String),
                execAction: instance => instance.Overwrite((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initValues の値が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる値が混ざっている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void OverwriteTest_Failure_WhenInitItemsContainDifferentFieldType()
        {
            const int index = 0;
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new("2"),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Overwrite((nameof(index), index), (nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Reset

        /// <summary>
        ///     処理が正常に終了すること。
        /// </summary>
        [Test]
        public static void ResetTest_Success()
        {
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionSuccess(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Reset((nameof(items), items))
            );
        }

        /// <summary>
        ///     initValues の値種別が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_Failure_WithMismatchedFieldType()
        {
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new(2),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.String),
                execAction: instance => instance.Reset((nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     initValues の値が DatabaseFieldValueListValidator のコンストラクタで指定された値種別と異なる値が混ざっている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ResetTest_Failure_WhenInitItemsContainDifferentFieldType()
        {
            var items = new DatabaseFieldValue[]
            {
                new(1),
                new("2"),
                new(3),
                new(4),
            };

            pureActionTestHelper.PureActionFailure(
                instance: GetTestInstance(databaseFieldType: DatabaseFieldType.Int),
                execAction: instance => instance.Reset((nameof(items), items)),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region TestClass

        private static DatabaseFieldValueListValidator GetTestInstance(
            int count = TestData.INIT_LENGTH,
            int maxCapacity = TestData.MAX_CAPACITY,
            int minCapacity = TestData.MIN_CAPACITY,
            DatabaseFieldType? databaseFieldType = null
        )
        {
            return new DatabaseFieldValueListValidator(
                countGetter: () => count,
                maxCapacityGetter: () => maxCapacity,
                minCapacityGetter: () => minCapacity,
                fieldType: databaseFieldType ?? DatabaseFieldType.Int
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
