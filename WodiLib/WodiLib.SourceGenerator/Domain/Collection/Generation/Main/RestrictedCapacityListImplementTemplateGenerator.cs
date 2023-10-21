// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : RestrictedCapacityListImplementTemplateGenerator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using WodiLib.SourceGenerator.Core.Templates.FromAttribute;
using WodiLib.SourceGenerator.Domain.Collection.Generation.PostInitAction.Attributes;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Domain.Collection.Generation.Main
{
    /// <summary>
    ///     テンプレートを用いたリスト実装クラス生成
    /// </summary>
    internal class RestrictedCapacityListImplementTemplateGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            RestrictedCapacityListImplementTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var className = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[RestrictedCapacityListImplementTemplateAttribute.Description.Name]!;
            var maxCapacity = propertyValues[RestrictedCapacityListImplementTemplateAttribute.MaxCapacity.Name]!;
            var minCapacity = propertyValues[RestrictedCapacityListImplementTemplateAttribute.MinCapacity.Name]!;
            var interfaceItemType =
                propertyValues[RestrictedCapacityListImplementTemplateAttribute.InterfaceItemType.Name]!;
            var fixedLengthInterfaceItemType =
                propertyValues[RestrictedCapacityListImplementTemplateAttribute.FixedLengthInterfaceItemType.Name]
                ?? interfaceItemType;
            var readOnlyInterfaceItemType =
                propertyValues[RestrictedCapacityListImplementTemplateAttribute.ReadOnlyInterfaceItemType.Name]
                ?? fixedLengthInterfaceItemType;
            var isOverrideMakeDefaultItem =
                bool.Parse(
                    propertyValues[
                        RestrictedCapacityListImplementTemplateAttribute.IsAutoOverrideMakeDefaultItem.Name]!
                );

            var fixedLengthClassName = $"FixedLength{className}";
            var readOnlyClassName = $"ReadOnly{className}";

            var lazyReadOnlyInstanceInitializeSource =
                $"lazyReadOnlyInstance = new System.Lazy<{readOnlyClassName}>(() => new {readOnlyClassName}(Items));";

            var lazyFixedLengthInstanceInitializeSource =
                $"lazyFixedLengthInstance = new System.Lazy<{fixedLengthClassName}>(() => new {fixedLengthClassName}(Items, lazyReadOnlyInstance));";

            return SourceTextFormatter.Format(
                "",

                #region RestrictedCapacityList

                new[]
                {
                    $"/// <summary>",
                    $"/// {__}{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {className} : Sys.Collections.RestrictedCapacityList<{interfaceItemType}, {className}>,",
                    $"{__}WodiLib.Sys.Collections.ICastableFixedLengthList<{fixedLengthClassName}, {fixedLengthInterfaceItemType}>,",
                    $"{__}WodiLib.Sys.Collections.ICastableReadOnlyExtendedList<{readOnlyClassName}, {readOnlyInterfaceItemType}>",
                    $"{{",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ完了後の処理",
                    $"{__}/// </summary>",
                    $"{__}partial void PostConstructor();",
                    $"",
                    $"{__}/// <summary>容量最大値</summary>",
                    $"{__}public static int MaxCapacity => {maxCapacity};",
                    $"{__}/// <summary>容量最小値</summary>",
                    $"{__}public static int MinCapacity => {minCapacity};",
                    $"",
                    $"{__}/// <summary>容量固定インスタンス</summary>",
                    $"{__}private readonly System.Lazy<{fixedLengthClassName}> lazyFixedLengthInstance;",
                    $"{__}/// <summary>読取専用インスタンス</summary>",
                    $"{__}private readonly System.Lazy<{readOnlyClassName}> lazyReadOnlyInstance;",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}public {className}() {{",
                    $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                    $"{__}{__}{lazyFixedLengthInstanceInitializeSource}",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"length\">要素数</param>",
                    $"{__}/// <exception cref=\"System.ArgumentOutOfRangeException\">",
                    $"{__}///{__} <paramref name=\"length\"/> が <see cref=\"MinCapacity\"/> 未満または <see cref=\"MaxCapacity\"/> を超える場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(int length) : base(length) {{",
                    $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                    $"{__}{__}{lazyFixedLengthInstanceInitializeSource}",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"initItems\">初期要素</param>",
                    $"{__}/// <exception cref=\"System.ArgumentNullException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> が <see langword=\"null\"/> の場合、",
                    $"{__}///{__} または <paramref name=\"initItems\"/> 中に <see langword=\"null\"/> が含まれる場合。",
                    $"{__}/// </exception>",
                    $"{__}/// <exception cref=\"System.ArgumentException\">",
                    $"{__}///{__} <paramref name=\"initItems\"/> の要素数が <see cref=\"MinCapacity\"/> 未満",
                    $"{__}///{__} または <see cref=\"MaxCapacity\"/> を超える場合。",
                    $"{__}/// </exception>",
                    $"{__}public {className}(System.Collections.Generic.IEnumerable<{interfaceItemType}> initItems) : base(initItems) {{",
                    $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                    $"{__}{__}{lazyFixedLengthInstanceInitializeSource}",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}/// <param name=\"itemsImpl\">リスト実装インスタンス</param>",
                    $"{__}internal {className}(WodiLib.Sys.Collections.IExtendedList<{interfaceItemType}> itemsImpl) : base(itemsImpl) {{",
                    $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                    $"{__}{__}{lazyFixedLengthInstanceInitializeSource}",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override int GetMaxCapacity() => MaxCapacity;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override int GetMinCapacity() => MinCapacity;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public {fixedLengthClassName} AsFixedLengthList() => lazyFixedLengthInstance.Value;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public {readOnlyClassName} AsReadOnlyList() => lazyReadOnlyInstance.Value;",
                    $""
                },
                SourceTextFormatter.If(
                    isOverrideMakeDefaultItem,
                    new[]
                    {
                        $"{__}/// <inheritdoc/>",
                        $"{__}protected override {interfaceItemType} MakeDefaultItem(int index) => new();",
                        $""
                    }
                ),
                new[]
                {
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override {className} DeepClone() => new(this);",
                    $"}}"
                },

                #endregion

                #region FixedLengthList

                new[]
                {
                    $"",
                    $"/// <summary>",
                    $"/// {__}【容量固定】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {fixedLengthClassName} : Sys.Collections.FixedLengthList<{fixedLengthInterfaceItemType}, {fixedLengthClassName}>,",
                    $"{__}WodiLib.Sys.Collections.ICastableReadOnlyExtendedList<{readOnlyClassName}, {readOnlyInterfaceItemType}>",
                    $"{{",
                    $"{__}/// <summary>読取専用インスタンス</summary>",
                    $"{__}private readonly System.Lazy<{readOnlyClassName}> lazyReadOnlyInstance;",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ完了後の処理",
                    $"{__}/// </summary>",
                    $"{__}partial void PostConstructor();",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}internal {fixedLengthClassName}(WodiLib.Sys.Collections.IExtendedList<{fixedLengthInterfaceItemType}> itemsImpl, System.Lazy<{readOnlyClassName}> lazyReadOnlyInstance) : base(itemsImpl) {{",
                    $"{__}{__}this.lazyReadOnlyInstance = lazyReadOnlyInstance;",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} ディープコピーコンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}private {fixedLengthClassName}(System.Collections.Generic.IEnumerable<{fixedLengthInterfaceItemType}> items) : base(items) {{",
                    $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public {readOnlyClassName} AsReadOnlyList() => lazyReadOnlyInstance.Value;",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}protected override {interfaceItemType} MakeDefaultItem(int index) => throw new System.InvalidOperationException();",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override {fixedLengthClassName} DeepClone() => new(this);",
                    $"}}"
                },

                #endregion

                #region ReadOnlyExtendedList

                new[]
                {
                    $"",
                    $"/// <summary>",
                    $"/// {__}【読取専用】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {readOnlyClassName} : Sys.Collections.ReadOnlyExtendedList<{readOnlyInterfaceItemType}, {readOnlyClassName}>",
                    $"{{",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ完了後の処理",
                    $"{__}/// </summary>",
                    $"{__}partial void PostConstructor();",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}internal {readOnlyClassName}(WodiLib.Sys.Collections.IExtendedList<{readOnlyInterfaceItemType}> itemsImpl) : base(itemsImpl) {{",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <summary>",
                    $"{__}///{__} ディープコピーコンストラクタ",
                    $"{__}/// </summary>",
                    $"{__}private {readOnlyClassName}(System.Collections.Generic.IEnumerable<{readOnlyInterfaceItemType}> items) : base(items) {{",
                    $"{__}{__}PostConstructor();",
                    $"{__}}}",
                    $"",
                    $"{__}/// <inheritdoc/>",
                    $"{__}public override {readOnlyClassName} DeepClone() => new(this);",
                    $"}}"
                }

                #endregion

            );
        }

        private RestrictedCapacityListImplementTemplateGenerator()
        {
        }

        public static RestrictedCapacityListImplementTemplateGenerator Instance { get; } = new();
    }
}
