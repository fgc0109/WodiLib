using Commons;
using NUnit.Framework;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys.Collections
{
    [TestFixture]
    [Ignore("StandardListValidatorTest 内でテストされるため、固有のユニットテストは行わない")]
    public class ListValidationHelperTest
    {
        private static Logger logger = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }
    }
}
