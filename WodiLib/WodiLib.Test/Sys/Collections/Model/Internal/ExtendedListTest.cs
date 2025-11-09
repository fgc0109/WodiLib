using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestList = WodiLib.Sys.Collections.ExtendedList<
    WodiLib.Test.Tools.IStubRestrictedCapacityListSettings,
    WodiLib.Test.Tools.StubModel,
    WodiLib.Test.Tools.IStubModelSettings
>;

// ReSharper disable RedundantArgumentDefaultValue

namespace WodiLib.Test.Sys.Collections
{
    /*
     * 各メソッドの引数検証は行わない、いずれもエラーとならない引数のみを指定してテストする。
     *      => 各メソッドの引数検証はコンストラクタで与えるValidatorによって決まるため
     * ただし、コンストラクタから Mock Validator を注入し、Validator の意図したメソッドが呼ばれることを検証する。
     */

    /// <summary>
    ///     <see cref="ExtendedList{TListSettings,TEditableElement,TElementSettings}"/> のテスト
    /// </summary>
    [TestFixture]
    public class ExtendedListTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
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
            Assert.IsInstanceOf<StubModel>(collectionChangedEventArgsList[0].OldItems![0]);
            CustomAssert.AreItemEquals(
                initItems[setIndex],
                (StubModel)collectionChangedEventArgsList[0].OldItems![0]!
            );
            Assert.AreEqual(setIndex, collectionChangedEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, collectionChangedEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubModel>(collectionChangedEventArgsList[0].NewItems![0]);
            Assert.AreSame(setItem, (StubModel)collectionChangedEventArgsList[0].NewItems![0]!);

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
        ///     <para>インデクサの取得・編集に成功すること。</para>
        ///     <para>取得結果が意図した値であること。</para>
        ///     <para>Validatorのメソッドが意図したとおり呼ばれること。</para>
        /// </summary>
        [Test]
        public static void IndexerGetterAndSetterTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int index = 1;
            var setItem = new StubModel("Update Stub Model");

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
                instance[index] = setItem;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);

            // プロパティ変更通知が発火していること（対象プロパティのみチェック）
            Assert.AreNotEqual(0, changedPropertyList.Count);
            Assert.AreEqual(
                ListConstant.IndexerName,
                changedPropertyList[0]
            );

            // Validatorのメソッドが意図したとおり呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.Set)
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            CustomAssert.AreSequenceEquals(
                new[] { setItem },
                (IStubModelSettings[])testClass.MockValidator.CalledMemberHistory[0].Args[1],
                EqualityComparerFactory.Create<IStubModelSettings>()
            );

            // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
            CustomAssert.AreItemEquals(setItem, testClass.InnerList[index]);
            Assert.AreNotSame(setItem, testClass.InnerList[index]);

            // 編集していない要素が変更されていないこと
            for (var i = 0; i < TestClass.INIT_LENGTH; i++)
            {
                if (!i.IsBetween(index - 1, index))
                {
                    CustomAssert.AreItemEquals(initItems[i], testClass.InnerList[i]);
                }
            }
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

        #region SimpleListAndCapacitiesAndValidatorAndBuilder

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_SimpleListAndCapacitiesAndValidatorAndBuilder_Success()
        {
            const int maxCapacity = 100;
            const int minCapacity = 10;
            var itemsImpl = new SimpleList<StubModel>(
                valueBuilder: new SimpleListValueBuilder<StubModel>(i => new StubModel(i.ToString())),
                initValues: minCapacity.Iterate(i => new StubModel((i * 10).ToString())).ToArray()
            );
            IWodiLibListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings> validator =
                new MockWodiLibListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>();
            TestList.BuildItemFromSettingsDelegate
                buildItemFromSettingsDelegate =
                    (_, settings) => new StubModel(settings);

            constructorTestHelper.ConstructorSuccess(
                factory: () => new TestList(
                    itemsImpl,
                    minCapacity,
                    maxCapacity,
                    validator,
                    buildItemFromSettingsDelegate
                ),
                instanceVerifier: new ValueVerifier<TestList>(instance =>
                    {
                        // itemsImpl に対する変更が instance にも反映されること
                        var notifiedPropertyChanged = new List<string>();
                        instance.PropertyChanged += (_, args) => { notifiedPropertyChanged.Add(args.PropertyName!); };

                        const int index = 1;
                        var setItem = new StubModel("set item");

                        itemsImpl[index] = setItem;

                        //   プロパティ変更通知が行われること
                        Assert.AreEqual(1, notifiedPropertyChanged.Count);
                        Assert.AreEqual(ListConstant.IndexerName, notifiedPropertyChanged[0]);

                        //   instance の要素が置換されていること
                        Assert.AreSame(setItem, instance[index]);
                    }
                )
            );
        }

        /// <summary>
        ///     null を許容しない引数に null が指定された場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        /// <param name="nullArgName"></param>
        [TestCase("itemsImpl")]
        public static void ConstructorTest_Standard_Failure_ArgumentNull(string nullArgName)
        {
            const int maxCapacity = 100;
            const int minCapacity = 10;
            var itemsImpl =
                nullArgName == "itemsImpl"
                    ? null!
                    : new SimpleList<StubModel>(
                        valueBuilder: new SimpleListValueBuilder<StubModel>(i => new StubModel(i.ToString())),
                        initValues: minCapacity.Iterate(i => new StubModel((i * 10).ToString())).ToArray()
                    );
            IWodiLibListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings> validator =
                new MockWodiLibListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>();
            TestList.BuildItemFromSettingsDelegate
                buildItemFromSettingsDelegate =
                    nullArgName == "buildItemFromSettings"
                        ? null!
                        : (_, settings) => new StubModel(settings);

            constructorTestHelper.ConstructorFailure(
                factory: () => new TestList(
                    itemsImpl,
                    minCapacity,
                    maxCapacity,
                    validator,
                    buildItemFromSettingsDelegate
                ),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region GetMaxCapacity

        /// <summary>
        ///     GetMaxCapacity が正常に実行され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetMaxCapacityTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int expected = TestClass.MAX_CAPACITY;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetMaxCapacity(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #region GetMinCapacity

        /// <summary>
        ///     GetMinCapacity が正常に実行され、意図した結果が取得されること。
        /// </summary>
        [Test]
        public static void GetMinCapacityTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int expected = TestClass.MIN_CAPACITY;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetMinCapacity(),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

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
                resultValueVerifier: new ValueVerifier<IEnumerator<StubModel>>(actual =>
                    {
                        // 取得した IEnumerator から値を取り出す
                        var actualValues = new List<StubModel>();
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
                resultValueVerifier: ValueVerifier<StubModel>.AreReferenceEquals(testClass.InnerList[index])
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
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
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

        #region Set

        /// <summary>
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int index = 1;
            var setItem = new StubModel("Update Stub Model");

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Set(index, setItem),
                resultValueVerifier: new ValueVerifier<StubModel>(actual =>
                    {
                        Assert.AreSame(testClass.InnerList[index], actual);
                        Assert.AreNotSame(actual, setItem);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Set),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(index, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        CustomAssert.AreItemEquals(target[index], setItem);
                        Assert.AreNotEqual(target[index], setItem);

                        // 編集していない要素が変更されていないこと
                        for (var i = 0; i < TestClass.INIT_LENGTH; i++)
                        {
                            if (i != index)
                            {
                                Assert.AreSame(initItems[i], target[i], $"target[{i}] not same initItems[{i}]");
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region SetRange

        /// <summary>
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void SetRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            var setItems = new StubModelSettings[]
            {
                new() { StringValue = "Update Stub Model 1", Tags = new[] { "Tag1", "Tag2" } },
                new() { StringValue = "Update Stub Model 2" },
            };
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRange(index, setItems),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(setItems.Length, actualArray.Length);
                        Assert.AreSame(testClass.InnerList[index], actualArray[0]);
                        Assert.AreSame(testClass.InnerList[index + 1], actualArray[1]);
                        Assert.AreNotSame(actualArray[0], setItems[0]);
                        Assert.AreNotSame(actualArray[0].Tags, setItems[0].Tags);
                        Assert.AreNotSame(actualArray[1], setItems[1]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Set),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(index, ((int)testClass.MockValidator.CalledMemberHistory[0].Args[0]));
                        Assert.IsTrue(
                            ((IEnumerable<StubModelSettings>)testClass.MockValidator.CalledMemberHistory[0].Args[1])
                            == setItems
                        );

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        CustomAssert.AreItemEquals(setItems[0], target[index]);
                        Assert.AreNotEqual(target[index], setItems[0]);
                        CustomAssert.AreItemEquals(setItems[1], target[index + 1]);
                        Assert.AreNotEqual(target[index + 1], setItems[1]);

                        // 編集していない要素が変更されていないこと
                        for (var i = 0; i < TestClass.INIT_LENGTH; i++)
                        {
                            if (!i.IsBetween(index, index + setItems.Length - 1))
                            {
                                Assert.AreSame(initItems[i], target[i], $"target[{i}] not same initItems[{i}]");
                            }
                        }
                    }
                )
            );
        }

        #endregion

        #region Add

        /// <summary>
        ///     <para>Add が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AddTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = new StubModelSettings { StringValue = "Add Item" };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Add(settings),
                resultValueVerifier: new ValueVerifier<StubModel>(result =>
                    {
                        CustomAssert.AreItemEquals(settings, result);
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Insert),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.INIT_LENGTH + 1, target.Count);
                        CustomAssert.AreItemEquals(settings, target[TestClass.INIT_LENGTH]);
                    }
                )
            );
        }

        #endregion

        #region AddRange

        /// <summary>
        ///     <para>AddRange が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AddRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = new[]
            {
                new StubModelSettings { StringValue = "Add Item 0" },
                new StubModelSettings { StringValue = "Add Item 1" },
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AddRange(settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        Assert.AreEqual(settings.Length, resultArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Insert),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.INIT_LENGTH + settings.Length, target.Count);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], target[TestClass.INIT_LENGTH + i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region Insert

        /// <summary>
        ///     <para>Insert が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void InsertTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            var settings = new StubModelSettings { StringValue = "Insert Item" };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Insert(index, settings),
                resultValueVerifier: new ValueVerifier<StubModel>(result =>
                    {
                        CustomAssert.AreItemEquals(settings, result);
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Insert),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.INIT_LENGTH + 1, target.Count);
                        CustomAssert.AreItemEquals(settings, target[index]);
                    }
                )
            );
        }

        #endregion

        #region InsertRange

        /// <summary>
        ///     <para>InsertRange が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void InsertRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            var settings = new[]
            {
                new StubModelSettings { StringValue = "Insert Item 0" },
                new StubModelSettings { StringValue = "Insert Item 1" },
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.InsertRange(index, settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        Assert.AreEqual(settings.Length, resultArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Insert),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.INIT_LENGTH + settings.Length, target.Count);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], target[index + i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region Overwrite

        /// <summary>
        ///     <para>Overwrite が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void OverwriteTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            var settings = new[]
            {
                new StubModelSettings { StringValue = "Overwrite Item 0" },
                new StubModelSettings { StringValue = "Overwrite Item 1" },
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        Assert.AreEqual(settings.Length, resultArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[] { ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Overwrite),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], target[index + i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region Move

        /// <summary>
        ///     <para>指定した要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int oldIndex = 1;
            const int newIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Move(oldIndex, newIndex),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Move),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 要素が正しく移動していること
                        Assert.AreSame(initItems[0], testClass.InnerList[0]);
                        Assert.AreSame(initItems[1], testClass.InnerList[2]);
                        Assert.AreSame(initItems[2], testClass.InnerList[1]);
                        Assert.AreSame(initItems[3], testClass.InnerList[3]);
                        Assert.AreSame(initItems[4], testClass.InnerList[4]);
                    }
                )
            );
        }

        #endregion

        #region MoveRange

        /// <summary>
        ///     <para>指定した範囲の要素が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void MoveRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = instance.ToArray();
            const int oldIndex = 1;
            const int newIndex = 3;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRange(oldIndex, newIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(_ =>
                    {
                        // Validatorのメソッドが意図したとおり呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Move),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.AreEqual(oldIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
                        Assert.AreEqual(newIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
                        Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);

                        // 要素が正しく移動していること
                        Assert.AreSame(testClass.InnerList[0], initItems[0]);
                        Assert.AreSame(testClass.InnerList[1], initItems[3]);
                        Assert.AreSame(testClass.InnerList[2], initItems[4]);
                        Assert.AreSame(testClass.InnerList[3], initItems[1]);
                        Assert.AreSame(testClass.InnerList[4], initItems[2]);
                    }
                )
            );
        }

        #endregion

        #region Remove

        /// <summary>
        ///     <para>Remove が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void RemoveTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            var originalItem = instance[index];

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Remove(index),
                resultValueVerifier: new ValueVerifier<StubModel>(result => { Assert.AreSame(originalItem, result); }),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Remove),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.INIT_LENGTH - 1, target.Count);
                    }
                )
            );
        }

        #endregion

        #region RemoveRange

        /// <summary>
        ///     <para>RemoveRange が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void RemoveRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            const int count = 2;
            var originalItems = instance.Skip(index).Take(count).ToArray();

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.RemoveRange(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        Assert.AreEqual(count, resultArray.Length);
                        for (var i = 0; i < count; i++)
                        {
                            Assert.AreSame(originalItems[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Remove),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.INIT_LENGTH - count, target.Count);
                    }
                )
            );
        }

        #endregion

        #region AdjustLength

        /// <summary>
        ///     <para>AdjustLength が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AdjustLengthTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int newLength = TestClass.INIT_LENGTH + 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustLength(newLength),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        var resultArray = result.ToArray();
                        Assert.AreEqual(2, resultArray.Length); // 追加された要素数
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target => { Assert.AreEqual(newLength, target.Count); })
            );
        }

        #endregion

        #region AdjustLengthIfShort

        /// <summary>
        ///     <para>AdjustLengthIfShort が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>現在より長い場合のみ調整されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AdjustLengthIfShortTest_Success_Extend()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int newLength = TestClass.INIT_LENGTH + 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustLengthIfShort(newLength),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        var resultArray = result.ToArray();
                        Assert.AreEqual(2, resultArray.Length); // 追加された要素数
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target => { Assert.AreEqual(newLength, target.Count); })
            );
        }

        /// <summary>
        ///     <para>現在より短い場合、何も変更されないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AdjustLengthIfShortTest_Success_NoChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int newLength = TestClass.INIT_LENGTH - 1;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustLengthIfShort(newLength),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        var resultArray = result.ToArray();
                        Assert.AreEqual(0, resultArray.Length); // 変更なし
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        Assert.AreEqual(TestClass.INIT_LENGTH, target.Count); // 変更なし
                    }
                )
            );
        }

        #endregion

        #region AdjustLengthIfLong

        /// <summary>
        ///     <para>AdjustLengthIfLong が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>現在より短い場合のみ調整されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AdjustLengthIfLongTest_Success_Shrink()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int newLength = TestClass.INIT_LENGTH - 2;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustLengthIfLong(newLength),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        var resultArray = result.ToArray();
                        Assert.AreEqual(2, resultArray.Length); // 削除された要素数
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target => { Assert.AreEqual(newLength, target.Count); })
            );
        }

        /// <summary>
        ///     <para>現在より長い場合、何も変更されないこと。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void AdjustLengthIfLongTest_Success_NoChange()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int newLength = TestClass.INIT_LENGTH + 1;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustLengthIfLong(newLength),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.AdjustLength),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        var resultArray = result.ToArray();
                        Assert.AreEqual(0, resultArray.Length); // 変更なし
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        Assert.AreEqual(TestClass.INIT_LENGTH, target.Count); // 変更なし
                    }
                )
            );
        }

        #endregion

        #region Reset

        #region WithSettings

        /// <summary>
        ///     <para>Reset が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_WithSettings_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var settings = new[]
            {
                new StubModelSettings { StringValue = "Reset Item 0" },
                new StubModelSettings { StringValue = "Reset Item 1" },
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(settings),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        Assert.AreEqual(settings.Length, resultArray.Length);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.IsTrue(
                            (IEnumerable<StubModelSettings>)testClass.MockValidator.CalledMemberHistory[0].Args[0]
                            == settings
                        );
                        Assert.AreEqual(
                            true,
                            testClass.MockValidator.CalledMemberHistory[0].Args[1]
                        );

                        Assert.AreEqual(settings.Length, target.Count);
                        for (var i = 0; i < settings.Length; i++)
                        {
                            CustomAssert.AreItemEquals(settings[i], target[i]);
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
        public static void ResetStrictTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var resetItems = new StubModelSettings[]
            {
                new() { StringValue = "Reset Item 0", Tags = new[] { "Reset", "Tag0" } },
                new() { StringValue = "Reset Item 1" },
                new() { StringValue = "Reset Item 2", Tags = new[] { "Reset", "Tag2" } },
                new() { StringValue = "Reset Item 3" },
                new() { StringValue = "Reset Item 4" },
            };

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.ResetStrict(resetItems),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(resetItems.Length, actualArray.Length);
                        for (var i = 0; i < resetItems.Length; i++)
                        {
                            Assert.AreSame(testClass.InnerList[i], actualArray[i]);
                            Assert.AreNotSame(actualArray[i], resetItems[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
                        Assert.IsTrue(
                            (IEnumerable<StubModelSettings>)testClass.MockValidator.CalledMemberHistory[0].Args[0]
                            == resetItems
                        );
                        Assert.AreEqual(
                            false,
                            testClass.MockValidator.CalledMemberHistory[0].Args[1]
                        );

                        // すべての要素がリセットされていること
                        for (var i = 0; i < resetItems.Length; i++)
                        {
                            CustomAssert.AreItemEquals(resetItems[i], target[i]);
                            Assert.AreNotEqual(target[i], resetItems[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region parameterless

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
            instance.Set(0, new StubModel("Modified Item"));
            instance.Set(1, new StubModel("Modified Item"));

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
                    {
                        var actualArray = actual.ToArray();
                        Assert.AreEqual(TestClass.INIT_LENGTH, actualArray.Length);
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
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Reset),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );
                        Assert.AreEqual(
                            0,
                            testClass.MockValidator.CalledMemberHistory[0].Args.Length
                        );

                        // すべての要素がデフォルト値にリセットされていること
                        for (var i = 0; i < TestClass.INIT_LENGTH; i++)
                        {
                            Assert.AreEqual(i.ToString(), target[i].StringValue);
                        }
                    }
                )
            );
        }

        #endregion

        #endregion

        #region Clear

        /// <summary>
        ///     <para>Clear が正常に実行され、リストの要素数が最小になること。</para>
        ///     <para>クリア後の要素がデフォルト要素作成処理によって作成された要素になること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ClearTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Clear(),
                expectedNotifyProperties: new[] { nameof(TestList.Count), ListConstant.IndexerName },
                instanceVerifier: new ValueVerifier<TestList>(target =>
                    {
                        // Validator の処理が呼ばれること
                        Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
                        Assert.AreEqual(
                            nameof(testClass.MockValidator.Clear),
                            testClass.MockValidator.CalledMemberHistory[0].MethodName
                        );

                        Assert.AreEqual(TestClass.MIN_CAPACITY, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(TestClass.BuildItemFromIndex(i), testClass.InnerList[i]);
                        }
                    }
                )
            );
        }

        #endregion

        #region ItemEquals

        /// <summary>
        ///     <para>同じインスタンスとの比較でTrueが返されること。</para>
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
        ///     <para>内容が同じ別インスタンスとの比較でTrueが返されること。</para>
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
        ///     <para>内容が異なるインスタンスとの比較でFalseが返されること。</para>
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
        ///     <para>nullとの比較でFalseが返されること。</para>
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
            public const int MAX_CAPACITY = 10;
            public const int MIN_CAPACITY = 3;

            public TestList TestInstance { get; }

            public MockWodiLibListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings> MockValidator
            {
                get;
            }

            public SimpleList<StubModel> InnerList { get; }

            public TestClass(int initLength = INIT_LENGTH)
            {
                var validator = new MockWodiLibListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>();
                MockValidator = validator;

                var innerList = new SimpleList<StubModel>(
                    valueBuilder: ElementBuilder,
                    initValues: initLength.Iterate(BuildItemFromIndex).ToArray()
                );
                InnerList = innerList;

                TestInstance = new TestList(
                    innerList,
                    MIN_CAPACITY,
                    MAX_CAPACITY,
                    validator,
                    BuildItemFromSettings
                );
            }

            private static SimpleListValueBuilder<StubModel> ElementBuilder { get; }
                = new(BuildItemFromIndex);

            public static StubModel BuildItemFromIndex(int index)
                => new(index.ToString());

            public static StubModel BuildItemFromSettings(int index, IStubModelSettings settings)
                => new(settings);
        }
    }
}
