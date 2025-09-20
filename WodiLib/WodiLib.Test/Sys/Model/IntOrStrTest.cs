using System;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Test.Tools;

namespace WodiLib.Test.Sys
{
    [TestFixture]
    public class IntOrStrTest
    {
        private static Logger logger = null!;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.SetupLoggerForDebug();
            logger = Logger.GetInstance();
        }

        private static readonly object[] CreateInstanceTestCaseSource =
        {
            new object[] { 10, "Test", IntOrStrType.Int },
            new object[] { 10, "Test", IntOrStrType.Str },
            new object[] { 10, "Test", IntOrStrType.IntAndStr },
            new object[] { 10, "Test", IntOrStrType.None },
        };

        [TestCaseSource(nameof(CreateInstanceTestCaseSource))]
        public static void CreateInstanceTest(
            int intValue,
            string stringValue,
            IntOrStrType type
        )
        {
            IntOrStr? instance = null;
            var errorOccured = false;

            try
            {
                if (type == IntOrStrType.Int)
                {
                    instance = new IntOrStr(intValue);
                }
                else if (type == IntOrStrType.Str)
                {
                    instance = new IntOrStr(stringValue);
                }
                else if (type == IntOrStrType.IntAndStr)
                {
                    instance = new IntOrStr(intValue, stringValue);
                }
                else if (type == IntOrStrType.None)
                {
                    instance = new IntOrStr();
                }
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            if (instance == null) Assert.Fail();
            // エラーが発生しないこと
            Assert.IsFalse(errorOccured);
            // 種別が一致すること
            Assert.AreEqual(type, instance!.InstanceIntOrStrType);
        }

        private static readonly object[] MergeTestCaseSource =
        {
            new object[] { IntOrStrType.Int, true, IntOrStrType.Int },
            new object[] { IntOrStrType.Int, false, IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.Str, true, IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.Str, false, IntOrStrType.Str },
            new object[] { IntOrStrType.IntAndStr, true, IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.IntAndStr, false, IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.None, true, IntOrStrType.Int },
            new object[] { IntOrStrType.None, false, IntOrStrType.Str },
        };

        [TestCaseSource(nameof(MergeTestCaseSource))]
        public static void MergeTest(IntOrStrType srcType, bool isMergeInt, IntOrStrType resultType)
        {
            IntOrStr instance;
            if (srcType == IntOrStrType.Int)
            {
                instance = new IntOrStr(10);
            }
            else if (srcType == IntOrStrType.Str)
            {
                instance = new IntOrStr("test");
            }
            else if (srcType == IntOrStrType.IntAndStr)
            {
                instance = new IntOrStr(10, "test");
            }
            else
            {
                instance = new IntOrStr();
            }

            var mergedInstance = isMergeInt
                ? instance.Merged(20)
                : instance.Merged("30");

            // 種別が一致すること
            Assert.AreEqual(resultType, mergedInstance.InstanceIntOrStrType);
        }

        private static readonly object[] MergeTest2CaseSource =
        {
            new object[] { IntOrStrType.Int, (IntOrStr)1000, IntOrStrType.Int },
            new object[] { IntOrStrType.Int, (IntOrStr)"abc", IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.Int, (IntOrStr)(1000, "abc"), IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.Str, (IntOrStr)1000, IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.Str, (IntOrStr)"abc", IntOrStrType.Str },
            new object[] { IntOrStrType.Str, (IntOrStr)(1000, "abc"), IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.IntAndStr, (IntOrStr)1000, IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.IntAndStr, (IntOrStr)"abc", IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.IntAndStr, (IntOrStr)(1000, "abc"), IntOrStrType.IntAndStr },
            new object[] { IntOrStrType.None, (IntOrStr)1000, IntOrStrType.Int },
            new object[] { IntOrStrType.None, (IntOrStr)"abc", IntOrStrType.Str },
            new object[] { IntOrStrType.None, (IntOrStr)(1000, "abc"), IntOrStrType.IntAndStr },
        };

        [TestCaseSource(nameof(MergeTest2CaseSource))]
        public static void MergeTest2(IntOrStrType srcType, IntOrStr mergeValue, IntOrStrType resultType)
        {
            IntOrStr instance;
            if (srcType == IntOrStrType.Int)
            {
                instance = new IntOrStr(10);
            }
            else if (srcType == IntOrStrType.Str)
            {
                instance = new IntOrStr("test");
            }
            else if (srcType == IntOrStrType.IntAndStr)
            {
                instance = new IntOrStr(10, "test");
            }
            else
            {
                instance = new IntOrStr();
            }

            var merged = instance.Merged(mergeValue);

            // 種別が一致すること
            Assert.AreEqual(resultType, merged.InstanceIntOrStrType);
        }

        private static readonly object[] ToIntTestCaseSource =
        {
            new object[] { IntOrStrType.Int, false },
            new object[] { IntOrStrType.Str, true },
            new object[] { IntOrStrType.IntAndStr, false },
            new object[] { IntOrStrType.None, true },
        };

        [TestCaseSource(nameof(ToIntTestCaseSource))]
        public static void ToIntTest(IntOrStrType srcType, bool isError)
        {
            var src = 100;
            IntOrStr instance;
            if (srcType == IntOrStrType.Int)
            {
                instance = new IntOrStr(src);
            }
            else if (srcType == IntOrStrType.Str)
            {
                instance = new IntOrStr("test");
            }
            else if (srcType == IntOrStrType.IntAndStr)
            {
                instance = new IntOrStr(src, "test");
            }
            else
            {
                instance = new IntOrStr();
            }

            // HasInt が正しいこと
            Assert.AreEqual(!isError, instance.HasInt);

            var toResult = 0;
            var errorOccured = false;
            try
            {
                toResult = instance.ToInt();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);

            if (!errorOccured)
            {
                // セットした値が正しいこと
                Assert.AreEqual(toResult, src);
            }
        }

        private static readonly object[] ToStrTestCaseSource =
        {
            new object[] { IntOrStrType.Int, true },
            new object[] { IntOrStrType.Str, false },
            new object[] { IntOrStrType.IntAndStr, false },
            new object[] { IntOrStrType.None, true },
        };

        [TestCaseSource(nameof(ToStrTestCaseSource))]
        public static void ToStrTest(IntOrStrType srcType, bool isError)
        {
            var src = "test";
            IntOrStr instance;
            if (srcType == IntOrStrType.Int)
            {
                instance = new IntOrStr(100);
            }
            else if (srcType == IntOrStrType.Str)
            {
                instance = new IntOrStr(src);
            }
            else if (srcType == IntOrStrType.IntAndStr)
            {
                instance = new IntOrStr(100, src);
            }
            else
            {
                instance = new IntOrStr();
            }

            // HasStr が正しいこと
            Assert.AreEqual(!isError, instance.HasStr);

            var toResult = "";
            var errorOccured = false;
            try
            {
                toResult = instance.ToStr();
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(isError, errorOccured);

            if (!errorOccured)
            {
                // セットした値が正しいこと
                Assert.AreEqual(src, toResult);
            }
        }

        private static readonly object[] IsOneSideValueTestCaseSource =
        {
            new object[] { IntOrStrType.Int, true },
            new object[] { IntOrStrType.Str, true },
            new object[] { IntOrStrType.IntAndStr, false },
            new object[] { IntOrStrType.None, false },
        };

        [TestCaseSource(nameof(IsOneSideValueTestCaseSource))]
        public static void IsOneSideValueTest(IntOrStrType srcType, bool result)
        {
            IntOrStr instance;
            if (srcType == IntOrStrType.Int)
            {
                instance = new IntOrStr(100);
            }
            else if (srcType == IntOrStrType.Str)
            {
                instance = new IntOrStr("test");
            }
            else if (srcType == IntOrStrType.IntAndStr)
            {
                instance = new IntOrStr(100, "test");
            }
            else
            {
                instance = new IntOrStr();
            }

            // IsOneSideValue の結果が正しいこと
            Assert.AreEqual(result, instance.IsOneSideValue);
        }

        [Test]
        public static void ImplicitFromIntTest()
        {
            var errorOccured = false;
            IntOrStr instance = null!;
            const int src = 10;
            try
            {
                instance = src;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが起こらないこと
            Assert.False(errorOccured);
            // 値が一致すること
            Assert.AreEqual(src, instance.ToInt());
        }

        [Test]
        public static void ImplicitFromStringTest()
        {
            var errorOccured = false;
            IntOrStr instance = null!;
            var src = "test";
            try
            {
                instance = src;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが起こらないこと
            Assert.False(errorOccured);
            // 値が一致すること
            Assert.AreEqual(src, instance.ToStr());
        }

        [Test]
        public static void ImplicitFromTuple()
        {
            var errorOccured = false;
            IntOrStr instance = null!;
            var src = new Tuple<int, string>(10, "test");
            try
            {
                instance = src;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが起こらないこと
            Assert.False(errorOccured);
            // 値が一致すること
            Assert.AreEqual(src.Item1, instance.ToInt());
            Assert.AreEqual(src.Item2, instance.ToStr());
        }

        [Test]
        public static void ImplicitFromValueTuple()
        {
            var errorOccured = false;
            IntOrStr instance = null!;
            var src = (10, "test");
            try
            {
                instance = src;
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーが起こらないこと
            Assert.False(errorOccured);
            // 値が一致すること
            Assert.AreEqual(src.Item1, instance.ToInt());
            Assert.AreEqual(src.Item2, instance.ToStr());
        }
    }
}
