using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseTypeDefinitionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region TypeName

        /// <summary>
        ///     プロパティ TypeName の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void TypeNameGetAndSetTest_Success()
        {
            var instance = new DatabaseTypeDefinition();
            TypeName setItem = "UpdateTypeName";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseTypeDefinition.TypeName),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.TypeName = v,
                getter: x => x.TypeName,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ TypeName に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void TypeNameSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseTypeDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (TypeName)null!,
                setter: (x, v) => x.TypeName = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region Memo

        /// <summary>
        ///     プロパティ Memo の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void MemoGetAndSetTest_Success()
        {
            var instance = new DatabaseTypeDefinition();
            DatabaseMemo setItem = "UpdateMemo";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseTypeDefinition.Memo),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.Memo = v,
                getter: x => x.Memo,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ Memo に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void MemoSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseTypeDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseMemo)null!,
                setter: (x, v) => x.Memo = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region FieldCount

        /// <summary>
        ///     プロパティ FieldCount の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void FieldCountGetAndSetTest_Success()
        {
            var instance = new DatabaseTypeDefinition();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.FieldCount,
                getValueVerifier: ValueVerifier<int>.IsType(typeof(int))
            );
        }

        #endregion

        #region FieldDefinitionList

        /// <summary>
        ///     プロパティ FieldDefinitionList の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void FieldDefinitionListGetAndSetTest_Success()
        {
            var instance = new DatabaseTypeDefinition();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.FieldDefinitionList,
                getValueVerifier: ValueVerifier<DatabaseFieldDefinitionList>.IsType(typeof(DatabaseFieldDefinitionList))
            );
        }

        #endregion

        #endregion

        #endregion

        #region SettingsInterface

        /// <summary>
        ///     設定DTOインタフェースに意図したプロパティがすべて定義されていること。
        /// </summary>
        [Test]
        public static void SettingInterfacePropertyTest()
        {
            IDatabaseTypeDefinitionSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.TypeName;
            _ = settings.Memo;
            _ = settings.FieldDefinitionList;
            Assert.Pass();
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
            _ = dto.TypeName;
            _ = dto.Memo;
            _ = dto.FieldDefinitionList;
            Assert.Pass();
        }

        #endregion

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
                factory: () => new DatabaseTypeDefinition(),
                instanceVerifier: ValueVerifier<DatabaseTypeDefinition>.AreItemEquals(
                    new DatabaseTypeDefinitionSettings()
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
                factory: () => new DatabaseTypeDefinition(settings),
                instanceVerifier: ValueVerifier<DatabaseTypeDefinition>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseTypeDefinitionSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DatabaseTypeDefinitionSettings.TypeName))]
        [TestCase(nameof(DatabaseTypeDefinitionSettings.Memo))]
        [TestCase(nameof(DatabaseTypeDefinitionSettings.FieldDefinitionList))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeDefinition(CreateSettingsDto(nullProperty: nullProperty)),
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
            var left = new DatabaseTypeDefinition(CreateSettingsDto());
            IDatabaseTypeDefinitionSettings right = left;
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
            var left = new DatabaseTypeDefinition(CreateSettingsDto());
            IDatabaseTypeDefinitionSettings right = CreateSettingsDto();
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
            var left = new DatabaseTypeDefinition(CreateSettingsDto());
            IDatabaseTypeDefinitionSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseTypeDefinitionSettings.TypeName))]
        [TestCase(nameof(DatabaseTypeDefinitionSettings.Memo))]
        [TestCase(nameof(DatabaseTypeDefinitionSettings.FieldDefinitionList))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseTypeDefinition(CreateSettingsDto());
            IDatabaseTypeDefinitionSettings right = CreateSettingsDto(replaceProperty: replaceProperty);
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
            var instance = new DatabaseTypeDefinition(CreateSettingsDto());

            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region テスト用Settings作成

        /// <summary>
        ///     設定DTO作成
        /// </summary>
        /// <param name="nullProperty">
        ///     null を設定するプロパティ名。<br/>
        ///     null の場合いずれのプロパティにも null を設定しない。
        /// </param>
        /// <param name="replaceProperty">
        ///     この引数で指定したプロパティは、指定しなかった場合とは違う値を設定する。
        /// </param>
        /// <returns></returns>
        private static DatabaseTypeDefinitionSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DatabaseTypeDefinitionSettings
            {
                TypeName = (nullProperty == nameof(DatabaseTypeDefinitionSettings.TypeName), replaceProperty == nameof(DatabaseTypeDefinitionSettings.TypeName)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "TestTypeName",
                    (  false, true ) => "テストタイプ名",
                    (  true,  _    ) => null!,
                },
                Memo = (nullProperty == nameof(DatabaseTypeDefinitionSettings.Memo), replaceProperty == nameof(DatabaseTypeDefinitionSettings.Memo)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "TestMemo",
                    (  false, true ) => "テストメモ",
                    (  true,  _    ) => null!,
                },
                FieldDefinitionList = (nullProperty == nameof(DatabaseTypeDefinitionSettings.FieldDefinitionList), replaceProperty == nameof(DatabaseTypeDefinitionSettings.FieldDefinitionList)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseFieldDefinitionListSettingsType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseFieldDefinitionListSettingsType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
