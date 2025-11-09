// ========================================
// Project Name : WodiLib
// File Name    : CommonListValidator.cs
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
    ///     容量制限ありリスト編集メソッドの引数汎用検証処理実施クラス
    /// </summary>
    /// <typeparam name="TListSettings">リストの入力パラメータ型</typeparam>
    /// <typeparam name="TElementSettings">リスト内包型の入力パラメータ型</typeparam>
    internal class RestrictedCapacityListValidator<TListSettings, TElementSettings> :
        StandardListValidator<TListSettings, TElementSettings>
        where TListSettings : IListSettings<TElementSettings>
    {
        public delegate int GetMaxCapacityDelegate();

        public delegate int GetMinCapacityDelegate();

        private static string ListItemsName => "要素数";

        protected GetMaxCapacityDelegate MaxCapacityGetter { get; }
        protected GetMinCapacityDelegate MinCapacityGetter { get; }

        public RestrictedCapacityListValidator(
            GetCountDelegate countGetter,
            GetMaxCapacityDelegate maxCapacityGetter,
            GetMinCapacityDelegate minCapacityGetter
        ) : base(countGetter)
        {
            MaxCapacityGetter = maxCapacityGetter;
            MinCapacityGetter = minCapacityGetter;
        }

        public override void Constructor(NamedValue<TListSettings> initSettings)
        {
            base.Constructor(initSettings);

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
                initSettings.Value.Settings.Count,
                minCapacity,
                maxCapacity
            );
        }

        public override void Insert(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
        {
            base.Insert(index, items);

            RestrictedCapacityListValidationHelper.ItemMaxCount(
                CountGetter.Invoke() + items.Value.Count(),
                MaxCapacityGetter.Invoke()
            );
        }

        public override void Overwrite(NamedValue<int> index, NamedValue<IEnumerable<TElementSettings>> items)
        {
            base.Overwrite(index, items);

            RestrictedCapacityListValidationHelper.OverwrittenCount(
                index.Value,
                items.Value.Count(),
                CountGetter.Invoke(),
                MaxCapacityGetter.Invoke()
            );
        }

        public override void Remove(NamedValue<int> index, NamedValue<int> count)
        {
            base.Remove(index, count);

            RestrictedCapacityListValidationHelper.ItemMinCount(
                (ListItemsName, CountGetter.Invoke() - count.Value),
                MinCapacityGetter.Invoke()
            );
        }

        public override void AdjustLength(NamedValue<int> length)
        {
            var min = MinCapacityGetter.Invoke();
            ThrowHelper.ValidateArgumentValueGreaterOrEqual(
                length.Value < min,
                length.Name,
                min,
                length.Value
            );

            var max = MaxCapacityGetter.Invoke();
            ThrowHelper.ValidateArgumentValueLessOrEqual(
                length.Value > max,
                length.Name,
                max,
                length.Value
            );
        }

        public override void Reset(NamedValue<IEnumerable<TElementSettings>> items, bool canChangeSize = true)
        {
            base.Reset(items, canChangeSize);

            RestrictedCapacityListValidationHelper.ArgumentItemsCount(
                items.Value.Count(),
                MinCapacityGetter.Invoke(),
                MaxCapacityGetter.Invoke(),
                items.Name
            );
        }
    }
}
