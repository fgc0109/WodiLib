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
    // public interface IStubFixedLengthListSettings : IEqualityComparable<IStubFixedLengthListSettings>
    // {
    //     /// <inheritdoc cref="StubFixedLengthList.Settings"/>
    //     public IReadOnlyList<IStubModelSettings> Settings { get; }
    //
    //     /// <inheritdoc cref="StubFixedLengthList.Tags"/>
    //     public IReadOnlyList<string> Tags { get; }
    // }
    //
    // public record StubFixedLengthListSettings(
    //     /*
    //      * 行データの入力パラメータは必ずコンストラクタで受け取るようになる。
    //      */
    //     IReadOnlyList<IStubModelSettings> Settings
    // ) : IStubFixedLengthListSettings
    // {
    //     /// <inheritdoc cref="StubFixedLengthList.Tags"/>
    //     public IReadOnlyList<string> Tags { get; set; } = new List<string>();
    //
    //     public bool ItemEquals(IStubFixedLengthListSettings? other)
    //     {
    //         return other is not null
    //                && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
    //                && Tags.SequenceEqual(other.Tags);
    //     }
    //
    //     public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLengthListSettings);
    // }

    [FixedLengthListImplementTemplate(
        Description = "<see cref=\"StubFixedLengthList\"/> スタブ用",
        ElementType = typeof(StubModel),
        ReadOnlyElementType = typeof(ReadOnlyStubModel),
        SettingsType = typeof(IStubModelSettings),
        MaxCapacity = 5, // MaxCapacity == MinCapacity としているが、 MaxCapacity > MinCapacity としても良い。
        MinCapacity = 5 // その場合でも通常の方法ではインスタンス作成後にサイズを変えることはできない。
    )]
    public partial class ReadOnlyStubFixedLengthList
        /*
         * 以下7つの基底クラス・インタフェースは SourceGenerator が自動的に付与する。
         */
        // ModelBase,
        // IReadOnlyList<ReadOnlyStubModel>,
        // IStubFixedLengthListSettings,
        // IEqualityComparable<ReadOnlyStubFixedLengthList>,
        // IEqualityComparable<StubFixedLengthList>,
        // IDeepCloneable<ReadOnlyStubFixedLengthList>
    {
        [SettingsProperty(
            DefaultValue = "new List<string>()"
        )]
        [FixedLengthListProperty(
            Accessibility = "NONE",
            ReturnType = typeof(IList<string>)
        )]
        public IReadOnlyList<string> Tags => tags;

        private readonly List<string> tags = new();

        /*
         * 作成するコンストラクタは必ず this(SimpleList{T}) を呼び出す。
         */
        [FixedLengthListConstructor]
        public ReadOnlyStubFixedLengthList(
            int length
        ) : this(BuildSimpleList(length))
        {
        }

        public ReadOnlyStubFixedLengthList(IStubFixedLengthListSettings settings)
            : this(BuildSimpleList(settings.Settings))
        {
            tags = settings.Tags.ToList();
        }

        /*
         * 純粋メソッド。
         * 編集可能モデルクラスでも通常使用可能なため、属性はつけない。
         */
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
                minCapacityGetter: GetCapacity,
                maxCapacityGetter: GetCapacity
            );
        }
    }

    /*
     * 以下は SourceGeneratorで生成されるクラス定義のサンプル。
     */
    // public partial class ReadOnlyStubFixedLengthList : ModelBase,
    //     IStubFixedLengthListSettings,
    //     IReadOnlyList<ReadOnlyStubModel>,
    //     IEqualityComparable<ReadOnlyStubFixedLengthList>,
    //     IEqualityComparable<StubFixedLengthList>,
    //     IDeepCloneable<ReadOnlyStubFixedLengthList>
    // {
    //
    //     #region Constants
    //
    //     public static int Capacity => 5;
    //
    //     #endregion
    //
    //     #region Properties
    //
    //     #region public
    //
    //     /// <summary>
    //     ///     インデクサによるアクセス
    //     /// </summary>
    //     /// <param name="index">
    //     ///     [Range(0, <see cref="Count"/> - 1)] インデックス
    //     /// </param>
    //     /// <returns>指定したインデックスの要素</returns>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     public ReadOnlyStubModel this[int index] => Get(index);
    //
    //     /// <summary>要素数</summary>
    //     public int Count => Items.Count;
    //
    //     /// <inheritdoc/>
    //     public IReadOnlyList<IStubModelSettings> Settings => Items.Cast<IStubModelSettings>().ToList();
    //
    //     #endregion
    //
    //     #region private protected
    //
    //     private protected ExtendedList<StubModel, ReadOnlyStubModel, IStubModelSettings> Items { get; }
    //
    //     #endregion
    //
    //     #endregion
    //
    //     #region Constructors
    //
    //     private protected ReadOnlyStubFixedLengthList(SimpleList<StubModel> itemsImpl)
    //     {
    //         Items = new ExtendedList<StubModel, ReadOnlyStubModel, IStubModelSettings>(
    //             itemsImpl,
    //             minCapacity: MinCapacity,
    //             maxCapacity: MaxCapacity,
    //             validator: BuildValidator(itemsImpl),
    //             buildItemFromSettings: BuildItemFromSettings
    //         );
    //     }
    //     #endregion
    //
    //     /// <inheritdoc/>
    //     public IEnumerator<ReadOnlyStubModel> GetEnumerator() => Items.GetEnumerator();
    //     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    //
    //     /// <summary>
    //     ///     指定インデックスの要素を取得する。
    //     /// </summary>
    //     /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
    //     /// <returns>指定範囲の要素簡易コピーリスト</returns>
    //     /// <exception cref="ArgumentOutOfRangeException">
    //     ///     <paramref name="index"/> が指定範囲外の場合。
    //     /// </exception>
    //     public ReadOnlyStubModel Get(int index) => Items.Get(index);
    //
    //     /// <summary>
    //     ///     指定範囲の要素を簡易コピーしたリストを取得する。
    //     /// </summary>
    //     /// <param name="index">[Range(0, <see cref="Count"/> - 1)] インデックス</param>
    //     /// <param name="count">[Range(0, <see cref="Count"/>)] 要素数</param>
    //     /// <returns>指定範囲の要素簡易コピーリスト</returns>
    //     /// <exception cref="ArgumentOutOfRangeException">
    //     ///     <paramref name="index"/>, <paramref name="count"/>が指定範囲外の場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を取得しようとした場合。</exception>
    //     public IEnumerable<ReadOnlyStubModel> GetRange(int index, int count) => Items.GetRange(index, count);
    //
    //     /// <summary>
    //     ///     <see cref="Get"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="Get" path="param|exception"/>
    //     public void ValidateGet(int index) => Items.ValidateGet(index);
    //
    //     /// <summary>
    //     ///     <see cref="GetRange"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="GetRange" path="param|exception"/>
    //     public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);
    //
    //     /// <summary>
    //     ///     <see cref="Get"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="Get" path="param"/>
    //     public ReadOnlyStubModel GetInternal(int index) => Items.GetInternal(index);
    //
    //     /// <summary>
    //     ///     <see cref="GetRange"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="GetRange" path="param"/>
    //     public IEnumerable<ReadOnlyStubModel> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);
    //
    //     /// <inheritdoc/>
    //     public bool ItemEquals(ReadOnlyStubFixedLengthList? other) => ItemEquals(other as IStubFixedLengthListSettings);
    //     /// <inheritdoc/>
    //     public bool ItemEquals(StubFixedLengthList? other) => ItemEquals(other as IStubFixedLengthListSettings);
    //     /// <inheritdoc/>
    //     public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLengthListSettings);
    //
    //     /// <inheritdoc/>
    //     public ReadOnlyStubFixedLengthList DeepClone() => new(this);
    //     object IDeepCloneable.DeepClone() => DeepClone();
    // }
    //
    // public partial class StubFixedLengthList : ReadOnlyStubFixedLengthList,
    //     IDeepCloneable<StubFixedLengthList>
    // {
    //     /// <summary>
    //     ///     インデクサによるアクセス
    //     /// </summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] インデックス</param>
    //     /// <returns>指定したインデックスの要素</returns>
    //     /// <exception cref="ArgumentNullException"><see lanword="null"/> をセットしようとした場合。</exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     public new StubModel this[int index]
    //     {
    //         get => Get(index);
    //         set => Set(index, value);
    //     }
    //
    //     public StubFixedLengthList(
    //         int length
    //     ) : this(BuildSimpleList(length))
    //     {
    //     }
    //
    //     public StubFixedLengthList(IIStubFixedLengthListSettings settings) : base(settings) { }
    //
    //     private protected StubFixedLengthList(SimpleList<StubModel> itemsImpl) : base(itemsImpl) { }
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.Get"/>
    //     public new StubModel Get(int index) => Items.Get(index);
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TElement}.GetRange"/>
    //     public new IEnumerable<StubModel> GetRange(int index, int count) => Items.GetRange(index, count);
    //
    //     /// <summary>
    //     ///     リストの要素を更新する。
    //     /// </summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 更新開始インデックス</param>
    //     /// <param name="settings">更新要素</param>
    //     /// <returns>セットした要素</returns>
    //     /// <exception cref="ArgumentNullException">
    //     ///     <paramref name="settings"/> が <see langword="null"/> の場合、
    //     ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">
    //     ///     有効な範囲外の要素を編集しようとした場合。
    //     /// </exception>
    //     public StubModel Set(int index, IStubModelSettings settings) => Items.Set(index, settings);
    //
    //     /// <summary>
    //     ///     リストの連続した要素を更新する。
    //     /// </summary>
    //     /// <param name="index">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 更新開始インデックス</param>
    //     /// <param name="settings">更新要素</param>
    //     /// <returns>セットした要素</returns>
    //     /// <exception cref="ArgumentNullException">
    //     ///     <paramref name="settings"/> が <see langword="null"/> の場合、
    //     ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>が指定範囲外の場合。</exception>
    //     /// <exception cref="ArgumentException">
    //     ///     有効な範囲外の要素を編集しようとした場合。
    //     /// </exception>
    //     public IEnumerable<StubModel> SetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRange(index, settings);
    //
    //     /// <summary>
    //     ///     指定したインデックスにある項目をコレクション内の新しい場所へ移動する。
    //     /// </summary>
    //     /// <param name="oldIndex">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 移動する項目のインデックス</param>
    //     /// <param name="newIndex">[Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)] 移動先のインデックス</param>
    //     /// <exception cref="InvalidOperationException">
    //     ///     自身の要素数が0の場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentOutOfRangeException">
    //     ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/> が指定範囲外の場合。
    //     /// </exception>
    //     public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);
    //
    //     /// <summary>
    //     ///     指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
    //     /// </summary>
    //     /// <param name="oldIndex">
    //     ///     [Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)]
    //     ///     移動する項目のインデックス開始位置
    //     /// </param>
    //     /// <param name="newIndex">
    //     ///     [Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/> - 1)]
    //     ///     移動先のインデックス開始位置
    //     /// </param>
    //     /// <param name="count">
    //     ///     [Range(0, <see cref="ReadOnlyStubFixedLengthList.Count"/>)]
    //     ///     移動させる要素数
    //     /// </param>
    //     /// <exception cref="InvalidOperationException">
    //     ///     自身の要素数が0の場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentOutOfRangeException">
    //     ///     <paramref name="oldIndex"/>, <paramref name="newIndex"/>, <paramref name="count"/> が指定範囲外の場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentException">有効な範囲外の要素を移動しようとした場合。</exception>
    //     public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, count);
    //
    //     /// <summary>
    //     ///     要素を与えられた内容で一新する。
    //     /// </summary>
    //     /// <param name="settings">リストに詰め直す要素</param>
    //     /// <returns>新たにリストに詰め直した要素</returns>
    //     /// <exception cref="ArgumentNullException">
    //     ///     <paramref name="settings"/> が <see langword="null"/> の場合、
    //     ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
    //     /// </exception>
    //     /// <exception cref="ArgumentException">
    //     ///     <paramref name="settings"/> の要素数が <see cref="ReadOnlyStubFixedLengthList.Count"/> と
    //     ///     異なる場合。
    //     /// </exception>
    //     public IEnumerable<StubModel> Reset(IEnumerable<IStubModelSettings> settings) => (FixedLengthList<StubModel, ReadOnlyStubModel, IStubModelSettings>)Items.Reset(settings);
    //
    //     /// <summary>
    //     ///     要素をデフォルト値で一新する。
    //     /// </summary>
    //     public IEnumerable<StubModel> Reset() => Items.Reset();
    //
    //     /// <summary>
    //     ///     <see cref="Set"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="Set" path="param|exception"/>
    //     public void ValidateSet(int index, IStubModelSettings settings) => Items.ValidateSet(index, settings);
    //
    //     /// <summary>
    //     ///     <see cref="SetRange"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="SetRange" path="param|exception"/>
    //     public void ValidateSetRange(int index, IEnumerable<IStubModelSettings> settings) => Items.ValidateSetRange(index, settings);
    //
    //     /// <summary>
    //     ///     <see cref="Move"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="Move" path="param|exception"/>
    //     public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);
    //
    //     /// <summary>
    //     ///     <see cref="MoveRange"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="MoveRange" path="param|exception"/>
    //     public void ValidateMoveRange(int oldIndex, int newIndex, int count) => Items.ValidateMoveRange(oldIndex, newIndex, count);
    //
    //     /// <summary>
    //     ///     <see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/> メソッドの検証処理。
    //     /// </summary>
    //     /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param|exception"/>
    //     public void ValidateReset(IEnumerable<IStubModelSettings> settings) => Items.ValidateReset(settings);
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetInternal"/>
    //     public new StubModel GetInternal(int index) => Items.GetInternal(index);
    //
    //     /// <inheritdoc cref="IReadOnlyExtendedList{TReadOnlyElement}.GetRangeInternal"/>
    //     public new IEnumerable<StubModel> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);
    //
    //     /// <summary>
    //     ///     <see cref="Set"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="Set" path="param"/>
    //     public StubModel SetInternal(int index, IStubModelSettings settings) => Items.SetInternal(index, settings);
    //
    //     /// <summary>
    //     ///     <see cref="SetRange"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="SetRange" path="param"/>
    //     public IEnumerable<StubModel> SetRangeInternal(int index, IEnumerable<IStubModelSettings> settings) => Items.SetRangeInternal(index, settings);
    //
    //     /// <summary>
    //     ///     <see cref="Move"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="Move" path="param"/>
    //     public void MoveInternal(int oldIndex, int newIndex) => Items.MoveInternal(oldIndex, newIndex);
    //
    //     /// <summary>
    //     ///     <see cref="MoveRange"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="MoveRange" path="param"/>
    //     public void MoveRangeInternal(int oldIndex, int newIndex, int count) => Items.MoveRangeInternal(oldIndex, newIndex, count);
    //
    //     /// <summary>
    //     ///     <see cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})"/>,
    //     ///     <see cref="Reset()"/> メソッド処理中核。
    //     /// </summary>
    //     /// <inheritdoc cref="Reset(System.Collections.Generic.IEnumerable{IStubModelSettings})" path="param"/>
    //     public IEnumerable<StubModel> ResetInternal(IEnumerable<IStubModelSettings> settings) => Items.ResetInternal(settings);
    // }
}
