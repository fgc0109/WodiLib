// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildFixedLengthClassSource_Delegating.cs
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
                $"{__}IEnumerable<{modelInfo.ElementType}>,",
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
                // SettingsInterface Implements
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
                $"/// <summary>インデクサによるアクセス</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定したインデックスの要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><see langword=\"null\"/> をセットしようとした場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"public {modelInfo.ElementType} this[int index]",
                $"{{",
                $"{__}[Pure]",
                $"{__}get => MutableInstance[index];",
                $"{__}set => MutableInstance[index] = value;",
                $"}}",
                $"",
                $"/// <summary>要素数</summary>",
                $"[Pure]",
                $"public int Count => MutableInstance.Count;",
                $"",
                modelInfo.Members.FixedLengthListProperties.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"[Pure]",
                $"public IList<{modelInfo.ElementSettingsType}> Settings => MutableInstance.Settings;",
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
                $"public IEnumerator<{modelInfo.ElementType}> GetEnumerator() => MutableInstance.GetEnumerator();",
                $"IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();",
                $"",
                $"/// <summary>指定インデックスの要素を取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"[Pure]",
                $"public {modelInfo.ElementType} Get(int index) => MutableInstance.Get(index);",
                $"",
                $"/// <summary>指定範囲の要素を簡易コピーしたリストを取得する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"Count\"/> - 1)] インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"Count\"/>)] 要素数</param>",
                $"/// <returns>指定範囲の要素簡易コピーリスト</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>, <paramref name=\"count\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を取得しようとした場合。</exception>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> GetRange(int index, int count) => MutableInstance.GetRange(index, count);",
                $"",
                $"/// <summary>リストの要素を更新する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 更新開始インデックス</param>",
                $"/// <param name=\"settings\">更新要素</param>",
                $"/// <returns>セットした要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を編集しようとした場合。</exception>",
                $"public {modelInfo.ElementType} Set(int index, {modelInfo.ElementSettingsType} settings) => MutableInstance.Set(index, settings);",
                $"",
                $"/// <summary>リストの連続した要素を更新する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 更新開始インデックス</param>",
                $"/// <param name=\"settings\">更新要素</param>",
                $"/// <returns>セットした要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を編集しようとした場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> SetRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => MutableInstance.SetRange(index, settings);",
                $"",
                $"/// <summary>指定したインデックスにある項目をコレクション内の新しい場所へ移動する。</summary>",
                $"/// <param name=\"oldIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 移動する項目のインデックス</param>",
                $"/// <param name=\"newIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] 移動先のインデックス</param>",
                $"/// <exception cref=\"InvalidOperationException\">自身の要素数が0の場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"oldIndex\"/>, <paramref name=\"newIndex\"/> が指定範囲外の場合。</exception>",
                $"public void Move(int oldIndex, int newIndex) => MutableInstance.Move(oldIndex, newIndex);",
                $"",
                $"/// <summary>指定したインデックスから始まる連続した項目をコレクション内の新しい場所へ移動する。</summary>",
                $"/// <param name=\"oldIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)]移動する項目のインデックス開始位置</param>",
                $"/// <param name=\"newIndex\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)]移動先のインデックス開始位置</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)]移動させる要素数</param>",
                $"/// <exception cref=\"InvalidOperationException\">自身の要素数が0の場合。</exception>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"oldIndex\"/>, <paramref name=\"newIndex\"/>, <paramref name=\"count\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を移動しようとした場合。</exception>",
                $"public void MoveRange(int oldIndex, int newIndex, int count) => MutableInstance.MoveRange(oldIndex, newIndex, count);",
                $"",
                $"/// <summary>要素を与えられた内容で一新する。</summary>",
                $"/// <param name=\"settings\">リストに詰め直す要素</param>",
                $"/// <returns>新たにリストに詰め直した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentException\"><paramref name=\"settings\"/> の要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> と異なる場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetStrict(IEnumerable<{modelInfo.ElementSettingsType}> settings) => MutableInstance.ResetStrict(settings);",
                $"",
                $"/// <summary>要素をデフォルト値で一新する。</summary>",
                $"public IEnumerable<{modelInfo.ElementType}> Reset() => MutableInstance.Reset();",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param|exception\"/>",
                $"public void ValidateGet(int index) => MutableInstance.ValidateGet(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param|exception\"/>",
                $"public void ValidateGetRange(int index, int count) => MutableInstance.ValidateGetRange(index, count);",
                $"",
                $"/// <summary><see cref=\"Set\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Set\" path=\"param|exception\"/>",
                $"public void ValidateSet(int index, {modelInfo.ElementSettingsType} settings) => MutableInstance.ValidateSet(index, settings);",
                $"",
                $"/// <summary><see cref=\"SetRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"SetRange\" path=\"param|exception\"/>",
                $"public void ValidateSetRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => MutableInstance.ValidateSetRange(index, settings);",
                $"",
                $"/// <summary><see cref=\"Move\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Move\" path=\"param|exception\"/>",
                $"public void ValidateMove(int oldIndex, int newIndex) => MutableInstance.ValidateMove(oldIndex, newIndex);",
                $"",
                $"/// <summary><see cref=\"MoveRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"MoveRange\" path=\"param|exception\"/>",
                $"public void ValidateMoveRange(int oldIndex, int newIndex, int count) => MutableInstance.ValidateMoveRange(oldIndex, newIndex, count);",
                $"",
                $"/// <summary><see cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|exception\"/>",
                $"public void ValidateResetStrict(IEnumerable<{modelInfo.ElementSettingsType}> settings) => MutableInstance.ValidateResetStrict(settings);",
                $"",
                $"/// <summary><see cref=\"Reset()\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|exception\"/>",
                $"public void ValidateReset() => MutableInstance.ValidateReset();",
                $"",
                $"/// <summary><see cref=\"Get\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Get\" path=\"param\"/>",
                $"[Pure]",
                $"public {modelInfo.ElementType} GetInternal(int index) => MutableInstance.GetInternal(index);",
                $"",
                $"/// <summary><see cref=\"GetRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"GetRange\" path=\"param\"/>",
                $"[Pure]",
                $"public IEnumerable<{modelInfo.ElementType}> GetRangeInternal(int index, int count) => MutableInstance.GetRangeInternal(index, count);",
                $"",
                $"/// <summary><see cref=\"Set\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Set\" path=\"param\"/>",
                $"public {modelInfo.ElementType} SetInternal(int index, {modelInfo.ElementSettingsType} settings) => MutableInstance.SetInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"SetRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"SetRange\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> SetRangeInternal(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => MutableInstance.SetRangeInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"Move\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Move\" path=\"param\"/>",
                $"public void MoveInternal(int oldIndex, int newIndex) => MutableInstance.MoveInternal(oldIndex, newIndex);",
                $"",
                $"/// <summary><see cref=\"MoveRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"MoveRange\" path=\"param\"/>",
                $"public void MoveRangeInternal(int oldIndex, int newIndex, int count) => MutableInstance.MoveRangeInternal(oldIndex, newIndex, count);",
                $"",
                $"/// <summary><see cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetStrictInternal(IEnumerable<{modelInfo.ElementSettingsType}> settings) => MutableInstance.ResetInternal(settings);",
                $"",
                $"/// <summary><see cref=\"Reset()\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetInternal() => MutableInstance.ResetInternal();"
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
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
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
                $"    return src?.MutableInstance;",
                $"}}"
            );
        }
    }
}
