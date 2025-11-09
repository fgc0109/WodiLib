// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : GeneratorBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WodiLib.SourceGenerator.Core.Extensions;

namespace WodiLib.SourceGenerator.Core
{
    /// <summary>
    ///     各種Generator基底クラス
    /// </summary>
    public abstract class GeneratorBase : IIncrementalGenerator
    {
        /// <summary>ログファイルに付与するキー名</summary>
        private protected abstract string Key { get; }

        private protected ILogger Logger { get; } = new DefaultLogger();

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            try
            {
                Logger.AppendLine($"{Key} Start");

                /*
                    必要な Attribute, Enum などの初期登録を行う
                */
                context.RegisterPostInitializationOutput(ctx =>
                    {
                        var cnt = 0;
                        foreach (var info in PostInitializationRegisterInfoList)
                        {
                            cnt++;
                            try
                            {
                                Logger.AppendLine(info.ToString());
                                info.Emit(ctx, Logger);
                            }
                            catch (Exception ex)
                            {
                                Logger.AppendLine(
                                    $"        {cnt}{Environment.NewLine}{info}{Environment.NewLine}{ex}{Environment.NewLine}"
                                );
                            }
                        }

                        if (Logger.HasLog)
                        {
                            ctx.AddSource($"{Key}_RegisterForPostInitialization.log", Logger.ToOutputText());
                        }
                    }
                );

                /*
                    Provider 作成・出力処理登録
                */
                foreach (var targetAttributeInfo in TargetAttributeInfoList)
                {
                    var provider = context.SyntaxProvider.CreateSyntaxProvider(
                            predicate: static (syntaxNode, _) =>
                                // クラス・レコード・構造体のみ対象
                                (syntaxNode is ClassDeclarationSyntax cls && cls.AttributeLists.Count > 0)
                                || (syntaxNode is RecordDeclarationSyntax rec && rec.AttributeLists.Count > 0)
                                || (syntaxNode is StructDeclarationSyntax str && str.AttributeLists.Count > 0),
                            transform: (ctx, _) =>
                            {
                                // 対象ノードをキャスト
                                var typeDecl = ctx.Node as BaseTypeDeclarationSyntax;
                                if (typeDecl is null) return null;

                                // 属性ごとに確認
                                foreach (var attrList in typeDecl.AttributeLists)
                                {
                                    foreach (var attrSyntax in attrList.Attributes)
                                    {
                                        var symbolInfo = ctx.SemanticModel.GetSymbolInfo(attrSyntax);
                                        var ctorSymbol = symbolInfo.Symbol as IMethodSymbol;
                                        if (ctorSymbol is null) continue;

                                        var attrClassSymbol = ctorSymbol.ContainingType;

                                        if (IsTargetAttribute(attrClassSymbol, targetAttributeInfo))
                                        {
                                            // 条件にマッチした型を返す
                                            if (ctx.SemanticModel.GetDeclaredSymbol(typeDecl) is INamedTypeSymbol
                                                typeSymbol)
                                            {
                                                return new Tuple<SemanticModel, BaseTypeDeclarationSyntax,
                                                    INamedTypeSymbol>(ctx.SemanticModel, typeDecl, typeSymbol);
                                            }

                                            return null;
                                        }
                                    }
                                }

                                // マッチしなかった場合は null
                                return null;
                            }
                        )
                        // null を除外
                        .Where(static x => x != null);

                    context.RegisterSourceOutput(
                        provider,
                        (ctx, tuple) =>
                        {
                            var (semanticModel, typeDecl, typeSymbol) = tuple;

                            try
                            {
                                Logger.AppendLine($"start Emit {targetAttributeInfo.TargetAttributeFullName}.");

                                targetAttributeInfo.EmitGeneratedSource(
                                    ctx,
                                    semanticModel,
                                    typeDecl,
                                    typeSymbol,
                                    Logger
                                );
                            }
                            catch (Exception ex)
                            {
                                Logger.AppendLine(ex.ToString());
                                ctx.AddSource($"{Key}_EmitGeneratedSourceError.log", ex.ToString());
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Logger.AppendLine($"{Key} Start Error");
                Logger.AppendLine(ex.ToString());

                context.RegisterPostInitializationOutput(ctx =>
                    ctx.AddSource(
                        $"{Key}_InitializeError.log",
                        $"// {ex.Message}{Environment.NewLine}// {ex.StackTrace}\r\n{Logger.ToOutputText()}"
                    )
                );
                Console.WriteLine(ex.Message);
            }
        }

        private bool IsTargetAttribute(INamedTypeSymbol attrSymbol, IMainSourceAddable targetAttributeInfo)
        {
            return attrSymbol.IsSameOrExtended(targetAttributeInfo.TargetAttributeFullName);
        }

        /// <summary>
        ///     初期登録に必要な Attribute や Enum などの情報一覧
        /// </summary>
        private protected abstract IInitializeSourceAddable[] PostInitializationRegisterInfoList { get; }

        /// <summary>SourceGenerator で自動生成する対象の属性情報とその出力処理</summary>
        private protected abstract IMainSourceAddable[] TargetAttributeInfoList { get; }
    }
}
