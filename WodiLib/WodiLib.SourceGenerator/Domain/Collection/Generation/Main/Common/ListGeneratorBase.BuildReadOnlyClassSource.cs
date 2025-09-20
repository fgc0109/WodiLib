// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildReadOnlyClassSource.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

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
                $"{__}IReadOnlyList<{modelInfo.ReadOnlyElementType}>,",
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
            if (modelInfo.MaxCapacity == modelInfo.MinCapacity)
            {
                return SourceTextFormatter.Format(
                    __,
                    new[]
                    {
                        $"/// <summary>容量</summary>",
                        $"public static int Capacity => {modelInfo.MaxCapacity};",
                    }
                );
            }

            return SourceTextFormatter.Format(
                __,
                new[]
                {
                    $"/// <summary>容量最大値</summary>",
                    $"public static int MaxCapacity => {modelInfo.MaxCapacity};",
                    $"/// <summary>容量最小値</summary>",
                    $"public static int MinCapacity => {modelInfo.MinCapacity};",
                }
            );
        }

        private static SourceFormatTargetBlock BuildEventSource(
            ModelInformation modelInfo
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
                $"private protected {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}(SimpleList<{modelInfo.ElementType}> itemsImpl)",
                $"{{",
                $"    Items = new ExtendedList<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}>(",
                $"        itemsImpl,",
                $"        minCapacity: {modelInfo.MaxCapacity},",
                $"        maxCapacity: {modelInfo.MinCapacity},",
                $"        validator: BuildValidator(itemsImpl),",
                $"        buildItemFromSettings: BuildItemFromSettings",
                $"    );",
                $"    PropagatePropertyChangeEvent(Items);",
                $"    PropagateCollectionChangeEvent(Items);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyListPropertiesSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <summary>インデクサによるアクセス</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定したインデックスの要素</returns>",
                $"/// <exception cref=\"System.ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"public {modelInfo.ReadOnlyElementType} this[int index] => Get(index);",
                $"",
                $"/// <summary>要素数</summary>",
                $"public int Count => Items.Count;",
                $"",
                $"/// <inheritdoc/>",
                $"public IReadOnlyList<{modelInfo.ElementSettingsType}> Settings => Items.Cast<{modelInfo.ElementSettingsType}>().ToList();",
                $"",
                $"private protected ExtendedList<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}> Items {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildReadOnlyListMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                SourceTextFormatter.If(
                    modelInfo.MaxCapacity == modelInfo.MinCapacity,
                    "",
                    $"/// <summary>容量を取得する。</summary>",
                    $"/// <returns>容量最大値</returns>",
                    $"public int GetCapacity() => Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxCapacity != modelInfo.MinCapacity,
                    "",
                    $"/// <summary>容量最大値を取得する。</summary>",
                    $"/// <returns>容量最大値</returns>",
                    $"public int GetMaxCapacity() => MaxCapacity;",
                    $"/// <summary>容量最小値を取得する。</summary>",
                    $"/// <returns>容量最小値</returns>",
                    $"public int GetMinCapacity() => MinCapacity;"
                ),
                $"/// <inheritdoc/>",
                $"public IEnumerator<{modelInfo.ReadOnlyElementType}> GetEnumerator() => Items.GetEnumerator();",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
                $"",
                $"/// <summary>指定インデックスの要素を取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"public {modelInfo.ReadOnlyElementType} Get(int index) => Items.Get(index);",
                $"",
                $"/// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"Count\"/>)] 要素数</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を取得しようとした場合。</exception>",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> GetRange(int index, int count) => Items.GetRange(index, count);",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param|exception\"/>",
                $"public void ValidateGet(int index) => Items.ValidateGet(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param|exception\"/>",
                $"public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param\"/>",
                $"public {modelInfo.ReadOnlyElementType} GetInternal(int index) => Items.GetInternal(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ReadOnlyElementType}> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);"
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
                $"public bool ItemEquals({modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"public bool ItemEquals({modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                SourceTextFormatter.If(
                    !modelInfo.IsFixed,
                    "",
                    $"/// <inheritdoc/>",
                    $"public bool ItemEquals({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
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
                $"///     <see cref=\"ExtendedList{{TEditableElement, TReadOnlyElement, TElementSettings}}\"/> が通知した",
                $"///     <see cref=\"INotifyCollectionChanged\"/> イベントを",
                $"///     自身のイベントとして通知する。",
                $"/// </summary>",
                $"/// <param name=\"target\">対象</param>",
                $"private void PropagateCollectionChangeEvent(ExtendedList<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}> target)",
                $"{{",
                $"    target.CollectionChanged += (_, args) => {{ collectionChanged?.Invoke(this, args); }};",
                $"}}"
            );
        }
    }
}
