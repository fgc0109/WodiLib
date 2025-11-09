// ========================================
// Project Name : WodiLib.Test
// File Name    : StubModel.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Domain.Collection.Attributes;
using WodiLib.SourceGenerator.Domain.Model.Attributes;
using WodiLib.Sys;
using WodiLib.Sys.Collections;

namespace WodiLib.Test.Tools
{
    /*
     * テスト用のリストクラス（容量制限あり）
     * WodiLib内で定義するリストクラスの実装サンプルも兼ねる。
     *
     * いくつかの属性を使用し、
     * SourceGenerator で自動生成する。
     *
     * 生成されたソースは Generated/{名前空間}/{クラス名}.cs に出力される。
     *
     * 基本的な解説は StubModel.cs を参照。
     */

    /*
     * IStubRestrictedCapacityListSettings は SourceGenerator で自動生成されるため、
     * 手作業での作成不要。
     *
     * 以下、自動生成される設定DTOインタフェースと設定DTOクラス
     */
    /*
        /// <summary>
        ///     <see cref="StubRestrictedCapacityList"/> スタブ用設定インタフェース
        /// </summary>
        public partial interface IStubRestrictedCapacityListSettings : WodiLib.Sys.IEqualityComparable<IStubRestrictedCapacityListSettings>, IListSettings<IStubModelSettings>
        {
            /// <inheritdoc cref="StubRestrictedCapacityList.Tags" />
            System.Collections.Generic.IReadOnlyList<System.String> Tags { get; }
        }

        /// <summary>
        ///     <see cref="StubRestrictedCapacityList"/> スタブ用設定DTO
        /// </summary>
        public partial record StubRestrictedCapacityListSettings(IList<IStubModelSettings> Settings) : IStubRestrictedCapacityListSettings
        {
            /// <inheritdoc cref="IStubRestrictedCapacityListSettings.Tags" path="summary|remarks" />
            public System.Collections.Generic.IReadOnlyList<System.String> Tags { get; set; } = new List<string>();

            /// <inheritdoc/>
            public bool ItemEquals(IStubRestrictedCapacityListSettings? other)
            {
                return other is not null
                       && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
                       && Tags.SequenceEqual(other.Tags);
            }

            /// <inheritdoc/>
            public bool ItemEquals(object? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
        }
     */

    [RestrictedCapacityListImplementTemplate(
        Description = "<see cref=\"StubRestrictedCapacityList\"/> スタブ用",
        ElementType = typeof(StubModel),
        ReadOnlyElementType = typeof(ReadOnlyStubModel),
        SettingsType = typeof(IStubModelSettings),
        MaxCapacity = 10,
        MinCapacity = 1
    )]
    public partial class StubRestrictedCapacityList
        /*
         * 以下8つの基底クラス・インタフェースは SourceGenerator が自動的に付与する。
         */
        // ModelBase,
        // IStubRestrictedCapacityListSettings,
        // IEnumerable<StubModel>,
        // INotifyCollectionChanged,
        // IEqualityComparable<StubRestrictedCapacityList>,
        // IEqualityComparable<FixedStubRestrictedCapacityList>,
        // IEqualityComparable<ReadOnlyStubRestrictedCapacityList>,
        // IDeepCloneable<StubRestrictedCapacityList>
    {
        /// <summary>
        ///     タグ
        /// </summary>
        [SettingsProperty(
            ReturnType = typeof(IReadOnlyList<string>),
            DefaultValue = "new List<string>()"
        )]
        /*
         * FixedLengthListProperty 属性を付与することで、
         * 容量固定リストに実装するプロパティの対象とする。
         *
         * なお、ここではサンプルなので付与しているが、
         * 本来は FixedLengthList の setter アクセシビリティが変わらず
         * 公開する型も変わっていないため不要。
         */
        [FixedLengthListProperty(
            Accessibility = "public", // デフォルト値が "public" のため、この指定はなくても良い
            SetterAccessibility = "NONE", // FixedList で Setter を設けない
            ReturnType = typeof(List<string>) // 返却型が同じ場合、指定しなくてもいい
        )]
        [ImmutableProperty(
            Accessibility = "public",
            ReturnType = typeof(IReadOnlyList<string>)
        )]
        public List<string> Tags { get; } = new();

        public StubRestrictedCapacityList(IStubRestrictedCapacityListSettings settings)
            : this(settings, BuildSimpleList(settings.Settings), BuildItemFromSettings)
        {
            Tags = settings.Tags.ToList();
        }

        public StubRestrictedCapacityList(
            int length
        ) : this(
            new StubRestrictedCapacityListSettings(
                length.Iterate<IStubModelSettings>(i => new StubModel(i.ToString())).ToList()
            )
        )
        {
        }

        [FixedLengthListMethod]
        [ImmutableMethod]
        public string ToJsonString()
        {
            // メソッド定義が参照できることのテストをするためだけのメソッドなので、
            // 戻り値は適当な値とする
            return "JSON RESULT";
        }

        /// <summary>
        ///     StringValueに現在の日時文字列をセットする
        /// </summary>
        /*
         * FixedLengthListMethod 属性を付与すると、
         * 読取専用モデルにも同じメソッドが定義される。
         * MutableMethod 属性ではないことに注意。
         *
         * メソッドの実装は読取専用クラスのメソッドへの転送となる。
         */
        [FixedLengthListMethod(
            Accessibility = "public" // デフォルト値が "public" のため、この指定はなくても良い
        )]
        public void SetNowStringValue()
        {
            Items.ForEach(item => item.SetNowStringValue());
        }

        public bool ItemEquals(IStubFixedLengthListSettings? other)
        {
            return other is not null
                   && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
                   && Tags.SequenceEqual(other.Tags);
        }

        public bool ItemEquals(IStubRestrictedCapacityListSettings? other)
        {
            return other is not null
                   && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
                   && Tags.SequenceEqual(other.Tags);
        }

        /*
         * 必要に応じて RequiredConstructor に渡す SimpleList を作成するメソッドを定義する。
         */
        private static SimpleList<StubModel> BuildSimpleList(IEnumerable<IStubModelSettings> settings)
        {
            return new SimpleList<StubModel>(
                ElementBuilder,
                settings.Select(setting => new StubModel(setting))
            );
        }

        private static SimpleListValueBuilder<StubModel> ElementBuilder { get; }
            = new(BuildItemFromIndex);

        private static StubModel BuildItemFromIndex(int index)
            => new(index.ToString());

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private static StubModel BuildItemFromSettings(int index, IStubModelSettings settings)
        {
            return new StubModel(settings);
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private RestrictedCapacityListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings> BuildValidator(
            IStubRestrictedCapacityListSettings _,
            SimpleList<StubModel> itemImpl
        )
        {
            return new RestrictedCapacityListValidator<IStubRestrictedCapacityListSettings, IStubModelSettings>(
                countGetter: () => itemImpl.Count,
                minCapacityGetter: GetMinCapacity,
                maxCapacityGetter: GetMaxCapacity
            );
        }
    }

    /*
     * 以下は SourceGeneratorで生成されるクラス定義のサンプル。
     */
    /*
    /// <summary>
    ///     <see cref="StubRestrictedCapacityList"/> スタブ用
    /// </summary>
    public partial class StubRestrictedCapacityList : ModelBase,
        IStubRestrictedCapacityListSettings,
        IEnumerable<WodiLib.Test.Tools.StubModel>,
        INotifyCollectionChanged,
        IEqualityComparable<StubRestrictedCapacityList>,
        IEqualityComparable<FixedStubRestrictedCapacityList>,
        IEqualityComparable<ReadOnlyStubRestrictedCapacityList>,
        IDeepCloneable<StubRestrictedCapacityList>
    {
        /// <summary>容量最大値</summary>
        public static int MaxCapacity => 10;
        /// <summary>容量最小値</summary>
        public static int MinCapacity => 1;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        /// <summary>インデクサによるアクセス</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public WodiLib.Test.Tools.StubModel this[int index]
        {
            [Pure]
            get => Get(index);
            set => Set(index, value);
        }

        /// <summary>要素数</summary>
        public int Count => Items.Count;

        /// <inheritdoc/>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public IList<IStubModelSettings> Settings => Items.Cast<IStubModelSettings>().ToList();

        private protected ExtendedList<IStubRestrictedCapacityListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings> Items { get; }

        private StubRestrictedCapacityList(
            IStubRestrictedCapacityListSettings settings,
            SimpleList<WodiLib.Test.Tools.StubModel> itemsImpl,
            Func<int, IStubModelSettings, WodiLib.Test.Tools.StubModel> itemBuilder
        )
        {
            var validator = BuildValidator(settings, itemsImpl);
            validator?.Constructor((nameof(settings), settings));
            Items = new ExtendedList<IStubRestrictedCapacityListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings>(
                itemsImpl,
                minCapacity: MaxCapacity,
                maxCapacity: MinCapacity,
                validator,
                buildItemFromSettings: (index, modelSettings) => itemBuilder(index, modelSettings)
            );
            PropagatePropertyChangeEvent(Items);
            PropagateCollectionChangeEvent(Items);
        }

        /// <summary>容量最大値を取得する。</summary>
        /// <returns>容量最大値</returns>
        [Pure]
        public int GetMaxCapacity() => MaxCapacity;
        /// <summary>容量最小値を取得する。</summary>
        /// <returns>容量最小値</returns>
        [Pure]
        public int GetMinCapacity() => MinCapacity;

        /// <inheritdoc/>
        [Pure]
        public IEnumerator<WodiLib.Test.Tools.StubModel> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>指定インデックスの要素を取得する。</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        [Pure]
        public WodiLib.Test.Tools.StubModel Get(int index) => Items.Get(index);

        /// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubModel> GetRange(int index, int count) => Items.GetRange(index, count);

        /// <summary>リストの要素を更新する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
        public WodiLib.Test.Tools.StubModel Set(int index, IStubModelSettings settings) => Items.Set(index, settings);

        /// <summary>リストの連続した要素を更新する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRange(index, settings);

        /// <summary>リストの末尾に要素を追加する。</summary>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。</exception>
        public WodiLib.Test.Tools.StubModel Add(IStubModelSettings settings) => Items.Add(settings);

        /// <summary>リストの末尾に要素を追加する。</summary>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AddRange(IEnumerable<IStubModelSettings> settings) => Items.AddRange(settings);

        /// <summary>指定したインデックスの位置に要素を挿入する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] インデックス</param>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。</exception>
        public WodiLib.Test.Tools.StubModel Insert(int index, IStubModelSettings settings) => Items.Insert(index, settings);

        /// <summary>指定したインデックスの位置に要素を挿入する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] インデックス</param>
        /// <param name="settings">追加する要素</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> InsertRange(int index, IEnumerable<IStubModelSettings> settings) => Items.InsertRange(index, settings);

        /// <summary>指定したインデックスを起点として、要素の上書き/追加を行う。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] インデックス</param>
        /// <param name="settings">上書き/追加リスト</param>
        /// <returns>上書きした要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMaxCapacity"/> を上回る場合。</exception>
        /// <example><code>var target = new List&lt;int&gt; { 0, 1, 2, 3 };var dst = new List&lt;int&gt; { 10, 11, 12 };target.Overwrite(2, dst);// target is { 0, 1, 10, 11, 12 }</code><code>var target = new List&lt;int&gt; { 0, 1, 2, 3 };var dst = new List&lt;int&gt; { 10 };target.Overwrite(2, dst);// target is { 0, 1, 10, 3 }</code></example>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Overwrite(int index, IEnumerable<IStubModelSettings> settings) => Items.Overwrite(index, settings);

        /// <summary>指定したインデックスにある項目をコレクション内の新しい場所へ移動する。</summary>
        /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。</exception>
        public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);

        /// <summary>指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。</summary>
        /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)]移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)]移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)]移動させる要素数</param>
        /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, count);

        /// <summary>指定したインデックスの要素を削除する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
        /// <returns>削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。</exception>
        public WodiLib.Test.Tools.StubModel Remove(int index) => Items.Remove(index);

        /// <summary>要素の範囲を削除する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] 削除する要素数</param>
        /// <returns>削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合。</exception>
        /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="GetMinCapacity"/> を下回る場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> RemoveRange(int index, int count) => Items.RemoveRange(index, count);

        /// <summary>要素数を指定の数に合わせる。</summary>
        /// <param name="length">[Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]調整する要素数</param>
        /// <returns>追加または削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> が指定範囲外の場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLength(int length) => Items.AdjustLength(length);

        /// <summary>要素数が不足している場合、要素数を指定の数に合わせる。</summary>
        /// <param name="length">[Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]調整する要素数</param>
        /// <returns>追加した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> が指定範囲外の場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLengthIfShort(int length) => Items.AdjustLengthIfShort(length);

        /// <summary>要素数が超過している場合、要素数を指定の数に合わせる。</summary>
        /// <param name="length">[Range(<see cref="GetMinCapacity"/>, <see cref="GetMaxCapacity"/>)]調整する要素数</param>
        /// <returns>削除した要素</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> が指定範囲外の場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLengthIfLong(int length) => Items.AdjustLengthIfLong(length);

        /// <summary>要素を与えられた内容で一新する。</summary>
        /// <param name="settings">リストに詰め直す要素</param>
        /// <returns>新たにリストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentException"><paramref name="settings"/> の要素数が <see cref="GetMinCapacity"/> 未満または <see cref="GetMaxCapacity"/> を超える場合。</exception>
        /// <remarks>このメソッドは <paramref name="settings"/> の要素数が<see cref="GetMinCapacity"/> 以上 <see cref="GetMaxCapacity"/> 以下であれば成功する。<br/>現在の要素数と一致しない場合エラーとしたい場合は、容量固定型にキャストしてから同メソッドを呼び出す。</remarks>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Reset(IEnumerable<IStubModelSettings> settings) => Items.Reset(settings);

        /// <summary>要素をデフォルト値で一新する。</summary>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Reset() => Items.Reset();

        /// <summary>自身を初期化する。</summary>
        public void Clear() => Items.Clear();

        /// <summary><see cref="Get"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Get" path="param|exception"/>
        [Pure]
        public void ValidateGet(int index) => Items.ValidateGet(index);

        /// <summary><see cref="GetRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="GetRange" path="param|exception"/>
        [Pure]
        public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);

        /// <summary><see cref="Set"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Set" path="param|exception"/>
        [Pure]
        public void ValidateSet(int index, IStubModelSettings settings) => Items.ValidateSet(index, settings);

        /// <summary><see cref="SetRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="SetRange" path="param|exception"/>
        [Pure]
        public void ValidateSetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateSetRange(index, settings);

        /// <summary><see cref="Add"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Add" path="param|exception"/>
        [Pure]
        public void ValidateAdd(IStubModelSettings settings) => Items.ValidateAdd(settings);

        /// <summary><see cref="AddRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="AddRange" path="param|exception"/>
        [Pure]
        public void ValidateAddRange(IEnumerable<IStubModelSettings> settings) => Items.ValidateAddRange(settings);

        /// <summary><see cref="Insert"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Insert" path="param|exception"/>
        [Pure]
        public void ValidateInsert(int index, IStubModelSettings settings) => Items.ValidateInsert(index, settings);

        /// <summary><see cref="InsertRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="InsertRange" path="param|exception"/>
        [Pure]
        public void ValidateInsertRange(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateInsertRange(index, settings);

        /// <summary><see cref="Overwrite"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Overwrite" path="param|exception"/>
        [Pure]
        public void ValidateOverwrite(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateOverwrite(index, settings);

        /// <summary><see cref="Move"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Move" path="param|exception"/>
        [Pure]
        public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);

        /// <summary><see cref="MoveRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="MoveRange" path="param|exception"/>
        [Pure]
        public void ValidateMoveRange(int oldIndex, int newIndex, int count) => Items.ValidateMoveRange(oldIndex, newIndex, count);

        /// <summary><see cref="Remove"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Remove" path="param|exception"/>
        [Pure]
        public void ValidateRemove(int index) => Items.ValidateRemove(index);

        /// <summary><see cref="RemoveRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="RemoveRange" path="param|exception"/>
        [Pure]
        public void ValidateRemoveRange(int index, int count) => Items.ValidateRemoveRange(index, count);

        /// <summary><see cref="AdjustLength"/>,<see cref="AdjustLengthIfShort"/>,<see cref="AdjustLengthIfLong"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="AdjustLength" path="param|exception"/>
        [Pure]
        public void ValidateAdjustLength(int length) => Items.ValidateAdjustLength(length);

        /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|exception"/>
        [Pure]
        public void ValidateReset(IEnumerable<IStubModelSettings> settings) => Items.ValidateReset(settings);

        /// <summary><see cref="Clear"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Clear" path="param|exception"/>
        [Pure]
        public void ValidateClear() => Items.ValidateClear();

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetInternal"/>
        [Pure]
        public WodiLib.Test.Tools.StubModel GetInternal(int index) => Items.GetInternal(index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetRangeInternal"/>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubModel> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);

        /// <summary><see cref="Set"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Set" path="param"/>
        public WodiLib.Test.Tools.StubModel SetInternal(int index, IStubModelSettings settings) => Items.SetInternal(index, settings);

        /// <summary><see cref="SetRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="SetRange" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetRangeInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRangeInternal(index, settings);

        /// <summary><see cref="Add"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Add" path="param|returns"/>
        public WodiLib.Test.Tools.StubModel AddInternal(IStubModelSettings settings) => Items.AddInternal(settings);

        /// <summary><see cref="AddRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="AddRange" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AddRangeInternal(
            IEnumerable<IStubModelSettings> settings
        ) => Items.AddRangeInternal(settings);

        /// <summary><see cref="Insert"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Insert" path="param|returns"/>
        public WodiLib.Test.Tools.StubModel InsertInternal(int index, IStubModelSettings settings) => Items.InsertInternal(index, settings);

        /// <summary><see cref="InsertRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="InsertRange" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> InsertRangeInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.InsertRangeInternal(index, settings);

        /// <summary><see cref="Overwrite"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Overwrite" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> OverwriteInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.OverwriteInternal(index, settings);

        /// <summary><see cref="Move"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Move" path="param"/>
        public void MoveInternal(int oldIndex, int newIndex) => Items.MoveInternal(oldIndex, newIndex);

        /// <summary><see cref="MoveRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="MoveRange" path="param"/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count) => Items.MoveRangeInternal(oldIndex, newIndex, count);

        /// <summary><see cref="Remove"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Remove" path="param|returns"/>
        public WodiLib.Test.Tools.StubModel RemoveInternal(int index) => Items.RemoveInternal(index);

        /// <summary><see cref="RemoveRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="RemoveRange" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> RemoveRangeInternal(int index, int count) => Items.RemoveRangeInternal(index, count);

        /// <summary><see cref="AdjustLength"/>,<see cref="AdjustLengthIfShort"/>,<see cref="AdjustLengthIfLong"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="AdjustLength" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLengthInternal(int length) => Items.AdjustLengthInternal(length);

        /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> ResetInternal(IEnumerable<IStubModelSettings> settings) => Items.ResetInternal(settings);

        /// <summary><see cref="Reset()"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Reset()" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> ResetInternal() => Items.ResetInternal();

        /// <summary><see cref="Clear"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Clear" path="param"/>
        public void ClearInternal() => Items.ClearInternal();

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubRestrictedCapacityList? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(FixedStubRestrictedCapacityList? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubRestrictedCapacityList? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);

        /// <inheritdoc/>
        [Pure]
        public StubRestrictedCapacityList DeepClone() => new(this);
        object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();

        System.Collections.Generic.IReadOnlyList<System.String> IStubRestrictedCapacityListSettings.Tags => Tags;

        /// <summary>
        ///     <see cref="ExtendedList{TListSettings, TEditableElement, TReadOnlyElement, TElementSettings}"/> が通知した
        ///     <see cref="INotifyCollectionChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <param name="target">対象</param>
        private void PropagateCollectionChangeEvent(ExtendedList<IStubRestrictedCapacityListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings> target)
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }

        private FixedStubRestrictedCapacityList? fixedLengthInstance = null;
        private ReadOnlyStubRestrictedCapacityList? readonlyInstance = null;

        /// <summary>
        ///     容量固定クラスへの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]
        public static implicit operator FixedStubRestrictedCapacityList?(StubRestrictedCapacityList? src)
        {
            if (src is null) return null;
            src.fixedLengthInstance ??= new FixedStubRestrictedCapacityList(src);
            return src.fixedLengthInstance;
        }

        /// <summary>
        ///     読取専用クラスへの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]
        public static implicit operator ReadOnlyStubRestrictedCapacityList?(StubRestrictedCapacityList? src)
        {
            if (src is null) return null;
            src.readonlyInstance ??= new ReadOnlyStubRestrictedCapacityList(src);
            return src.readonlyInstance;
        }
    }
    */
    /*
    /// <summary>
    ///     【容量固定】<see cref="StubRestrictedCapacityList"/> スタブ用
    /// </summary>
    public partial class FixedStubRestrictedCapacityList : ModelBase,
        IStubRestrictedCapacityListSettings,
        IEnumerable<WodiLib.Test.Tools.StubModel>,
        INotifyCollectionChanged,
        IEqualityComparable<StubRestrictedCapacityList>,
        IEqualityComparable<FixedStubRestrictedCapacityList>,
        IEqualityComparable<ReadOnlyStubRestrictedCapacityList>,
        WodiLib.Sys.IDeepCloneable<FixedStubRestrictedCapacityList>
    {
        /// <summary>容量最大値</summary>
        [Pure]
        public static int MaxCapacity => 10;
        /// <summary>容量最小値</summary>
        [Pure]
        public static int MinCapacity => 1;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => MutableInstance.CollectionChanged += value;
            remove => MutableInstance.CollectionChanged -= value;
        }

        /// <summary>インデクサによるアクセス</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        public WodiLib.Test.Tools.StubModel this[int index]
        {
            [Pure]
            get => MutableInstance[index];
            set => MutableInstance[index] = value;
        }

        /// <summary>要素数</summary>
        [Pure]
        public int Count => MutableInstance.Count;

        /// <inheritdoc/>
        public System.Collections.Generic.List<string> Tags => MutableInstance.Tags;

        /// <inheritdoc/>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Pure]
        public IList<IStubModelSettings> Settings => MutableInstance.Settings;

        internal StubRestrictedCapacityList MutableInstance { get; }

        internal FixedStubRestrictedCapacityList(StubRestrictedCapacityList mutableInstance)
        {
            MutableInstance = mutableInstance;
            PropagatePropertyChangeEvent(MutableInstance);
        }

        /// <inheritdoc/>
        public System.String ToJsonString() => MutableInstance.ToJsonString();
        /// <inheritdoc/>
        public void SetNowStringValue() => MutableInstance.SetNowStringValue();

        /// <inheritdoc/>
        [Pure]
        public IEnumerator<WodiLib.Test.Tools.StubModel> GetEnumerator() => MutableInstance.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>指定インデックスの要素を取得する。</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        [Pure]
        public WodiLib.Test.Tools.StubModel Get(int index) => MutableInstance.Get(index);

        /// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubModel> GetRange(int index, int count) => MutableInstance.GetRange(index, count);

        /// <summary>リストの要素を更新する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
        public WodiLib.Test.Tools.StubModel Set(int index, IStubModelSettings settings) => MutableInstance.Set(index, settings);

        /// <summary>リストの連続した要素を更新する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetRange(int index, IEnumerable<IStubModelSettings> settings) => MutableInstance.SetRange(index, settings);

        /// <summary>指定したインデックスにある項目をコレクション内の新しい場所へ移動する。</summary>
        /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。</exception>
        public void Move(int oldIndex, int newIndex) => MutableInstance.Move(oldIndex, newIndex);

        /// <summary>指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。</summary>
        /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)]移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)]移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)]移動させる要素数</param>
        /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRange(int oldIndex, int newIndex, int count) => MutableInstance.MoveRange(oldIndex, newIndex, count);

        /// <summary>要素を与えられた内容で一新する。</summary>
        /// <param name="settings">リストに詰め直す要素</param>
        /// <returns>新たにリストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentException"><paramref name="settings"/> の要素数が <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> と異なる場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Reset(IEnumerable<IStubModelSettings> settings) => (FixedLengthList<IStubRestrictedCapacityListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings>)MutableInstance.Reset(settings);

        /// <summary>要素をデフォルト値で一新する。</summary>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Reset() => MutableInstance.Reset();

        /// <summary><see cref="Get"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Get" path="param|exception"/>
        [Pure]
        public void ValidateGet(int index) => MutableInstance.ValidateGet(index);

        /// <summary><see cref="GetRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="GetRange" path="param|exception"/>
        [Pure]
        public void ValidateGetRange(int index, int count) => MutableInstance.ValidateGetRange(index, count);

        /// <summary><see cref="Set"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Set" path="param|exception"/>
        [Pure]
        public void ValidateSet(int index, IStubModelSettings settings) => MutableInstance.ValidateSet(index, settings);

        /// <summary><see cref="SetRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="SetRange" path="param|exception"/>
        [Pure]
        public void ValidateSetRange(int index, IEnumerable<IStubModelSettings> settings) => MutableInstance.ValidateSetRange(index, settings);

        /// <summary><see cref="Move"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Move" path="param|exception"/>
        [Pure]
        public void ValidateMove(int oldIndex, int newIndex) => MutableInstance.ValidateMove(oldIndex, newIndex);

        /// <summary><see cref="MoveRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="MoveRange" path="param|exception"/>
        [Pure]
        public void ValidateMoveRange(int oldIndex, int newIndex, int count) => MutableInstance.ValidateMoveRange(oldIndex, newIndex, count);

        /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|exception"/>
        [Pure]
        public void ValidateReset(IEnumerable<IStubModelSettings> settings) => MutableInstance.ValidateReset(settings);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetInternal"/>
        [Pure]
        public WodiLib.Test.Tools.StubModel GetInternal(int index) => MutableInstance.GetInternal(index);

        /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetRangeInternal"/>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubModel> GetRangeInternal(int index, int count) => MutableInstance.GetRangeInternal(index, count);

        /// <summary><see cref="Set"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Set" path="param"/>
        public WodiLib.Test.Tools.StubModel SetInternal(int index, IStubModelSettings settings) => MutableInstance.SetInternal(index, settings);

        /// <summary><see cref="SetRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="SetRange" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetRangeInternal(int index, IEnumerable<IStubModelSettings> settings) => MutableInstance.SetRangeInternal(index, settings);

        /// <summary><see cref="Move"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Move" path="param"/>
        public void MoveInternal(int oldIndex, int newIndex) => MutableInstance.MoveInternal(oldIndex, newIndex);

        /// <summary><see cref="MoveRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="MoveRange" path="param"/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count) => MutableInstance.MoveRangeInternal(oldIndex, newIndex, count);

        /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/>,<see cref="Reset()"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> ResetInternal(IEnumerable<IStubModelSettings> settings) => MutableInstance.ResetInternal(settings);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubRestrictedCapacityList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(FixedStubRestrictedCapacityList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubRestrictedCapacityList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IStubRestrictedCapacityListSettings? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public FixedStubRestrictedCapacityList DeepClone() => new(MutableInstance.DeepClone());
        object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();

        System.Collections.Generic.IReadOnlyList<System.String> IStubRestrictedCapacityListSettings.Tags => Tags;


        /// <summary>
        ///     読取専用クラスへの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]
        public static implicit operator ReadOnlyStubRestrictedCapacityList?(FixedStubRestrictedCapacityList? src)
        {
            return src?.MutableInstance;
        }
    }
    */
    /*
    /// <summary>
    ///     【読取専用】<see cref="StubRestrictedCapacityList"/> スタブ用
    /// </summary>
    public partial class ReadOnlyStubRestrictedCapacityList : ModelBase,
        IStubRestrictedCapacityListSettings,
        IEnumerable<ReadOnlyStubModel>,
        INotifyCollectionChanged,
        IEqualityComparable<ReadOnlyStubRestrictedCapacityList>,
        IEqualityComparable<FixedStubRestrictedCapacityList>,
        IEqualityComparable<StubRestrictedCapacityList>,
        IDeepCloneable<ReadOnlyStubRestrictedCapacityList>
    {

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => MutableInstance.CollectionChanged += value;
            remove => MutableInstance.CollectionChanged -= value;
        }

        /// <summary>インデクサによるアクセス</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定したインデックスの要素</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        [Pure]
        public ReadOnlyStubModel this[int index] => MutableInstance[index];

        /// <summary>要素数</summary>
        [Pure]
        public int Count => MutableInstance.Count;

        /// <inheritdoc/>
        public System.Collections.Generic.IReadOnlyList<string> Tags => MutableInstance.Tags;

        /// <inheritdoc/>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Pure]
        public IList<IStubModelSettings> Settings => MutableInstance.Settings;

        internal FixedStubRestrictedCapacityList MutableInstance { get; }

        internal ReadOnlyStubRestrictedCapacityList(FixedStubRestrictedCapacityList mutableInstance)
        {
            MutableInstance = mutableInstance;
            PropagatePropertyChangeEvent(MutableInstance);
        }

        /// <inheritdoc/>
        public IEnumerator<ReadOnlyStubModel> GetEnumerator() => MutableInstance.Cast<ReadOnlyStubModel>().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>指定インデックスの要素を取得する。</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
        [Pure]
        public ReadOnlyStubModel Get(int index) => MutableInstance.Get(index);

        /// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>
        /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
        /// <param name="count">[Range(0, <see cref="Count"/>)] 要素数</param>
        /// <returns>指定範囲の要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<ReadOnlyStubModel> GetRange(int index, int count) => MutableInstance.GetRange(index, count).Cast<ReadOnlyStubModel>();

        /// <summary><see cref="Get"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Get" path="param|exception"/>
        [Pure]
        public void ValidateGet(int index) => MutableInstance.ValidateGet(index);

        /// <summary><see cref="GetRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="GetRange" path="param|exception"/>
        [Pure]
        public void ValidateGetRange(int index, int count) => MutableInstance.ValidateGetRange(index, count);

        /// <summary><see cref="Get"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Get" path="param"/>
        [Pure]
        public ReadOnlyStubModel GetInternal(int index) => MutableInstance.GetInternal(index);

        /// <summary><see cref="GetRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="GetRange" path="param"/>
        [Pure]
        public IEnumerable<ReadOnlyStubModel> GetRangeInternal(int index, int count) => MutableInstance.GetRangeInternal(index, count).Cast<ReadOnlyStubModel>();

        /// <inheritdoc/>
        public System.String ToJsonString() => MutableInstance.ToJsonString();

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubRestrictedCapacityList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(FixedStubRestrictedCapacityList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubRestrictedCapacityList? other) => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IStubRestrictedCapacityListSettings? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public ReadOnlyStubRestrictedCapacityList DeepClone() => new(MutableInstance.DeepClone());
        object IDeepCloneable.DeepClone() => DeepClone();

        System.Collections.Generic.IReadOnlyList<System.String> IStubRestrictedCapacityListSettings.Tags => Tags;


    }
    */
}
