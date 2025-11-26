using NUnit.Framework;
using WodiLib.Sys;
using WodiLib.Sys.Cmn;

namespace WodiLib.Test.Tools
{
    /// <summary>
    ///     <see cref="IEqualityComparable{T}.ItemEquals(T?)"/>メソッドテスト用テンプレート処理を定義したクラス
    /// </summary>
    /// <remarks>
    ///     テストの手順は <see cref="EqualsTestHelper"/> と同様。
    /// </remarks>
    internal class ItemEqualsTestHelper : TestHelperBase
    {
        private readonly PureFunctionTestHelper pureFunctionTestHelper;

        public ItemEqualsTestHelper(WodiLibLogger logger) : base(logger)
        {
            pureFunctionTestHelper = new PureFunctionTestHelper(logger);
        }

        /// <summary>
        ///     比較処理(<see cref="IEqualityComparable{T}.ItemEquals(T?)"/>)のテスト
        /// </summary>
        /// <param name="left">比較対象の左辺インスタンス</param>
        /// <param name="right">比較対象の右辺インスタンス</param>
        /// <param name="expected">期待する比較結果</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        /// <typeparam name="TComp">テスト対象比較相手型</typeparam>
        public void ItemEquals<TTarget, TComp>(
            TTarget left,
            TComp? right,
            bool expected
        )
            where TTarget : IEqualityComparable<TComp>
        {
            pureFunctionTestHelper.PureFuncSuccess(
                left,
                target => target.ItemEquals(right),
                resultValueVerifier: new ValueVerifier<bool>(actual =>
                    {
                        Assert.AreEqual(expected, actual, $"expected not eq actual. ({expected}, {actual})");
                    }
                )
            );
        }

        /// <summary>
        ///     比較処理(<see cref="IEqualityComparable.ItemEquals"/>)のテスト
        /// </summary>
        /// <param name="left">比較対象の左辺インスタンス</param>
        /// <param name="right">比較対象の右辺インスタンス</param>
        /// <param name="expected">期待する比較結果</param>
        /// <typeparam name="TTarget">テスト対象インスタンス型</typeparam>
        public void ItemEquals<TTarget>(
            TTarget left,
            object? right,
            bool expected
        )
            where TTarget : IEqualityComparable
        {
            pureFunctionTestHelper.PureFuncSuccess(
                left,
                target => target.ItemEquals(right),
                resultValueVerifier: new ValueVerifier<bool>(actual =>
                    {
                        Assert.AreEqual(expected, actual, $"expected not eq actual. ({expected}, {actual})");
                    }
                )
            );
        }
    }
}
