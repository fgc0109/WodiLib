using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Collections;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys.Collections
{
    [TestFixture]
    public class SimpleListTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Meta

        /// <summary>
        ///     SimpleListがObservableCollectionを継承していることを確認する。
        /// </summary>
        [Test]
        public static void BaseClassTest()
        {
            var instance = InitInstance.Generate().instance;

            Assert.IsInstanceOf<ObservableCollection<StubModel>>(instance);
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     <para>initValues が null の場合にコンストラクタが正常に終了すること。</para>
        ///     <para>初期化後の Count が 0 であること。</para>
        /// </summary>
        [Test]
        public static void ConstructorTest_Success_InitValuesNull()
        {
            SimpleListValueBuilder<StubModel> valueBuilder = new(i => new StubModel(i.ToString()));
            IEnumerable<StubModel>? initValues = null;

            constructorTestHelper.ConstructorSuccess(
                factory: () => new SimpleList<StubModel>(valueBuilder, initValues),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(instance =>
                    {
                        // 意図した初期値で初期化されること
                        Assert.AreEqual(0, instance.Count);
                    }
                )
            );
        }

        /// <summary>
        ///     <para>initValues が非 null の場合にコンストラクタが正常に終了すること。</para>
        ///     <para>初期化後の Count と内容が意図したものであること。</para>
        /// </summary>
        [Test]
        public static void ConstructorTest_Success_InitValuesNotNull()
        {
            SimpleListValueBuilder<StubModel> valueBuilder = new(i => new StubModel(i.ToString()));
            var initValues = new StubModel[] { new("0"), new("1"), new("2") };

            constructorTestHelper.ConstructorSuccess(
                factory: () => new SimpleList<StubModel>(valueBuilder, initValues),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(instance =>
                    {
                        // 意図した初期値で初期化されること
                        Assert.AreEqual(initValues.Length, instance.Count);
                        for (var i = 0; i < initValues.Length; i++)
                        {
                            Assert.AreSame(initValues[i], instance[i]);
                        }
                    }
                )
            );
        }

        /// <summary>
        ///     valueBuilder が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorTest_Failure_NullArgs()
        {
            SimpleListValueBuilder<StubModel>? valueBuilder = null;
            IEnumerable<StubModel>? initValues = null;

            constructorTestHelper.ConstructorFailure(
                factory: () => new SimpleList<StubModel>(valueBuilder!, initValues),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        #endregion

        #region Events

        #region NotifyPropertyChanged

        /// <summary>
        ///     <para>アイテムのプロパティ変更時、SimpleList のプロパティ変更通知が発生すること。</para>
        ///     <para>通知されるプロパティ名が "Item[]" であること。</para>
        ///     <para>コレクション変更通知が発生しないこと。</para>
        /// </summary>
        [Test]
        public static void SetTest_NotifyPropertyChanged_AfterItemPropertyChanged()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            var notifiedProperties = new List<string>();
            var stubModelNotifiedProperties = new List<string>();


            ((INotifyPropertyChanged)instance).PropertyChanged += (_, args) =>
            {
                notifiedProperties.Add(args.PropertyName!);
            };
            instance[0].PropertyChanged += (_, args) => { stubModelNotifiedProperties.Add(args.PropertyName!); };

            // アイテムのプロパティを変更
            var newStringValue = "DIFF VALUE";
            instance[0].StringValue = newStringValue;

            // 前提条件：stubModel がプロパティ変更通知を行っていること
            CustomAssert.AreSequenceEquals(new[] { nameof(StubModel.StringValue) }, stubModelNotifiedProperties);

            // リストのプロパティ変更通知が発生していないこと ※ WodiLib2 までは発生していたが、3以降は通知しないように
            Assert.AreEqual(0, notifiedProperties.Count);
            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
            // アイテムの値が実際に変更されていること
            Assert.AreEqual(newStringValue, instance[0].StringValue);
        }

        /// <summary>
        ///     <para>
        ///         SimpleList から除去された要素がプロパティ変更通知を行ったとき
        ///         SimpleList 自身はプロパティ変更通知を行わないこと
        ///     </para>
        ///     <para>コレクション変更通知も発生しないこと。</para>
        ///     <para>※ 前提として、SetTest_NotifyPropertyChanged_AfterItemPropertyChanged がパスしていること</para>
        /// </summary>
        [Test]
        public static void SetTest_NoNotifyPropertyChanged_AfterItemRemoved()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            var notifiedProperties = new List<string>();
            var stubModelNotifiedProperties = new List<string>();

            ((INotifyPropertyChanged)instance).PropertyChanged += (_, args) =>
            {
                notifiedProperties.Add(args.PropertyName!);
            };
            instance[0].PropertyChanged += (_, args) => { stubModelNotifiedProperties.Add(args.PropertyName!); };

            var oldItem = instance[0];
            instance[0] = new StubModel("new Value");
            // インデクサを通した編集で変更通知されるため、クリアが必要
            notifiedProperties.Clear();
            raiseCollectionChangeEventArgsList.Clear();

            // 除去されたアイテムのプロパティを変更
            oldItem.StringValue = "DIFF VALUE";

            // 前提条件：除去された要素がプロパティ変更通知を行っていること
            CustomAssert.AreSequenceEquals(new[] { nameof(StubModel.StringValue) }, stubModelNotifiedProperties);

            // SimpleList がプロパティ変更通知を行っていないこと
            Assert.AreEqual(0, notifiedProperties.Count);

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>
        ///         SimpleList に編集された要素がプロパティ変更通知を行ったとき
        ///         SimpleList 自身がプロパティ変更通知を行うこと
        ///     </para>
        ///     <para>通知されるプロパティ名が"Item[]"であること。</para>
        ///     <para>コレクション変更通知は発生しないこと。</para>
        ///     <para>※ 前提として、SetTest_NotifyPropertyChanged_AfterItemPropertyChanged がパスしていること</para>
        /// </summary>
        [Test]
        public static void SetTestNotifyPropertyChanged_AfterItemSet()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            var notifiedProperties = new List<string>();
            var stubModelNotifiedProperties = new List<string>();

            ((INotifyPropertyChanged)instance).PropertyChanged += (_, args) =>
            {
                notifiedProperties.Add(args.PropertyName!);
            };

            var newItem = new StubModel("new Value");
            newItem.PropertyChanged += (_, args) => { stubModelNotifiedProperties.Add(args.PropertyName!); };

            instance[0] = newItem;
            // インデクサを通した編集で変更通知されるため、クリアが必要
            notifiedProperties.Clear();
            raiseCollectionChangeEventArgsList.Clear();

            // 新しくセットされたアイテムのプロパティを変更
            newItem.StringValue = "DIFF VALUE";

            // 前提条件：stubModel がプロパティ変更通知を行っていること
            CustomAssert.AreSequenceEquals(new[] { nameof(StubModel.StringValue) }, stubModelNotifiedProperties);

            // リストのプロパティ変更通知が発生していないこと ※ WodiLib2 までは発生していたが、3以降は通知しないように
            Assert.AreEqual(0, notifiedProperties.Count);

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        #endregion

        #endregion

        #region Methods

        #region public

        #region Get

        /// <summary>
        ///     <para>count=0の場合、空のコレクションが取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void GetTest_Success_NoItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int index = 2;
            const int count = 0;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.Get(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 空要素が返却されること
                        Assert.AreEqual(count, resultArray.Length);
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>count=1の場合、指定したインデックスの要素が1つ取得されること。</para>
        ///     <para>取得した要素が意図したものであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void GetTest_Success_SingleItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int index = 2;
            const int count = 1;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.Get(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 選択した要素が返却されること
                        Assert.AreEqual(count, resultArray.Length);
                        Assert.AreSame(instance[index], resultArray[0]);
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>count>=2の場合、指定したインデックスから指定した数の要素が取得されること。</para>
        ///     <para>プロパティ変更通知がされないこと</para>
        ///     <para>コレクション変更通知がされないこと</para>
        /// </summary>
        [Test]
        public static void GetTest_Success_MultiItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int index = 1;
            const int count = 3;

            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                execFunc: target => target.Get(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 選択した要素が返却されること
                        Assert.AreEqual(count, resultArray.Length);
                        for (var i = 0; i < count; i++)
                        {
                            Assert.AreSame(instance[index + i], resultArray[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        #endregion

        #region Set

        /// <summary>
        ///     <para>items配列が空の場合、いずれの要素も変更されないこと。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [TestCase(0)]
        [TestCase(InitInstance.INIT_ITEMS_LENGTH - 1)]
        public static void SetTest_Success_ItemsEmpty(int index)
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            var items = Array.Empty<StubModel>();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Set(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        // 空要素が返却されること
                        Assert.AreEqual(0, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>指定したインデックスの要素が指定したインスタンスに置き換えられること</para>
        ///     <para>プロパティ変更通知がされること</para>
        ///     <para>コレクション変更通知がされること</para>
        /// </summary>
        [Test]
        public static void SetTest_Success_SingleItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int setIndex = 3;
            var newItem = new StubModel("newStubItem");

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Set(setIndex, newItem),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var actualItems = result.ToList();

                        // 設定した要素が返却されること
                        Assert.AreEqual(1, actualItems.Count);
                        Assert.AreSame(newItem, actualItems[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 指定した箇所が変更されていること
                        Assert.AreSame(newItem, target[setIndex]);
                        // その他の箇所は変更されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            if (i == setIndex) continue;
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(setIndex, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].OldItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].OldItems![0]);
            CustomAssert.AreItemEquals(
                InitInstance.MakeInitItems()[setIndex],
                (StubModel)raiseCollectionChangeEventArgsList[0].OldItems![0]!
            );
            Assert.AreEqual(setIndex, raiseCollectionChangeEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].NewItems![0]);
            Assert.AreSame(newItem, (StubModel)raiseCollectionChangeEventArgsList[0].NewItems![0]!);
        }

        /// <summary>
        ///     <para>指定したインデックスの要素が指定したインスタンスに置き換えられること</para>
        ///     <para>プロパティ変更通知がされること</para>
        ///     <para>コレクション変更通知がされること</para>
        /// </summary>
        [Test]
        public static void SetTest_Success_MultiItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = 2;
            var items = new StubModel[]
            {
                new("newStubItem"),
                new("newStubItem2"),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Set(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 戻り値がセットした配列であること
                        Assert.AreEqual(items.Length, resultArray.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 指定した範囲が変更されていること
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[index + i]);
                        }

                        // その他の箇所は変更されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            if (index <= i && i < index + items.Length) continue;
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Add

        /// <summary>
        ///     <para>いずれの要素も更新されないこと</para>
        ///     <para>戻り値が空の配列であること。</para>
        ///     <para>プロパティ変更通知がされないこと</para>
        ///     <para>コレクション変更通知がされないこと</para>
        /// </summary>
        [Test]
        public static void AddTest_Success_ItemsEmpty()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            var items = Array.Empty<StubModel>();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Add(items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 戻り値が空の配列であること
                        Assert.AreEqual(0, resultArray.Length);
                        Assert.AreSame(items, result);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>引数で与えた要素が末尾に追加されること。</para>
        ///     <para>戻り値が追加した配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Addアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void AddTest_Success_SingleItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            var item = new StubModel("newStubItem");

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Add(item),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        Assert.AreSame(item, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(SimpleList<StubModel>.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が+1されていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + 1, target.Count);
                        // 末尾に要素が追加されていること
                        Assert.AreSame(item, target[InitInstance.INIT_ITEMS_LENGTH]);
                        // 元の要素は変更されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(-1, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.IsNull(raiseCollectionChangeEventArgsList[0].OldItems!);
            Assert.AreEqual(5, raiseCollectionChangeEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].NewItems![0]);
            Assert.AreSame(item, (StubModel)raiseCollectionChangeEventArgsList[0].NewItems![0]!);
        }

        /// <summary>
        ///     <para>引数で与えた要素が末尾に追加されること。</para>
        ///     <para>戻り値が追加した配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void AddTest_Success_MultiItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            var items = new StubModel[]
            {
                new("newStubItem"),
                new("newStubItem2"),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Add(items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(items.Length, resultArray.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(SimpleList<StubModel>.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + items.Length, target.Count);
                        // 末尾に要素が追加されていること
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[InitInstance.INIT_ITEMS_LENGTH + i]);
                        }

                        // 元の要素は変更されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Insert

        /// <summary>
        ///     <para>items配列が空の場合、何も変更されないこと。</para>
        ///     <para>戻り値が空の配列であること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [TestCase(0)]
        [TestCase(InitInstance.INIT_ITEMS_LENGTH)]
        public static void InsertTest_Success_ItemsEmpty(int index)
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            var items = Array.Empty<StubModel>();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Insert(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // 戻り値が空の配列であること
                        var resultArray = result.ToArray();
                        Assert.AreEqual(0, resultArray.Length);
                        Assert.AreSame(items, result);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>指定位置に要素が挿入されること。</para>
        ///     <para>戻り値が挿入した配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Addアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void InsertTest_Success_SingleItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = 3;
            var newItem = new StubModel("newStubItem");

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Insert(index, newItem),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        Assert.AreSame(newItem, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(SimpleList<StubModel>.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が+1されていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + 1, target.Count);
                        // 挿入した要素が指定位置に格納されていること
                        // 更新していない要素がそのままであること
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[0], target[0]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[1], target[1]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[2], target[2]);
                        Assert.AreSame(newItem, target[3]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[3], target[4]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[4], target[5]);
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(-1, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.IsNull(raiseCollectionChangeEventArgsList[0].OldItems!);
            Assert.AreEqual(index, raiseCollectionChangeEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].NewItems![0]);
            Assert.AreSame(newItem, (StubModel)raiseCollectionChangeEventArgsList[0].NewItems![0]!);
        }

        /// <summary>
        ///     <para>指定したインデックスに要素が挿入されること</para>
        ///     <para>プロパティ変更通知がされること</para>
        ///     <para>コレクション変更通知がされること</para>
        /// </summary>
        [Test]
        public static void InsertTest_Success_MultiItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int setIndex = 2;
            var items = new StubModel[]
            {
                new("newStubItem"),
                new("newStubItem2"),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Insert(setIndex, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(items.Length, resultArray.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が+2されていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + 2, target.Count);
                        // 更新した要素が設定したインスタンスに置換されていること
                        // 更新していない要素がそのままであること
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[0], target[0]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[1], target[1]);
                        Assert.AreSame(items[0], target[2]);
                        Assert.AreSame(items[1], target[3]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[2], target[4]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[3], target[5]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[4], target[6]);
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Overwrite

        /// <summary>
        ///     <para>いずれの要素も更新されないこと</para>
        ///     <para>プロパティ変更通知がされないこと</para>
        ///     <para>コレクション変更通知がされないこと</para>
        /// </summary>
        [TestCase(0)]
        [TestCase(InitInstance.INIT_ITEMS_LENGTH)]
        public static void OverwriteTest_Success_ItemsEmpty(int index)
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            var items = Array.Empty<StubModel>();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        // 空要素が返却されること
                        Assert.AreEqual(0, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>items配列が1要素でindex&lt;Countの場合、指定位置の要素が置換されること。</para>
        ///     <para>戻り値が上書きした配列であること。</para>
        ///     <para>"Item[]"プロパティ変更通知が1回発生すること。</para>
        ///     <para>Replaceアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void OverwriteTest_Success_SingleItem_Replace()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = 3;
            var item = new StubModel("newStubItem");

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, item),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        Assert.AreSame(item, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        // 指定位置の要素が置換されていること
                        Assert.AreSame(item, target[index]);
                        // その他の要素は変更されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            if (i == index) continue;
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(index, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].OldItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].OldItems![0]);
            CustomAssert.AreItemEquals(
                InitInstance.MakeInitItems()[index],
                (StubModel)raiseCollectionChangeEventArgsList[0].OldItems![0]!
            );
            Assert.AreEqual(index, raiseCollectionChangeEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].NewItems![0]);
            Assert.AreSame(item, (StubModel)raiseCollectionChangeEventArgsList[0].NewItems![0]!);
        }

        /// <summary>
        ///     <para>items配列が1要素でindex==Countの場合、末尾に要素が追加されること。</para>
        ///     <para>戻り値が上書きした配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Addアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void OverwriteTest_Success_SingleItem_Add()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = InitInstance.INIT_ITEMS_LENGTH;
            var item = new StubModel("newStubItem");

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, item),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        Assert.AreSame(resultArray[0], item);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(SimpleList<StubModel>.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が+1されていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + 1, target.Count);
                        // 末尾に要素が追加されていること
                        Assert.AreSame(item, target[index]);
                        // 元の要素は変更されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(-1, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.IsNull(raiseCollectionChangeEventArgsList[0].OldItems!);
            Assert.AreEqual(index, raiseCollectionChangeEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].NewItems!.Count);
            Assert.IsInstanceOf<StubModel>(raiseCollectionChangeEventArgsList[0].NewItems![0]);
            Assert.AreSame(item, (StubModel)raiseCollectionChangeEventArgsList[0].NewItems![0]!);
        }

        /// <summary>
        ///     <para>items配列が2要素で全て既存要素の置換の場合、指定位置から2要素が置換されること。</para>
        ///     <para>戻り値が上書きした配列であること。</para>
        ///     <para>"Item[]"プロパティ変更通知が1回発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void OverwriteTest_Success_MultiItem_Replace()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = 2;
            var items = new StubModel[]
            {
                new("newStubItem"),
                new("newStubItem2"),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること;
                        Assert.AreEqual(items.Length, resultArray.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        // 指定位置の要素が置換されていること
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[index + i]);
                        }

                        // その他の要素は変更されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            if (index <= i && i < index + items.Length) continue;
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>items配列が2要素で全て末尾への追加の場合、末尾に2要素が追加されること。</para>
        ///     <para>戻り値が上書きした配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void OverwriteTest_Success_MultiItem_Add()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = InitInstance.INIT_ITEMS_LENGTH;
            var items = new StubModel[]
            {
                new("newStubItem"),
                new("newStubItem2"),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること
                        Assert.AreEqual(resultArray.Length, items.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(SimpleList<StubModel>.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + items.Length, target.Count);
                        // 末尾に要素が追加されていること
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[index + i]);
                        }

                        // 元の要素は変更されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>items配列が2要素以上で一部置換・一部追加の場合、指定位置から置換と末尾への追加が行われること。</para>
        ///     <para>戻り値が上書きした配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void OverwriteTest_Success_MultiItem_ReplaceAndAdd()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            const int index = InitInstance.INIT_ITEMS_LENGTH - 1;
            var items = new StubModel[]
            {
                new("newStubItem"), // 置換される要素
                new("newStubItem2"), // 追加される要素
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Overwrite(index, items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 設定した要素が返却されること.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(SimpleList<StubModel>.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + 1, target.Count);
                        // 指定位置から要素が上書き/追加されていること
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[index + i]);
                        }

                        // それより前の要素は変更されていないこと
                        for (var i = 0; i < index; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Move

        /// <summary>
        ///     <para>移動前後のインデックスが同じ場合、何も変更されないこと。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void MoveTest_Success_NoMove_SameOldNewIndex()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int oldIndex = 1;
            const int newIndex = oldIndex;
            const int count = 1;

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Move(oldIndex, newIndex, count),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>count=0の場合、何も変更されないこと。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void MoveTest_Success_NoMove_CountZero()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int oldIndex = 1;
            const int newIndex = 2;
            const int count = 0;

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Move(oldIndex, newIndex, count),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>count=1の場合、指定した要素が正しく移動されること。</para>
        ///     <para>"Item[]"プロパティ変更通知が1回発生すること。</para>
        ///     <para>Moveアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void MoveTest_Success_SingleMove()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int oldIndex = 2;
            const int newIndex = 4;
            const int count = 1;

            var movedItem = instance[oldIndex];

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Move(oldIndex, newIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が変わらないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        // 要素が正しく移動されていること
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[0], target[0]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[1], target[1]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[3], target[2]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[4], target[3]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[2], target[4]);
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Move, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(oldIndex, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.AreEqual(newIndex, raiseCollectionChangeEventArgsList[0].NewStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].OldItems!.Count);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].NewItems!.Count);
            Assert.AreSame(movedItem, raiseCollectionChangeEventArgsList[0].OldItems![0]);
            Assert.AreSame(movedItem, raiseCollectionChangeEventArgsList[0].NewItems![0]);
        }

        /// <summary>
        ///     <para>count=2の場合、指定した範囲の要素が正しく移動されること。</para>
        ///     <para>"Item[]"プロパティ変更通知が1回発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void MoveTest_Success_MultiMove()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int oldIndex = 3;
            const int newIndex = 0;
            const int count = 2;

            impureActionTestHelper.ImpureActionSuccess(
                instance,
                execAction: target => target.Move(oldIndex, newIndex, count),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が変わらないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        // 移動した要素が正しい位置にあること
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[3], target[0]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[4], target[1]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[0], target[2]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[1], target[3]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[2], target[4]);
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Remove

        /// <summary>
        ///     <para>count=0の場合、何も削除されないこと。</para>
        ///     <para>戻り値が空のコレクションであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void RemoveTest_Success_NoRemove()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int index = 2;
            const int count = 0;

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Remove(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        // 空要素が返却されること
                        var resultArray = result.ToArray();
                        Assert.AreEqual(count, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>count=1の場合、指定した要素が削除されること。</para>
        ///     <para>戻り値が削除された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Removeアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void RemoveTest_Success_SingleItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int index = 2;
            const int count = 1;

            var removedItem = instance[index];

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Remove(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 除去した要素が返却されること
                        Assert.AreEqual(count, resultArray.Length);
                        Assert.AreSame(removedItem, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が減っていること
                        Assert.AreEqual(InitInstance.InitLength - 1, target.Count);
                        // 要素が正しく削除されていること
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[0], target[0]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[1], target[1]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[3], target[2]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[4], target[3]);
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, raiseCollectionChangeEventArgsList[0].Action);
            Assert.AreEqual(index, raiseCollectionChangeEventArgsList[0].OldStartingIndex);
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList[0].OldItems!.Count);
            Assert.AreSame(removedItem, raiseCollectionChangeEventArgsList[0].OldItems![0]);
        }

        /// <summary>
        ///     <para>count=2の場合、指定した範囲の要素が削除されること。</para>
        ///     <para>戻り値が削除された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void RemoveTest_MultiItem()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int index = 2;
            const int count = 2;

            var removedItems = instance.Skip(index).Take(count).ToArray();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Remove(index, count),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 除去した要素が返却されること
                        Assert.AreEqual(count, resultArray.Length);
                        for (var i = 0; i < count; i++)
                        {
                            Assert.AreSame(removedItems[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が減っていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH - count, target.Count);
                        // 要素が正しく削除されていること
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[0], target[0]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[1], target[1]);
                        CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[4], target[2]);
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Adjust

        /// <summary>
        ///     <para>現在のCountと同じlengthを指定した場合、何も変更されないこと。</para>
        ///     <para>戻り値が空のコレクションであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void AdjustTest_Success_NoAddAndRemove()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH;

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Adjust(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 空要素が返却されること
                        Assert.AreEqual(0, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>現在のCountより1大きいlengthを指定した場合、要素が追加されること。</para>
        ///     <para>戻り値が追加された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>コレクション変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustTest_Success_AddSingle()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH + 1;

            var expectedAddItem = InitInstance.GenerateTestModel(InitInstance.INIT_ITEMS_LENGTH);

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Adjust(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();
                        // 追加された要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        CustomAssert.AreItemEquals(expectedAddItem, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }

                        // 意図した要素が追加されていること
                        CustomAssert.AreItemEquals(expectedAddItem, target[5]);
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>現在のCountより以上大きいlengthを指定した場合、要素が追加されること。</para>
        ///     <para>戻り値が追加された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>コレクション変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustTest_Success_AddMulti()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH + 2;

            var expectedAddItem = new[]
            {
                InitInstance.GenerateTestModel(InitInstance.INIT_ITEMS_LENGTH),
                InitInstance.GenerateTestModel(InitInstance.INIT_ITEMS_LENGTH + 1),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Adjust(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 追加された要素が返却されること
                        Assert.AreEqual(2, resultArray.Length);
                        for (var i = 0; i < resultArray.Length; i++)
                        {
                            CustomAssert.AreItemEquals(expectedAddItem[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        var i = 0;
                        for (; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }

                        // 意図した要素が追加されていること
                        for (var j = 0; i < target.Count; i++, j++)
                        {
                            CustomAssert.AreItemEquals(expectedAddItem[j], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>現在のCountより1小さいlengthを指定した場合、1要素が削除されること。</para>
        ///     <para>戻り値が削除された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>コレクション変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustTest_Success_RemoveSingle()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH - 1;

            var removedItem = instance.Last();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Adjust(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 除去された要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        Assert.AreSame(removedItem, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が減っていること
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>現在のCountより2以上小さいlengthを指定した場合、要素が削除されること。</para>
        ///     <para>戻り値が削除された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>コレクション変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustTest_Success_RemoveMulti()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH - 2;

            var removedItems = instance.Skip(length).Take(2).ToArray();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Adjust(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 除去された要素が返却されること
                        Assert.AreEqual(2, resultArray.Length);
                        for (var i = 0; i < resultArray.Length; i++)
                        {
                            Assert.AreSame(removedItems[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が減っていること
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region AdjustIfLong

        /// <summary>
        ///     <para>現在のCountより大きいlengthを指定した場合、何も変更されないこと。</para>
        ///     <para>戻り値が空のコレクションであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void AdjustIfLongTest_Success_HigherLength()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH + 1;

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfLong(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 空要素が返却されること
                        Assert.AreEqual(resultArray.Length, 0);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>現在のCountと同じlengthを指定した場合、何も変更されないこと。</para>
        ///     <para>戻り値が空のコレクションであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void AdjustIfLongTest_Success_SameLength()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH;

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfLong(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 空要素が返却されること
                        Assert.AreEqual(0, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>現在のCountより1小さいlengthを指定した場合、1要素が削除されること。</para>
        ///     <para>戻り値が削除された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustIfLongTest_Success_LowerLength_RemoveSingle()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH - 1;

            var removedItem = instance.Last();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfLong(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 除去された要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        Assert.AreSame(removedItem, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が減っていること
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>現在のCountより2小さいlengthを指定した場合、2要素が削除されること。</para>
        ///     <para>戻り値が削除された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>コレクション変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustIfLongTest_Success_LowerLength_RemoveMulti()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH - 2;

            var removedItems = instance.Skip(length).Take(2).ToArray();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfLong(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 除去された要素が返却されること
                        Assert.AreEqual(2, resultArray.Length);
                        for (var i = 0; i < resultArray.Length; i++)
                        {
                            Assert.AreSame(removedItems[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region AdjustIfShort

        /// <summary>
        ///     <para>現在のCountより小さいlengthを指定した場合、何も変更されないこと。</para>
        ///     <para>戻り値が空のコレクションであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void AdjustIfShortTest_Success_LowerLength()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH - 1;

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfShort(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 空要素が返却されること
                        Assert.AreEqual(0, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>現在のCountと同じlengthを指定した場合、何も変更されないこと。</para>
        ///     <para>戻り値が空のコレクションであること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void AdjustIfShortTest_Success_SameLength()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH;

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfShort(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 空要素が返却されること
                        Assert.AreEqual(0, resultArray.Length);
                    }
                ),
                expectedNotifyProperties: Array.Empty<string>(),
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // いずれの要素も変化していないこと
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH, target.Count);
                        for (var i = 0; i < target.Count; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>現在のCountより1大きいlengthを指定した場合、1要素が追加されること。</para>
        ///     <para>戻り値が追加された要素を含むコレクションであること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>コレクション変更通知が発生すること。</para>
        /// </summary>
        [Test]
        public static void AdjustIfShortTest_Success_HigherLength_AddSingle()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH + 1;

            var expectedAddItem = InitInstance.GenerateTestModel(InitInstance.INIT_ITEMS_LENGTH);

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfShort(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 追加された要素が返却されること
                        Assert.AreEqual(1, resultArray.Length);
                        CustomAssert.AreItemEquals(expectedAddItem, resultArray[0]);
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(length, target.Count);
                        // 意図しない範囲の値が更新されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }

                        // 意図した要素が追加されていること
                        CustomAssert.AreItemEquals(expectedAddItem, target[5]);
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>指定した長さに不足している数だけ要素が追加されること</para>
        ///     <para>プロパティ変更通知がされること</para>
        ///     <para>コレクション変更通知がされること</para>
        /// </summary>
        [Test]
        public static void AdjustIfShortTest_Success_HigherLength_AddMulti()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH + 2;

            var expectedAddItems = new[]
            {
                InitInstance.GenerateTestModel(InitInstance.INIT_ITEMS_LENGTH),
                InitInstance.GenerateTestModel(InitInstance.INIT_ITEMS_LENGTH + 1),
            };
            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.AdjustIfShort(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 追加された要素が返却されること
                        Assert.AreEqual(2, resultArray.Length);
                        for (var i = 0; i < resultArray.Length; i++)
                        {
                            CustomAssert.AreItemEquals(expectedAddItems[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素数が増えていること
                        Assert.AreEqual(InitInstance.INIT_ITEMS_LENGTH + expectedAddItems.Length, target.Count);
                        // 末尾に要素が追加されていること
                        for (var i = 0; i < expectedAddItems.Length; i++)
                        {
                            CustomAssert.AreItemEquals(target[InitInstance.INIT_ITEMS_LENGTH + i], expectedAddItems[i]);
                        }

                        // 元の要素は変更されていないこと
                        for (var i = 0; i < InitInstance.INIT_ITEMS_LENGTH; i++)
                        {
                            CustomAssert.AreItemEquals(InitInstance.MakeInitItems()[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Reset

        #region Items

        /// <summary>
        ///     <para>現在のCountと異なる数の要素でリセットした場合、要素が正しく置換されること。</para>
        ///     <para>戻り値がリセットした配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_Items_Success_SizeChange()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            var items = new[]
            {
                new StubModel("Reset Item0"),
                new StubModel("Reset Item1"),
            };

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 引数で与えた要素が返却されること
                        Assert.AreEqual(items.Length, resultArray.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素がリセットされていること
                        Assert.AreEqual(items.Length, target.Count);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>現在のCountと同じ数の要素でリセットした場合、要素が正しく置換されること。</para>
        ///     <para>戻り値がリセットした配列であること。</para>
        ///     <para>"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_Items_Success_SizeNotChange()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            var items = InitInstance.InitLength.Iterate(i => InitInstance.GenerateTestModel(i + 100)).ToArray();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(items),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 引数で与えた要素が返却されること
                        Assert.AreEqual(items.Length, resultArray.Length);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素がリセットされていること
                        Assert.AreEqual(items.Length, target.Count);
                        for (var i = 0; i < items.Length; i++)
                        {
                            Assert.AreSame(items[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #region Length

        /// <summary>
        ///     <para>現在のCountと異なる数の要素でリセットした場合、要素が正しく置換されること。</para>
        ///     <para>戻り値がリセットした配列であること。</para>
        ///     <para>"Count"と"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_Length_Success_SizeChange()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH + 1;

            var expectedArray = length.Iterate(InitInstance.GenerateTestModel).ToArray();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 引数で与えた要素が返却されること
                        Assert.AreEqual(length, resultArray.Length);
                        for (var i = 0; i < length; i++)
                        {
                            CustomAssert.AreItemEquals(expectedArray[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    nameof(instance.Count),
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素がリセットされていること
                        Assert.AreEqual(expectedArray.Length, target.Count);
                        for (var i = 0; i < expectedArray.Length; i++)
                        {
                            CustomAssert.AreItemEquals(expectedArray[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        /// <summary>
        ///     <para>現在のCountと同じ数の要素でリセットした場合、要素が正しく置換されること。</para>
        ///     <para>戻り値がリセットした配列であること。</para>
        ///     <para>"Item[]"プロパティ変更通知が発生すること。</para>
        ///     <para>Resetアクションのコレクション変更通知が1回発生すること。</para>
        /// </summary>
        [Test]
        public static void ResetTest_Length_Success_SizeNotChange()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            const int length = InitInstance.INIT_ITEMS_LENGTH;

            var expectedArray = length.Iterate(InitInstance.GenerateTestModel).ToArray();

            impureFunctionTestHelper.ImpureFuncSuccess(
                instance,
                execFunc: target => target.Reset(length),
                resultValueVerifier: new ValueVerifier<IEnumerable<StubModel>>(result =>
                    {
                        var resultArray = result.ToArray();

                        // 引数で与えた要素が返却されること
                        Assert.AreEqual(expectedArray.Length, resultArray.Length);
                        for (var i = 0; i < expectedArray.Length; i++)
                        {
                            CustomAssert.AreItemEquals(expectedArray[i], resultArray[i]);
                        }
                    }
                ),
                expectedNotifyProperties: new[]
                {
                    ListConstant.IndexerName,
                },
                instanceVerifier: new ValueVerifier<SimpleList<StubModel>>(target =>
                    {
                        // 要素がリセットされていること
                        Assert.AreEqual(expectedArray.Length, target.Count);
                        for (var i = 0; i < expectedArray.Length; i++)
                        {
                            CustomAssert.AreItemEquals(expectedArray[i], target[i]);
                        }
                    }
                )
            );

            // 意図したコレクション変更通知が発生していること
            Assert.AreEqual(1, raiseCollectionChangeEventArgsList.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, raiseCollectionChangeEventArgsList[0].Action);
        }

        #endregion

        #endregion

        #region ItemEquals_ISimpleList

        /// <summary>
        ///     <para>同じオブジェクトとの比較でTrueが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_ISimpleList_True_SameObject()
        {
            var (left, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            ISimpleList<StubModel> right = left;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>内容が同じ別のオブジェクトとの比較でTrueが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Props_True_EqualityObject()
        {
            var (left, raiseCollectionChangeEventArgsList1) = InitInstance.Generate();
            var (right, raiseCollectionChangeEventArgsList2) = InitInstance.Generate(
                initItems: InitInstance.MakeInitItems().Select(item => item.DeepClone()).ToArray()
            );

            // 前提条件：left と right の各要素が同一値であり、同一インスタンスでないこと
            Assert.IsTrue(
                left.ToList().SequenceEqual(right.ToList(), (l, r) => l.ItemEquals(r) && !ReferenceEquals(l, r))
            );

            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList1.Count);
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList2.Count);
        }

        /// <summary>
        ///     <para>nullとの比較でFalseが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Props_False_NullObject()
        {
            var (left, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            ISimpleList<StubModel>? right = null;

            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        #endregion

        #region ItemEquals_Object

        /// <summary>
        ///     <para>同じオブジェクトとの比較でTrueが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Object_True_SameObject()
        {
            var (left, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            object right = left;
            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: true
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>内容が同じ別のオブジェクトとの比較でTrueが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Object_True_EqualityObject()
        {
            var (left, raiseCollectionChangeEventArgsList1) = InitInstance.Generate();
            var (right, raiseCollectionChangeEventArgsList2) = InitInstance.Generate(
                initItems: InitInstance.MakeInitItems().Select(item => item.DeepClone()).ToArray()
            );
            object objRight = right;

            // 前提条件：left と right の各要素が同一値であり、同一インスタンスでないこと
            Assert.IsTrue(
                left.ToList().SequenceEqual(right.ToList(), (l, r) => l.ItemEquals(r) && !ReferenceEquals(l, r))
            );

            itemEqualsTestHelper.ItemEquals(
                left,
                objRight,
                expected: true
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList1.Count);
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList2.Count);
        }

        /// <summary>
        ///     <para>nullとの比較でFalseが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Object_False_NullObject()
        {
            var (left, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            object? right = null;

            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        /// <summary>
        ///     <para>無関係なオブジェクトとの比較でFalseが返されること。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void ItemEqualsTest_Object_False_IrrelevantObject()
        {
            var (left, raiseCollectionChangeEventArgsList) = InitInstance.Generate();
            object right = new StubModel("x");

            itemEqualsTestHelper.ItemEquals(
                left,
                right,
                expected: false
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        #endregion

        #region DeepClone

        /// <summary>
        ///     <para>DeepCloneが正常に実行され、別のオブジェクトが作成されること。</para>
        ///     <para>クローンされたオブジェクトの内容が元のオブジェクトと等しいこと。</para>
        ///     <para>プロパティ変更通知がされないこと。</para>
        ///     <para>コレクション変更通知がされないこと。</para>
        /// </summary>
        [Test]
        public static void DeepCloneTest()
        {
            var (instance, raiseCollectionChangeEventArgsList) = InitInstance.Generate();

            deepCloneTestHelper.DeepClone(
                instance,
                resultValueVerifier: new ValueVerifier<SimpleList<StubModel>>(cloned =>
                    {
                        // 別のオブジェクトであること
                        Assert.AreNotSame(instance, cloned);
                        // 内容が等しいこと
                        CustomAssert.AreItemEquals(cloned, instance);
                        // 要素数が等しいこと
                        Assert.AreEqual(instance.Count, cloned.Count);
                        // 各要素が等しいこと（参照は異なってもよい）
                        for (var i = 0; i < instance.Count; i++)
                        {
                            CustomAssert.AreItemEquals(cloned[i], instance[i]);
                        }
                    }
                )
            );

            // コレクション変更通知が発生しないこと
            Assert.AreEqual(0, raiseCollectionChangeEventArgsList.Count);
        }

        #endregion

        #endregion

        #endregion

        #region For Test

        private static class InitInstance
        {
            public const int INIT_ITEMS_LENGTH = 5;

            public static StubModel[] MakeInitItems() => new StubModel[]
            {
                new("InitStr"),
                new("\t_"),
                new("初期文字列"),
                new("Init String"),
                new("string123"),
            };

            public static SimpleListValueBuilder<StubModel> ValueBuilder { get; }
                = new(GenerateTestModel);

            public static int InitLength => INIT_ITEMS_LENGTH;

            public static (SimpleList<StubModel> instance, List<NotifyCollectionChangedEventArgs>
                raiseCollectionChangeEventArgsList) Generate(StubModel[]? initItems = null)
            {
                var raiseCollectionChangeEventArgsList = new List<NotifyCollectionChangedEventArgs>();
                var instance = new SimpleList<StubModel>(ValueBuilder, initItems ?? MakeInitItems());
                instance.CollectionChanged += (_, args) => raiseCollectionChangeEventArgsList.Add(args);
                return (instance, raiseCollectionChangeEventArgsList);
            }

            public static StubModel GenerateTestModel(int index) => new($"{index}");
        }

        #endregion
    }
}
