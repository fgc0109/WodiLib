// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : ListGeneratorBase.BuildFixedLengthClassSource.cs
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
        private static SourceFormatTargetBlock BuildFixedLengthClassSource(ModelInformation modelInfo)
        {
            return SourceTextFormatter.Format(
                "",
                // -----
                // class start
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{modelInfo.Description}",
                    $"/// </summary>",
                    $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.FixedLengthListInfo.FixedLengthListClassName} : {modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword},",
                    $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}>",
                    $"{{",
                },
                // Properties
                BuildFixedLengthListPropertiesSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                BuildFixedLengthClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildFixedLengthListConstructorSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildFixedLengthListMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildFixedLengthListDeepCloneSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthListPropertiesSource(
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
                $"public new {modelInfo.ElementType} this[int index]",
                $"{{",
                $"    get => Get(index);",
                $"    set => Set(index, value);",
                $"}}",
                $"",
                $"/// <summary>すべての編集可能型要素</summary>",
                $"public {modelInfo.ElementType}[] EditableItems => Items.ToArray<{modelInfo.ElementType}>();"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthClassSettingsInterfaceImplementsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.FixedLengthListProperties.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthListConstructorSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                $"public {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings) : base(settings) {{ }}",
                $"",
                $"private protected {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword}({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings, SimpleList<{modelInfo.ElementType}> itemsImpl) : base(settings, itemsImpl) {{ }}",
                $"",
                modelInfo.Members.FixedLengthListConstructors.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthListMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.FixedLengthListMethods.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <inheritdoc cref=\"IReadOnlyExtendedList{{TElement}}.Get\"/>",
                $"public new {modelInfo.ElementType} Get(int index) => Items.Get(index);",
                $"",
                $"/// <inheritdoc cref=\"IReadOnlyExtendedList{{TElement}}.GetRange\"/>",
                $"public new IEnumerable<{modelInfo.ElementType}> GetRange(int index, int count) => Items.GetRange(index, count);",
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
                $"public IEnumerable<{modelInfo.ElementType}> Reset(IEnumerable<{modelInfo.ElementSettingsType}> settings) => (FixedLengthList<{modelInfo.ElementType}, {modelInfo.ReadOnlyElementType}, {modelInfo.ElementSettingsType}>)Items.Reset(settings);",
                $"",
                $"/// <summary>要素をデフォルト値で一新する。</summary>",
                $"public IEnumerable<{modelInfo.ElementType}> Reset() => Items.Reset();",
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
                $"/// <summary><see cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param|exception\"/>",
                $"public void ValidateReset(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateReset(settings);",
                $"",
                $"/// <inheritdoc cref=\"IReadOnlyExtendedList{{TReadOnlyElement}}.GetInternal\"/>",
                $"public new {modelInfo.ElementType} GetInternal(int index) => Items.GetInternal(index);",
                $"",
                $"/// <inheritdoc cref=\"IReadOnlyExtendedList{{TReadOnlyElement}}.GetRangeInternal\"/>",
                $"public new IEnumerable<{modelInfo.ElementType}> GetRangeInternal(int index, int count) => Items.GetRangeInternal(index, count);",
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
                $"/// <summary><see cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\"/>,<see cref=\"Reset()\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Reset(System.Collections.Generic.IEnumerable{{{modelInfo.ElementSettingsType}}})\" path=\"param\"/>",
                $"public IEnumerable<{modelInfo.ElementType}> ResetInternal(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ResetInternal(settings);"
            );
        }

        private static SourceFormatTargetBlock BuildFixedLengthListDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public new {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
            );
        }
    }
}
