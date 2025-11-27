using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class FieldNameTest : TestFixtureBase
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
                factory: () => new FieldName(value),
                instanceVerifier: new ValueVerifier<FieldName>(instance =>
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
                factory: () => new FieldName(null!),
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
                factory: () => new FieldName("Hello\r\nWorld!"),
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
            var instance1 = new FieldName("a");
            var instance2 = new FieldName("a");
            var instance3 = new FieldName("aa");

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
                instance: new FieldName(value),
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
        ///     string から FieldName に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastStringToFieldNameTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<FieldName>.AreEquals(value)
            );
        }

        /// <summary>
        ///     改行を含む文字 から FieldName に暗黙的型変換した場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void CastStringToFieldNameTest_Failure_NewLine()
        {
            staticFunctionTestHelper.StaticFuncFailure<FieldName>(
                execFunc: () => (string)"Wolf\nRPG\nEditor.",
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     FieldName から string に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastFieldNameToStringTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new FieldName(value),
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
            var leftValue = (FieldName?)left;
            var rightValue = (FieldName?)right;

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
            var leftValue = (FieldName?)left;
            var rightValue = (FieldName?)right;

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
        public static void EqualsTest_FieldName(string left, string? right, bool expected)
        {
            var leftValue = (FieldName)left;
            var rightValue = (FieldName?)right;

            equalsTestHelper.Equals(
                leftValue,
                rightValue,
                expected
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { "abc", new FieldName("abc"), true },
            new object?[] { "abc", new FieldName("cba"), false },
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
            var leftValue = (FieldName)left;

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
