using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;
using TestList = WodiLib.Sys.Collections.ExtendedList<
    WodiLib.Test.Tools.StubModel,
    WodiLib.Test.Tools.ReadOnlyStubModel,
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
    ///     <see cref="ExtendedList{TEditableElement,TReadOnlyElement,TElementSettings}"/> のテスト
    /// </summary>
    [TestFixture]
    public class ExtendedListTest
    {
        private static Logger logger = null!;

        private static ConstructorTestHelper constructorTestHelper = null!;
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
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            impureActionTestHelper = new ImpureActionTestHelper(logger);
            impureFunctionTestHelper = new ImpureFunctionTestHelper(logger);
            itemEqualsTestHelper = new ItemEqualsTestHelper(logger);
        }

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
            IWodiLibListValidator<IStubModelSettings> validator = new MockWodiLibListValidator<IStubModelSettings>();
            FixedLengthList<StubModel, ReadOnlyStubModel, IStubModelSettings>.BuildItemFromSettingsDelegate
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
            IWodiLibListValidator<IStubModelSettings> validator = new MockWodiLibListValidator<IStubModelSettings>();
            FixedLengthList<StubModel, ReadOnlyStubModel, IStubModelSettings>.BuildItemFromSettingsDelegate
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
            var originalItems = ((IEnumerable<StubModel>)instance).Skip(index).Take(count).ToArray();

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

        /// <summary>
        ///     <para>Reset が正常に実行され、意図した結果が取得されること。</para>
        ///     <para>Validator の処理が呼ばれること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_Success()
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
