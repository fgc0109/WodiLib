using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using Test2DList = WodiLib.Sys.Collections.TwoDimensionalList<
    WodiLib.Test.Tools.IStubRestrictedCapacity2DListSettings,
    WodiLib.Test.Tools.StubRestrictedCapacityList,
    WodiLib.Test.Tools.FixedStubRestrictedCapacityList,
    WodiLib.Test.Tools.IStubRestrictedCapacityListSettings,
    WodiLib.Test.Tools.StubModel,
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
    public class TwoDimensionalListTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
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
                        CustomAssert.AreItemEquals(setItem, target[setIndex]);
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
            Assert.IsInstanceOf<StubRestrictedCapacityList>(collectionChangedEventArgsList[0].OldItems![0]);
            CustomAssert.AreItemEquals(
                initItems[setIndex],
                (StubRestrictedCapacityList)collectionChangedEventArgsList[0].OldItems![0]!
            );
            Assert.AreEqual(setIndex, collectionChangedEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, collectionChangedEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubRestrictedCapacityList>(collectionChangedEventArgsList[0].NewItems![0]);
            Assert.AreSame(
                setItem,
                (StubRestrictedCapacityList)collectionChangedEventArgsList[0].NewItems![0]!
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
                        CustomAssert.AreItemEquals(setItem2, target[setIndex]);
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
                new MockWodiLib2DListValidator<IStubRestrictedCapacity2DListSettings,
                    IStubRestrictedCapacityListSettings, IStubModelSettings>();
            var innerList = new SimpleList<StubRestrictedCapacityList>(
                valueBuilder: TestClass.RowBuilder,
                initValues: TestClass.INIT_ROW_LENGTH.Iterate(rowIndex
                    => TestClass.BuildItemFromIndex(rowIndex)
                )
            );
            var twoDList = new TwoDimensionalList<
                IStubRestrictedCapacity2DListSettings,
                StubRestrictedCapacityList,
                FixedStubRestrictedCapacityList,
                IStubRestrictedCapacityListSettings,
                StubModel,
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
                getValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList>(result =>
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
                        CustomAssert.AreItemEquals(testClass.InnerList[rowIndex], result);
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
                getValueVerifier: new ValueVerifier<StubModel>(result =>
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
                        CustomAssert.AreItemEquals(addItem, instance[addIndex]);
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
                resultValueVerifier: new ValueVerifier<IEnumerator<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        // 取得した IEnumerator から値を取り出す
                        var actualValues = new List<FixedStubRestrictedCapacityList>();
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
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
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
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
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
                resultValueVerifier: new ValueVerifier<IEnumerable<IEnumerable<StubModel>>>(actual =>
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
                resultValueVerifier: ValueVerifier<StubModel>.AreReferenceEquals(
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            CustomAssert.AreItemEquals(testClass.InnerList[rowIndex + i], actualArray[i]);
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
                                Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[beforeRow], target[r]);

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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[beforeRow], target[r]);

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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[r], target[r]);
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[r], target[r]);
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[r], target[r]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                        CustomAssert.AreItemEquals(initRows[2], testClass.InnerList[0]);
                        CustomAssert.AreItemEquals(initRows[0], testClass.InnerList[1]);
                        CustomAssert.AreItemEquals(initRows[1], testClass.InnerList[2]);
                        CustomAssert.AreItemEquals(initRows[3], testClass.InnerList[3]);
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
            var initRows = instance.ToArray();
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
                        CustomAssert.AreItemEquals(initRows[1], testClass.InnerList[0]);
                        CustomAssert.AreItemEquals(initRows[2], testClass.InnerList[1]);
                        CustomAssert.AreItemEquals(initRows[0], testClass.InnerList[2]);
                        CustomAssert.AreItemEquals(initRows[3], testClass.InnerList[3]);
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
            var initRows = instance.ToArray();
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
                        CustomAssert.AreItemEquals(initRows[0], testClass.InnerList[2]);
                        CustomAssert.AreItemEquals(initRows[1], testClass.InnerList[3]);
                        CustomAssert.AreItemEquals(initRows[2], testClass.InnerList[0]);
                        CustomAssert.AreItemEquals(initRows[3], testClass.InnerList[1]);
                        CustomAssert.AreItemEquals(initRows[4], testClass.InnerList[4]);
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
            var initRows = instance.ToArray();
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
                        CustomAssert.AreItemEquals(initRows[3], testClass.InnerList[0]);
                        CustomAssert.AreItemEquals(initRows[4], testClass.InnerList[1]);
                        CustomAssert.AreItemEquals(initRows[0], testClass.InnerList[2]);
                        CustomAssert.AreItemEquals(initRows[1], testClass.InnerList[3]);
                        CustomAssert.AreItemEquals(initRows[2], testClass.InnerList[4]);
                        CustomAssert.AreItemEquals(initRows[5], testClass.InnerList[5]);
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
            var initRows = instance.Select(row => row.ToArray()).ToArray();
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
            var initRows = instance.Select(row => row.ToArray()).ToArray();
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
            var initRows = instance.Select(row => row.ToArray()).ToArray();
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
            var initRows = instance.Select(row => row.ToArray()).ToArray();
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
            var initRows = instance.ToArray();
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
                                    target[beforeRow],
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
            var initRows = instance.ToArray();
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
                                    target[beforeRow],
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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

        #region WithSettings

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

        #region Strict

        /// <summary>
        ///     <para>指定した設定でリセットされること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ResetStrictTest_WithSettings_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH.Iterate(r => TestClass.BuildRowSettingsFromRowIndex(100 + r)
                )
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.ResetStrict(settings),
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
                                .Args[0],
                            EqualityComparerFactory.Create<IStubRestrictedCapacityListSettings>()
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
                            CustomAssert.AreItemEquals(actualArray[i], testClass.InnerList[i]);
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
        ///     ValidateReset（設定あり） メソッドで Validator の処理が呼ばれること。
        /// </summary>
        [Test]
        public static void ValidateResetTest_WithSettings_Success()
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

        /// <summary>
        ///     <para>ValidateResetStrict メソッドで Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateReseStricttTest_Success()
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
                execAction: target => target.ValidateResetStrict(settings)
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
                (IStubRestrictedCapacityListSettings[])testClass.MockValidator.CalledMemberHistory[0].Args[0],
                EqualityComparerFactory.Create<IStubRestrictedCapacityListSettings>()
            );
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
                resultValueVerifier: ValueVerifier<FixedStubRestrictedCapacityList>.AreItemEquals(
                    testClass.InnerList[rowIndex]
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
                        CustomAssert.AreItemEquals(testClass.InnerList[0], actualArray[0]);
                        CustomAssert.AreItemEquals(testClass.InnerList[1], actualArray[1]);
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
            var initRows = instance.ToArray();
            const int rowIndex = 1;
            var settings = TestClass.BuildRowSettingsFromRowIndex(rowIndex + 100);

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRowInternal(rowIndex, settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<FixedStubRestrictedCapacityList>(actual =>
                    CustomAssert.AreItemEquals(testClass.InnerList[rowIndex], actual)
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
                                Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                        CustomAssert.AreItemEquals(testClass.InnerList[rowIndex], actualArray[0]);
                        CustomAssert.AreItemEquals(testClass.InnerList[rowIndex + 1], actualArray[1]);
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
                                Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[beforeRow], target[r]);

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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                                Assert.AreSame(initRows[r], target[r]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                        CustomAssert.AreItemEquals(initRows[1], testClass.InnerList[0]);
                        CustomAssert.AreItemEquals(initRows[2], testClass.InnerList[1]);
                        CustomAssert.AreItemEquals(initRows[0], testClass.InnerList[2]);
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
            var initRows = instance.Select(row => row.ToArray()).ToArray();
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
            var initRows = instance.Select(row => row.ToArray()).ToArray();
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
            var initRows = instance.ToArray();
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
                                target[r],
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[beforeRow], target[r]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
            var initRows = instance.ToArray();
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
                            Assert.AreSame(initRows[i], target[i]);
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
            var initRows = instance.Select(r => r.DeepClone()).ToArray();
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
                            CustomAssert.AreItemEquals(testClass.InnerList[i], actualArray[i]);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        /// <summary>
        ///     <para>ResetStrictInternal メソッドが正常に処理され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ResetStrictInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = TestClass.INIT_ROW_LENGTH.Iterate(i => TestClass.BuildRowSettingsFromRowIndex(i + 100))
                .ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.ResetStrictInternal(settings),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                resultValueVerifier: new ValueVerifier<IEnumerable<FixedStubRestrictedCapacityList>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(settings.Length, actualArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(testClass.InnerList[i], actualArray[i]);
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
                            CustomAssert.AreItemEquals(actualArray[i], testClass.InnerList[i]);
                        }
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
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

            public MockWodiLib2DListValidator<IStubRestrictedCapacity2DListSettings, IStubRestrictedCapacityListSettings
                , IStubModelSettings> MockValidator { get; }

            public SimpleList<StubRestrictedCapacityList> InnerList { get; }

            public TestClass(int rowCount = INIT_ROW_LENGTH, int columnCount = INIT_COLUMN_LENGTH)
            {
                var validator =
                    new MockWodiLib2DListValidator<IStubRestrictedCapacity2DListSettings,
                        IStubRestrictedCapacityListSettings, IStubModelSettings>();
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
                MockWodiLib2DListValidator<IStubRestrictedCapacity2DListSettings, IStubRestrictedCapacityListSettings,
                    IStubModelSettings>? validator
            )
            {
                return new Test2DList.Config(
                    RowSettingsFactoryRowIndex: BuildItemFromIndex,
                    RowFactoryFromSettings: BuildRowFromSettings,
                    ItemFactory: BuildListElementFromSetting,
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
                int columnLength = INIT_COLUMN_LENGTH,
                SimpleList<StubRestrictedCapacityList> _ = null!
            )
                => new(BuildRowSettingsFromRowIndex(rowIndex, columnLength));

            public static IStubRestrictedCapacityListSettings BuildRowSettingsFromRowIndex(
                int rowIndex,
                int columnLength = INIT_COLUMN_LENGTH
            )
                => new StubRestrictedCapacityListSettings(
                    columnLength.Iterate<IStubModelSettings>(columnIndex => new StubModelSettings
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
        }
    }
}
