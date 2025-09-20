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

    // /*
    //  * IStubRestrictedCapacityListSettings は SourceGenerator で自動生成されるため、
    //  * 手作業での作成不要。
    //  *
    //  * 以下、自動生成される設定DTOインタフェース
    //  */
    // public partial interface IStubRestrictedCapacityListSettings : WodiLib.Sys.IEqualityComparable<IStubRestrictedCapacityListSettings>
    // {
    //     /// <inheritdoc cref="StubRestrictedCapacityList.Settings"/>
    //     public IReadOnlyList<IStubModelSettings> Settings { get; }
    //
    //     /// <inheritdoc cref="ReadOnlyStubRestrictedCapacityList.Tags" />
    //     System.Collections.Generic.IReadOnlyList<System.String> Tags { get; }
    // }
    //
    // public record StubRestrictedCapacityListSettings(
    //     /*
    //      * 行データの入力パラメータは必ずコンストラクタで受け取るようになる。
    //      */
    //     IReadOnlyList<IStubModelSettings> Settings
    // ) : IStubRestrictedCapacityListSettings
    // {
    //     /// <inheritdoc cref="IStubRestrictedCapacityListSettings.Tags" path="summary|remarks" />
    //     public System.Collections.Generic.IReadOnlyList<System.String> Tags { get; set; } = new List<string>();
    //
    //     public bool ItemEquals(IStubRestrictedCapacityListSettings? other)
    //     {
    //         return other is not null
    //                && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
    //                && Tags.SequenceEqual(other.Tags);
    //     }
    //
    //     public bool ItemEquals(object? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
    // }

    [RestrictedCapacityListImplementTemplate(
        Description = "<see cref=\"StubRestrictedCapacityList\"/> スタブ用",
        ElementType = typeof(StubModel),
        ReadOnlyElementType = typeof(ReadOnlyStubModel),
        SettingsType = typeof(IStubModelSettings),
        MaxCapacity = 10,
        MinCapacity = 1
    )]
    public partial class ReadOnlyStubRestrictedCapacityList
        /*
         * 以下8つの基底クラス・インタフェースは SourceGenerator が自動的に付与する。
         */
        // ModelBase,
        // IReadOnlyList<ReadOnlyStubModel>,
        // INotifyCollectionChanged,
        // IStubRestrictedCapacityListSettings,
        // IEqualityComparable<ReadOnlyStubRestrictedCapacityList>,
        // IEqualityComparable<FixedStubRestrictedCapacityList>,
        // IEqualityComparable<StubRestrictedCapacityList>,
        // IDeepCloneable<ReadOnlyStubRestrictedCapacityList>
    {
        [SettingsProperty(
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
            Accessibility = "NONE"
        )]
        [MutableProperty(
            Accessibility = "NONE",
            ReturnType = typeof(IList<string>)
        )]
        public IReadOnlyList<string> Tags => tags;

        private readonly List<string> tags = new();

        /*
         * 作成するコンストラクタは必ず this(SimpleList{T}) を呼び出す。
         */
        [FixedLengthListConstructor]
        [MutableConstructor]
        public ReadOnlyStubRestrictedCapacityList(
            int length
        ) : this(BuildSimpleList(length))
        {
        }

        public ReadOnlyStubRestrictedCapacityList(IStubRestrictedCapacityListSettings settings)
            : this(BuildSimpleList(settings.Settings))
        {
            tags = settings.Tags.ToList();
        }

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
        protected void SetNowStringValue()
        {
            ((IEnumerable<StubModel>)Items).ForEach(item => item.SetNowStringValue());
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
        private protected static SimpleList<StubModel> BuildSimpleList(int length)
        {
            return new SimpleList<StubModel>(
                ElementBuilder,
                length.Iterate(BuildItemFromIndex)
            );
        }

        private protected static SimpleList<StubModel> BuildSimpleList(IEnumerable<IStubModelSettings> settings)
        {
            return new SimpleList<StubModel>(
                ElementBuilder,
                settings.Select(setting => new StubModel(setting))
            );
        }

        private protected static SimpleListValueBuilder<StubModel> ElementBuilder { get; }
            = new(BuildItemFromIndex);

        private protected static StubModel BuildItemFromIndex(int index)
            => new(index.ToString());

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private protected static StubModel BuildItemFromSettings(int index, IStubModelSettings settings)
        {
            return new StubModel(settings);
        }

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する ExtendedList のコンストラクタ引数として指定する。
         */
        private protected IWodiLibListValidator<IStubModelSettings> BuildValidator(
            SimpleList<StubModel> itemsImpl
        )
        {
            return new RestrictedCapacityListValidator<IStubModelSettings>(
                countGetter: () => itemsImpl.Count,
                minCapacityGetter: GetMinCapacity,
                maxCapacityGetter: GetMaxCapacity
            );
        }
    }

    /*
     * 以下は SourceGeneratorで生成されるクラス定義のサンプル。
     */
    // public partial class ReadOnlyStubRestrictedCapacityList : ModelBase,
    //     IStubRestrictedCapacityListSettings,
    //     IReadOnlyList<WodiLib.Test.Tools.ReadOnlyStubModel>,
    //     IEqualityComparable<ReadOnlyStubRestrictedCapacityList>,
    //     IEqualityComparable<FixedStubRestrictedCapacityList>,
    //     IEqualityComparable<StubRestrictedCapacityList>,
    //     IDeepCloneable<ReadOnlyStubRestrictedCapacityList>
    // {
    //     /// <summary>容量最大値</summary>
    //     public static int MaxCapacity => 10;
    //     /// <summary>容量最小値</summary>
    //     public static int MinCapacity => 1;
    //
    //     private protected ReadOnlyStubRestrictedCapacityList(SimpleList<WodiLib.Test.Tools.StubModel> itemsImpl)
    //     {
    //         Items = new ExtendedList<WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel, IStubModelSettings>(
    //             itemsImpl,
    //             minCapacity: 10,
    //             maxCapacity: 1,
    //             validator: BuildValidator(itemsImpl),
    //             buildItemFromSettings: BuildItemFromSettings
    //         );
    //     }
    //
    //     /// <summary>インデクサによるアクセス</summary>
    //     /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
    //     /// <returns>指定したインデックスの要素</returns>
    //     /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     public WodiLib.Test.Tools.ReadOnlyStubModel this[int index] => Get(index);
    //
    //     /// <summary>要素数</summary>
    //     public int Count => Items.Count;
    //
    //     /// <inheritdoc/>
    //     public IReadOnlyList<IStubModelSettings> Settings => Items.Cast<IStubModelSettings>().ToList();
    //
    //     private protected ExtendedList<WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel, IStubModelSettings> Items { get; }
    //
    //     /// <summary>容量最大値を取得する。</summary>
    //     /// <returns>容量最大値</returns>
    //     public int GetMaxCapacity() => MaxCapacity;
    //     /// <summary>容量最小値を取得する。</summary>
    //     /// <returns>容量最小値</returns>
    //     public int GetMinCapacity() => MinCapacity;
    //     /// <inheritdoc/>
    //     public IEnumerator<WodiLib.Test.Tools.ReadOnlyStubModel> GetEnumerator() => Items.GetEnumerator();
    //     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //
    //     /// <summary>指定インデックスの要素を取得する。</summary>
    //     /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
    //     /// <returns>指定範囲の要素簡易コピーリスト</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
    //     public WodiLib.Test.Tools.ReadOnlyStubModel Get(int index) => Items.Get(index);
    //
    //     /// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>
    //     /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
    //     /// <param name="count">[Range(0, <see cref="Count"/>)] 要素数</param>
    //     /// <returns>指定範囲の要素簡易コピーリスト</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.ReadOnlyStubModel> GetRange(int index, int count) => Items.GetRange(index, count);
    //
    //     /// <summary><see cref="Get"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Get" path="param|exception"/>
    //     public void ValidateGet(int index) => Items.ValidateGet(index);
    //
    //     /// <summary><see cref="GetRange"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="GetRange" path="param|exception"/>
    //     public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);
    //
    //     /// <summary><see cref="Get"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Get" path="param"/>
    //     public WodiLib.Test.Tools.ReadOnlyStubModel GetInternal(int index) => Items.GetInternal(index);
    //
    //     /// <summary><see cref="GetRange"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="GetRange" path="param"/>
    //     public IEnumerable<WodiLib.Test.Tools.ReadOnlyStubModel> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);
    //
    //     /// <inheritdoc/>
    //     public bool ItemEquals(ReadOnlyStubRestrictedCapacityList? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
    //     /// <inheritdoc/>
    //     public bool ItemEquals(FixedStubRestrictedCapacityList? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
    //     /// <inheritdoc/>
    //     public bool ItemEquals(StubRestrictedCapacityList? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
    //     /// <inheritdoc/>
    //     public bool ItemEquals(object? other) => ItemEquals(other as IStubRestrictedCapacityListSettings);
    //
    //     /// <inheritdoc/>
    //     public ReadOnlyStubRestrictedCapacityList DeepClone() => new(this);
    //     object IDeepCloneable.DeepClone() => DeepClone();
    // }

    // public partial class FixedStubRestrictedCapacityList : ReadOnlyStubRestrictedCapacityList,
    //     WodiLib.Sys.IDeepCloneable<FixedStubRestrictedCapacityList>
    // {
    //     /// <summary>インデクサによるアクセス</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
    //     /// <returns>指定したインデックスの要素</returns>
    //     /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     public new WodiLib.Test.Tools.StubModel this[int index]
    //     {
    //         get => Get(index);
    //         set => Set(index, value);
    //     }
    //
    //     /// <inheritdoc/>
    //     public new System.Collections.Generic.IReadOnlyList<string> Tags
    //     {
    //         get => base.Tags;
    //     }
    //
    //
    //     public FixedStubRestrictedCapacityList(IStubRestrictedCapacityListSettings settings) : base(settings) { }
    //
    //     private protected FixedStubRestrictedCapacityList(SimpleList<WodiLib.Test.Tools.StubModel> itemsImpl) : base(itemsImpl) { }
    //
    //     /// <inheritdoc/>
    //     public FixedStubRestrictedCapacityList(int length) : base(length) {}
    //
    //     /// <inheritdoc/>
    //     public new void SetNowStringValue() => base.SetNowStringValue();
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.Get"/>
    //     public new WodiLib.Test.Tools.StubModel Get(int index) => Items.Get(index);
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.GetRange"/>
    //     public new IEnumerable<WodiLib.Test.Tools.StubModel> GetRange(int index, int count) => Items.GetRange(index, count);
    //
    //     /// <summary>リストの要素を更新する。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 更新開始インデックス</param>
    //     /// <param name="settings">更新要素</param>
    //     /// <returns>セットした要素</returns>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
    //     public WodiLib.Test.Tools.StubModel Set(int index, IStubModelSettings settings) => Items.Set(index, settings);
    //
    //     /// <summary>リストの連続した要素を更新する。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 更新開始インデックス</param>
    //     /// <param name="settings">更新要素</param>
    //     /// <returns>セットした要素</returns>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を編集しようとした場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> SetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRange(index, settings);
    //
    //     /// <summary>指定したインデックスにある項目をコレクション内の新しい場所へ移動する。</summary>
    //     /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 移動する項目のインデックス</param>
    //     /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] 移動先のインデックス</param>
    //     /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。</exception>
    //     public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);
    //
    //     /// <summary>指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。</summary>
    //     /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)]移動する項目のインデックス開始位置</param>
    //     /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)]移動先のインデックス開始位置</param>
    //     /// <param name="count">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)]移動させる要素数</param>
    //     /// <exception cref="InvalidOperationException">自身の要素数が0の場合。</exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
    //     public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, count);
    //
    //     /// <summary>要素を与えられた内容で一新する。</summary>
    //     /// <param name="settings">リストに詰め直す要素</param>
    //     /// <returns>新たにリストに詰め直した要素</returns>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="ArgumentException"><paramref name="settings"/> の要素数が <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> と異なる場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> Reset(IEnumerable<IStubModelSettings> settings) => (FixedLengthList<WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel, IStubModelSettings>)Items.Reset(settings);
    //
    //     /// <summary>要素をデフォルト値で一新する。</summary>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> Reset() => Items.Reset();
    //
    //     /// <summary><see cref="Set"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Set" path="param|exception"/>
    //     public void ValidateSet(int index, IStubModelSettings settings) => Items.ValidateSet(index, settings);
    //
    //     /// <summary><see cref="SetRange"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="SetRange" path="param|exception"/>
    //     public void ValidateSetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateSetRange(index, settings);
    //
    //     /// <summary><see cref="Move"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Move" path="param|exception"/>
    //     public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);
    //
    //     /// <summary><see cref="MoveRange"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="MoveRange" path="param|exception"/>
    //     public void ValidateMoveRange(int oldIndex, int newIndex, int count) => Items.ValidateMoveRange(oldIndex, newIndex, count);
    //
    //     /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|exception"/>
    //     public void ValidateReset(IEnumerable<IStubModelSettings> settings) => Items.ValidateReset(settings);
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetInternal"/>
    //     public new WodiLib.Test.Tools.StubModel GetInternal(int index) => Items.GetInternal(index);
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetRangeInternal"/>
    //     public new IEnumerable<WodiLib.Test.Tools.StubModel> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);
    //
    //     /// <summary><see cref="Set"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Set" path="param"/>
    //     public WodiLib.Test.Tools.StubModel SetInternal(int index, IStubModelSettings settings) => Items.SetInternal(index, settings);
    //
    //     /// <summary><see cref="SetRange"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="SetRange" path="param"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> SetRangeInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRangeInternal(index, settings);
    //
    //     /// <summary><see cref="Move"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Move" path="param"/>
    //     public void MoveInternal(int oldIndex, int newIndex) => Items.MoveInternal(oldIndex, newIndex);
    //
    //     /// <summary><see cref="MoveRange"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="MoveRange" path="param"/>
    //     public void MoveRangeInternal(int oldIndex, int newIndex, int count) => Items.MoveRangeInternal(oldIndex, newIndex, count);
    //
    //     /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/>,<see cref="Reset()"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> ResetInternal(IEnumerable<IStubModelSettings> settings) => Items.ResetInternal(settings);
    //
    //     /// <inheritdoc/>
    //     public new FixedStubRestrictedCapacityList DeepClone() => new(this);
    //     object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();
    // }

    // public partial class StubRestrictedCapacityList : FixedStubRestrictedCapacityList,
    //     WodiLib.Sys.IDeepCloneable<StubRestrictedCapacityList>
    // {
    //     /// <inheritdoc/>
    //     public new System.Collections.Generic.IList<string> Tags
    //     {
    //         get => (System.Collections.Generic.IList<string>)base.Tags;
    //     }
    //
    //
    //     public StubRestrictedCapacityList(IStubRestrictedCapacityListSettings settings) : base(settings) { }
    //
    //     private protected StubRestrictedCapacityList(SimpleList<WodiLib.Test.Tools.StubModel> itemsImpl) : base(itemsImpl) { }
    //
    //     /// <inheritdoc/>
    //     public StubRestrictedCapacityList(int length) : base(length) {}
    //
    //
    //     /// <summary>リストの末尾に要素を追加する。</summary>
    //     /// <param name="settings">追加する要素</param>
    //     /// <returns>追加した要素</returns>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> を上回る場合。</exception>
    //     public WodiLib.Test.Tools.StubModel Add(IStubModelSettings settings) => Items.Add(settings);
    //
    //     /// <summary>リストの末尾に要素を追加する。</summary>
    //     /// <param name="settings">追加する要素</param>
    //     /// <returns>追加した要素</returns>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> を上回る場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> AddRange(IEnumerable<IStubModelSettings> settings) => Items.AddRange(settings);
    //
    //     /// <summary>指定したインデックスの位置に要素を挿入する。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] インデックス</param>
    //     /// <param name="settings">追加する要素</param>
    //     /// <returns>追加した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> を上回る場合。</exception>
    //     public WodiLib.Test.Tools.StubModel Insert(int index, IStubModelSettings settings) => Items.Insert(index, settings);
    //
    //     /// <summary>指定したインデックスの位置に要素を挿入する。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] インデックス</param>
    //     /// <param name="settings">追加する要素</param>
    //     /// <returns>追加した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> を上回る場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> InsertRange(int index, IEnumerable<IStubModelSettings> settings) => Items.InsertRange(index, settings);
    //
    //     /// <summary>指定したインデックスを起点として、要素の上書き/追加を行う。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] インデックス</param>
    //     /// <param name="settings">上書き/追加リスト</param>
    //     /// <returns>上書きした要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> を上回る場合。</exception>
    //     /// <example><code>var target = new List&lt;int&gt; { 0, 1, 2, 3 };var dst = new List&lt;int&gt; { 10, 11, 12 };target.Overwrite(2, dst);// target is { 0, 1, 10, 11, 12 }</code><code>var target = new List&lt;int&gt; { 0, 1, 2, 3 };var dst = new List&lt;int&gt; { 10 };target.Overwrite(2, dst);// target is { 0, 1, 10, 3 }</code></example>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> Overwrite(int index, IEnumerable<IStubModelSettings> settings) => Items.Overwrite(index, settings);
    //
    //     /// <summary>指定したインデックスの要素を削除する。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
    //     /// <returns>削除した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> が指定範囲外の場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/> を下回る場合。</exception>
    //     public WodiLib.Test.Tools.StubModel Remove(int index) => Items.Remove(index);
    //
    //     /// <summary>要素の範囲を削除する。</summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/> - 1)] インデックス</param>
    //     /// <param name="count">[Range(0, <see cref="ReadOnlyStubRestrictedCapacityList.Count"/>)] 削除する要素数</param>
    //     /// <returns>削除した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>, <paramref name="count"/> が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を削除しようとした場合。</exception>
    //     /// <exception cref="InvalidOperationException">操作によって要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/> を下回る場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> RemoveRange(int index, int count) => Items.RemoveRange(index, count);
    //
    //     /// <summary>要素数を指定の数に合わせる。</summary>
    //     /// <param name="length">[Range(<see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/>, <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/>)]調整する要素数</param>
    //     /// <returns>追加または削除した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> が指定範囲外の場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLength(int length) => Items.AdjustLength(length);
    //
    //     /// <summary>要素数が不足している場合、要素数を指定の数に合わせる。</summary>
    //     /// <param name="length">[Range(<see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/>, <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/>)]調整する要素数</param>
    //     /// <returns>追加した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> が指定範囲外の場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLengthIfShort(int length) => Items.AdjustLengthIfShort(length);
    //
    //     /// <summary>要素数が超過している場合、要素数を指定の数に合わせる。</summary>
    //     /// <param name="length">[Range(<see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/>, <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/>)]調整する要素数</param>
    //     /// <returns>削除した要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> が指定範囲外の場合。</exception>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLengthIfLong(int length) => Items.AdjustLengthIfLong(length);
    //
    //     /// <summary>要素を与えられた内容で一新する。</summary>
    //     /// <param name="settings">リストに詰め直す要素</param>
    //     /// <returns>新たにリストに詰め直した要素</returns>
    //     /// <exception cref="ArgumentNullException"><paramref name="settings"/> が <see langword="null"/> の場合、または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。</exception>
    //     /// <exception cref="ArgumentException"><paramref name="settings"/> の要素数が <see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/> 未満または <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> を超える場合。</exception>
    //     /// <remarks>このメソッドは <paramref name="settings"/> の要素数が<see cref="ReadOnlyStubRestrictedCapacityList.GetMinCapacity"/> 以上 <see cref="ReadOnlyStubRestrictedCapacityList.GetMaxCapacity"/> 以下であれば成功する。<br/>現在の要素数と一致しない場合エラーとしたい場合は、容量固定型にキャストしてから同メソッドを呼び出す。</remarks>
    //     public new IEnumerable<WodiLib.Test.Tools.StubModel> Reset(IEnumerable<IStubModelSettings> settings) => Items.Reset(settings);
    //
    //     /// <summary>自身を初期化する。</summary>
    //     public void Clear() => Items.Clear();
    //
    //     /// <summary><see cref="Add"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Add" path="param|exception"/>
    //     public void ValidateAdd(IStubModelSettings settings) => Items.ValidateAdd(settings);
    //
    //     /// <summary><see cref="AddRange"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="AddRange" path="param|exception"/>
    //     public void ValidateAddRange(IEnumerable<IStubModelSettings> settings) => Items.ValidateAddRange(settings);
    //
    //     /// <summary><see cref="Insert"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Insert" path="param|exception"/>
    //     public void ValidateInsert(int index, IStubModelSettings settings) => Items.ValidateInsert(index, settings);
    //
    //     /// <summary><see cref="InsertRange"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="InsertRange" path="param|exception"/>
    //     public void ValidateInsertRange(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateInsertRange(index, settings);
    //
    //     /// <summary><see cref="Overwrite"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Overwrite" path="param|exception"/>
    //     public void ValidateOverwrite(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateOverwrite(index, settings);
    //
    //     /// <summary><see cref="Remove"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Remove" path="param|exception"/>
    //     public void ValidateRemove(int index) => Items.ValidateRemove(index);
    //
    //     /// <summary><see cref="RemoveRange"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="RemoveRange" path="param|exception"/>
    //     public void ValidateRemoveRange(int index, int count) => Items.ValidateRemoveRange(index, count);
    //
    //     /// <summary><see cref="AdjustLength"/>,<see cref="AdjustLengthIfShort"/>,<see cref="AdjustLengthIfLong"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="AdjustLength" path="param|exception"/>
    //     public void ValidateAdjustLength(int length) => Items.ValidateAdjustLength(length);
    //
    //     /// <summary><see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|exception"/>
    //     public new void ValidateReset(IEnumerable<IStubModelSettings> settings) => Items.ValidateReset(settings);
    //
    //     /// <summary><see cref="Clear"/> メソッドの検証処理。</summary>
    //     /// <inheritdoc cref="Clear" path="param|exception"/>
    //     public void ValidateClear() => Items.ValidateClear();
    //
    //     /// <summary><see cref="Add"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Add" path="param|returns"/>
    //     public WodiLib.Test.Tools.StubModel AddInternal(IStubModelSettings settings) => Items.AddInternal(settings);
    //
    //     /// <summary><see cref="AddRange"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="AddRange" path="param|returns"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> AddRangeInternal(
    //         IEnumerable<IStubModelSettings> settings
    //     ) => Items.AddRangeInternal(settings);
    //
    //     /// <summary><see cref="Insert"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Insert" path="param|returns"/>
    //     public WodiLib.Test.Tools.StubModel InsertInternal(int index, IStubModelSettings settings) => Items.InsertInternal(index, settings);
    //
    //     /// <summary><see cref="InsertRange"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="InsertRange" path="param|returns"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> InsertRangeInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.InsertRangeInternal(index, settings);
    //
    //     /// <summary><see cref="Overwrite"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Overwrite" path="param|returns"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> OverwriteInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.OverwriteInternal(index, settings);
    //
    //     /// <summary><see cref="Remove"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Remove" path="param|returns"/>
    //     public WodiLib.Test.Tools.StubModel RemoveInternal(int index) => Items.RemoveInternal(index);
    //
    //     /// <summary><see cref="RemoveRange"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="RemoveRange" path="param|returns"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> RemoveRangeInternal(int index, int count) => Items.RemoveRangeInternal(index, count);
    //
    //     /// <summary><see cref="AdjustLength"/>,<see cref="AdjustLengthIfShort"/>,<see cref="AdjustLengthIfLong"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="AdjustLength" path="param|returns"/>
    //     public IEnumerable<WodiLib.Test.Tools.StubModel> AdjustLengthInternal(int length) => Items.AdjustLengthInternal(length);
    //
    //     /// <summary><see cref="Clear"/> メソッド処理中核。</summary>
    //     /// <inheritdoc cref="Clear" path="param"/>
    //     public void ClearInternal() => Items.ClearInternal();
    //
    //     /// <inheritdoc/>
    //     public new StubRestrictedCapacityList DeepClone() => new(this);
    //     object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();
    // }
}
