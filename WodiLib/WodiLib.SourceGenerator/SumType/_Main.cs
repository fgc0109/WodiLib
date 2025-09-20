// ========================================
// Project Name : WodiLib.SourceGenerator
// File Name    : _Main.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using WodiLib.SourceGenerator.Core.Extensions;
using WodiLib.SourceGenerator.Core.SourceBuilder;
using static WodiLib.SourceGenerator.Core.SourceBuilder.SourceConstants;
using static WodiLib.SourceGenerator.SumType.GenerationConst;

namespace WodiLib.SourceGenerator.SumType
{
    /// <summary>
    ///     共用型SourceGenerator
    /// </summary>
    [Generator]
    public class Generator : ISourceGenerator
    {
        private const string NoFieldName = "genericNo";
        private const string ValueFieldName = "genericValue";

        /// <summary>Unionする型の数の候補</summary>
        private readonly int[] unionTypeLengths =
        {
            2,
        };

        /// <inheritdoc/>
        public void Initialize(GeneratorInitializationContext context)
        {
            // 事前準備の段階でクラスを登録する
            context.RegisterForPostInitialization(i =>
                {
                    var hintName = HintName;
                    var source = GenerateSource();
                    i.AddSource(hintName, source);
                }
            );
        }

        /// <inheritdoc/>
        public void Execute(GeneratorExecutionContext context)
        {
            // こちらでは何もしない
        }

        private static string HintName => FullName.ReplaceAngleBracketsToUnderscore();

        private string GenerateSource()
        {
            return SourceTextFormatter.Format(
                new SourceFormatTarget[]
                {
                    $"using System;",
                    $"using System.Collections.Generic;",
                    $"",
                    $"namespace {Namespace}",
                    $"{{",
                },
                SourceTextFormatter.Format(
                    IndentSpace,
                    unionTypeLengths.SelectMany(GenerateClassSource).ToArray()
                ),
                new SourceFormatTarget[]
                {
                    $"}}",
                }
            );
        }

        private static SourceFormatTargetBlock GenerateClassSource(int genericParamLength)
            => SourceTextFormatter.Format(
                "",
                new SourceFormatTarget[]
                {
                    $"internal class {ClassName}<{GenericParams(genericParamLength)}>",
                    $"{{",
                    // fields
                    $"{__}private static readonly List<Type> genericTypes;",
                    $"{__}private readonly int genericNo;",
                    $"{__}private readonly dynamic genericValue;",
                },
                SourceTextFormatter.Format(IndentSpace, StaticConstructor(genericParamLength)),
                SourceTextFormatter.Format(IndentSpace, ConstructorsSource(genericParamLength)),
                new SourceFormatTarget[]
                {
                    // methods
                    $"{__}private Type MyType => genericTypes[genericNo];",
                    $"{__}public bool CanCast<T>() => CanCast(typeof(T));",
                    $"{__}public bool CanCast(Type type) => type == MyType;",
                    $"{__}public T Cast<T>()",
                    $"{__}{{",
                    $"{__}{__}var type = typeof(T);",
                    $"{__}{__}if (!CanCast(type))",
                    $"{__}{__}{{",
                    $@"{__}{__}{__}throw new InvalidCastException($""Cannot cast type of {{type}}"");",
                    $"{__}{__}}}",
                    $"{__}{__}return (T)genericValue;",
                    $"{__}}}",
                    $"}}",
                }
            );

        private static SourceFormatTarget[] StaticConstructor(int genericParamLength)
        {
            var fieldGenericTypesInitValues = GenericParamNames(genericParamLength)
                .Select(t => $"typeof({t})");
            var fieldGenericTypesInitCode = string.Join(",", fieldGenericTypesInitValues);

            return new SourceFormatTarget[]
            {
                $"static {ClassName}()",
                $"{{",
                $"{__}genericTypes = new List<Type>{{{fieldGenericTypesInitCode}}};",
                $"}}",
            };
        }

        private static SourceFormatTarget[] ConstructorsSource(int genericParamLength)
            => Enumerable.Range(0, genericParamLength)
                .SelectMany(ConstructorSource)
                .ToArray();

        private static SourceFormatTarget[] ConstructorSource(int genericNo)
        {
            return new SourceFormatTarget[]
            {
                $"public {ClassName}(T{genericNo} value) {{ {ValueFieldName} = value!; {NoFieldName} = {genericNo}; }}",
            };
        }

        private static string GenericParams(int genericParamLength)
            => string.Join(", ", GenericParamNames(genericParamLength));

        private static IEnumerable<string> GenericParamNames(int genericParamLength)
            => Enumerable.Range(0, genericParamLength).Select(i => $"T{i}");
    }
}
