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
    ///     <see cref="StubFixedLength2DList"> スタブ用設定インタフェース
    /// </summary>
    public partial interface IStubFixedLength2DListSettings : WodiLib.Sys.IEqualityComparable<IStubFixedLength2DListSettings>, IListSettings<IStubFixedLengthListSettings>
    {
        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.Tags" />
        System.Collections.Generic.IReadOnlyList<System.String> Tags { get; }
    }
   */

    /*
    /// <summary>
    ///     <see cref="StubFixedLength2DList"> スタブ用設定DTO
    /// </summary>
    public partial record StubFixedLength2DListSettings(IReadOnlyList<IStubFixedLengthListSettings> Settings) : IStubFixedLength2DListSettings
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
        Description = "<see cref=\"StubFixedLength2DList\"> スタブ用",
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
    public partial class ReadOnlyStubFixedLength2DList
    {
        [SettingsProperty(
            DefaultValue = "new List<string>()"
        )]
        [FixedLengthListProperty(
            Accessibility = "NONE"
        )]
        public IReadOnlyList<string> Tags => tags;

        private readonly List<string> tags = new();

        [FixedLengthListConstructor]
        public ReadOnlyStubFixedLength2DList(
            int xSize,
            int ySize
        ) : this(BuildSimpleList(xSize, ySize))
        {
        }

        public ReadOnlyStubFixedLength2DList(IStubFixedLength2DListSettings settings)
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

        [FixedLengthListMethod(
            Accessibility = "public" // デフォルト値が "public" のため、この指定はなくても良い
        )]
        protected void SetNowStringValue()
        {
            Table.EditableRows.ForEach(row
                => row.EditableItems.ForEach(cell => cell.SetNowStringValue())
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
                rowLength.Iterate(rowIndex => BuildItemFromIndex(rowIndex, columnLength))
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
            = new((list, index) => BuildItemFromIndex(index, list.Count));

        private protected static StubFixedLengthList BuildItemFromIndex(int rowIndex, int columnLength)
            => new(BuildRowSettingsFromRowIndex(rowIndex, columnLength));

        /*
         * SourceGenerator が作成する RequiredConstructor で使用されるメソッド。
         * クラス内に保持する Config のコンストラクタ引数として指定する。
         */
        private protected static StubFixedLengthListSettings BuildRowSettingsFromRowIndex(
            int rowIndex,
            int columnLength
        )
            => new(
                columnLength.Iterate(columnIndex => new StubModelSettings()
                        { StringValue = $"{rowIndex}_{columnIndex}" }
                    )
                    .ToArray()
            );

        private protected static StubFixedLengthList BuildRowFromSettings(
            int rowIndex,
            IStubFixedLengthListSettings settings
        )
            => new(settings);

        private protected static StubModel BuildListElementFromSetting(IStubModelSettings settings) => new(settings);

        private protected static bool CompareElement(IStubModelSettings left, IStubModelSettings? right)
            => left.ItemEquals(right);

        private protected IWodiLib2DListValidator<IStubFixedLengthListSettings, IStubModelSettings>
            BuildValidator(
                SimpleList<StubFixedLengthList> itemsImpl
            )
        {
            return new RestrictedCapacity2DListValidator<IStubFixedLengthListSettings, IStubModelSettings>(
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
    ///     <see cref="StubFixedLength2DList"> スタブ用
    /// </summary>
    public partial class StubFixedLength2DList : FixedStubFixedLength2DList,
        WodiLib.Sys.IDeepCloneable<StubFixedLength2DList>
    {
        /// <inheritdoc/>
        public new System.Collections.Generic.IList<string> Tags
        {
            get => (System.Collections.Generic.IList<string>)base.Tags;
        }


        public StubFixedLength2DList(IStubFixedLength2DListSettings settings) : base(settings) { }

        private protected StubFixedLength2DList(SimpleList<StubFixedLengthList> itemsImpl) : base(itemsImpl) { }

        /// <inheritdoc/>
        public StubFixedLength2DList(int xSize, int ySize) : base(xSize, ySize) {}


        /// <summary>
        ///     二次元リストの末尾にY座標要素を追加する。
        /// </summary>
        /// <param name="settings">追加するY座標要素</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> を上回る場合。
        /// </exception>
        public FixedStubFixedLengthList AddX(IStubFixedLengthListSettings settings)
            => Table.AddRow(settings);

        /// <summary>
        ///     二次元リストの末尾にY座標要素を追加する。
        /// </summary>
        /// <param name="settings">追加するY座標要素</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> AddXRange(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.AddRowRange(settings);

        /// <summary>
        ///     指定したY座標インデックスの位置にY座標要素を挿入する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/>)] Y座標インデックス</param>
        /// <param name="settings">追加するY座標要素</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> を上回る場合。
        /// </exception>
        public FixedStubFixedLengthList InsertX(int xIndex, IStubFixedLengthListSettings settings)
            => Table.InsertRow( xIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスの位置にY座標要素を挿入する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/>)] Y座標インデックス</param>
        /// <param name="settings">追加するY座標要素</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> InsertXRange(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.InsertRowRange( xIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスを起点として、Y座標要素の上書き/追加を行う。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/>)] Y座標インデックス</param>
        /// <param name="settings">上書き/追加Y座標リスト</param>
        /// <returns>上書きしたY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> OverwriteX(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.OverwriteRow( xIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスのY座標要素を削除する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/> を下回る場合。
        /// </exception>
        public FixedStubFixedLengthList RemoveX(int xIndex) => Table.RemoveRow( xIndex);

        /// <summary>
        ///     Y座標要素の範囲を削除する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/>)] 削除するY座標数</param>
        /// <returns>削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のY座標要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> RemoveXRange(int xIndex, int count)
            => Table.RemoveRowRange( xIndex, count);

        /// <summary>
        ///     Y座標数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/>,
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/>)]
        ///     調整するY座標数
        /// </param>
        /// <returns>追加または削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> AdjustRowLength(int length)
            => Table.AdjustRowLength(length);

        /// <summary>
        ///     Y座標数が不足している場合、Y座標数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/>,
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/>)]
        ///     調整するY座標数
        /// </param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> AdjustXLengthIfShort(int length)
            => Table.AdjustRowLengthIfShort(length);

        /// <summary>
        ///     Y座標数が超過している場合、Y座標数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/>,
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/>)]
        ///     調整するY座標数
        /// </param>
        /// <returns>削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> AdjustXLengthIfLong(int length)
            => Table.AdjustRowLengthIfLong(length);

        /// <summary>
        ///     二次元リストの末尾にY座標要素を追加する。
        /// </summary>
        /// <param name="settings">追加するY座標要素</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AddY(IEnumerable<IStubModelSettings> settings) => Table.AddColumn(settings);

        /// <summary>
        ///     二次元リストの末尾にY座標要素を追加する。
        /// </summary>
        /// <param name="settings">追加するY座標要素（外側のIEnumerableがY座標、内側のIEnumerableが各Y座標のY座標要素）</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> AddYRange(
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.AddColumnRange(settings);

        /// <summary>
        ///     指定したY座標インデックスの位置にY座標要素を挿入する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/>)] Y座標インデックス</param>
        /// <param name="settings">追加するY座標要素</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> InsertY(
            int yIndex,
            IEnumerable<IStubModelSettings> settings
        ) => Table.InsertColumn(yIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスの位置にY座標要素を挿入する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/>)] Y座標インデックス</param>
        /// <param name="settings">追加するY座標要素（外側のIEnumerableがY座標、内側のIEnumerableが各Y座標のY座標要素）</param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> InsertYRange(
            int yIndex,
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.InsertColumnRange(yIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスを起点として、Y座標要素の上書き/追加を行う。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/>)] Y座標インデックス</param>
        /// <param name="settings">上書き/追加Y座標リスト（外側のIEnumerableがY座標、内側のIEnumerableが各Y座標のY座標要素）</param>
        /// <returns>上書きしたY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="settings"/> が <see langword="null"/> の場合、
        ///     または <paramref name="settings"/> に <see langword="null"/> 要素が含まれる場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> を上回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> OverwriteY(
            int yIndex,
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.OverwriteColumn(yIndex, settings);

        /// <summary>
        ///     指定したY座標インデックスのY座標要素を削除する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] Y座標インデックス</param>
        /// <returns>削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.StubModel> RemoveY(int yIndex) => Table.RemoveColumn(yIndex);

        /// <summary>
        ///     Y座標要素の範囲を削除する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/>)] 削除するY座標数</param>
        /// <returns>削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/>, <paramref name="count"/> が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     有効な範囲外のY座標要素を削除しようとした場合。
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     操作によってY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/> を下回る場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> RemoveYRange(int yIndex, int count)
            => Table.RemoveColumnRange(yIndex, count);

        /// <summary>
        ///     Y座標数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/>,
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/>)]
        ///     調整するY座標数
        /// </param>
        /// <returns>追加または削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> AdjustYLength(int length) => Table.AdjustColumnLength(length);

        /// <summary>
        ///     Y座標数が不足している場合、Y座標数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/>,
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/>)]
        ///     調整するY座標数
        /// </param>
        /// <returns>追加したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> AdjustYLengthIfShort(int length)
            => Table.AdjustColumnLengthIfShort(length);

        /// <summary>
        ///     Y座標数が超過している場合、Y座標数を指定の数に合わせる。
        /// </summary>
        /// <param name="length">
        ///     [Range(<see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/>,
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/>)]
        ///     調整するY座標数
        /// </param>
        /// <returns>削除したY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="length"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> AdjustYLengthIfLong(int length)
            => Table.AdjustColumnLengthIfLong(length);

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
        ///     <paramref name="settings"/> のY座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/> 未満
        ///     または <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> を超える場合、
        ///     Y座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/> 未満
        ///     または <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> を超える場合。
        /// </exception>
        /// <remarks>
        ///     このメソッドは <paramref name="settings"/> のY座標数が
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMinXCapacity"/> 以上
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxXCapacity"/> 以下、
        ///     Y座標数が <see cref="ReadOnlyStubFixedLength2DList.GetMinYCapacity"/> 以上
        ///     <see cref="ReadOnlyStubFixedLength2DList.GetMaxYCapacity"/> 以下であれば
        ///     成功する。<br/>
        ///     現在のY座標数・Y座標数と一致しない場合エラーとしたい場合は、
        ///     容量固定型にキャストしてから同メソッドを呼び出す。
        /// </remarks>
        public new IEnumerable<FixedStubFixedLengthList> Reset(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.Reset(settings);

        /// <summary>
        ///     自身を初期化する。
        /// </summary>
        public void Clear() => Table.Clear();

        /// <summary>
        ///     <see cref="AddX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddX" path="param|exception"/>
        public void ValidateAddX(IStubFixedLengthListSettings settings) => Table.ValidateAddRow(settings);

        /// <summary>
        ///     <see cref="AddXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddXRange" path="param|exception"/>
        public void ValidateAddXRange(IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateAddRowRange(settings);

        /// <summary>
        ///     <see cref="InsertX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertX" path="param|exception"/>
        public void ValidateInsertX(int xIndex, IStubFixedLengthListSettings settings)
            => Table.ValidateInsertRow( xIndex, settings);

        /// <summary>
        ///     <see cref="InsertXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertXRange" path="param|exception"/>
        public void ValidateInsertXRange(int xIndex, IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateInsertRowRange( xIndex, settings);

        /// <summary>
        ///     <see cref="OverwriteX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="OverwriteX" path="param|exception"/>
        public void ValidateOverwriteX(int xIndex, IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateOverwriteRow( xIndex, settings);

        /// <summary>
        ///     <see cref="RemoveX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveX" path="param|exception"/>
        public void ValidateRemoveX(int xIndex) => Table.ValidateRemoveRow( xIndex);

        /// <summary>
        ///     <see cref="RemoveXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveXRange" path="param|exception"/>
        public void ValidateRemoveXRange(int xIndex, int count) => Table.ValidateRemoveRowRange( xIndex, count);

        /// <summary>
        ///     <see cref="AdjustXLength"/>,
        ///     <see cref="AdjustXLengthIfShort"/>,
        ///     <see cref="AdjustXLengthIfLong"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustXLength" path="param|exception"/>
        public void ValidateAdjustXLength(int length) => Table.ValidateAdjustRowLength(length);

        /// <summary>
        ///     <see cref="AddY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddY" path="param|exception"/>
        public void ValidateAddY(IEnumerable<IStubModelSettings> settings) => Table.ValidateAddColumn(settings);

        /// <summary>
        ///     <see cref="AddYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AddYRange" path="param|exception"/>
        public void ValidateAddYRange(IEnumerable<IEnumerable<IStubModelSettings>> settings)
            => Table.ValidateAddColumnRange(settings);

        /// <summary>
        ///     <see cref="InsertY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertY" path="param|exception"/>
        public void ValidateInsertY(int yIndex, IEnumerable<IStubModelSettings> settings)
            => Table.ValidateInsertColumn(yIndex, settings);

        /// <summary>
        ///     <see cref="InsertYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="InsertYRange" path="param|exception"/>
        public void ValidateInsertYRange(int yIndex, IEnumerable<IEnumerable<IStubModelSettings>> settings)
            => Table.ValidateInsertColumnRange(yIndex, settings);

        /// <summary>
        ///     <see cref="OverwriteY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="OverwriteY" path="param|exception"/>
        public void ValidateOverwriteY(int yIndex, IEnumerable<IEnumerable<IStubModelSettings>> settings)
            => Table.ValidateOverwriteColumn(yIndex, settings);

        /// <summary>
        ///     <see cref="RemoveY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveY" path="param|exception"/>
        public void ValidateRemoveY(int yIndex) => Table.ValidateRemoveColumn(yIndex);

        /// <summary>
        ///     <see cref="RemoveYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="RemoveYRange" path="param|exception"/>
        public void ValidateRemoveYRange(int yIndex, int count)
            => Table.ValidateRemoveColumnRange(yIndex, count);

        /// <summary>
        ///     <see cref="AdjustYLength"/>,
        ///     <see cref="AdjustYLengthIfShort"/>,
        ///     <see cref="AdjustYLengthIfLong"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="AdjustYLength" path="param|exception"/>
        public void ValidateAdjustYLength(int length) => Table.ValidateAdjustColumnLength(length);

        /// <summary>
        ///     <see cref="Clear"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="Clear" path="param|exception"/>
        public void ValidateClear() => Table.ValidateClear();

        /// <summary>
        ///     <see cref="AddX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddX" path="param|returns"/>
        public FixedStubFixedLengthList AddXInternal(IStubFixedLengthListSettings settings)
            => Table.AddRowInternal(settings);

        /// <summary>
        ///     <see cref="AddXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddXRange" path="param|returns"/>
        public IEnumerable<FixedStubFixedLengthList> AddXRangeInternal(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.AddRowRangeInternal(settings);

        /// <summary>
        ///     <see cref="InsertX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertX" path="param|returns"/>
        public FixedStubFixedLengthList InsertXInternal(
            int xIndex,
            IStubFixedLengthListSettings settings
        ) => Table.InsertRowInternal( xIndex, settings);

        /// <summary>
        ///     <see cref="InsertXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertXRange" path="param|returns"/>
        public IEnumerable<FixedStubFixedLengthList> InsertXRangeInternal(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.InsertRowRangeInternal( xIndex, settings);

        /// <summary>
        ///     <see cref="OverwriteX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="OverwriteX" path="param|returns"/>
        public IEnumerable<FixedStubFixedLengthList> OverwriteXInternal(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.OverwriteRowInternal( xIndex, settings);

        /// <summary>
        ///     <see cref="RemoveX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveX" path="param|returns"/>
        public FixedStubFixedLengthList RemoveXInternal(int xIndex) => Table.RemoveRowInternal( xIndex);

        /// <summary>
        ///     <see cref="RemoveXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveXRange" path="param|returns"/>
        public IEnumerable<FixedStubFixedLengthList> RemoveXRangeInternal(int xIndex, int count)
            => Table.RemoveRowRangeInternal( xIndex, count);

        /// <summary>
        ///     <see cref="AdjustXLength"/>,
        ///     <see cref="AdjustXLengthIfShort"/>,
        ///     <see cref="AdjustXLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustXLength" path="param|returns"/>
        public IEnumerable<FixedStubFixedLengthList> AdjustXLengthInternal(int length)
            => Table.AdjustRowLengthInternal(length);

        /// <summary>
        ///     <see cref="AddY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddY" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> AddYInternal(IEnumerable<IStubModelSettings> settings)
            => Table.AddColumnInternal(settings);

        /// <summary>
        ///     <see cref="AddYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AddYRange" path="param|returns"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> AddYRangeInternal(
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.AddColumnRangeInternal(settings);

        /// <summary>
        ///     <see cref="InsertY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertY" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> InsertYInternal(
            int yIndex,
            IEnumerable<IStubModelSettings> settings
        ) => Table.InsertColumnInternal(yIndex, settings);

        /// <summary>
        ///     <see cref="InsertYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="InsertYRange" path="param|returns"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> InsertYRangeInternal(
            int yIndex,
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.InsertColumnRangeInternal(yIndex, settings);

        /// <summary>
        ///     <see cref="OverwriteY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="OverwriteY" path="param|returns"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> OverwriteYInternal(
            int yIndex,
            IEnumerable<IEnumerable<IStubModelSettings>> settings
        ) => Table.OverwriteColumnInternal(yIndex, settings);

        /// <summary>
        ///     <see cref="RemoveY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveY" path="param|returns"/>
        public IEnumerable<WodiLib.Test.Tools.StubModel> RemoveYInternal(int yIndex) => Table.RemoveColumnInternal(yIndex);

        /// <summary>
        ///     <see cref="RemoveYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="RemoveYRange" path="param|returns"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> RemoveYRangeInternal(int yIndex, int count)
            => Table.RemoveColumnRangeInternal(yIndex, count);

        /// <summary>
        ///     <see cref="AdjustYLength"/>,
        ///     <see cref="AdjustYLengthIfShort"/>,
        ///     <see cref="AdjustYLengthIfLong"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="AdjustYLength" path="param|returns"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> AdjustYLengthInternal(int length)
            => Table.AdjustColumnLengthInternal(length);

        /// <inheritdoc
        ///     cref="FixedStubFixedLength2DList.Reset(System.Collections.Generic.IEnumerable{WodiLib.Test.Tools.IStubFixedLengthListSettings})"/>
        public new IEnumerable<FixedStubFixedLengthList> ResetInternal(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.ResetInternal(settings);

        /// <inheritdoc/>
        public new StubFixedLength2DList DeepClone() => new(this);
        object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();
    }
    */

    /*
    /// <summary>
    ///     <see cref="StubFixedLength2DList"> スタブ用
    /// </summary>
    public partial class FixedStubFixedLength2DList : ReadOnlyStubFixedLength2DList,
        WodiLib.Sys.IDeepCloneable<FixedStubFixedLength2DList>
    {
        /// <summary>
        ///     Y座標インデクサによるアクセス
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標インデックスのY座標要素（長さ固定型）</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>が指定範囲外の場合。</exception>
        public new FixedStubFixedLengthList this[int xIndex]
        {
            get => GetX( xIndex);
            set => SetX( xIndex, value);
        }

        /// <summary>
        ///     座標インデクサによるアクセス
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標・Y座標インデックスの座標要素</returns>
        /// <exception cref="ArgumentNullException"><see langword="null"/> をセットしようとした場合。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="xIndex"/>, <paramref name="yIndex"/>が指定範囲外の場合。</exception>
        public new WodiLib.Test.Tools.StubModel this[int xIndex, int yIndex]
        {
            get => GetPoint( xIndex, yIndex);
            set => SetPoint( xIndex, yIndex, value);
        }

        /// <summary>すべての編集可能Y座標型要素</summary>
        public FixedStubFixedLengthList[] EditableXs => Table.EditableRows;

        /// <inheritdoc/>
        public new System.Collections.Generic.IReadOnlyList<string> Tags
        {
            get => base.Tags;
        }


        public FixedStubFixedLength2DList(IStubFixedLength2DListSettings settings) : base(settings) { }

        private protected FixedStubFixedLength2DList(SimpleList<StubFixedLengthList> itemsImpl) : base(itemsImpl) { }

        /// <inheritdoc/>
        public FixedStubFixedLength2DList(int xSize, int ySize) : base(xSize, ySize) {}

        /// <inheritdoc/>
        public new void SetNowStringValue() => base.SetNowStringValue();

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetX"/>
        public new FixedStubFixedLengthList GetX(int xIndex) => Table.GetRow( xIndex);

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetXRange"/>
        public new IEnumerable<FixedStubFixedLengthList> GetXRange(int xIndex, int count)
            => Table.GetRowRange( xIndex, count);

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
        public FixedStubFixedLengthList SetX(int xIndex, IStubFixedLengthListSettings settings)
            => Table.SetRow( xIndex, settings);

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
        public IEnumerable<FixedStubFixedLengthList> SetXRange(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.SetRowRange( xIndex, settings);

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

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetY"/>
        public new IEnumerable<WodiLib.Test.Tools.StubModel> GetY(int yIndex) => Table.GetColumn(yIndex);

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetYRange"/>
        public new IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> GetYRange(int yIndex, int count)
            => Table.GetColumnRange(yIndex, count);

        /// <summary>
        ///     二次元リストのY座標要素を更新する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] 更新Y座標インデックス</param>
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
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] 更新開始Y座標インデックス</param>
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
        ///     指定したY座標インデックスにある項目をコレクション内の新しい場所へ移動する。
        /// </summary>
        /// <param name="oldYIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)]
        ///     移動するY座標のインデックス
        /// </param>
        /// <param name="newYIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] 移動先のY座標インデックス</param>
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
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)]
        ///     移動するY座標のインデックス開始位置
        /// </param>
        /// <param name="newYIndex">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)]
        ///     移動先のY座標インデックス開始位置
        /// </param>
        /// <param name="count">
        ///     [Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/>)]
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

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetPoint"/>
        public new WodiLib.Test.Tools.StubModel GetPoint(int xIndex, int yIndex) => Table.GetCell( xIndex, yIndex);

        /// <summary>
        ///     二次元リストの座標要素を更新する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="ReadOnlyStubFixedLength2DList.Y"/> - 1)] Y座標インデックス</param>
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
            => Table.SetCell( xIndex, yIndex, settings);

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
        ///     Y座標数が <see cref="ReadOnlyStubFixedLength2DList.Y"/> と異なる場合。
        /// </exception>
        public IEnumerable<FixedStubFixedLengthList> Reset(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.Reset(settings);

        /// <summary>
        ///     要素をデフォルト値で一新する。
        /// </summary>
        public IEnumerable<FixedStubFixedLengthList> Reset() => Table.Reset();

        /// <summary>
        ///     <see cref="SetX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetX" path="param|exception"/>
        public void ValidateSetX(int xIndex, IStubFixedLengthListSettings settings)
            => Table.ValidateSetRow( xIndex, settings);

        /// <summary>
        ///     <see cref="SetXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetXRange" path="param|exception"/>
        public void ValidateSetXRange(int xIndex, IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateSetRowRange( xIndex, settings);

        /// <summary>
        ///     <see cref="SetY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetY" path="param|exception"/>
        public void ValidateSetY(int yIndex, IEnumerable<IStubModelSettings> settings)
            => Table.ValidateSetColumn(yIndex, settings);

        /// <summary>
        ///     <see cref="SetYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetYRange" path="param|exception"/>
        public void ValidateSetYRange(int yIndex, IEnumerable<IEnumerable<IStubModelSettings>> settings)
            => Table.ValidateSetColumnRange(yIndex, settings);

        /// <summary>
        ///     <see cref="SetPoint"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="SetPoint" path="param|exception"/>
        public void ValidateSetPoint(int xIndex, int yIndex, IStubModelSettings settings)
            => Table.ValidateSetCell( xIndex, yIndex, settings);

        /// <summary>
        ///     <see cref="MoveX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveX" path="param|exception"/>
        public void ValidateMoveX(int oldXIndex, int newXIndex)
            => Table.ValidateMoveRow(oldXIndex, newXIndex);

        /// <summary>
        ///     <see cref="MoveXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveXRange" path="param|exception"/>
        public void ValidateMoveXRange(int oldXIndex, int newXIndex, int count)
            => Table.ValidateMoveRowRange(oldXIndex, newXIndex, count);

        /// <summary>
        ///     <see cref="MoveY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveY" path="param|exception"/>
        public void ValidateMoveY(int oldYIndex, int newYIndex)
            => Table.ValidateMoveColumn(oldYIndex, newYIndex);

        /// <summary>
        ///     <see cref="MoveYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="MoveYRange" path="param|exception"/>
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
        public void ValidateReset(IEnumerable<IStubFixedLengthListSettings> settings)
            => Table.ValidateReset(settings);

        /// <summary>
        ///     <see
        ///         cref="Reset()"/>
        ///     メソッドの検証処理。
        /// </summary>
        /// <inheritdoc
        ///     cref="Reset()"
        ///     path="param|exception"/>
        public void ValidateReset() => Table.ValidateReset();

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetXInternal"/>
        public new FixedStubFixedLengthList GetXInternal(int xIndex) => Table.GetRowInternal( xIndex);

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetXRangeInternal"/>
        public new IEnumerable<FixedStubFixedLengthList> GetXRangeInternal(int xIndex, int count)
            => Table.GetRowRangeInternal( xIndex, count);

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetYInternal"/>
        public new IEnumerable<WodiLib.Test.Tools.StubModel> GetYInternal(int yIndex) => Table.GetColumnInternal(yIndex);

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetYRangeInternal"/>
        public new IEnumerable<IEnumerable<WodiLib.Test.Tools.StubModel>> GetYRangeInternal(int yIndex, int count)
            => Table.GetColumnRangeInternal(yIndex, count);

        /// <inheritdoc cref="ReadOnlyStubFixedLength2DList.GetPointInternal"/>
        public new WodiLib.Test.Tools.StubModel GetPointInternal(int xIndex, int yIndex)
            => Table.GetCellInternal( xIndex, yIndex);

        /// <summary>
        ///     <see cref="SetX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetX" path="param"/>
        public FixedStubFixedLengthList SetXInternal(
            int xIndex,
            IStubFixedLengthListSettings settings
        ) => Table.SetRowInternal( xIndex, settings);

        /// <summary>
        ///     <see cref="SetXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="SetXRange" path="param"/>
        public IEnumerable<FixedStubFixedLengthList> SetXRangeInternal(
            int xIndex,
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.SetRowRangeInternal( xIndex, settings);

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
            => Table.SetCellInternal( xIndex, yIndex, settings);

        /// <summary>
        ///     <see cref="MovX"/> メソッド処理中核。
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
        public IEnumerable<FixedStubFixedLengthList> ResetInternal(
            IEnumerable<IStubFixedLengthListSettings> settings
        ) => Table.ResetInternal(settings);

        /// <inheritdoc/>
        public new FixedStubFixedLength2DList DeepClone() => new(this);
        object IDeepCloneable.DeepClone() => DeepClone();
    }
    */

    /*
    /// <summary>
    ///     【読取専用】<see cref="StubFixedLength2DList"> スタブ用
    /// </summary>
    public partial class ReadOnlyStubFixedLength2DList : ModelBase,
        IStubFixedLength2DListSettings,
        IReadOnlyList<WodiLib.Test.Tools.ReadOnlyStubFixedLengthList>,
        INotifyCollectionChanged,
        IEqualityComparable<ReadOnlyStubFixedLength2DList>,
        IEqualityComparable<FixedStubFixedLength2DList>,
        IEqualityComparable<StubFixedLength2DList>,
        IDeepCloneable<ReadOnlyStubFixedLength2DList>
    {
        /// <summary>Y座標容量最大値</summary>
        public static int MaxXCapacity => 10;
        /// <summary>Y座標容量最小値</summary>
        public static int MinXCapacity => 1;
        /// <summary>Y座標容量最大値</summary>
        public static int MaxYCapacity => 9;
        /// <summary>Y座標容量最小値</summary>
        public static int MinYCapacity => 0;

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => collectionChanged += value;
            remove => collectionChanged -= value;
        }

        private event NotifyCollectionChangedEventHandler? collectionChanged;

        private protected ReadOnlyStubFixedLength2DList(SimpleList<StubFixedLengthList> itemsImpl)
        {
            Table =
                new TwoDimensionalList<StubFixedLengthList, FixedStubFixedLengthList, WodiLib.Test.Tools.ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel, IStubModelSettings>(
                    itemsImpl,
                    new ReadOnly2DList<StubFixedLengthList, FixedStubFixedLengthList, WodiLib.Test.Tools.ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel, IStubModelSettings>.Config(
                        BuildRowSettingsFromRowIndex,
                        BuildRowFromSettings,
                        BuildListElementFromSetting,
                        CompareElement,
                        BuildValidator(itemsImpl)
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

        /// <summary>Y座標インデクサによるアクセス</summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標インデックスのY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>が指定範囲外の場合。
        /// </exception>
        public WodiLib.Test.Tools.ReadOnlyStubFixedLengthList this[int xIndex] => GetX( xIndex);

        /// <summary>座標インデクサによるアクセス</summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="Y"/> - 1)] Y座標インデックス</param>
        /// <returns>指定したY座標・Y座標インデックスの座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="yIndex"/>が指定範囲外の場合。
        /// </exception>
        public WodiLib.Test.Tools.ReadOnlyStubModel this[int xIndex, int yIndex] => GetPoint(xIndex, yIndex);

        /// <summary>Y座標数</summary>
        public int XCount => Table.RowCount;

        /// <summary>Y座標数</summary>
        public int Y => Table.ColumnCount;

        /// <summary>全要素数</summary>
        public int TotalCount => Table.TotalCount;

        /// <summary>すべての編集可能型Y座標要素</summary>
        public WodiLib.Test.Tools.ReadOnlyStubFixedLengthList[] EditableXs => Table.EditableRows;

        /// <inheritdoc/>
        public IReadOnlyList<IStubFixedLengthListSettings> Settings => Table;

        int IReadOnlyCollection<WodiLib.Test.Tools.ReadOnlyStubFixedLengthList>.Count => XCount;

        private protected TwoDimensionalList<StubFixedLengthList, FixedStubFixedLengthList, WodiLib.Test.Tools.ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel, IStubModelSettings> Table { get; }

        /// <inheritdoc/>
        public IEnumerator<WodiLib.Test.Tools.ReadOnlyStubFixedLengthList> GetEnumerator() => Table.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///     すべてのY座標要素に対し <see cref="INotifyPropertyChanged"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///         また、新規Y座標データが追加された場合には自動でイベントが付与される。
        ///     </para>
        ///     <para>
        ///         <see cref="AddXPropertyChanged"/> メソッドで登録したイベントを任意のタイミングで解除するには
        ///         <see cref="RemoveXPropertyChanged"/> を実行する。
        ///     </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddXPropertyChanged(PropertyChangedEventHandler handler) => Table.AddRowPropertyChanged(handler);

        /// <summary>
        ///     すべてのY座標要素から、登録した <see cref="INotifyPropertyChanged"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="handler"/> が <see cref="AddXPropertyChanged"/> を通して登録されたものでない場合は解除されない点に注意。
        ///     </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveXPropertyChanged(PropertyChangedEventHandler handler)
            => Table.RemoveRowPropertyChanged(handler);

        /// <summary>
        ///     すべてのY座標要素に対し <see cref="INotifyCollectionChanged.CollectionChanged"/> イベントを登録する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         このメソッドで登録したイベントは、要素がリストから除去されるときに同時に解除される。
        ///         また、新規Y座標データが追加された場合には自動でイベントが付与される。
        ///     </para>
        /// </remarks>
        /// <param name="handler">登録するイベント</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void AddXCollectionChanged(NotifyCollectionChangedEventHandler handler)
            => Table.AddRowCollectionChanged(handler);

        /// <summary>
        ///     すべてのY座標要素から登録した <see cref="INotifyCollectionChanged.CollectionChanged"/> イベントを解除する。
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <paramref name="handler"/> が <see cref="AddXCollectionChanged"/> を通して登録されたものでない場合はなにもしない。
        ///     </para>
        /// </remarks>
        /// <param name="handler">解除するイベント</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="handler"/> が <see langword="null"/> の場合。
        /// </exception>
        public void RemoveXCollectionChanged(NotifyCollectionChangedEventHandler handler)
            => Table.RemoveRowCollectionChanged(handler);

        /// <summary>Y座標容量最大値を取得する。</summary>
        /// <returns>Y座標容量最大値</returns>
        public int GetMaxXCapacity() => MaxXCapacity;
        /// <summary>Y座標容量最小値を取得する。</summary>
        /// <returns>Y座標容量最小値</returns>
        public int GetMinXCapacity() => MinXCapacity;
        /// <summary>Y座標容量最大値を取得する。</summary>
        /// <returns>Y座標容量最大値</returns>
        public int GetMaxYCapacity() => MaxYCapacity;
        /// <summary>Y座標容量最小値を取得する。</summary>
        /// <returns>Y座標容量最小値</returns>
        public int GetMinYCapacity() => MinYCapacity;

        /// <summary>
        ///     指定Y座標インデックスのY座標要素を取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <returns>指定行のY座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/> が指定範囲外の場合。
        /// </exception>
        public WodiLib.Test.Tools.ReadOnlyStubFixedLengthList GetX(int xIndex) => Table.GetRow( xIndex);

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
        public IEnumerable<WodiLib.Test.Tools.ReadOnlyStubFixedLengthList> GetXRange(int xIndex, int count)
            => Table.GetRowRange( xIndex, count);

        /// <summary>
        ///     指定Y座標インデックスのY座標要素を取得する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="Y"/> - 1)] Y座標インデックス</param>
        /// <returns>指定Y座標の要素リスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        public IEnumerable<WodiLib.Test.Tools.ReadOnlyStubModel> GetY(int yIndex) => Table.GetColumn(yIndex);

        /// <summary>
        ///     指定範囲のY座標要素を簡易コピーしたリストを取得する。
        /// </summary>
        /// <param name="yIndex">[Range(0, <see cref="Y"/> - 1)] Y座標インデックス</param>
        /// <param name="count">[Range(0, <see cref="Y"/>)] Y座標数</param>
        /// <returns>指定範囲のY座標要素簡易コピーリスト</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="yIndex"/>, <paramref name="count"/>が指定範囲外の場合。
        /// </exception>
        /// <exception cref="ArgumentException">有効な範囲外のY座標要素を取得しようとした場合。</exception>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.ReadOnlyStubModel>> GetYRange(int yIndex, int count)
            => Table.GetColumnRange(yIndex, count);

        /// <summary>
        ///     指定Y座標・Y座標インデックスの座標要素を取得する。
        /// </summary>
        /// <param name="xIndex">[Range(0, <see cref="XCount"/> - 1)] Y座標インデックス</param>
        /// <param name="yIndex">[Range(0, <see cref="Y"/> - 1)] Y座標インデックス</param>
        /// <returns>指定Y座標・Y座標の座標要素</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="xIndex"/>, <paramref name="yIndex"/> が指定範囲外の場合。
        /// </exception>
        public WodiLib.Test.Tools.ReadOnlyStubModel GetPoint(int xIndex, int yIndex) => Table.GetCell( xIndex, yIndex);

        /// <summary>
        ///     <see cref="GetX"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetX" path="param|exception"/>
        public void ValidateGetX(int xIndex) => Table.ValidateGetRow( xIndex);

        /// <summary>
        ///     <see cref="GetXRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetXRange" path="param|exception"/>
        public void ValidateGetXRange(int xIndex, int count) => Table.ValidateGetRowRange( xIndex, count);

        /// <summary>
        ///     <see cref="GetY"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetY" path="param|exception"/>
        public void ValidateGetY(int yIndex) => Table.ValidateGetColumn(yIndex);

        /// <summary>
        ///     <see cref="GetYRange"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetYRange" path="param|exception"/>
        public void ValidateGetYRange(int yIndex, int count)
            => Table.ValidateGetColumnRange(yIndex, count);

        /// <summary>
        ///     <see cref="GetPoint"/> メソッドの検証処理。
        /// </summary>
        /// <inheritdoc cref="GetPoint" path="param|exception"/>
        public void ValidateGetPoint(int xIndex, int yIndex) => Table.ValidateGetCell( xIndex, yIndex);

        /// <summary>
        ///     <see cref="GetX"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetX" path="param"/>
        public WodiLib.Test.Tools.ReadOnlyStubFixedLengthList GetXInternal(int xIndex) => Table.GetRowInternal( xIndex);

        /// <summary>
        ///     <see cref="GetXRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetXRange" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.ReadOnlyStubFixedLengthList> GetXRangeInternal(int xIndex, int count)
            => Table.GetRowRangeInternal( xIndex, count);

        /// <summary>
        ///     <see cref="GetY"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetY" path="param"/>
        public IEnumerable<WodiLib.Test.Tools.ReadOnlyStubModel> GetYnternal(int yIndex)
            => Table.GetColumnInternal(yIndex);

        /// <summary>
        ///     <see cref="GetYRange"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetYRange" path="param"/>
        public IEnumerable<IEnumerable<WodiLib.Test.Tools.ReadOnlyStubModel>> GetYRangeInternal(int yIndex, int count)
            => Table.GetColumnRangeInternal(yIndex, count);

        /// <summary>
        ///     <see cref="GetPoint"/> メソッド処理中核。
        /// </summary>
        /// <inheritdoc cref="GetPoint" path="param"/>
        public WodiLib.Test.Tools.ReadOnlyStubModel GetPointInternal(int xIndex, int yIndex)
            => Table.GetCellInternal( xIndex, yIndex);

        /// <inheritdoc/>
        public bool ItemEquals(ReadOnlyStubFixedLength2DList? other)
            => ItemEquals(other as IStubFixedLength2DListSettings);

        /// <inheritdoc/>
        public bool ItemEquals(FixedStubFixedLength2DList? other)
            => ItemEquals(other as IStubFixedLength2DListSettings);

        /// <inheritdoc/>
        public bool ItemEquals(StubFixedLength2DList? other)
            => ItemEquals(other as IStubFixedLength2DListSettings);

        /// <inheritdoc/>
        public bool ItemEquals(object? other) => ItemEquals(other as IStubFixedLength2DListSettings);

        /// <inheritdoc/>
        public ReadOnlyStubFixedLength2DList DeepClone() => new(this);
        object IDeepCloneable.DeepClone() => DeepClone();

        /// <summary>
        ///     <see cref="SimpleList{T}"/> が通知した
        ///     <see cref="INotifyCollectionChanged"/> イベントを
        ///     自身のイベントとして通知する。
        /// </summary>
        /// <param name="target">対象</param>
        private void PropagateCollectionChangeEvent(
            TwoDimensionalList<StubFixedLengthList, FixedStubFixedLengthList,
                WodiLib.Test.Tools.ReadOnlyStubFixedLengthList, IStubFixedLengthListSettings, WodiLib.Test.Tools.StubModel, WodiLib.Test.Tools.ReadOnlyStubModel,
                IStubModelSettings> target
        )
        {
            target.CollectionChanged += (_, args) => { collectionChanged?.Invoke(this, args); };
        }
    }
    */
}
