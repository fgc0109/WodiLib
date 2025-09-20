using System.Collections.Generic;
using Commons;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    internal abstract class ImpureTestHelperBase : TestHelperBase
    {
        protected ImpureTestHelperBase(Logger logger) : base(logger)
        {
        }

        protected void AssertEqualsNotifiedPropertyNames(
            IReadOnlyList<string> expected,
            IReadOnlyList<string> actual
        )
        {
            Assert.AreEqual(
                expected.Count,
                actual.Count,
                $"Expected: {expected.Count} ({string.Join(",", expected)})))\n"
                + $"  But was: {actual.Count} ({string.Join(",", actual)})"
            );
            CustomAssert.AreSequenceEquals(expected, actual);
        }
    }
}
