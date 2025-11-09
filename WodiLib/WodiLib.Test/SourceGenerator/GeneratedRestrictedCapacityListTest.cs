using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.SourceGenerator
{
    /*
     * 自動生成した容量制限リストクラスの動作確認 兼 容量制限リストクラスのテストケースサンプル
     *
     * 自動生成される、リストとしての機能は
     *      RestrictedCapacityListTest
     *      FixedLengthListTest
     *      ReadOnlyExtendedListTest
     * でテストを行う。自動生成した個別のクラスでは行わない。
     */
    [TestFixture]
    public class GenerateRestrictedCapacityListTest : TestFixtureBase
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
                execFunc: () => StubRestrictedCapacityList.MaxCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(10)
            );
        }

        /// <summary>
        ///     最小容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => StubRestrictedCapacityList.MinCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(1)
            );
        }

        #endregion

        #region Properties

        #region MutableClass

        #region public

        #region Tags

        /// <summary>
        ///     編集可能モデルクラスで読取専用型とは別の型として公開したプロパティについて
        ///     型情報が適用されていること
        /// </summary>
        [Test]
        public static void TagsGetTest_Success()
        {
            var instance = new StubRestrictedCapacityList(CreateSettingsDto());
            instance.Tags.Add("test1");
            instance.Tags.Add("test2");

            var expected = new List<string>() { "Tag1", "Tag2", "test1", "test2" };

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.Tags,
                getValueVerifier: new ValueVerifier<IList<string>>(actual =>
                    {
                        CustomAssert.AreSequenceEquals(expected, actual);
                    }
                )
            );
        }

        #endregion

        #endregion

        #endregion

        #region SettingsInterface

        /// <summary>
        ///     設定DTOインタフェースに意図したプロパティがすべて定義されていること。
        /// </summary>
        // [Test] // テスト実行はしない
        public static void SettingInterfacePropertyTest()
        {
            IStubRestrictedCapacityListSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.Tags;
        }

        #endregion

        #region SettingsDto

        /// <summary>
        ///     設定DTOに意図したプロパティがすべて実装されていること。
        /// </summary>
        // [Test] // テスト実行はしない
        public static void SettingsDtoPropertyTest()
        {
            var dto = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = dto.Tags;
        }

        #endregion

        #endregion

        #region Constructors

        #region NoParams

        /// <summary>
        ///     編集可能クラスにも実装するよう指定したコンストラクタが
        ///     実装されていること
        /// </summary>
        [Test]
        public static void ConstructorTest_Length_Success()
        {
            const int length = 5;
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubRestrictedCapacityList(length),
                instanceVerifier: null
            );
        }

        #endregion

        #region SettingsDto

        /// <summary>
        ///     コピーコンストラクタが正常に終了すること
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Success()
        {
            var settings = CreateSettingsDto(length: 3);
            var src = new StubRestrictedCapacityList(settings);
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubRestrictedCapacityList(src),
                instanceVerifier: ValueVerifier.AreItemEquals(src)
            );
        }

        #endregion

        #endregion

        #region EditableList

        #region Methods

        #region public

        #region ToJsonString

        /// <summary>
        ///     読取専用クラスで定義した純粋関数が編集可能クラスでも使用できること
        /// </summary>
        [Test]
        public static void ToJsonStringTest_Success()
        {
            var instance = new StubRestrictedCapacityList(length: 3);
            var expected = "JSON RESULT";
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                target => target.ToJsonString(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region ToJsonString

        /// <summary>
        ///     読取専用クラスでprotected定義された非純粋関数で、編集可能クラスで公開するよう設定されたメソッドについて、
        ///     編集可能クラスで参照できること。
        /// </summary>
        [Test]
        public static void SetNowStringValue_Success()
        {
            var instance = new StubRestrictedCapacityList(CreateSettingsDto(length: 3));
            impureActionTestHelper.ImpureActionSuccess(
                instance,
                target => target.SetNowStringValue(),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: null
            );
        }

        #endregion

        #region ItemEquals

        #region Settings

        /// <summary>
        ///     設定DTOと比較した場合 true が返却されること
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_EqualityObject()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto(length: 3));
            var right = CreateSettingsDto(length: 3);
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        /// <summary>
        ///     null と比較した場合 false が返却されること
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_False_NullObject()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto());
            IStubRestrictedCapacityListSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_False_DifferElement()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto(length: 3))
            {
                [1] =
                {
                    StringValue = "Diff Value",
                },
            };
            IStubRestrictedCapacityListSettings right = CreateSettingsDto(length: 3);
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #region ReadOnlyList

        /// <summary>
        ///     読取専用クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_ReadOnlyModel()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto(length: 4));
            var right = new ReadOnlyStubRestrictedCapacityList(
                new StubRestrictedCapacityList(CreateSettingsDto(length: 4))
            );
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        #endregion

        #region FixedLengthList

        /// <summary>
        ///     読取専用クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_FixedLengthList()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto(length: 4));
            var right = new FixedStubRestrictedCapacityList(
                new StubRestrictedCapacityList(CreateSettingsDto(length: 4))
            );
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        #endregion

        #region EditableList

        /// <summary>
        ///     編集可能クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_EditableModel()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto(length: 4));
            var right = new StubRestrictedCapacityList(CreateSettingsDto(length: 4));
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        #endregion

        #region Object

        /// <summary>
        ///     objectとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Object()
        {
            var left = new StubRestrictedCapacityList(CreateSettingsDto(length: 4));
            object right = "CompareTest";
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #endregion

        #endregion

        #endregion

        /*
         * 読み取り専用クラスについては特に検証しない。
         * 各メソッドの機能検証は EditableClass のメソッドテストで行っているため、改めて行う必要はない。
         */

        #endregion

        #region テスト用Settings作成

        private static StubRestrictedCapacityListSettings CreateSettingsDto(int length = 4)
        {
            return new StubRestrictedCapacityListSettings(length.Iterate(CreateItemSettingsDto).ToList())
            {
                Tags = new List<string>() { "Tag1", "Tag2" },
            };
        }

        private static IStubModelSettings CreateItemSettingsDto(int index)
        {
            return new StubModelSettings()
            {
                StringValue = index.ToString(),
            };
        }

        #endregion
    }
}
