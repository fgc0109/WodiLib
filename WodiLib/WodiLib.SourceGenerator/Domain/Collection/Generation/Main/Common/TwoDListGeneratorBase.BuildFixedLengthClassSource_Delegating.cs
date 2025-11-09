// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : TwoDListGeneratorBase.BuildFixedLengthClassSource_Delegating.cs
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
        ///     容量制限ありクラスに処理を委譲する容量固定リストクラス定義を出力する。
        /// </summary>
        /// <param name="modelInfo"></param>
        /// <returns></returns>
        private static SourceFormatTargetBlock BuildFixedLengthClassSource_Delegating(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                // class start
                $"/// <summary>",
                $"/// {__}【容量固定】{modelInfo.Description}",
                $"/// </summary>",
                $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.FixedLengthListInfo.FixedLengthListClassName} : ModelBase,",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                $"{__}IEnumerable<{modelInfo.FixedLengthRowType}>,",
                $"{__}INotifyCollectionChanged,",
                $"{__}IEqualityComparable<{modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}>,",
                $"{__}IEqualityComparable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>,",
                $"{__}IEqualityComparable<{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}>,",
                $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>",
                $"{{",
                // Constants
                BuildFixedLengthClassConstantsSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Events
                BuildFixedLengthClassEventSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Properties
                BuildFixedLengthClassPropertiesSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildFixedLengthClassConstructorSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildFixedLengthClassMethodsSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // ItemEquals
                BuildFixedLengthClassItemEqualsSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildFixedLengthClassDeepCloneSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                BuildFixedLengthClassSettingsInterfaceImplementsSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Private Methods
                BuildFixedLengthClassPrivateMethodSource_Delegating(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Implicit Type Conversion Operator
                BuildFixedLengthClassImplicitTypeConversionOperatorSource_Delegating(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassConstantsSource_Delegating(
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

        private static SourceFormatTargetBlock BuildFixedLengthClassEventSource_Delegating(
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

        private static SourceFormatTargetBlock BuildFixedLengthClassPropertiesSource_Delegating(
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
                $"public {modelInfo.FixedLengthRowType} this[int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index] => MutableInstance[{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index];",
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
                $"{__}get => MutableInstance[{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index];",
                $"{__}set => MutableInstance[{modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index] = value;",
                $"}}",
                $"",
                $"/// <summary>{modelInfo.RowLogicalName}数</summary>",
                $"[Pure]",
                $"public int {modelInfo.RowPhysicalName}Count => MutableInstance.{modelInfo.RowPhysicalName}Count;",
                $"",
                $"/// <summary>{modelInfo.ColumnLogicalName}数</summary>",
                $"[Pure]",
                $"public int {modelInfo.ColumnPhysicalName}Count => MutableInstance.{modelInfo.ColumnPhysicalName}Count;",
                $"",
                modelInfo.Members.FixedLengthListProperties.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"[Pure]",
                $"public IList<{modelInfo.RowSettingsType}> Settings => MutableInstance.Settings;",
                $"",
                $"internal {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword} MutableInstance {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassConstructorSource_Delegating(
            ModelInformation modelInfo
        )
        {
            // 容量固定リスト単体ではインスタンス作成不可
            return SourceTextFormatter.Format(
                __,
                $"internal {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword} mutableInstance)",
                $"{{",
                $"{__}MutableInstance = mutableInstance;",
                $"{__}PropagatePropertyChangeEvent(MutableInstance);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassMethodsSource_Delegating(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                modelInfo.Members.FixedLengthListMethods.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public IEnumerator<{modelInfo.FixedLengthRowType}> GetEnumerator() => MutableInstance.GetEnumerator();",
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
                $"public {modelInfo.FixedLengthRowType} Get{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
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
                $"{__}=> MutableInstance.Get{modelInfo.RowPhysicalName}Range({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
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
                $"public IEnumerable<{modelInfo.ElementType}> Get{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.ColumnPhysicalName}({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
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
                $"{__}=> MutableInstance.Get{modelInfo.ColumnPhysicalName}Range({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
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
                $"public {modelInfo.ElementType} Get{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
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
                $"{__}=> MutableInstance.Set{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
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
                $") => MutableInstance.Set{modelInfo.RowPhysicalName}Range({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
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
                $"{__}=> MutableInstance.Set{modelInfo.ColumnPhysicalName}({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
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
                $") => MutableInstance.Set{modelInfo.ColumnPhysicalName}Range({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
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
                $"{__}=> MutableInstance.Set{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
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
                $"public void Move{modelInfo.RowPhysicalName}(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index) => MutableInstance.Move{modelInfo.RowPhysicalName}(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index);",
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
                $"{__}=> MutableInstance.Move{modelInfo.RowPhysicalName}Range(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index, count);",
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
                $"{__}=> MutableInstance.Move{modelInfo.ColumnPhysicalName}(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index);",
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
                $"{__}=> MutableInstance.Move{modelInfo.ColumnPhysicalName}Range(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index, count);",
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
                $") => MutableInstance.ResetStrict(settings);",
                $"",
                $"/// <summary>",
                $"/// {__}要素をデフォルト値で一新する。",
                $"/// </summary>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Reset() => MutableInstance.Reset();",
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
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.RowPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.RowSettingsType} settings)",
                $"{__}=> MutableInstance.ValidateSet{modelInfo.RowPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.RowPhysicalName}Range(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, IEnumerable<{modelInfo.RowSettingsType}> settings)",
                $"{__}=> MutableInstance.ValidateSet{modelInfo.RowPhysicalName}Range({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.ColumnPhysicalName}(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, IEnumerable<{modelInfo.ElementSettingsType}> settings)",
                $"{__}=> MutableInstance.ValidateSet{modelInfo.ColumnPhysicalName}({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.ColumnPhysicalName}Range(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, IEnumerable<IEnumerable<{modelInfo.ElementSettingsType}>> settings)",
                $"{__}=> MutableInstance.ValidateSet{modelInfo.ColumnPhysicalName}Range({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.CellPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.CellPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateSet{modelInfo.CellPhysicalName}(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ElementSettingsType} settings)",
                $"{__}=> MutableInstance.ValidateSet{modelInfo.CellPhysicalName}({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.RowPhysicalName}(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index)",
                $"{__}=> MutableInstance.ValidateMove{modelInfo.RowPhysicalName}(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.RowPhysicalName}Range(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index, int count)",
                $"{__}=> MutableInstance.ValidateMove{modelInfo.RowPhysicalName}Range(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.ColumnPhysicalName}(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index)",
                $"{__}=> MutableInstance.ValidateMove{modelInfo.ColumnPhysicalName}(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}Range\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}Range\" path=\"param|exception\"/>",
                $"public void ValidateMove{modelInfo.ColumnPhysicalName}Range(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index, int count)",
                $"{__}=> MutableInstance.ValidateMove{modelInfo.ColumnPhysicalName}Range(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"ResetStrict\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"ResetStrict\" path=\"param|exception\"/>",
                $"public void ValidateResetStrict(IEnumerable<{modelInfo.RowSettingsType}> settings)",
                $"{__}=> MutableInstance.ValidateResetStrict(settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Reset()\"/> メソッドの検証処理。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|exception\"/>",
                $"public void ValidateReset() => MutableInstance.ValidateReset();",
                $"",
                $"/// <inheritdoc cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Get{modelInfo.RowPhysicalName}Internal\"/>",
                $"[Pure]",
                $"public {modelInfo.FixedLengthRowType} Get{modelInfo.RowPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.RowPhysicalName}Internal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <inheritdoc cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Get{modelInfo.RowPhysicalName}RangeInternal\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Get{modelInfo.RowPhysicalName}RangeInternal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.Get{modelInfo.RowPhysicalName}RangeInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <inheritdoc cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Get{modelInfo.ColumnPhysicalName}Internal\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> Get{modelInfo.ColumnPhysicalName}Internal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index) => MutableInstance.Get{modelInfo.ColumnPhysicalName}Internal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <inheritdoc cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Get{modelInfo.ColumnPhysicalName}RangeInternal\"/>",
                $"[Pure]",
                $"public IEnumerable<IEnumerable<{modelInfo.ElementType}>> Get{modelInfo.ColumnPhysicalName}RangeInternal(int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, int count)",
                $"{__}=> MutableInstance.Get{modelInfo.ColumnPhysicalName}RangeInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, count);",
                $"",
                $"/// <inheritdoc cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Get{modelInfo.CellPhysicalName}Internal\"/>",
                $"[Pure]",
                $"public {modelInfo.ElementType} Get{modelInfo.CellPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index)",
                $"{__}=> MutableInstance.Get{modelInfo.CellPhysicalName}Internal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"public {modelInfo.FixedLengthRowType} Set{modelInfo.RowPhysicalName}Internal(",
                $"{__}int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}{modelInfo.RowSettingsType} settings",
                $") => MutableInstance.Set{modelInfo.RowPhysicalName}Internal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> Set{modelInfo.RowPhysicalName}RangeInternal(",
                $"{__}int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<{modelInfo.RowSettingsType}> settings",
                $") => MutableInstance.Set{modelInfo.RowPhysicalName}RangeInternal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> Set{modelInfo.ColumnPhysicalName}Internal(",
                $"{__}int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<{modelInfo.ElementSettingsType}> settings",
                $") => MutableInstance.Set{modelInfo.ColumnPhysicalName}Internal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"public IEnumerable<IEnumerable<{modelInfo.ElementType}>> Set{modelInfo.ColumnPhysicalName}RangeInternal(",
                $"{__}int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index,",
                $"{__}IEnumerable<IEnumerable<{modelInfo.ElementSettingsType}>> settings",
                $") => MutableInstance.Set{modelInfo.ColumnPhysicalName}RangeInternal({modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Set{modelInfo.CellPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Set{modelInfo.CellPhysicalName}\" path=\"param\"/>",
                $"public {modelInfo.ElementType} Set{modelInfo.CellPhysicalName}Internal(int {modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, int {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ElementSettingsType} settings)",
                $"{__}=> MutableInstance.Set{modelInfo.CellPhysicalName}Internal({modelInfo.RowPhysicalName.ToLowerFirstChar()}Index, {modelInfo.ColumnPhysicalName.ToLowerFirstChar()}Index, settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}\" path=\"param\"/>",
                $"public void Move{modelInfo.RowPhysicalName}Internal(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index)",
                $"{__}=> MutableInstance.Move{modelInfo.RowPhysicalName}Internal(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.RowPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.RowPhysicalName}Range\" path=\"param\"/>",
                $"public void Move{modelInfo.RowPhysicalName}RangeInternal(int old{modelInfo.RowPhysicalName}Index, int new{modelInfo.RowPhysicalName}Index, int count)",
                $"{__}=> MutableInstance.Move{modelInfo.RowPhysicalName}RangeInternal(old{modelInfo.RowPhysicalName}Index, new{modelInfo.RowPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}\" path=\"param\"/>",
                $"public void Move{modelInfo.ColumnPhysicalName}Internal(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index)",
                $"{__}=> MutableInstance.Move{modelInfo.ColumnPhysicalName}Internal(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Move{modelInfo.ColumnPhysicalName}Range\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Move{modelInfo.ColumnPhysicalName}Range\" path=\"param\"/>",
                $"public void Move{modelInfo.ColumnPhysicalName}RangeInternal(int old{modelInfo.ColumnPhysicalName}Index, int new{modelInfo.ColumnPhysicalName}Index, int count)",
                $"{__}=> MutableInstance.Move{modelInfo.ColumnPhysicalName}RangeInternal(old{modelInfo.ColumnPhysicalName}Index, new{modelInfo.ColumnPhysicalName}Index, count);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"ResetStrict\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"ResetStrict\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> ResetStrictInternal(",
                $"{__}IEnumerable<{modelInfo.RowSettingsType}> settings",
                $") => MutableInstance.ResetStrictInternal(settings);",
                $"",
                $"/// <summary>",
                $"/// {__}<see cref=\"Reset()\"/> メソッド処理中核。",
                $"/// </summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.FixedLengthRowType}> ResetInternal() => MutableInstance.ResetInternal();"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassItemEqualsSource_Delegating(
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
                $"public bool ItemEquals({modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => MutableInstance.ItemEquals(other);"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassDeepCloneSource_Delegating(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword} DeepClone() => new(MutableInstance.DeepClone());",
                $"object IDeepCloneable.DeepClone() => DeepClone();"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassSettingsInterfaceImplementsSource_Delegating(
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

        private static SourceFormatTargetBlock BuildFixedLengthClassPrivateMethodSource_Delegating(
            ModelInformation _
        )
        {
            return SourceTextFormatter.Format(
                __
                // 何も出力しない
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassImplicitTypeConversionOperatorSource_Delegating(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <summary>",
                $"/// {__}読取専用クラスへの暗黙的型変換",
                $"/// </summary>",
                $"/// <param name=\"src\">変換元</param>",
                $"/// <returns>変換したインスタンス</returns>",
                $"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]",
                $"public static implicit operator {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}?({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? src)",
                $"{{",
                $"{__}return src?.MutableInstance;",
                $"}}"
            );
        }
    }
}
