using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Commons;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     戻り値のある非純粋テスト用テンプレート処理を定義したクラス
    /// </summary>
    /// <remarks>
    ///     以下の手順のテストを行う。
    ///     <list type="number">
    ///         <item>
    ///             <description>
    ///                 メソッドを実行し、エラーの有無を検証する。
    ///                 エラー発生時はここで終了。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 返戻値が意図した値であることを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 メソッド実行によりプロパティ変更通知が意図したとおり発火していることを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>実行結果が意図した値であることをテスト</description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 メソッド実行後のインスタンスの状態が意図した状態であることを検証する。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class ImpureFunctionTestHelper : ImpureTestHelperBase
    {
        public ImpureFunctionTestHelper(Logger logger) : base(logger)
        {
        }

        /// <summary>
        ///     非純粋メソッドのテスト(戻り値あり)（成功パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="resultValueVerifier">
        ///     戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <param name="expectedNotifyProperties">
        ///     期待する変更通知されたプロパティ列挙<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、変更通知を検証しない。
        /// </param>
        /// <param name="instanceVerifier">
        ///     処理対象インスタンスの状態検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <typeparam name="TTarget">テスト対象型</typeparam>
        /// <typeparam name="TResult">メソッド戻り値型</typeparam>
        public void ImpureFuncSuccess<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            ValueVerifier<TResult>? resultValueVerifier = null,
            IEnumerable<string>? expectedNotifyProperties = null,
            ValueVerifier<TTarget>? instanceVerifier = null
        ) => ImpureFuncInternal(
            instance,
            execFunc,
            expectedFailure: false,
            resultValueVerifier,
            expectedNotifyProperties,
            instanceVerifier,
            exceptionVerifier: null
        );

        /// <summary>
        ///     非純粋メソッドのテスト(戻り値あり)（失敗パターン）
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="execFunc">メソッド実行処理</param>
        /// <param name="exceptionVerifier">
        ///     例外発生時の例外検証処理。<br/>
        ///     <br/>
        ///     <see langword="null"/> の場合、例外が発生しても検証処理を行わない。
        /// </param>
        /// <typeparam name="TTarget">テスト対象型</typeparam>
        /// <typeparam name="TResult">メソッド戻り値型</typeparam>
        public void ImpureFuncFailure<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            ValueVerifier<Exception>? exceptionVerifier = null
        ) => ImpureFuncInternal(
            instance,
            execFunc,
            expectedFailure: true,
            resultValueVerifier: null,
            expectedNotifyProperties: null,
            instanceVerifier: null,
            exceptionVerifier
        );

        private void ImpureFuncInternal<TTarget, TResult>(
            TTarget instance,
            Func<TTarget, TResult> execFunc,
            bool expectedFailure,
            ValueVerifier<TResult>? resultValueVerifier = null,
            IEnumerable<string>? expectedNotifyProperties = null,
            ValueVerifier<TTarget>? instanceVerifier = null,
            ValueVerifier<Exception>? exceptionVerifier = null
        )
        {
            var changedPropertyList = new List<string>();
            var propertyChangedNotifiable = instance as INotifyPropertyChanged;
            if (propertyChangedNotifiable is not null)
            {
                propertyChangedNotifiable.PropertyChanged += (_, args) =>
                {
                    changedPropertyList.Add(args.PropertyName!);
                };
            }

            TTarget? original = default;
            if (instance is IDeepCloneable<TTarget> deepCloneable)
            {
                original = deepCloneable.DeepClone();
            }

            var errorOccured = false;
            TResult result = default!;
            Exception exception = null!;
            try
            {
                result = execFunc.Invoke(instance);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
                exception = ex;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(
                expectedFailure,
                errorOccured,
                $"expectedFailure not eq errorOccured. ({expectedFailure}, {errorOccured}, {exception})"
            );

            if (errorOccured)
            {
                // 発生したエラーの検証処理
                exceptionVerifier?.Verify(exception);

                // 状態が変化していないこと
                if (original is not null)
                {
                    if (instance is IEqualityComparable<TTarget> comparableInstance)
                    {
                        CustomAssert.AreItemEquals(comparableInstance, original);
                    }
                    else
                    {
                        Assert.AreEqual(instance, original);
                    }
                }

                // プロパティ変更通知が発火していないこと
                Assert.AreEqual(0, changedPropertyList.Count);

                return;
            }

            // 戻り値が意図した値であること
            resultValueVerifier?.Verify(result);

            // プロパティ変更通知が発火していること
            if (propertyChangedNotifiable is not null && expectedNotifyProperties is not null)
            {
                var expectedNotifyPropertyChangeList = expectedNotifyProperties.ToList();

                AssertEqualsNotifiedPropertyNames(
                    expectedNotifyPropertyChangeList,
                    changedPropertyList
                );
            }

            // 実行後のインスタンスが意図した状態であること
            instanceVerifier?.Verify(instance);
        }
    }
}
