using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DBProjectTest : TestFixtureBase
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
            var instance = new DBProject();
            var setItem = DatabaseKind.System;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBProject.DbKind),
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
            var instance = new DBProject();
            DatabaseKind? setItem = null;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBProject.DbKind),
                setItem,
                isValueEqualsBefore: true,
                setter: (x, v) => x.DbKind = v,
                getter: x => x.DbKind,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        #endregion

        #region ProjectTypeList

        /// <summary>
        ///     プロパティ ProjectTypeList の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void ProjectTypeListGetAndSetTest_Success()
        {
            var instance = new DBProject();
            var setItem = DatabaseTestData.CreateDatabaseProjectTypeListType2();

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBProject.ProjectTypeList),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.ProjectTypeList = v,
                getter: x => x.ProjectTypeList,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ ProjectTypeList に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void ProjectTypeListSetTest_Failure_PropertyNullException()
        {
            var instance = new DBProject();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseProjectTypeList)null!,
                setter: (x, v) => x.ProjectTypeList = v,
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
            IDBProjectSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.DbKind;
            _ = settings.ProjectTypeList;
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
            _ = dto.ProjectTypeList;
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
                factory: () => new DBProject(),
                instanceVerifier: ValueVerifier<DBProject>.AreItemEquals(new DBProjectSettings())
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
                factory: () => new DBProject(settings),
                instanceVerifier: ValueVerifier<DBProject>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDBProjectSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DBProject(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DBProjectSettings.ProjectTypeList))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DBProject(CreateSettingsDto(nullProperty: nullProperty)),
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
            var left = new DBProject(CreateSettingsDto());
            IDBProjectSettings right = left;
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
            var left = new DBProject(CreateSettingsDto());
            IDBProjectSettings right = CreateSettingsDto();
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
            var left = new DBProject(CreateSettingsDto());
            IDBProjectSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DBProjectSettings.DbKind))]
        [TestCase(nameof(DBProjectSettings.ProjectTypeList))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DBProject(CreateSettingsDto());
            IDBProjectSettings right = CreateSettingsDto(replaceProperty: replaceProperty);
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
            var instance = new DBProject(CreateSettingsDto());

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
        private static DBProjectSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DBProjectSettings
            {
                DbKind = (nullProperty == nameof(DBProjectSettings.DbKind), replaceProperty == nameof(DBProjectSettings.DbKind)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseKind.System,
                    (  false, true ) => DatabaseKind.Changeable,
                    (  true,  _    ) => null!,
                },
                ProjectTypeList = (nullProperty == nameof(DBProjectSettings.ProjectTypeList), replaceProperty == nameof(DBProjectSettings.ProjectTypeList)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseProjectTypeListType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseProjectTypeListType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
