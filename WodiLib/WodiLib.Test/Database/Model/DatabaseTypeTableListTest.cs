using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseTypeTableListTest : TestFixtureBase
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
                execFunc: () => DatabaseTypeTableList.MaxCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MaxTypeLength)
            );
        }

        /// <summary>
        ///     最小容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeTableList.MinCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MinTypeLength)
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
                factory: () => new DatabaseTypeTableList(),
                instanceVerifier: ValueVerifier<DatabaseTypeTableList>.AreItemEquals(
                    new DatabaseTypeTableListSettings()
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
                factory: () => new DatabaseTypeTableList(settings),
                instanceVerifier: ValueVerifier<DatabaseTypeTableList>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseTypeTableListSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeTableList(settings),
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
                factory: () => new DatabaseTypeTableList(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     設定DTOの要素数が不足している場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_ItemsSizeUnderCapacity()
        {
            var settings = CreateSettingsDto(itemLength: DatabaseConst.MinTypeLength - 1);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeTableList(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     設定DTOの要素数が超過している場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_ItemsSizeOverCapacity()
        {
            var settings = CreateSettingsDto(itemLength: DatabaseConst.MaxTypeLength + 1);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeTableList(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region EditableClass

        #region public

        #region ItemEquals

        #region Settings

        /// <summary>
        ///     対象インスタンスと other が同じインスタンスの場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_SameObject()
        {
            var left = new DatabaseTypeTableList(CreateSettingsDto());
            IDatabaseTypeTableListSettings right = left;
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
            var left = new DatabaseTypeTableList(CreateSettingsDto());
            IDatabaseTypeTableListSettings right = CreateSettingsDto();
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
            var left = new DatabaseTypeTableList(CreateSettingsDto());
            IDatabaseTypeTableListSettings? right = null;
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
            var left = new DatabaseTypeTableList(CreateSettingsDto());
            IDatabaseTypeTableListSettings right = CreateSettingsDto(hasNullItem: true);
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
            var instance = new DatabaseTypeTableList(CreateSettingsDto());

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
                () => new DatabaseTypeTableListSettings(),
                instanceVerifier: new ValueVerifier<DatabaseTypeTableListSettings>(instance =>
                    {
                        // 要素数が最小であること
                        Assert.AreEqual(DatabaseTypeTableList.MinCapacity, instance.Settings.Count);
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
        private static DatabaseTypeTableListSettings CreateSettingsDto(
            int itemLength = 3,
            bool hasDiffItem = false,
            bool hasNullItem = false
        )
        {
            return new DatabaseTypeTableListSettings(
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
        private static IDatabaseTypeTableSettings CreateSettingsItem(
            int index,
            bool requestDiffItem,
            bool requestNullItem
        )
        {
            if (index == 0 && requestDiffItem)
            {
                return DatabaseTestData.CreateTypeTableSettingsType2();
            }

            if (index == 0 && requestNullItem)
            {
                return null!;
            }

            return DatabaseTestData.CreateTypeTableSettingsType1(typeId: index);
        }

        #endregion
    }
}
