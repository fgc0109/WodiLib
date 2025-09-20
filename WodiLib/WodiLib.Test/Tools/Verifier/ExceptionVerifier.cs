using System;
using NUnit.Framework;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     例外検証設定
    /// </summary>
    internal static class ExceptionVerifier
    {
        internal delegate string MakeIsTypeCustomMessage(Type expected, Exception actual);

        /// <summary>
        ///     値が指定した型であることを検証する。
        /// </summary>
        /// <param name="expected">比較する値</param>
        /// <param name="customMessage">値が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<Exception> IsType(Type expected, MakeIsTypeCustomMessage? customMessage = null)
            => new(actual => Assert.AreEqual(
                    expected,
                    actual.GetType(),
                    customMessage?.Invoke(expected, actual)
                    ?? $"Expected: {expected}\n"
                    + $"  But was: {actual.GetType()}\n"
                    + $"{actual}"
                )
            );
    }
}
