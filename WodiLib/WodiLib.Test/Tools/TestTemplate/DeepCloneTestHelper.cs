using Commons;
using NUnit.Framework;
using WodiLib.Sys;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IDeepCloneable{T}.DeepClone"/>メソッドテスト用テンプレート処理を定義したクラス
    /// </summary>
    /// <remarks>
    ///     以下の手順のテストを行う。
    ///     <list type="number">
    ///         <item>
    ///             <description>
    ///                 比較メソッドを実行し、
    ///                 エラーが発生しないことを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 返戻値がクローン元のインスタンスと同値であり、
    ///                 別インスタンスであることを検証する。<br/>
    ///                 また、呼び出し元に応じた追加検証を行う。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 メソッド実行によりプロパティ変更通知が発火していないことを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 メソッド実行前後で状態が変化していないことを検証する。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class DeepCloneTestHelper : TestHelperBase
    {
        private readonly PureFunctionTestHelper pureFunctionTestHelper;

        public DeepCloneTestHelper(Logger logger) : base(logger)
        {
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
        }

        /// <summary>
        ///     DeepCloneメソッドのテスト
        /// </summary>
        /// <param name="instance">テスト対象のインスタンス</param>
        /// <param name="resultValueVerifier">
        ///     戻り値の検証処理<br/>
        ///     エラー時には <see cref="Assert"/> などを利用して例外を発生させる。
        /// </param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        public void DeepClone<TTarget>(
            TTarget instance,
            ValueVerifier<TTarget>? resultValueVerifier = null
        )
            where TTarget : IDeepCloneable<TTarget>, IEqualityComparable<TTarget>
        {
            pureFunctionTestHelper.PureFuncSuccess(
                instance,
                target => target.DeepClone(),
                resultValueVerifier: new ValueVerifier<TTarget>(cloned =>
                    {
                        Assert.AreNotSame(instance, cloned);
                        CustomAssert.AreItemEquals(instance, cloned);

                        resultValueVerifier?.Verify(cloned);
                    }
                )
            );
        }
    }
}
