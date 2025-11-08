// ========================================
// Project Name : WodiLib
// File Name    : TypeConverter.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WodiLib.Sys
{
    internal static class TypeConverter<TSource, TTarget>
    {
        private static readonly Func<TSource, TTarget> _converter = CreateConverter();

        public static TTarget Convert(TSource value) => _converter(value);

        private static Func<TSource, TTarget> CreateConverter()
        {
            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            if (targetType.IsAssignableFrom(sourceType))
            {
                // そのままキャスト可能
                var param = Expression.Parameter(sourceType, "value");
                var body = Expression.Convert(param, targetType);
                return Expression.Lambda<Func<TSource, TTarget>>(body, param).Compile();
            }

            // 暗黙または明示的変換演算子を探す
            var op = FindUserDefinedConversion(sourceType, targetType)
                ?? FindUserDefinedConversion(targetType, sourceType);

            if (op is not null)
            {
                var param = Expression.Parameter(sourceType, "value");
                var body = Expression.Call(op, param);
                return Expression.Lambda<Func<TSource, TTarget>>(body, param).Compile();
            }

            return new Func<TSource, TTarget>(_ => throw new InvalidCastException());
        }

        private static MethodInfo? FindUserDefinedConversion(Type from, Type to)
        {
            return from.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Concat(to.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .FirstOrDefault(m =>
                    (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                    m.ReturnType == to &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == from);
        }
    }

    internal static class TypeConverterExtension
    {
        public static TTarget Cast<TSource, TTarget>(this TSource value)
            => TypeConverter<TSource, TTarget>.Convert(value);
    }
}