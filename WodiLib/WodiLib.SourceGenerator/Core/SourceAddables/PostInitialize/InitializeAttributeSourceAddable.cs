// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : InitializeAttributeSourceAddable.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Dtos;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;

namespace WodiLib.SourceGenerator.Core.SourceAddables.PostInitialize
{
    /// <summary>
    ///     <see cref="ISourceGenerator.Initialize"/> で
    ///     属性を追加する処理を行うためのテンプレートクラス
    /// </summary>
    internal abstract class InitializeAttributeSourceAddable : IInitializeSourceAddable
    {
        /// <summary>属性名</summary>
        public abstract string AttributeName { get; }

        /// <summary>属性付与可能対象</summary>
        public abstract AttributeTargets AttributeTargets { get; }

        /// <summary>Summary</summary>
        public abstract string Summary { get; }

        /// <summary>Remarks</summary>
        public virtual string? Remarks => null;

        /// <summary>派生クラスへの継承フラグ</summary>
        public virtual bool Inherited => true;

        /// <summary>複数属性付与可能フラグ</summary>
        public virtual bool AllowMultiple => false;

        /// <summary>名前空間</summary>
        public abstract string NameSpace { get; }

        /// <summary>型名</summary>
        public string TypeName => AttributeName;

        /// <summary>型名（フル）</summary>
        public string TypeFullName => $"{NameSpace}.{TypeName}";

        /// <summary>プロパティ一覧</summary>
        public abstract IEnumerable<PropertyInfo> Properties();

        public void Emit(IncrementalGeneratorPostInitializationContext context, ILogger logger)
        {
            try
            {
                context.AddSource(HintName(), Source());
            }
            catch (Exception ex)
            {
                context.AddSource($"{HintName()}_Error.log", ex.ToString());
            }
        }

        /// <returns>SourceGenerator用ヒント名</returns>
        private string HintName()
            => $"{$"{NameSpace}.{AttributeName}".CompressNameSpace()}.cs";

        /// <returns>SourceGenerator出力ソースコード</returns>
        private string Source()
        {
            return SourceTextFormatter.Format(
                new SourceFormatTarget[]
                {
                    $"namespace {NameSpace}",
                    $"{{",
                },
                SourceTextFormatter.Format(
                    IndentSpace,
                    new SourceFormatTarget[]
                    {
                        $"/// <summary>",
                        $"/// {__}{Summary}",
                        $"/// </summary>",
                    },
                    RemarksSourceText(),
                    new SourceFormatTarget[]
                    {
                        $"[System.AttributeUsage({AttributeTargets.ToSource()}, Inherited = {Inherited.ToString().ToLower()}, AllowMultiple = {AllowMultiple.ToString().ToLower()})]",
                        $"internal partial class {AttributeName} : System.Attribute",
                        $"{{",
                    },
                    SourceTextFormatter.ReduceMany(
                            IndentSpace,
                            Properties()
                                .Select(prop =>
                                    prop.ToSourceFormatTargets()
                                )
                                .ToArray()
                        )
                        .TrimLastEmptyLine()
                ),
                new SourceFormatTarget[]
                {
                    $"{__}}}",
                    $"}}",
                }
            );
        }

        /// <returns>ドキュメントコメント：Remarks</returns>
        private SourceFormatTargetBlock RemarksSourceText()
        {
            if (Remarks is null) return Array.Empty<SourceFormatTarget>();

            return SourceTextFormatter.Format(
                IndentSpace,
                new SourceFormatTarget[]
                {
                    $"/// <remarks>",
                    $"/// {__}{Remarks.TrimNewLine()}",
                    $"/// </remarks>",
                }
            );
        }
    }
}
