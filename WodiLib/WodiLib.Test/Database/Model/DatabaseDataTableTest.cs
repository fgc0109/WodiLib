using System;
using System.Linq;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.Model
{
    [TestFixture]
    public class DatabaseDataTableTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constants

        /// <summary>
        ///     最大データ容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MaxDataCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTable.MaxDataCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MaxDataLength)
            );
        }

        /// <summary>
        ///     最小データ容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinDataCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTable.MinDataCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MinDataLength)
            );
        }

        /// <summary>
        ///     最大項目容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MaxFieldCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTable.MaxFieldCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MaxFieldLength)
            );
        }

        /// <summary>
        ///     最小項目容量が意図した値であること。
        /// </summary>
        [Test]
        public static void MinFieldCapacityTest()
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => DatabaseDataTable.MinFieldCapacity,
                resultValueVerifier: ValueVerifier.AreEquals(DatabaseConst.MinFieldLength)
            );
        }

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
                factory: () => new DatabaseDataTable(),
                instanceVerifier: ValueVerifier<DatabaseDataTable>.AreItemEquals(new DatabaseDataTableSettings())
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
                factory: () => new DatabaseDataTable(settings),
                instanceVerifier: ValueVerifier<DatabaseDataTable>.AreItemEquals(settings)
            );
        }

        /// <summary>
        ///     settings が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SettingsDto_Failure_NullArgs()
        {
            IDatabaseDataTableSettings settings = null!;
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseDataTable(settings),
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
                factory: () => new DatabaseDataTable(settings),
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
                factory: () => new DatabaseDataTable(settings),
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
                factory: () => new DatabaseDataTable(settings),
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
                factory: () => new DatabaseDataTable(settings),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region EditableClass

        #region public

        #region GetFieldTypes

        /// <summary>
        ///     意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetFieldTypesTest_Success()
        {
            IDatabaseDataRowSettings rowSettings = new DatabaseDataRowSettings(
                new[]
                {
                    new DatabaseFieldValue(1),
                    new DatabaseFieldValue(2),
                    new DatabaseFieldValue("Three"),
                }
            );
            var instance = new DatabaseDataTable(new DatabaseDataTableSettings(new[] { rowSettings }));
            var expected = new[]
            {
                DatabaseFieldType.Int,
                DatabaseFieldType.Int,
                DatabaseFieldType.String,
            };

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetFieldTypes(),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(expected)
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
            var left = new DatabaseDataTable(CreateSettingsDto());
            IDatabaseDataTableSettings right = left;
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
            var left = new DatabaseDataTable(CreateSettingsDto());
            IDatabaseDataTableSettings right = CreateSettingsDto();
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
            var left = new DatabaseDataTable(CreateSettingsDto());
            IDatabaseDataTableSettings? right = null;
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
            var left = new DatabaseDataTable(CreateSettingsDto());
            IDatabaseDataTableSettings right = CreateSettingsDto(hasNullItem: true);
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
            var instance = new DatabaseDataTable(CreateSettingsDto());

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
                () => new DatabaseDataTableSettings(),
                instanceVerifier: new ValueVerifier<DatabaseDataTableSettings>(instance =>
                    {
                        // 要素数が最小であること
                        Assert.AreEqual(DatabaseDataTable.MinDataCapacity, instance.Settings.Count);
                        if (instance.Settings.Count > 0)
                        {
                            Assert.AreEqual(DatabaseDataTable.MinFieldCapacity, instance.Settings[0].Settings.Count);
                        }
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
        private static DatabaseDataTableSettings CreateSettingsDto(
            int rowLength = INIT_ROW_LENGTH,
            int columnLength = INIT_FIELD_LENGTH,
            bool hasDiffItem = false,
            bool hasNullItem = false
        )
        {
            return new DatabaseDataTableSettings(
                rowLength.Iterate(i => CreateSettingsRow(i, columnLength, hasDiffItem, hasNullItem)).ToArray()
            );
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
        private static IDatabaseDataRowSettings CreateSettingsRow(
            int rowIndex,
            int columnLength,
            bool requestDiffItem,
            bool requestNullItem
        )
        {
            if (rowIndex == 0 && requestDiffItem)
            {
                return new DatabaseDataRowSettings(
                    columnLength.Iterate(c => CreateSettingsItem(rowIndex, c + 10000)).ToArray()
                );
            }

            if (rowIndex == 0 && requestNullItem)
            {
                return null!;
            }

            return new DatabaseDataRowSettings(
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
