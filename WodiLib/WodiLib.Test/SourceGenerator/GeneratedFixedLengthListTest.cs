using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.SourceGenerator
{
    /*
     * 自動生成した容量固定リストクラスの動作確認 兼 容量固定リストクラスのテストケースサンプル
     *
     * 自動生成される、リストとしての機能は
     *      StubRestrictedCapacityList
     * でテストを行う。自動生成した個別のクラスでは行わない。
     */
    [TestFixture]
    public class GenerateFixedLengthListTest
    {
        private static Logger logger = null!;

        private static PropertyTestHelper propertyTestHelper = null!;
        private static ConstructorTestHelper constructorTestHelper = null!;
        private static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private static ImpureActionTestHelper impureActionTestHelper = null!;
        private static ItemEqualsTestHelper itemEqualsTestHelper = null!;
        private static StaticFunctionTestHelper staticFunctionTestHelper = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            propertyTestHelper = new PropertyTestHelper(logger);
            constructorTestHelper = new ConstructorTestHelper(logger);
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            impureActionTestHelper = new ImpureActionTestHelper(logger);
            itemEqualsTestHelper = new ItemEqualsTestHelper(logger);
            staticFunctionTestHelper = new StaticFunctionTestHelper(logger);
        }

        #region Constants

        /// <summary>
        ///     容量が意図した値であること。
        /// </summary>
        [Test]
        public static void CapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => StubFixedLengthList.Capacity,
                resultValueVerifier: ValueVerifier.AreEquals(5)
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
            var instance = new StubFixedLengthList(CreateSettingsDto());

            var expected = new List<string>() { "Tag1", "Tag2" };

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
            IStubFixedLengthListSettings settings = CreateSettingsDto();

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
                factory: () => new StubFixedLengthList(length),
                instanceVerifier: null
            );
        }

        #endregion

        #region Copy

        /// <summary>
        ///     コピーコンストラクタが正常に終了すること
        /// </summary>
        [Test]
        public static void ConstructorTest_Copy_Success()
        {
            var settings = CreateSettingsDto(length: 3);
            var src = new StubFixedLengthList(settings);
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubFixedLengthList(src),
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
            var instance = new StubFixedLengthList(length: 1);
            const string expected = "JSON RESULT";
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                target => target.ToJsonString(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region SetNowStringValue

        /// <summary>
        ///     読取専用クラスでprotected定義された非純粋関数で、編集可能クラスで公開するよう設定されたメソッドについて、
        ///     編集可能クラスで参照できること。
        /// </summary>
        [Test]
        public static void SetNowStringValue_Success()
        {
            var instance = new StubFixedLengthList(length: 3);
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
        ///     読取専用クラスでprotected定義された非純粋関数で、編集可能クラスで公開するよう設定されたメソッドについて、
        ///     編集可能クラスで参照できること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_EqualityObject()
        {
            var left = new StubFixedLengthList(CreateSettingsDto(length: 3));
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
            var left = new StubFixedLengthList(CreateSettingsDto());
            IStubFixedLengthListSettings? right = null;
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
            var left = new StubFixedLengthList(CreateSettingsDto(length: 3))
            {
                [1] =
                {
                    StringValue = "Diff Value",
                },
            };
            IStubFixedLengthListSettings right = CreateSettingsDto(length: 3);
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
            var left = new StubFixedLengthList(CreateSettingsDto(length: 4));
            var right = new ReadOnlyStubFixedLengthList(CreateSettingsDto(length: 4));
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        #endregion

        #region FixedLengthList

        /// <summary>
        ///     編集可能クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_EditableModel()
        {
            var left = new StubFixedLengthList(CreateSettingsDto(length: 4));
            var right = new StubFixedLengthList(CreateSettingsDto(length: 4));
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
            var left = new StubFixedLengthList(CreateSettingsDto(length: 4));
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

        private static StubFixedLengthListSettings CreateSettingsDto(int length = 4)
        {
            return new StubFixedLengthListSettings(length.Iterate(CreateItemSettingsDto).ToList())
            {
                Tags = new List<string>() { "Tag1", "Tag2" },
            };
        }

        private static StubModelSettings CreateItemSettingsDto(int index)
        {
            return new StubModelSettings()
            {
                StringValue = index.ToString(),
            };
        }

        #endregion
    }
}
