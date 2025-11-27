using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionManualSettingsTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Methods

        #region ItemEquals

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_IDatabaseFieldSpecialSettingDefinitionSettings_Normal()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionSettings other =
                new DatabaseFieldSpecialSettingDefinitionNormalSettings();
            itemEqualsTestHelper.ItemEquals(
                instance,
                other,
                false
            );
        }

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_IDatabaseFieldSpecialSettingDefinitionSettings_LoadFile()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionSettings other =
                new DatabaseFieldSpecialSettingDefinitionLoadFileSettings();
            itemEqualsTestHelper.ItemEquals(
                instance,
                other,
                false
            );
        }

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_IDatabaseFieldSpecialSettingDefinitionSettings_DatabaseReference()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionSettings other =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings();
            itemEqualsTestHelper.ItemEquals(
                instance,
                other,
                false
            );
        }

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_IDatabaseFieldSpecialSettingDefinitionSettings_Manual()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionSettings other = CreateSettingsDto();
            itemEqualsTestHelper.ItemEquals(
                instance,
                other,
                true
            );
        }

        #endregion

        #endregion

        #region TryCastNormalSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastNormalSettingsTest()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionNormalSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastNormalSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastLoadFileSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastLoadFileSettingsTest()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastLoadFileSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastDatabaseReferenceSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastDatabaseReferenceSettingsTest()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastDatabaseReferenceSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastManualSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastManualSettingsTest()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionManualSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastManualSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(true)
            );

            Assert.IsNotNull(result);
        }

        #endregion

        #endregion

        private static DatabaseFieldSpecialSettingDefinitionManualSettings CreateSettingsDto()
        {
            return new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                InitValue = 1,
                SpecialCases = new DatabaseValueCaseListSettings(
                    new DatabaseValueCase[]
                    {
                        new(1, "1"),
                        new(1, "1"),
                    }
                ),
            };
        }
    }

    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionManualTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region SettingType

        /// <summary>
        ///     プロパティ SettingType の取得に成功すること。
        /// </summary>
        [Test]
        public static void SettingTypeGetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.SettingType,
                getValueVerifier: ValueVerifier.AreEquals(DatabaseFieldSpecialSettingType.Manual)
            );
        }

        #endregion

        #region DefaultType

        /// <summary>
        ///     プロパティ DefaultType の取得に成功すること。
        /// </summary>
        [Test]
        public static void DefaultTypeGetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.DefaultType,
                getValueVerifier: ValueVerifier.AreEquals(DatabaseFieldType.Int)
            );
        }

        #endregion

        #region InitValue

        /// <summary>
        ///     プロパティ InitValue の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void InitValueGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual();
            DatabaseValueInt setItem = 1;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionManual.InitValue),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.InitValue = v,
                getter: x => x.InitValue,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ InitValue に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void InitValueSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueInt)null!,
                setter: (x, v) => x.InitValue = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region SpecialCases

        /// <summary>
        ///     プロパティ SpecialCases の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void SpecialCasesGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.SpecialCases,
                getValueVerifier: ValueVerifier<DatabaseValueCaseList>.IsType(typeof(DatabaseValueCaseList))
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
            IDatabaseFieldSpecialSettingDefinitionManualSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.InitValue;
            _ = settings.SpecialCases;
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
            _ = dto.InitValue;
            _ = dto.SpecialCases;
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
                factory: () => new DatabaseFieldSpecialSettingDefinitionManual(),
                instanceVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinitionManual>.AreItemEquals(
                    new DatabaseFieldSpecialSettingDefinitionManualSettings()
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
                factory: () => new DatabaseFieldSpecialSettingDefinitionManual(settings),
                instanceVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinitionManual>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseFieldSpecialSettingDefinitionManualSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionManual(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionManual(
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

        #region CanChangeFieldType

        /// <summary>
        ///     意図した結果が取得されること
        /// </summary>
        [TestCase("Int", true)]
        [TestCase("String", false)]
        public static void CanChangeFieldTypeTest_Success(string fieldTypeName, bool expected)
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            var fieldType = fieldTypeName switch
            {
                nameof(DatabaseFieldType.Int) => DatabaseFieldType.Int,
                nameof(DatabaseFieldType.String) => DatabaseFieldType.String,
                _ => throw new ArgumentOutOfRangeException(nameof(fieldTypeName), fieldTypeName, null),
            };

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.CanChangeFieldType(fieldType),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void CanChangeFieldTypeTest_Failure_NullArgs()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());

            pureFunctionTestHelper.PureFuncFailure(
                instance,
                execFunc: target => target.CanChangeFieldType(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region ItemEquals

        #region Settings

        /// <summary>
        ///     対象インスタンスと other が同じインスタンスの場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_SameObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionManualSettings right = left;
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
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionManualSettings right = CreateSettingsDto();
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
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionManualSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionManualSettings right =
                CreateSettingsDto(replaceProperty: replaceProperty);
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #region IReadOnlyDatabaseFieldSpecialSettingDefinition

        /// <summary>
        ///     対象インスタンスと other が同じインスタンスの場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_SpecialSettingsDefinition_True_SameObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right = left;
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
        public static void ItemEqualsTest_SpecialSettingsDefinition_True_EqualityObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right =
                new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
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
        public static void ItemEqualsTest_SpecialSettingsDefinition_False_NullObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases))]
        public static void ItemEqualsTest_SpecialSettingsDefinition_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right =
                new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto(replaceProperty: replaceProperty));
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     DB項目値特殊指定タイプが異なるインスタンスと比較した場合
        ///     false が返却されること。
        /// </summary>
        /// <param name="typeName"></param>
        [TestCase(nameof(DatabaseFieldSpecialSettingType.Normal))]
        [TestCase(nameof(DatabaseFieldSpecialSettingType.LoadFile))]
        [TestCase(nameof(DatabaseFieldSpecialSettingType.ReferDatabase))]
        public static void ItemEqualsTest_SpecialSettingsDefinition_False_DifferType(string typeName)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right = typeName switch
            {
                nameof(DatabaseFieldSpecialSettingType.Normal) => new DatabaseFieldSpecialSettingDefinitionNormal(),
                nameof(DatabaseFieldSpecialSettingType.LoadFile) => new DatabaseFieldSpecialSettingDefinitionLoadFile(),
                nameof(DatabaseFieldSpecialSettingType.ReferDatabase) =>
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReference(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName, null),
            };
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #region IDatabaseFieldSpecialSettingDefinitionSettings

        /// <summary>
        ///     対象インスタンスと other が同じインスタンスの場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_True_SameObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right = left;
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
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_True_EqualityObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right =
                new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
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
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_False_NullObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases))]
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right =
                new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto(replaceProperty: replaceProperty));
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     DB項目値特殊指定タイプが異なるインスタンスと比較した場合
        ///     false が返却されること。
        /// </summary>
        /// <param name="typeName"></param>
        [TestCase(nameof(DatabaseFieldSpecialSettingType.Normal))]
        [TestCase(nameof(DatabaseFieldSpecialSettingType.LoadFile))]
        [TestCase(nameof(DatabaseFieldSpecialSettingType.ReferDatabase))]
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_False_DifferType(string typeName)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right = typeName switch
            {
                nameof(DatabaseFieldSpecialSettingType.Normal) => new DatabaseFieldSpecialSettingDefinitionNormal(),
                nameof(DatabaseFieldSpecialSettingType.LoadFile) => new DatabaseFieldSpecialSettingDefinitionLoadFile(),
                nameof(DatabaseFieldSpecialSettingType.ReferDatabase) =>
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReference(),
                _ => throw new ArgumentOutOfRangeException(nameof(typeName), typeName, null),
            };
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        #endregion

        #endregion

        #region TryCastNormalSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastNormalSettingsTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionNormalSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastNormalSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastLoadFileSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastLoadFileSettingsTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionLoadFileSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastLoadFileSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastDatabaseReferenceSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastDatabaseReferenceSettingsTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastDatabaseReferenceSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastManualSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastManualSettingsTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionManualSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastManualSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(true)
            );

            Assert.IsNotNull(result);
        }

        #endregion

        #region TryCastNormal

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastNormalTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            DatabaseFieldSpecialSettingDefinitionNormal? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastNormal(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastLoadFile

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastLoadFileTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            DatabaseFieldSpecialSettingDefinitionLoadFile? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastLoadFile(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastDatabaseReference

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastDatabaseReferenceTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            DatabaseFieldSpecialSettingDefinitionDatabaseReference? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastDatabaseReference(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastManual

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastManualTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());
            DatabaseFieldSpecialSettingDefinitionManual? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastManual(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(true)
            );

            Assert.IsNotNull(result);
        }

        #endregion

        #region DeepClone

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionManual(CreateSettingsDto());

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
        private static DatabaseFieldSpecialSettingDefinitionManualSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DatabaseFieldSpecialSettingDefinitionManualSettings
            {
                InitValue = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.InitValue), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.InitValue)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => 1,
                    (  false, true ) => 2,
                    (  true,  _    ) => null!,
                },
                SpecialCases = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionManualSettings.SpecialCases)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateValueCaseListSettingsType1(),
                    (  false, true ) => DatabaseTestData.CreateValueCaseListSettingsType2(),
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
