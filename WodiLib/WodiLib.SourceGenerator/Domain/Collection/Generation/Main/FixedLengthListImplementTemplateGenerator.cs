// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : FixedLengthListImplementTemplateGenerator.cs
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
    internal class FixedLengthListImplementTemplateGenerator : MainSourceAddableTemplate
    {
        public override InitializeAttributeSourceAddable TargetAttribute =>
            FixedLengthListImplementTemplateAttribute.Instance;

        private protected override SourceFormatTargetBlock GenerateTypeDefinitionSource(WorkState workState)
        {
            var propertyValues = workState.PropertyValues;
            var fixedLengthClassName = workState.Name;

            var typeDefinitionInfo = workState.CurrentTypeDefinitionInfo;
            var accessibility = AccessibilityConverter.ConvertSourceText(typeDefinitionInfo.Accessibility);

            var description = propertyValues[FixedLengthListImplementTemplateAttribute.Description.Name]!;
            var maxCapacity = propertyValues[FixedLengthListImplementTemplateAttribute.MaxCapacity.Name]!;
            var minCapacity = propertyValues[FixedLengthListImplementTemplateAttribute.MinCapacity.Name]!;
            var interfaceItemType =
                propertyValues[FixedLengthListImplementTemplateAttribute.InterfaceItemType.Name]!;
            var readOnlyInterfaceItemType =
                propertyValues[FixedLengthListImplementTemplateAttribute.ReadOnlyInterfaceItemType.Name]
                ?? interfaceItemType;
            var isOverrideMakeDefaultItem =
                bool.Parse(
                    propertyValues[
                        FixedLengthListImplementTemplateAttribute.IsAutoOverrideMakeDefaultItem.Name]!
                );

            var readOnlyClassName = fixedLengthClassName.Replace("FixedLength", "ReadOnly");

            var lazyReadOnlyInstanceInitializeSource =
                $"lazyReadOnlyInstance = new System.Lazy<{readOnlyClassName}>(() => new {readOnlyClassName}(Items));";

            return SourceTextFormatter.Format(
                "",

                #region FixedLengthList

                new[]
                {
                    $"/// <summary>",
                    $"/// {__}【容量固定】{description}",
                    $"/// </summary>",
                    $"{accessibility} partial class {fixedLengthClassName} : Sys.Collections.FixedLengthList<{interfaceItemType}, {fixedLengthClassName}>,",
                    $"{__}WodiLib.Sys.Collections.ICastableReadOnlyExtendedList<{readOnlyClassName}, {readOnlyInterfaceItemType}>",
                    $"{{",
                    $"{__}/// <summary>",
                    $"{__}///{__} コンストラクタ完了後の処理",
                    $"{__}/// </summary>",
                    $"{__}partial void PostConstructor();",
                    $"",
                    $"{__}/// <summary>読取専用インスタンス</summary>",
                    $"{__}private readonly System.Lazy<{readOnlyClassName}> lazyReadOnlyInstance;",
                    $""
                },
                SourceTextFormatter.If(
                    maxCapacity != minCapacity,
                    new[]
                    {
                        $"{__}/// <summary>容量最大値</summary>",
                        $"{__}protected static int MaxCapacity => {maxCapacity};",
                        $"{__}/// <summary>容量最小値</summary>",
                        $"{__}protected static int MinCapacity => {minCapacity};",
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
                        $"{__}public {fixedLengthClassName}(System.Collections.Generic.IEnumerable<{interfaceItemType}> initItems) : base(initItems, MinCapacity, MaxCapacity) {{",
                        $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                        $"{__}{__}PostConstructor();",
                        $"{__}}}",
                        $""
                    }
                ),
                SourceTextFormatter.If(
                    maxCapacity == minCapacity,
                    new[]
                    {
                        $"{__}/// <summary>容量</summary>",
                        $"{__}public static int Capacity => {maxCapacity};",
                        $"",
                        $"{__}/// <summary>",
                        $"{__}///{__} コンストラクタ",
                        $"{__}/// </summary>",
                        $"{__}public {fixedLengthClassName}() : base(Capacity) {{",
                        $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
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
                        $"{__}///{__} <paramref name=\"initItems\"/> の要素数が <see cref=\"Capacity\"/> と一致しない場合。",
                        $"{__}/// </exception>",
                        $"{__}public {fixedLengthClassName}(System.Collections.Generic.IEnumerable<{interfaceItemType}> initItems) : base(initItems, Capacity) {{",
                        $"{__}{__}{lazyReadOnlyInstanceInitializeSource}",
                        $"{__}{__}PostConstructor();",
                        $"{__}}}"
                    }
                ),
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
                    $"{__}public {readOnlyClassName} AsReadOnlyList() => lazyReadOnlyInstance.Value;",
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
                    $"{accessibility} partial class {readOnlyClassName} : Sys.Collections.ReadOnlyExtendedList<{interfaceItemType}, {readOnlyClassName}>",
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

        private FixedLengthListImplementTemplateGenerator()
        {
        }

        public static FixedLengthListImplementTemplateGenerator Instance { get; } = new();
    }
}
