// ========================================
// Project Name : WodiLib.Test
// File Name    : AssertExtension.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     独自アサーション
    /// </summary>
    internal static class CustomAssert
    {
        public static void AreItemEquals<TExp, TAct>(TExp expected, TAct? actual, string? customMessage = null)
            where TExp : IEqualityComparable<TAct>
        {
            Assert.IsTrue(
                expected.ItemEquals(actual),
                $"Expected: {expected}\n  But was: {actual}"
                + (customMessage is not null
                    ? $", {customMessage}"
                    : "")
            );
        }

        public static void AreSequenceEquals<TExp, TAct>(IEnumerable<TExp> expected, IEnumerable<TAct?>? actual)
            where TAct : TExp
            => AreSequenceEquals<TExp>(expected, actual?.Cast<TExp>());

        public static void AreSequenceEquals<TExp, TAct>(
            IEnumerable<IEnumerable<TExp>> expected,
            IEnumerable<IEnumerable<TAct?>>? actual
        )
            where TAct : TExp
            => AreSequenceEquals<TExp>(expected, actual?.Cast<IEnumerable<TExp>>());

        public static void AreSequenceEquals<T>(
            IEnumerable<T> expected,
            IEnumerable<T?>? actual,
            IEqualityComparer<T>? comparer = null
        )
        {
            if (ReferenceEquals(expected, actual)) return;
            Assert.NotNull(actual);

            var expectedArray = expected.ToArray();
            var actualArray = actual!.ToArray();

            Assert.AreEqual(
                expectedArray.Length,
                actualArray.Length,
                $"Expected Size: {expectedArray.Length}\n  But was: {actualArray.Length}"
            );

            for (var i = 0; i < expectedArray.Length; i++)
            {
                var expectedItem = expectedArray[i];
                var actualItem = actualArray[i];
                Assert.IsTrue(
                    (comparer ?? EqualityComparerFactory.Create<T>()).Equals(expectedItem, actualItem),
                    $"Index={i}, Expected: {expectedItem}\n  But was: {actualItem}"
                );
            }
        }

        public static void AreSequenceEquals<T>(
            IEnumerable<IEnumerable<T?>> expected,
            IEnumerable<IEnumerable<T?>>? actual,
            IEqualityComparer<T>? comparer = null
        )
        {
            if (ReferenceEquals(expected, actual)) return;
            Assert.NotNull(actual);

            var expectedArray = expected.ToArray();
            var actualArray = actual!.ToArray();

            Assert.AreEqual(expectedArray.Length, actualArray.Length);

            for (var i = 0; i < expectedArray.Length; i++)
            {
                var expectedRow = expectedArray[i].ToArray();
                var actualRow = actualArray[i].ToArray();

                Assert.AreEqual(expectedRow.Length, actualRow.Length, $"i: {i}");

                for (var j = 0; j < expectedRow.Length; j++)
                {
                    var expectedItem = expectedRow[j];
                    var actualItem = actualRow[j];

                    Assert.IsTrue(
                        (comparer ?? EqualityComparerFactory.Create<T>()).Equals(expectedItem, actualItem),
                        $"i:{i}, j:{j}"
                    );
                }
            }
        }
    }
}
