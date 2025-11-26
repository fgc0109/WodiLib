using System;
using System.IO;
using NUnit.Framework;
using WodiLib.Cmn;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Cmn.ValueObject
{
    [TestFixture]
    public class FilePathTest : TestFixtureBase
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
        [TestCase("abc")]
        [TestCase("あいうえお")]
        [TestCase(@"c:\＜Not＞Error\file")]
        [TestCase(
            @"d:\Too\Long\Long\PathName\123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234"
        )]
        [TestCase(@"c:\COM0.test")]
        [TestCase(@".\relative\path.txt")]
        [TestCase(@"..\relative\path.txt")]
        [TestCase("file.🐺")]
        public static void ConstructorStringTest_Success(string value)
        {
            constructorTestHelper.ConstructorSuccess(
                factory: () => new FilePath(value),
                instanceVerifier: new ValueVerifier<FilePath>(instance =>
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
                factory: () => new FilePath(null!),
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
                factory: () => new FilePath("Hello\r\nWorld!"),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数が最小文字列長未満の場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorStringTest_Failure_Empty()
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new FilePath("12"),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数が最大文字列長を超える場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void ConstructorStringTest_Failure_OverMaxSize()
        {
            var value = new string('a', 32768);
            constructorTestHelper.ConstructorFailure(
                factory: () => new FilePath(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数がパス名に使用できない文字を含むと場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [TestCase(@"c:\Error:Path\file")]
        [TestCase(@"c:\Error<string>\file")]
        [TestCase(@"c:\Error|name.txt")]
        [TestCase(@"c:\Error""name.txt")]
        [TestCase(@".\Error*name")]
        [TestCase(@".\Error?name")] //
        [TestCase(@"c:\CON.test")]
        [TestCase(@"c:\COM1.test")]
        public static void ConstructorStringTest_Failure_InvalidChar(string value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new FilePath(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数にボリューム識別子の一部ではないコロンを含む場合、
        ///     NotSupportedException が発生すること。
        /// </summary>
        [TestCase(@"c:\Error:Path\file")]
        [TestCase(@"c:\Error<string>\file")]
        public static void ConstructorStringTest_Failure_InvalidColon(string value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new FilePath(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        /// <summary>
        ///     引数がパスとして長過ぎる場合、
        ///     PathTooLongException が発生すること。
        /// </summary>
        // [TestCase(
        //     @"d:\Too\Long\Long\PathName\1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345"
        // )]
        [Ignore("システムでファイルパス260文字超えを許容していない場合のみ発生する")]
        public static void ConstructorStringTest_Failure_TooLong(string value)
        {
            constructorTestHelper.ConstructorFailure(
                factory: () => new FilePath(value),
                exceptionVerifier: ExceptionVerifier.IsType(typeof(PathTooLongException))
            );
        }

        #endregion

        #region Methods

        #region public

        #region GetHashCode

        [Test]
        public static void GetHashCodeTest()
        {
            var instance1 = new FilePath("abc");
            var instance2 = new FilePath("abc");
            var instance3 = new FilePath("aac");

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
                instance: new FilePath(value),
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
        ///     string から FilePath に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastStringToFilePathTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => value,
                resultValueVerifier: ValueVerifier<FilePath>.AreEquals(value)
            );
        }

        /// <summary>
        ///     改行を含む文字 から FilePath に暗黙的型変換した場合、
        ///     ArgumentException が発生すること。
        /// </summary>
        [Test]
        public static void CastStringToFilePathTest_Failure_NewLine()
        {
            staticFunctionTestHelper.StaticFuncFailure<FilePath>(
                execFunc: () => (string)"Wolf\nRPG\nEditor.",
                exceptionVerifier: ExceptionVerifier.IsType(typeof(ArgumentException))
            );
        }

        #endregion

        #region To

        /// <summary>
        ///     FilePath から string に暗黙的型変換できること。
        /// </summary>
        [Test]
        public static void CastFilePathToStringTest_Success()
        {
            const string value = "abc";
            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => new FilePath(value),
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
        [TestCase("abc", "abc", true)]
        [TestCase("abc", "cba", false)]
        [TestCase("abc", null, false)]
        [TestCase(null, "cba", false)]
        public static void OperatorEqualTest(string? left, string? right, bool expected)
        {
            var leftValue = (FilePath?)left;
            var rightValue = (FilePath?)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue == rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     等価比較演算 != が意図した値を返すこと。
        /// </summary>
        [TestCase("abc", "abc", false)]
        [TestCase("abc", "cba", true)]
        [TestCase("abc", null, true)]
        [TestCase(null, "cba", true)]
        public static void OperatorNotEqualTest(string? left, string? right, bool expected)
        {
            var leftValue = (FilePath?)left;
            var rightValue = (FilePath?)right;

            staticFunctionTestHelper.StaticFuncSuccess(
                execFunc: () => leftValue != rightValue,
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        /// <summary>
        ///     Equals メソッドが意図した値を返すこと。
        /// </summary>
        [TestCase("abc", "abc", true)]
        [TestCase("abc", "cba", false)]
        [TestCase("abc", null, false)]
        public static void EqualsTest_FilePath(string left, string? right, bool expected)
        {
            var leftValue = (FilePath)left;
            var rightValue = (FilePath?)right;

            pureFunctionTestHelper.PureFuncSuccess(
                instance: leftValue,
                execFunc: target => target.Equals(rightValue),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        private static readonly object?[][] EqualsObjectTestCaseSource =
        {
            // [left, right, expected]
            new object?[] { "abc", new FilePath("abc"), true },
            new object?[] { "abc", new FilePath("cba"), false },
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
            var leftValue = (FilePath)left;

            pureFunctionTestHelper.PureFuncSuccess(
                instance: leftValue,
                execFunc: target => target.Equals(right),
                resultValueVerifier: ValueVerifier.AreEquals(expected)
            );
        }

        #endregion

        #endregion
    }
}
