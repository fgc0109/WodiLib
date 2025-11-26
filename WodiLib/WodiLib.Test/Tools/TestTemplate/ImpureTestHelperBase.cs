using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    internal abstract class ImpureTestHelperBase : TestHelperBase
    {
        protected ImpureTestHelperBase(WodiLibLogger logger) : base(logger)
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
