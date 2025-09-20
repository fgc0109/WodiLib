using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using Test2DList = WodiLib.Sys.Collections.ReadOnly2DList<
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
    public class ReadOnly2DListTest
    {
        private static Logger logger = null!;

        private static PropertyTestHelper propertyTestHelper = null!;
        private static ConstructorTestHelper constructorTestHelper = null!;
        private static PureActionTestHelper pureActionTestHelper = null!;
        private static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private static ImpureActionTestHelper impureActionTestHelper = null!;
        private static ItemEqualsTestHelper itemEqualsTestHelper = null!;

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
            itemEqualsTestHelper = new ItemEqualsTestHelper(logger);
        }

        #region EventHandlers

        /*
         * プロパティ変更通知・コレクション変更通知のテストは SimpleList で行う。
         * 2DList は内部的には SimpleList を呼び出す前提であるため。
         */

        /// <summary>
        ///     <para>
        ///         内部プロパティ Items の PropertyChanged イベントが
        ///         ExtendedList{TIn,TOut} 自身の PropertyChanged イベントとして
        ///         伝播すること。
        ///     </para>
        ///     <para>
        ///         内部プロパティ Items の CollectionChanged イベントが
        ///         ExtendedList{TIn,TOut} 自身の CollectionChanged イベントとして
        ///         伝播すること。
        ///     </para>
        ///     <para>イベントハンドラを解除した後通知されないこと。</para>
        /// </summary>
        [Test]
        public static void ChangedEventHandlerTest_ChangeItem()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();

            var propertyChangedEventArgsList = new List<PropertyChangedEventArgs>();
            var collectionChangedEventArgsList = new List<NotifyCollectionChangedEventArgs>();

            PropertyChangedEventHandler propertyChangedEventHandler = (sender, args) =>
            {
                // sender が instance であること
                Assert.AreSame(instance, sender);
                propertyChangedEventArgsList.Add(args);
            };
            NotifyCollectionChangedEventHandler collectionChangedEventHandler = (sender, args) =>
            {
                // sender が instance であること
                Assert.AreSame(instance, sender);
                collectionChangedEventArgsList.Add(args);
            };

            instance.PropertyChanged += propertyChangedEventHandler;
            instance.CollectionChanged += collectionChangedEventHandler;
            var setItem = new StubRestrictedCapacityList(TestClass.INIT_COLUMN_LENGTH);
            const int setIndex = 1;

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: _ => testClass.InnerList[setIndex] = setItem,
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // 編集した要素が設定されていること
                        Assert.AreSame(setItem, target[setIndex]);
                    }
                )
            );

            // プロパティ変更通知が起きていること
            Assert.AreEqual(1, propertyChangedEventArgsList.Count);
            Assert.AreEqual(ListConstant.IndexerName, propertyChangedEventArgsList[0].PropertyName);
            // コレクション変更通知が起きていること
            Assert.AreEqual(1, collectionChangedEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, collectionChangedEventArgsList[0].Action);
            Assert.AreEqual(setIndex, collectionChangedEventArgsList[0].OldStartingIndex);
            Assert.AreEqual(1, collectionChangedEventArgsList[0].OldItems!.Count);
            Assert.IsInstanceOf<ReadOnlyStubRestrictedCapacityList>(collectionChangedEventArgsList[0].OldItems![0]);
            CustomAssert.AreItemEquals(
                initItems[setIndex],
                (ReadOnlyStubRestrictedCapacityList)collectionChangedEventArgsList[0].OldItems![0]!
            );
            Assert.AreEqual(setIndex, collectionChangedEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, collectionChangedEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<ReadOnlyStubRestrictedCapacityList>(collectionChangedEventArgsList[0].NewItems![0]);
            Assert.AreSame(
                setItem,
                (ReadOnlyStubRestrictedCapacityList)collectionChangedEventArgsList[0].NewItems![0]!
            );

            // ----------------------------------------
            //      イベントハンドラ解除後、通知されないことの確認

            propertyChangedEventArgsList.Clear();
            collectionChangedEventArgsList.Clear();
            // 前提条件：propertyChangedEventArgsList, collectionChangedEventArgsList がクリアされること
            Assert.AreEqual(0, propertyChangedEventArgsList.Count);
            Assert.AreEqual(0, collectionChangedEventArgsList.Count);

            instance.PropertyChanged -= propertyChangedEventHandler;
            instance.CollectionChanged -= collectionChangedEventHandler;

            var setItem2 = new StubRestrictedCapacityList(TestClass.INIT_COLUMN_LENGTH);
            setItem2.Tags.Add("Expand Tag.");

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: _ => testClass.InnerList[setIndex] = setItem2,
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<Test2DList>(target =>
                    {
                        // 編集した要素が設定されていること
                        Assert.AreSame(setItem2, target[setIndex]);
                    }
                )
            );

            // プロパティ変更通知が起きていないこと
            Assert.AreEqual(0, propertyChangedEventArgsList.Count);
            // コレクション変更通知が起きていないこと
            Assert.AreEqual(0, collectionChangedEventArgsList.Count);
        }

        /// <summary>
        ///     行数が変化した場合、
        ///     RowCount のプロパティ変更が通知されること。
        /// </summary>
        [Test]
        public static void ChangedEventHandlerTest_ChangeRowCount()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            var propertyChangedEventArgsList = new List<PropertyChangedEventArgs>();
            PropertyChangedEventHandler propertyChangedEventHandler = (sender, args) =>
            {
                // sender が instance であること
                Assert.AreSame(instance, sender);
                propertyChangedEventArgsList.Add(args);
            };
            instance.PropertyChanged += propertyChangedEventHandler;
            var addItem = new StubRestrictedCapacityList(TestClass.INIT_COLUMN_LENGTH);

            testClass.InnerList.Add(addItem);

            // プロパティ変更通知が起きていること
            Assert.AreEqual(2, propertyChangedEventArgsList.Count);
            Assert.AreEqual(nameof(instance.RowCount), propertyChangedEventArgsList[0].PropertyName);
            Assert.AreEqual(ListConstant.IndexerName, propertyChangedEventArgsList[1].PropertyName);
        }

        /// <summary>
        ///     列数が変化した場合、
        ///     ColumnCount のプロパティ変更が通知されること。
        /// </summary>
        [Test]
        public static void ChangedEventHandlerTest_ChangeColumnCount()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            var propertyChangedEventArgsList = new List<PropertyChangedEventArgs>();
            PropertyChangedEventHandler propertyChangedEventHandler = (sender, args) =>
            {
                // sender が instance であること
                Assert.AreSame(instance, sender);
                propertyChangedEventArgsList.Add(args);
            };
            instance.PropertyChanged += propertyChangedEventHandler;
            var addItem = new StubModelSettings { StringValue = "Add Column Item." };

            testClass.InnerList.ForEach(row => { row.Add(addItem); });

            // プロパティ変更通知が起きていること
            Assert.AreEqual(1, propertyChangedEventArgsList.Count);
            Assert.AreEqual(nameof(instance.ColumnCount), propertyChangedEventArgsList[0].PropertyName);
        }

        /// <summary>
        ///     プロパティ変更通知が
        ///     意図したとおり行われること。
        /// </summary>
        /// <remarks>
        ///     ホワイトボックステスト。
        ///     ColumnCount のプロパティ変更通知を内部配列0行目の PropertyChanged イベントを介して行っているため
        ///     内部配列0行目が別のインスタンスに差し替えられても問題ないことをテストする。
        /// </remarks>
        public static void ChangedEventHandlerTest_SpecialCase()
        {
            /*
             * 二次元リストを外部から操作する必要があるため、
             * このテストでは ReadOnly2DList ではなく TwoDimensionalList を使用する。
             *
             * 内部の配列を直接操作された場合は考慮しない。
             */
            var validator =
                new MockWodiLib2DListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>();
            var innerList = new SimpleList<StubRestrictedCapacityList>(
                valueBuilder: TestClass.RowBuilder,
                initValues: TestClass.INIT_ROW_LENGTH.Iterate(rowIndex
                    => TestClass.BuildItemFromIndex(rowIndex)
                )
            );
            var twoDList = new TwoDimensionalList<
                StubRestrictedCapacityList,
                FixedStubRestrictedCapacityList,
                ReadOnlyStubRestrictedCapacityList,
                IStubRestrictedCapacityListSettings,
                StubModel,
                ReadOnlyStubModel,
                IStubModelSettings
            >(innerList, TestClass.CreateConfig(validator));

            var propertyChangedEventArgsList = new List<PropertyChangedEventArgs>();
            PropertyChangedEventHandler propertyChangedEventHandler = (sender, args) =>
            {
                // sender が instance であること
                Assert.AreSame(twoDList, sender);
                propertyChangedEventArgsList.Add(args);
            };
            twoDList.PropertyChanged += propertyChangedEventHandler;

            // 0行目を削除したとき、意図したプロパティ変更通知が起きること
            twoDList.RemoveRow(0);
            Assert.AreEqual(2, propertyChangedEventArgsList.Count);
            Assert.AreEqual(nameof(twoDList.RowCount), propertyChangedEventArgsList[0].PropertyName);
            Assert.AreEqual(ListConstant.IndexerName, propertyChangedEventArgsList[1].PropertyName);

            propertyChangedEventArgsList.Clear();

            // 列数を変化させたとき、意図したプロパティ変更通知が起きること
            twoDList.RemoveColumn(0);
            Assert.AreEqual(2, propertyChangedEventArgsList.Count);
            Assert.AreEqual(nameof(twoDList.ColumnCount), propertyChangedEventArgsList[0].PropertyName);
            Assert.AreEqual(ListConstant.IndexerName, propertyChangedEventArgsList[1].PropertyName);

            propertyChangedEventArgsList.Clear();

            // 0行目を挿入したとき、意図したプロパティ変更通知が起きること
            twoDList.InsertRow(0, new StubRestrictedCapacityList(TestClass.INIT_COLUMN_LENGTH - 1));
            Assert.AreEqual(2, propertyChangedEventArgsList.Count);
            Assert.AreEqual(nameof(twoDList.RowCount), propertyChangedEventArgsList[0].PropertyName);
            Assert.AreEqual(ListConstant.IndexerName, propertyChangedEventArgsList[1].PropertyName);

            propertyChangedEventArgsList.Clear();

            // 列数を変化させたとき、意図したプロパティ変更通知が起きること
            twoDList.RemoveColumn(0);
            Assert.AreEqual(2, propertyChangedEventArgsList.Count);
            Assert.AreEqual(nameof(twoDList.ColumnCount), propertyChangedEventArgsList[0].PropertyName);
            Assert.AreEqual(ListConstant.IndexerName, propertyChangedEventArgsList[1].PropertyName);
        }

        #endregion

        #region Properties

        #region public

        #region RowIndexer

        /// <summary>
        ///     <para>インデクサの取得に成功すること。</para>
        ///     <para>取得結果が意図した値であること。</para>
        ///     <para>Validatorのメソッドが意図したとおり呼ばれること。</para>
        /// </summary>
        [Test]
        public static void RowIndexerGetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target[rowIndex],
                getValueVerifier: new ValueVerifier<ReadOnlyStubRestrictedCapacityList>(result =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.GetRow),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 意図した値が取得されること
                        Assert.AreSame(testClass.InnerList[rowIndex], result);
                    }
                )
            );
        }

        #endregion

        #region CellIndexer

        /// <summary>
        ///     <para>インデクサの取得に成功すること。</para>
        ///     <para>取得結果が意図した値であること。</para>
        ///     <para>Validatorのメソッドが意図したとおり呼ばれること。</para>
        /// </summary>
        [Test]
        public static void CellIndexerGetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target[rowIndex, columnIndex],
                getValueVerifier: new ValueVerifier<ReadOnlyStubModel>(result =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.GetCell),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(columnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 意図した値が取得されること
                        Assert.AreSame(testClass.InnerList[rowIndex][columnIndex], result);
                    }
                )
            );
        }

        #endregion

        #region RowCount

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void RowCountGetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int expected = TestClass.INIT_ROW_LENGTH;

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.RowCount,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region ColumnCount

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void ColumnCountGetterTest_Success_WhenEmpty()
        {
            var testClass = new TestClass(rowCount: 0, columnCount: 0);
            var instance = testClass.TestInstance;
            const int expected = 0;

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.ColumnCount,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void ColumnCountGetterTest_Success_WhenNotEmpty()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int expected = TestClass.INIT_COLUMN_LENGTH;

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.ColumnCount,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        #region SimpleListAndConfig

        /// <summary>
        ///     <para>コンストラクタが正常に終了すること。</para>
        ///     <para>Items プロパティの実態がコンストラクタで与えた IExtendedList であること。</para>
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

                        var insertIndex = itemsImpl.Count;
                        var addItem = new StubRestrictedCapacityList(TestClass.INIT_COLUMN_LENGTH);

                        itemsImpl.Insert(insertIndex, addItem);

                        //   プロパティ変更通知が行われること
                        Assert.AreEqual(2, notifiedPropertyChanged.Count);
                        Assert.AreEqual(nameof(instance.RowCount), notifiedPropertyChanged[0]);
                        Assert.AreEqual(ListConstant.IndexerName, notifiedPropertyChanged[1]);

                        //   instance の要素が置換されていること
                        Assert.AreSame(addItem, instance[insertIndex]);
                    }
                )
            );
        }

        /// <summary>
        ///     引数が null のとき、
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

        #endregion

        #region Methods

        #region public

        #region GetEnumerator

        /// <summary>
        ///     メソッド GetEnumerator が正常に処理され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetEnumeratorTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var innerList = testClass.InnerList;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetEnumerator(),
                resultValueVerifier: new ValueVerifier<IEnumerator<ReadOnlyStubRestrictedCapacityList>>(actual =>
                    {
                        // 取得した IEnumerator から値を取り出す
                        var actualValues = new List<ReadOnlyStubRestrictedCapacityList>();
                        while (actual.MoveNext())
                        {
                            actualValues.Add(actual.Current);
                        }

                        // 取得した値が意図した値であること
                        CustomAssert.AreSequenceEquals(innerList, actualValues);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetRow

        /// <summary>
        ///     <para>指定した要素が取得されること。</para>
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
                resultValueVerifier: ValueVerifier<ReadOnlyStubRestrictedCapacityList>.AreReferenceEquals(
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
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int rowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRowRange(rowIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<ReadOnlyStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(initItems[1], actualArray[0]);
                        Assert.AreSame(initItems[2], actualArray[1]);
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
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumn(columnIndex),
                resultValueVerifier: new ValueVerifier<IEnumerable<ReadOnlyStubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(TestClass.INIT_ROW_LENGTH, actualArray.Length);
                        for (var i = 0; i < TestClass.INIT_ROW_LENGTH; i++)
                        {
                            Assert.AreSame(testClass.InnerList[i][columnIndex], actualArray[i]);
                        }
                    }
                )
            );

            // Validatorのメソッドが意図したとおり呼ばれること
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
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumnRange(columnIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<ReadOnlyStubModel>>>(actual =>
                    {
                        var actualArray = actual.To2DArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[0][columnIndex], actualArray[0][0]);
                        Assert.AreSame(testClass.InnerList[1][columnIndex], actualArray[0][1]);
                        Assert.AreSame(testClass.InnerList[2][columnIndex], actualArray[0][2]);
                        Assert.AreSame(testClass.InnerList[3][columnIndex], actualArray[0][3]);
                        Assert.AreSame(testClass.InnerList[0][columnIndex + 1], actualArray[1][0]);
                        Assert.AreSame(testClass.InnerList[1][columnIndex + 1], actualArray[1][1]);
                        Assert.AreSame(testClass.InnerList[2][columnIndex + 1], actualArray[1][2]);
                        Assert.AreSame(testClass.InnerList[3][columnIndex + 1], actualArray[1][3]);
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
        ///     <para>指定した要素が取得されること。</para>
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
                resultValueVerifier: ValueVerifier<ReadOnlyStubModel>.AreReferenceEquals(
                    testClass.InnerList[rowIndex][columnIndex]
                )
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetCell),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(columnIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateGetRow

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetRowTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGetRow(rowIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetRow),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(rowIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(1, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region ValidateGetRowRange

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetRowRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGetRowRange(rowIndex, count)
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

        #region ValidateGetColumn

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetColumnTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGetColumn(columnIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.GetColumn),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(columnIndex, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(1, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region ValidateGetColumnRange

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetColumnRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGetColumnRange(columnIndex, count)
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

        #region ValidateGetCell

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetCellTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int columnIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGetCell(rowIndex, columnIndex)
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

        #region GetRowInternal

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
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
                resultValueVerifier: ValueVerifier<ReadOnlyStubRestrictedCapacityList>.AreReferenceEquals(
                    testClass.InnerList[rowIndex]
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetRowRangeInternal

        /// <summary>
        ///     <para>指定した範囲の要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetRowRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int rowIndex = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRowRangeInternal(rowIndex, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<ReadOnlyStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[1], actualArray[0]);
                        Assert.AreSame(testClass.InnerList[2], actualArray[1]);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetColumnInternal

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetColumnInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int columnIndex = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetColumnInternal(columnIndex),
                resultValueVerifier: new ValueVerifier<IEnumerable<ReadOnlyStubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(TestClass.INIT_ROW_LENGTH, actualArray.Length);
                        for (var i = 0; i < TestClass.INIT_ROW_LENGTH; i++)
                        {
                            Assert.AreSame(testClass.InnerList[i][columnIndex], actualArray[i]);
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
        ///     <para>指定した範囲の要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
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
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<ReadOnlyStubModel>>>(actual =>
                    {
                        var actualArray = actual.To2DArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[0][columnIndex], actualArray[0][0]);
                        Assert.AreSame(testClass.InnerList[1][columnIndex], actualArray[0][1]);
                        Assert.AreSame(testClass.InnerList[2][columnIndex], actualArray[0][2]);
                        Assert.AreSame(testClass.InnerList[3][columnIndex], actualArray[0][3]);
                        Assert.AreSame(testClass.InnerList[0][columnIndex + 1], actualArray[1][0]);
                        Assert.AreSame(testClass.InnerList[1][columnIndex + 1], actualArray[1][1]);
                        Assert.AreSame(testClass.InnerList[2][columnIndex + 1], actualArray[1][2]);
                        Assert.AreSame(testClass.InnerList[3][columnIndex + 1], actualArray[1][3]);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetCellInternal

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
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
                resultValueVerifier: new ValueVerifier<ReadOnlyStubModel>(result =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        Assert.AreSame(testClass.InnerList[rowIndex][columnIndex], result);
                    }
                )
            );
        }

        #endregion

        #region ItemEquals

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Success_Same()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            itemEqualsTestHelper.ItemEquals(
                left: instance,
                right: instance,
                expected: true
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Success_Equal()
        {
            var testClass = new TestClass();
            var left = testClass.TestInstance;
            var right = new TestClass().TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Success_DiffItem()
        {
            var testClass = new TestClass();
            var left = testClass.TestInstance;
            var right = new TestClass().TestInstance;
            testClass.InnerList[0][0] = new StubModel("Diff Item");

            testClass.MockValidator.ClearCalledHistory();

            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Success_Null()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            itemEqualsTestHelper.ItemEquals(
                left: instance,
                right: null,
                expected: false
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #endregion

        #endregion

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

            public static Test2DList.Config CreateConfig(
                MockWodiLib2DListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>? validator
            )
            {
                return new Test2DList.Config(
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

            public static StubRestrictedCapacityListSettings BuildRowSettingsFromRowIndex(
                int rowIndex,
                int columnLength = INIT_COLUMN_LENGTH
            )
                => new(
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

            public static StubModel BuildListElementFromSetting(IStubModelSettings settings)
                => new(settings);

            public static bool CompareElement(IStubModelSettings left, IStubModelSettings? right)
                => left.ItemEquals(right);
        }
    }
}
