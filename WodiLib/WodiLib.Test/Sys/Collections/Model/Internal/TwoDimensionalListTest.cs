using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using Test2DList = WodiLib.Sys.Collections.TwoDimensionalList<
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
     */

    [TestFixture]
    public class TwoDimensionalListTest
    {
        private static Logger logger = null!;

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

            constructorTestHelper = new ConstructorTestHelper(logger);
            pureActionTestHelper = new PureActionTestHelper(logger);
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            impureActionTestHelper = new ImpureActionTestHelper(logger);
            impureFunctionTestHelper = new ImpureFunctionTestHelper(logger);
        }

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

        #region GetMaxRowCapacity

        /// <summary>
        ///     GetMaxRowCapacity メソッドが正常に処理され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetMaxRowCapacityTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetMaxRowCapacity(),
                resultValueVerifier: new ValueVerifier<int>(actual =>
                    Assert.AreEqual(TestClass.MAX_ROW_CAPACITY, actual)
                )
            );
        }

        #endregion

        #region GetMinRowCapacity

        /// <summary>
        ///     GetMinRowCapacity メソッドが正常に処理され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetMinRowCapacityTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetMinRowCapacity(),
                resultValueVerifier: new ValueVerifier<int>(actual =>
                    Assert.AreEqual(TestClass.MIN_ROW_CAPACITY, actual)
                )
            );
        }

        #endregion

        #region GetMaxColumnCapacity

        /// <summary>
        ///     GetMaxColumnCapacity メソッドが正常に処理され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetMaxColumnCapacityTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetMaxColumnCapacity(),
                resultValueVerifier: new ValueVerifier<int>(actual =>
                    Assert.AreEqual(TestClass.MAX_COLUMN_CAPACITY, actual)
                )
            );
        }

        #endregion

        #region GetMinColumnCapacity

        /// <summary>
        ///     GetMinColumnCapacity メソッドが正常に処理され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetMinColumnCapacityTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetMinColumnCapacity(),
                resultValueVerifier: new ValueVerifier<int>(actual =>
                    Assert.AreEqual(TestClass.MIN_COLUMN_CAPACITY, actual)
                )
            );
        }

        #endregion

        #region AddRow

        /// <summary>
        ///     AddRow メソッドが正常に処理され、行が追加されること。
        /// </summary>
        [Test]
        public static void AddRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            var settings = TestClass.BuildRowSettingsFromRowIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddRow(settings),
                resultValueVerifier: ValueVerifier<FixedStubRestrictedCapacityList>.AreReferenceEquals(()
                    => testClass.InnerList[initRowCount]
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(initRowCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            new[] { settings },
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + 1, target.RowCount);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        CustomAssert.AreItemEquals(settings, testClass.InnerList[initRowCount]);
                        Assert.AreNotSame(settings, testClass.InnerList[initRowCount]);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region AddRowRange

        /// <summary>
        ///     AddRowRange メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void AddRowRangeTest_Success_SettingsEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var settings = Array.Empty<IStubRestrictedCapacityListSettings>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AddRowRange(settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(initRowCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     AddRowRange メソッドが正常に処理され、複数行が追加されること。
        /// </summary>
        [Test]
        public static void AddRowRangeTest_Success_SettingsNotEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddRowRange(settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(initRowCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + settingsLength, target.RowCount);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        for (var i = 0; i < settingsLength; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], testClass.InnerList[initRowCount + i]);
                            Assert.AreNotSame(settings, testClass.InnerList[initRowCount + i]);
                        }

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region AddColumn

        /// <summary>
        ///     AddColumn メソッドが正常に処理され、列が追加されること。
        /// </summary>
        [Test]
        public static void AddColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddColumn(settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(initColumnCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            new[] { settings },
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + 1, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < 1; c++)
                            {
                                if (c == initColumnCount)
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[r], testClass.InnerList[r][c]);
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region AddColumnRange

        /// <summary>
        ///     AddColumnRange メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void AddColumnRangeTest_Success_SettingsEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var settings = Array.Empty<IEnumerable<IStubModelSettings>>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AddColumnRange(settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(initColumnCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     AddColumnRange メソッドが正常に処理され、列が追加されること。
        /// </summary>
        [Test]
        public static void AddColumnRangeTest_Success_SettingsNotEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddColumnRange(settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(initColumnCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + settingsLength, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(initColumnCount, initColumnCount + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertRow

        /// <summary>
        ///     InsertRow メソッドが正常に処理され、指定位置に行が挿入されること。
        /// </summary>
        [Test]
        public static void InsertRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            var settings = TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertRow(rowIndex, settings),
                resultValueVerifier: ValueVerifier<FixedStubRestrictedCapacityList>.AreReferenceEquals(()
                    => testClass.InnerList[rowIndex]
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            new[] { settings },
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + 1, target.RowCount);

                        var beforeRow = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (r != rowIndex)
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[beforeRow], target.EditableRows[r]);

                                beforeRow++;
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings, testClass.InnerList[r]);
                                Assert.AreNotSame(settings, testClass.InnerList[r]);
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertRowRange

        /// <summary>
        ///     InsertRowRange メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void InsertRowRangeTest_Success_SettingsEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            var settings = Array.Empty<IStubRestrictedCapacityListSettings>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.InsertRowRange(rowIndex, settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     InsertRowRange メソッドが正常に処理され、指定位置に複数行が挿入されること。
        /// </summary>
        [Test]
        public static void InsertRowRangeTest_Success_SettingsNotEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertRowRange(rowIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + settingsLength, target.RowCount);

                        var beforeRow = 0;
                        var insertOffset = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[beforeRow], target.EditableRows[r]);

                                beforeRow++;
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings[insertOffset], testClass.InnerList[r]);
                                Assert.AreNotSame(settings[insertOffset], testClass.InnerList[r]);

                                insertOffset++;
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertColumn

        /// <summary>
        ///     InsertColumn メソッドが正常に処理され、指定位置に列が挿入されること。
        /// </summary>
        [Test]
        public static void InsertColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 2;
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertColumn(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            new[] { settings },
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + 1, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var beforeColumnIndex = 0;
                            for (var c = 0; c < target.ColumnCount; c++)
                            {
                                if (c == columnIndex)
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[r], testClass.InnerList[r][c]);
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][beforeColumnIndex], target[r][c]);

                                    beforeColumnIndex++;
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertColumnRange

        /// <summary>
        ///     InsertColumnRange メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void InsertColumnRangeTest_Success_SettingsEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;
            var settings = Array.Empty<IEnumerable<IStubModelSettings>>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.InsertColumnRange(columnIndex, settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     InsertColumnRange メソッドが正常に処理され、指定位置に複数列が挿入されること。
        /// </summary>
        [Test]
        public static void InsertColumnRangeTest_Success_SettingsNotEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 2;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100))
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertColumnRange(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.InsertColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + settingsLength, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            var beforeColumnIndex = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(initColumnCount, initColumnCount + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][beforeColumnIndex], target[r][c]);

                                    beforeColumnIndex++;
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region OverwriteRow

        /// <summary>
        ///     OverwriteRow メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Success_SettingsEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            var settings = Array.Empty<IStubRestrictedCapacityListSettings>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteRow(rowIndex, settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.OverwriteRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     OverwriteRow メソッドが正常に処理され、指定位置から行が上書きされること。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Success_OnlyReplace()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 0;
            const int settingsLength = TestClass.INIT_ROW_LENGTH - 1;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteRow(rowIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.OverwriteRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        var insertOffset = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[r], target.EditableRows[r]);
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings[insertOffset], testClass.InnerList[rowIndex + r]);
                                Assert.AreNotSame(settings[insertOffset], testClass.InnerList[rowIndex + r]);

                                insertOffset++;
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     OverwriteRow メソッドが正常に処理され、行が追加されること。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Success_OnlyAdd()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = TestClass.INIT_ROW_LENGTH;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteRow(rowIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.OverwriteRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        var insertOffset = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[r], target.EditableRows[r]);
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings[insertOffset], testClass.InnerList[r]);
                                Assert.AreNotSame(settings[insertOffset], testClass.InnerList[r]);

                                insertOffset++;
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     OverwriteRow メソッドが正常に処理され、行が上書き・追加追加されること。
        /// </summary>
        [Test]
        public static void OverwriteRowTest_Success_ReplaceAndAdd()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = TestClass.INIT_ROW_LENGTH - 2;
            const int settingsLength = 3;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteRow(rowIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.OverwriteRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[1]
                        );

                        var insertOffset = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[r], target.EditableRows[r]);
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings[insertOffset], testClass.InnerList[r]);
                                Assert.AreNotSame(settings[insertOffset], testClass.InnerList[r]);

                                insertOffset++;
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region OverwriteColumn

        /// <summary>
        ///     OverwriteColumn メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void OverwriteColumnTest_Success_SettingsEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            var settings = Array.Empty<IEnumerable<IStubModelSettings>>();

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteColumn(columnIndex, settings),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.OverwriteColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     OverwriteColumn メソッドが正常に処理され、指定位置から列が上書きされること。
        /// </summary>
        [Test]
        public static void OverwriteColumnTest_Success_OnlyReplace()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteColumn(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.OverwriteColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(columnIndex, columnIndex + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     OverwriteColumn メソッドが正常に処理され、指定位置から列が追加されること。
        /// </summary>
        [Test]
        public static void OverwriteColumnTest_Success_OnlyAdd()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = TestClass.INIT_COLUMN_LENGTH;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteColumn(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.OverwriteColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(columnIndex, columnIndex + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     OverwriteColumn メソッドが正常に処理され、指定位置から列が上書き・追加されること。
        /// </summary>
        [Test]
        public static void OverwriteColumnTest_Success_ReplaceAndAdd()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = TestClass.INIT_COLUMN_LENGTH - 1;
            const int settingsLength = 3;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteColumn(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.OverwriteColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0]
                                .Args[1]
                        );

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(columnIndex, columnIndex + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveRow

        /// <summary>
        ///     RemoveRow メソッドが正常に処理され、指定行が削除されること。
        /// </summary>
        [Test]
        public static void RemoveRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            var removedRow = instance[rowIndex];

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveRow(rowIndex),
                resultValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList>(actual =>
                    Assert.AreSame(removedRow, actual)
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.RemoveRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 行数が減少していること
                        Assert.AreEqual(initRowCount - 1, target.RowCount);

                        var beforeRow = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (r != rowIndex)
                            {
                                // 編集していない行要素が変更されていないこと
                                CustomAssert.AreItemEquals(
                                    initRows[r],
                                    target.EditableRows[beforeRow],
                                    $"beforeRow={beforeRow}, r={r}"
                                );

                                beforeRow++;
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveRowRange

        /// <summary>
        ///     RemoveRowRange メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void RemoveRowRangeTest_Success_NoRemove()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int count = 0;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.RemoveRowRange(rowIndex, count),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );
            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.RemoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     RemoveRowRange メソッドが正常に処理され、指定範囲の行が削除されること。
        /// </summary>
        [Test]
        public static void RemoveRowRangeTest_Success_Removed()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveRowRange(rowIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        for (var i = 0; i < count; i++)
                        {
                            Assert.AreSame(initRows[rowIndex + i], actualArray[i], $"Offset={i}");
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.RemoveRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 行数が減少していること
                        Assert.AreEqual(initRowCount - count, target.RowCount);

                        var beforeRow = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + count - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                CustomAssert.AreItemEquals(
                                    initRows[r],
                                    target.EditableRows[beforeRow],
                                    $"beforeRow={beforeRow}, r={r}"
                                );

                                beforeRow++;
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveColumn

        /// <summary>
        ///     RemoveColumn メソッドが正常に処理され、指定列が削除されること。
        /// </summary>
        [Test]
        public static void RemoveColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveColumn(columnIndex),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(instance.RowCount, actualArray.Length);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.RemoveColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 列数が減少していること
                        Assert.AreEqual(initColumnCount - 1, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var beforeColumnIndex = 0;
                            for (var c = 0; c < target.ColumnCount; c++)
                            {
                                if (c != columnIndex)
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(
                                        initRows[r][c],
                                        target[r][beforeColumnIndex],
                                        $"r={r}, c={c}, beforeColumnIndex={beforeColumnIndex}"
                                    );

                                    beforeColumnIndex++;
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveColumnRange

        /// <summary>
        ///     RemoveColumnRange メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void RemoveColumnRangeTest_Success_NoRemove()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            const int count = 0;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.RemoveColumnRange(columnIndex, count),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.RemoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        /// <summary>
        ///     RemoveColumnRange メソッドが正常に処理され、指定範囲の列が削除されること。
        /// </summary>
        [Test]
        public static void RemoveColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveColumnRange(columnIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.RemoveColumn),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 列数が減少していること
                        Assert.AreEqual(initColumnCount - count, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var beforeColumnIndex = 0;
                            for (var c = 0; c < target.ColumnCount; c++)
                            {
                                if (!c.IsBetween(columnIndex, columnIndex + count - 1))
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(
                                        initRows[r][c],
                                        target[r][beforeColumnIndex],
                                        $"r={r}, c={c}, beforeColumnIndex={beforeColumnIndex}"
                                    );

                                    beforeColumnIndex++;
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region AdjustRowLength

        /// <summary>
        ///     AdjustRowLength メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void AdjustRowLengthTest_Success_NoSizeChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int length = TestClass.INIT_ROW_LENGTH;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLength(length),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustRowLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        /// <summary>
        ///     AdjustRowLength メソッドが正常に処理され、行数が調整されること。
        /// </summary>
        [Test]
        public static void AdjustRowLengthTest_Success_ChangeSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int addLength = 2;
            const int length = TestClass.INIT_ROW_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLength(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(addLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustRowLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + addLength, target.RowCount);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region AdjustRowLengthIfShort

        /// <summary>
        ///     AdjustRowLengthIfShort メソッドが正常に処理され、行数が不足している場合のみ調整されること。
        /// </summary>
        [Test]
        public static void AdjustRowLengthIfShortTest_Success_ChangeSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int addLength = 2;
            const int length = TestClass.INIT_ROW_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLengthIfShort(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(addLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustRowLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + addLength, target.RowCount);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     AdjustRowLengthIfShort メソッドが正常に処理され、行数が既に十分な場合は何もしないこと。
        /// </summary>
        [Test]
        public static void AdjustRowLengthIfShortTest_Success_NoChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int addLength = -1;
            const int length = TestClass.INIT_ROW_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLengthIfShort(length),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustRowLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        #endregion

        #region AdjustRowLengthIfLong

        /// <summary>
        ///     AdjustRowLengthIfLong メソッドが正常に処理され、行数が多すぎる場合のみ調整されること。
        /// </summary>
        [Test]
        public static void AdjustRowLengthIfLongTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int removeLength = 2;
            const int length = TestClass.INIT_ROW_LENGTH - removeLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLengthIfLong(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(removeLength, actualArray.Length); // 調整が行われること
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustRowLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 行数が減少していること
                        Assert.AreEqual(initRowCount - removeLength, target.RowCount);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < instance.RowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     AdjustRowLengthIfLong メソッドが正常に処理され、行数が既に適切な場合は何もしないこと。
        /// </summary>
        [Test]
        public static void AdjustRowLengthIfLongTest_Success_NoChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int removeLength = -1;
            const int length = TestClass.INIT_ROW_LENGTH - removeLength;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLengthIfLong(length),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );
            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustRowLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        #endregion

        #region AdjustColumnLength

        /// <summary>
        ///     AdjustColumnLength メソッドが正常に処理され、状態が変化しないこと。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthTest_Success_NoSizeChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int length = TestClass.INIT_COLUMN_LENGTH;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLength(length),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustColumnLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        /// <summary>
        ///     AdjustColumnLength メソッドが正常に処理され、列数が調整されること。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthTest_Success_ChangeSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int addLength = 2;
            const int length = TestClass.INIT_COLUMN_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLength(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(addLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustColumnLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 列数が調整されていること
                        Assert.AreEqual(length, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < instance.ColumnCount; c++)
                            {
                                if (!c.IsBetween(initColumnCount, initColumnCount + addLength - 1))
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region AdjustColumnLengthIfShort

        /// <summary>
        ///     AdjustColumnLengthIfShort メソッドが正常に処理され、列数が不足している場合のみ調整されること。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthIfShortTest_Success_ChangeSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int addLength = 2;
            const int length = TestClass.INIT_COLUMN_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLengthIfShort(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(addLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustColumnLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 列数が調整されていること
                        Assert.AreEqual(length, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < instance.ColumnCount; c++)
                            {
                                if (!c.IsBetween(initColumnCount, initColumnCount + addLength - 1))
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     AdjustColumnLengthIfShort メソッドが正常に処理され、列数が既に十分な場合は何もしないこと。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthIfShortTest_Success_NoChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int addLength = -1;
            const int length = TestClass.INIT_COLUMN_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLengthIfShort(length),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustColumnLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        #endregion

        #region AdjustColumnLengthIfLong

        /// <summary>
        ///     AdjustColumnLengthIfLong メソッドが正常に処理され、列数が多すぎる場合のみ調整されること。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthIfLongTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int removeLength = 2;
            const int length = TestClass.INIT_COLUMN_LENGTH - removeLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLengthIfLong(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(removeLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustColumnLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 列数が調整されていること
                        Assert.AreEqual(length, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < instance.ColumnCount; c++)
                            {
                                // 編集していない要素が変更されていないこと
                                CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     AdjustColumnLengthIfLong メソッドが正常に処理され、列数が既に適切な場合は何もしないこと。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthIfLongTest_Success_NoChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int removeLength = -1;
            const int length = TestClass.INIT_COLUMN_LENGTH - removeLength;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLengthIfLong(length),
                resultValueVerifier: ValueVerifier.IsEmpty()
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustColumnLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        #endregion

        #region Reset

        /// <summary>
        ///     <para>Reset メソッドが正常に処理され、リストが指定した内容でリセットされること。</para>
        ///     <para>プロパティ変更が正しく通知されること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_WithSettings_Success_NoChangeSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH
                .Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 * r))
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], actualArray[i], $"RowOffset={i}");
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                    // RowCount, ColumnCount が通知されないこと
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[0]
                        );
                        Assert.AreEqual(true, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 要素が正しくリセットされていること
                        Assert.AreEqual(settings.Length, target.RowCount);
                        Assert.AreEqual(TestClass.INIT_COLUMN_LENGTH, target.ColumnCount);
                        for (var i = 0; i < target.RowCount; i++)
                        {
                            for (var j = 0; j < target.ColumnCount; j++)
                            {
                                CustomAssert.AreItemEquals(
                                    settings[i].Settings[j],
                                    target[i, j],
                                    $"RowOffset={i}, ColumnOffset={j}"
                                );
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>Reset メソッドが正常に処理され、リストが指定した内容でリセットされること。</para>
        ///     <para>プロパティ変更が正しく通知されること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_WithSettings_Success_ChangeRowSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = (TestClass.INIT_ROW_LENGTH + 1)
                .Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 * r))
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], actualArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                    // ColumnCount が通知されないこと
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[0]
                        );
                        Assert.AreEqual(true, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 要素が正しくリセットされていること
                        Assert.AreEqual(settings.Length, target.RowCount);
                        Assert.AreEqual(TestClass.INIT_COLUMN_LENGTH, target.ColumnCount);
                        for (var i = 0; i < target.RowCount; i++)
                        {
                            for (var j = 0; j < target.ColumnCount; j++)
                            {
                                CustomAssert.AreItemEquals(
                                    settings[i].Settings[j],
                                    target[i, j],
                                    $"RowOffset={i}, ColumnOffset={j}"
                                );
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>Reset メソッドが正常に処理され、リストが指定した内容でリセットされること。</para>
        ///     <para>プロパティ変更が正しく通知されること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_WithSettings_Success_ChangeColumnSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH
                .Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 * r, TestClass.INIT_COLUMN_LENGTH - 1))
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], actualArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                    nameof(instance.ColumnCount),
                    // RowCount が通知されないこと
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[0]
                        );
                        Assert.AreEqual(true, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 要素が正しくリセットされていること
                        Assert.AreEqual(settings.Length, target.RowCount);
                        Assert.AreEqual(TestClass.INIT_COLUMN_LENGTH - 1, target.ColumnCount);
                        for (var i = 0; i < target.RowCount; i++)
                        {
                            for (var j = 0; j < target.ColumnCount; j++)
                            {
                                CustomAssert.AreItemEquals(
                                    settings[i].Settings[j],
                                    target[i, j],
                                    $"RowOffset={i}, ColumnOffset={j}"
                                );
                            }
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     <para>Reset メソッドが正常に処理され、リストが指定した内容でリセットされること。</para>
        ///     <para>プロパティ変更が正しく通知されること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_WithSettings_Success_ChangeTableSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = (TestClass.INIT_ROW_LENGTH + 1)
                .Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 * r, TestClass.INIT_COLUMN_LENGTH - 1))
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], actualArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                    nameof(instance.ColumnCount),
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        CustomAssert.AreSequenceEquals(
                            settings,
                            (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator
                                .CalledMemberHistory[0]
                                .Args[0]
                        );
                        Assert.AreEqual(true, testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 要素が正しくリセットされていること
                        Assert.AreEqual(settings.Length, target.RowCount);
                        Assert.AreEqual(settings[0].Settings.Count, target.ColumnCount);
                        for (var i = 0; i < target.RowCount; i++)
                        {
                            for (var j = 0; j < target.ColumnCount; j++)
                            {
                                CustomAssert.AreItemEquals(
                                    settings[i].Settings[j],
                                    target[i, j],
                                    $"RowOffset={i}, ColumnOffset={j}"
                                );
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region Clear

        /// <summary>
        ///     Clear メソッドが正常に処理され、リストがクリアされること。
        /// </summary>
        [Test]
        public static void ClearTest_Success_NoChangeSize()
        {
            var testClass = new TestClass(
                rowCount: TestClass.MIN_ROW_CAPACITY,
                columnCount: TestClass.MIN_COLUMN_CAPACITY
            );
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Clear(),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Clear),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);

                        // 最小容量にリセットされていること
                        Assert.AreEqual(TestClass.MIN_ROW_CAPACITY, target.RowCount);
                        Assert.AreEqual(TestClass.MIN_COLUMN_CAPACITY, target.ColumnCount);
                    }
                )
            );
        }

        /// <summary>
        ///     Clear メソッドが正常に処理され、リストがクリアされること。
        /// </summary>
        [Test]
        public static void ClearTest_Success_ChangeRowSize()
        {
            var testClass = new TestClass(columnCount: TestClass.MIN_COLUMN_CAPACITY);
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Clear(),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Clear),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);

                        // 最小容量にリセットされていること
                        Assert.AreEqual(TestClass.MIN_ROW_CAPACITY, target.RowCount);
                        Assert.AreEqual(TestClass.MIN_COLUMN_CAPACITY, target.ColumnCount);
                    }
                )
            );
        }

        /// <summary>
        ///     Clear メソッドが正常に処理され、リストがクリアされること。
        /// </summary>
        [Test]
        public static void ClearTest_Success_ChangeColumnSize()
        {
            var testClass = new TestClass(
                rowCount: TestClass.MIN_ROW_CAPACITY,
                columnCount: TestClass.MAX_COLUMN_CAPACITY
            );
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Clear(),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                    nameof(instance.ColumnCount),
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Clear),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);

                        // 最小容量にリセットされていること
                        Assert.AreEqual(TestClass.MIN_ROW_CAPACITY, target.RowCount);
                        Assert.AreEqual(TestClass.MIN_COLUMN_CAPACITY, target.ColumnCount);
                    }
                )
            );
        }

        /// <summary>
        ///     Clear メソッドが正常に処理され、リストがクリアされること。
        /// </summary>
        [Test]
        public static void ClearTest_Success_ChangeTableSize()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Clear(),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                    nameof(instance.ColumnCount),
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Clear),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);

                        // 最小容量にリセットされていること
                        Assert.AreEqual(TestClass.MIN_ROW_CAPACITY, target.RowCount);
                        Assert.AreEqual(TestClass.MIN_COLUMN_CAPACITY, target.ColumnCount);
                    }
                )
            );
        }

        #endregion

        #region ValidateAddRow

        /// <summary>
        ///     ValidateAddRow メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateAddRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var settings = TestClass.BuildRowSettingsFromRowIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateAddRow(settings)
            );

            // Validator が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(initRowCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(
                settings,
                ((IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator.CalledMemberHistory[0]
                    .Args[1]).First()
            );
        }

        #endregion

        #region ValidateAddRowRange

        /// <summary>
        ///     ValidateAddRowRange メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateAddRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateAddRowRange(settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(initRowCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateAddColumn

        /// <summary>
        ///     ValidateAddColumn メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateAddColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateAddColumn(settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(instance.ColumnCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(
                settings,
                ((IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0].Args[1])
                .First()
            );
        }

        #endregion

        #region ValidateAddColumnRange

        /// <summary>
        ///     ValidateAddColumnRange メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateAddColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateAddColumnRange(settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(initColumnCount, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateInsertRow

        /// <summary>
        ///     ValidateInsertRow メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateInsertRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            var settings = TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateInsertRow(rowIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(
                settings,
                ((IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator.CalledMemberHistory[0]
                    .Args[1]).First()
            );
        }

        #endregion

        #region ValidateInsertRowRange

        /// <summary>
        ///     ValidateInsertRowRange メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateInsertRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateInsertRowRange(rowIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateInsertColumn

        /// <summary>
        ///     ValidateInsertColumn メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateInsertColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateInsertColumn(columnIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(
                settings,
                ((IEnumerable<IEnumerable<IStubModelSettings>>)testClass.MockValidator.CalledMemberHistory[0].Args[1])
                .First()
            );
        }

        #endregion

        #region ValidateInsertColumnRange

        /// <summary>
        ///     ValidateInsertColumnRange メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateInsertColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 2;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateInsertColumnRange(columnIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.InsertColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(settingsLength, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateOverwriteRow

        /// <summary>
        ///     ValidateOverwriteRow メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateOverwriteRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateOverwriteRow(rowIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.OverwriteRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateOverwriteColumn

        /// <summary>
        ///     ValidateOverwriteColumn メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateOverwriteColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateOverwriteColumn(columnIndex, settings)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.OverwriteColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(settingsLength, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(settings, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateRemoveRow

        /// <summary>
        ///     ValidateRemoveRow メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateRemoveRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateRemoveRow(rowIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.RemoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateRemoveRowRange

        /// <summary>
        ///     ValidateRemoveRowRange メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateRemoveRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateRemoveRowRange(rowIndex, count)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.RemoveRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateRemoveColumn

        /// <summary>
        ///     ValidateRemoveColumn メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateRemoveColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateRemoveColumn(columnIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.RemoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateRemoveColumnRange

        /// <summary>
        ///     ValidateRemoveColumnRange メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateRemoveColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateRemoveColumnRange(columnIndex, count)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.RemoveColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(count, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateAdjustRowLength

        /// <summary>
        ///     ValidateAdjustRowLength メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateAdjustRowLengthTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int addLength = 2;
            const int length = TestClass.INIT_ROW_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateAdjustRowLength(length)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustRowLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        #endregion

        #region ValidateAdjustColumnLength

        /// <summary>
        ///     ValidateAdjustColumnLength メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateAdjustColumnLengthTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int addLength = 2;
            const int length = TestClass.INIT_COLUMN_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateAdjustColumnLength(length)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.AdjustColumnLength),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(length, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
        }

        #endregion

        #region ValidateReset

        /// <summary>
        ///     ValidateReset メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateResetTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH
                .Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 * r))
                .ToArray();

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
            CustomAssert.AreSequenceEquals(
                settings,
                (IEnumerable<IStubRestrictedCapacityListSettings>)testClass.MockValidator.CalledMemberHistory[0].Args[0]
            );
            Assert.AreEqual(true, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateClear

        /// <summary>
        ///     ValidateClear メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateClearTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateClear()
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.Clear),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
        }

        #endregion

        #region AddRowInternal

        /// <summary>
        ///     AddRowInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void AddRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            var settings = TestClass.BuildRowSettingsFromRowIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddRowInternal(settings),
                resultValueVerifier: ValueVerifier<FixedStubRestrictedCapacityList>.AreReferenceEquals(()
                    => testClass.InnerList[initRowCount]
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + 1, target.RowCount);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        CustomAssert.AreItemEquals(settings, testClass.InnerList[initRowCount]);
                        Assert.AreNotSame(settings, testClass.InnerList[initRowCount]);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region AddRowRangeInternal

        /// <summary>
        ///     AddRowRangeInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void AddRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddRowRangeInternal(settings),
                resultValueVerifier: ValueVerifier
                    .AreItemSequenceEquals(
                        settings
                    ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + settingsLength, target.RowCount);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        for (var i = 0; i < settingsLength; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], testClass.InnerList[initRowCount + i]);
                            Assert.AreNotSame(settings, testClass.InnerList[initRowCount + i]);
                        }

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region AddColumnInternal

        /// <summary>
        ///     AddColumnInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void AddColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddColumnInternal(settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + 1, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < 1; c++)
                            {
                                if (c == initColumnCount)
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[r], testClass.InnerList[r][c]);
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region AddColumnRangeInternal

        /// <summary>
        ///     AddColumnRangeInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void AddColumnRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddColumnRangeInternal(settings),
                resultValueVerifier: ValueVerifier
                    .AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + settingsLength, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(initColumnCount, initColumnCount + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertRowInternal

        /// <summary>
        ///     InsertRowInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void InsertRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertRowRangeInternal(rowIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + settingsLength, target.RowCount);

                        var beforeRow = 0;
                        var insertOffset = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[beforeRow], target.EditableRows[r]);

                                beforeRow++;
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings[insertOffset], testClass.InnerList[r]);
                                Assert.AreNotSame(settings[insertOffset], testClass.InnerList[r]);

                                insertOffset++;
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertRowRange

        /// <summary>
        ///     InsertRowRangeInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void InsertRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 2;
            var settings = TestClass.BuildColumnSettingsFromColumnIndex(100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertColumnInternal(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + 1, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var beforeColumnIndex = 0;
                            for (var c = 0; c < target.ColumnCount; c++)
                            {
                                if (c == columnIndex)
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[r], testClass.InnerList[r][c]);
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][beforeColumnIndex], target[r][c]);

                                    beforeColumnIndex++;
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region InsertColumnInternal

        /// <summary>
        ///     InsertColumnInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void InsertColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            const int columnIndex = 2;
            var settings = instance.RowCount.Iterate(i => new StubModelSettings { StringValue = $"InsertedColumn_{i}" })
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertColumnInternal(columnIndex, settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + 1, target.ColumnCount);
                    }
                )
            );
        }

        #endregion

        #region InsertColumnRangeInternal

        /// <summary>
        ///     InsertColumnRangeInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void InsertColumnRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 2;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertColumnRangeInternal(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が増加していること
                        Assert.AreEqual(initColumnCount + settingsLength, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            var beforeColumnIndex = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(initColumnCount, initColumnCount + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][beforeColumnIndex], target[r][c]);

                                    beforeColumnIndex++;
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region OverwriteRowInternal

        /// <summary>
        ///     OverwriteRowInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void OverwriteRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(i
                    => TestClass.BuildRowSettingsFromRowIndex(100 + i)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteRowInternal(rowIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals(settings),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        var insertOffset = 0;
                        for (var r = 0; r < target.RowCount; r++)
                        {
                            if (!r.IsBetween(rowIndex, rowIndex + settingsLength - 1))
                            {
                                // 編集していない行要素が変更されていないこと
                                Assert.AreSame(initRows[r], target.EditableRows[r]);
                            }
                            else
                            {
                                // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                CustomAssert.AreItemEquals(settings[insertOffset], testClass.InnerList[r]);
                                Assert.AreNotSame(settings[insertOffset], testClass.InnerList[r]);

                                insertOffset++;
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region OverwriteColumnInternal

        /// <summary>
        ///     OverwriteColumnInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void OverwriteColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 1;
            const int settingsLength = 2;
            var settings = settingsLength.Iterate(c => TestClass.BuildColumnSettingsFromColumnIndex(c + 100)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.OverwriteColumnInternal(columnIndex, settings),
                resultValueVerifier: ValueVerifier.AreItemSequenceEquals<IStubModelSettings>(settings),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            var i = 0;
                            for (var c = 0; c < settingsLength; c++)
                            {
                                if (c.IsBetween(columnIndex, columnIndex + settingsLength - 1))
                                {
                                    // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                                    CustomAssert.AreItemEquals(settings[i][r], testClass.InnerList[r][c]);
                                    Assert.AreNotSame(settings[i][r], testClass.InnerList[r][c]);

                                    i++;
                                }
                                else
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveRowInternal

        /// <summary>
        ///     RemoveRowInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void RemoveRowInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            var removedRow = instance[rowIndex];

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveRowInternal(rowIndex),
                resultValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList>(actual =>
                    Assert.AreSame(removedRow, actual)
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行数が減少していること
                        Assert.AreEqual(initRowCount - 1, target.RowCount);

                        for (var r = 0; r < target.RowCount; r++)
                        {
                            var beforeRow = r < rowIndex
                                ? r
                                : r + 1;
                            // 編集していない行要素が変更されていないこと
                            Assert.AreSame(
                                initRows[beforeRow],
                                target.EditableRows[r],
                                $"r = {r}, beforeRow={beforeRow}"
                            );
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveRowRangeInternal

        /// <summary>
        ///     RemoveRowRangeInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void RemoveRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int rowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveRowRangeInternal(rowIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        for (var i = 0; i < count; i++)
                        {
                            Assert.AreSame(initRows[rowIndex + i], actualArray[i], $"Offset={i}");
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行数が減少していること
                        Assert.AreEqual(initRowCount - count, target.RowCount);

                        for (var r = 0; r < target.RowCount; r++)
                        {
                            var beforeRow = r < rowIndex
                                ? r
                                : r + count;
                            // 編集していない行要素が変更されていないこと
                            Assert.AreSame(initRows[beforeRow], target.EditableRows[r]);
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveColumnInternal

        /// <summary>
        ///     RemoveColumnInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void RemoveColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveColumnInternal(columnIndex),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(instance.RowCount, actualArray.Length);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が減少していること
                        Assert.AreEqual(initColumnCount - 1, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < target.ColumnCount; c++)
                            {
                                var beforeColumnIndex = c < columnIndex
                                    ? c
                                    : c + 1;
                                // 編集していない要素が変更されていないこと
                                CustomAssert.AreItemEquals(
                                    initRows[r][beforeColumnIndex],
                                    target[r][c],
                                    $"r={r}, c={c}, beforeColumnIndex={beforeColumnIndex}"
                                );
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region RemoveColumnRangeInternal

        /// <summary>
        ///     RemoveColumnRangeInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void RemoveColumnRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int columnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveColumnRangeInternal(columnIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が減少していること
                        Assert.AreEqual(initColumnCount - count, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < target.ColumnCount; c++)
                            {
                                var beforeColumnIndex = c < columnIndex
                                    ? c
                                    : c + count;
                                // 編集していない要素が変更されていないこと
                                CustomAssert.AreItemEquals(
                                    initRows[r][beforeColumnIndex],
                                    target[r][c],
                                    $"r={r}, c={c}, beforeColumnIndex={beforeColumnIndex}"
                                );
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region AdjustRowLengthInternal

        /// <summary>
        ///     AdjustRowLengthInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void AdjustRowLengthInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initRowCount = instance.RowCount;
            var initRows = instance.EditableRows.ToArray();
            const int addLength = 2;
            const int length = TestClass.INIT_ROW_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustRowLengthInternal(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(addLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.RowCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 行数が増加していること
                        Assert.AreEqual(initRowCount + addLength, target.RowCount);

                        // 編集していない行要素が変更されていないこと
                        for (var i = 0; i < initRowCount; i++)
                        {
                            Assert.AreSame(initRows[i], target.EditableRows[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region AdjustColumnLengthInternal

        /// <summary>
        ///     AdjustColumnLengthInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void AdjustColumnLengthInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initColumnCount = instance.ColumnCount;
            var initRows = instance.EditableRows.Select(r => r.DeepClone()).ToArray();
            const int addLength = 2;
            const int length = TestClass.INIT_COLUMN_LENGTH + addLength;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustColumnLengthInternal(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(addLength, actualArray.Length); // 何らかの変更があったこと
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.ColumnCount),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 列数が調整されていること
                        Assert.AreEqual(length, target.ColumnCount);

                        for (var r = 0; r < instance.RowCount; r++)
                        {
                            for (var c = 0; c < instance.ColumnCount; c++)
                            {
                                if (!c.IsBetween(initColumnCount, initColumnCount + addLength - 1))
                                {
                                    // 編集していない要素が変更されていないこと
                                    CustomAssert.AreItemEquals(initRows[r][c], target[r][c]);
                                }
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region ClearInternal

        /// <summary>
        ///     ClearInternal メソッドが正常に処理され、Validator が呼ばれないこと。
        /// </summary>
        [Test]
        public static void ClearInternalTest_Success()
        {
            var testClass = new TestClass(
                rowCount: TestClass.MIN_ROW_CAPACITY,
                columnCount: TestClass.MIN_COLUMN_CAPACITY
            );
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.ClearInternal(),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // Validator が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 最小容量にリセットされていること
                        Assert.AreEqual(TestClass.MIN_ROW_CAPACITY, target.RowCount);
                        Assert.AreEqual(TestClass.MIN_COLUMN_CAPACITY, target.ColumnCount);
                    }
                )
            );
        }

        #endregion

        #endregion

        private class TestClass
        {
            public const int MAX_ROW_CAPACITY = 10;
            public const int MIN_ROW_CAPACITY = 1;
            public const int MAX_COLUMN_CAPACITY = 7;
            public const int MIN_COLUMN_CAPACITY = 2;

            public const int INIT_ROW_LENGTH = 4;
            public const int INIT_COLUMN_LENGTH = 5;

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
