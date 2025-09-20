// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TwoDListGeneratorBase.BuildReadOnlyClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.ValueObject.Extensions;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal abstract partial class TwoDListGeneratorBase
    {
        private static SourceFormatTargetBlock BuildReadOnlyClassSource(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                // class start
                $"/// <summary>",
                $"/// {__}【読取専用】{modelInfo.Description}",
                $"/// </summary>",
                $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.ReadOnlyListInfo.ReadOnlyListClassName} : ModelBase,",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                $"{__}IReadOnlyList<{modelInfo.ReadOnlyRowType}>,",
                $"{__}INotifyCollectionChanged,",
                $"{__}IEqualityComparable<{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}>,",
                $"{__}IEqualityComparable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>,",
                SourceTextFormatter.If(
                    !modelInfo.IsFixed,
                    __,
                    $"IEqualityComparable<{modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}>,"
                ),
                $"{__}IDeepCloneable<{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}>",
                $"{{",
                // Constants
                BuildReadOnlyListConstantsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Events
                BuildEventSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructor
                BuildReadOnlyListConstructorsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Properties
                BuildReadOnlyListPropertiesSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildReadOnlyListMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // ItemEquals
                BuildReadOnlyClassItemEqualsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildReadOnlyClassPropDeepCloneSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Private Methods
                BuildPrivateMethodSource(modelInfo),
                // class end
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyListConstantsSource(
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
                        $"public static int Max{modelInfo.RowPhysicalName}Capacity => {modelInfo.MaxRowCapacity};",
                        $"/// <summary>{modelInfo.RowLogicalName}容量最小値</summary>",
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
                        $"public static int Max{modelInfo.ColumnPhysicalName}Capacity => {modelInfo.MaxColumnCapacity};",
                        $"/// <summary>{modelInfo.ColumnLogicalName}容量最小値</summary>",
                        $"public static int Min{modelInfo.ColumnPhysicalName}Capacity => {modelInfo.MinColumnCapacity};",
                    }
                );
            }

            return SourceTextFormatter.Format(
                __,
                lines.ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildEventSource(
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

        private static SourceFormatTargetBlock BuildReadOnlyListConstructorsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"private protected {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}(SimpleList<{modelInfo.RowType}> itemsImpl)",
                $"{{",
                $"{__}Table =",
                $"{__}{__}new TwoDimensionalList<{modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.ReadOnlyRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}>(",
                $"{__}{__}{__}itemsImpl,",
                $"{__}{__}{__}new ReadOnly2DList<{modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.ReadOnlyRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}>.Config(",
                $"{__}{__}{__}{__}BuildRowSettingsFromRowIndex,",
                $"{__}{__}{__}{__}BuildRowFromSettings,",
                $"{__}{__}{__}{__}BuildListElementFromSetting,",
                $"{__}{__}{__}{__}CompareElement,",
                $"{__}{__}{__}{__}BuildValidator(itemsImpl)",
                $"{__}{__}{__})",
                $"{__}{__}{__}{{",
                $"{__}{__}{__}{__}MaxRowCapacity = Max{modelInfo.RowPhysicalName}Capacity,",
                $"{__}{__}{__}{__}MinRowCapacity = Min{modelInfo.RowPhysicalName}Capacity,",
                $"{__}{__}{__}{__}MaxColumnCapacity = Max{modelInfo.ColumnPhysicalName}Capacity,",
                $"{__}{__}{__}{__}MinColumnCapacity = Min{modelInfo.ColumnPhysicalName}Capacity,",
                $"{__}{__}{__}}}",
                $"{__}{__});",
                $"{__}PropagatePropertyChangeEvent(Table);",
                $"{__}PropagateCollectionChangeEvent(Table);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyListPropertiesSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <summary>{modelInfo.RowLogicalName}インデクサによるアクセス</summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <returns>指定した{modelInfo.RowLogicalName}インデックスの{modelInfo.RowLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。",
                $"/// </exception>",
                $"public {modelInfo.ReadOnlyRowType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index] => Get{modelInfo.RowPhysicalName}( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>{modelInfo.CellLogicalName}インデクサによるアクセス</summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定した{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}インデックスの{modelInfo.CellLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。",
                $"/// </exception>",
                $"public {modelInfo.ReadOnlyElementType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index] => Get{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>{modelInfo.RowLogicalName}数</summary>",
                $"public int {modelInfo.RowPhysicalName}Count => Table.RowCount;",
                $"",
                $"/// <summary>{modelInfo.ColumnLogicalName}数</summary>",
                $"public int {modelInfo.ColumnPhysicalName} => Table.ColumnCount;",
                $"",
                $"/// <summary>すべての編集可能型{modelInfo.RowLogicalName}要素</summary>",
                $"public {modelInfo.ReadOnlyRowType}[] Editable{modelInfo.RowPhysicalName}s => Table.EditableRows;",
                $"",
                $"/// <inheritdoc/>",
                $"public IReadOnlyList<{modelInfo.RowSettingsType}> Settings => Table;",
                $"",
                $"int IReadOnlyCollection<{modelInfo.ReadOnlyRowType}>.Count => {modelInfo.RowPhysicalName}Count;",
                $"",
                $"private protected TwoDimensionalList<{modelInfo.RowType}, {modelInfo.FixedLengthRowType}, {modelInfo.ReadOnlyRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}> Table {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyListMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public IEnumerator<{modelInfo.ReadOnlyRowType}> GetEnumerator() => Table.GetEnumerator();",
                $"",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
                $"",
                SourceTextFormatter.If(
                    modelInfo.MaxRowCapacity == modelInfo.MinRowCapacity,
                    "",
                    $"/// <summary>{modelInfo.RowLogicalName}容量を取得する。</summary>",
                    $"/// <returns>{modelInfo.RowLogicalName}容量</returns>",
                    $"public int Get{modelInfo.RowPhysicalName}Capacity() => {modelInfo.RowPhysicalName}Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxRowCapacity != modelInfo.MinRowCapacity,
                    "",
                    $"/// <summary>{modelInfo.RowLogicalName}容量最大値を取得する。</summary>",
                    $"/// <returns>{modelInfo.RowLogicalName}容量最大値</returns>",
                    $"public int GetMax{modelInfo.RowPhysicalName}Capacity() => Max{modelInfo.RowPhysicalName}Capacity;",
                    $"/// <summary>{modelInfo.RowLogicalName}容量最小値を取得する。</summary>",
                    $"/// <returns>{modelInfo.RowLogicalName}容量最小値</returns>",
                    $"public int GetMin{modelInfo.RowPhysicalName}Capacity() => Min{modelInfo.RowPhysicalName}Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxColumnCapacity == modelInfo.MinColumnCapacity,
                    "",
                    $"/// <summary>{modelInfo.ColumnLogicalName}容量を取得する。</summary>",
                    $"/// <returns>{modelInfo.ColumnLogicalName}容量</returns>",
                    $"public int Get{modelInfo.ColumnPhysicalName}Capacity() => {modelInfo.ColumnPhysicalName}Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxColumnCapacity != modelInfo.MinColumnCapacity,
                    "",
                    $"/// <summary>{modelInfo.ColumnLogicalName}容量最大値を取得する。</summary>",
                    $"/// <returns>{modelInfo.ColumnLogicalName}容量最大値</returns>",
                    $"public int GetMax{modelInfo.ColumnPhysicalName}Capacity() => Max{modelInfo.ColumnPhysicalName}Capacity;",
                    $"/// <summary>{modelInfo.ColumnLogicalName}容量最小値を取得する。</summary>",
                    $"/// <returns>{modelInfo.ColumnLogicalName}容量最小値</returns>",
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
                $"public {modelInfo.ReadOnlyRowType} Get{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => Table.GetRow( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
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
                $"public IEnumerable<{modelInfo.ReadOnlyRowType}> Get{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetRowRange( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}指定{modelInfo.ColumnLogicalName}インデックスの{modelInfo.ColumnLogicalName}要素を取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定{modelInfo.ColumnLogicalName}の要素リスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> Get{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.GetColumn({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}指定範囲の{modelInfo.ColumnLogicalName}要素を簡易コピーしたリストを取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}\"/>)] {modelInfo.ColumnLogicalName}数</param>",
                $"/// <returns>指定範囲の{modelInfo.ColumnLogicalName}要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。",
                $"/// </exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の{modelInfo.ColumnLogicalName}要素を取得しようとした場合。</exception>",
                $"public IEnumerable<IEnumerable<{modelInfo.ReadOnlyElementType}>> Get{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetColumnRange({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}指定{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}インデックスの{modelInfo.CellLogicalName}要素を取得する。",
                $"/// </summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}の{modelInfo.CellLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/> が指定範囲外の場合。",
                $"/// </exception>",
                $"public {modelInfo.ReadOnlyElementType} Get{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.GetCell( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => Table.ValidateGetRow( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count) => Table.ValidateGetRowRange( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
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
                $"public void ValidateGet{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => Table.ValidateGetCell( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"public {modelInfo.ReadOnlyRowType} Get{modelInfo.RowPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => Table.GetRowInternal( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ReadOnlyRowType}> Get{modelInfo.RowPhysicalName}RangeInternal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetRowRangeInternal( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> Get{modelInfo.ColumnPhysicalName}Internal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> Table.GetColumnInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"public IEnumerable<IEnumerable<{modelInfo.ReadOnlyElementType}>> Get{modelInfo.ColumnPhysicalName}RangeInternal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> Table.GetColumnRangeInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.CellPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.CellPhysicalName}\" path=\"param\"/>",
                $"public {modelInfo.ReadOnlyElementType} Get{modelInfo.CellPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> Table.GetCellInternal( {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassItemEqualsSource(
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
                $"public bool ItemEquals({modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? other)",
                $"{__}=> ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"",
                $"/// <inheritdoc/>",
                $"public bool ItemEquals({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? other)",
                $"{__}=> ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"",
                SourceTextFormatter.If(
                    !modelInfo.IsFixed,
                    "",
                    $"/// <inheritdoc/>",
                    $"public bool ItemEquals({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? other)",
                    $"{__}=> ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                    $""
                ),
                $"/// <inheritdoc/>",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassPropDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.If(
                !modelInfo.IsAbstract,
                __,
                $"/// <inheritdoc/>",
                $"public {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object IDeepCloneable.DeepClone() => DeepClone();"
            );
        }

        private static SourceFormatTargetBlock BuildPrivateMethodSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.If(
                !modelInfo.IsAbstract,
                __,
                $"/// <summary>",
                $"/// {__}<see cref=\"SimpleList{{T}}\"/> が通知した",
                $"/// {__}<see cref=\"INotifyCollectionChanged\"/> イベントを",
                $"/// {__}自身のイベントとして通知する。",
                $"/// </summary>",
                $"/// <param name=\"target\">対象</param>",
                $"private void PropagateCollectionChangeEvent(",
                $"{__}TwoDimensionalList<{modelInfo.RowType}, {modelInfo.FixedLengthRowType},",
                $"{__}{__}{modelInfo.ReadOnlyRowType}, {modelInfo.RowSettingsType}, {modelInfo.ElementType}, {modelInfo.ReadOnlyElementType},",
                $"{__}{__}{modelInfo.ElementSettingsType}> target",
                $")",
                $"{{",
                $"{__}target.CollectionChanged += (_, args) => {{ collectionChanged?.Invoke(this, args); }};",
                $"}}"
            );
        }
    }
}
