using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseValueIntTest : TestFixtureBase
    {
        [SetUp]
        public static void Setup()
        {
            InitializeTestHelpers();
        }

        #region Constructor

        /// <summary>
        ///     コンストラクタが正常に終了すること。
        /// </summary>
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public static void ConstructorIntTest_Success(int value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueInt(value),
                instanceVerifier: new ValueVerifier<DatabaseValueInt>(instance =>
                    {
                        // インスタンスが意図したとおり作成されること
                        Assert.AreEqual(instance.RawValue, value);
                    }
                )
            );
        }

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     int から DatabaseValueInt に暗黙的型変換できること。
        /// </summary>
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public static void CastIntToDatabaseValueIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<DatabaseValueInt>.AreEquals(value)
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     DatabaseValueInt から int に暗黙的型変換できること。
        /// </summary>
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public static void CastDatabaseValueIntToIntTest_Success(int value)
        {
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new DatabaseValueInt(value),
                resultValueVerifier: ValueVerifier<int>.AreEquals(value)
            );
        }

        #endregion

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCase(int.MinValue, int.MinValue, true)]
        [TestCase(int.MinValue, int.MaxValue, false)]
        public static void OperatorEqualTest(int left, int right, bool expected)
        {
            var leftValue = (DatabaseValueInt)left;
            var rightValue = (DatabaseValueInt)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase(int.MinValue, int.MinValue, false)]
        [TestCase(int.MinValue, int.MaxValue, true)]
        public static void OperatorNotEqualTest(int left, int right, bool expected)
        {
            var leftValue = (DatabaseValueInt)left;
            var rightValue = (DatabaseValueInt)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(int.MinValue, int.MinValue, true)]
        [TestCase(int.MinValue, int.MaxValue, false)]
        public static void OperatorEqualsTest(int left, int right, bool expected)
        {
            var leftValue = (DatabaseValueInt)left;
            var rightValue = (DatabaseValueInt)right;

            pureFunctionTestHelper.PureFuncSuccess(
                instance: leftValue,
                execFunc: target => target.Equals(rightValue),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase(1, 1, true)]
        [TestCase(1, 2, false)]
        [TestCase(1, null, false)]
        public static void EqualsTest_DatabaseValueInt(int left, int? right, bool expected)
        {
            var leftValue = (DatabaseValueInt)left;
            var rightValue = (DatabaseValueInt?)right;

            equalsTestHelper.Equals(
                leftValue,
                rightValue,
                expected
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { 1, new DatabaseValueInt(1), true },
            new object?[] { 1, new DatabaseValueInt(2), false },
            new object?[] { 1, 1, false },
            new object?[] { 1, "1", false },
            new object?[] { 1, null, false },
        };

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsObjectTestCaseSource))]
        public static void EqualsTest_Object(int left, object? right, bool expected)
        {
            var leftValue = (DatabaseValueInt)left;

            equalsTestHelper.Equals(
                leftValue,
                right,
                expected
            );
        }

        #endregion

        #endregion
    }
}
