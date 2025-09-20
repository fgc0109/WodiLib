// ========================================
// Project Name : WodiLib
// File Name    : FixedLengthListValidator.cs
//
// MIT License Copyright(c) 2019 kameske
// see LICENSE file
// ========================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace WodiLib.Sys.Collections
{
    /// <summary>
    ///     容量固定リスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TElementSettings">リスト内包型の入力パラメータ型</typeparam>
    internal class FixedLengthListValidator<TElementSettings> : StandardListValidator<TElementSettings>
    {
        public delegate int GetMaxCapacityDelegate();

        public delegate int GetMinCapacityDelegate();

        protected GetMaxCapacityDelegate MaxCapacityGetter { get; }
        protected GetMinCapacityDelegate MinCapacityGetter { get; }

        public FixedLengthListValidator(
            GetCountDelegate countGetter,
            GetMaxCapacityDelegate maxCapacityGetter,
            GetMinCapacityDelegate minCapacityGetter
        ) : base(countGetter)
        {
            MaxCapacityGetter = maxCapacityGetter;
            MinCapacityGetter = minCapacityGetter;
        }

        public override void Constructor(NamedValue<IEnumerable<TElementSettings>> initItems)
        {
            base.Constructor(initItems);

            var maxCapacity = MaxCapacityGetter.Invoke();
            var minCapacity = MinCapacityGetter.Invoke();
#if DEBUG
            try
            {
                RestrictedCapacityListValidationHelper.CapacityConfig(
                    ($"GetMinCapacity", minCapacity),
                    ($"GetMaxCapacity", maxCapacity)
                );
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(GetType().Name, ex);
            }
#endif

            RestrictedCapacityListValidationHelper.ArgumentItemsCount(
                initItems.Value.Count(),
                minCapacity,
                maxCapacity
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
            => throw new InvalidOperationException();

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
            => throw new InvalidOperationException();

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
            => throw new InvalidOperationException();

        public override void AdjustLength(NamedValue<int> length)
            => throw new InvalidOperationException();

        public override void Reset(NamedValue<IEnumerable<TElementSettings>> items, bool canChangeSize = true)
        {
            if (canChangeSize)
            {
                throw new InvalidOperationException();
            }

            base.Reset(items, canChangeSize);
        }

        public override void Clear()
            => throw new InvalidOperationException();
    }
}
