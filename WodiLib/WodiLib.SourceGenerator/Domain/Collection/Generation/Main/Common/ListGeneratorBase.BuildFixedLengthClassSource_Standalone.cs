// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildFixedLengthClassSource_Standalone.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Linq;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main.Common
{
    internal abstract partial class ListGeneratorBase
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
                $"{__}IEnumerable<{modelInfo.ElementType}>,",
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
            if (modelInfo.MaxCapacity == modelInfo.MinCapacity)
            {
                return SourceTextFormatter.Format(
                    __,
                    new[]
                    {
                        $"/// <summary>容量</summary>",
                        $"[Pure]",
                        $"public static int Capacity => {modelInfo.MaxCapacity};",
                    }
                );
            }

            return SourceTextFormatter.Format(
                __,
                new[]
                {
                    $"/// <summary>容量最大値</summary>",
                    $"[Pure]",
                    $"public static int MaxCapacity => {modelInfo.MaxCapacity};",
                    $"/// <summary>容量最小値</summary>",
                    $"[Pure]",
                    $"public static int MinCapacity => {modelInfo.MinCapacity};",
                }
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
                $"/// <summary>インデクサによるアクセス</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定したインデックスの要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><see langword=\"null\"/> をセットしようとした場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"public {modelInfo.ElementType} this[int index]",
                $"{{",
                $"{__}[Pure]",
                $"{__}get => Get(index);",
                $"{__}set => Set(index, value);",
                $"}}",
                $"",
                $"/// <summary>要素数</summary>",
                $"[Pure]",
                $"public int Count => Items.Count;",
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"[Pure]",
                $"public IList<{modelInfo.ElementSettingsType}> Settings => Items.Cast<{modelInfo.ElementSettingsType}>().ToList();",
                $"",
                $"private protected ExtendedList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}> Items {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassConstructorSource_Standalone(
            ModelInformation modelInfo
        )
        {
            var (maxCapacity, minCapacity) = modelInfo.MaxCapacity == modelInfo.MinCapacity
                ? ("Capacity", "Capacity")
                : ("MaxCapacity", "MinCapacity");

            // 容量固定リスト単体ではインスタンス作成不可
            return SourceTextFormatter.Format(
                __,
                $"private {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}(",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings,",
                $"{__}SimpleList<{modelInfo.ElementType}> itemsImpl,",
                $"{__}Func<int, {modelInfo.ElementSettingsType}, {modelInfo.ElementType}> itemBuilder",
                $")",
                $"{{",
                $"    var validator = BuildValidator(settings, itemsImpl);",
                $"    validator?.Constructor((nameof(settings), settings));",
                $"    Items = new ExtendedList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}>(",
                $"        itemsImpl,",
                $"        minCapacity: {minCapacity},",
                $"        maxCapacity: {maxCapacity},",
                $"        validator,",
                $"        buildItemFromSettings: (index, modelSettings) => itemBuilder(index, modelSettings)",
                $"    );",
                $"    PropagatePropertyChangeEvent(Items);",
                $"    PropagateCollectionChangeEvent(Items);",
                $"}}"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassMethodsSource_Standalone(
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
                    $"[Pure]",
                    $"public int GetCapacity() => Capacity;"
                ),
                SourceTextFormatter.If(
                    modelInfo.MaxCapacity != modelInfo.MinCapacity,
                    "",
                    $"/// <summary>容量最大値を取得する。</summary>",
                    $"/// <returns>容量最大値</returns>",
                    $"[Pure]",
                    $"public int GetMaxCapacity() => MaxCapacity;",
                    $"/// <summary>容量最小値を取得する。</summary>",
                    $"/// <returns>容量最小値</returns>",
                    $"[Pure]",
                    $"public int GetMinCapacity() => MinCapacity;"
                ),
                $"",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public IEnumerator<{modelInfo.ElementType}> GetEnumerator() => Items.GetEnumerator();",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
                $"",
                $"/// <summary>指定インデックスの要素を取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"[Pure]",
                $"public {modelInfo.ElementType} Get(int index) => Items.Get(index);",
                $"",
                $"/// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"Count\"/>)] 要素数</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を取得しようとした場合。</exception>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> GetRange(int index, int count) => Items.GetRange(index, count);",
                $"",
                $"/// <summary>リストの要素を更新する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 更新開始インデックス</param>",
                $"/// <param name=\"settings\">更新要素</param>",
                $"/// <returns>セットした要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を編集しようとした場合。</exception>",
                $"public {modelInfo.ElementType} Set(int index, {modelInfo.ElementSettingsType} settings) => Items.Set(index, settings);",
                $"",
                $"/// <summary>リストの連続した要素を更新する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 更新開始インデックス</param>",
                $"/// <param name=\"settings\">更新要素</param>",
                $"/// <returns>セットした要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を編集しようとした場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> SetRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.SetRange(index, settings);",
                $"",
                $"/// <summary>指定したインデックスにある項目をコレクション内の新しい場所へ移動する。</summary>",
                $"/// <param name=\"oldIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 移動する項目のインデックス</param>",
                $"/// <param name=\"newIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 移動先のインデックス</param>",
                $"/// <exception cref=\"InvalidOperationException\">自身の要素数が0の場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"oldIndex\"/>, <paramref name=\"newIndex\"/> が指定範囲外の場合。</exception>",
                $"public void Move(int oldIndex, int newIndex) => Items.Move(oldIndex, newIndex);",
                $"",
                $"/// <summary>指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。</summary>",
                $"/// <param name=\"oldIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)]移動する項目のインデックス開始位置</param>",
                $"/// <param name=\"newIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)]移動先のインデックス開始位置</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)]移動させる要素数</param>",
                $"/// <exception cref=\"InvalidOperationException\">自身の要素数が0の場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"oldIndex\"/>, <paramref name=\"newIndex\"/>, <paramref name=\"count\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を移動しようとした場合。</exception>",
                $"public void MoveRange(int oldIndex, int newIndex, int count) => Items.MoveRange(oldIndex, newIndex, count);",
                $"",
                $"/// <summary>要素を与えられた内容で一新する。</summary>",
                $"/// <param name=\"settings\">リストに詰め直す要素</param>",
                $"/// <returns>新たにリストに詰め直した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentException\"><paramref name=\"settings\"/> の要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> と異なる場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetStrict(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ResetStrict(settings);",
                $"",
                $"/// <summary>要素をデフォルト値で一新する。</summary>",
                $"public IEnumerable<{modelInfo.ElementType}> Reset() => Items.Reset();",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param|exception\"/>",
                $"public void ValidateGet(int index) => Items.ValidateGet(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param|exception\"/>",
                $"public void ValidateGetRange(int index, int count) => Items.ValidateGetRange(index, count);",
                $"",
                $"/// <summary><see cref=\"Set\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Set\" path=\"param|exception\"/>",
                $"public void ValidateSet(int index, {modelInfo.ElementSettingsType} settings) => Items.ValidateSet(index, settings);",
                $"",
                $"/// <summary><see cref=\"SetRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"SetRange\" path=\"param|exception\"/>",
                $"public void ValidateSetRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateSetRange(index, settings);",
                $"",
                $"/// <summary><see cref=\"Move\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Move\" path=\"param|exception\"/>",
                $"public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);",
                $"",
                $"/// <summary><see cref=\"MoveRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"MoveRange\" path=\"param|exception\"/>",
                $"public void ValidateMoveRange(int oldIndex, int newIndex, int count) => Items.ValidateMoveRange(oldIndex, newIndex, count);",
                $"",
                $"/// <summary><see cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|exception\"/>",
                $"public void ValidateResetStrict(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateResetStrict(settings);",
                $"",
                $"/// <summary><see cref=\"Reset()\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|exception\"/>",
                $"public void ValidateReset() => Items.ValidateReset();",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.ElementType} GetInternal(int index) => Items.GetInternal(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);",
                $"",
                $"/// <summary><see cref=\"Set\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Set\" path=\"param\"/>",
                $"public {modelInfo.ElementType} SetInternal(int index, {modelInfo.ElementSettingsType} settings) => Items.SetInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"SetRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"SetRange\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> SetRangeInternal(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.SetRangeInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"Move\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Move\" path=\"param\"/>",
                $"public void MoveInternal(int oldIndex, int newIndex) => Items.MoveInternal(oldIndex, newIndex);",
                $"",
                $"/// <summary><see cref=\"MoveRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"MoveRange\" path=\"param\"/>",
                $"public void MoveRangeInternal(int oldIndex, int newIndex, int count) => Items.MoveRangeInternal(oldIndex, newIndex, count);",
                $"",
                $"/// <summary><see cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetStrictInternal(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ResetInternal(settings);",
                $"",
                $"/// <summary><see cref=\"Reset()\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetInternal() => Items.ResetInternal();"
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
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
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
                $"///     <see cref=\"ExtendedList{{TListSettings, TEditableElement, TElementSettings}}\"/> が通知した",
                $"///     <see cref=\"INotifyCollectionChanged\"/> イベントを",
                $"///     自身のイベントとして通知する。",
                $"/// </summary>",
                $"/// <param name=\"target\">対象</param>",
                $"private void PropagateCollectionChangeEvent(ExtendedList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}> target)",
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
