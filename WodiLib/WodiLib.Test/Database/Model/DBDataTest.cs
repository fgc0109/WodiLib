using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DBDataTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region DataTable

        /// <summary>
        ///     プロパティ DataTable の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DataTableGetAndSetTest_Success()
        {
            var instance = new DBData();
            var setItem = DatabaseTestData.CreateDatabaseNamedDataTableType2();

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DBData.DataTable),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DataTable = v,
                getter: x => x.DataTable,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DataTable に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void DataTableSetTest_Failure_PropertyNullException()
        {
            var instance = new DBData();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseNamedDataTable)null!,
                setter: (x, v) => x.DataTable = v,
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
            IDBDataSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.DataTable;
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
            _ = dto.DataTable;
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
                factory: () => new DBData(),
                instanceVerifier: ValueVerifier<DBData>.AreItemEquals(new DBDataSettings())
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
                factory: () => new DBData(settings),
                instanceVerifier: ValueVerifier<DBData>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDBDataSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DBData(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DBDataSettings.DataTable))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DBData(CreateSettingsDto(nullProperty: nullProperty)),
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
            var left = new DBData(CreateSettingsDto());
            IDBDataSettings right = left;
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
            var left = new DBData(CreateSettingsDto());
            IDBDataSettings right = CreateSettingsDto();
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
            var left = new DBData(CreateSettingsDto());
            IDBDataSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DBDataSettings.DataTable))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DBData(CreateSettingsDto());
            IDBDataSettings right = CreateSettingsDto(replaceProperty: replaceProperty);
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
            var instance = new DBData(CreateSettingsDto());

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
        private static DBDataSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DBDataSettings
            {
                DataTable = (nullProperty == nameof(DBDataSettings.DataTable), replaceProperty == nameof(DBDataSettings.DataTable)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseNamedDataTableType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseNamedDataTableType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
