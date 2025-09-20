using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace

namespace WodiLib.Sys
{
    internal static partial class WodiLibContainer
    {
        static WodiLibContainer()
        {
            RegisterModels();
        }

        static partial void RegisterModels();
    }

    internal class SumType2<T0, T1>
    {
        private static readonly List<Type> genericTypes;
        private readonly int genericNo;
        private readonly dynamic genericValue;

        static SumType2()
        {
            genericTypes = new List<Type> { typeof(T0), typeof(T1) };
        }

        public SumType2(T0 value)
        {
            genericValue = value!;
            genericNo = 0;
        }

        public SumType2(T1 value)
        {
            genericValue = value!;
            genericNo = 1;
        }

        private Type MyType => genericTypes[genericNo];
        public bool CanCast<T>() => CanCast(typeof(T));
        public bool CanCast(Type type) => type == MyType;

        public T Cast<T>()
        {
            var type = typeof(T);
            if (!CanCast(type))
            {
                throw new InvalidCastException($"Cannot cast type of {type}");
            }

            return (T)genericValue;
        }
    }
}
