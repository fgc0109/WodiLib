using Commons;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     テストクラス基底クラス
    /// </summary>
    /// <remarks>
    ///     各種TestHelperクラスを定義し、初期化を行うメソッドを提供する。
    /// </remarks>
    public abstract class TestFixtureBase
    {
        private protected static Logger logger = null!;

        private protected static PropertyTestHelper propertyTestHelper = null!;
        private protected static ConstructorTestHelper constructorTestHelper = null!;
        private protected static PureActionTestHelper pureActionTestHelper = null!;
        private protected static PureFunctionTestHelper pureFunctionTestHelper = null!;
        private protected static ImpureActionTestHelper impureActionTestHelper = null!;
        private protected static ImpureFunctionTestHelper impureFunctionTestHelper = null!;
        private protected static EqualsTestHelper equalsTestHelper = null!;
        private protected static ItemEqualsTestHelper itemEqualsTestHelper = null!;
        private protected static DeepCloneTestHelper deepCloneTestHelper = null!;
        private protected static StaticFunctionTestHelper staticFunctionTestHelper = null!;
        private protected static StaticActionTestHelper staticActionTestHelper = null!;

        private protected static void InitializeTestHelpers()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();

            propertyTestHelper = new PropertyTestHelper(logger);
            constructorTestHelper = new ConstructorTestHelper(logger);
            pureActionTestHelper = new PureActionTestHelper(logger);
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
            impureActionTestHelper = new ImpureActionTestHelper(logger);
            impureFunctionTestHelper = new ImpureFunctionTestHelper(logger);
            equalsTestHelper = new EqualsTestHelper(logger);
            itemEqualsTestHelper = new ItemEqualsTestHelper(logger);
            deepCloneTestHelper = new DeepCloneTestHelper(logger);
            staticFunctionTestHelper = new StaticFunctionTestHelper(logger);
            staticActionTestHelper = new StaticActionTestHelper(logger);
        }
    }
}
