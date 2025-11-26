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
    public class GenerateFixedLength2DListTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     <para>容量設定が意図した値であること。</para>
        ///     <para>プロパティ名の "Row","Column" が設定に従って変化していること</para>
        /// </summary>
        [Test]
        public static void CapacityTest()
        {
            Assert.AreEqual(10, StubFixedLength2DList.MaxXCapacity);
            Assert.AreEqual(1, StubFixedLength2DList.MinXCapacity);
            Assert.AreEqual(9, StubFixedLength2DList.MaxYCapacity);
            Assert.AreEqual(0, StubFixedLength2DList.MinYCapacity);
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
            var instance = new StubFixedLength2DList(CreateSettingsDto());

            var expected = new List<string> { "Tag1", "Tag2" };

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.Tags,
                getValueVerifier: new ValueVerifier<IReadOnlyList<string>>(actual =>
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
            IStubFixedLength2DListSettings settings = CreateSettingsDto();

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
        public static void ConstructorTest_RowAndColumnLength_Success()
        {
            const int xSize = 3;
            const int ySize = 5;
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubFixedLength2DList(xSize: xSize, ySize: ySize),
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
            var settings = CreateSettingsDto(3);
            var src = new StubFixedLength2DList(settings);
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubFixedLength2DList(src),
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
            var instance = new StubFixedLength2DList(xSize: 3, ySize: 5);
            var expected = "JSON RESULT";
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
            var instance = new StubFixedLength2DList(CreateSettingsDto(4));
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
            var left = new StubFixedLength2DList(CreateSettingsDto(3));
            var right = CreateSettingsDto(3);
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
            var left = new StubFixedLength2DList(CreateSettingsDto());
            IStubFixedLength2DListSettings? right = null;
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
            var left = new StubFixedLength2DList(CreateSettingsDto())
            {
                [1, 0] =
                {
                    StringValue = "Diff Value",
                },
            };
            IStubFixedLength2DListSettings right = CreateSettingsDto();
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
            var left = new StubFixedLength2DList(CreateSettingsDto());
            var right = new ReadOnlyStubFixedLength2DList(new StubFixedLength2DList(CreateSettingsDto()));
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
            var left = new StubFixedLength2DList(CreateSettingsDto());
            var right = new StubFixedLength2DList(CreateSettingsDto());
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
            var left = new StubFixedLength2DList(CreateSettingsDto());
            object right = "CompareTest";
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #endregion

        #region ListMethods

        /// <summary>
        ///     メソッド名の "Row", "Column", "Cell" が設定した名称に置き換えられていること
        /// </summary>
        [Test]
        public static void MutableMethodDefinitionNameTest_InEditableList()
        {
            _ = nameof(StubFixedLength2DList.GetX);
            _ = nameof(StubFixedLength2DList.GetXRange);
            _ = nameof(StubFixedLength2DList.GetY);
            _ = nameof(StubFixedLength2DList.GetYRange);
            _ = nameof(StubFixedLength2DList.GetPoint);
            _ = nameof(StubFixedLength2DList.SetX);
            _ = nameof(StubFixedLength2DList.SetXRange);
            _ = nameof(StubFixedLength2DList.SetY);
            _ = nameof(StubFixedLength2DList.SetYRange);
            _ = nameof(StubFixedLength2DList.SetPoint);
            _ = nameof(StubFixedLength2DList.MoveX);
            _ = nameof(StubFixedLength2DList.MoveXRange);
            _ = nameof(StubFixedLength2DList.MoveY);
            _ = nameof(StubFixedLength2DList.MoveYRange);
            _ = nameof(StubFixedLength2DList.Reset);

            _ = nameof(StubFixedLength2DList.ValidateGetX);
            _ = nameof(StubFixedLength2DList.ValidateGetXRange);
            _ = nameof(StubFixedLength2DList.ValidateGetY);
            _ = nameof(StubFixedLength2DList.ValidateGetYRange);
            _ = nameof(StubFixedLength2DList.ValidateGetPoint);
            _ = nameof(StubFixedLength2DList.ValidateSetX);
            _ = nameof(StubFixedLength2DList.ValidateSetXRange);
            _ = nameof(StubFixedLength2DList.ValidateSetY);
            _ = nameof(StubFixedLength2DList.ValidateSetYRange);
            _ = nameof(StubFixedLength2DList.ValidateSetPoint);
            _ = nameof(StubFixedLength2DList.ValidateMoveX);
            _ = nameof(StubFixedLength2DList.ValidateMoveXRange);
            _ = nameof(StubFixedLength2DList.ValidateMoveY);
            _ = nameof(StubFixedLength2DList.ValidateMoveYRange);
            _ = nameof(StubFixedLength2DList.ValidateReset);

            _ = nameof(StubFixedLength2DList.GetXInternal);
            _ = nameof(StubFixedLength2DList.GetXRangeInternal);
            _ = nameof(StubFixedLength2DList.GetYInternal);
            _ = nameof(StubFixedLength2DList.GetYRangeInternal);
            _ = nameof(StubFixedLength2DList.GetPointInternal);
            _ = nameof(StubFixedLength2DList.SetXInternal);
            _ = nameof(StubFixedLength2DList.SetXRangeInternal);
            _ = nameof(StubFixedLength2DList.SetYInternal);
            _ = nameof(StubFixedLength2DList.SetYRangeInternal);
            _ = nameof(StubFixedLength2DList.SetPointInternal);
            _ = nameof(StubFixedLength2DList.MoveXInternal);
            _ = nameof(StubFixedLength2DList.MoveXRangeInternal);
            _ = nameof(StubFixedLength2DList.MoveYInternal);
            _ = nameof(StubFixedLength2DList.MoveYRangeInternal);
            _ = nameof(StubFixedLength2DList.ResetInternal);
        }

        #endregion

        #endregion

        #endregion

        /*
         * 読み取り専用クラスについては特に検証しない。
         * 各メソッドの機能検証は EditableClass のメソッドテストで行っているため、改めて行う必要はない。
         */

        #endregion

        #region ReadOnlyList

        #region ListMethods

        /// <summary>
        ///     メソッド名の "Row", "Column", "Cell" が設定した名称に置き換えられていること
        /// </summary>
        [Test]
        public static void MutableMethodDefinitionNameTest_InReadOnlyList()
        {
            _ = nameof(ReadOnlyStubFixedLength2DList.GetX);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetXRange);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetY);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetYRange);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetPoint);

            _ = nameof(ReadOnlyStubFixedLength2DList.ValidateGetX);
            _ = nameof(ReadOnlyStubFixedLength2DList.ValidateGetXRange);
            _ = nameof(ReadOnlyStubFixedLength2DList.ValidateGetY);
            _ = nameof(ReadOnlyStubFixedLength2DList.ValidateGetYRange);
            _ = nameof(ReadOnlyStubFixedLength2DList.ValidateGetPoint);

            _ = nameof(ReadOnlyStubFixedLength2DList.GetXInternal);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetXRangeInternal);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetYInternal);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetYRangeInternal);
            _ = nameof(ReadOnlyStubFixedLength2DList.GetPointInternal);
        }

        #endregion

        #endregion

        #region テスト用Settings作成

        private static StubFixedLength2DListSettings CreateSettingsDto(int rowLength = 4)
        {
            return new StubFixedLength2DListSettings(
                rowLength.Iterate<IStubFixedLengthListSettings>(rowIndex => CreateRowSettingsDto(rowIndex)
                    )
                    .ToList()
            )
            {
                Tags = new List<string> { "Tag1", "Tag2" },
            };
        }

        private static StubFixedLengthListSettings CreateRowSettingsDto(int rowIndex)
        {
            return new StubFixedLengthListSettings(
                StubFixedLengthList.Capacity
                    .Iterate<IStubModelSettings>(colIndex => CreateItemSettingsDto(rowIndex, colIndex)
                    )
                    .ToList()
            )
            {
                Tags = new List<string> { "Tag1", "Tag2" },
            };
        }

        private static StubModelSettings CreateItemSettingsDto(int rowIndex, int columnIndex)
        {
            return new StubModelSettings
            {
                StringValue = $"Row{rowIndex} Col{columnIndex}",
            };
        }

        #endregion
    }
}
