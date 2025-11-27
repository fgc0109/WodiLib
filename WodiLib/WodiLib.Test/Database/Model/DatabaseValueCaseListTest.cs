using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseValueCaseListTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     最大容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MaxCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseValueCaseList.MaxCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(int.MaxValue)
            );
        }

        /// <summary>
        ///     最小容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseValueCaseList.MinCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(0)
            );
        }

        #endregion

        #region Constructors

        #region NoParam

        /// <summary>
        ///     引数なしコンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_NoParam_Success()
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueCaseList(),
                instanceVerifier: ValueVerifier<DatabaseValueCaseList>.AreItemEquals(
                    new DatabaseValueCaseListSettings()
                )
            );
        }

        #endregion

        #region SettingsDto

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Success()
        {
            var settings = CreateSettingsDto();
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueCaseList(settings),
                instanceVerifier: ValueVerifier<DatabaseValueCaseList>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseValueCaseListSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseValueCaseList(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOに null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_HasNullItem()
        {
            var settings = CreateSettingsDto(hasNullItem: true);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseValueCaseList(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region EditableClass

        #region public

        #region GetForCaseNumber

        private static object[] GetForCaseNumber_Success_TestCaseSource =
        {
            // caseNumber, expected
            new object[] { 1, new DatabaseValueCase(1, "Case 1") },
            new object?[] { 3, null },
        };

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        /// <param name="caseNumber">検索する選択肢番号</param>
        /// <param name="expected">期待する選択肢文字列</param>
        [TestCaseSource(nameof(GetForCaseNumber_Success_TestCaseSource))]
        public static void GetForCaseNumber_Success(int caseNumber, DatabaseValueCase? expected)
        {
            var instance = new DatabaseValueCaseList(
                new DatabaseValueCaseListSettings(
                    new[]
                    {
                        new DatabaseValueCase(0, "Case 0"),
                        new DatabaseValueCase(1, "Case 1"),
                        new DatabaseValueCase(2, "Case 2"),
                    }
                )
            );
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: x => x.GetForCaseNumber(caseNumber),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     caseNumber が null の場合、
        ///     null が返却されること。
        /// </summary>
        [Test]
        public static void GetForCaseNumber_Success_ArgumentNull()
        {
            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DatabaseValueCaseList(CreateSettingsDto()),
                execFunc: x => x.GetForCaseNumber(null),
                resultValueVerifier: ValueVerifier.IsNull()
            );
        }

        #endregion

        #region ItemEquals

        #region Settings

        /// <summary>
        ///     対象インスタンスと other が同じインスタンスの場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_SameObject()
        {
            var left = new DatabaseValueCaseList(CreateSettingsDto());
            IDatabaseValueCaseListSettings right = left;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        /// <summary>
        ///     対象インスタンスと同じ値を持つ設定DTOと比較した場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_EqualityObject()
        {
            var left = new DatabaseValueCaseList(CreateSettingsDto());
            IDatabaseValueCaseListSettings right = CreateSettingsDto();
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        /// <summary>
        ///     null と比較した場合 false が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_False_NullObject()
        {
            var left = new DatabaseValueCaseList(CreateSettingsDto());
            IDatabaseValueCaseListSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     null 要素を持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_False_NullItem()
        {
            var left = new DatabaseValueCaseList(CreateSettingsDto());
            IDatabaseValueCaseListSettings right = CreateSettingsDto(hasNullItem: true);
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #endregion

        #region DeepClone

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest()
        {
            var instance = new DatabaseValueCaseList(CreateSettingsDto());

            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region SettingsDto

        #region Constructor

        /// <summary>
        ///     引数なしコンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void SettingsDtoConstructorTest_Success_NoParam()
        {
            constructorTestHelper.ConstructorSuccess(
                () => new DatabaseValueCaseListSettings(),
                instanceVerifier: new ValueVerifier<DatabaseValueCaseListSettings>(instance =>
                    {
                        // 要素数が最小であること
                        Assert.AreEqual(DatabaseValueCaseList.MinCapacity, instance.Settings.Count);
                    }
                )
            );
        }

        #endregion

        #endregion

        #region テスト用Settings作成

        /// <summary>
        ///     設定DTO作成
        /// </summary>
        /// <param name="itemLength">
        ///     Items に詰める要素数。
        /// </param>
        /// <param name="hasDiffItem">
        ///     Items に通常と異なる要素をもたせるかどうか<br/>
        ///     0番目の要素を置き換える
        /// </param>
        /// <param name="hasNullItem">
        ///     Items に null 要素をもたせるかどうか<br/>
        ///     0番目の要素を null にする
        /// </param>
        /// <returns></returns>
        private static DatabaseValueCaseListSettings CreateSettingsDto(
            int itemLength = 01,
            bool hasDiffItem = false,
            bool hasNullItem = false
        )
        {
            return new DatabaseValueCaseListSettings(
                itemLength.Iterate(i => CreateSettingsItem(i, hasDiffItem, hasNullItem)).ToArray()
            );
        }

        /// <summary>
        ///     設定DTOの要素作成
        /// </summary>
        /// <param name="index">要素番号</param>
        /// <param name="requestDiffItem">
        ///     Items に通常と異なる要素をもたせるかどうか<br/>
        ///     0番目の要素を置き換える
        /// </param>
        /// <param name="requestNullItem">
        ///     Items に null 要素をもたせるかどうか<br/>
        ///     0番目の要素を null にする
        /// </param>
        /// <returns></returns>
        private static DatabaseValueCase CreateSettingsItem(
            int index,
            bool requestDiffItem,
            bool requestNullItem
        )
        {
            if (index == 0 && requestDiffItem)
            {
                return new DatabaseValueCase(10000, "Diff Case");
            }

            if (index == 0 && requestNullItem)
            {
                return null!;
            }

            return new DatabaseValueCase(index, $"Case {index}");
        }

        #endregion
    }
}
