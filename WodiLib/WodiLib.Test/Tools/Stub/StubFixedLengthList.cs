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
     * テスト用のリストクラス（容量固定）
     * WodiLib内で定義するリストクラスの実装サンプルも兼ねる。
     *
     * いくつかの属性を使用し、
     * SourceGenerator で自動生成する。
     *
     * 生成されたソースは Generated/{名前空間}/{クラス名}.cs に出力される。
     *
     * 基本的な解説は StubModel.cs, StubRestrictedCapacityList.cs を参照。
     */

    // /*
    //  * IStubFixedLengthListSettings は SourceGenerator で自動生成されるため、
    //  * 手作業での作成不要。
    //  *
    //  * 以下、自動生成される設定DTOインタフェース
    //  */
    /*
    /// <summary>
    ///     <see cref="StubFixedLengthList"/> スタブ用設定インタフェース
    /// </summary>
    public partial interface IStubFixedLengthListSettings : WodiLib.Sys.IEqualityComparable<IStubFixedLengthListSettings>, IListSettings<IStubModelSettings>
    {
        /// <inheritdoc cref="StubFixedLengthList.Tags" />
        System.Collections.Generic.IReadOnlyList<System.String> Tags { get; }
    }
    */

    /*
    /// <summary>
    ///     <see cref="StubFixedLengthList"/> スタブ用設定DTO
    /// </summary>
    public partial record StubFixedLengthListSettings(IList<IStubModelSettings> Settings) : IStubFixedLengthListSettings
    {
        /// <inheritdoc cref="IStubFixedLengthListSettings.Tags" path="summary|remarks" />
        public System.Collections.Generic.IReadOnlyList<System.String> Tags { get; set; } = new List<string>();

        /// <inheritdoc/>
        public bool ItemEquals(IStubFixedLengthListSettings? other)
        {
            return other is not null
                   && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
                   && Tags.SequenceEqual(other.Tags);
        }

        /// <inheritdoc/>
        public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLengthListSettings);
    }
    */

    [FixedLengthListImplementTemplate(
        Description = "<see cref=\"StubFixedLengthList\"/> スタブ用",
        ElementType = typeof(StubModel),
        ReadOnlyElementType = typeof(ReadOnlyStubModel),
        SettingsType = typeof(IStubModelSettings),
        MaxCapacity = 5, // MaxCapacity == MinCapacity としているが、 MaxCapacity > MinCapacity としても良い。
        MinCapacity = 5 // その場合でも通常の方法ではインスタンス作成後にサイズを変えることはできない。
    )]
    public partial class StubFixedLengthList
        /*
         * 以下7つの基底クラス・インタフェースは SourceGenerator が自動的に付与する。
         */
        // ModelBase,
        // IStubFixedLengthListSettings,
        // IEnumerable<StubModel>,
        // INotifyCollectionChanged,
        // IEqualityComparable<StubFixedLengthList>,
        // IEqualityComparable<ReadOnlyStubFixedLengthList>,
        // IDeepCloneable<StubFixedLengthList>
    {
        [SettingsProperty(
            ReturnType = typeof(IReadOnlyList<string>),
            DefaultValue = "new List<string>()"
        )]
        [ImmutableProperty(
            Accessibility = "public"
        )]
        public IReadOnlyList<string> Tags => tags;

        private readonly List<string> tags = new();

        public StubFixedLengthList(IStubFixedLengthListSettings settings)
            : this(settings, BuildSimpleList(settings.Settings), BuildItemFromSettings)
        {
            tags = settings.Tags.ToList();
        }

        public StubFixedLengthList(
            int length
        ) : this(
            new StubFixedLengthListSettings(length.Iterate(i => (IStubModelSettings)BuildItemFromIndex(i)).ToArray())
        )
        {
        }

        /*
         * 純粋メソッド。
         * 編集可能モデルクラスでも通常使用可能なため、属性はつけない。
         */
        [ImmutableMethod]
        public string ToJsonString()
        {
            // メソッド定義が参照できることのテストをするためだけのメソッドなので、
            // 戻り値は適当な値とする
            return "JSON RESULT";
        }

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
        private IWodiLibListValidator<IStubFixedLengthListSettings, IStubModelSettings> BuildValidator(
            IStubFixedLengthListSettings _,
            SimpleList<StubModel> itemsImpl
        )
        {
            return new RestrictedCapacityListValidator<IStubFixedLengthListSettings, IStubModelSettings>(
                countGetter: () => itemsImpl.Count,
                minCapacityGetter: GetCapacity,
                maxCapacityGetter: GetCapacity
            );
        }
    }

    /*
     * 以下は SourceGeneratorで生成されるクラス定義のサンプル。
     */
    /*

    /// <summary>
    ///     <see cref="StubFixedLengthList"/> スタブ用
    /// </summary>
    public partial class StubFixedLengthList : ModelBase,
        IStubFixedLengthListSettings,
        IEnumerable<WodiLib.Test.Tools.StubModel>,
        INotifyCollectionChanged,
        IEqualityComparable<StubFixedLengthList>,
        IEqualityComparable<ReadOnlyStubFixedLengthList>,
        WodiLib.Sys.IDeepCloneable<StubFixedLengthList>
    {
        /// <summary>容量</summary>
        [Pure]
        public static int Capacity => 5;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        /// <summary>インデクサによるアクセス</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] インデックス</param>
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
        [Pure]
        public int Count => Items.Count;

        /// <inheritdoc/>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Pure]
        public IList<IStubModelSettings> Settings => Items.Cast<IStubModelSettings>().ToList();

        private protected ExtendedList<IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings> Items { get; }

        private StubFixedLengthList(
            IStubFixedLengthListSettings settings,
            SimpleList<WodiLib.Test.Tools.StubModel> itemsImpl,
            Func<int, IStubModelSettings, WodiLib.Test.Tools.StubModel> itemBuilder
        )
        {
            var validator = BuildValidator(settings, itemsImpl);
            validator?.Constructor((nameof(settings), settings));
            Items = new ExtendedList<IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings>(
                itemsImpl,
                minCapacity: Capacity,
                maxCapacity: Capacity,
                validator,
                buildItemFromSettings: (index, modelSettings) => itemBuilder(index, modelSettings)
            );
            PropagatePropertyChangeEvent(Items);
            PropagateCollectionChangeEvent(Items);
        }

        /// <summary>容量を取得する。</summary>
        /// <returns>容量最大値</returns>
        [Pure]
        public int GetCapacity() => Capacity;

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
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
        public WodiLib.Test.Tools.StubModel Set(int index, IStubModelSettings settings) => Items.Set(index, settings);

        /// <summary>リストの連続した要素を更新する。</summary>
        /// <param name="index">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 更新開始インデックス</param>
        /// <param name="settings">更新要素</param>
        /// <returns>セットした要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRange(index, settings);

        /// <summary>指定したインデックスにある項目をコレクション内の新しい場所へ移動する。</summary>
        /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 移動する項目のインデックス</param>
        /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 移動先のインデックス</param>
        /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。</exception>
        public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);

        /// <summary>指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。</summary>
        /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)]移動する項目のインデックス開始位置</param>
        /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)]移動先のインデックス開始位置</param>
        /// <param name="count">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/>)]移動させる要素数</param>
        /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
        public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, count);

        /// <summary>要素を与えられた内容で一新する。</summary>
        /// <param name="settings">リストに詰め直す要素</param>
        /// <returns>新たにリストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
        /// <exception cref="ArgumentException"><paramref name="settings"/> の要素数が <see cref="ReadOnlyStubFixedLengthList.Count"/> と異なる場合。</exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Reset(IEnumerable<IStubModelSettings> settings) => (FixedLengthList<IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings>)Items.Reset(settings);

        /// <summary>要素をデフォルト値で一新する。</summary>
        public IEnumerable<WodiLib.Test.Tools.StubModel> Reset() => Items.Reset();

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

        /// <summary><see cref="Move"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Move" path="param|exception"/>
        [Pure]
        public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);

        /// <summary><see cref="MoveRange"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="MoveRange" path="param|exception"/>
        [Pure]
        public void ValidateMoveRange(int oldIndex, int newIndex, int count) => Items.ValidateMoveRange(oldIndex, newIndex, count);

        /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッドの検証処理。</summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|exception"/>
        [Pure]
        public void ValidateReset(IEnumerable<IStubModelSettings> settings) => Items.ValidateReset(settings);

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

        /// <summary><see cref="Move"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Move" path="param"/>
        public void MoveInternal(int oldIndex, int newIndex) => Items.MoveInternal(oldIndex, newIndex);

        /// <summary><see cref="MoveRange"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="MoveRange" path="param"/>
        public void MoveRangeInternal(int oldIndex, int newIndex, int count) => Items.MoveRangeInternal(oldIndex, newIndex, count);

        /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/>,<see cref="Reset()"/> メソッド処理中核。</summary>
        /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> ResetInternal(IEnumerable<IStubModelSettings> settings) => Items.ResetInternal(settings);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubFixedLengthList? other) => ItemEquals(other as IStubFixedLengthListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubFixedLengthList? other) => ItemEquals(other as IStubFixedLengthListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLengthListSettings);

        /// <inheritdoc/>
        [Pure]
        public StubFixedLengthList DeepClone() => new(this);
        object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();

        System.Collections.Generic.IReadOnlyList<System.String> IStubFixedLengthListSettings.Tags => Tags;

        /// <summary>
        ///     <see cref="ExtendedList{TListSettings, TEditableElement, TReadOnlyElement, TElementSettings}"/> が通知した
        ///     <see cref="INotifyCollectionChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <param name="target">対象</param>
        private void PropagateCollectionChangeEvent(ExtendedList<IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings> target)
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }

        private ReadOnlyStubFixedLengthList? readonlyInstance = null;

        /// <summary>
        ///     読取専用クラスへの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]
        public static implicit operator ReadOnlyStubFixedLengthList?(StubFixedLengthList? src)
        {
            if (src is null) return null;
            src.readonlyInstance ??= new ReadOnlyStubFixedLengthList(src);
            return src.readonlyInstance;
        }
    }
    */
    /*
    /// <summary>
    ///     【読取専用】<see cref="StubFixedLengthList"/> スタブ用
    /// </summary>
    public partial class ReadOnlyStubFixedLengthList : ModelBase,
        IStubFixedLengthListSettings,
        IEnumerable<ReadOnlyStubModel>,
        INotifyCollectionChanged,
        IEqualityComparable<ReadOnlyStubFixedLengthList>,
        IEqualityComparable<StubFixedLengthList>,
        IDeepCloneable<ReadOnlyStubFixedLengthList>
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

        internal StubFixedLengthList MutableInstance { get; }

        internal ReadOnlyStubFixedLengthList(StubFixedLengthList mutableInstance)
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
        public bool ItemEquals(ReadOnlyStubFixedLengthList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubFixedLengthList? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IStubFixedLengthListSettings? other) => MutableInstance.ItemEquals(other);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public ReadOnlyStubFixedLengthList DeepClone() => new(MutableInstance.DeepClone());
        object IDeepCloneable.DeepClone() => DeepClone();

        System.Collections.Generic.IReadOnlyList<System.String> IStubFixedLengthListSettings.Tags => Tags;


    }
    */
}
