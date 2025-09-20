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
using TestList = WodiLib.Sys.Collections.ReadOnlyExtendedList<
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
    public class ReadOnlyExtendedListTest
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
         * ExtendedList は内部的には SimpleList を呼び出す前提であるため。
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
            var setItem = new StubModel("new item");
            const int setIndex = 1;

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: _ => testClass.InnerList[setIndex] = setItem,
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(target =>
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
            Assert.IsInstanceOf<ReadOnlyStubModel>(collectionChangedEventArgsList[0].OldItems![0]);
            CustomAssert.AreItemEquals(
                initItems[setIndex],
                (ReadOnlyStubModel)collectionChangedEventArgsList[0].OldItems![0]!
            );
            Assert.AreEqual(setIndex, collectionChangedEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, collectionChangedEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<ReadOnlyStubModel>(collectionChangedEventArgsList[0].NewItems![0]);
            Assert.AreSame(setItem, (ReadOnlyStubModel)collectionChangedEventArgsList[0].NewItems![0]!);

            // ----------------------------------------
            //      イベントハンドラ解除後、通知されないことの確認

            propertyChangedEventArgsList.Clear();
            collectionChangedEventArgsList.Clear();
            // 前提条件：propertyChangedEventArgsList, collectionChangedEventArgsList がクリアされること
            Assert.AreEqual(0, propertyChangedEventArgsList.Count);
            Assert.AreEqual(0, collectionChangedEventArgsList.Count);

            instance.PropertyChanged -= propertyChangedEventHandler;
            instance.CollectionChanged -= collectionChangedEventHandler;

            var setItem2 = new StubModel("update model value");

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: _ => testClass.InnerList[setIndex] = setItem2,
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(target =>
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

        #endregion

        #region Properties

        #region public

        #region Indexer

        /// <summary>
        ///     <para>インデクサの取得に成功すること。</para>
        ///     <para>取得結果が意図した値であること。</para>
        ///     <para>Validatorのメソッドが意図したとおり呼ばれること。</para>
        /// </summary>
        [Test]
        public static void IndexerGetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target[index],
                getValueVerifier: new ValueVerifier<ReadOnlyStubModel>(result =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Get),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(index, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);

                        // 意図した値が取得されること
                        Assert.AreSame(testClass.InnerList[index], result);
                    }
                )
            );
        }

        #endregion

        #region Count

        /// <summary>
        ///     意図した値が取得されること。
        /// </summary>
        [Test]
        public static void CountGetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int expected = TestClass.INIT_LENGTH;

            propertyTestHelper.PropertyGetSuccess(
                instance,
                getter: target => target.Count,
                getValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Constructors

        #region SimpleListAndValidator

        /// <summary>
        ///     <para>コンストラクタが正常に終了すること。</para>
        ///     <para>Items プロパティの実態がコンストラクタで与えた IExtendedList であること。</para>
        /// </summary>
        [Test]
        public static void ConstructorTest_SimpleListAndValidator_Success()
        {
            var itemsImpl = new SimpleList<StubModel>(
                valueBuilder: new SimpleListValueBuilder<StubModel>(i => new StubModel(i.ToString()))
            );
            IWodiLibListValidator<IStubModelSettings> validator = new MockWodiLibListValidator<IStubModelSettings>();

            constructorTestHelper.ConstructorSuccess(
                factory: () => new TestList(itemsImpl, validator),
                instanceVerifier: new ValueVerifier<TestList>(instance =>
                    {
                        // itemsImpl に対する変更が instance にも反映されること
                        var notifiedPropertyChanged = new List<string>();
                        instance.PropertyChanged += (_, args) => { notifiedPropertyChanged.Add(args.PropertyName!); };

                        var insertIndex = itemsImpl.Count;
                        var addItem = new StubModel("set item");

                        itemsImpl.Insert(insertIndex, addItem);

                        //   プロパティ変更通知が行われること
                        Assert.AreEqual(2, notifiedPropertyChanged.Count);
                        Assert.AreEqual(nameof(instance.Count), notifiedPropertyChanged[0]);
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
        [Test]
        public static void ConstructorTest_SimpleListAndValidator_Failure_NullArgs()
        {
            SimpleList<StubModel> itemsImpl = null!;
            IWodiLibListValidator<IStubModelSettings> validator = new MockWodiLibListValidator<IStubModelSettings>();

            constructorTestHelper.ConstructorFailure(
                factory: () => new TestList(itemsImpl, validator),
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
                resultValueVerifier: new ValueVerifier<IEnumerator<ReadOnlyStubModel>>(actual =>
                    {
                        // 取得した IEnumerator から値を取り出す
                        var actualValues = new List<ReadOnlyStubModel>();
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

        #region Get

        /// <summary>
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.Get(index),
                resultValueVerifier: ValueVerifier<ReadOnlyStubModel>.AreReferenceEquals(testClass.InnerList[index])
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.Get),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region GetRange

        /// <summary>
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void GetRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int index = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRange(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<ReadOnlyStubModel>>(actual =>
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
                nameof(testClass.MockValidator.Get),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(count, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region ValidateGet

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGet(index)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.Get),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(1, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region ValidateGetRange

        /// <summary>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateGetRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateGetRange(index, count)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.AreEqual(
                nameof(testClass.MockValidator.Get),
                testClass.MockValidator.CalledMemberHistory[0].MethodName
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
            Assert.AreEqual(count, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[1]));
        }

        #endregion

        #region GetInternal

        /// <summary>
        ///     <para>意図した結果が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetInternal(index),
                resultValueVerifier: ValueVerifier<ReadOnlyStubModel>.AreReferenceEquals(testClass.InnerList[index])
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
        }

        #endregion

        #region GetRangeInternal

        /// <summary>
        ///     <para>指定した範囲の要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void GetRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int index = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRangeInternal(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<ReadOnlyStubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(count, actualArray.Length);
                        Assert.AreSame(initItems[1], actualArray[0]);
                        Assert.AreSame(initItems[2], actualArray[1]);
                    }
                )
            );

            // Validator の処理が呼ばれないこと
            Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);
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
            testClass.InnerList[0] = new StubModel("Diff Item");

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
            public const int INIT_LENGTH = 5;

            public TestList TestInstance { get; }
            public MockWodiLibListValidator<IStubModelSettings> MockValidator { get; }
            public SimpleList<StubModel> InnerList { get; }

            public TestClass(int initLength = INIT_LENGTH)
            {
                var validator = new MockWodiLibListValidator<IStubModelSettings>();
                MockValidator = validator;

                var innerList = new SimpleList<StubModel>(
                    valueBuilder: ElementBuilder,
                    initValues: initLength.Iterate(BuildItemFromIndex).ToArray()
                );
                InnerList = innerList;

                TestInstance = new TestList(
                    innerList,
                    validator
                );
            }

            private static SimpleListValueBuilder<StubModel> ElementBuilder { get; }
                = new(BuildItemFromIndex);

            public static StubModel BuildItemFromIndex(int index)
                => new(index.ToString());
        }
    }
}
