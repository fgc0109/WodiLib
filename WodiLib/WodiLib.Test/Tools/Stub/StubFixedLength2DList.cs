// ========================================
// Project Name : WodiLib.Test
// File Name    : StubFixedLength2DList.cs
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
     * 基本的な解説は StubModel.cs を参照。
     */


    /*
     * IStubFixedLength2DListSettings は SourceGenerator で自動生成されるため、
     * 手作業での作成不要。
     *
     * 以下、自動生成される設定DTOインタフェース
     */
    /*
    /// <summary>
    ///     <see cref="StubFixedLength2DList"/> スタブ用設定インタフェース
    /// </summary>
    public partial interface IStubFixedLength2DListSettings : WodiLib.Sys.IEqualityComparable<IStubFixedLength2DListSettings>, IListSettings<IStubFixedLengthListSettings>
    {
        /// <inheritdoc cref="StubFixedLength2DList.Tags" />
        System.Collections.Generic.IReadOnlyList<System.String> Tags { get; }
    }
   */

    /*
    /// <summary>
    ///     <see cref="StubFixedLength2DList"/> スタブ用設定DTO
    /// </summary>
    public partial record StubFixedLength2DListSettings(IList<IStubFixedLengthListSettings> Settings) : IStubFixedLength2DListSettings
    {
        /// <inheritdoc cref="IStubFixedLength2DListSettings.Tags" path="summary|remarks" />
        public System.Collections.Generic.IReadOnlyList<System.String> Tags { get; set; } = new List<string>();

        /// <inheritdoc/>
        public bool ItemEquals(IStubFixedLength2DListSettings? other)
        {
            return other is not null
                   && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
                   && Tags.SequenceEqual(other.Tags);
        }

        /// <inheritdoc/>
        public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLength2DListSettings);
    }
    */

    [FixedLength2DListImplementTemplate(
        Description = "<see cref=\"StubFixedLength2DList\"/> スタブ用",
        RowElementType = typeof(StubFixedLengthList),
        ReadOnlyRowElementType = typeof(ReadOnlyStubFixedLengthList),
        CellElementType = typeof(StubModel),
        ReadOnlyCellElementType = typeof(ReadOnlyStubModel),
        RowSettingsType = typeof(IStubFixedLengthListSettings),
        CellSettingsType = typeof(IStubModelSettings),
        MaxRowCapacity = 10,
        MinRowCapacity = 1,
        MaxColumnCapacity = 9,
        MinColumnCapacity = 0,
        RowPhysicalName = "X",
        RowLogicalName = "Y座標",
        ColumnPhysicalName = "Y",
        ColumnLogicalName = "Y座標",
        CellPhysicalName = "Point",
        CellLogicalName = "座標"
    )]
    public partial class StubFixedLength2DList
    {
        [SettingsProperty(
            ReturnType = typeof(IReadOnlyList<string>),
            DefaultValue = "new List<string>()"
        )]
        [ImmutableProperty(
            ReturnType = typeof(IReadOnlyList<string>),
            Accessibility = "public"
        )]
        public IReadOnlyList<string> Tags => tags;

        private readonly List<string> tags = new();

        public StubFixedLength2DList(IStubFixedLength2DListSettings settings)
            : this(settings, BuildSimpleList(settings.Settings))
        {
            tags = settings.Tags.ToList();
        }

        public StubFixedLength2DList(
            int xSize,
            int ySize
        ) : this(
            new StubFixedLength2DListSettings(
                xSize.Iterate<IStubFixedLengthListSettings>(x => BuildRowSettingsFromRowIndex(x, ySize, null!)).ToList()
            )
        )
        {
        }

        [ImmutableMethod]
        public string ToJsonString()
        {
            // メソッド定義が参照できることのテストをするためだけのメソッドなので、
            // 戻り値は適当な値とする
            return "JSON RESULT";
        }

        public void SetNowStringValue()
        {
            Table.ForEach(row
                => row.ForEach(cell => cell.SetNowStringValue())
            );
        }

        public bool ItemEquals(IStubFixedLength2DListSettings? other)
        {
            return other is not null
                   && Settings.SequenceEqual(other.Settings, (left, right) => left.ItemEquals(right))
                   && Tags.SequenceEqual(other.Tags);
        }

        private protected static SimpleList<StubFixedLengthList> BuildSimpleList(int rowLength, int columnLength)
        {
            return new SimpleList<StubFixedLengthList>(
                RowBuilder,
                rowLength.Iterate(rowIndex => BuildItemFromIndex(rowIndex, columnLength, null!))
            );
        }

        private protected static SimpleList<StubFixedLengthList> BuildSimpleList(
            IEnumerable<IStubFixedLengthListSettings> settings
        )
        {
            return new SimpleList<StubFixedLengthList>(
                RowBuilder,
                settings.Select(setting => new StubFixedLengthList(setting))
            );
        }

        private protected static SimpleListValueBuilder<StubFixedLengthList> RowBuilder { get; }
            = new((list, index) => BuildItemFromIndex(index, list.Count, list));

        private protected static StubFixedLengthList BuildItemFromIndex(
            int rowIndex,
            int columnLength,
            SimpleList<StubFixedLengthList> list
        )
            => new(BuildRowSettingsFromRowIndex(rowIndex, columnLength, list));

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する Config のコンストラクタ引数として指定する。
         */
        private protected static StubFixedLengthListSettings BuildRowSettingsFromRowIndex(
            int rowIndex,
            int columnLength,
            SimpleList<StubFixedLengthList> list
        )
            => new(
                columnLength.Iterate<IStubModelSettings>(columnIndex => new StubModelSettings
                        { StringValue = $"{rowIndex}_{columnIndex}" }
                    )
                    .ToList()
            );

        private protected static StubFixedLengthList BuildRowFromSettings(
            int rowIndex,
            IStubFixedLengthListSettings settings
        )
            => new(settings);

        private protected static StubModel BuildListElementFromSetting(IStubModelSettings settings) => new(settings);

        private protected IWodiLib2DListValidator<IStubFixedLength2DListSettings, IStubFixedLengthListSettings,
                IStubModelSettings>
            BuildValidator(
                IStubFixedLength2DListSettings settings,
                SimpleList<StubFixedLengthList> itemsImpl
            )
        {
            return new RestrictedCapacity2DListValidator<IStubFixedLength2DListSettings, IStubFixedLengthListSettings,
                IStubModelSettings>(
                rowCountGetter: () => itemsImpl.Count,
                columnCountGetter: () => itemsImpl.Count == 0
                    ? 0
                    : itemsImpl[0].Count,
                minRowCapacityGetter: () => MinXCapacity,
                maxRowCapacityGetter: () => MaxXCapacity,
                minColumnCapacityGetter: () => MinYCapacity,
                maxColumnCapacityGetter: () => MaxYCapacity
            );
        }
    }

    /*
    /// <summary>
    ///     <see cref="StubFixedLength2DList"/> スタブ用
    /// </summary>
    public partial class StubFixedLength2DList : ModelBase,
        IStubFixedLength2DListSettings,
        IEnumerable<WodiLib.Test.Tools.StubFixedLengthList>,
        INotifyCollectionChanged,
        IEqualityComparable<StubFixedLength2DList>,
        IEqualityComparable<ReadOnlyStubFixedLength2DList>,
        WodiLib.Sys.IDeepCloneable<StubFixedLength2DList>
    {
        /// <summary>Y座標容量最大値</summary>
        [Pure]
        public static int MaxXCapacity => 10;
        /// <summary>Y座標容量最小値</summary>
        [Pure]
        public static int MinXCapacity => 1;
        /// <summary>Y座標容量最大値</summary>
        [Pure]
        public static int MaxYCapacity => 9;
        /// <summary>Y座標容量最小値</summary>
        [Pure]
        public static int MinYCapacity => 0;
 /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        /// <summary>
        ///     Y座標インデクサによるアクセス
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標インデックスのY座標要素（長さ固定型）</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>が指定範囲外の場合。</exception>
        [Pure]
        public WodiLib.Test.Tools.StubFixedLengthList this[int xIndex]
            => GetX(xIndex);

        /// <summary>
        ///     座標インデクサによるアクセス
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標・Y座標インデックスの座標要素</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>, <paramref name="yIndex"/>が指定範囲外の場合。</exception>
        public WodiLib.Test.Tools.StubModel this[int xIndex, int yIndex]
        {
            [Pure]
            get => GetPoint(xIndex, yIndex);
            set => SetPoint(xIndex, yIndex, value);
        }

        /// <summary>Y座標数</summary>
        [Pure]
        public int XCount => Table.RowCount;

        /// <summary>Y座標数</summary>
        [Pure]
        public int YCount => Table.ColumnCount;

        /// <inheritdoc/>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Pure]
        public IList<IStubFixedLengthListSettings> Settings => Table.Select(row => (IStubFixedLengthListSettings)row).ToArray();

        private protected TwoDimensionalList<IStubFixedLength2DListSettings, WodiLib.Test.Tools.StubFixedLengthList, WodiLib.Test.Tools.StubFixedLengthList, ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings> Table { get; }

        private protected StubFixedLength2DList(IStubFixedLength2DListSettings settings, SimpleList<WodiLib.Test.Tools.StubFixedLengthList> itemsImpl)
        {
            var validator = BuildValidator(settings, itemsImpl);
            validator?.Constructor((nameof(settings), settings));
            Table =
                new TwoDimensionalList<IStubFixedLength2DListSettings, WodiLib.Test.Tools.StubFixedLengthList, WodiLib.Test.Tools.StubFixedLengthList, ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings>(
                    itemsImpl,
                    new TwoDimensionalList<IStubFixedLength2DListSettings, WodiLib.Test.Tools.StubFixedLengthList, WodiLib.Test.Tools.StubFixedLengthList, ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings>.Config(
                        BuildRowSettingsFromRowIndex,
                        BuildRowFromSettings,
                        BuildListElementFromSetting,
                        BuildValidator(settings, itemsImpl)
                    )
                    {
                        MaxRowCapacity = MaxXCapacity,
                        MinRowCapacity = MinXCapacity,
                        MaxColumnCapacity = MaxYCapacity,
                        MinColumnCapacity = MinYCapacity,
                    }
                );
            PropagatePropertyChangeEvent(Table);
            PropagateCollectionChangeEvent(Table);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerator<WodiLib.Test.Tools.StubFixedLengthList> GetEnumerator() => Table.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>Y座標容量最大値を取得する。</summary>
        /// <returns>Y座標容量最大値</returns>
        [Pure]
        public int GetMaxXCapacity() => MaxXCapacity;
        /// <summary>Y座標容量最小値を取得する。</summary>
        /// <returns>Y座標容量最小値</returns>
        [Pure]
        public int GetMinXCapacity() => MinXCapacity;
        /// <summary>Y座標容量最大値を取得する。</summary>
        /// <returns>Y座標容量最大値</returns>
        [Pure]
        public int GetMaxYCapacity() => MaxYCapacity;
        /// <summary>Y座標容量最小値を取得する。</summary>
        /// <returns>Y座標容量最小値</returns>
        [Pure]
        public int GetMinYCapacity() => MinYCapacity;

        /// <summary>
        ///     指定Y座標インデックスのY座標要素を取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定行のY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        public WodiLib.Test.Tools.StubFixedLengthList GetX(int xIndex) => Table.GetRow(xIndex);

        /// <summary>
        ///     指定範囲のY座標要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="XCount"/>)] Y座標数</param>
        /// <returns>指定範囲のY座標要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> GetXRange(int xIndex, int count)
            => Table.GetRowRange(xIndex, count);

        /// <summary>
        ///     指定Y座標インデックスのY座標要素を取得する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定Y座標の要素リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubModel> GetY(int yIndex) => Table.GetColumn(yIndex);

        /// <summary>
        ///     指定範囲のY座標要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="YCount"/>)] Y座標数</param>
        /// <returns>指定範囲のY座標要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> GetYRange(int yIndex, int count)
            => Table.GetColumnRange(yIndex, count);

        /// <summary>
        ///     指定Y座標・Y座標インデックスの座標要素を取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定Y座標・Y座標の座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        public WodiLib.Test.Tools.StubModel GetPoint(int xIndex, int yIndex) => Table.GetCell(xIndex, yIndex);

        /// <summary>
        ///     二次元リストのY座標要素を更新する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] 更新Y座標インデックス</param>
        /// <param name="settings">更新Y座標要素</param>
        /// <returns>セットしたY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のY座標要素を編集しようとした場合。
        /// </exception>
        public WodiLib.Test.Tools.StubFixedLengthList SetX(int xIndex, IStubFixedLengthListSettings settings)
            => Table.SetRow(xIndex, settings);

        /// <summary>
        ///     二次元リストの連続したY座標要素を更新する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] 更新開始Y座標インデックス</param>
        /// <param name="settings">更新Y座標要素</param>
        /// <returns>セットしたY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のY座標要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> SetXRange(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.SetRowRange(xIndex, settings);

        /// <summary>
        ///     二次元リストのY座標要素を更新する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)] 更新Y座標インデックス</param>
        /// <param name="settings">更新Y座標要素</param>
        /// <returns>セットしたY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="yIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のY座標要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetY(int yIndex, IEnumerable<IStubModelSettings> settings)
            => Table.SetColumn(yIndex, settings);

        /// <summary>
        ///     二次元リストの連続したY座標要素を更新する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)] 更新開始Y座標インデックス</param>
        /// <param name="settings">更新Y座標要素（外側のIEnumerableがY座標、内側のIEnumerableが各Y座標のY座標要素）</param>
        /// <returns>セットしたY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="yIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のY座標要素を編集しようとした場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> SetYRange(
            int yIndex,
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.SetColumnRange(yIndex, settings);

        /// <summary>
        ///     二次元リストの座標要素を更新する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)] Y座標インデックス</param>
        /// <param name="settings">更新座標要素</param>
        /// <returns>セットした座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>, <paramref name="yIndex"/>が指定範囲外の場合。</exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外の座標要素を編集しようとした場合。
        /// </exception>
        public WodiLib.Test.Tools.StubModel SetPoint(int xIndex, int yIndex, IStubModelSettings settings)
            => Table.SetCell(xIndex, yIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldXIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] 移動するY座標のインデックス</param>
        /// <param name="newXIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] 移動先のY座標インデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身のY座標数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldXIndex"/>, <paramref name="newXIndex"/> が指定範囲外の場合。
        /// </exception>
        public void MoveX(int oldXIndex, int newXIndex) => Table.MoveRow(oldXIndex, newXIndex);

        /// <summary>
        ///     指定したY座標インデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldXIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)]
        ///     移動するY座標のインデックス開始位置
        /// </param>
        /// <param name="newXIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)]
        ///     移動先のY座標インデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/>)]
        ///     移動させるY座標数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身のY座標数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldXIndex"/>, <paramref name="newXIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を移動しようとした場合。</exception>
        public void MoveXRange(int oldXIndex, int newXIndex, int count)
            => Table.MoveRowRange(oldXIndex, newXIndex, count);

        /// <summary>
        ///     指定したY座標インデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldYIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)]
        ///     移動するY座標のインデックス
        /// </param>
        /// <param name="newYIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)] 移動先のY座標インデックス</param>
        /// <exception cref="InvalidOperationException">
        ///     自身のY座標数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldYIndex"/>, <paramref name="newYIndex"/> が指定範囲外の場合。
        /// </exception>
        public void MoveY(int oldYIndex, int newYIndex)
            => Table.MoveColumn(oldYIndex, newYIndex);

        /// <summary>
        ///     指定したY座標インデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldYIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)]
        ///     移動するY座標のインデックス開始位置
        /// </param>
        /// <param name="newYIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/> - 1)]
        ///     移動先のY座標インデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.YCount"/>)]
        ///     移動させるY座標数
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     自身のY座標数が0の場合。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="oldYIndex"/>, <paramref name="newYIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を移動しようとした場合。</exception>
        public void MoveYRange(int oldYIndex, int newYIndex, int count)
            => Table.MoveColumnRange(oldYIndex, newYIndex, count);

        /// <summary>
        ///     要素を与えられた内容で一新する。
        /// </summary>
        /// <param name="settings">二次元リストに詰め直す要素</param>
        /// <returns>新たに二次元リストに詰め直した要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="settings"/> のY座標数が <see cref="ReadOnlyStubFixedLength2DList.XCount"/>、
        ///     Y座標数が <see cref="ReadOnlyStubFixedLength2DList.YCount"/> と異なる場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> Reset(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.Reset(settings);

        /// <summary>
        ///     要素をデフォルト値で一新する。
        /// </summary>
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> Reset() => Table.Reset();

        /// <summary>
        ///     <see cref="GetX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetX" path="param|exception"/>
        [Pure]
        public void ValidateGetX(int xIndex) => Table.ValidateGetRow(xIndex);

        /// <summary>
        ///     <see cref="GetXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetXRange" path="param|exception"/>
        [Pure]
        public void ValidateGetXRange(int xIndex, int count) => Table.ValidateGetRowRange(xIndex, count);

        /// <summary>
        ///     <see cref="GetY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetY" path="param|exception"/>
        [Pure]
        public void ValidateGetY(int yIndex) => Table.ValidateGetColumn(yIndex);

        /// <summary>
        ///     <see cref="GetYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetYRange" path="param|exception"/>
        [Pure]
        public void ValidateGetYRange(int yIndex, int count)
            => Table.ValidateGetColumnRange(yIndex, count);

        /// <summary>
        ///     <see cref="GetPoint"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetPoint" path="param|exception"/>
        [Pure]
        public void ValidateGetPoint(int xIndex, int yIndex) => Table.ValidateGetCell(xIndex, yIndex);

        /// <summary>
        ///     <see cref="SetX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetX" path="param|exception"/>
        [Pure]
        public void ValidateSetX(int xIndex, IStubFixedLengthListSettings settings)
            => Table.ValidateSetRow(xIndex, settings);

        /// <summary>
        ///     <see cref="SetXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetXRange" path="param|exception"/>
        [Pure]
        public void ValidateSetXRange(int xIndex, IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateSetRowRange(xIndex, settings);

        /// <summary>
        ///     <see cref="SetY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetY" path="param|exception"/>
        [Pure]
        public void ValidateSetY(int yIndex, IEnumerable<IStubModelSettings> settings)
            => Table.ValidateSetColumn(yIndex, settings);

        /// <summary>
        ///     <see cref="SetYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetYRange" path="param|exception"/>
        [Pure]
        public void ValidateSetYRange(int yIndex, IEnumerable<IEnumerable<IStubModelSettings>> settings)
            => Table.ValidateSetColumnRange(yIndex, settings);

        /// <summary>
        ///     <see cref="SetPoint"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetPoint" path="param|exception"/>
        [Pure]
        public void ValidateSetPoint(int xIndex, int yIndex, IStubModelSettings settings)
            => Table.ValidateSetCell(xIndex, yIndex, settings);

        /// <summary>
        ///     <see cref="MoveX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveX" path="param|exception"/>
        [Pure]
        public void ValidateMoveX(int oldXIndex, int newXIndex)
            => Table.ValidateMoveRow(oldXIndex, newXIndex);

        /// <summary>
        ///     <see cref="MoveXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveXRange" path="param|exception"/>
        [Pure]
        public void ValidateMoveXRange(int oldXIndex, int newXIndex, int count)
            => Table.ValidateMoveRowRange(oldXIndex, newXIndex, count);

        /// <summary>
        ///     <see cref="MoveY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveY" path="param|exception"/>
        [Pure]
        public void ValidateMoveY(int oldYIndex, int newYIndex)
            => Table.ValidateMoveColumn(oldYIndex, newYIndex);

        /// <summary>
        ///     <see cref="MoveYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveYRange" path="param|exception"/>
        [Pure]
        public void ValidateMoveYRange(int oldYIndex, int newYIndex, int count)
            => Table.ValidateMoveColumnRange(oldYIndex, newYIndex, count);

        /// <summary>
        ///     <see
        ///         cref="Reset(System.Collections.Generic.IEnumerable{IStubFixedLengthListSettings})"/>
        ///     メソッドの検証処理。
        /// </summary>
        /// <inheritdoc
        ///     cref="Reset(System.Collections.Generic.IEnumerable{IStubFixedLengthListSettings})"
        ///     path="param|exception"/>
        [Pure]
        public void ValidateReset(IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateReset(settings);

        /// <summary>
        ///     <see cref="Reset()"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Reset()" path="param|exception"/>
        [Pure]
        public void ValidateReset() => Table.ValidateReset();

        /// <summary>
        ///     <see cref="GetX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetX" path="param"/>
        [Pure]
        public WodiLib.Test.Tools.StubFixedLengthList GetXInternal(int xIndex) => Table.GetRowInternal(xIndex);

        /// <summary>
        ///     <see cref="GetXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetXRange" path="param"/>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> GetXRangeInternal(int xIndex, int count)
            => Table.GetRowRangeInternal(xIndex, count);

        /// <summary>
        ///     <see cref="GetY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetY" path="param"/>
        [Pure]
        public IEnumerable<WodiLib.Test.Tools.StubModel> GetYInternal(int yIndex)
            => Table.GetColumnInternal(yIndex);

        /// <summary>
        ///     <see cref="GetYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetYRange" path="param"/>
        [Pure]
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> GetYRangeInternal(int yIndex, int count)
            => Table.GetColumnRangeInternal(yIndex, count);

        /// <summary>
        ///     <see cref="GetPoint"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetPoint" path="param"/>
        [Pure]
        public WodiLib.Test.Tools.StubModel GetPointInternal(int xIndex, int yIndex)
            => Table.GetCellInternal(xIndex, yIndex);

        /// <summary>
        ///     <see cref="SetX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetX" path="param"/>
        public WodiLib.Test.Tools.StubFixedLengthList SetXInternal(
            int xIndex,
            IStubFixedLengthListSettings settings
        ) => Table.SetRowInternal(xIndex, settings);

        /// <summary>
        ///     <see cref="SetXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetXRange" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> SetXRangeInternal(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.SetRowRangeInternal(xIndex, settings);

        /// <summary>
        ///     <see cref="SetY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetY" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> SetYInternal(
            int yIndex,
            IEnumerable<IStubModelSettings> settings
        ) => Table.SetColumnInternal(yIndex, settings);

        /// <summary>
        ///     <see cref="SetYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetYRange" path="param"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> SetYRangeInternal(
            int yIndex,
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.SetColumnRangeInternal(yIndex, settings);

        /// <summary>
        ///     <see cref="SetPoint"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetPoint" path="param"/>
        public WodiLib.Test.Tools.StubModel SetPointInternal(int xIndex, int yIndex, IStubModelSettings settings)
            => Table.SetCellInternal(xIndex, yIndex, settings);

        /// <summary>
        ///     <see cref="MoveX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveX" path="param"/>
        public void MoveXInternal(int oldXIndex, int newXIndex)
            => Table.MoveRowInternal(oldXIndex, newXIndex);

        /// <summary>
        ///     <see cref="MoveXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveXRange" path="param"/>
        public void MoveXRangeInternal(int oldXIndex, int newXIndex, int count)
            => Table.MoveRowRangeInternal(oldXIndex, newXIndex, count);

        /// <summary>
        ///     <see cref="MoveY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveY" path="param"/>
        public void MoveYInternal(int oldYIndex, int newYIndex)
            => Table.MoveColumnInternal(oldYIndex, newYIndex);

        /// <summary>
        ///     <see cref="MoveYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="MoveYRange" path="param"/>
        public void MoveYRangeInternal(int oldYIndex, int newYIndex, int count)
            => Table.MoveColumnRangeInternal(oldYIndex, newYIndex, count);

        /// <summary>
        ///     <see
        ///         cref="Reset(System.Collections.Generic.IEnumerable{IStubFixedLengthListSettings})"/>
        ///     メソッド処理中核。
        /// </summary>
        /// <inheritdoc
        ///     cref="Reset(System.Collections.Generic.IEnumerable{IStubFixedLengthListSettings})"
        ///     path="param"/>
        public IEnumerable<WodiLib.Test.Tools.StubFixedLengthList> ResetInternal(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.ResetInternal(settings);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubFixedLength2DList? other) => ItemEquals(other as IStubFixedLength2DListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubFixedLength2DList? other) => ItemEquals(other as IStubFixedLength2DListSettings);
        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLength2DListSettings);

        /// <inheritdoc/>
        [Pure]
        public StubFixedLength2DList DeepClone() => new(this);
        object IDeepCloneable.DeepClone() => DeepClone();

        System.Collections.Generic.IReadOnlyList<System.String> IStubFixedLength2DListSettings.Tags => Tags;

        /// <summary>
        ///     <see cref="ExtendedList{TListSettings, TEditableElement, TReadOnlyElement, TElementSettings}"/> が通知した
        ///     <see cref="INotifyCollectionChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <param name="target">対象</param>
        private void PropagateCollectionChangeEvent(TwoDimensionalList<IStubFixedLength2DListSettings, WodiLib.Test.Tools.StubFixedLengthList, WodiLib.Test.Tools.StubFixedLengthList, ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, ReadOnlyStubModel, IStubModelSettings> target)
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }

        private ReadOnlyStubFixedLength2DList? readonlyInstance = null;

        /// <summary>
        ///     読取専用クラスへの暗黙的型変換
        /// </summary>
        /// <param name="src">変換元</param>
        /// <returns>変換したインスタンス</returns>
        [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]
        public static implicit operator ReadOnlyStubFixedLength2DList?(StubFixedLength2DList? src)
        {
            if (src is null) return null;
            src.readonlyInstance ??= new ReadOnlyStubFixedLength2DList(src);
            return src.readonlyInstance;
        }
    }
    */

    /*
    /// <summary>
    ///     【読取専用】<see cref="StubFixedLength2DList"/> スタブ用
    /// </summary>
    public partial class ReadOnlyStubFixedLength2DList : ModelBase,
        IStubFixedLength2DListSettings,
        IEnumerable<ReadOnlyStubFixedLengthList>,
        INotifyCollectionChanged,
        IEqualityComparable<ReadOnlyStubFixedLength2DList>,
        IEqualityComparable<StubFixedLength2DList>,
        IDeepCloneable<ReadOnlyStubFixedLength2DList>
    {

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => MutableInstance.CollectionChanged += value;
            remove => MutableInstance.CollectionChanged -= value;
        }

        /// <summary>Y座標インデクサによるアクセス</summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標インデックスのY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>が指定範囲外の場合。
        /// </exception>
        [Pure]
        public ReadOnlyStubFixedLengthList this[int xIndex] => GetX(xIndex);

        /// <summary>座標インデクサによるアクセス</summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標・Y座標インデックスの座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="yIndex"/>が指定範囲外の場合。
        /// </exception>
        [Pure]
        public ReadOnlyStubModel this[int xIndex, int yIndex] => GetPoint(xIndex, yIndex);

        /// <summary>Y座標数</summary>
        [Pure]
        public int XCount => MutableInstance.XCount;

        /// <summary>Y座標数</summary>
        [Pure]
        public int YCount => MutableInstance.YCount;

        /// <inheritdoc/>
        public System.Collections.Generic.IReadOnlyList<string> Tags => MutableInstance.Tags;

        /// <inheritdoc/>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Pure]
        public IList<IStubFixedLengthListSettings> Settings => MutableInstance.Settings;

        internal StubFixedLength2DList MutableInstance { get; }

        internal ReadOnlyStubFixedLength2DList(StubFixedLength2DList mutableInstance)
        {
            MutableInstance = mutableInstance;
            PropagatePropertyChangeEvent(MutableInstance);
        }

        /// <inheritdoc/>
        [Pure]
        public IEnumerator<ReadOnlyStubFixedLengthList> GetEnumerator() => MutableInstance.Cast<ReadOnlyStubFixedLengthList>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     指定Y座標インデックスのY座標要素を取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定行のY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        public ReadOnlyStubFixedLengthList GetX(int xIndex) => MutableInstance.GetX(xIndex);

        /// <summary>
        ///     指定範囲のY座標要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="XCount"/>)] Y座標数</param>
        /// <returns>指定範囲のY座標要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<ReadOnlyStubFixedLengthList> GetXRange(int xIndex, int count)
            => MutableInstance.GetXRange(xIndex, count).Cast<ReadOnlyStubFixedLengthList>();

        /// <summary>
        ///     指定Y座標インデックスのY座標要素を取得する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定Y座標の要素リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        public IEnumerable<ReadOnlyStubModel> GetY(int yIndex) => MutableInstance.GetY(yIndex).Cast<ReadOnlyStubModel>();

        /// <summary>
        ///     指定範囲のY座標要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="YCount"/>)] Y座標数</param>
        /// <returns>指定範囲のY座標要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を取得しようとした場合。</exception>
        [Pure]
        public IEnumerable<IEnumerable<ReadOnlyStubModel>> GetYRange(int yIndex, int count)
            => MutableInstance.GetYRange(yIndex, count).Select(columns => columns.Cast<ReadOnlyStubModel>());

        /// <summary>
        ///     指定Y座標・Y座標インデックスの座標要素を取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="YCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定Y座標・Y座標の座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        [Pure]
        public ReadOnlyStubModel GetPoint(int xIndex, int yIndex) => MutableInstance.GetPoint(xIndex, yIndex).Cast<WodiLib.Test.Tools.StubModel, ReadOnlyStubModel>();

        /// <summary>
        ///     <see cref="GetX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetX" path="param|exception"/>
        [Pure]
        public void ValidateGetX(int xIndex) => MutableInstance.ValidateGetX(xIndex);

        /// <summary>
        ///     <see cref="GetXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetXRange" path="param|exception"/>
        [Pure]
        public void ValidateGetXRange(int xIndex, int count) => MutableInstance.ValidateGetXRange(xIndex, count);

        /// <summary>
        ///     <see cref="GetY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetY" path="param|exception"/>
        [Pure]
        public void ValidateGetY(int yIndex) => MutableInstance.ValidateGetY(yIndex);

        /// <summary>
        ///     <see cref="GetYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetYRange" path="param|exception"/>
        [Pure]
        public void ValidateGetYRange(int yIndex, int count)
            => MutableInstance.ValidateGetYRange(yIndex, count);

        /// <summary>
        ///     <see cref="GetPoint"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetPoint" path="param|exception"/>
        [Pure]
        public void ValidateGetPoint(int xIndex, int yIndex) => MutableInstance.ValidateGetPoint(xIndex, yIndex);

        /// <summary>
        ///     <see cref="GetX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetX" path="param"/>
        [Pure]
        public ReadOnlyStubFixedLengthList GetXInternal(int xIndex) => MutableInstance.GetXInternal(xIndex);

        /// <summary>
        ///     <see cref="GetXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetXRange" path="param"/>
        [Pure]
        public IEnumerable<ReadOnlyStubFixedLengthList> GetXRangeInternal(int xIndex, int count)
            => MutableInstance.GetXRangeInternal(xIndex, count).Cast<ReadOnlyStubFixedLengthList>();

        /// <summary>
        ///     <see cref="GetY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetY" path="param"/>
        [Pure]
        public IEnumerable<ReadOnlyStubModel> GetYInternal(int yIndex)
            => MutableInstance.GetYInternal(yIndex).Cast<ReadOnlyStubModel>();

        /// <summary>
        ///     <see cref="GetYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetYRange" path="param"/>
        [Pure]
        public IEnumerable<IEnumerable<ReadOnlyStubModel>> GetYRangeInternal(int yIndex, int count)
            => MutableInstance.GetYRangeInternal(yIndex, count).Select(columns => columns.Cast<ReadOnlyStubModel>());

        /// <summary>
        ///     <see cref="GetPoint"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetPoint" path="param"/>
        [Pure]
        public ReadOnlyStubModel GetPointInternal(int xIndex, int yIndex)
            => MutableInstance.GetPointInternal(xIndex, yIndex).Cast<WodiLib.Test.Tools.StubModel, ReadOnlyStubModel>();

        /// <inheritdoc/>
        public System.String ToJsonString() => MutableInstance.ToJsonString();

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(ReadOnlyStubFixedLength2DList? other)
            => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(StubFixedLength2DList? other)
            => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(IStubFixedLength2DListSettings? other)
            => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public bool ItemEquals(object? other) => MutableInstance.ItemEquals(other);

        /// <inheritdoc/>
        [Pure]
        public ReadOnlyStubFixedLength2DList DeepClone() => new(MutableInstance.DeepClone());
        object IDeepCloneable.DeepClone() => DeepClone();


        System.Collections.Generic.IReadOnlyList<System.String> IStubFixedLength2DListSettings.Tags => Tags;

    }
    */
}
