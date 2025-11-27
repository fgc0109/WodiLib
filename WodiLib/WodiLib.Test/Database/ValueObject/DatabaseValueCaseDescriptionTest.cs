using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DatabaseValueCaseDescriptionTest : TestFixtureBase
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
        [TestCase("")]
        [TestCase("abc")]
        [TestCase("あいうえお")]
        public static void ConstructorStringTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new DatabaseValueCaseDescription(value),
                instanceVerifier: new ValueVerifier<DatabaseValueCaseDescription>(instance =>
                    {
                        // インスタンスが意図したとおり作成されること
                        Assert.AreEqual(instance.RawValue, value);
                    }
                )
            );
        }

        /// <summary>
        ///     引数が null の場合、
        ///     ArgumentNullException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorStringTest_Failure_NullArgs()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseValueCaseDescription(null!),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentNullException))
            );
        }

        /// <summary>
        ///     引数に改行が含まれる場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorStringTest_Failure_NewLine()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new DatabaseValueCaseDescription("Hello\r\nWorld!"),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region Methods

        #region public

        #region GetHashCode

        [Test]
        public static void GetHashCodeTest()
        {
            var instance1 = new DatabaseValueCaseDescription("a");
            var instance2 = new DatabaseValueCaseDescription("a");
            var instance3 = new DatabaseValueCaseDescription("aa");

            // 同じ値は同じハッシュコードを返すこと
            Assert.AreEqual(instance1.GetHashCode(), instance2.GetHashCode());

            // 異なる値は異なるハッシュコードを返すこと
            Assert.AreNotEqual(instance1.GetHashCode(), instance3.GetHashCode());
        }

        #endregion

        #region ToString

        [Test]
        public static void ToStringTest_Success()
        {
            const string value = "String TestValue";
            pureFunctionTestHelper.PureFuncSuccess(
                instance: new DatabaseValueCaseDescription(value),
                execFunc: target => target.ToString(),
                resultValueVerifier: ValueVerifier.AreEquals(value)
            );
        }

        #endregion

        #endregion

        #endregion

        #region Cast

        #region From

        /// <summary>
        ///     string から DatabaseValueCaseDescription に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastStringToDatabaseValueCaseDescriptionTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<DatabaseValueCaseDescription>.AreEquals(value)
            );
        }

        /// <summary>
        ///     改行を含む文字 から DatabaseValueCaseDescription に暗黙的型変換した場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void CastStringToDatabaseValueCaseDescriptionTest_Failure_NewLine()
        {
            staticFunctionTestHelper.StaticFuncFailure<DatabaseValueCaseDescription>(
                execFunc: () => (string)"Wolf\nRPG\nEditor.",
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     DatabaseValueCaseDescription から string に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastDatabaseValueCaseDescriptionToStringTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new DatabaseValueCaseDescription(value),
                resultValueVerifier: ValueVerifier.AreEquals(value)
            );
        }

        #endregion

        #endregion

        #region Operation

        #region Equal / Equals(Method)

        /// <summary>
        ///     等価比較演算 == が意図した値を返すこと。
        /// </summary>
        [TestCase("a", "a", true)]
        [TestCase("a", "b", false)]
        [TestCase("a", null, false)]
        [TestCase(null, "b", false)]
        public static void OperatorEqualTest(string? left, string? right, bool expected)
        {
            var leftValue = (DatabaseValueCaseDescription?)left;
            var rightValue = (DatabaseValueCaseDescription?)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase("a", "a", false)]
        [TestCase("a", "b", true)]
        [TestCase("a", null, true)]
        [TestCase(null, "b", true)]
        public static void OperatorNotEqualTest(string? left, string? right, bool expected)
        {
            var leftValue = (DatabaseValueCaseDescription?)left;
            var rightValue = (DatabaseValueCaseDescription?)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase("a", "a", true)]
        [TestCase("a", "b", false)]
        [TestCase("a", null, false)]
        public static void EqualsTest_DatabaseValueCaseDescription(string left, string? right, bool expected)
        {
            var leftValue = (DatabaseValueCaseDescription)left;
            var rightValue = (DatabaseValueCaseDescription?)right;

            equalsTestHelper.Equals(
                leftValue,
                rightValue,
                expected
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { "abc", new DatabaseValueCaseDescription("abc"), true },
            new object?[] { "abc", new DatabaseValueCaseDescription("cba"), false },
            new object?[] { "abc", "abc", false },
            new object?[] { "abc", 10, false },
            new object?[] { "abc", null, false },
        };

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCaseSource(nameof(EqualsObjectTestCaseSource))]
        public static void EqualsTest_Object(string left, object? right, bool expected)
        {
            var leftValue = (DatabaseValueCaseDescription)left;

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
