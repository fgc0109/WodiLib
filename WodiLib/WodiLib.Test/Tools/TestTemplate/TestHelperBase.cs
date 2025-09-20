// ========================================
// Project Name : WodiLib.Test
// File Name    : TestHelperBase.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using Commons;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     すべてのTestHelperの基底クラス
    /// </summary>
    internal abstract class TestHelperBase
    {
        protected readonly Logger logger;

        protected TestHelperBase(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        ///     期待値と実値が一致するかどうかを返す。
        /// </summary>
        /// <typeparam name="T">期待値の型</typeparam>
        /// <param name="expected">期待値</param>
        /// <returns></returns>
        protected Action<T> VerifyExpectedValue<T>(T expected)
        {
            return (actual) => { Assert.AreEqual(expected, actual); };
        }
    }
}
