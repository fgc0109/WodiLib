using System;
using NUnit.Framework;
using WodiLib.Database;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Database.ValueObject
{
    [TestFixture]
    public class DBSettingFolderNameTest : TestFixtureBase
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
                factory: () => new DBSettingFolderName(value),
                instanceVerifier: new ValueVerifier<DBSettingFolderName>(instance =>
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
                factory: () => new DBSettingFolderName(null!),
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
                factory: () => new DBSettingFolderName("Hello\r\nWorld!"),
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
            var instance1 = new DBSettingFolderName("a");
            var instance2 = new DBSettingFolderName("a");
            var instance3 = new DBSettingFolderName("aa");

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
                instance: new DBSettingFolderName(value),
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
        ///     string から DBSettingFolderName に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastStringToDBSettingFolderNameTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<DBSettingFolderName>.AreEquals(value)
            );
        }

        /// <summary>
        ///     改行を含む文字 から DBSettingFolderName に暗黙的型変換した場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void CastStringToDBSettingFolderNameTest_Failure_NewLine()
        {
            staticFunctionTestHelper.StaticFuncFailure<DBSettingFolderName>(
                execFunc: () => (string)"Wolf\nRPG\nEditor.",
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     DBSettingFolderName から string に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastDBSettingFolderNameToStringTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new DBSettingFolderName(value),
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
            var leftValue = (DBSettingFolderName?)left;
            var rightValue = (DBSettingFolderName?)right;

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
            var leftValue = (DBSettingFolderName?)left;
            var rightValue = (DBSettingFolderName?)right;

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
        public static void EqualsTest_DBSettingFolderName(string left, string? right, bool expected)
        {
            var leftValue = (DBSettingFolderName)left;
            var rightValue = (DBSettingFolderName?)right;

            equalsTestHelper.Equals(
                leftValue,
                rightValue,
                expected
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { "abc", new DBSettingFolderName("abc"), true },
            new object?[] { "abc", new DBSettingFolderName("cba"), false },
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
            var leftValue = (DBSettingFolderName)left;

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
