using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettingsTest : TestFixtureBase
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
            IDatabaseFieldSpecialSettingDefinitionSettings other = CreateSettingsDto();
            itemEqualsTestHelper.ItemEquals(
                instance,
                other,
                true
            );
        }

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_IDatabaseFieldSpecialSettingDefinitionSettings_Manual()
        {
            var instance = CreateSettingsDto();
            IDatabaseFieldSpecialSettingDefinitionSettings other =
                new DatabaseFieldSpecialSettingDefinitionManualSettings();
            itemEqualsTestHelper.ItemEquals(
                instance,
                other,
                false
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
                resultValueVerifier: ValueVerifier<bool>.AreEquals(true)
            );

            Assert.IsNotNull(result);
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
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #endregion

        private static DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings CreateSettingsDto()
        {
            return new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                InitValue = 1,
                DatabaseReferKind = DatabaseReferType.System,
                DatabaseDbTypeId = 1,
                IsUseAdditionalItems = true,
                AdditionalCase1 = "Case 1",
                AdditionalCase2 = "Case 2",
                AdditionalCase3 = "Case 3",
            };
        }
    }

    [TestFixture]
    public class DatabaseFieldSpecialSettingDefinitionDatabaseReferenceTest : TestFixtureBase
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.SettingType,
                getValueVerifier: ValueVerifier.AreEquals(DatabaseFieldSpecialSettingType.ReferDatabase)
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            DatabaseValueInt setItem = 1;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.InitValue),
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueInt)null!,
                setter: (x, v) => x.InitValue = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region DatabaseReferKind

        /// <summary>
        ///     プロパティ DatabaseReferKind の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DatabaseReferKindGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            var setItem = DatabaseReferType.System;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.DatabaseReferKind),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DatabaseReferKind = v,
                getter: x => x.DatabaseReferKind,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DatabaseReferKind に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void DatabaseReferKindSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseReferType)null!,
                setter: (x, v) => x.DatabaseReferKind = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region DatabaseDbTypeId

        /// <summary>
        ///     プロパティ DatabaseDbTypeId の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void DatabaseDbTypeIdGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            TypeId setItem = 1;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.DatabaseDbTypeId),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.DatabaseDbTypeId = v,
                getter: x => x.DatabaseDbTypeId,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ DatabaseDbTypeId に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void DatabaseDbTypeIdSetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (TypeId)null!,
                setter: (x, v) => x.DatabaseDbTypeId = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region IsUseAdditionalItems

        /// <summary>
        ///     プロパティ IsUseAdditionalItems の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void IsUseAdditionalItemsGetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            const bool setItem = true;

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.IsUseAdditionalItems),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.IsUseAdditionalItems = v,
                getter: x => x.IsUseAdditionalItems,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        #endregion

        #region AdditionalCase1

        /// <summary>
        ///     プロパティ AdditionalCase1 の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void AdditionalCase1GetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            DatabaseValueCaseDescription setItem = "ケース1";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase1),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.AdditionalCase1 = v,
                getter: x => x.AdditionalCase1,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ AdditionalCase1 に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void AdditionalCase1SetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueCaseDescription)null!,
                setter: (x, v) => x.AdditionalCase1 = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region AdditionalCase2

        /// <summary>
        ///     プロパティ AdditionalCase2 の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void AdditionalCase2GetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            DatabaseValueCaseDescription setItem = "ケース2";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase2),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.AdditionalCase2 = v,
                getter: x => x.AdditionalCase2,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ AdditionalCase2 に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void AdditionalCase2SetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueCaseDescription)null!,
                setter: (x, v) => x.AdditionalCase2 = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region AdditionalCase3

        /// <summary>
        ///     プロパティ AdditionalCase3 の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void AdditionalCase3GetAndSetTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();
            DatabaseValueCaseDescription setItem = "ケース3";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase3),
                setItem,
                isValueEqualsBefore: false,
                setter: (x, v) => x.AdditionalCase3 = v,
                getter: x => x.AdditionalCase3,
                getValueVerifier: ValueVerifier.AreEquals(setItem)
            );
        }

        /// <summary>
        ///     プロパティ AdditionalCase3 に null を設定した場合、
        ///     PropertyNullException が発生すること。
        /// </summary>
        [Test]
        public static void AdditionalCase3SetTest_Failure_PropertyNullException()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseValueCaseDescription)null!,
                setter: (x, v) => x.AdditionalCase3 = v,
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
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings settings = CreateSettingsDto();

            // 以下、ビルドエラーが発生しないこと
            _ = settings.InitValue;
            _ = settings.DatabaseReferKind;
            _ = settings.DatabaseDbTypeId;
            _ = settings.IsUseAdditionalItems;
            _ = settings.AdditionalCase1;
            _ = settings.AdditionalCase2;
            _ = settings.AdditionalCase3;
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
            _ = dto.DatabaseReferKind;
            _ = dto.DatabaseDbTypeId;
            _ = dto.IsUseAdditionalItems;
            _ = dto.AdditionalCase1;
            _ = dto.AdditionalCase2;
            _ = dto.AdditionalCase3;
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
                factory: () => new DatabaseFieldSpecialSettingDefinitionDatabaseReference(),
                instanceVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinitionDatabaseReference>.AreItemEquals(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings()
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
            var src = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(settings);
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseFieldSpecialSettingDefinitionDatabaseReference(src),
                instanceVerifier: ValueVerifier<DatabaseFieldSpecialSettingDefinitionDatabaseReference>.AreItemEquals(
                    src
                )
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionDatabaseReference(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOのプロパティに不適切な null 要素が指定されている場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3))]
        public static void ConstructorTest_SettingsDto_Failure_NullPropertyRecord(string nullProperty)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
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

        #region GetAdditionalItem

        /// <summary>
        ///     意図した結果が取得できること。
        /// </summary>
        /// <param name="caseNumber">引数 caseNumber </param>
        /// <param name="expected">期待する結果</param>
        [TestCase(-1, "Case 1")]
        [TestCase(-2, "Case 2")]
        [TestCase(-3, "Case 3")]
        public static void GetAdditionalItemTest_Success(int caseNumber, string expected)
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                {
                    IsUseAdditionalItems = true,
                    AdditionalCase1 = "Case 1",
                    AdditionalCase2 = "Case 2",
                    AdditionalCase3 = "Case 3",
                }
            );
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetAdditionalItem(caseNumber),
                resultValueVerifier: ValueVerifier<DatabaseValueCaseDescription>.AreEquals(expected)
            );
        }

        /// <summary>
        ///     caseNumber が範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        /// <param name="caseNumber"></param>
        public static void GetAdditionalItemTest_Failure_CaseNumberOutOfRange(int caseNumber)
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                {
                    IsUseAdditionalItems = true,
                    AdditionalCase1 = "Case 1",
                    AdditionalCase2 = "Case 2",
                    AdditionalCase3 = "Case 3",
                }
            );
            pureFunctionTestHelper.PureFuncFailure(
                instance,
                execFunc: target => target.GetAdditionalItem(caseNumber),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region CanChangeFieldType

        /// <summary>
        ///     意図した結果が取得されること
        /// </summary>
        [TestCase("Int", true)]
        [TestCase("String", false)]
        public static void CanChangeFieldTypeTest_Success(string fieldTypeName, bool expected)
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());

            pureFunctionTestHelper.PureFuncFailure(
                instance,
                execFunc: target => target.CanChangeFieldType(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region UpdateAdditionalItem

        /// <summary>
        ///     正常に処理されること。
        /// </summary>
        [Test]
        public static void UpdateAdditionalItemTest_Success()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            var beforeInstance = instance.DeepClone();
            var caseNumber = -1;
            var description = "Case 1";
            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.UpdateAdditionalItem(caseNumber, description),
                expectedNotifyProperties: new[]
                    { nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReference.AdditionalCase1) },
                instanceVerifier: new ValueVerifier<DatabaseFieldSpecialSettingDefinitionDatabaseReference>(target =>
                    {
                        // 編集した追加選択肢文字列が変更されていること
                        Assert.AreEqual(new DatabaseValueCaseDescription(description), target.AdditionalCase1);
                        // 編集していない追加選択肢文字列が変更されていないこと
                        Assert.AreEqual(beforeInstance.AdditionalCase2, target.AdditionalCase2);
                        Assert.AreEqual(beforeInstance.AdditionalCase3, target.AdditionalCase3);
                    }
                )
            );
        }

        /// <summary>
        ///     caseNumber, description が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        /// <param name="nullArgName"></param>
        [TestCase("caseNumber")]
        [TestCase("description")]
        public static void UpdateAdditionalItemTest_Failure_NullArgs(string nullArgName)
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            DatabaseValueCaseNumber caseNumber = nullArgName == "caseNumber"
                ? null!
                : -1;
            DatabaseValueCaseDescription description = nullArgName == "description"
                ? null!
                : "Case 1";
            impureActionTestHelper.ImpureActionFailure(
                instance,
                execAction: target => target.UpdateAdditionalItem(caseNumber, description),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     caseNumber が指定範囲外の場合、
        ///     ArgumentOutOfRangeException が発生すること。
        /// </summary>
        /// <param name="caseNumber"></param>
        [TestCase(-4)]
        [TestCase(0)]
        public static void UpdateAdditionalItemTest_Failure_CaseNumberOutOfRange(int caseNumber)
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            DatabaseValueCaseDescription description = "Case 1";
            impureActionTestHelper.ImpureActionFailure(
                instance,
                execAction: target => target.UpdateAdditionalItem(caseNumber, description),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentOutOfRangeException))
            );
        }

        #endregion

        #region GetSpecialCases

        [Test]
        public static void GetSpecialCasesTest_Success()
        {
            const string case1 = "Case 1";
            const string case2 = "Case 2";
            const string case3 = "Case 3";

            IReadOnlyDatabaseFieldSpecialSettingDefinition instance =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                    new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
                    {
                        IsUseAdditionalItems = true,
                        AdditionalCase1 = case1,
                        AdditionalCase2 = case2,
                        AdditionalCase3 = case3,
                    }
                );
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetSpecialCases(),
                resultValueVerifier: new ValueVerifier<IEnumerable<DatabaseValueCase>>(result =>
                    {
                        var resultArray = result.ToArray();
                        Assert.AreEqual(3, resultArray.Length);
                        Assert.AreEqual(new DatabaseValueCaseNumber(-1), resultArray[0].CaseNumber);
                        Assert.AreEqual(new DatabaseValueCaseDescription(case1), resultArray[0].Description);
                        Assert.AreEqual(new DatabaseValueCaseNumber(-2), resultArray[1].CaseNumber);
                        Assert.AreEqual(new DatabaseValueCaseDescription(case2), resultArray[1].Description);
                        Assert.AreEqual(new DatabaseValueCaseNumber(-3), resultArray[2].CaseNumber);
                        Assert.AreEqual(new DatabaseValueCaseDescription(case3), resultArray[2].Description);
                    }
                )
            );
        }

        #endregion

        #region ItemEquals

        #region IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings

        /// <summary>
        ///     対象インスタンスと other が同じインスタンスの場合 true が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_True_SameObject()
        {
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings right = left;
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings right = CreateSettingsDto();
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     値が異なるプロパティを持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.IsUseAdditionalItems))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3))]
        public static void ItemEqualsTest_Settings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings right =
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.IsUseAdditionalItems))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3))]
        public static void ItemEqualsTest_SpecialSettingsDefinition_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                    CreateSettingsDto(replaceProperty: replaceProperty)
                );
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
        [TestCase(nameof(DatabaseFieldSpecialSettingType.Manual))]
        public static void ItemEqualsTest_SpecialSettingsDefinition_False_DifferType(string typeName)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IReadOnlyDatabaseFieldSpecialSettingDefinition right = typeName switch
            {
                nameof(DatabaseFieldSpecialSettingType.Normal) => new DatabaseFieldSpecialSettingDefinitionNormal(),
                nameof(DatabaseFieldSpecialSettingType.LoadFile) => new DatabaseFieldSpecialSettingDefinitionLoadFile(),
                nameof(DatabaseFieldSpecialSettingType.Manual) => new DatabaseFieldSpecialSettingDefinitionManual(),
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.IsUseAdditionalItems))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2))]
        [TestCase(nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3))]
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_False_DifferProperty(string replaceProperty)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right =
                new DatabaseFieldSpecialSettingDefinitionDatabaseReference(
                    CreateSettingsDto(replaceProperty: replaceProperty)
                );
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
        [TestCase(nameof(DatabaseFieldSpecialSettingType.Manual))]
        public static void ItemEqualsTest_SpecialSettingsDefinitionSettings_False_DifferType(string typeName)
        {
            var left = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionSettings right = typeName switch
            {
                nameof(DatabaseFieldSpecialSettingType.Normal) => new DatabaseFieldSpecialSettingDefinitionNormal(),
                nameof(DatabaseFieldSpecialSettingType.LoadFile) => new DatabaseFieldSpecialSettingDefinitionLoadFile(),
                nameof(DatabaseFieldSpecialSettingType.Manual) => new DatabaseFieldSpecialSettingDefinitionManual(),
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastDatabaseReferenceSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(true)
            );

            Assert.IsNotNull(result);
        }

        #endregion

        #region TryCastManualSettings

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastManualSettingsTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            IDatabaseFieldSpecialSettingDefinitionManualSettings? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastManualSettings(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region TryCastNormal

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastNormalTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
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
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            DatabaseFieldSpecialSettingDefinitionDatabaseReference? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastDatabaseReference(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(true)
            );

            Assert.IsNotNull(result);
        }

        #endregion

        #region TryCastManual

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void TryCastManualTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());
            DatabaseFieldSpecialSettingDefinitionManual? result = null;
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.TryCastManual(out result),
                resultValueVerifier: ValueVerifier<bool>.AreEquals(false)
            );

            Assert.IsNull(result);
        }

        #endregion

        #region DeepClone

        /// <summary>
        ///     ディープコピーがコピー元と同一値であること。
        /// </summary>
        [Test]
        public static void DeepCloneTest()
        {
            var instance = new DatabaseFieldSpecialSettingDefinitionDatabaseReference(CreateSettingsDto());

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
        private static DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings CreateSettingsDto(
            string? nullProperty = null,
            string? replaceProperty = null
        )
        {
            // @formatter:off
            return new DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings
            {
                InitValue = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.InitValue)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => 1,
                    (  false, true ) => 2,
                    (  true,  _    ) => null!,
                },
                DatabaseReferKind = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseReferKind)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => DatabaseReferType.System,
                    (  false, true ) => DatabaseReferType.Changeable,
                    (  true,  _    ) => null!,
                },
                DatabaseDbTypeId = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.DatabaseDbTypeId)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => 1,
                    (  false, true ) => 2,
                    (  true,  _    ) => null!,
                },
                IsUseAdditionalItems = replaceProperty != nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.IsUseAdditionalItems),
                AdditionalCase1 = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase1)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "ケース1",
                    (  false, true ) => "Case1",
                    (  true,  _    ) => null!,
                },
                AdditionalCase2 = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase2)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "ケース2",
                    (  false, true ) => "Case2",
                    (  true,  _    ) => null!,
                },
                AdditionalCase3 = (nullProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3), replaceProperty == nameof(DatabaseFieldSpecialSettingDefinitionDatabaseReferenceSettings.AdditionalCase3)) switch
                {
                    // null?  replace?  setValue
                    (  false, false) => "ケース3",
                    (  false, true ) => "Case3",
                    (  true,  _    ) => null!,
                },
            };
            // @formatter:on
        }

        #endregion
    }
}
