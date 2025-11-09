// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TwoDListGeneratorBase.BuildFixedLengthClassSource_Standalone.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal abstract partial class TwoDListGeneratorBase
    {
        /// <summary>
        ///     容量制限ありクラスに処理を委譲しない容量固定リストクラス定義を出力する。
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        private static SourceFormatTargetBlock BuildFixedLengthClassSource_Standalone(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                // class start
                $"/// <summary>",
                $"/// {__}{modelInfo.Description}",
                $"/// </summary>",
                $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.FixedLengthListInfo.FixedLengthListClassName} : ModelBase,",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                $"{__}IEnumerable<{modelInfo.FixedLengthRowType}>,",
                $"{__}INotifyCollectionChanged,",
                $"{__}IEqualityComparable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>,",
                $"{__}IEqualityComparable<{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}>,",
                $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>",
                $"{{",
                // Constants
                BuildFixedLengthClassConstantsSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Events
                BuildFixedLengthClassEventSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Properties
                BuildFixedLengthClassPropertiesSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildFixedLengthClassConstructorSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildFixedLengthClassMethodsSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // ItemEquals
                BuildFixedLengthClassItemEqualsSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildFixedLengthClassDeepCloneSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // SettingsInterface Implements
                BuildFixedLengthClassSettingsInterfaceImplementsSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Private Methods
                BuildFixedLengthClassPrivateMethodSource_Standalone(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Implicit Type Conversion Operator
                BuildImplicitTypeConversionOperatorSource_Standalone(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassConstantsSource_Standalone(
            ModelInformation modelInfo
        )
        {
            var lines = new List<string>();

            if (modelInfo.MaxRowCapacity == modelInfo.MinRowCapacity)
            {
                lines.AddRange(
                    new[]
                    {
                        $"/// <summary>{modelInfo.RowLogicalName}容量</summary>",
                        $"[Pure]",
                        $"public static int {modelInfo.RowPhysicalName}Capacity => {modelInfo.MaxRowCapacity};",
                    }
                );
            }
            else
            {
                lines.AddRange(
                    new[]
                    {
                        $"/// <summary>{modelInfo.RowLogicalName}容量最大値</summary>",
                        $"[Pure]",
                        $"public static int Max{modelInfo.RowPhysicalName}Capacity => {modelInfo.MaxRowCapacity};",
                        $"/// <summary>{modelInfo.RowLogicalName}容量最小値</summary>",
                        $"[Pure]",
                        $"public static int Min{modelInfo.RowPhysicalName}Capacity => {modelInfo.MinRowCapacity};",
                    }
                );
            }

            if (modelInfo.MaxColumnCapacity == modelInfo.MinColumnCapacity)
            {
                lines.AddRange(
                    new[]
                    {
                        $"/// <summary>{modelInfo.ColumnLogicalName}容量</summary>",
                        $"[Pure]",
                        $"public static int {modelInfo.ColumnPhysicalName}Capacity => {modelInfo.MaxColumnCapacity};",
                    }
                );
            }
            else
            {
                lines.AddRange(
                    new[]
                    {
                        $"/// <summary>{modelInfo.ColumnLogicalName}容量最大値</summary>",
                        $"[Pure]",
                        $"public static int Max{modelInfo.ColumnPhysicalName}Capacity => {modelInfo.MaxColumnCapacity};",
                        $"/// <summary>{modelInfo.ColumnLogicalName}容量最小値</summary>",
                        $"[Pure]",
                        $"public static int Min{modelInfo.ColumnPhysicalName}Capacity => {modelInfo.MinColumnCapacity};",
                    }
                );
            }

            return SourceTextFormatter.Format(
                __,
                lines.ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassEventSource_Standalone(
            ModelInformation _
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public event NotifyCollectionChangedEventHandler? CollectionChanged",
                $"{{",
                $"{__}add => collectionChanged += value;",
                $"{__}remove => collectionChanged -= value;",
                $"}}",
                $"",
                $"private event NotifyCollectionChangedEventHandler? collectionChanged;"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassPropertiesSource_Standalone(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <summary>",
                $"/// {__}{modelInfo.RowLogicalName}インデクサによるアクセス",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <returns>指定した{modelInfo.RowLogicalName}インデックスの{modelInfo.RowLogicalName}要素（長さ固定型）</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"[Pure]",
                $"public {modelInfo.FixedLengthRowType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index]",
                $"{__}=> Get{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}{modelInfo.CellLogicalName}インデクサによるアクセス",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定した{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}インデックスの{modelInfo.CellLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><see langword=\"null\"/> をセットしようとした場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"public {modelInfo.ElementType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index]",
                $"{{",
                $"{__}[Pure]",
                $"{__}get => Get{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"{__}set => Set{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, value);",
                $"}}",
                $"",
                $"/// <summary>{modelInfo.RowLogicalName}数</summary>",
                $"[Pure]",
                $"public int {modelInfo.RowPhysicalName}Count => Table.RowCount;",
                $"",
                $"/// <summary>{modelInfo.ColumnLogicalName}数</summary>",
                $"[Pure]",
                $"public int {modelInfo.ColumnPhysicalName}Count => Table.ColumnCount;",
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"[Pure]",
                $"public IList<{modelInfo.RowSettingsType}> Settings => Table.Select(row => ({modelInfo.RowSettingsType})row).ToArray();",
                $"",
                $"private protected TwoDimensionalList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}> Table {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassConstructorSource_Standalone(
            ModelInformation modelInfo
        )
        {
            var (maxRowCapacity, minRowCapacity) = modelInfo.MaxRowCapacity == modelInfo.MinRowCapacity
                ? ($"{modelInfo.RowPhysicalName}Capacity", $"{modelInfo.RowPhysicalName}Capacity")
                : ($"Max{modelInfo.RowPhysicalName}Capacity", $"Min{modelInfo.RowPhysicalName}Capacity");

            var (maxColumnCapacity, minColumnCapacity) = modelInfo.MaxColumnCapacity == modelInfo.MinColumnCapacity
                ? ($"{modelInfo.ColumnPhysicalName}Capacity", $"{modelInfo.ColumnPhysicalName}Capacity")
                : ($"Max{modelInfo.ColumnPhysicalName}Capacity", $"Min{modelInfo.ColumnPhysicalName}Capacity");

            return SourceTextFormatter.Format(
                __,
                $"private protected {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings, SimpleList<{modelInfo.RowType}> itemsImpl)",
                $"{{",
                $"{__}var validator = BuildValidator(settings, itemsImpl);",
                $"{__}validator?.Constructor((nameof(settings), settings));",
                $"{__}Table =",
                $"{__}{__}new TwoDimensionalList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}>(",
                $"{__}{__}{__}itemsImpl,",
                $"{__}{__}{__}new TwoDimensionalList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}>.Config(",
                $"{__}{__}{__}{__}BuildRowSettingsFromRowIndex,",
                $"{__}{__}{__}{__}BuildRowFromSettings,",
                $"{__}{__}{__}{__}BuildListElementFromSetting,",
                $"{__}{__}{__}{__}BuildValidator(settings, itemsImpl)",
                $"{__}{__}{__})",
                $"{__}{__}{__}{{",
                $"{__}{__}{__}{__}MaxRowCapacity = {maxRowCapacity},",
                $"{__}{__}{__}{__}MinRowCapacity = {minRowCapacity},",
                $"{__}{__}{__}{__}MaxColumnCapacity = {maxColumnCapacity},",
                $"{__}{__}{__}{__}MinColumnCapacity = {minColumnCapacity},",
                $"{__}{__}{__}}}",
                $"{__}{__});",
                $"{__}PropagatePropertyChangeEvent(Table);",
                $"{__}PropagateCollectionChangeEvent(Table);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassMethodsSource_Standalone(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public IEnumerator<{modelInfo.FixedLengthRowType}> GetEnumerator() => Table.GetEnumerator();",
                $"",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
                $"",
                SourceTextFormatter.If(
                    modelInfo.MaxRowCapacity == modelInfo.MinRowCapacity,
                    "",
                    $"/// <summary>{modelInfo.RowLogicalName}容量を取得する。</summary>",
                    $"/// <returns>{modelInfo.RowLogicalName}容量</returns>",
                    $"[Pure]",
                    $"public int Get{modelInfo.RowPhysicalName}Capacity() => {modelInfo.RowPhysicalName}Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxRowCapacity != modelInfo.MinRowCapacity,
                    "",
                    $"/// <summary>{modelInfo.RowLogicalName}容量最大値を取得する。</summary>",
                    $"/// <returns>{modelInfo.RowLogicalName}容量最大値</returns>",
                    $"[Pure]",
                    $"public int GetMax{modelInfo.RowPhysicalName}Capacity() => Max{modelInfo.RowPhysicalName}Capacity;",
                    $"/// <summary>{modelInfo.RowLogicalName}容量最小値を取得する。</summary>",
                    $"/// <returns>{modelInfo.RowLogicalName}容量最小値</returns>",
                    $"[Pure]",
                    $"public int GetMin{modelInfo.RowPhysicalName}Capacity() => Min{modelInfo.RowPhysicalName}Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxColumnCapacity == modelInfo.MinColumnCapacity,
                    "",
                    $"/// <summary>{modelInfo.ColumnLogicalName}容量を取得する。</summary>",
                    $"/// <returns>{modelInfo.ColumnLogicalName}容量</returns>",
                    $"[Pure]",
                    $"public int Get{modelInfo.ColumnPhysicalName}Capacity() => {modelInfo.ColumnPhysicalName}Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxColumnCapacity != modelInfo.MinColumnCapacity,
                    "",
                    $"/// <summary>{modelInfo.ColumnLogicalName}容量最大値を取得する。</summary>",
                    $"/// <returns>{modelInfo.ColumnLogicalName}容量最大値</returns>",
                    $"[Pure]",
                    $"public int GetMax{modelInfo.ColumnPhysicalName}Capacity() => Max{modelInfo.ColumnPhysicalName}Capacity;",
                    $"/// <summary>{modelInfo.ColumnLogicalName}容量最小値を取得する。</summary>",
                    $"/// <returns>{modelInfo.ColumnLogicalName}容量最小値</returns>",
                    $"[Pure]",
                    $"public int GetMin{modelInfo.ColumnPhysicalName}Capacity() => Min{modelInfo.ColumnPhysicalName}Capacity;"
                ),
                $"",
                $"/// <summary>",
                $"/// {__}指定{modelInfo.RowLogicalName}インデックスの{modelInfo.RowLogicalName}要素を取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <returns>指定行の{modelInfo.RowLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"[Pure]",
                $"public {modelInfo.FixedLengthRowType} Get{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => Table.GetRow({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}指定範囲の{modelInfo.RowLogicalName}要素を簡易コピーしたリストを取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/>)] {modelInfo.RowLogicalName}数</param>",
                $"/// <returns>指定範囲の{modelInfo.RowLogicalName}要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の{modelInfo.RowLogicalName}要素を取得しようとした場合。</exception>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Get{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetRowRange({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}指定{modelInfo.ColumnLogicalName}インデックスの{modelInfo.ColumnLogicalName}要素を取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}Count\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定{modelInfo.ColumnLogicalName}の要素リスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> Get{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.GetColumn({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}指定範囲の{modelInfo.ColumnLogicalName}要素を簡易コピーしたリストを取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}Count\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}Count\"/>)] {modelInfo.ColumnLogicalName}数</param>",
                $"/// <returns>指定範囲の{modelInfo.ColumnLogicalName}要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の{modelInfo.ColumnLogicalName}要素を取得しようとした場合。</exception>",
                $"[Pure]",
                $"public IEnumerable<IEnumerable<{modelInfo.ElementType}>> Get{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetColumnRange({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}指定{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}インデックスの{modelInfo.CellLogicalName}要素を取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}Count\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}の{modelInfo.CellLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"[Pure]",
                $"public {modelInfo.ElementType} Get{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.GetCell({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}二次元リストの{modelInfo.RowLogicalName}要素を更新する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] 更新{modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"settings\">更新{modelInfo.RowLogicalName}要素</param>",
                $"/// <returns>セットした{modelInfo.RowLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\">",
                $"/// {__}<paramref name=\"settings\"/> が <see langword=\"null\"/> の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">",
                $"/// {__}有効な範囲外の{modelInfo.RowLogicalName}要素を編集しようとした場合。",
                $"/// </exception>",
                $"public {modelInfo.FixedLengthRowType} Set{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.RowSettingsType} settings)",
                $"{__}=> Table.SetRow({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}二次元リストの連続した{modelInfo.RowLogicalName}要素を更新する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] 更新開始{modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"settings\">更新{modelInfo.RowLogicalName}要素</param>",
                $"/// <returns>セットした{modelInfo.RowLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\">",
                $"/// {__}<paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、",
                $"/// {__}または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">",
                $"/// {__}有効な範囲外の{modelInfo.RowLogicalName}要素を編集しようとした場合。",
                $"/// </exception>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Set{modelInfo.RowPhysicalName}Range(",
                $"{__}int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<{modelInfo.RowSettingsType}> settings",
                $") => Table.SetRowRange({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}二次元リストの{modelInfo.ColumnLogicalName}要素を更新する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)] 更新{modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <param name=\"settings\">更新{modelInfo.ColumnLogicalName}要素</param>",
                $"/// <returns>セットした{modelInfo.ColumnLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\">",
                $"/// {__}<paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、",
                $"/// {__}または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">",
                $"/// {__}有効な範囲外の{modelInfo.ColumnLogicalName}要素を編集しようとした場合。",
                $"/// </exception>",
                $"public IEnumerable<{modelInfo.ElementType}> Set{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, IEnumerable<{modelInfo.ElementSettingsType}> settings)",
                $"{__}=> Table.SetColumn({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}二次元リストの連続した{modelInfo.ColumnLogicalName}要素を更新する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)] 更新開始{modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <param name=\"settings\">更新{modelInfo.ColumnLogicalName}要素（外側のIEnumerableが{modelInfo.ColumnLogicalName}、内側のIEnumerableが各{modelInfo.ColumnLogicalName}の{modelInfo.RowLogicalName}要素）</param>",
                $"/// <returns>セットした{modelInfo.ColumnLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\">",
                $"/// {__}<paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、",
                $"/// {__}または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">",
                $"/// {__}有効な範囲外の{modelInfo.ColumnLogicalName}要素を編集しようとした場合。",
                $"/// </exception>",
                $"public IEnumerable<IEnumerable<{modelInfo.ElementType}>> Set{modelInfo.ColumnPhysicalName}Range(",
                $"{__}int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<IEnumerable<{modelInfo.ElementSettingsType}>> settings",
                $") => Table.SetColumnRange({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}二次元リストの{modelInfo.CellLogicalName}要素を更新する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <param name=\"settings\">更新{modelInfo.CellLogicalName}要素</param>",
                $"/// <returns>セットした{modelInfo.CellLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\">",
                $"/// {__}<paramref name=\"settings\"/> が <see langword=\"null\"/> の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">",
                $"/// {__}有効な範囲外の{modelInfo.CellLogicalName}要素を編集しようとした場合。",
                $"/// </exception>",
                $"public {modelInfo.ElementType} Set{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ElementSettingsType} settings)",
                $"{__}=> Table.SetCell({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}指定した{modelInfo.RowLogicalName}インデックスにある項目をコレクション内の新しい場所へ移動する。",
                $"/// </summary>",
                $"/// <param name=\"old{modelInfo.RowPhysicalName}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] 移動する{modelInfo.RowLogicalName}のインデックス</param>",
                $"/// <param name=\"new{modelInfo.RowPhysicalName}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)] 移動先の{modelInfo.RowLogicalName}インデックス</param>",
                $"/// <exception cref=\"InvalidOperationException\">",
                $"/// {__}自身の{modelInfo.RowLogicalName}数が0の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"old{modelInfo.RowPhysicalName}Index\"/>, <paramref name=\"new{modelInfo.RowPhysicalName}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"public void Move{modelInfo.RowPhysicalName}(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index) => Table.MoveRow(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}指定した{modelInfo.RowLogicalName}インデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。",
                $"/// </summary>",
                $"/// <param name=\"old{modelInfo.RowPhysicalName}Index\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)]",
                $"/// {__}移動する{modelInfo.RowLogicalName}のインデックス開始位置",
                $"/// </param>",
                $"/// <param name=\"new{modelInfo.RowPhysicalName}Index\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/> - 1)]",
                $"/// {__}移動先の{modelInfo.RowLogicalName}インデックス開始位置",
                $"/// </param>",
                $"/// <param name=\"count\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/>)]",
                $"/// {__}移動させる{modelInfo.RowLogicalName}数",
                $"/// </param>",
                $"/// <exception cref=\"InvalidOperationException\">",
                $"/// {__}自身の{modelInfo.RowLogicalName}数が0の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"old{modelInfo.RowPhysicalName}Index\"/>, <paramref name=\"new{modelInfo.RowPhysicalName}Index\"/>, <paramref name=\"count\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の{modelInfo.RowLogicalName}要素を移動しようとした場合。</exception>",
                $"public void Move{modelInfo.RowPhysicalName}Range(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index, int count)",
                $"{__}=> Table.MoveRowRange(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}指定した{modelInfo.ColumnLogicalName}インデックスにある項目をコレクション内の新しい場所へ移動する。",
                $"/// </summary>",
                $"/// <param name=\"old{modelInfo.ColumnPhysicalName}Index\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)]",
                $"/// {__}移動する{modelInfo.ColumnLogicalName}のインデックス",
                $"/// </param>",
                $"/// <param name=\"new{modelInfo.ColumnPhysicalName}Index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)] 移動先の{modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <exception cref=\"InvalidOperationException\">",
                $"/// {__}自身の{modelInfo.ColumnLogicalName}数が0の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"old{modelInfo.ColumnPhysicalName}Index\"/>, <paramref name=\"new{modelInfo.ColumnPhysicalName}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"public void Move{modelInfo.ColumnPhysicalName}(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index)",
                $"{__}=> Table.MoveColumn(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}指定した{modelInfo.ColumnLogicalName}インデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。",
                $"/// </summary>",
                $"/// <param name=\"old{modelInfo.ColumnPhysicalName}Index\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)]",
                $"/// {__}移動する{modelInfo.ColumnLogicalName}のインデックス開始位置",
                $"/// </param>",
                $"/// <param name=\"new{modelInfo.ColumnPhysicalName}Index\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> - 1)]",
                $"/// {__}移動先の{modelInfo.ColumnLogicalName}インデックス開始位置",
                $"/// </param>",
                $"/// <param name=\"count\">",
                $"/// {__}[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/>)]",
                $"/// {__}移動させる{modelInfo.ColumnLogicalName}数",
                $"/// </param>",
                $"/// <exception cref=\"InvalidOperationException\">",
                $"/// {__}自身の{modelInfo.ColumnLogicalName}数が0の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"old{modelInfo.ColumnPhysicalName}Index\"/>, <paramref name=\"new{modelInfo.ColumnPhysicalName}Index\"/>, <paramref name=\"count\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の{modelInfo.ColumnLogicalName}要素を移動しようとした場合。</exception>",
                $"public void Move{modelInfo.ColumnPhysicalName}Range(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index, int count)",
                $"{__}=> Table.MoveColumnRange(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}要素を与えられた内容で一新する。",
                $"/// </summary>",
                $"/// <param name=\"settings\">二次元リストに詰め直す要素</param>",
                $"/// <returns>新たに二次元リストに詰め直した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\">",
                $"/// {__}<paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、",
                $"/// {__}または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentException\">",
                $"/// {__}<paramref name=\"settings\"/> の{modelInfo.RowLogicalName}数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.RowPhysicalName}Count\"/>、",
                $"/// {__}{modelInfo.ColumnLogicalName}数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.{modelInfo.ColumnPhysicalName}Count\"/> と異なる場合。",
                $"/// </exception>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> ResetStrict(",
                $"{__}IEnumerable<{modelInfo.RowSettingsType}> settings",
                $") => Table.ResetStrict(settings);",
                $"",
                $"/// <summary>",
                $"/// {__}要素をデフォルト値で一新する。",
                $"/// </summary>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Reset() => Table.Reset();",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => Table.ValidateGetRow({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count) => Table.ValidateGetRowRange({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.ValidateGetColumn({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.ValidateGetColumnRange({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.CellPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.CellPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.ValidateGetCell({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.RowSettingsType} settings)",
                $"{__}=> Table.ValidateSetRow({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, IEnumerable<{modelInfo.RowSettingsType}> settings)",
                $"{__}=> Table.ValidateSetRowRange({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, IEnumerable<{modelInfo.ElementSettingsType}> settings)",
                $"{__}=> Table.ValidateSetColumn({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, IEnumerable<IEnumerable<{modelInfo.ElementSettingsType}>> settings)",
                $"{__}=> Table.ValidateSetColumnRange({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.CellPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.CellPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ElementSettingsType} settings)",
                $"{__}=> Table.ValidateSetCell({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.RowPhysicalName}(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index)",
                $"{__}=> Table.ValidateMoveRow(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.RowPhysicalName}Range(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index, int count)",
                $"{__}=> Table.ValidateMoveRowRange(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.ColumnPhysicalName}(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index)",
                $"{__}=> Table.ValidateMoveColumn(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.ColumnPhysicalName}Range(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index, int count)",
                $"{__}=> Table.ValidateMoveColumnRange(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"ResetStrict\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"ResetStrict\" path=\"param|exception\"/>",
                $"public void ValidateResetStrict(IEnumerable<{modelInfo.RowSettingsType}> settings)",
                $"{__}=> Table.ValidateResetStrict(settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Reset()\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|exception\"/>",
                $"public void ValidateReset() => Table.ValidateReset();",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.FixedLengthRowType} Get{modelInfo.RowPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => Table.GetRowInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Get{modelInfo.RowPhysicalName}RangeInternal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetRowRangeInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> Get{modelInfo.ColumnPhysicalName}Internal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> Table.GetColumnInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<IEnumerable<{modelInfo.ElementType}>> Get{modelInfo.ColumnPhysicalName}RangeInternal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetColumnRangeInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.CellPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.CellPhysicalName}\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.ElementType} Get{modelInfo.CellPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> Table.GetCellInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"public {modelInfo.FixedLengthRowType} Set{modelInfo.RowPhysicalName}Internal(",
                $"{__}int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}{modelInfo.RowSettingsType} settings",
                $") => Table.SetRowInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Set{modelInfo.RowPhysicalName}RangeInternal(",
                $"{__}int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<{modelInfo.RowSettingsType}> settings",
                $") => Table.SetRowRangeInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> Set{modelInfo.ColumnPhysicalName}Internal(",
                $"{__}int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<{modelInfo.ElementSettingsType}> settings",
                $") => Table.SetColumnInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"public IEnumerable<IEnumerable<{modelInfo.ElementType}>> Set{modelInfo.ColumnPhysicalName}RangeInternal(",
                $"{__}int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<IEnumerable<{modelInfo.ElementSettingsType}>> settings",
                $") => Table.SetColumnRangeInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.CellPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.CellPhysicalName}\" path=\"param\"/>",
                $"public {modelInfo.ElementType} Set{modelInfo.CellPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ElementSettingsType} settings)",
                $"{__}=> Table.SetCellInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"public void Move{modelInfo.RowPhysicalName}Internal(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index)",
                $"{__}=> Table.MoveRowInternal(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"public void Move{modelInfo.RowPhysicalName}RangeInternal(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index, int count)",
                $"{__}=> Table.MoveRowRangeInternal(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"public void Move{modelInfo.ColumnPhysicalName}Internal(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index)",
                $"{__}=> Table.MoveColumnInternal(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"public void Move{modelInfo.ColumnPhysicalName}RangeInternal(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index, int count)",
                $"{__}=> Table.MoveColumnRangeInternal(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"ResetStrict\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"ResetStrict\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> ResetStrictInternal(",
                $"{__}IEnumerable<{modelInfo.RowSettingsType}> settings",
                $") => Table.ResetStrictInternal(settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Reset()\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> ResetInternal() => Table.ResetInternal();"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassItemEqualsSource_Standalone(
            ModelInformation modelInfo
        )
        {
            var objectItemEqualsKeyword = (modelInfo.IsExtendClass, modelInfo.IsAbstract) switch
            {
                (true, _) => "override ",
                (_, true) => "virtual ",
                (_, false) => "",
            };

            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public {objectItemEqualsKeyword}bool ItemEquals(object? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassDeepCloneSource_Standalone(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object IDeepCloneable.DeepClone() => DeepClone();"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassSettingsInterfaceImplementsSource_Standalone(
            ModelInformation modelInfo
        )
        {
            var targetProperties =
                modelInfo.Members.SettingsProperties.Where(definition => definition.IsOverrideReturnType);

            return SourceTextFormatter.Format(
                __,
                targetProperties.Select(definition => definition.GetInterfaceImplementCode)
                    .ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassPrivateMethodSource_Standalone(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.If(
                !modelInfo.IsAbstract,
                __,
                $"/// <summary>",
                $"///     <see cref=\"ExtendedList{{TListSettings, TEditableElement, TReadOnlyElement, TElementSettings}}\"/> が通知した",
                $"///     <see cref=\"INotifyCollectionChanged\"/> イベントを",
                $"///     自身のイベントとして通知する。",
                $"/// </summary>",
                $"/// <param name=\"target\">対象</param>",
                $"private void PropagateCollectionChangeEvent(TwoDimensionalList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}> target)",
                $"{{",
                $"    target.CollectionChanged += (_, args) => {{ collectionChanged?.Invoke(this, args); }};",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildImplicitTypeConversionOperatorSource_Standalone(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"private {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? readonlyInstance = null;",
                $"",
                $"/// <summary>",
                $"/// {__}読取専用クラスへの暗黙的型変換",
                $"/// </summary>",
                $"/// <param name=\"src\">変換元</param>",
                $"/// <returns>変換したインスタンス</returns>",
                $"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]",
                $"public static implicit operator {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}?({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? src)",
                $"{{",
                $"{__}if (src is null) return null;",
                $"{__}src.readonlyInstance ??= new {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}(src);",
                $"{__}return src.readonlyInstance;",
                $"}}"
            );
        }
    }
}
