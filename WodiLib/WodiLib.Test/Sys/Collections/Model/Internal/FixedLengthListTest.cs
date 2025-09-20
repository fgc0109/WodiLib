using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestList = WodiLib.Sys.Collections.FixedLengthList<
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
     * ReadOnlyExtendedList で実装されており、オーバーライド等していないメソッドのテストは行わない。
     */

    [TestFixture]
    public class FixedLengthListTest
    {
        private static Logger logger = null!;

        private static ConstructorTestHelper constructorTestHelper = null!;
        private static PureActionTestHelper pureActionTestHelper = null!;
        private static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private static ImpureActionTestHelper impureActionTestHelper = null!;
        private static ImpureFunctionTestHelper impureFunctionTestHelper = null!;
        private static ItemEqualsTestHelper itemEqualsTestHelper = null!;

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
            itemEqualsTestHelper = new ItemEqualsTestHelper(logger);
        }

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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
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

        #endregion

        #endregion

        #region Constructors

        #region SimpleListAndValidatorAndBuilder

        /// <summary>
        ///     <para>コンストラクタが正常に終了すること。</para>
        ///     <para>Items プロパティの実態がコンストラクタで与えた IExtendedList であること。</para>
        /// </summary>
        [Test]
        public static void ConstructorTest_SimpleListAndValidatorAndBuilder_Success()
        {
            var itemsImpl = new SimpleList<StubModel>(
                valueBuilder: new SimpleListValueBuilder<StubModel>(i => new StubModel(i.ToString()))
            );
            IWodiLibListValidator<IStubModelSettings> validator = new MockWodiLibListValidator<IStubModelSettings>();
            TestList.BuildItemFromSettingsDelegate buildItemFromSettingsDelegate =
                (_, settings) => new StubModel(settings);

            constructorTestHelper.ConstructorSuccess(
                factory: () => new TestList(itemsImpl, validator, buildItemFromSettingsDelegate),
                instanceVerifier: new ValueVerifier<TestList>(instance =>
                    {
                        // itemsImpl に対する変更が instance にも反映されること
                        var notifiedPropertyChanged = new List<string>();
                        instance.PropertyChanged += (_, args) => { notifiedPropertyChanged.Add(args.PropertyName!); };

                        var addIndex = itemsImpl.Count;
                        var addItem = new StubModel("Add Item.");

                        itemsImpl.Add(addItem);

                        //   プロパティ変更通知が行われること
                        Assert.AreEqual(2, notifiedPropertyChanged.Count);
                        Assert.AreEqual(nameof(instance.Count), notifiedPropertyChanged[0]);
                        Assert.AreEqual(ListConstant.IndexerName, notifiedPropertyChanged[1]);

                        //   instance の要素が追加されていること
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
        [TestCase("buildItemFromSettings")]
        public static void ConstructorTest_SimpleListAndValidatorAndBuilder_Failure_NullArgs(
            string nullArgName
        )
        {
            var itemsImpl =
                nullArgName == "itemsImpl"
                    ? null!
                    : new SimpleList<StubModel>(
                        valueBuilder: new SimpleListValueBuilder<StubModel>(i => new StubModel(i.ToString()))
                    );
            IWodiLibListValidator<IStubModelSettings> validator = new MockWodiLibListValidator<IStubModelSettings>();
            TestList.BuildItemFromSettingsDelegate buildItemFromSettingsDelegate =
                nullArgName == "buildItemFromSettings"
                    ? null!
                    : (_, settings) => new StubModel(settings);

            constructorTestHelper.ConstructorFailure(
                factory: () => new TestList(itemsImpl, validator, buildItemFromSettingsDelegate),
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
                resultValueVerifier: new ValueVerifier<IEnumerator<StubModel>>(actual =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 取得した IEnumerator から値を取り出す
                        var actualValues = new List<StubModel>();
                        while (actual.MoveNext())
                        {
                            actualValues.Add(actual.Current);
                        }

                        // 取得した値が意図した値であること
                        Assert.IsTrue(
                            actualValues.SequenceEqual(innerList, ReferenceEquals)
                        );
                    }
                )
            );
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
                resultValueVerifier: new ValueVerifier<StubModel>(actual => Assert.AreSame(
                        actual,
                        testClass.InnerList[index]
                    )
                )
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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
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
                execFunc: target => target.Reset(resetItems),
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
                            ((IEnumerable<StubModelSettings>)testClass.MockValidator.CalledMemberHistory[0].Args[0])
                            == resetItems
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

        #region ValidateSet

        /// <summary>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int index = 1;
            var setItem = new StubModel("Update Stub Model");

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSet(index, setItem)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.Set)
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(setItem, ((object[])testClass.MockValidator.CalledMemberHistory[0].Args[1])[0]);
        }

        #endregion

        #region ValidateSetRange

        /// <summary>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateSetRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var setItems = new StubModelSettings[]
            {
                new() { StringValue = "Update Stub Model 1", Tags = new[] { "Tag1", "Tag2" } },
                new() { StringValue = "Update Stub Model 2" },
            };
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateSetRange(index, setItems)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.Set)
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(index, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreSame(setItems, testClass.MockValidator.CalledMemberHistory[0].Args[1]);
        }

        #endregion

        #region ValidateMove

        /// <summary>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateMoveTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldIndex = 1;
            const int newIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateMove(oldIndex, newIndex)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.Move)
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(1, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateMoveRange

        /// <summary>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateMoveRangeTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            const int oldIndex = 1;
            const int newIndex = 3;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateMoveRange(oldIndex, newIndex, count)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.Move)
            );
            Assert.AreEqual(3, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreEqual(oldIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[0]);
            Assert.AreEqual(newIndex, (int)testClass.MockValidator.CalledMemberHistory[0].Args[1]);
            Assert.AreEqual(count, (int)testClass.MockValidator.CalledMemberHistory[0].Args[2]);
        }

        #endregion

        #region ValidateReset

        /// <summary>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ValidateResetTest_Success()
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

            pureActionTestHelper.PureActionSuccess(
                instance,
                execAction: target => target.ValidateReset(resetItems)
            );

            // Validator の処理が呼ばれること
            Assert.AreEqual(1, testClass.MockValidator.CalledMemberHistory.Count);
            Assert.IsTrue(
                testClass.MockValidator.CalledMemberHistory[0].MethodName
                == nameof(testClass.MockValidator.Reset)
            );
            Assert.AreEqual(2, testClass.MockValidator.CalledMemberHistory[0].Args.Length);
            Assert.AreSame(resetItems, testClass.MockValidator.CalledMemberHistory[0].Args[0]);
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
                resultValueVerifier: ValueVerifier.AreReferenceEquals(testClass.InnerList[index])
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
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
            const int index = 1;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.GetRangeInternal(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(actual =>
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

        #region SetInternal

        /// <summary>
        ///     <para>指定した要素が設定されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
            const int index = 1;
            var setItem = new StubModel("Update Stub Model");

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetInternal(index, setItem),
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
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 編集した要素が変更されていること、与えた設定DTOから作成したインスタンスであること
                        CustomAssert.AreItemEquals(setItem, target[index]);
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

        #region SetRangeInternal

        /// <summary>
        ///     <para>指定した範囲の要素が設定されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void SetRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
            var setItems = new StubModelSettings[]
            {
                new() { StringValue = "Update Stub Model 1", Tags = new[] { "Tag1", "Tag2" } },
                new() { StringValue = "Update Stub Model 2" },
            };
            const int index = 1;

            testClass.MockValidator.ClearCalledHistory();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.SetRangeInternal(index, setItems),
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
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

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

        #region MoveInternal

        /// <summary>
        ///     <para>指定した要素が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void MoveInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
            const int oldIndex = 1;
            const int newIndex = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveInternal(oldIndex, newIndex),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(_ =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

                        // 要素が正しく移動していること
                        Assert.AreSame(testClass.InnerList[0], initItems[0]);
                        Assert.AreSame(testClass.InnerList[2], initItems[1]);
                        Assert.AreSame(testClass.InnerList[1], initItems[2]);
                        Assert.AreSame(testClass.InnerList[3], initItems[3]);
                        Assert.AreSame(testClass.InnerList[4], initItems[4]);
                    }
                )
            );
        }

        #endregion

        #region MoveRangeInternal

        /// <summary>
        ///     <para>指定した範囲の要素が移動されること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void MoveRangeInternalTest_Success()
        {
            var testClass = new TestClass();
            var instance = testClass.TestInstance;
            var initItems = ((IEnumerable<StubModel>)instance).ToArray();
            const int oldIndex = 1;
            const int newIndex = 3;
            const int count = 2;

            testClass.MockValidator.ClearCalledHistory();

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.MoveRangeInternal(oldIndex, newIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<TestList>(_ =>
                    {
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

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

        #region ResetInternal

        /// <summary>
        ///     <para>指定した設定でリセットされること。</para>
        ///     <para>プロパティ変更通知がされること。</para>
        ///     <para>Validator の処理が呼ばれないこと。</para>
        /// </summary>
        [Test]
        public static void ResetInternalTest_Success()
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
                execFunc: target => target.ResetInternal(resetItems),
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
                        // Validator の処理が呼ばれないこと
                        Assert.AreEqual(0, testClass.MockValidator.CalledMemberHistory.Count);

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
