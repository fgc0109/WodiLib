using System;
using NUnit.Framework;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="object.Equals(object?)"/> メソッドテスト用テンプレート処理を定義したクラス
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
    ///                 結果が意図した値であることを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 メソッド実行によりプロパティ変更通知が発火していないことを検証する。
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <description>
    ///                 （対象が IDeepCloneable を実装する場合のみ）
    ///                 メソッド実行前後で状態が変化していないことを検証する。
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    internal class EqualsTestHelper : TestHelperBase
    {
        private readonly PureFunctionTestHelper pureFunctionTestHelper;

        public EqualsTestHelper(WodiLibLogger logger) : base(logger)
        {
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
        }

        /// <summary>
        ///     比較処理のテスト
        /// </summary>
        /// <param name="left">比較対象の左辺インスタンス</param>
        /// <param name="right">比較対象の右辺インスタンス</param>
        /// <param name="expected">期待する比較結果</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TComp">テスト対象比較相手型</typeparam>
        public void Equals<TTarget, TComp>(
            TTarget left,
            TComp? right,
            bool expected
        )
        {
            pureFunctionTestHelper.PureFuncSuccess(
                left,
                execFunc: target => target is IEquatable<TComp> equatable
                    ? equatable.Equals(right)
                    : target?.Equals(right),
                resultValueVerifier: new ValueVerifier<bool?>(actual =>
                    {
                        Assert.AreEqual(expected, actual, $"expected not eq actual. ({expected}, {actual})");
                    }
                )
            );
        }
    }
}
