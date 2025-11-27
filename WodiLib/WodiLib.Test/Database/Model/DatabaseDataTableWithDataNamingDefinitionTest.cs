using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseDataTableWithDataNamingDefinitionTest : TestFixtureBase
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
        public static void DataTableGeTest_Success()
        {
            var instance = new DatabaseDataTableWithDataNamingDefinition();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.DataTable,
                getValueVerifier: ValueVerifier<DatabaseDataTable>.IsType(typeof(DatabaseDataTable))
            );
        }

        #endregion

        #region DataNamingDefinition

        /// <summary>
        ///     プロパティ DataNamingDefinition の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DataNamingDefinitionGetAndSetTest_Success()
        {
            var instance = new DatabaseDataTableWithDataNamingDefinition();
            var setItem = DatabaseTestData.CreateDatabaseDataNamingDefinitionType1();

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseDataTableWithDataNamingDefinition.DataNamingDefinition),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DataNamingDefinition = v,
                getter: x => x.DataNamingDefinition,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DataNamingDefinition に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void DataNamingDefinitionSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseDataTableWithDataNamingDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseDataNamingDefinition)null!,
                setter: (x, v) => x.DataNamingDefinition = v,
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
            IDatabaseDataTableWithDataNamingDefinitionSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.DataTable;
            _ = settings.DataNamingDefinition;
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
            _ = dto.DataNamingDefinition;
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
                factory: () => new DatabaseDataTableWithDataNamingDefinition(),
                instanceVerifier: ValueVerifier<DatabaseDataTableWithDataNamingDefinition>.AreItemEquals(
                    new DatabaseDataTableWithDataNamingDefinitionSettings()
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
                factory: () => new DatabaseDataTableWithDataNamingDefinition(settings),
                instanceVerifier: ValueVerifier<DatabaseDataTableWithDataNamingDefinition>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseDataTableWithDataNamingDefinitionSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseDataTableWithDataNamingDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataTable))]
        [TestCase(nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataNamingDefinition))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseDataTableWithDataNamingDefinition(
                    CreateSettingsDto(nullProperty: nullProperty)
                ),
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
            var left = new DatabaseDataTableWithDataNamingDefinition(CreateSettingsDto());
            IDatabaseDataTableWithDataNamingDefinitionSettings right = left;
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
            var left = new DatabaseDataTableWithDataNamingDefinition(CreateSettingsDto());
            IDatabaseDataTableWithDataNamingDefinitionSettings right = CreateSettingsDto();
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
            var left = new DatabaseDataTableWithDataNamingDefinition(CreateSettingsDto());
            IDatabaseDataTableWithDataNamingDefinitionSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataTable))]
        [TestCase(nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataNamingDefinition))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseDataTableWithDataNamingDefinition(CreateSettingsDto());
            IDatabaseDataTableWithDataNamingDefinitionSettings right =
                CreateSettingsDto(replaceProperty: replaceProperty);
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
            var instance = new DatabaseDataTableWithDataNamingDefinition(CreateSettingsDto());

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
        private static DatabaseDataTableWithDataNamingDefinitionSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DatabaseDataTableWithDataNamingDefinitionSettings
            {
                DataTable = (nullProperty == nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataTable), replaceProperty == nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataTable)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseDataTableSettingsType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseDataTableSettingsType2(),
                    (  true,  _    ) => null!,
                },
                DataNamingDefinition = (nullProperty == nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataNamingDefinition), replaceProperty == nameof(DatabaseDataTableWithDataNamingDefinitionSettings.DataNamingDefinition)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseDataNamingDefinitionType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseDataNamingDefinitionType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
