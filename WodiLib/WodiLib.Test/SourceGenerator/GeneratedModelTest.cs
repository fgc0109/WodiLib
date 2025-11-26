using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Test.Tools;

// ReSharper disable NotAccessedField.Local

namespace WodiLib.Test.SourceGenerator
{
    /*
     * 自動生成したモデルクラスの動作確認 兼 モデルクラスのテストケースサンプル
     */
    [TestFixture]
    public class GeneratedModelTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region StringValue

        /// <summary>
        ///     編集可能モデルクラスで公開するよう設定したプロパティが公開されていること
        /// </summary>
        [Test]
        public static void StringValueGetAndSetTest_Success()
        {
            var instance = new StubModel();
            var setValue = "test";
            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(StubModel.StringValue),
                setValue,
                isValueEqualsBefore: false,
                setter: (x, v) => x.StringValue = v,
                getter: x => x.StringValue,
                getValueVerifier: ValueVerifier.AreEquals(setValue)
            );
        }

        #endregion

        #region Tags

        /// <summary>
        ///     編集可能モデルクラスで読取専用型とは別の型として公開したプロパティについて
        ///     型情報が適用されていること
        /// </summary>
        [Test]
        public static void TagsGetTest_Success()
        {
            var instance = new StubModel();

            var expected = new List<string> { "Tag1", "Tag2" };

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
            IStubModelSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.StringValue;
            _ = settings.Tags;
        }

        #endregion

        #region SettingsDto

        /// <summary>
        ///     設定DTOに意図したプロパティがすべて実装されていること。
        /// </summary>
        [Test]
        public static void SettingsDtoPropertyTest()
        {
            var dto = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = dto.StringValue;
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
        public static void ConstructorTest_NoParam_Success()
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubModel(),
                instanceVerifier: ValueVerifier<StubModel>.AreItemEquals(new StubModelSettings())
            );
        }

        #endregion

        #region StringValue

        /// <summary>
        ///     編集可能クラスにも実装するよう指定したコンストラクタが
        ///     親クラスの同コンストラクタを呼び出していること
        /// </summary>
        [Test]
        public static void ConstructorTest_StringValue_Success()
        {
            var strValue = "InitValue";
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubModel(strValue),
                instanceVerifier: new ValueVerifier<StubModel>(instance =>
                    {
                        Assert.AreEqual(strValue, instance.StringValue);
                        Assert.AreEqual(0, instance.Tags.Count);
                    }
                )
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
            var settings = CreateSettingsDto(str: "stringValue");
            var src = new StubModel(settings);
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubModel(src),
                instanceVerifier: ValueVerifier.AreItemEquals(src)
            );
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region ToJsonString

        /// <summary>
        ///     読取専用クラスで定義した純粋関数が編集可能クラスでも使用できること
        /// </summary>
        [Test]
        public static void ToJsonStringTest_Success()
        {
            var strValue = "InitValue";
            var instance = new StubModel(strValue);
            var expected = $@"{{""{nameof(StubModel.StringValue)}"":""{strValue}""}}";
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
            var strValue = "InitValue";
            var instance = new StubModel(strValue);
            impureActionTestHelper.ImpureActionSuccess(
                instance,
                target => target.SetNowStringValue(),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.StringValue),
                },
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
            var left = new StubModel(CreateSettingsDto(str: "CompareTest"));
            var right = CreateSettingsDto(str: "CompareTest");
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
            var left = new StubModel(CreateSettingsDto());
            IStubModelSettings? right = null;
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
        public static void ItemEqualsTest_Settings_False_DifferProperty()
        {
            var left = new StubModel(CreateSettingsDto(str: "A"));
            IStubModelSettings right = CreateSettingsDto(str: "B");
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #region ReadOnlyModel

        /// <summary>
        ///     読取専用クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_ReadOnlyModel()
        {
            var left = new StubModel(CreateSettingsDto(str: "CompareTest"));
            var right = new ReadOnlyStubModel(new StubModel(CreateSettingsDto(str: "CompareTest")));
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        #endregion

        #region EditableyModel

        /// <summary>
        ///     編集可能クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_EditableModel()
        {
            var left = new StubModel(CreateSettingsDto(str: "CompareTest"));
            var right = new StubModel(CreateSettingsDto(str: "CompareTest"));
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
            var left = new StubModel(CreateSettingsDto(str: "CompareTest"));
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

        /*
         * 読み取り専用クラスについては特に検証しない。
         * 各メソッドの機能検証は EditableClass のメソッドテストで行っているため、改めて行う必要はない。
         */

        #endregion

        #region テスト用Settings作成

        private static StubModelSettings CreateSettingsDto(string str = "")
        {
            return new StubModelSettings
            {
                StringValue = str,
            };
        }

        #endregion
    }
}
