// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildReadOnlyClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    /// <summary>
    ///     テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal abstract partial class ListGeneratorBase
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
                $"{__}IEnumerable<{modelInfo.ReadOnlyElementType}>,",
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
                // SettingsInterface Implements
                BuildReadOnlyClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Private Methods
                BuildReadOnlyClassPrivateMethodSource(modelInfo),
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
                $"/// <summary>インデクサによるアクセス</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定したインデックスの要素</returns>",
                $"/// <exception cref=\"System.ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"[Pure]",
                $"public {modelInfo.ReadOnlyElementType} this[int index] => MutableInstance[index];",
                $"",
                $"/// <summary>要素数</summary>",
                $"[Pure]",
                $"public int Count => MutableInstance.Count;",
                $"",
                modelInfo.Members.ReadOnlyListProperties.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"[Pure]",
                $"public IList<{modelInfo.ElementSettingsType}> Settings => MutableInstance.Settings;",
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
                $"public IEnumerator<{modelInfo.ReadOnlyElementType}> GetEnumerator() => MutableInstance.Select(item => item.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>()).GetEnumerator();",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
                $"",
                $"/// <summary>指定インデックスの要素を取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"[Pure]",
                $"public {modelInfo.ReadOnlyElementType} Get(int index) => MutableInstance.Get(index);",
                $"",
                $"/// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"Count\"/>)] 要素数</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を取得しようとした場合。</exception>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> GetRange(int index, int count) => MutableInstance.GetRange(index, count).Select(item => item.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>());",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param|exception\"/>",
                $"public void ValidateGet(int index) => MutableInstance.ValidateGet(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param|exception\"/>",
                $"public void ValidateGetRange(int index, int count) => MutableInstance.ValidateGetRange(index, count);",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.ReadOnlyElementType} GetInternal(int index) => MutableInstance.GetInternal(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> GetRangeInternal(int index, int count) => MutableInstance.GetRangeInternal(index, count).Select(item => item.Cast<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}>());",
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
                $"public bool ItemEquals({modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                SourceTextFormatter.If(
                    !modelInfo.IsFixed,
                    "",
                    $"/// <inheritdoc/>",
                    $"[Pure]",
                    $"public bool ItemEquals({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? other) => MutableInstance.ItemEquals(other);",
                    $""
                ),
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool ItemEquals({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}? other) => MutableInstance.ItemEquals(other);",
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
