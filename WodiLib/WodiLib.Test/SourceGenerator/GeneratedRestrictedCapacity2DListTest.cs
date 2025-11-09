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
    public class GenerateRestrictedCapacity2DListTest : TestFixtureBase
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
            Assert.AreEqual(10, StubRestrictedCapacity2DList.MaxXCapacity);
            Assert.AreEqual(1, StubRestrictedCapacity2DList.MinXCapacity);
            Assert.AreEqual(9, StubRestrictedCapacity2DList.MaxYCapacity);
            Assert.AreEqual(0, StubRestrictedCapacity2DList.MinYCapacity);
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
            var instance = new StubRestrictedCapacity2DList(CreateSettingsDto());
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
            IStubRestrictedCapacity2DListSettings settings = CreateSettingsDto();

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
            const int xSize = 5;
            const int ySize = 4;
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubRestrictedCapacity2DList(xSize: xSize, ySize: ySize),
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
            var settings = CreateSettingsDto(3, 5);
            var src = new StubRestrictedCapacity2DList(settings);
            constructorTestHelper.ConstructorSuccess(
                factory: () => new StubRestrictedCapacity2DList(src),
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
            var instance = new StubRestrictedCapacity2DList(xSize: 3, ySize: 5);
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
            var instance = new StubRestrictedCapacity2DList(CreateSettingsDto(4, 3));
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
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto(5, 4));
            var right = CreateSettingsDto(5, 4);
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
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto());
            IStubRestrictedCapacity2DListSettings? right = null;
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
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto())
            {
                [1, 0] =
                {
                    StringValue = "Diff Value",
                },
            };
            IStubRestrictedCapacity2DListSettings right = CreateSettingsDto();
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
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto());
            var right = new ReadOnlyStubRestrictedCapacity2DList(new StubRestrictedCapacity2DList(CreateSettingsDto()));
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );
        }

        #endregion

        #region FixedLengthList

        /// <summary>
        ///     容量固定クラスとの比較処理が実装されていること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_FixedLengthList()
        {
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto());
            var right = new FixedStubRestrictedCapacity2DList(new StubRestrictedCapacity2DList(CreateSettingsDto()));
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
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto());
            var right = new StubRestrictedCapacity2DList(CreateSettingsDto());
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
            var left = new StubRestrictedCapacity2DList(CreateSettingsDto());
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
            _ = nameof(StubRestrictedCapacity2DList.GetX);
            _ = nameof(StubRestrictedCapacity2DList.GetXRange);
            _ = nameof(StubRestrictedCapacity2DList.GetY);
            _ = nameof(StubRestrictedCapacity2DList.GetYRange);
            _ = nameof(StubRestrictedCapacity2DList.GetPoint);
            _ = nameof(StubRestrictedCapacity2DList.SetX);
            _ = nameof(StubRestrictedCapacity2DList.SetXRange);
            _ = nameof(StubRestrictedCapacity2DList.SetY);
            _ = nameof(StubRestrictedCapacity2DList.SetYRange);
            _ = nameof(StubRestrictedCapacity2DList.SetPoint);
            _ = nameof(StubRestrictedCapacity2DList.AddX);
            _ = nameof(StubRestrictedCapacity2DList.AddXRange);
            _ = nameof(StubRestrictedCapacity2DList.AddY);
            _ = nameof(StubRestrictedCapacity2DList.AddYRange);
            _ = nameof(StubRestrictedCapacity2DList.InsertX);
            _ = nameof(StubRestrictedCapacity2DList.InsertXRange);
            _ = nameof(StubRestrictedCapacity2DList.InsertY);
            _ = nameof(StubRestrictedCapacity2DList.InsertYRange);
            _ = nameof(StubRestrictedCapacity2DList.OverwriteX);
            _ = nameof(StubRestrictedCapacity2DList.OverwriteY);
            _ = nameof(StubRestrictedCapacity2DList.MoveX);
            _ = nameof(StubRestrictedCapacity2DList.MoveXRange);
            _ = nameof(StubRestrictedCapacity2DList.MoveY);
            _ = nameof(StubRestrictedCapacity2DList.MoveYRange);
            _ = nameof(StubRestrictedCapacity2DList.RemoveX);
            _ = nameof(StubRestrictedCapacity2DList.RemoveXRange);
            _ = nameof(StubRestrictedCapacity2DList.RemoveY);
            _ = nameof(StubRestrictedCapacity2DList.RemoveYRange);
            _ = nameof(StubRestrictedCapacity2DList.AdjustXLength);
            _ = nameof(StubRestrictedCapacity2DList.AdjustYLength);
            _ = nameof(StubRestrictedCapacity2DList.AdjustXLengthIfShort);
            _ = nameof(StubRestrictedCapacity2DList.AdjustYLengthIfShort);
            _ = nameof(StubRestrictedCapacity2DList.AdjustXLengthIfLong);
            _ = nameof(StubRestrictedCapacity2DList.AdjustYLengthIfLong);
            _ = nameof(StubRestrictedCapacity2DList.Reset);
            _ = nameof(StubRestrictedCapacity2DList.Clear);

            _ = nameof(StubRestrictedCapacity2DList.ValidateGetX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateGetXRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateGetY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateGetYRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateGetPoint);
            _ = nameof(StubRestrictedCapacity2DList.ValidateSetX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateSetXRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateSetY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateSetYRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateSetPoint);
            _ = nameof(StubRestrictedCapacity2DList.ValidateAddX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateAddXRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateAddY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateAddYRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateInsertX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateInsertXRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateInsertY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateInsertYRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateOverwriteX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateOverwriteY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateMoveX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateMoveXRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateMoveY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateMoveYRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateRemoveX);
            _ = nameof(StubRestrictedCapacity2DList.ValidateRemoveXRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateRemoveY);
            _ = nameof(StubRestrictedCapacity2DList.ValidateRemoveYRange);
            _ = nameof(StubRestrictedCapacity2DList.ValidateAdjustXLength);
            _ = nameof(StubRestrictedCapacity2DList.ValidateAdjustYLength);
            _ = nameof(StubRestrictedCapacity2DList.ValidateReset);
            _ = nameof(StubRestrictedCapacity2DList.ValidateClear);

            _ = nameof(StubRestrictedCapacity2DList.GetXInternal);
            _ = nameof(StubRestrictedCapacity2DList.GetXRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.GetYInternal);
            _ = nameof(StubRestrictedCapacity2DList.GetYRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.GetPointInternal);
            _ = nameof(StubRestrictedCapacity2DList.SetXInternal);
            _ = nameof(StubRestrictedCapacity2DList.SetXRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.SetYInternal);
            _ = nameof(StubRestrictedCapacity2DList.SetYRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.SetPointInternal);
            _ = nameof(StubRestrictedCapacity2DList.AddXInternal);
            _ = nameof(StubRestrictedCapacity2DList.AddXRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.AddYInternal);
            _ = nameof(StubRestrictedCapacity2DList.AddYRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.InsertXInternal);
            _ = nameof(StubRestrictedCapacity2DList.InsertXRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.InsertYInternal);
            _ = nameof(StubRestrictedCapacity2DList.InsertYRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.OverwriteXInternal);
            _ = nameof(StubRestrictedCapacity2DList.OverwriteYInternal);
            _ = nameof(StubRestrictedCapacity2DList.MoveXInternal);
            _ = nameof(StubRestrictedCapacity2DList.MoveXRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.MoveYInternal);
            _ = nameof(StubRestrictedCapacity2DList.MoveYRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.RemoveXInternal);
            _ = nameof(StubRestrictedCapacity2DList.RemoveXRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.RemoveYInternal);
            _ = nameof(StubRestrictedCapacity2DList.RemoveYRangeInternal);
            _ = nameof(StubRestrictedCapacity2DList.AdjustXLengthInternal);
            _ = nameof(StubRestrictedCapacity2DList.AdjustYLengthInternal);
            _ = nameof(StubRestrictedCapacity2DList.ResetInternal);
            _ = nameof(StubRestrictedCapacity2DList.ClearInternal);
        }

        #endregion

        #endregion

        #endregion

        /*
         * 読み取り専用クラスについては特に検証しない。
         * 各メソッドの機能検証は EditableClass のメソッドテストで行っているため、改めて行う必要はない。
         */

        #endregion

        #region FixedList

        #region ListMethods

        /// <summary>
        ///     メソッド名の "Row", "Column", "Cell" が設定した名称に置き換えられていること
        /// </summary>
        [Test]
        public static void MutableMethodDefinitionNameTest_InFixedList()
        {
            _ = nameof(FixedStubRestrictedCapacity2DList.GetX);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetXRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetY);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetYRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetPoint);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetX);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetXRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetY);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetYRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetPoint);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveX);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveXRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveY);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveYRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.Reset);

            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateGetX);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateGetXRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateGetY);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateGetYRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateGetPoint);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateSetX);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateSetXRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateSetY);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateSetYRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateSetPoint);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateMoveX);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateMoveXRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateMoveY);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateMoveYRange);
            _ = nameof(FixedStubRestrictedCapacity2DList.ValidateReset);

            _ = nameof(FixedStubRestrictedCapacity2DList.GetXInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetXRangeInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetYInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetYRangeInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.GetPointInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetXInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetXRangeInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetYInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetYRangeInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.SetPointInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveXInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveXRangeInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveYInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.MoveYRangeInternal);
            _ = nameof(FixedStubRestrictedCapacity2DList.ResetInternal);
        }

        #endregion

        #endregion

        #region ReadOnlyList

        #region ListMethods

        /// <summary>
        ///     メソッド名の "Row", "Column", "Cell" が設定した名称に置き換えられていること
        /// </summary>
        [Test]
        public static void MutableMethodDefinitionNameTest_InReadOnlyList()
        {
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetX);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetXRange);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetY);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetYRange);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetPoint);

            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.ValidateGetX);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.ValidateGetXRange);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.ValidateGetY);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.ValidateGetYRange);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.ValidateGetPoint);

            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetXInternal);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetXRangeInternal);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetYInternal);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetYRangeInternal);
            _ = nameof(ReadOnlyStubRestrictedCapacity2DList.GetPointInternal);
        }

        #endregion

        #endregion

        #region テスト用Settings作成

        private static StubRestrictedCapacity2DListSettings CreateSettingsDto(int rowLength = 4, int columnLength = 3)
        {
            return new StubRestrictedCapacity2DListSettings(
                rowLength.Iterate(rowIndex => CreateRowSettingsDto(rowIndex, columnLength)
                    )
                    .ToList()
            )
            {
                Tags = new List<string>() { "Tag1", "Tag2" },
            };
        }

        private static IStubRestrictedCapacityListSettings CreateRowSettingsDto(int rowIndex, int columnLength)
        {
            return new StubRestrictedCapacityListSettings(
                columnLength.Iterate(colIndex => CreateItemSettingsDto(rowIndex, colIndex)
                    )
                    .ToList()
            )
            {
                Tags = new List<string>() { "Tag1", "Tag2" },
            };
        }

        private static IStubModelSettings CreateItemSettingsDto(int rowIndex, int columnIndex)
        {
            return new StubModelSettings()
            {
                StringValue = $"Row{rowIndex} Col{columnIndex}",
            };
        }

        #endregion
    }
}
