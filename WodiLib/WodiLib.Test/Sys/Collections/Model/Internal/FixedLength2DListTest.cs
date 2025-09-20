using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using Test2DList = WodiLib.Sys.Collections.FixedLength2DList<
    WodiLib.Test.Tools.StubRestrictedCapacityList,
    WodiLib.Test.Tools.FixedStubRestrictedCapacityList,
    WodiLib.Test.Tools.ReadOnlyStubRestrictedCapacityList,
    WodiLib.Test.Tools.IStubRestrictedCapacityListSettings,
    WodiLib.Test.Tools.StubModel,
    WodiLib.Test.Tools.ReadOnlyStubModel,
    WodiLib.Test.Tools.IStubModelSettings
>;

namespace WodiLib.Test.Sys.Collections
{
    /*
     * 各メソッドの引数検証は行わない、いずれもエラーとならない引数のみを指定してテストする。
     *      => 各メソッドの引数検証はコンストラクタで与えるValidatorによって決まるため
     * ただし、コンストラクタから Mock Validator を注入し、Validator の意図したメソッドが呼ばれることを検証する。
     *
     * ReadOnly2DList で実装されており、オーバーライド等していないメソッドのテストは行わない。
     */

    [TestFixture]
    public class FixedLength2DListTest
    {
        private static Logger logger = null!;

        private static PropertyTestHelper propertyTestHelper = null!;
        private static ConstructorTestHelper constructorTestHelper = null!;
        private static PureActionTestHelper pureActionTestHelper = null!;
        private static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private static ImpureActionTestHelper impureActionTestHelper = null!;
        private static ImpureFunctionTestHelper impureFunctionTestHelper = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            propertyTestHelper = new PropertyTestHelper(logger);
            constructorTestHelper = new ConstructorTestHelper(logger);
            pureActionTestHelper = new PureActionTestHelper(logger);
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            impureActionTestHelper = new ImpureActionTestHelper(logger);
            impureFunctionTestHelper = new ImpureFunctionTestHelper(logger);
        }

        #region Properties

        #region public

        #region RowIndexer

        /// <summary>
        ///     <para>行インデクサの取得・編集に成功すること。</para>
        ///     <para>取得結果が意図した値であること。</para>
        ///     <para>Validatorのメソッドが意図したとおり呼ばれること。</para>
        /// </summary>
        [Test]
        public static void RowIndexerGetterAndSetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            var settings = TestClass.BuildItemFromIndex(
                rowIndex + 100
            );

            /*
             * PropertyTestHelper.PropertyGetAndSetSuccess を使用せず、
             * 自前の検証処理を行う。
             * PropertyTestHelper.PropertyGetAndSetSuccess を使用してチェックすると、
             * インデクサーを通した再代入の際にもプロパティ変更通知が発生するためテストNGとなってしまうため。
             */

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName!); };

            var errorOccured = false;
            try
            {
                instance[rowIndex] = settings;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していること
            Assert.AreNotEqual(0, changedPropertyList.Count);
            Assert.AreEqual(
                ListConstant.IndexerName,
                changedPropertyList[0]
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.SetRow)
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            CustomAssert.AreSequenceEquals(
                new IStubRestrictedCapacityListSettings[] { settings },
                (IStubRestrictedCapacityListSettings[])testClass.MockValidator.CalledMemberHistory[0].Args[1],
                EqualityComparerFactory.Create<IStubRestrictedCapacityListSettings>()
            );

            // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
            CustomAssert.AreItemEquals(settings, testClass.InnerList[rowIndex]);
            Assert.AreNotSame(settings, testClass.InnerList[rowIndex]);

            // 編集していない行要素が変更されていないこと
            for (var i = 0; i < testClass.InnerList.Count; i++)
            {
                if (i != rowIndex)
                {
                    Assert.AreSame(initRows[i], instance.EditableRows[i]);
                }
            }
        }

        #endregion

        #region Indexer Cell

        /// <summary>
        ///     <para>セルインデクサの取得・編集に成功すること。</para>
        ///     <para>取得結果が意図した値であること。</para>
        ///     <para>Validatorのメソッドが意図したとおり呼ばれること。</para>
        /// </summary>
        [Test]
        public static void CellIndexerGetterAndSetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;
            var settings = new StubModel
            {
                StringValue = "Update Cell",
            };

            var changedPropertyList = new List<string>();
            instance.PropertyChanged += (_, args) => { changedPropertyList.Add(args.PropertyName!); };

            var errorOccured = false;
            try
            {
                instance[rowIndex, columnIndex] = settings;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火しないこと
            Assert.AreEqual(0, changedPropertyList.Count);

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.SetCell)
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(columnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            CustomAssert.AreItemEquals(
                settings,
                (IStubModelSettings)testClass.MockValidator.CalledMemberHistory[0].Args[2]
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory.Any(x =>
                    x.MethodName == nameof(testClass.MockValidator.SetCell)
                )
            );
            // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
            CustomAssert.AreItemEquals(settings, testClass.InnerList[rowIndex][columnIndex]);
            Assert.AreNotSame(settings, testClass.InnerList[rowIndex][columnIndex]);
        }

        #endregion

        #region EditableItems

        /// <summary>
        ///     EditableItemsプロパティが正常に取得されること。
        /// </summary>
        [Test]
        public static void EditableItemsGetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.EditableRows,
                getValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList[]>(actual =>
                    {
                        Assert.AreEqual(testClass.InnerList.Count, actual.Length);
                        for (var i = 0; i < actual.Length; i++)
                        {
                            Assert.AreSame(testClass.InnerList[i], actual[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        ///     <para>コンストラクタが正常に終了すること。</para>
        ///     <para>Items プロパティの実態がコンストラクタで与えた SimpleList であること。</para>
        /// </summary>
        [Test]
        public static void ConstructorTest_SimpleListAndConfig_Success()
        {
            var itemsImpl = new SimpleList<StubRestrictedCapacityList>(
                valueBuilder: new SimpleListValueBuilder<StubRestrictedCapacityList>((list, _)
                    => new StubRestrictedCapacityList(list.Count)
                )
            );
            var config = TestClass.CreateConfig(null);

            constructorTestHelper.ConstructorSuccess(
                factory: () => new Test2DList(itemsImpl, config),
                instanceVerifier: new ValueVerifier<Test2DList>(instance =>
                    {
                        // itemsImpl に対する変更が instance にも反映されること
                        var notifiedPropertyChanged = new List<string>();
                        instance.PropertyChanged += (_, args) => { notifiedPropertyChanged.Add(args.PropertyName!); };

                        var addIndex = itemsImpl.Count;
                        var addItem = new StubRestrictedCapacityList(TestClass.INIT_COLUMN_LENGTH);

                        itemsImpl.Add(addItem);

                        // プロパティ変更通知が行われること
                        Assert.AreEqual(2, notifiedPropertyChanged.Count);
                        Assert.AreEqual(nameof(instance.RowCount), notifiedPropertyChanged[0]);
                        Assert.AreEqual(ListConstant.IndexerName, notifiedPropertyChanged[1]);

                        // instance の要素が追加されていること
                        Assert.AreSame(addItem, instance[addIndex]);
                    }
                )
            );
        }

        /// <summary>
        ///     引数が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [TestCase("itemsImpl")]
        [TestCase("config")]
        public static void ConstructorTest_SimpleListAndConfig_Failure_NullArgs(string nullArgName)
        {
            var itemsImpl = nullArgName != "itemsImpl"
                ? new SimpleList<StubRestrictedCapacityList>(
                    valueBuilder: new SimpleListValueBuilder<StubRestrictedCapacityList>((list, _)
                        => new StubRestrictedCapacityList(list.Count)
                    )
                )
                : null!;
            var config = nullArgName != "config"
                ? TestClass.CreateConfig(null)
                : null!;

            constructorTestHelper.ConstructorFailure(
                factory: () => new Test2DList(itemsImpl, config),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region Methods

        #region public

        #region GetRow

        /// <summary>
        ///     <para>指定した行が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRow(rowIndex),
                resultValueVerifier: ValueVerifier<FixedStubRestrictedCapacityList>.AreReferenceEquals(
                    testClass.InnerList[rowIndex]
                )
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region GetRowRange

        /// <summary>
        ///     <para>指定した範囲の行が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 0;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRowRange(rowIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[0], actualArray[0]);
                        Assert.AreSame(testClass.InnerList[1], actualArray[1]);
                    }
                )
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(count, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region GetColumn

        /// <summary>
        ///     <para>指定した列が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumn(columnIndex),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(testClass.InnerList.Count, actualArray.Length);
                        for (var i = 0; i < actualArray.Length; i++)
                        {
                            Assert.AreSame(testClass.InnerList[i][columnIndex], actualArray[i]);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region GetColumnRange

        /// <summary>
        ///     <para>指定した範囲の行が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 0;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumnRange(columnIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.To2DArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreEqual(testClass.InnerList.Count, actualArray[0].Length);
                        for (var i = 0; i < count; i++)
                        {
                            for (var j = 0; j < testClass.InnerList.Count; j++)
                            {
                                Assert.AreSame(
                                    testClass.InnerList[j][columnIndex + i],
                                    actualArray[i][j],
                                    $"i: {i}, j: {j}"
                                );
                            }
                        }
                    }
                )
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(count, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region GetCell

        /// <summary>
        ///     <para>指定したセルが取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetCellTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetCell(rowIndex, columnIndex),
                resultValueVerifier: ValueVerifier<StubModel>.AreReferenceEquals(
                    testClass.InnerList[rowIndex].GetInternal(columnIndex)
                )
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetCell),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(columnIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region SetRow

        /// <summary>
        ///     <para>指定した行が設定されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            var settings = TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRow(rowIndex, settings),
                resultValueVerifier: ValueVerifier<FixedStubRestrictedCapacityList>.AreReferenceEquals(()
                    => testClass.InnerList[rowIndex]
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.SetRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        CustomAssert.AreItemEquals(target[rowIndex], settings);
                        Assert.AreNotEqual(target[rowIndex], settings);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < testClass.InnerList.Count; i++)
                        {
                            if (i != rowIndex)
                            {
                                Assert.AreSame(initRows[i], target.EditableRows[i]);
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region SetRowRange

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetRowRangeTest_Success_EmptySettings()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 0;
            var settings = Array.Empty<IStubRestrictedCapacityListSettings>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.SetRowRange(rowIndex, settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     <para>指定した範囲の行が設定されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetRowRangeTest_Success_NotEmptySettings()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 0;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                => TestClass.BuildRowSettingsFromRowIndex(100 + i)
            );

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRowRange(rowIndex, settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settingsLength, actualArray.Length);
                        for (var i = 0; i < settingsLength; i++)
                        {
                            Assert.AreSame(testClass.InnerList[rowIndex + i], actualArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.SetRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < testClass.InnerList.Count; i++)
                        {
                            if (!i.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                Assert.AreSame(initRows[i], target.EditableRows[i]);
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region SetColumn

        /// <summary>
        ///     <para>指定した行が設定されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(columnIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetColumn(columnIndex, settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<IStubModelSettings>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        for (var i = 0; i < actualArray.Length; i++)
                        {
                            CustomAssert.AreItemEquals(actualArray[i], settings[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.SetColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            new[] { settings },
                            ((IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1])
                        );
                    }
                )
            );
        }

        #endregion

        #region SetColumnRange

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetColumnRangeTest_Success_EmptySettings()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 0;
            var settings = Array.Empty<IEnumerable<IStubModelSettings>>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.SetColumnRange(columnIndex, settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.IsEmpty((IEnumerable<IStubModelSettings>[])testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     <para>指定した範囲の行が設定されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetColumnRangeTest_Success_NotEmptySettings()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 0;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildColumnSettingsFromColumnIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetColumnRange(columnIndex, settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<IStubModelSettings>>>(actual =>
                    {
                        var actualArray = actual.To2DArray();
                        Assert.AreEqual(settingsLength, actualArray.Length);
                        for (var i = 0; i < settingsLength; i++)
                        {
                            for (var j = 0; j < settings[i].Length; j++)
                            {
                                CustomAssert.AreItemEquals(
                                    testClass.InnerList[j][columnIndex + i],
                                    actualArray[i][j],
                                    $"i: {i}, j: {j}"
                                );
                            }
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.SetColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );
                    }
                )
            );
        }

        #endregion

        #region SetCell

        /// <summary>
        ///     <para>指定したセルが設定されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetCellTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;
            var settings = new StubModelSettings
            {
                StringValue = "Update Cell",
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetCell(rowIndex, columnIndex, settings),
                resultValueVerifier: ValueVerifier<StubModel>.AreItemEquals(()
                    => testClass.InnerList[rowIndex].GetInternal(columnIndex)
                ),
                // セルを直接編集したときはプロパティ変更通知は起こらない
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.SetCell),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(columnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreSame(
                            settings,
                            (IStubModelSettings)testClass.MockValidator.CalledMemberHistory[0].Args[2]
                        );
                    }
                )
            );
        }

        #endregion

        #region MoveRow

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowTest_Success_MoveFront()
        {
            var testClass = new TestClass(rowCount: 4);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int oldRowIndex = 2;
            const int newRowIndex = 0;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRow(oldRowIndex, newRowIndex),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 行要素が正しく移動していること
                        Assert.AreSame(initRows[2], testClass.InnerList[0]);
                        Assert.AreSame(initRows[0], testClass.InnerList[1]);
                        Assert.AreSame(initRows[1], testClass.InnerList[2]);
                        Assert.AreSame(initRows[3], testClass.InnerList[3]);
                    }
                )
            );
        }

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowTest_Success_MoveBack()
        {
            var testClass = new TestClass(rowCount: 4);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int oldRowIndex = 0;
            const int newRowIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRow(oldRowIndex, newRowIndex),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 行要素が正しく移動していること
                        Assert.AreSame(initRows[1], testClass.InnerList[0]);
                        Assert.AreSame(initRows[2], testClass.InnerList[1]);
                        Assert.AreSame(initRows[0], testClass.InnerList[2]);
                        Assert.AreSame(initRows[3], testClass.InnerList[3]);
                    }
                )
            );
        }

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowTest_Success_NoMove()
        {
            var testClass = new TestClass(rowCount: 4);
            var instance = testClass.TestInstance;
            const int oldRowIndex = 2;
            const int newRowIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.MoveRow(oldRowIndex, newRowIndex)
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region MoveRowRange

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowRangeTest_Success_MoveFront()
        {
            var testClass = new TestClass(rowCount: 5);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int oldRowIndex = 2;
            const int newRowIndex = 0;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRowRange(oldRowIndex, newRowIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 行要素が正しく移動していること
                        Assert.AreSame(initRows[0], testClass.InnerList[2]);
                        Assert.AreSame(initRows[1], testClass.InnerList[3]);
                        Assert.AreSame(initRows[2], testClass.InnerList[0]);
                        Assert.AreSame(initRows[3], testClass.InnerList[1]);
                        Assert.AreSame(initRows[4], testClass.InnerList[4]);
                    }
                )
            );
        }

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowRangeTest_Success_MoveBack()
        {
            var testClass = new TestClass(rowCount: 6);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int oldRowIndex = 0;
            const int newRowIndex = 2;
            const int count = 3;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRowRange(oldRowIndex, newRowIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 行要素が正しく移動していること
                        Assert.AreSame(initRows[3], testClass.InnerList[0]);
                        Assert.AreSame(initRows[4], testClass.InnerList[1]);
                        Assert.AreSame(initRows[0], testClass.InnerList[2]);
                        Assert.AreSame(initRows[1], testClass.InnerList[3]);
                        Assert.AreSame(initRows[2], testClass.InnerList[4]);
                        Assert.AreSame(initRows[5], testClass.InnerList[5]);
                    }
                )
            );
        }

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowRangeTest_Success_NoMove_SameIndex()
        {
            var testClass = new TestClass(rowCount: 4);
            var instance = testClass.TestInstance;
            const int oldRowIndex = 2;
            const int newRowIndex = 2;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.MoveRowRange(oldRowIndex, newRowIndex, count)
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRowRangeTest_Success_NoMove_CountZero()
        {
            var testClass = new TestClass(rowCount: 4);
            var instance = testClass.TestInstance;
            const int oldRowIndex = 0;
            const int newRowIndex = 2;
            const int count = 0;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.MoveRowRange(oldRowIndex, newRowIndex, count)
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newRowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region MoveColumn

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnTest_Success_MoveFront()
        {
            var testClass = new TestClass(columnCount: 4);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(row => row.ToArray()).ToArray();
            const int oldColumnIndex = 2;
            const int newColumnIndex = 0;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveColumn(oldColumnIndex, newColumnIndex),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 列要素が正しく移動していること
                        for (var r = 0; r < initRows.Length; r++)
                        {
                            Assert.AreSame(initRows[r][2], testClass.InnerList[r][0]);
                            Assert.AreSame(initRows[r][0], testClass.InnerList[r][1]);
                            Assert.AreSame(initRows[r][1], testClass.InnerList[r][2]);
                            Assert.AreSame(initRows[r][3], testClass.InnerList[r][3]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnTest_Success_MoveBack()
        {
            var testClass = new TestClass(columnCount: 4);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(row => row.ToArray()).ToArray();
            const int oldColumnIndex = 0;
            const int newColumnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveColumn(oldColumnIndex, newColumnIndex),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 列要素が正しく移動していること
                        for (var r = 0; r < initRows.Length; r++)
                        {
                            Assert.AreSame(initRows[r][1], testClass.InnerList[r][0]);
                            Assert.AreSame(initRows[r][2], testClass.InnerList[r][1]);
                            Assert.AreSame(initRows[r][0], testClass.InnerList[r][2]);
                            Assert.AreSame(initRows[r][3], testClass.InnerList[r][3]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnTest_Success_NoMove()
        {
            var testClass = new TestClass(columnCount: 4);
            var instance = testClass.TestInstance;
            const int oldColumnIndex = 2;
            const int newColumnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.MoveColumn(oldColumnIndex, newColumnIndex)
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region MoveColumnRange

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnRangeTest_Success_MoveFront()
        {
            var testClass = new TestClass(columnCount: 5);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(row => row.ToArray()).ToArray();
            const int oldColumnIndex = 2;
            const int newColumnIndex = 0;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveColumnRange(oldColumnIndex, newColumnIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 列要素が正しく移動していること
                        for (var r = 0; r < initRows.Length; r++)
                        {
                            Assert.AreSame(initRows[r][2], testClass.InnerList[r][0]);
                            Assert.AreSame(initRows[r][3], testClass.InnerList[r][1]);
                            Assert.AreSame(initRows[r][0], testClass.InnerList[r][2]);
                            Assert.AreSame(initRows[r][1], testClass.InnerList[r][3]);
                            Assert.AreSame(initRows[r][4], testClass.InnerList[r][4]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>指定した行が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnRangeTest_Success_MoveBack()
        {
            var testClass = new TestClass(columnCount: 6);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(row => row.ToArray()).ToArray();
            const int oldColumnIndex = 0;
            const int newColumnIndex = 2;
            const int count = 3;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveColumnRange(oldColumnIndex, newColumnIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.MoveColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 列要素が正しく移動していること
                        for (var r = 0; r < initRows.Length; r++)
                        {
                            Assert.AreSame(initRows[r][3], testClass.InnerList[r][0]);
                            Assert.AreSame(initRows[r][4], testClass.InnerList[r][1]);
                            Assert.AreSame(initRows[r][0], testClass.InnerList[r][2]);
                            Assert.AreSame(initRows[r][1], testClass.InnerList[r][3]);
                            Assert.AreSame(initRows[r][2], testClass.InnerList[r][4]);
                            Assert.AreSame(initRows[r][5], testClass.InnerList[r][5]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnRangeTest_Success_NoMove_SameIndex()
        {
            var testClass = new TestClass(columnCount: 4);
            var instance = testClass.TestInstance;
            const int oldColumnIndex = 2;
            const int newColumnIndex = 2;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.MoveColumnRange(oldColumnIndex, newColumnIndex, count)
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        /// <summary>
        ///     <para>状態が変化しないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveColumnRangeTest_Success_NoMove_CountZero()
        {
            var testClass = new TestClass(columnCount: 4);
            var instance = testClass.TestInstance;
            const int oldColumnIndex = 0;
            const int newColumnIndex = 2;
            const int count = 0;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.MoveColumnRange(oldColumnIndex, newColumnIndex, count)
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newColumnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region Reset

        #region WithSettings

        /// <summary>
        ///     <para>指定した設定でリセットされること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_WithSettings_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH.Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 + r)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IStubRestrictedCapacityListSettings[])testClass.MockValidator.CalledMemberHistory[0]
                                .Args[0]
                        );
                        Assert.AreEqual(false, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                    }
                )
            );
        }

        #endregion

        #region NoParam

        /// <summary>
        ///     <para>デフォルト設定でリセットされること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_Parameterless_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            // 先に要素を変更しておく
            instance.SetRow(0, TestClass.BuildRowSettingsFromRowIndex(100));
            instance.SetRow(1, TestClass.BuildRowSettingsFromRowIndex(200));

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(TestClass.INIT_ROW_LENGTH, actualArray.Length);
                        for (var i = 0; i < actualArray.Length; i++)
                        {
                            Assert.AreSame(actualArray[i], testClass.InnerList[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                    }
                )
            );
        }

        #endregion

        #endregion

        #region ValidateSetRow

        /// <summary>
        ///     <para>ValidateSetRow メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            var settings = TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSetRow(rowIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, ((object[])testClass.MockValidator.CalledMemberHistory[0].Args[1])[0]);
        }

        #endregion

        #region ValidateSetRowRange

        /// <summary>
        ///     <para>ValidateSetRowRange メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 0;
            var settings = new[]
            {
                TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100),
                TestClass.BuildRowSettingsFromRowIndex(rowIndex + 200),
            };

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSetRowRange(rowIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateSetColumn

        /// <summary>
        ///     <para>ValidateSetColumn メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;
            var settings = new[]
            {
                new StubModelSettings { StringValue = "Column Cell 1" },
                new StubModelSettings { StringValue = "Column Cell 2" },
                new StubModelSettings { StringValue = "Column Cell 3" },
            };

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSetColumn(columnIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            CustomAssert.AreSequenceEquals(
                new[] { settings },
                (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0].Args[1]
            );
        }

        #endregion

        #region ValidateSetColumnRange

        /// <summary>
        ///     <para>ValidateSetColumnRange メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            var settings = new[]
            {
                new[]
                {
                    new StubModelSettings { StringValue = "Col1 Row1" },
                    new StubModelSettings { StringValue = "Col1 Row2" },
                    new StubModelSettings { StringValue = "Col1 Row3" },
                },
                new[]
                {
                    new StubModelSettings { StringValue = "Col2 Row1" },
                    new StubModelSettings { StringValue = "Col2 Row2" },
                    new StubModelSettings { StringValue = "Col2 Row3" },
                },
            };

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSetColumnRange(columnIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateSetCell

        /// <summary>
        ///     <para>ValidateSetCell メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetCellTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;
            var settings = new StubModelSettings
            {
                StringValue = "Update Cell",
            };

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSetCell(rowIndex, columnIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.SetCell),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateMoveRow

        /// <summary>
        ///     <para>ValidateMoveRow メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateMoveRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldRowIndex = 0;
            const int newRowIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateMoveRow(oldRowIndex, newRowIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldRowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newRowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateMoveRowRange

        /// <summary>
        ///     <para>ValidateMoveRowRange メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateMoveRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldRowIndex = 0;
            const int newRowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateMoveRowRange(oldRowIndex, newRowIndex, count)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldRowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newRowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateMoveColumn

        /// <summary>
        ///     <para>ValidateMoveColumn メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateMoveColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldColumnIndex = 1;
            const int newColumnIndex = 3;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateMoveColumn(oldColumnIndex, newColumnIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldColumnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newColumnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateMoveColumnRange

        /// <summary>
        ///     <para>ValidateMoveColumnRange メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateMoveColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldColumnIndex = 0;
            const int newColumnIndex = 2;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateMoveColumnRange(oldColumnIndex, newColumnIndex, count)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.MoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldColumnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newColumnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateReset

        /// <summary>
        ///     <para>ValidateReset（設定あり）メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateResetTest_WithSettings_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = new[]
            {
                TestClass.BuildRowSettingsFromRowIndex(100),
                TestClass.BuildRowSettingsFromRowIndex(200),
                TestClass.BuildRowSettingsFromRowIndex(300),
            };

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateReset(settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.Reset),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(false, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     <para>ValidateReset（設定なし）メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateResetTest_Parameterless_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateReset()
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.Reset),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
        }

        #endregion

        #region GetRowInternal

        /// <summary>
        ///     <para>GetRowInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRowInternal(rowIndex),
                resultValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList>(actual =>
                    Assert.AreSame(actual, testClass.InnerList[rowIndex])
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetRowRangeInternal

        /// <summary>
        ///     <para>GetRowRangeInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 0;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRowRangeInternal(rowIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[0], actualArray[0]);
                        Assert.AreSame(testClass.InnerList[1], actualArray[1]);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetColumnInternal

        /// <summary>
        ///     <para>GetColumnInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumnInternal(columnIndex),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(testClass.InnerList.Count, actualArray.Length);

                        for (var i = 0; i < testClass.InnerList.Count; i++)
                        {
                            var expectedCell = testClass.InnerList[i].GetInternal(columnIndex);
                            Assert.AreSame(expectedCell, actualArray[i]);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetColumnRangeInternal

        /// <summary>
        ///     <para>GetColumnRangeInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetColumnRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumnRangeInternal(columnIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);

                        for (var col = 0; col < count; col++)
                        {
                            var columnArray = actualArray[col].ToArray();
                            Assert.AreEqual(testClass.InnerList.Count, columnArray.Length);

                            for (var row = 0; row < testClass.InnerList.Count; row++)
                            {
                                var expectedCell = testClass.InnerList[row].GetInternal(columnIndex + col);
                                Assert.AreSame(expectedCell, columnArray[row]);
                            }
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetCellInternal

        /// <summary>
        ///     <para>GetCellInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetCellInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetCellInternal(rowIndex, columnIndex),
                resultValueVerifier: new ValueVerifier<StubModel>(actual =>
                    {
                        var expectedCell = testClass.InnerList[rowIndex].GetInternal(columnIndex);
                        Assert.AreSame(expectedCell, actual);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region SetRowInternal

        /// <summary>
        ///     <para>SetRowInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            var settings = TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRowInternal(rowIndex, settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList>(actual =>
                    Assert.AreSame(testClass.InnerList[rowIndex], actual)
                ),
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < testClass.InnerList.Count; i++)
                        {
                            if (i != rowIndex)
                            {
                                Assert.AreSame(initRows[i], target.EditableRows[i]);
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region SetRowRangeInternal

        /// <summary>
        ///     <para>SetRowRangeInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 0;
            var settings = new[]
            {
                TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100),
                TestClass.BuildRowSettingsFromRowIndex(rowIndex + 200),
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRowRangeInternal(rowIndex, settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[rowIndex], actualArray[0]);
                        Assert.AreSame(testClass.InnerList[rowIndex + 1], actualArray[1]);
                    }
                ),
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < testClass.InnerList.Count; i++)
                        {
                            if (!i.IsBetween(rowIndex, rowIndex + settings.Length - 1))
                            {
                                Assert.AreSame(initRows[i], target.EditableRows[i]);
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region SetColumnInternal

        /// <summary>
        ///     <para>SetColumnInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;
            var settings = TestClass.INIT_ROW_LENGTH
                .Iterate(i => new StubModelSettings { StringValue = $"Column Cell {i}" })
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetColumnInternal(columnIndex, settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(testClass.InnerList.Count, actualArray.Length);

                        for (var i = 0; i < actualArray.Length; i++)
                        {
                            var expectedCell = testClass.InnerList[i].GetInternal(columnIndex);
                            Assert.AreSame(expectedCell, actualArray[i]);
                        }
                    }
                ),
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
                    }
                )
            );
        }

        #endregion

        #region SetColumnRangeInternal

        /// <summary>
        ///     <para>SetColumnRangeInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetColumnRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            var settings = 2.Iterate(c =>
                    TestClass.INIT_ROW_LENGTH.Iterate(i => new StubModelSettings { StringValue = $"Column Cell {c}{i}" }
                    )
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetColumnRangeInternal(columnIndex, settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);

                        foreach (var actualColumns in actualArray)
                        {
                            var columnArray = actualColumns.ToArray();
                            Assert.AreEqual(testClass.InnerList.Count, columnArray.Length);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region SetCellInternal

        /// <summary>
        ///     <para>SetCellInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetCellInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;
            var settings = new StubModelSettings
            {
                StringValue = "Update Cell",
            };

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.SetCellInternal(rowIndex, columnIndex, settings),
                resultValueVerifier: new ValueVerifier<StubModel>(actual =>
                    {
                        var expectedCell = testClass.InnerList[rowIndex].GetInternal(columnIndex);
                        Assert.AreSame(expectedCell, actual);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region MoveRowInternal

        /// <summary>
        ///     <para>MoveRowInternal メソッドが正常に処理されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void MoveRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int oldRowIndex = 0;
            const int newRowIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRowInternal(oldRowIndex, newRowIndex),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行要素が正しく移動していること
                        Assert.AreSame(initRows[1], testClass.InnerList[0]);
                        Assert.AreSame(initRows[2], testClass.InnerList[1]);
                        Assert.AreSame(initRows[0], testClass.InnerList[2]);
                    }
                )
            );
        }

        #endregion

        #region MoveRowRangeInternal

        /// <summary>
        ///     <para>MoveRowRangeInternal メソッドが正常に処理されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void MoveRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldRowIndex = 0;
            const int newRowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRowRangeInternal(oldRowIndex, newRowIndex, count),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<Test2DList>(_ =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
                    }
                )
            );
        }

        #endregion

        #region MoveColumnInternal

        /// <summary>
        ///     <para>MoveColumnInternal メソッドが正常に処理されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void MoveColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(row => row.ToArray()).ToArray();
            const int oldColumnIndex = 1;
            const int newColumnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveColumnInternal(oldColumnIndex, newColumnIndex),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列が入れ替わっていること
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            Assert.AreSame(initRows[r][0], target[r][0], $"Row {r}");
                            Assert.AreSame(initRows[r][2], target[r][1], $"Row {r}");
                            Assert.AreSame(initRows[r][1], target[r][2], $"Row {r}");
                        }
                    }
                )
            );
        }

        #endregion

        #region MoveColumnRangeInternal

        /// <summary>
        ///     <para>MoveColumnRangeInternal メソッドが正常に処理されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void MoveColumnRangeInternalTest_Success()
        {
            var testClass = new TestClass(columnCount: 5);
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(row => row.ToArray()).ToArray();
            const int oldColumnIndex = 0;
            const int newColumnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveColumnRangeInternal(oldColumnIndex, newColumnIndex, count),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列が入れ替わっていること
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            Assert.AreSame(initRows[r][2], target[r][0], $"Row {r}");
                            Assert.AreSame(initRows[r][0], target[r][1], $"Row {r}");
                            Assert.AreSame(initRows[r][1], target[r][2], $"Row {r}");
                            Assert.AreSame(initRows[r][3], target[r][3], $"Row {r}");
                            Assert.AreSame(initRows[r][4], target[r][4], $"Row {r}");
                        }
                    }
                )
            );
        }

        #endregion

        #region ResetInternal

        /// <summary>
        ///     <para>ResetInternal（設定あり）メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ResetInternalTest_WithSettings_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH.Iterate(i => TestClass.BuildRowSettingsFromRowIndex(i + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.ResetInternal(settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            Assert.AreSame(testClass.InnerList[i], actualArray[i]);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        /// <summary>
        ///     <para>ResetInternal（設定なし）メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ResetInternalTest_Parameterless_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            // 先に要素を変更しておく
            instance.SetRowInternal(0, TestClass.BuildRowSettingsFromRowIndex(100));
            instance.SetRowInternal(1, TestClass.BuildRowSettingsFromRowIndex(200));

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.ResetInternal(),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(TestClass.INIT_ROW_LENGTH, actualArray.Length);
                        for (var i = 0; i < actualArray.Length; i++)
                        {
                            Assert.AreSame(actualArray[i], testClass.InnerList[i]);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        ///     テスト用クラス
        /// </summary>
        private class TestClass
        {
            public const int MAX_ROW_CAPACITY = 10;
            public const int MIN_ROW_CAPACITY = 1;
            public const int MAX_COLUMN_CAPACITY = 7;
            public const int MIN_COLUMN_CAPACITY = 2;

            public const int INIT_ROW_LENGTH = 4;
            public const int INIT_COLUMN_LENGTH = 3;

            public Test2DList TestInstance { get; }

            public MockWodiLib2DListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings> MockValidator
            {
                get;
            }

            public SimpleList<StubRestrictedCapacityList> InnerList { get; }

            public TestClass(int rowCount = INIT_ROW_LENGTH, int columnCount = INIT_COLUMN_LENGTH)
            {
                var validator =
                    new MockWodiLib2DListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>();
                MockValidator = validator;

                var innerList = new SimpleList<StubRestrictedCapacityList>(
                    valueBuilder: RowBuilder,
                    initValues: rowCount.Iterate(rowIndex => BuildItemFromIndex(rowIndex, columnCount))
                );
                InnerList = innerList;

                TestInstance = new Test2DList(
                    innerList,
                    CreateConfig(validator)
                );
            }

            public static ReadOnly2DList<StubRestrictedCapacityList, FixedStubRestrictedCapacityList,
                ReadOnlyStubRestrictedCapacityList, IStubRestrictedCapacityListSettings, StubModel, ReadOnlyStubModel,
                IStubModelSettings>.Config CreateConfig(
                MockWodiLib2DListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>? validator
            )
            {
                return new ReadOnly2DList<StubRestrictedCapacityList, FixedStubRestrictedCapacityList,
                    ReadOnlyStubRestrictedCapacityList, IStubRestrictedCapacityListSettings, StubModel,
                    ReadOnlyStubModel, IStubModelSettings>.Config(
                    RowSettingsFactoryRowIndex: BuildItemFromIndex,
                    RowFactoryFromSettings: BuildRowFromSettings,
                    ItemFactory: BuildListElementFromSetting,
                    ItemComparer: CompareElement,
                    Validator: validator
                )
                {
                    MaxRowCapacity = MAX_ROW_CAPACITY,
                    MinRowCapacity = MIN_ROW_CAPACITY,
                    MaxColumnCapacity = MAX_COLUMN_CAPACITY,
                    MinColumnCapacity = MIN_COLUMN_CAPACITY,
                };
            }

            public static SimpleListValueBuilder<StubRestrictedCapacityList> RowBuilder { get; }
                = new((list, index) => BuildItemFromIndex(index, list.Count));

            public static StubRestrictedCapacityList BuildItemFromIndex(
                int rowIndex,
                int columnLength = INIT_COLUMN_LENGTH
            )
                => new(BuildRowSettingsFromRowIndex(rowIndex, columnLength));

            public static IStubRestrictedCapacityListSettings BuildRowSettingsFromRowIndex(
                int rowIndex,
                int columnLength = INIT_COLUMN_LENGTH
            )
                => new StubRestrictedCapacityListSettings(
                    columnLength.Iterate(columnIndex => new StubModelSettings
                            { StringValue = $"{rowIndex}_{columnIndex}" }
                        )
                        .ToArray()
                );

            public static StubRestrictedCapacityList BuildRowFromSettings(
                int rowIndex,
                IStubRestrictedCapacityListSettings settings
            )
                => new(settings);

            public static IStubModelSettings[] BuildColumnSettingsFromColumnIndex(
                int columnIndex,
                int rowCount = INIT_ROW_LENGTH
            )
                => rowCount.Iterate<IStubModelSettings>(rowIndex => new StubModelSettings
                        { StringValue = $"{rowIndex}_{columnIndex}" }
                    )
                    .ToArray();

            public static StubModel BuildListElementFromSetting(IStubModelSettings settings)
                => new(settings);

            public static bool CompareElement(IStubModelSettings left, IStubModelSettings? right)
                => left.ItemEquals(right);
        }
    }
}
