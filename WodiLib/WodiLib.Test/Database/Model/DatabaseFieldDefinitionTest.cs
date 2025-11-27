using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseFieldDefinitionTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Properties

        #region MutableClass

        #region public

        #region FieldName

        /// <summary>
        ///     プロパティ FieldName の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void FieldNameGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldDefinition();
            FieldName setItem = "TestFieldName";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldDefinition.FieldName),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.FieldName = v,
                getter: x => x.FieldName,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ FieldName に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void FieldNameSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (FieldName)null!,
                setter: (x, v) => x.FieldName = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region FieldType

        /// <summary>
        ///     プロパティ FieldType の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void FieldTypeGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldDefinition();
            var setItem = DatabaseFieldType.String;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldDefinition.FieldType),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.FieldType = v,
                getter: x => x.FieldType,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ FieldType に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void FieldTypeSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseFieldType)null!,
                setter: (x, v) => x.FieldType = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region SpecialSettingDefinition

        /// <summary>
        ///     プロパティ SpecialSettingDefinition の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void SpecialSettingDefinitionGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldDefinition();
            var setItem = DatabaseTestData.CreateDatabaseFieldSpecialSettingDefinitionType1();

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldDefinition.SpecialSettingDefinition),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.SpecialSettingDefinition = v,
                getter: x => x.SpecialSettingDefinition,
                getValueVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinition>(result =>
                    {
                        CustomAssert.AreItemEquals(setItem, result);
                    }
                )
            );
        }

        /// <summary>
        ///     プロパティ SpecialSettingDefinition に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void SpecialSettingDefinitionSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseFieldSpecialSettingDefinition)null!,
                setter: (x, v) => x.SpecialSettingDefinition = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region FieldMemo

        /// <summary>
        ///     プロパティ FieldMemo の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void FieldMemoGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldDefinition();
            FieldMemo setItem = "TestFieldName";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldDefinition.FieldMemo),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.FieldMemo = v,
                getter: x => x.FieldMemo,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ FieldMemo に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void FieldMemoSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldDefinition();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (FieldMemo)null!,
                setter: (x, v) => x.FieldMemo = v,
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
            IDatabaseFieldDefinitionSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.FieldName;
            _ = settings.FieldType;
            _ = settings.SpecialSettingDefinition;
            _ = settings.FieldMemo;
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
            _ = dto.FieldName;
            _ = dto.FieldType;
            _ = dto.SpecialSettingDefinition;
            _ = dto.FieldMemo;
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
                factory: () => new DatabaseFieldDefinition(),
                instanceVerifier: ValueVerifier<DatabaseFieldDefinition>.AreItemEquals(
                    new DatabaseFieldDefinitionSettings()
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
                factory: () => new DatabaseFieldDefinition(settings),
                instanceVerifier: ValueVerifier<DatabaseFieldDefinition>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseFieldDefinitionSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldDefinition(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldDefinitionSettings.FieldName))]
        [TestCase(nameof(DatabaseFieldDefinitionSettings.FieldType))]
        [TestCase(nameof(DatabaseFieldDefinitionSettings.SpecialSettingDefinition))]
        [TestCase(nameof(DatabaseFieldDefinitionSettings.FieldMemo))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldDefinition(CreateSettingsDto(nullProperty: nullProperty)),
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
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void CanChangeFieldTypeTest_Success()
        {
            /*
                条件と結果の組み合わせはテストしない
            */

            var instance = new DatabaseFieldDefinition(
                new DatabaseFieldDefinitionSettings
                {
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings(),
                    FieldType = DatabaseFieldType.String,
                }
            );
            var fieldType = DatabaseFieldType.Int;
            const bool expected = false;

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
            var instance = new DatabaseFieldDefinition(
                new DatabaseFieldDefinitionSettings
                {
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings(),
                    FieldType = DatabaseFieldType.String,
                }
            );

            pureFunctionTestHelper.PureFuncFailure(
                instance,
                execFunc: target => target.CanChangeFieldType(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region ChangeFieldType

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void ChangeFieldTypeTest_Success()
        {
            var instance = new DatabaseFieldDefinition(
                new DatabaseFieldDefinitionSettings
                {
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionNormalSettings(),
                    FieldType = DatabaseFieldType.String,
                }
            );
            var fieldType = DatabaseFieldType.Int;

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.ChangeFieldType(fieldType),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.FieldType),
                },
                instanceVerifier: new ValueVerifier<DatabaseFieldDefinition>(target =>
                    {
                        // 値種別が変化していること
                        Assert.AreEqual(target.FieldType, fieldType);
                    }
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ChangeFieldTypeTest_Failure_NullArgs()
        {
            var instance = new DatabaseFieldDefinition(
                new DatabaseFieldDefinitionSettings
                {
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings(),
                    FieldType = DatabaseFieldType.String,
                }
            );

            pureActionTestHelper.PureActionFailure(
                instance,
                execAction: target => target.ChangeFieldType(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     変更不可能な値種別が指定された場合、
        ///     InvalidOperationException が発生すること。
        /// </summary>
        [Test]
        public static void ChangeFieldTypeTest_Failure_CannotChangeType()
        {
            var instance = new DatabaseFieldDefinition(
                new DatabaseFieldDefinitionSettings
                {
                    SpecialSettingDefinition = new DatabaseFieldSpecialSettingDefinitionLoadFileSettings(),
                    FieldType = DatabaseFieldType.String,
                }
            );

            pureActionTestHelper.PureActionFailure(
                instance,
                execAction: target => target.ChangeFieldType(DatabaseFieldType.Int),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(InvalidOperationException))
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
            var left = new DatabaseFieldDefinition(CreateSettingsDto());
            IDatabaseFieldDefinitionSettings right = left;
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
            var left = new DatabaseFieldDefinition(CreateSettingsDto());
            IDatabaseFieldDefinitionSettings right = CreateSettingsDto();
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
            var left = new DatabaseFieldDefinition(CreateSettingsDto());
            IDatabaseFieldDefinitionSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldDefinitionSettings.FieldName))]
        [TestCase(nameof(DatabaseFieldDefinitionSettings.FieldType))]
        [TestCase(nameof(DatabaseFieldDefinitionSettings.SpecialSettingDefinition))]
        [TestCase(nameof(DatabaseFieldDefinitionSettings.FieldMemo))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldDefinition(CreateSettingsDto());
            IDatabaseFieldDefinitionSettings right = CreateSettingsDto(replaceProperty: replaceProperty);
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
        public static void DeepCloneTest_DatabaseFieldDefinitionSettings()
        {
            var instance = new DatabaseFieldDefinition(CreateSettingsDto());

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
        private static DatabaseFieldDefinitionSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DatabaseFieldDefinitionSettings
            {
                FieldName = (nullProperty == nameof(DatabaseFieldDefinitionSettings.FieldName), replaceProperty == nameof(DatabaseFieldDefinitionSettings.FieldName)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "TestFieldName",
                    (  false, true ) => "テスト項目名",
                    (  true,  _    ) => null!,
                },
                FieldType = (nullProperty == nameof(DatabaseFieldDefinitionSettings.FieldType), replaceProperty == nameof(DatabaseFieldDefinitionSettings.FieldType)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseFieldType.String,
                    (  false, true ) => DatabaseFieldType.Int,
                    (  true,  _    ) => null!,
                },
                SpecialSettingDefinition = (nullProperty == nameof(DatabaseFieldDefinitionSettings.SpecialSettingDefinition), replaceProperty == nameof(DatabaseFieldDefinitionSettings.SpecialSettingDefinition)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseTestData.CreateDatabaseFieldSpecialSettingDefinitionSettingsType1(),
                    (  false, true ) => DatabaseTestData.CreateDatabaseFieldSpecialSettingDefinitionSettingsType2(),
                    (  true,  _    ) => null!,
                },
                FieldMemo = (nullProperty == nameof(DatabaseFieldDefinitionSettings.FieldMemo), replaceProperty == nameof(DatabaseFieldDefinitionSettings.FieldMemo)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "TestFieldName",
                    (  false, true ) => "テスト項目メモ",
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
