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
                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{modelInfo.Description}",
                    $"/// </summary>",
                    $"{modelInfo.Accessibility} {modelInfo.AbstractKeyword}partial class {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassName} : {modelInfo.FixedLengthListInfo.FixedLengthListClassNameWithoutInOutKeyword},",
                    $"{__}WodiLib.Sys.IDeepCloneable<{modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}>",
                    $"{{",
                },
                // Properties
                BuildRestrictedCapacityListClassSettingsInterfaceImplementsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Constructors
                BuildRestrictedCapacityListConstructorSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // Methods
                BuildRestrictedCapacityListMethodsSource(modelInfo),
                SourceFormatTargetBlock.Empty,
                // DeepClone
                BuildRestrictedCapacityListDeepCloneSource(modelInfo),
                // class end
                new[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedCapacityListClassSettingsInterfaceImplementsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.RestrictedListProperties.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedCapacityListConstructorSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                $"public {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}({modelInfo.SettingsInterfaceInfo.SettingsInterfaceNameWithoutIOKeyword} settings) : base(settings) {{ }}",
                $"",
                $"private protected {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword}(SimpleList<{modelInfo.ElementType}> itemsImpl) : base(itemsImpl) {{ }}",
                $"",
                modelInfo.Members.RestrictedListConstructors.SelectMany(p => p.ImplementationCode).ToArray()
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedCapacityListMethodsSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                $"{__}",
                modelInfo.Members.RestrictedListMethods.SelectMany(p => p.ImplementationCode).ToArray(),
                $"",
                $"/// <summary>リストの末尾に要素を追加する。</summary>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public {modelInfo.ElementType} Add({modelInfo.ElementSettingsType} settings) => Items.Add(settings);",
                $"",
                $"/// <summary>リストの末尾に要素を追加する。</summary>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AddRange(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.AddRange(settings);",
                $"",
                $"/// <summary>指定したインデックスの位置に要素を挿入する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] インデックス</param>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public {modelInfo.ElementType} Insert(int index, {modelInfo.ElementSettingsType} settings) => Items.Insert(index, settings);",
                $"",
                $"/// <summary>指定したインデックスの位置に要素を挿入する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] インデックス</param>",
                $"/// <param name=\"settings\">追加する要素</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> を上回る場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> InsertRange(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.InsertRange(index, settings);",
                $"",
                $"/// <summary>指定したインデックスを起点として、要素の上書き/追加を行う。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] インデックス</param>",
                $"/// <param name=\"settings\">上書き/追加リスト</param>",
                $"/// <returns>上書きした要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> を上回る場合。</exception>",
                $"/// <example><code>var target = new List&lt;int&gt; {{ 0, 1, 2, 3 }};var dst = new List&lt;int&gt; {{ 10, 11, 12 }};target.Overwrite(2, dst);// target is {{ 0, 1, 10, 11, 12 }}</code><code>var target = new List&lt;int&gt; {{ 0, 1, 2, 3 }};var dst = new List&lt;int&gt; {{ 10 }};target.Overwrite(2, dst);// target is {{ 0, 1, 10, 3 }}</code></example>",
                $"public IEnumerable<{modelInfo.ElementType}> Overwrite(int index, IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.Overwrite(index, settings);",
                $"",
                $"/// <summary>指定したインデックスの要素を削除する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] インデックス</param>",
                $"/// <returns>削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/> を下回る場合。</exception>",
                $"public {modelInfo.ElementType} Remove(int index) => Items.Remove(index);",
                $"",
                $"/// <summary>要素の範囲を削除する。</summary>",
                $"/// <param name=\"index\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/> - 1)] インデックス</param>",
                $"/// <param name=\"count\">[Range(0, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.Count\"/>)] 削除する要素数</param>",
                $"/// <returns>削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"index\"/>, <paramref name=\"count\"/> が指定範囲外の場合。</exception>",
                $"/// <exception cref=\"ArgumentException\">有効な範囲外の要素を削除しようとした場合。</exception>",
                $"/// <exception cref=\"InvalidOperationException\">操作によって要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/> を下回る場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> RemoveRange(int index, int count) => Items.RemoveRange(index, count);",
                $"",
                $"/// <summary>要素数を指定の数に合わせる。</summary>",
                $"/// <param name=\"length\">[Range(<see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/>, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/>)]調整する要素数</param>",
                $"/// <returns>追加または削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"length\"/> が指定範囲外の場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLength(int length) => Items.AdjustLength(length);",
                $"",
                $"/// <summary>要素数が不足している場合、要素数を指定の数に合わせる。</summary>",
                $"/// <param name=\"length\">[Range(<see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/>, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/>)]調整する要素数</param>",
                $"/// <returns>追加した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"length\"/> が指定範囲外の場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLengthIfShort(int length) => Items.AdjustLengthIfShort(length);",
                $"",
                $"/// <summary>要素数が超過している場合、要素数を指定の数に合わせる。</summary>",
                $"/// <param name=\"length\">[Range(<see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/>, <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/>)]調整する要素数</param>",
                $"/// <returns>削除した要素</returns>",
                $"/// <exception cref=\"ArgumentOutOfRangeException\"><paramref name=\"length\"/> が指定範囲外の場合。</exception>",
                $"public IEnumerable<{modelInfo.ElementType}> AdjustLengthIfLong(int length) => Items.AdjustLengthIfLong(length);",
                $"",
                $"/// <summary>要素を与えられた内容で一新する。</summary>",
                $"/// <param name=\"settings\">リストに詰め直す要素</param>",
                $"/// <returns>新たにリストに詰め直した要素</returns>",
                $"/// <exception cref=\"ArgumentNullException\"><paramref name=\"settings\"/> が <see langword=\"null\"/> の場合、または <paramref name=\"settings\"/> に <see langword=\"null\"/> 要素が含まれる場合。</exception>",
                $"/// <exception cref=\"ArgumentException\"><paramref name=\"settings\"/> の要素数が <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/> 未満または <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> を超える場合。</exception>",
                $"/// <remarks>このメソッドは <paramref name=\"settings\"/> の要素数が<see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMinCapacity\"/> 以上 <see cref=\"{modelInfo.ReadOnlyListInfo.ReadOnlyListClassNameWithoutInOutKeyword}.GetMaxCapacity\"/> 以下であれば成功する。<br/>現在の要素数と一致しない場合エラーとしたい場合は、容量固定型にキャストしてから同メソッドを呼び出す。</remarks>",
                $"public new IEnumerable<{modelInfo.ElementType}> Reset(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.Reset(settings);",
                $"",
                $"/// <summary>自身を初期化する。</summary>",
                $"public void Clear() => Items.Clear();",
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
                $"public new void ValidateReset(IEnumerable<{modelInfo.ElementSettingsType}> settings) => Items.ValidateReset(settings);",
                $"",
                $"/// <summary><see cref=\"Clear\"/> メソッドの検証処理。</summary>",
                $"/// <inheritdoc cref=\"Clear\" path=\"param|exception\"/>",
                $"public void ValidateClear() => Items.ValidateClear();",
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
                $"/// <summary><see cref=\"Clear\"/> メソッド処理中核。</summary>",
                $"/// <inheritdoc cref=\"Clear\" path=\"param\"/>",
                $"public void ClearInternal() => Items.ClearInternal();"
            );
        }

        private static SourceFormatTargetBlock BuildRestrictedCapacityListDeepCloneSource(
            ModelInformation modelInfo
        )
        {
            return SourceTextFormatter.Format(
                __,
                $"/// <inheritdoc/>",
                $"public new {modelInfo.RestrictedCapacityListInfo.RestrictedCapacityListClassNameWithoutInOutKeyword} DeepClone() => new(this);",
                $"object WodiLib.Sys.IDeepCloneable.DeepClone() => DeepClone();"
            );
        }
    }
}
