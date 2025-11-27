using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DBDatTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region DbKind

        /// <summary>
        ///     プロパティ DbKind の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DbKindGetAndSetTest_Success()
        {
            var instance = new DBDat();
            var setItem = DatabaseKind.User;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBDat.DbKind),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DbKind = v,
                getter: x => x.DbKind,
                getValueVerifier: ValueVerifier<DatabaseKind?>.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DbKind に null を設定した場合、
        ///     取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DbKindSetTest_Success_NullValue()
        {
            var instance = new DBDat();
            DatabaseKind? setItem = null;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBDat.DbKind),
                setItem,
                isValueEqualsBefore: true,
                setter: (x, v) => x.DbKind = v,
                getter: x => x.DbKind,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        #endregion

        #region DataTableDefinitionList

        /// <summary>
        ///     プロパティ DataTableDefinitionList の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DataTableDefinitionListGetAndSetTest_Success()
        {
            var instance = new DBDat();
            var setItem =
                DatabaseTestData.CreateDatabaseDataTableWithDataNamingDefinitionListType2();

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBDat.DataTableDefinitionList),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DataTableDefinitionList = v,
                getter: x => x.DataTableDefinitionList,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DataTableDefinitionList に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void DataTableDefinitionListSetTest_Failure_PropertyNullException()
        {
            var instance = new DBDat();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseDataTableWithDataNamingDefinitionList)null!,
                setter: (x, v) => x.DataTableDefinitionList = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
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
            IDBDatSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.DbKind;
            _ = settings.DataTableDefinitionList;
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
            _ = dto.DataTableDefinitionList;
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
                factory: () => new DBDat(),
                instanceVerifier: ValueVerifier<DBDat>.AreItemEquals(new DBDatSettings())
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
                factory: () => new DBDat(settings),
                instanceVerifier: ValueVerifier<DBDat>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDBDatSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DBDat(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DBDatSettings.DataTableDefinitionList))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DBDat(CreateSettingsDto(nullProperty: nullProperty)),
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
            var left = new DBDat(CreateSettingsDto());
            IDBDatSettings right = left;
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
            var left = new DBDat(CreateSettingsDto());
            IDBDatSettings right = CreateSettingsDto();
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
            var left = new DBDat(CreateSettingsDto());
            IDBDatSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DBDatSettings.DbKind))]
        [TestCase(nameof(DBDatSettings.DataTableDefinitionList))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DBDat(CreateSettingsDto());
            IDBDatSettings right = CreateSettingsDto(replaceProperty: replaceProperty);
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
            var instance = new DBDat(CreateSettingsDto());

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
        private static DBDatSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DBDatSettings
            {
                DbKind = (nullProperty == nameof(DBDatSettings.DbKind), replaceProperty == nameof(DBDatSettings.DbKind)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseKind.User,
                    (  false, true ) => DatabaseKind.Changeable,
                    (  true,  _    ) => null!,
                },
                DataTableDefinitionList = (nullProperty == nameof(DBDatSettings.DataTableDefinitionList), replaceProperty == nameof(DBDatSettings.DataTableDefinitionList)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseDataTableWithDataNamingDefinitionListType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseDataTableWithDataNamingDefinitionListType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
