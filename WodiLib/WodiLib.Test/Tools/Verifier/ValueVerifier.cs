using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     値検証設定
    /// </summary>
    internal static class ValueVerifier
    {
        /// <inheritdoc cref="ValueVerifier{T}.AreEquals"/>
        public static ValueVerifier<T> AreEquals<T>(
            T expected,
            ValueVerifier<T?>.MakeEqualsCustomMessage? customMessage = null
        )
            => new(actual => Assert.AreEqual(expected, actual, customMessage?.Invoke(expected, actual)));

        /// <inheritdoc
        ///     cref="ValueVerifier{T}.AreItemEquals{TVal}(TVal,WodiLib.Test.Tools.ValueVerifier{T}.MakeItemEqualsCustomMessage{TVal}?)"/>
        public static ValueVerifier<T> AreItemEquals<T>(
            T expected,
            ValueVerifier<T?>.MakeItemEqualsCustomMessage<T>? customMessage = null
        )
            where T : IEqualityComparable<T>
            => new(actual => CustomAssert.AreItemEquals(
                    expected,
                    actual,
                    customMessage?.Invoke(expected, actual)
                )
            );

        /// <inheritdoc
        ///     cref="ValueVerifier{T}.AreReferenceEquals(T,WodiLib.Test.Tools.ValueVerifier{T}.MakeReferenceEqualsCustomMessage?)"/>
        public static ValueVerifier<T> AreReferenceEquals<T>(
            T other,
            ValueVerifier<T>.MakeReferenceEqualsCustomMessage? customMessage = null
        )
            => new(target => Assert.AreSame(other, target, customMessage?.Invoke(other, target)));

        /// <summary>
        ///     2つのシーケンスの要素が一致することを検証する。
        /// </summary>
        /// <typeparam name="T">期待する型</typeparam>
        /// <param name="expected">比較する値</param>
        /// <returns></returns>
        public static ValueVerifier<IEnumerable<T>> AreItemSequenceEquals<T>(
            IEnumerable<T> expected
        )
            => new(actual => CustomAssert.AreSequenceEquals(expected, actual));

        /// <summary>
        ///     2つのシーケンスの要素が一致することを検証する。
        /// </summary>
        /// <typeparam name="T">期待する型</typeparam>
        /// <param name="expected">比較する値</param>
        /// <returns></returns>
        public static ValueVerifier<IEnumerable<IEnumerable<T>>> AreItemSequenceEquals<T>(
            IEnumerable<IEnumerable<T>> expected
        )
            => new(actual => CustomAssert.AreSequenceEquals(expected, actual));

        /// <summary>
        ///     2つのシーケンスの要素が一致することを検証する。
        /// </summary>
        /// <typeparam name="TExpected">期待する型</typeparam>
        /// <typeparam name="TActual">実際の型</typeparam>
        /// <param name="expected">比較する値</param>
        /// <returns></returns>
        public static ValueVerifier<IEnumerable<TExpected>> AreItemSequenceEquals<TExpected, TActual>(
            IEnumerable<TExpected> expected
        )
            where TActual : TExpected
            => new(actual => CustomAssert.AreSequenceEquals(expected, actual));

        /// <summary>
        ///     シーケンスが空であることを検証する。
        /// </summary>
        /// <returns></returns>
        public static ValueVerifier<IEnumerable> IsEmpty()
            => new(Assert.IsEmpty);

        /// <summary>
        ///     要素が null であることを検証する。
        /// </summary>
        /// <returns></returns>
        public static ValueVerifier<object?> IsNull()
            => new(Assert.IsNull);
    }

    /// <summary>
    ///     値検証設定
    /// </summary>
    /// <typeparam name="T">検証する値の型</typeparam>
    internal class ValueVerifier<T>
    {
        internal delegate string MakeEqualsCustomMessage(T? expected, T? actual);

        internal delegate string MakeItemEqualsCustomMessage<in TVal>(TVal? expected, T? actual);

        internal delegate string MakeReferenceEqualsCustomMessage(T? expected, T? actual);

        internal delegate string MakeIsTypeCustomMessage(Type? expected, T? actual);

        internal delegate string MakeIsNullCustomMessage(T? actual);

        /// <summary>
        ///     値を引数で指定した処理で検証する。
        /// </summary>
        /// <param name="verifier">
        ///     検証処理<br/>
        ///     検証に失敗させる場合は <paramref name="verifier"/> の処理内で Assert を呼び出すこと。
        /// </param>
        /// <returns></returns>
        public ValueVerifier(Action<T>? verifier = null)
        {
            this.verifier = verifier;
        }

        /// <summary>
        ///     値を <see cref="Object.Equals(object?)"/> メソッドで比較することで検証する。
        /// </summary>
        /// <param name="expected">比較する値</param>
        /// <param name="customMessage">値が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> AreEquals(T expected, MakeEqualsCustomMessage? customMessage = null)
            => new(actual => Assert.AreEqual(expected, actual, customMessage?.Invoke(expected, actual)));

        /// <summary>
        ///     値を <see cref="Object.Equals(object?)"/> メソッドで比較することで検証する。
        /// </summary>
        /// <param name="expected">比較する値</param>
        /// <param name="customMessage">値が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> AreItemEquals<TVal>(
            TVal expected,
            MakeItemEqualsCustomMessage<TVal>? customMessage = null
        )
            where TVal : IEqualityComparable<T>
            => new(actual => CustomAssert.AreItemEquals(expected, actual, customMessage?.Invoke(expected, actual)));

        /// <summary>
        ///     値を <see cref="Object.Equals(object?)"/> メソッドで比較することで検証する。
        /// </summary>
        /// <param name="getExpected">比較する値取得処理</param>
        /// <param name="customMessage">値が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> AreItemEquals<TVal>(
            Func<TVal> getExpected,
            MakeItemEqualsCustomMessage<TVal>? customMessage = null
        )
            where TVal : IEqualityComparable<T>
            => new(actual =>
                {
                    var expected = getExpected.Invoke();
                    CustomAssert.AreItemEquals(expected, actual, customMessage?.Invoke(expected, actual));
                }
            );

        /// <summary>
        ///     値の参照が一致することを検証する。
        /// </summary>
        /// <param name="other">比較対象のオブジェクト</param>
        /// <param name="customMessage">参照が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> AreReferenceEquals(
            T other,
            MakeReferenceEqualsCustomMessage? customMessage = null
        )
            => new(target => Assert.AreSame(other, target, customMessage?.Invoke(other, target)));

        /// <summary>
        ///     値の参照が一致することを検証する。
        /// </summary>
        /// <param name="getOther">比較対象のオブジェクト取得処理</param>
        /// <param name="customMessage">参照が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> AreReferenceEquals(
            Func<T> getOther,
            MakeReferenceEqualsCustomMessage? customMessage = null
        )
            => new(target =>
                {
                    var other = getOther.Invoke();
                    Assert.AreSame(other, target, customMessage?.Invoke(other, target));
                }
            );

        /// <summary>
        ///     値が指定した型であることを検証する。
        /// </summary>
        /// <param name="expected">比較する値</param>
        /// <param name="customMessage">値が一致しなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> IsType(Type expected, MakeIsTypeCustomMessage? customMessage = null)
            => new(actual => Assert.AreEqual(expected, actual?.GetType(), customMessage?.Invoke(expected, actual)));

        /// <summary>
        ///     値がnullであることを検証する。
        /// </summary>
        /// <param name="customMessage">値がnullではなかった場合のカスタムメッセージ</param>
        /// <returns></returns>
        public static ValueVerifier<T> IsNull(MakeIsNullCustomMessage? customMessage = null)
            => new(actual => Assert.IsNull(actual, customMessage?.Invoke(actual)));

        private readonly Action<T>? verifier;

        public void Verify(T actual) => verifier?.Invoke(actual);
    }
}
