using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;
using WodiLib.Test.Tools.TestData;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseTypeMetadataTableTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     データ最大容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MaxDataCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeMetadataTable.MaxDataCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MaxDataLength)
            );
        }

        /// <summary>
        ///     データ最小容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinDataCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeMetadataTable.MinDataCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MinDataLength)
            );
        }

        /// <summary>
        ///     項目最大容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MaxFieldCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeMetadataTable.MaxFieldCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MaxFieldLength)
            );
        }

        /// <summary>
        ///     項目最小容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinFieldCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseTypeMetadataTable.MinFieldCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MinFieldLength)
            );
        }

        #endregion

        #region Properties

        #region MutableClass

        #region TypeName

        /// <summary>
        ///     プロパティ TypeName の取得・編集に成功すること。
        /// </summary>
        [Test]
        public static void TypeNameGetAndSetTest_Success()
        {
            var instance = new DatabaseTypeMetadataTable();
            TypeName setItem = "TestTypeName";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseTypeMetadataTable.TypeName),
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
            var instance = new DatabaseTypeMetadataTable();

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
            var instance = new DatabaseTypeMetadataTable();
            DatabaseMemo setItem = "TestMemo";

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseTypeMetadataTable.Memo),
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
            var instance = new DatabaseTypeMetadataTable();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseMemo)null!,
                setter: (x, v) => x.Memo = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
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
            var instance = new DatabaseTypeMetadataTable();
            var setItem = new DatabaseDataNamingDefinition(
                DatabaseDataNamingType.EqualBefore
            );

            propertyTestHelper.PropertyGetAndSetSuccess(
                instance,
                propertyName: nameof(DatabaseTypeMetadataTable.DataNamingDefinition),
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
            var instance = new DatabaseTypeMetadataTable();

            propertyTestHelper.PropertySetFailure(
                instance,
                setItem: (DatabaseDataNamingDefinition)null!,
                setter: (x, v) => x.DataNamingDefinition = v,
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PropertyNullException))
            );
        }

        #endregion

        #region FieldMetadataList

        /// <summary>
        ///     プロパティ FieldMetadataList の取得に成功すること。
        /// </summary>
        [Test]
        public static void FieldMetadataListGetAndSetTest_Success()
        {
            var instance = new DatabaseTypeMetadataTable();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: x => x.FieldMetadataList,
                getValueVerifier: null
            );
        }

        #endregion

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
                factory: () => new DatabaseTypeMetadataTable(),
                instanceVerifier: ValueVerifier<DatabaseTypeMetadataTable>.AreItemEquals(
                    new DatabaseTypeMetadataTableSettings()
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
                factory: () => new DatabaseTypeMetadataTable(settings),
                instanceVerifier: ValueVerifier<DatabaseTypeMetadataTable>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseTypeMetadataTableSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeMetadataTable(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     設定DTOに null 要素が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_HasNullItem()
        {
            var settings = CreateSettingsDto(hasNullItem: true);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeMetadataTable(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     設定DTOのデータ数が不足している場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_DataSizeUnderCapacity()
        {
            var settings = CreateSettingsDto(rowLength: DatabaseConst.MinDataLength - 1);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeMetadataTable(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     設定DTOのデータ数が超過している場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_DataSizeOverCapacity()
        {
            var settings = CreateSettingsDto(rowLength: DatabaseConst.MaxDataLength + 1);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeMetadataTable(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     設定DTOの項目数が不足している場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Ignore("最小項目数 = 0 のためテスト不要")]
        public static void ConstructorTest_SettingsDto_Failure_FieldSizeUnderCapacity()
        {
        }

        /// <summary>
        ///     設定DTOの項目数が超過している場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_FieldSizeOverCapacity()
        {
            var settings = CreateSettingsDto(columnLength: DatabaseConst.MaxFieldLength + 1);
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseTypeMetadataTable(settings),
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
            var left = new DatabaseTypeMetadataTable(CreateSettingsDto());
            IDatabaseTypeMetadataTableSettings right = left;
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
            var left = new DatabaseTypeMetadataTable(CreateSettingsDto());
            IDatabaseTypeMetadataTableSettings right = CreateSettingsDto();
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
            var left = new DatabaseTypeMetadataTable(CreateSettingsDto());
            IDatabaseTypeMetadataTableSettings? right = null;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );
        }

        /// <summary>
        ///     null 要素を持つ設定DTOと比較した場合 false が返却されること。
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Settings_False_NullItem()
        {
            var left = new DatabaseTypeMetadataTable(CreateSettingsDto());
            IDatabaseTypeMetadataTableSettings right = CreateSettingsDto(hasNullItem: true);
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
            var instance = new DatabaseTypeMetadataTable(CreateSettingsDto());

            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: null
            );
        }

        #endregion

        #endregion

        #endregion

        #endregion

        #region SettingsDto

        #region Constructor

        /// <summary>
        ///     引数なしコンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void SettingsDtoConstructorTest_Success_NoParam()
        {
            constructorTestHelper.ConstructorSuccess(
                () => new DatabaseTypeMetadataTableSettings(),
                instanceVerifier: new ValueVerifier<DatabaseTypeMetadataTableSettings>(instance =>
                    {
                        // 要素数が最小であること
                        Assert.AreEqual(DatabaseTypeMetadataTable.MinDataCapacity, instance.Settings.Count);
                        Assert.AreEqual(
                            DatabaseTypeMetadataTable.MinFieldCapacity,
                            instance.Settings[0].Settings.Count
                        );
                    }
                )
            );
        }

        #endregion

        #endregion

        #region テスト用Settings作成

        private const int INIT_ROW_LENGTH = 4;
        private const int INIT_FIELD_LENGTH = 3;

        /// <summary>
        ///     設定DTO作成
        /// </summary>
        /// <param name="rowLength">
        ///     Items に詰める行数。
        /// </param>
        /// <param name="columnLength">
        ///     Items に詰める列数。
        /// </param>
        /// <param name="hasDiffItem">
        ///     Items に通常と異なる要素をもたせるかどうか<br/>
        ///     0番目の要素を置き換える
        /// </param>
        /// <param name="hasNullItem">
        ///     Items に null 要素をもたせるかどうか<br/>
        ///     0番目の要素を null にする
        /// </param>
        /// <returns></returns>
        private static DatabaseTypeMetadataTableSettings CreateSettingsDto(
            int rowLength = INIT_ROW_LENGTH,
            int columnLength = INIT_FIELD_LENGTH,
            bool hasDiffItem = false,
            bool hasNullItem = false
        )
        {
            var namedDataRowSettingsList =
                rowLength.Iterate(i => CreateSettingsRow(i, columnLength, hasDiffItem, hasNullItem)).ToArray();
            var fieldMetadataSettingsList = columnLength.Iterate<IDatabaseFieldMetadataSettings>(i
                    => new DatabaseFieldMetadataSettings
                    {
                        FieldName = $"FieldName{i}",
                        FieldMemo = $"FieldMemo_{i}",
                        SpecialSettingDefinition =
                            DatabaseTestData.CreateDatabaseFieldSpecialSettingDefinitionSettingsType1(
                                initValue: new DatabaseValueInt(234)
                            ),
                    }
                )
                .ToArray();

            return new DatabaseTypeMetadataTableSettings(
                namedDataRowSettingsList
            )
            {
                TypeName = "TestTypeName",
                Memo = "TestMemo",
                DataNamingDefinition = new DatabaseDataNamingDefinition(DatabaseDataNamingType.EqualBefore),
                FieldMetadataList = new DatabaseFieldMetadataListSettings(fieldMetadataSettingsList),
            };
        }

        /// <summary>
        ///     設定DTOの行要素作成
        /// </summary>
        /// <param name="rowIndex">要素番号</param>
        /// <param name="columnLength">列数</param>
        /// <param name="requestDiffItem">
        ///     Items に通常と異なる要素をもたせるかどうか<br/>
        ///     0番目の要素を置き換える
        /// </param>
        /// <param name="requestNullItem">
        ///     Items に null 要素をもたせるかどうか<br/>
        ///     0番目の要素を null にする
        /// </param>
        /// <returns></returns>
        private static IDatabaseNamedDataRowSettings CreateSettingsRow(
            int rowIndex,
            int columnLength,
            bool requestDiffItem,
            bool requestNullItem
        )
        {
            if (rowIndex == 0 && requestDiffItem)
            {
                return new DatabaseNamedDataRowSettings(
                    columnLength.Iterate(c => CreateSettingsItem(rowIndex, c + 10000)).ToArray()
                );
            }

            if (rowIndex == 0 && requestNullItem)
            {
                return null!;
            }

            return new DatabaseNamedDataRowSettings(
                columnLength.Iterate(c => CreateSettingsItem(rowIndex, c)).ToArray()
            );
        }

        private static DatabaseFieldValue CreateSettingsItem(
            int rowIndex,
            int columnIndex
        )
        {
            return new DatabaseFieldValue(rowIndex * columnIndex);
        }

        #endregion
    }
}
