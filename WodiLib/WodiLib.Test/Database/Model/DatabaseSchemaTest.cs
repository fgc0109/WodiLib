using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseSchemaTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region DatabaseKind

        /// <summary>
        ///     プロパティ DatabaseKind の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DatabaseKindGetAndSetTest_Success_NotNull()
        {
            var instance = new DatabaseSchema();
            var setItem = DatabaseKind.Changeable;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseSchema.DbKind),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DbKind = v,
                getter: x => x.DbKind,
                getValueVerifier: ValueVerifier<DatabaseKind?>.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DatabaseKind に null を設定した場合、
        ///     PropertyNullException が発生しないこと。
        /// </summary>
        [Test]
        public static void DatabaseKindSetTest_Success_Null()
        {
            var instance = new DatabaseSchema
            {
                DbKind = DatabaseKind.Changeable,
            };
            DatabaseKind? setItem = null;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseSchema.DbKind),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DbKind = v,
                getter: x => x.DbKind,
                getValueVerifier: ValueVerifier<DatabaseKind?>.AreEquals(setItem)
            );
        }

        #endregion

        #region TypeTableList

        /// <summary>
        ///     プロパティ TypeTableList の取得に成功すること。
        /// </summary>
        [Test]
        public static void TypeTableListGetTest_Success()
        {
            var instance = new DatabaseSchema();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.TypeTableList
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
            IDatabaseSchemaSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.DbKind;
            _ = settings.TypeTableList;
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
            _ = dto.DbKind;
            _ = dto.TypeTableList;
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
                factory: () => new DatabaseSchema(),
                instanceVerifier: ValueVerifier<DatabaseSchema>.AreItemEquals(new DatabaseSchemaSettings())
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
                factory: () => new DatabaseSchema(settings),
                instanceVerifier: ValueVerifier<DatabaseSchema>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseSchemaSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseSchema(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     コピー元のプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DatabaseSchemaSettings.TypeTableList))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseSchema(CreateSettingsDto(nullProperty: nullProperty)),
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
            var left = new DatabaseSchema(CreateSettingsDto());
            IDatabaseSchemaSettings right = left;
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
            var left = new DatabaseSchema(CreateSettingsDto());
            IDatabaseSchemaSettings right = CreateSettingsDto();
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
            var left = new DatabaseSchema(CreateSettingsDto());
            IDatabaseSchemaSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseSchemaSettings.DbKind))]
        [TestCase(nameof(DatabaseSchemaSettings.TypeTableList))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseSchema(CreateSettingsDto());
            IDatabaseSchemaSettings right = CreateSettingsDto(replaceProperty: replaceProperty);
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
            var instance = new DatabaseSchema(CreateSettingsDto());

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
        private static DatabaseSchemaSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DatabaseSchemaSettings
            {
                DbKind = (nullProperty == nameof(DatabaseSchemaSettings.DbKind), replaceProperty == nameof(DatabaseSchemaSettings.DbKind)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseKind.Changeable,
                    (  false, true ) => DatabaseKind.User,
                    (  true,  _    ) => null!,
                },
                TypeTableList = (nullProperty == nameof(DatabaseSchemaSettings.TypeTableList), replaceProperty == nameof(DatabaseSchemaSettings.TypeTableList)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateTypeTableListSettingsType1(),
                    (  false, true ) => DatabaseTestData.CreateTypeTableListSettingsType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
