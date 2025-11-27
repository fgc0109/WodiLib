// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildRestrictedClassSource.cs
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
        private static SourceFormatTargetBlock BuildRestrictedClassSource(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                // class start
                $"/// <summary>",
                $"/// {__}{modelInfo.Description}",
                $"/// </summary>",
                $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassName} : ModelBase,",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword},",
                $"{__}IEnumerable<{modelInfo.ElementType}>,",
                $"{__}INotifyCollectionChanged,",
                $"{__}IEqualityComparable<{modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}>,",
                $"{__}IEqualityComparable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>,",
                $"{__}IEqualityComparable<{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}>,",
                $"{__}IDeepCloneable<{modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}>",
                $"{{",
                // Constants
                BuildRestrictedClassConstantsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Events
                BuildRestrictedClassEventSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Properties
                BuildRestrictedClassPropertiesSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildRestrictedClassConstructorsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildRestrictedClassMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // ItemEquals
                BuildRestrictedClassItemEqualsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildRestrictedClassDeepCloneSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // SettingsInterface Implements
                BuildRestrictedClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Private Methods
                BuildRestrictedClassPrivateMethodSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Implicit Type Conversion Operator
                BuildImplicitTypeConversionOperatorSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedClassConstantsSource(
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

        private static SourceFormatTargetBlock BuildRestrictedClassEventSource(
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

        private static SourceFormatTargetBlock BuildRestrictedClassPropertiesSource(
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
                $"public int Count => Items.Count;",
                $"",
                $"/// <inheritdoc/>",
                $"[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]",
                $"public IList<{modelInfo.ElementSettingsType}> Settings => Items.Cast<{modelInfo.ElementSettingsType}>().ToList();",
                $"",
                $"private protected ExtendedList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}> Items {{ get; }}"
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedClassConstructorsSource(
            ModelInformation modelInfo
        )
        {
            var useConstructorExpansion = modelInfo is { IsExtendClass: false, UseConstructorExpansion: true };

            return SourceTextFormatter.Format(
                __,
                $"private {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}(",
                $"{__}{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings,",
                $"{__}SimpleList<{modelInfo.ElementType}> itemsImpl,",
                $"{__}Func<int, {modelInfo.ElementSettingsType}, {modelInfo.ElementType}> itemBuilder",
                $")",
                $"{{",
                $"{__}var validator = BuildValidator(settings, itemsImpl);",
                $"{__}validator?.Constructor((nameof(settings), settings));",
                $"{__}Items = new ExtendedList<{modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword}, {modelInfo.ElementType}, {modelInfo.ElementSettingsType}>(",
                $"{__}{__}itemsImpl,",
                $"{__}{__}minCapacity: MaxCapacity,",
                $"{__}{__}maxCapacity: MinCapacity,",
                $"{__}{__}validator,",
                $"{__}{__}buildItemFromSettings: (index, modelSettings) => itemBuilder(index, modelSettings)",
                $"{__});",
                $"{__}PropagatePropertyChangeEvent(Items);",
                $"{__}PropagateCollectionChangeEvent(Items);",
                SourceTextFormatter.If(
                    useConstructorExpansion,
                    new SourceFormatTarget[]
                    {
                        $"{__}DoConstructorExpansion(settings);",
                    }
                ),
                $"}}",
                SourceTextFormatter.If(
                    useConstructorExpansion,
                    new SourceFormatTarget[]
                    {
                        $"/// <summary>",
                        $"///     コンストラクタの最後で呼び出す処理",
                        $"/// </summary>",
                        $"/// <param name=\"settings\">設定DTO</param>",
                        $"protected virtual partial void DoConstructorExpansion({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings);",
                    }
                )
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedClassMethodsSource(
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
                $"/// <summary>リストの末尾に要素を追加する。</summary>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public {modelInfo.ElementType} Add({modelInfo.ElementSettingsType} settings) => Items.Add(settings);",
                $"",
                $"/// <summary>リストの末尾に要素を追加する。</summary>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AddRange(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.AddRange(settings);",
                $"",
                $"/// <summary>指定したインデックスの位置に要素を挿入する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] インデックス</param>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public {modelInfo.ElementType} Insert(int index, {modelInfo.ElementSettingsType} settings) => Items.Insert(index, settings);",
                $"",
                $"/// <summary>指定したインデックスの位置に要素を挿入する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] インデックス</param>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> InsertRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.InsertRange(index, settings);",
                $"",
                $"/// <summary>指定したインデックスを起点として、要素の上書き/追加を行う。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] インデックス</param>",
                $"/// <param name=\"settings\">上書き/追加リスト</param>",
                $"/// <returns>上書きした要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMaxCapacity\"/> を上回る場合。</exception>",
                $"/// <example><code>var target = new List&lt;int&gt; {{ 0, 1, 2, 3 }};var dst = new List&lt;int&gt; {{ 10, 11, 12 }};target.Overwrite(2, dst);// target is {{ 0, 1, 10, 11, 12 }}</code><code>var target = new List&lt;int&gt; {{ 0, 1, 2, 3 }};var dst = new List&lt;int&gt; {{ 10 }};target.Overwrite(2, dst);// target is {{ 0, 1, 10, 3 }}</code></example>",
                $"public IEnumerable<{modelInfo.ElementType}> Overwrite(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.Overwrite(index, settings);",
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
                $"/// <summary>指定したインデックスの要素を削除する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] インデックス</param>",
                $"/// <returns>削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMinCapacity\"/> を下回る場合。</exception>",
                $"public {modelInfo.ElementType} Remove(int index) => Items.Remove(index);",
                $"",
                $"/// <summary>要素の範囲を削除する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] 削除する要素数</param>",
                $"/// <returns>削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>, <paramref name=\"count\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を削除しようとした場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"GetMinCapacity\"/> を下回る場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> RemoveRange(int index, int count) => Items.RemoveRange(index, count);",
                $"",
                $"/// <summary>要素数を指定の数に合わせる。</summary>",
                $"/// <param name=\"length\">[Range(<see cref=\"GetMinCapacity\"/>, <see cref=\"GetMaxCapacity\"/>)]調整する要素数</param>",
                $"/// <returns>追加または削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"length\"/> が指定範囲外の場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLength(int length) => Items.AdjustLength(length);",
                $"",
                $"/// <summary>要素数が不足している場合、要素数を指定の数に合わせる。</summary>",
                $"/// <param name=\"length\">[Range(<see cref=\"GetMinCapacity\"/>, <see cref=\"GetMaxCapacity\"/>)]調整する要素数</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"length\"/> が指定範囲外の場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLengthIfShort(int length) => Items.AdjustLengthIfShort(length);",
                $"",
                $"/// <summary>要素数が超過している場合、要素数を指定の数に合わせる。</summary>",
                $"/// <param name=\"length\">[Range(<see cref=\"GetMinCapacity\"/>, <see cref=\"GetMaxCapacity\"/>)]調整する要素数</param>",
                $"/// <returns>削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"length\"/> が指定範囲外の場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLengthIfLong(int length) => Items.AdjustLengthIfLong(length);",
                $"",
                $"/// <summary>要素を与えられた内容で一新する。</summary>",
                $"/// <param name=\"settings\">リストに詰め直す要素</param>",
                $"/// <returns>新たにリストに詰め直した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentException\"><paramref name=\"settings\"/> の要素数が <see cref=\"GetMinCapacity\"/> 未満または <see cref=\"GetMaxCapacity\"/> を超える場合。</exception>",
                $"/// <remarks>このメソッドは <paramref name=\"settings\"/> の要素数が<see cref=\"GetMinCapacity\"/> 以上 <see cref=\"GetMaxCapacity\"/> 以下であれば成功する。<br/>現在の要素数と一致しない場合エラーとしたい場合は、容量固定型にキャストしてから同メソッドを呼び出す。</remarks>",
                $"public IEnumerable<{modelInfo.ElementType}> Reset(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.Reset(settings);",
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
                $"/// <summary>自身を初期化する。</summary>",
                $"public void Clear() => Items.Clear();",
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
                $"/// <summary><see cref=\"Add\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Add\" path=\"param|exception\"/>",
                $"public void ValidateAdd({modelInfo.ElementSettingsType} settings) => Items.ValidateAdd(settings);",
                $"",
                $"/// <summary><see cref=\"AddRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"AddRange\" path=\"param|exception\"/>",
                $"public void ValidateAddRange(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateAddRange(settings);",
                $"",
                $"/// <summary><see cref=\"Insert\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Insert\" path=\"param|exception\"/>",
                $"public void ValidateInsert(int index, {modelInfo.ElementSettingsType} settings) => Items.ValidateInsert(index, settings);",
                $"",
                $"/// <summary><see cref=\"InsertRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"InsertRange\" path=\"param|exception\"/>",
                $"public void ValidateInsertRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateInsertRange(index, settings);",
                $"",
                $"/// <summary><see cref=\"Overwrite\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Overwrite\" path=\"param|exception\"/>",
                $"public void ValidateOverwrite(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateOverwrite(index, settings);",
                $"",
                $"/// <summary><see cref=\"Move\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Move\" path=\"param|exception\"/>",
                $"public void ValidateMove(int oldIndex, int newIndex) => Items.ValidateMove(oldIndex, newIndex);",
                $"",
                $"/// <summary><see cref=\"MoveRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"MoveRange\" path=\"param|exception\"/>",
                $"public void ValidateMoveRange(int oldIndex, int newIndex, int count) => Items.ValidateMoveRange(oldIndex, newIndex, count);",
                $"",
                $"/// <summary><see cref=\"Remove\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Remove\" path=\"param|exception\"/>",
                $"public void ValidateRemove(int index) => Items.ValidateRemove(index);",
                $"",
                $"/// <summary><see cref=\"RemoveRange\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"RemoveRange\" path=\"param|exception\"/>",
                $"public void ValidateRemoveRange(int index, int count) => Items.ValidateRemoveRange(index, count);",
                $"",
                $"/// <summary><see cref=\"AdjustLength\"/>,<see cref=\"AdjustLengthIfShort\"/>,<see cref=\"AdjustLengthIfLong\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"AdjustLength\" path=\"param|exception\"/>",
                $"public void ValidateAdjustLength(int length) => Items.ValidateAdjustLength(length);",
                $"",
                $"/// <summary><see cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|exception\"/>",
                $"public void ValidateReset(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateReset(settings);",
                $"",
                $"/// <summary><see cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|exception\"/>",
                $"public void ValidateResetStrict(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateResetStrict(settings);",
                $"",
                $"/// <summary><see cref=\"Reset()\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|exception\"/>",
                $"public void ValidateReset() => Items.ValidateReset();",
                $"",
                $"/// <summary><see cref=\"Clear\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Clear\" path=\"param|exception\"/>",
                $"public void ValidateClear() => Items.ValidateClear();",
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
                $"/// <summary><see cref=\"Add\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Add\" path=\"param|returns\"/>",
                $"public {modelInfo.ElementType} AddInternal({modelInfo.ElementSettingsType} settings) => Items.AddInternal(settings);",
                $"",
                $"/// <summary><see cref=\"AddRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"AddRange\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> AddRangeInternal(",
                $"    IEnumerable<{modelInfo.ElementSettingsType}> settings",
                $") => Items.AddRangeInternal(settings);",
                $"",
                $"/// <summary><see cref=\"Insert\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Insert\" path=\"param|returns\"/>",
                $"public {modelInfo.ElementType} InsertInternal(int index, {modelInfo.ElementSettingsType} settings) => Items.InsertInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"InsertRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"InsertRange\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> InsertRangeInternal(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.InsertRangeInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"Overwrite\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Overwrite\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> OverwriteInternal(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.OverwriteInternal(index, settings);",
                $"",
                $"/// <summary><see cref=\"Move\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Move\" path=\"param\"/>",
                $"public void MoveInternal(int oldIndex, int newIndex) => Items.MoveInternal(oldIndex, newIndex);",
                $"",
                $"/// <summary><see cref=\"MoveRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"MoveRange\" path=\"param\"/>",
                $"public void MoveRangeInternal(int oldIndex, int newIndex, int count) => Items.MoveRangeInternal(oldIndex, newIndex, count);",
                $"",
                $"/// <summary><see cref=\"Remove\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Remove\" path=\"param|returns\"/>",
                $"public {modelInfo.ElementType} RemoveInternal(int index) => Items.RemoveInternal(index);",
                $"",
                $"/// <summary><see cref=\"RemoveRange\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"RemoveRange\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> RemoveRangeInternal(int index, int count) => Items.RemoveRangeInternal(index, count);",
                $"",
                $"/// <summary><see cref=\"AdjustLength\"/>,<see cref=\"AdjustLengthIfShort\"/>,<see cref=\"AdjustLengthIfLong\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"AdjustLength\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLengthInternal(int length) => Items.AdjustLengthInternal(length);",
                $"",
                $"/// <summary><see cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetInternal(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ResetInternal(settings);",
                $"",
                $"/// <summary><see cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"ResetStrict(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetStrictInternal(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ResetInternal(settings);",
                $"",
                $"/// <summary><see cref=\"Reset()\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Reset()\" path=\"param|returns\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetInternal() => Items.ResetInternal();",
                $"",
                $"/// <summary><see cref=\"Clear\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Clear\" path=\"param\"/>",
                $"public void ClearInternal() => Items.ClearInternal();"
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedClassItemEqualsSource(
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
                $"public bool ItemEquals({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});",
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public bool {objectItemEqualsKeyword}ItemEquals(object? other) => ItemEquals(other as {modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword});"
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedClassDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"[Pure]",
                $"public {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedClassSettingsInterfaceImplementsSource(
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

        private static SourceFormatTargetBlock BuildRestrictedClassPrivateMethodSource(
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

        private static SourceFormatTargetBlock BuildImplicitTypeConversionOperatorSource(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                __,
                $"private {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}? fixedLengthInstance = null;",
                $"private {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}? readonlyInstance = null;",
                $"",
                $"/// <summary>",
                $"/// {__}容量固定クラスへの暗黙的型変換",
                $"/// </summary>",
                $"/// <param name=\"src\">変換元</param>",
                $"/// <returns>変換したインスタンス</returns>",
                $"[return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(src))]",
                $"public static implicit operator {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}?({modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}? src)",
                $"{{",
                $"{__}if (src is null) return null;",
                $"{__}src.fixedLengthInstance ??= new {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}(src);",
                $"{__}return src.fixedLengthInstance;",
                $"}}",
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
