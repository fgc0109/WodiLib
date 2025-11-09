// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TwoDListGeneratorBase.BuildReadOnlyClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
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
                $"{__}IEnumerable<{modelInfo.ReadOnlyRowType}>,",
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
                BuildReadOnlyClassConstantsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Events
                BuildReadOnlyClassEventSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Properties
                BuildReadOnlyClassPropertiesSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructor
                BuildReadOnlyClassConstructorsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildReadOnlyClassMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // ItemEquals
                BuildReadOnlyClassItemEqualsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildReadOnlyClassPropDeepCloneSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Private Methods
                BuildReadOnlyClassPrivateMethodSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Settings Interface Implements
                BuildReadOnlyClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Implicit Type Conversion Operator
                BuildReadOnlyClassImplicitTypeConversionOperatorSource(modelInfo),
                // class end
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassConstantsSource(
            ModelInformation _
        )
        {
            return SourceTextFormatter.Format(
                __
                // 何も出力しない
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassEventSource(
            ModelInformation _
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public event NotifyCollectionChangedEventHandler? CollectionChanged",
                $"{{",
                $"{__}add => MutableInstance.CollectionChanged += value;",
                $"{__}remove => MutableInstance.CollectionChanged -= value;",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassPropertiesSource(
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
                $"[Pure]",
                $"public {modelInfo.ReadOnlyRowType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index] => Get{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>{modelInfo.CellLogicalName}インデクサによるアクセス</summary>",
                $"/// <param name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.RowPhysicalName}Count\"/> - 1)] {modelInfo.RowLogicalName}インデックス</param>",
                $"/// <param name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\">[Range(0, <see cref=\"{modelInfo.ColumnPhysicalName}Count\"/> - 1)] {modelInfo.ColumnLogicalName}インデックス</param>",
                $"/// <returns>指定した{modelInfo.RowLogicalName}・{modelInfo.ColumnLogicalName}インデックスの{modelInfo.CellLogicalName}要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\">",
                $"/// {__}<paramref name=\"{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index\"/>, <paramref name=\"{modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index\"/>が指定範囲外の場合。",
                $"/// </exception>",
                $"[Pure]",
                $"public {modelInfo.ReadOnlyElementType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index] => Get{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>{modelInfo.RowLogicalName}数</summary>",
                $"[Pure]",
                $"public int {modelInfo.RowPhysicalName}Count => MutableInstance.{modelInfo.RowPhysicalName}Count;",
                $"",
                $"/// <summary>{modelInfo.ColumnLogicalName}数</summary>",
                $"[Pure]",
                $"public int {modelInfo.ColumnPhysicalName}Count => MutableInstance.{modelInfo.ColumnPhysicalName}Count;",
                $"",
                modelInfo.Members.ReadOnlyListProperties.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"[Pure]",
                $"public IList<{modelInfo.RowSettingsType}> Settings => MutableInstance.Settings;",
                $"",
                $"internal {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword} MutableInstance {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassConstructorsSource(
            ModelInformation modelInfo
        )
        {
            // 読取専用リスト単体ではインスタンス作成不可
            return SourceTextFormatter.Format(
                __,
                $"internal {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword} mutableInstance)",
                $"{{",
                $"{__}MutableInstance = mutableInstance;",
                $"{__}PropagatePropertyChangeEvent(MutableInstance);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public IEnumerator<{modelInfo.ReadOnlyRowType}> GetEnumerator() => MutableInstance.Select(row => row.Cast<{modelInfo.FixedLengthRowType}, {modelInfo.ReadOnlyRowType}>()).GetEnumerator();",
                $"",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
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
                $"public {modelInfo.ReadOnlyRowType} Get{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
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
                $"public IEnumerable<{modelInfo.ReadOnlyRowType}> Get{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.Get{modelInfo.RowPhysicalName}Range({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count).Select(row => row.Cast<{modelInfo.FixedLengthRowType}, {modelInfo.ReadOnlyRowType}>());",
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
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> Get{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.ColumnPhysicalName}({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index).Select(column => column.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>());",
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
                $"public IEnumerable<IEnumerable<{modelInfo.ReadOnlyElementType}>> Get{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.Get{modelInfo.ColumnPhysicalName}Range({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count).Select(columns => columns.Select(column => column.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>()));",
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
                $"public {modelInfo.ReadOnlyElementType} Get{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index).Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>();",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.ValidateGet{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count) => MutableInstance.ValidateGet{modelInfo.RowPhysicalName}Range({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.ValidateGet{modelInfo.ColumnPhysicalName}({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.ValidateGet{modelInfo.ColumnPhysicalName}Range({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.CellPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.CellPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateGet{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.ValidateGet{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.ReadOnlyRowType} Get{modelInfo.RowPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.RowPhysicalName}Internal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ReadOnlyRowType}> Get{modelInfo.RowPhysicalName}RangeInternal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.Get{modelInfo.RowPhysicalName}RangeInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count).Select(row => row.Cast<{modelInfo.FixedLengthRowType}, {modelInfo.ReadOnlyRowType}>());",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> Get{modelInfo.ColumnPhysicalName}Internal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> MutableInstance.Get{modelInfo.ColumnPhysicalName}Internal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index).Select(column => column.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>());",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<IEnumerable<{modelInfo.ReadOnlyElementType}>> Get{modelInfo.ColumnPhysicalName}RangeInternal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.Get{modelInfo.ColumnPhysicalName}RangeInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count).Select(columns => columns.Select(column => column.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>()));",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Get{modelInfo.CellPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Get{modelInfo.CellPhysicalName}\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.ReadOnlyElementType} Get{modelInfo.CellPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> MutableInstance.Get{modelInfo.CellPhysicalName}Internal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index).Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>();",
                $"",
                modelInfo.Members.ReadOnlyListMethods.SelectMany(x => x.ImplementationCode).ToArray()
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
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? other)",
                $"{__}=> MutableInstance.ItemEquals(other);",
                $"",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? other)",
                $"{__}=> MutableInstance.ItemEquals(other);",
                $"",
                SourceTextFormatter.If(
                    !modelInfo.IsFixed,
                    "",
                    $"/// <inheritdoc/>",
                    $"[Pure]",
                    $"public bool ItemEquals({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? other)",
                    $"{__}=> MutableInstance.ItemEquals(other);",
                    $""
                ),
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}? other)",
                $"{__}=> MutableInstance.ItemEquals(other);",
                $"",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => MutableInstance.ItemEquals(other);"
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
                $"[Pure]",
                $"public {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword} DeepClone() => new(MutableInstance.DeepClone());",
                $"object IDeepCloneable.DeepClone() => DeepClone();"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassSettingsInterfaceImplementsSource(
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

        private static SourceFormatTargetBlock BuildReadOnlyClassPrivateMethodSource(
            ModelInformation _
        )
        {
            return SourceTextFormatter.Format(
                __
                // 何も出力しない
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyClassImplicitTypeConversionOperatorSource(
            ModelInformation _
        )
        {
            return SourceTextFormatter.Format(
                __
                // 何も出力しない
            );
        }
    }
}
